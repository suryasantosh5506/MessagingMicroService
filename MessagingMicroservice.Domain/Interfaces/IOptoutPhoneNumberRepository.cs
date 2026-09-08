using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IOptoutPhoneNumberRepository
{
    Task<int> InsertOptoutPhoneNumber(OptoutPhoneNumberModel optOutDetails);
    Task<bool> IsOptedout(string phoneNumber);
    Task<int?> GetOptoutIdByPhoneNumber(string phoneNumber);
}