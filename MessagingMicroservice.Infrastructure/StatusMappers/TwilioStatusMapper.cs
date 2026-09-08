using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Infrastructure.StatusMappers;

public static class TwilioStatusMapper
{
    public static MessageStatus Map(TwilioMessageStatus status)
    {
        return status switch
        {
            TwilioMessageStatus.Accepted => MessageStatus.Pending,
            TwilioMessageStatus.Scheduled => MessageStatus.Pending,
            TwilioMessageStatus.Canceled => MessageStatus.Cancelled,
            TwilioMessageStatus.Queued => MessageStatus.Queued,
            TwilioMessageStatus.Sending => MessageStatus.Sending,
            TwilioMessageStatus.Sent => MessageStatus.Sent,
            TwilioMessageStatus.Failed => MessageStatus.Failed,
            TwilioMessageStatus.Delivered => MessageStatus.Delivered,
            TwilioMessageStatus.Undelivered => MessageStatus.Undelivered,
            _ => MessageStatus.Unknown
        };
    }
}