using MessagingMicroservice.Application.Interfaces;
using MessagingMicroservice.Application.Models.WebhookVerification;
using MessagingMicroservice.Application.Services;
using MessagingMicroservice.Domain.Interfaces;
using MessagingMicroservice.Infrastructure.Data;
using MessagingMicroservice.Infrastructure.Repositories;
using MessagingMicroservice.Infrastructure.Services;
using MessagingMicroservice.Infrastructure.Services.ValidateWebhookRequests;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingMicroservice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddTransient<IMessageProviderFactory, MessageProviderFactory>();
        services.AddScoped<IOutboundMessageRepository, OutboundMessageRepository>();
        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IInboundMessageRepository, InboundMessageRepository>();
        services.AddTransient<IOptoutPhoneNumberRepository, OptoutPhoneNumberRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IValidateWebhookRequest<TwilioWebhookVerificationRequest>,TwilioWebhookValidator>();
        services.AddTransient<IValidateWebhookRequest<TelnyxWebhookVerificationRequest>,TelnyxWebhookValidator>();
        services.AddTransient<IUpdateMessageStatusService, UpdateMessageStatusService>();
        services.AddTransient<IInboundMessageService, InboundMessageService>();
        services.AddTransient<IOptoutPhoneNumberService, OptoutPhoneNumberService>();
        services.AddScoped<DapperContext>();
        services.AddHttpClient<TelnyxService>();
        services.AddTransient<TwilioService>();
        return services;
    }
}