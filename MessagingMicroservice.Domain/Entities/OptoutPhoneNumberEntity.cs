namespace MessagingMicroservice.Domain.Entities;

public class OptoutPhoneNumberEntity
{
    public int Id{get;set;}
    public string PhoneNumber { get; set; } = null!;
    public DateTime OptedOutAt{get;set;}
    public int? InboundMessageId{get;set;}
    public int ProviderId{get;set;}
}