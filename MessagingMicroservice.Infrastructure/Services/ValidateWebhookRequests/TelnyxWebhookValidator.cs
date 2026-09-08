using System.Text;
using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.WebhookVerification;
using Microsoft.Extensions.Configuration;
using NSec.Cryptography;

namespace MessagingMicroservice.Infrastructure.Services.ValidateWebhookRequests;

public class TelnyxWebhookValidator : IValidateWebhookRequest<TelnyxWebhookVerificationRequest>
{
    private readonly IConfiguration _configuration;
    private const int TimestampToleranceSeconds = 300; // 5-minute replay window

    public TelnyxWebhookValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> ValidateAsync(TelnyxWebhookVerificationRequest data)
    {
        var publicKeyBase64 = _configuration["Telnyx:WebhookPublicKey"];

        if (string.IsNullOrWhiteSpace(publicKeyBase64) ||
            string.IsNullOrWhiteSpace(data.Signature) ||
            string.IsNullOrWhiteSpace(data.Timestamp) ||
            string.IsNullOrWhiteSpace(data.Body))
        {
            return Task.FromResult(false);
        }

        try
        {
            // 1. Verify Timestamp (Replay Attack Check)
            if (!long.TryParse(data.Timestamp, out var unixTimestamp))
            {
                return Task.FromResult(false);
            }

            var requestTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            var now = DateTimeOffset.UtcNow;

            if (Math.Abs((now - requestTime).TotalSeconds) > TimestampToleranceSeconds)
            {
                return Task.FromResult(false);
            }

            // 2. Prepare payload bytes: "{timestamp}|{raw_body}"
            var signedPayload = $"{data.Timestamp}|{data.Body}";
            var payloadBytes = Encoding.UTF8.GetBytes(signedPayload);

            var publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
            var signatureBytes = Convert.FromBase64String(data.Signature);

            // 3. Verify Ed25519 Signature using NSec
            var algorithm = SignatureAlgorithm.Ed25519;
            
            // Import Telnyx raw 32-byte public key
            var publicKey = PublicKey.Import(algorithm, publicKeyBytes, KeyBlobFormat.RawPublicKey);

            // Validate signature against payload bytes
            bool isValid = algorithm.Verify(publicKey, payloadBytes, signatureBytes);

            return Task.FromResult(isValid);
        }
        catch
        {
            // Invalid base64, signature length mismatch, or key format error
            return Task.FromResult(false);
        }
    }
}