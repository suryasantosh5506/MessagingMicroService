using MessagingMicroservice.Domain.Entities;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IInboundMessageRepository
{
    Task<InboundMessageEntity?> GetMessage(int id);
    Task<int> InsertMessage(InboundMessageEntity message);
}