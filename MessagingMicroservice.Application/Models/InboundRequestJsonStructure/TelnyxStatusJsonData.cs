namespace MessagingMicroservice.Application.Models.InboundRequestJsonStructure;

public class TelnyxStatusJsonData
{
    public TelnyxStatusData Data { get; set; } = null!;
}

public class TelnyxStatusData
{
    public string EventType { get; set; } = null!;
    public string EventId { get; set; } = null!; // Useful for idempotency checks
    public TelnyxStatusPayload Payload { get; set; } = null!;
}

public class TelnyxStatusPayload
{
    public string Id { get; set; } = null!;
    public string? OccurredAt { get; set; }
    public List<TelnyxStatusTo> To { get; set; } = [];
    public List<TelnyxStatusError>? Errors { get; set; } = []; 
}

public class TelnyxStatusTo
{
    public string PhoneNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
}

public class TelnyxStatusError
{
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Detail { get; set; }
}