using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Infrastructure.StatusMappers;

public static class TelnyxStatusMapper
{
    public static MessageStatus Map(TelnyxMessageStatus status)
    {
        return status switch
        {
            TelnyxMessageStatus.Queued=>MessageStatus.Queued,
            TelnyxMessageStatus.Sending=>MessageStatus.Sending,
            TelnyxMessageStatus.Sent=>MessageStatus.Sent,
            TelnyxMessageStatus.Delivered=>MessageStatus.Delivered,
            TelnyxMessageStatus.SendingFailed=>MessageStatus.Failed,
            TelnyxMessageStatus.DeliveryFailed=>MessageStatus.Undelivered,
            _ => MessageStatus.Unknown
        };
    }
}