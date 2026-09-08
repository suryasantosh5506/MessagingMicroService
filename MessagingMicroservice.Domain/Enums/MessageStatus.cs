namespace MessagingMicroservice.Domain.Enums;

public enum MessageStatus
{
    Pending,
    Queued,
    Sending,
    Sent,
    Delivered,
    Undelivered,
    Failed,
    Cancelled,
    Unknown
}