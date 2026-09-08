using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Exceptions;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeFailingMessageProvider:IMessageProvider
{
    public Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        throw new ProviderException("Failed to send the message", true);
    }
}