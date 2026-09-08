using MessagingMicroservice.Application.Models.ProviderModels;

namespace MessagingMicroservice.Application.Interfaces;

public interface IMessageProvider
{
    Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request);
}