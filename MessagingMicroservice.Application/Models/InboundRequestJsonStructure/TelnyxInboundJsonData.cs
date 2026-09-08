namespace MessagingMicroservice.Application.Models.InboundRequestJsonStructure;

public class TelnyxInboundJsonData
{
    public TelnyxInboundData Data { get; set; } = null!;
}

public class TelnyxInboundData
{
    public string EventType { get; set; } = null!;
    public TelnyxInboundPayload Payload { get; set; } = null!;
}

public class TelnyxInboundPayload
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string? ReceivedAt { get; set; }
    public string? SentAt { get; set; }
    public List<string>? Tags { get; set; } = new();
    public TelnyxInboundFrom From { get; set; } = null!;
    public List<TelnyxInboundTo> To { get; set; } = new();
}

public class TelnyxInboundFrom
{
    public string PhoneNumber { get; set; } = null!;
}

public class TelnyxInboundTo
{
    public string PhoneNumber { get; set; } = null!;
}