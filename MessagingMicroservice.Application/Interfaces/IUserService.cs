using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Application.Interfaces;

public interface IUserService
{
    Task<bool> ExistsByEmail(string email);

    Task<int> InsertUser(UserModel user);

    Task<UserEntity?> GetUserByEmail(string email);
}