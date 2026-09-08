namespace MessagingMicroservice.Domain.Enums;

public enum TwilioMessageErrorCode
{
    QueueOverflow = 30001,
    AccountSuspended = 30002,
    UnreachableDestination = 30003,
    MessageBlocked = 30004,
    UnknownDestination = 30005,
    LandlineOrUnreachableCarrier = 30006,
    MessageFiltered = 30007,
    UnknownError = 30008,
    MissingInboundSegment = 30009,
    MessagePriceExceedsMaxPrice = 30010,
    MmsNotSupported = 30011,
    TtlTooSmall = 30012,
    TtlTooBig = 30013,
    InvalidToAttributes = 30014,
    UnsupportedChannelType = 30015,
    IncompatibleChannelTypes = 30016,
    CarrierNetworkCongestion = 30017,
    SenderIdPreRegistrationRequired = 30018,
    ContentSizeExceedsCarrierLimit = 30019,
    OutboundMessagingDisabled = 30037,
    OtpMessageBodyFiltered = 30038,
    SenderRestrictedOrUnregistered = 30041,
    AlphanumericSenderUnauthorized = 30042,
    TrialMessageLengthExceeded = 30044,
    InvalidParameters = 30400,
    ProviderTimeout = 30410,
    MessageCouldNotBeDelivered = 30610,
    AttemptToSendToUnsubscribedRecipient = 30630
}