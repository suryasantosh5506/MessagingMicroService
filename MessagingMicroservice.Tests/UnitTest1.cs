using MessagingMicroservice.Application.Models.ConsumerModels;
using MessagingMicroservice.Application.Services;
using MessagingMicroservice.Domain.Enums;
using MessagingMicroservice.Domain.Exceptions;
using MessagingMicroservice.Tests.Fakes;

namespace MessagingMicroservice.Tests;

public class UnitTest1
{
    [Fact]
    public async Task ControlFlowTest1()
    {
        var primaryProvider=new FakeMessageProvider();
        var fallBackProvider=new FakeFailingMessageProvider();
        var factory = new FakeMessageProviderFactory(primaryProvider,fallBackProvider);
        var repository = new FakeOutboundMessageRepository();
        
        var service=new SendMessageService(factory,repository);
        var request = new SendMessageRequest()
        {
            To = "+919378893890",
            Content = "Hello World!"
        };
        var response = await service.SendMessageAsync(request);
        Assert.Equal(1,response.MessageId);;
        Assert.Equal("12334487636248365665643434",repository.InsertedMessage!.ProviderMessageId);
        Assert.Equal("+919378893890",repository.InsertedMessage!.ToNumber);
        Assert.Equal("+917836476466",repository.InsertedMessage!.FromNumber);
        Assert.Equal(1,repository.InsertedMessage!.ProviderId);
        Assert.Equal(MessageStatus.Queued,Enum.Parse<MessageStatus>(repository.InsertedMessage!.Status));        
    }

    [Fact]
    public async Task FallbackTest()
    {
        var primaryProvider=new FakeFailingMessageProvider();
        var fallBackProvider=new FakeFallbackMessageProvider();
        var factory = new FakeMessageProviderFactory(primaryProvider, fallBackProvider);
        var repository = new FakeOutboundMessageRepository();
        var service=new SendMessageService(factory,repository);
        var request = new SendMessageRequest()
        {
            To = "+919378893890",
            Content = "Hello World!"
        };
        var response=await service.SendMessageAsync(request);
        
        Assert.True(fallBackProvider.Called);
        Assert.Equal(1,response.MessageId);
        Assert.Equal("12334487636248365665643434",repository.InsertedMessage!.ProviderMessageId);
        Assert.Equal("+919378893890",repository.InsertedMessage!.ToNumber);
        Assert.Equal("+917836476466",repository.InsertedMessage!.FromNumber);
        Assert.Equal(2,repository.InsertedMessage!.ProviderId);
        Assert.Equal(MessageStatus.Queued,Enum.Parse<MessageStatus>(repository.InsertedMessage!.Status));     
    }
    
    [Fact]
    public async Task ShouldNotUseFallback_WhenPrimaryFailureCannotFallback()
    {
        var primaryProvider=new FakeNonFallbackMessageProvider();
        var fallbackProvider=new FakeFallbackMessageProvider();
        var factory = new FakeMessageProviderFactory(primaryProvider, fallbackProvider);
        var repository = new FakeOutboundMessageRepository();
        var service=new SendMessageService(factory,repository);
        var ex =await Assert.ThrowsAsync<ProviderException>(async () =>
            await service.SendMessageAsync(new SendMessageRequest()));
        Assert.Equal("Invalid Number",ex.Message);
        Assert.False(ex.CanFallback);
    }
}
