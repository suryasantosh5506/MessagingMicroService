using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Application.Services;

public class OptoutPhoneNumberService:IOptoutPhoneNumberService
{

    private readonly IOptoutPhoneNumberRepository _optoutPhoneNumberRepository;
    
    public OptoutPhoneNumberService(IOptoutPhoneNumberRepository optoutPhoneNumberRepository)
    {
        _optoutPhoneNumberRepository = optoutPhoneNumberRepository;
    }
        
    public async Task<int> InsertOptoutPhoneNumberAsync(OptoutPhoneNumberModel optoutDetails)
    {
        var existingId=await GetOptoutIdByPhoneNumber(optoutDetails.PhoneNumber);
        if (existingId is not null) return existingId.Value;
        int id=await _optoutPhoneNumberRepository.InsertOptoutPhoneNumber(optoutDetails);
        return id;
    }

    public async Task<bool> IsOptedout(string phoneNumber)
    {
        return await _optoutPhoneNumberRepository.IsOptedout(phoneNumber);
    }

    public async Task<int?> GetOptoutIdByPhoneNumber(string phoneNumber)
    {
        return await _optoutPhoneNumberRepository.GetOptoutIdByPhoneNumber(phoneNumber);
    }
}