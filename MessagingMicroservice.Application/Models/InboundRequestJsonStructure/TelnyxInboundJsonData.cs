using System.Text.Json.Serialization;

namespace MessagingMicroservice.Application.Models.InboundRequestJsonStructure;

public class TelnyxInboundJsonData
{
    public TelnyxInboundData Data { get; set; } = null!;
}

public class TelnyxInboundData
{
    [JsonPropertyName("event_type")]
    public string EventType { get; set; } = null!;
    public TelnyxInboundPayload Payload { get; set; } = null!;
}

public class TelnyxInboundPayload
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    [JsonPropertyName("received_at")]
    public string? ReceivedAt { get; set; }
    [JsonPropertyName("sent_at")]
    public string? SentAt { get; set; }
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; } = new();
    public TelnyxInboundFrom From { get; set; } = null!;
    public List<TelnyxInboundTo> To { get; set; } = new();
}

public class TelnyxInboundFrom
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = null!;
}

public class TelnyxInboundTo
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = null!;
}