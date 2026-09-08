namespace MessagingMicroservice.Application.Models;

public class InboundMessageRequest
{
    public string MessageId { get; set; }=null!;
    public string FromNumber { get; set; }=null!;
    public string ToNumber { get; set; }=null!;
    public string Content { get; set; }=null!;
    public int ProviderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}