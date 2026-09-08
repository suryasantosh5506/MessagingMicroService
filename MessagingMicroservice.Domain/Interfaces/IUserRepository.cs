using MessagingMicroservice.Domain.Entities;
using MessagingMicroservice.Domain.Models;

namespace MessagingMicroservice.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByEmail(string email);

    Task<int> InsertUser(UserModel user);

    Task<UserEntity?> GetUserByEmail(string email);
}