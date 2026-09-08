using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;

namespace MessagingMicroservice.Application.Services;

public class InboundMessageService:IInboundMessageService
{

    private readonly IInboundMessageRepository _inboundMessageRepository;

    public InboundMessageService(IInboundMessageRepository inboundMessageRepository)
    {
        _inboundMessageRepository = inboundMessageRepository;
    }
    
    public async Task<int> SaveInboundMessageAsync(InboundMessageRequest request)
    {
        var message = new InboundMessageEntity()
        {
            ProviderMessageId = request.MessageId,
            FromNumber = request.FromNumber,
            ToNumber = request.ToNumber,
            Content = request.Content,
            CreatedAt = request.CreatedAt,
            ProviderId = request.ProviderId,
        };
        int id=await _inboundMessageRepository.InsertMessage(message);
        return id;
    }
}