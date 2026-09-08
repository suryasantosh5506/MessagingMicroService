using MessagingMicroservice.Application.Models;

namespace MessagingMicroservice.Application.Interfaces;

public interface IInboundMessageService
{
    Task<int> SaveInboundMessageAsync(InboundMessageRequest request);
}