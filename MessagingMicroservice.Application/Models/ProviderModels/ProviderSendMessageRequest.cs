namespace MessagingMicroservice.Application.Models.ProviderModels;

public class ProviderSendMessageRequest
{
    public string To { get; set; } = null!;
    public string Content { get; set; } = null!;
}