using MessagingMicroservice.Application.Models.AuthModels;

namespace MessagingMicroservice.Application.Interfaces;

public interface IAuthService
{
    Task<int> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}