using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Application.Interfaces;

public interface IUpdateMessageStatusService
{
    Task UpdateMessageStatusAsync(UpdateMessageStatus request);
}