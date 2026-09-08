using MessagingMicroservice.Application.Models;
using MessagingMicroservice.Application.Models.ConsumerModels;

namespace MessagingMicroservice.Application.Interfaces;

public interface ISendMessageService
{
    Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request); 
}