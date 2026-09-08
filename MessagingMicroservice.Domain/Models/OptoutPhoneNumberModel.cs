namespace MessagingMicroservice.Domain.Models;

public class OptoutPhoneNumberModel
{
    public string PhoneNumber { get; set; } = null!;
    public DateTime OptedOutAt{get;set;}
    public int InboundMessageId{get;set;}
    public int ProviderId{get;set;}
}