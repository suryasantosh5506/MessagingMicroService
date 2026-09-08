using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Application.Interfaces;

public interface IOptoutPhoneNumberService
{
    Task<int> InsertOptoutPhoneNumberAsync(OptoutPhoneNumberModel optoutDetails);
    Task<bool> IsOptedout(string phoneNumber);
    Task<int?> GetOptoutIdByPhoneNumber(string phoneNumber);
}