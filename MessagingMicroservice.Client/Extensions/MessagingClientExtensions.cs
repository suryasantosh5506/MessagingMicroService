using MessagingMicroService.Client.Interface;
using MessagingMicroservice.Client.Options;
using MessagingMicroservice.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingMicroservice.Client.Extensions;

public static class MessagingClientExtensions
{
    public static IServiceCollection AddMessagingClient(this IServiceCollection services,
        Action<MessagingClientOptions> configure)
    {
        var options = new MessagingClientOptions();
        configure(options);
        services.AddSingleton(options);
        services.AddHttpClient<IMessagingClient,MessagingClient>(client => client.BaseAddress = new Uri(options.BaseUrl));
        return services;
    } 
}