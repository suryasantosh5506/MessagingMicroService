using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.ProviderModels;
using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Tests.Fakes;

public class FakeMessageProvider:IMessageProvider
{
    public async Task<ProviderSendMessageResult> SendMessageAsync(ProviderSendMessageRequest request)
    {
        return await Task.FromResult(
                new ProviderSendMessageResult()
                {
                    ProviderMessageId = "12334487636248365665643434",
                    FromNumber = "+917836476466",
                    ToNumber = "+919378893890",
                    Provider=MessageProvider.Twilio,
                    Status = MessageStatus.Queued
                }
            );
    }
}