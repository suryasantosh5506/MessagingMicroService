using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Domain.Entities;

public class ProviderEntity
{
    public int Id { get; set; }
    public MessageProvider MessageProvider { get; set; }
}