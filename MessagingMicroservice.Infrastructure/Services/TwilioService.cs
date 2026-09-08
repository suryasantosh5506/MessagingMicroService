using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Exceptions;
using MessagingMicroservice.Infrastructure.StatusMappers;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Lookups.V2;
using Twilio.Types;

namespace MessagingMicroservice.Infrastructure.Services;

public class TwilioService : IMessageProvider
{
    private readonly IConfiguration _configuration;

    public TwilioService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        string accountSid = _configuration["Twilio:AccountSid"]!;
        string authToken = _configuration["Twilio:AuthToken"]!;
        string from = _configuration["Twilio:From"]!;

        TwilioClient.Init(accountSid, authToken);

        var lookUpResult = await PhoneNumberResource.FetchAsync(request.To);

        if (lookUpResult.Valid==false)
        {
            throw new ProviderException("Invalid PhoneNumber",false);
        }

        try
        {
            var messageResult = await MessageResource.CreateAsync(
                to: new PhoneNumber(request.To),
                from: new PhoneNumber(from),
                body: request.Content,
                statusCallback: new Uri(_configuration["Twilio:CallbackUrl"]!)
            );

            var rawStatus=messageResult.Status.ToString();
            var normalisedStatus=rawStatus.Replace("_","");
            TwilioMessageStatus status=TwilioMessageStatus.Queued;

            if (Enum.TryParse<TwilioMessageStatus>(normalisedStatus,ignoreCase:true ,out var parsedStatus))
            {
                status = parsedStatus;
            }

            return new ProviderSendMessageResult
            {
                ProviderMessageId = messageResult.Sid,
                Status = TwilioStatusMapper.Map(status),
                FromNumber = from,
                ToNumber = request.To,
                Provider = MessageProvider.Twilio
            };
        }
        catch (Twilio.Exceptions.ApiException e)
        {
            var errorCode = (TwilioMessageErrorCode)e.Code;

            bool canFallback =
                errorCode == TwilioMessageErrorCode.QueueOverflow ||
                errorCode == TwilioMessageErrorCode.CarrierNetworkCongestion ||
                errorCode == TwilioMessageErrorCode.ProviderTimeout ||
                errorCode == TwilioMessageErrorCode.AccountSuspended ||
                errorCode == TwilioMessageErrorCode.MessageFiltered ||
                errorCode == TwilioMessageErrorCode.UnknownError ||
                errorCode == TwilioMessageErrorCode.SenderIdPreRegistrationRequired ||
                errorCode == TwilioMessageErrorCode.OutboundMessagingDisabled ||
                errorCode == TwilioMessageErrorCode.OtpMessageBodyFiltered ||
                errorCode == TwilioMessageErrorCode.SenderRestrictedOrUnregistered ||
                errorCode == TwilioMessageErrorCode.AlphanumericSenderUnauthorized;

            throw new ProviderException(e.Message,canFallback);
        }
    }
}