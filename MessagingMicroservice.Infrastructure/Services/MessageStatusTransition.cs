using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Infrastructure.Services;

public static class MessageStatusTransition
{
    public static bool IsValid(MessageStatus current,MessageStatus incoming)
    {
        if (current == incoming)
            return false;

        return current switch
        {
            MessageStatus.Pending =>
                incoming == MessageStatus.Queued ||
                incoming == MessageStatus.Sending ||
                incoming == MessageStatus.Sent ||
                incoming == MessageStatus.Failed ||
                incoming == MessageStatus.Cancelled,

            MessageStatus.Queued =>
                incoming == MessageStatus.Sending ||
                incoming == MessageStatus.Sent ||
                incoming == MessageStatus.Failed ||
                incoming == MessageStatus.Cancelled,

            MessageStatus.Sending =>
                incoming == MessageStatus.Sent ||
                incoming == MessageStatus.Failed ||
                incoming == MessageStatus.Undelivered,

            MessageStatus.Sent =>
                incoming == MessageStatus.Delivered ||
                incoming == MessageStatus.Undelivered ||
                incoming == MessageStatus.Failed,

            MessageStatus.Delivered =>
                false,

            MessageStatus.Undelivered =>
                false,

            MessageStatus.Failed =>
                false,

            MessageStatus.Cancelled =>
                false,

            MessageStatus.Unknown =>
                incoming != MessageStatus.Unknown,

            _ => false
        };
    }
}