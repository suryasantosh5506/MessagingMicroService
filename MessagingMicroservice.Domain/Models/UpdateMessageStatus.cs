using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Domain.Models;

public class UpdateMessageStatus
{
    public string ProviderMessageId { get; set; } = null!;
    public MessageStatus Status { get; set; }
}