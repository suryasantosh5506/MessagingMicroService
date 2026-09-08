namespace MessagingMicroservice.Domain.Entities;

public class OutboundMessageEntity
{
    public int Id { get; set; }
    public string ProviderMessageId { get; set; } = null!;
    public string FromNumber { get; set; } = null!;
    public string ToNumber { get; set; } = null!;
    public int ProviderId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}