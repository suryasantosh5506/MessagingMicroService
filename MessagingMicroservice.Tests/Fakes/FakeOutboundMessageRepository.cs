using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeOutboundMessageRepository:IOutboundMessageRepository
{
    public OutboundMessageEntity? InsertedMessage { get; set; }
    
    public Task<OutboundMessageEntity?> GetMessageById(int id)
    {
        return Task.FromResult<OutboundMessageEntity?>(null);
    }

    public Task<OutboundMessageEntity?> GetMessageByProviderId(string providerMessageId)
    {
        return Task.FromResult<OutboundMessageEntity?>(null);
    }

    public Task<int> InsertMessage(OutboundMessageEntity inboundMessage)
    {
        InsertedMessage = inboundMessage;
        return Task.FromResult(1);
    }

    public Task UpdateMessage(UpdateMessageStatus request)
    {
        return Task.CompletedTask;
    }
}