using MessagingMicroservice.Domain.Entities;

namespace MessagingMicroservice.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UserEntity user);
}