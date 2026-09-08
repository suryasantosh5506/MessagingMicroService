using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IOutboundMessageRepository
{
    Task<OutboundMessageEntity?> GetMessageById(int id);
    Task<OutboundMessageEntity?> GetMessageByProviderId(string providerMessageId);
    Task<int> InsertMessage(OutboundMessageEntity inboundMessage);
    Task UpdateMessage(UpdateMessageStatus request);
}