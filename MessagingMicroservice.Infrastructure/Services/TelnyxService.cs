using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Exceptions;
using MessagingMicroservice.Infrastructure.StatusMappers;
using Microsoft.Extensions.Configuration;
using Telnyx;

namespace MessagingMicroservice.Infrastructure.Services;

public class TelnyxService : IMessageProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TelnyxService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        var apiKey = _configuration["Telnyx:ApiKey"];
        var fromNumber = _configuration["Telnyx:From"];
        var callbackUrl = _configuration["Telnyx:CallbackUrl"];

        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(fromNumber))
        {
            throw new ProviderException("Telnyx configuration settings are missing.", canFallback: true);
        }

        try
        {
            string url = $"https://api.telnyx.com/v2/number_lookup/{request.To}?type=carrier";
            using var getRequest = new HttpRequestMessage(HttpMethod.Get, url);
            getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            using var response = await _httpClient.SendAsync(getRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new ProviderException("Invalid Phone Number",false);
            }

            TelnyxConfiguration.SetApiKey(apiKey);
            var service = new MessageService();

            var options = new NewMessage
            {
                From = fromNumber,
                To = request.To,
                Text = request.Content,
                WebhookUrl = callbackUrl
            };

            var message = await service.CreateAsync(options);

            var jsonStatus = message.To?.FirstOrDefault()?.Status?.ToString();
            TelnyxMessageStatus status = TelnyxMessageStatus.Queued;

            if (!string.IsNullOrEmpty(jsonStatus))  
            {
                string normalisedStatus = jsonStatus.Replace("_", "").Replace("-", "");

                if (Enum.TryParse<TelnyxMessageStatus>(normalisedStatus, ignoreCase: true, out var castedStatus))
                {
                    status = castedStatus;
                }
            }

            return new ProviderSendMessageResult
            {
                ProviderMessageId = message.Id.ToString()!,
                Status = TelnyxStatusMapper.Map(status),
                FromNumber = fromNumber,
                ToNumber = request.To,
                Provider = MessageProvider.Telnyx
            };
        }
        catch (HttpRequestException ex)
        {
            throw new ProviderException($"Telnyx connection failure: {ex.Message}", canFallback: true);
        }
        catch (Telnyx.TelnyxException e)
        {
            var telnyxError = e.TelnyxErrors?.FirstOrDefault();
            if (telnyxError == null)
            {
                throw new ProviderException(e.Message, canFallback: true);
            }

            bool isValidCode = int.TryParse(telnyxError.Code, out var code);

            bool canFallback =
                isValidCode && (
                    code == (int)TelnyxMessageErrorCode.RateLimitExceeded ||
                    code == (int)TelnyxMessageErrorCode.TemporarilyUnusableSender ||
                    code == (int)TelnyxMessageErrorCode.BlockedAsSpamPermanent ||
                    code == (int)TelnyxMessageErrorCode.BlockedAsSpamTemporary ||
                    code == (int)TelnyxMessageErrorCode.ExpiredInQueue ||
                    code == (int)TelnyxMessageErrorCode.InternalSpamFilter ||
                    code == (int)TelnyxMessageErrorCode.InvalidSourceNumber ||
                    code == (int)TelnyxMessageErrorCode.InvalidFromAddress ||
                    code == (int)TelnyxMessageErrorCode.NoUsableNumbersInPool ||
                    code == (int)TelnyxMessageErrorCode.InvalidAlphaSenderId
                );

            throw new ProviderException(e.Message, canFallback);
        }
    }
}