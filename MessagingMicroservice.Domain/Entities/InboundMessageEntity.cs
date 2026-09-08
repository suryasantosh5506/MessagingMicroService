namespace MessagingMicroservice.Domain.Entities;

public class InboundMessageEntity
{
    public int Id { get; set; }
    public string ProviderMessageId { get; set; } = null!;
    public string FromNumber { get; set; } = null!;
    public string ToNumber { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int ProviderId { get; set; }
    public DateTime CreatedAt { get; set; }
}