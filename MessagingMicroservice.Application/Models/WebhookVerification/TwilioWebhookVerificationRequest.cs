namespace MessagingMicroservice.Application.Models.WebhookVerification;

public class TwilioWebhookVerificationRequest
{
    public string Url { get; set; } = null!;
    public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    public string Signature { get; set; } = null!;
}