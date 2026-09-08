namespace MessagingMicroservice.Domain.Enums;

public enum TelnyxMessageErrorCode
{
    NotRoutable = 40001,
    BlockedAsSpamTemporary = 40002,
    BlockedAsSpamPermanent = 40003,
    RejectedByDestination = 40004,
    MessageExpired = 40005,
    RecipientUnavailable = 40006,
    Undeliverable = 40008,
    InvalidMessageBody = 40009,
    RateLimitExceeded = 40011,
    InvalidDestinationNumber = 40012,
    InvalidSourceNumber = 40013,
    ExpiredInQueue = 40014,
    InternalSpamFilter = 40015,
    BlockedStop = 40300,
    UnsupportedMessageType = 40301,
    InvalidFromAddress = 40305,
    InvalidDestinationRegion = 40309,
    InvalidToAddress = 40310,
    IncompatibleMessageType = 40319,
    TemporarilyUnusableSender = 40320,
    NoUsableNumbersInPool = 40321,
    InvalidAlphaSenderId = 40325
}