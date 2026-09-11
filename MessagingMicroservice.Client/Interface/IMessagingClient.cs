using MessagingMicroService.Client.Models;
namespace MessagingMicroService.Client.Interface;

public interface IMessagingClient
{
    Task<SendMessageResponse?> SendMessageAsync(SendMessageRequest request);
}