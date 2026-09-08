using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Services;
using MessagingMicroservice.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingMicroservice.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddTransient<ISendMessageService, SendMessageService>();
        services.AddTransient<IInboundMessageService, InboundMessageService>();
        services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}