namespace MessagingMicroservice.Domain.Enums;

public enum TwilioMessageStatus
{
    Accepted,
    Scheduled,
    Canceled,
    Queued,
    Sending,
    Sent,
    Failed,
    Delivered,
    Undelivered,
}