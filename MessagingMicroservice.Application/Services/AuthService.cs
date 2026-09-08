using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.AuthModels;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace MessagingMicroservice.Application.Services;

public class AuthService:IAuthService
{
    private readonly IUserService _userService;
    private readonly IPasswordHasher<UserModel> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUserService userService,IPasswordHasher<UserModel> passwordHasher,ITokenService tokenService)
    {
        _userService = userService;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }
    
    public async Task<int> RegisterAsync(RegisterRequest request)
    {
        bool exists =await _userService.ExistsByEmail(request.Email);
        if (exists) throw new Exception("Email Already Registered");
        var userModel = new UserModel() { Email = request.Email, PasswordHash = string.Empty };
        string passwordHash=_passwordHasher.HashPassword(userModel,request.Password);
        userModel.PasswordHash = passwordHash;
        return await _userService.InsertUser(userModel);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user =await _userService.GetUserByEmail(request.Email);
        if (user is null) throw new Exception("User not found");
        var userModel = new UserModel()
        {
            Email = request.Email,
            PasswordHash = string.Empty
        };
        if (_passwordHasher.VerifyHashedPassword(userModel, user.PasswordHash, request.Password) ==
            PasswordVerificationResult.Failed)
        {
            throw new Exception("Invalid Credentials");
        }

        return new LoginResponse()
        {
            Token = _tokenService.GenerateToken(user)
        };
    }
}