using System.Text.Json;
using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models;
using MessagingMicroservice.Application.Models.InboundRequestJsonStructure;
using MessagingMicroservice.Application.Models.WebhookVerification;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Models;
using MessagingMicroservice.Infrastructure.StatusMappers;
using Microsoft.AspNetCore.Mvc;

namespace MessagingMicroservice.Controllers.Webhooks;

[ApiController]
[Route("api/webhooks/telnyx")]
public class TelnyxWebhookController:ControllerBase
{
    private readonly IInboundMessageService _inboundMessageService;
    private readonly IValidateWebhookRequest<TelnyxWebhookVerificationRequest> _validator;
    private readonly IUpdateMessageStatusService _updateMessageStatusService;
    private readonly IOptoutPhoneNumberService _optoutPhoneNumberService;
    private static readonly string[] OptOutKeywords = ["STOP", "UNSUBSCRIBE", "CANCEL", "END", "QUIT"];

    public TelnyxWebhookController(IInboundMessageService inboundMessageService,
        IValidateWebhookRequest<TelnyxWebhookVerificationRequest> validator,
        IUpdateMessageStatusService updateMessageStatusService,
        IOptoutPhoneNumberService optoutPhoneNumberService)
    {
        _inboundMessageService = inboundMessageService;
        _validator = validator;
        _updateMessageStatusService = updateMessageStatusService;
        _optoutPhoneNumberService = optoutPhoneNumberService;
    }

    [HttpPost("inbound")]
    public async Task<IActionResult> ReceiveMessageAsync()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationRequest = GetWebhookVerificationRequest(body);

        bool isValid = await _validator.ValidateAsync(validationRequest);

        if (!isValid) return Unauthorized();

        var webhook = JsonSerializer.Deserialize<TelnyxInboundJsonData>(
            body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }); 

        if (webhook?.Data?.Payload is null || webhook?.Data?.EventType != "message.received")
            return Ok();

        var message = GetInboundMessageRequest(webhook);

        var inboundMessageId=await _inboundMessageService.SaveInboundMessageAsync(message);

        if (IsOptoutRequest(webhook))
        {
            await SaveOptOutPhoneNumberAsync(webhook, inboundMessageId);
        }

        return Ok();
    }

    [HttpPost("status")]
    public async Task<IActionResult> UpdateMessageAsync()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        var validationRequest = GetWebhookVerificationRequest(body);
        bool isValid = await _validator.ValidateAsync(validationRequest);

        if (!isValid) return Unauthorized();

        var webhook = JsonSerializer.Deserialize<TelnyxStatusJsonData>(
            body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var payload = webhook?.Data?.Payload;
        if (payload == null) return Ok();

        string? rawStatus = payload.To?.FirstOrDefault()?.Status;
        if (string.IsNullOrWhiteSpace(rawStatus)) return Ok();
        
        string normalizedStatus = rawStatus.Replace("_", "").Replace("-", "");
        if (!Enum.TryParse<TelnyxMessageStatus>(normalizedStatus, ignoreCase: true, out var parsedStatus))
        {
            return Ok();
        }
        
        var updateStatusRequest = new UpdateMessageStatus
        {
            ProviderMessageId = payload.Id,
            Status = TelnyxStatusMapper.Map(parsedStatus)
        };

        await _updateMessageStatusService.UpdateMessageStatusAsync(updateStatusRequest);

        return Ok();
    }
    
    private TelnyxWebhookVerificationRequest GetWebhookVerificationRequest(string body)
    {
        var signature = Request.Headers["telnyx-signature-ed25519"].ToString();
        var timestamp = Request.Headers["telnyx-timestamp"].ToString();

        return new TelnyxWebhookVerificationRequest
        {
            Body = body,
            Signature = signature,
            Timestamp = timestamp
        };
    }

    private InboundMessageRequest GetInboundMessageRequest(TelnyxInboundJsonData data)
    {
        return new InboundMessageRequest
        {
            MessageId = data.Data.Payload.Id,
            FromNumber = data.Data.Payload.From?.PhoneNumber ?? "",
            ToNumber = data.Data.Payload.To?.FirstOrDefault()?.PhoneNumber ?? "",
            Content = data.Data.Payload.Text ?? "",
            ProviderId = (int)MessageProvider.Telnyx,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    private bool IsOptoutRequest(TelnyxInboundJsonData webhook)
    {
        var payload = webhook.Data.Payload;
        if (payload.Tags is not null &&
            payload.Tags.Any(t => string.Equals(t, "STOP", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }
        string body = payload.Text?.Trim() ?? string.Empty;
        return OptOutKeywords.Contains(body, StringComparer.OrdinalIgnoreCase);
    }

    private OptoutPhoneNumberModel GetOptoutPhoneNumberDetails(TelnyxInboundJsonData webhook,int inboundMessageId)
    {
        var payload = webhook.Data.Payload;
        string? dateString = payload.ReceivedAt;

        if (string.IsNullOrWhiteSpace(dateString))
        {
            dateString = payload.SentAt;
        }

        DateTime optedoutAt = DateTimeOffset.TryParse(dateString, out var parsedDate)
            ? parsedDate.UtcDateTime
            : DateTime.UtcNow;

        return new OptoutPhoneNumberModel()
        {
            InboundMessageId = inboundMessageId,
            OptedOutAt = optedoutAt,
            PhoneNumber = payload.From?.PhoneNumber?? string.Empty,
            ProviderId = (int)MessageProvider.Telnyx
        };
    }
    
    private async Task<int> SaveOptOutPhoneNumberAsync(TelnyxInboundJsonData webhook, int inboundMessageId)
    {
        var optoutDetails = GetOptoutPhoneNumberDetails(webhook, inboundMessageId);
        return await _optoutPhoneNumberService.InsertOptoutPhoneNumberAsync(optoutDetails);
    }
}