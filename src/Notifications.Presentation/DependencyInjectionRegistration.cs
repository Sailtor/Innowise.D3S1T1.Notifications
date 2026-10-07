using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Application;

namespace Notifications.Presentation;

public static class DependencyInjectionRegistration
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddScoped<IReadingNotificationPublisher, SignalRReadingNotificationPublisher>();
        services.AddHealthChecks();
    }
}
