using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Application.Services;

public class UserService:IUserService
{

    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> ExistsByEmail(string email)
    {
        return await _userRepository.ExistsByEmail(email);
    }

    public async Task<int> InsertUser(UserModel user)
    {
        return await _userRepository.InsertUser(user);
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }
}