namespace MessagingMicroservice.Domain.Enums;

public enum TelnyxMessageStatus
{
    Queued,
    Sending,
    Sent,
    Delivered,
    SendingFailed,
    DeliveryFailed,
    DeliveryUnconfirmed
}