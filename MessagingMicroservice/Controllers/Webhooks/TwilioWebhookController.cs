using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models;
using MessagingMicroservice.Application.Models.WebhookVerification;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Models;
using MessagingMicroservice.Infrastructure.StatusMappers;
using Microsoft.AspNetCore.Mvc;

namespace MessagingMicroservice.Controllers.Webhooks;

[ApiController]
[Route("api/webhooks/twilio")]
public class TwilioWebhookController : ControllerBase
{
    private readonly IValidateWebhookRequest<TwilioWebhookVerificationRequest> _validator;
    private readonly IInboundMessageService _inboundMessageService;
    private readonly IUpdateMessageStatusService _updateMessageStatusService;
    private readonly IOptoutPhoneNumberService _optoutPhoneNumberService;
    private static readonly string[] OptOutKeywords = ["STOP", "UNSUBSCRIBE", "CANCEL", "END", "QUIT"];

    public TwilioWebhookController(
        IValidateWebhookRequest<TwilioWebhookVerificationRequest> validator,
        IInboundMessageService inboundMessageService,
        IUpdateMessageStatusService updateMessageStatusService,
        IOptoutPhoneNumberService optoutPhoneNumberService)
    {
        _validator = validator;
        _inboundMessageService = inboundMessageService;
        _updateMessageStatusService = updateMessageStatusService;
        _optoutPhoneNumberService = optoutPhoneNumberService;
    }

    [HttpPost("inbound")]
    public async Task<IActionResult> ReceiveMessageAsync()
    {
        var form = await Request.ReadFormAsync();
        
        var validationRequest = CreateVerificationRequest(Request, form);
        bool isValid = await _validator.ValidateAsync(validationRequest);

        if (!isValid) return Unauthorized();

        var messageRequest = CreateInboundMessageRequest(form);
        var inboundMessageId=await _inboundMessageService.SaveInboundMessageAsync(messageRequest);

        bool isOptedOut = IsOptOutRequest(form);
        
        if (isOptedOut)
        {
            await SaveOptOutPhoneNumber(form,inboundMessageId);
        }
        
        return Ok();
    }

    [HttpPost("status")]
    public async Task<IActionResult> UpdateMessageStatusAsync()
    {
        var form = await Request.ReadFormAsync();
        
        var validationRequest = CreateVerificationRequest(Request, form);
        bool isValid = await _validator.ValidateAsync(validationRequest);

        if (!isValid) return Unauthorized();

        string providerMessageId = form["MessageSid"].ToString();
        string rawStatus = form["MessageStatus"].ToString();

        if (string.IsNullOrWhiteSpace(providerMessageId) || string.IsNullOrWhiteSpace(rawStatus))
        {
            return Ok();
        }

        var normalizedStatus = rawStatus.Replace("-", "").Replace("_", "");
        if (!Enum.TryParse<TwilioMessageStatus>(normalizedStatus, ignoreCase: true, out var parsedStatus))
        {
            return Ok();
        }

        var updateStatusRequest = new UpdateMessageStatus
        {
            ProviderMessageId = providerMessageId,
            Status = TwilioStatusMapper.Map(parsedStatus)
        };

        await _updateMessageStatusService.UpdateMessageStatusAsync(updateStatusRequest);

        return Ok();
    }

    private TwilioWebhookVerificationRequest CreateVerificationRequest(HttpRequest request, IFormCollection form)
    {
        var signature = request.Headers["X-Twilio-Signature"].ToString();
        var parameters = form.ToDictionary(k => k.Key, v => v.Value.ToString());

        string url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

        return new TwilioWebhookVerificationRequest
        {
            Parameters = parameters,
            Signature = signature,
            Url = url
        };
    }

    private InboundMessageRequest CreateInboundMessageRequest(IFormCollection form)
    {
        return new InboundMessageRequest
        {
            MessageId = form["MessageSid"].ToString(),
            FromNumber = form["From"].ToString(),
            ToNumber = form["To"].ToString(),
            Content = form["Body"].ToString(),
            ProviderId = (int)MessageProvider.Twilio,
            CreatedAt = DateTime.UtcNow
        };
    }

    private bool IsOptOutRequest(IFormCollection form)
    {
        if (form.TryGetValue("OptOutType", out var optOutType))
        {
            return string.Equals(optOutType.ToString(), "STOP", StringComparison.OrdinalIgnoreCase);
        }
        
        string body = form["Body"].ToString().Trim();
        return OptOutKeywords.Contains(body, StringComparer.OrdinalIgnoreCase);
    }

    private OptoutPhoneNumberModel GetOptoutPhoneNumberDetails(IFormCollection form,int inboundMessageId)
    {
        string dateString = form["DateCreated"].ToString();
        if (string.IsNullOrWhiteSpace(dateString))
        {
            dateString = form["DateSent"].ToString();
        }
        
        DateTime optedOutAt = DateTimeOffset.TryParse(dateString, out var parsedDate)
            ? parsedDate.UtcDateTime
            : DateTime.UtcNow;
        
        return new OptoutPhoneNumberModel
        {
            PhoneNumber = form["From"].ToString(),
            OptedOutAt = optedOutAt,
            InboundMessageId = inboundMessageId,
            ProviderId = (int)MessageProvider.Twilio
        };
    }
    
    private async Task<int> SaveOptOutPhoneNumber(IFormCollection form, int inboundMessageId)
    {
        var optoutDetails = GetOptoutPhoneNumberDetails(form, inboundMessageId);
        var optOutId=await _optoutPhoneNumberService.InsertOptoutPhoneNumberAsync(optoutDetails);
        return optOutId;
    }
}