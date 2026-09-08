using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Exceptions;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeNonFallbackMessageProvider:IMessageProvider
{
    public Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        throw new ProviderException("Invalid Number", false);
    }
}