namespace MessagingMicroservice.Application.Models.WebhookVerification;

public class TelnyxWebhookVerificationRequest
{
    public string Body { get; set; } = null!;
    public string Signature { get; set; } = null!;
    public string Timestamp { get; set; } = null!;
}