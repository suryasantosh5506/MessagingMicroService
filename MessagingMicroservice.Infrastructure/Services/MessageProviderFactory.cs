using MessagingMicroservice.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MessagingMicroservice.Infrastructure.Services;

public class MessageProviderFactory:IMessageProviderFactory
{

    private readonly IConfiguration _configuration;
    private readonly TwilioService _twilioService;
    private readonly TelnyxService _telnyxService;
    
    public MessageProviderFactory(IConfiguration configuration, TwilioService twilioService,
        TelnyxService telnyxService)
    {
        _configuration = configuration;
        _twilioService = twilioService;
        _telnyxService = telnyxService;
    }
    
    public IMessageProvider GetPrimaryMessageProvider()
    {
        var provider = _configuration["Messaging:PrimaryProvider"];
        return provider switch
        {
            "Twilio" => _twilioService,
            "Telnyx" => _telnyxService,
            _=> throw new InvalidOperationException($"Unsupported message provider: {provider}")
        };
    }

    public IMessageProvider GetFallBackMessageProvider()
    {
        var provider = _configuration["Messaging:FallbackProvider"];
        return provider switch
        {
            "Twilio" => _twilioService,
            "Telnyx" => _telnyxService,
            _=> throw new InvalidOperationException($"Unsupported message provider: {provider}")
        };
    }
}