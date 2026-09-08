using MessagingMicroservice.Domain.Enums;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IProviderRepository
{
    Task<int> InsertProvider(MessageProvider provider);
}