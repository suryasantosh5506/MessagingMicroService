using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Application.Models.ProviderModels;

public class ProviderSendMessageResult
{
    public string ProviderMessageId { get; set; } = null!;
    public string FromNumber { get; set; } = null!;
    public string ToNumber { get; set; } = null!;
    public MessageProvider Provider { get; set; }
    public MessageStatus Status { get; set; }
}