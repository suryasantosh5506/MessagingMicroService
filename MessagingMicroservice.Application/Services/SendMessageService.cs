using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ConsumerModels;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Exceptions;
using MessagingMicroservice.Domain.Interfaces;

namespace MessagingMicroservice.Application.Services;

public class SendMessageService:ISendMessageService
{
    private readonly IMessageProviderFactory _providerFactory;
    private readonly IOutboundMessageRepository _outboundMessageRepository;
    private readonly IOptoutPhoneNumberService _optoutPhoneNumberService;

    public SendMessageService(IMessageProviderFactory providerFactory,
        IOutboundMessageRepository outboundMessageRepository,
        IOptoutPhoneNumberService optoutPhoneNumberService)
    {
        _providerFactory = providerFactory;
        _outboundMessageRepository = outboundMessageRepository;
        _optoutPhoneNumberService = optoutPhoneNumberService;
    }
    
    public async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request)
    {
        bool isOptedout = await _optoutPhoneNumberService.IsOptedout(request.To);
        if(isOptedout) throw new Exception("User opted out");
        
        var primaryProvider = _providerFactory.GetPrimaryMessageProvider();
        
        var providerRequest = new ProviderSendMessageRequest()
        {
            To = request.To,
            Content = request.Content,
        };

        try
        {
            var result = await primaryProvider.SendMessageAsync(providerRequest);
            return await SaveMessage(result);
        }
        catch (ProviderException e)
        {
            if (!e.CanFallback) throw;

            var fallBackProvider = _providerFactory.GetFallBackMessageProvider();

            var result = await fallBackProvider.SendMessageAsync(providerRequest);
            return await SaveMessage(result);
        }
    }
    
    private async Task<SendMessageResponse> SaveMessage(ProviderSendMessageResult result)
    {
        var message = new OutboundMessageEntity()
        {
            ProviderMessageId = result.ProviderMessageId,
            FromNumber = result.FromNumber,
            ToNumber = result.ToNumber,
            ProviderId = (int)result.Provider,
            Status = result.Status.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var messageId =await _outboundMessageRepository.InsertMessage(message);

        return new SendMessageResponse
        {
            MessageId = messageId
        };
    }
}