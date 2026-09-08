using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeFallbackMessageProvider:IMessageProvider
{
    public bool Called { get; set; } = false;
    public async Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        Called=true;
        return await Task.FromResult(
            new ProviderSendMessageResult()
            {
                ProviderMessageId = "12334487636248365665643434",
                FromNumber = "+917836476466",
                ToNumber = "+919378893890",
                Provider=MessageProvider.Telnyx,
                Status = MessageStatus.Queued
            }
        );
    }
}