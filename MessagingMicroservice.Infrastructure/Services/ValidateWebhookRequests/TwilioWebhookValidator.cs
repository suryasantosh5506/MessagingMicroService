using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.WebhookVerification;
using Microsoft.Extensions.Configuration;
using Twilio.Security;

namespace MessagingMicroservice.Infrastructure.Services.ValidateWebhookRequests;

public class TwilioWebhookValidator: IValidateWebhookRequest<TwilioWebhookVerificationRequest>
{
    
    private readonly IConfiguration _configuration;
    
    public TwilioWebhookValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public Task<bool> ValidateAsync(TwilioWebhookVerificationRequest request)
    {
        var authToken=_configuration["Twilio:AuthToken"];
        if (string.IsNullOrWhiteSpace(authToken))
        {
            return Task.FromResult(false);
        }
        var validator=new RequestValidator(authToken);
        bool isValid=validator.Validate(request.Url,request.Parameters,request.Signature);
        return Task.FromResult(isValid);
    }
}