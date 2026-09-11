namespace MessagingMicroService.Client.Models;

public class SendMessageRequest
{
    public string To { get; set; } = null!;
    public string Content { get; set; } = null!;
}