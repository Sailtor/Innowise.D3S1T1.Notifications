using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Application;

namespace Notifications.Presentation;

public static class DependencyInjectionRegistration
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        AddCorsPolicy(services, configuration);
        services.AddSignalR();
        services.AddScoped<IReadingNotificationPublisher, SignalRReadingNotificationPublisher>();
        services.AddHealthChecks();
    }

    private static void AddCorsPolicy(IServiceCollection services, IConfiguration configuration)
    {
        // Empty by default: the frontend origin is deployment configuration, not a source-code
        // constant, and a wide-open default is not something to inherit by accident.
        string[] allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options => options.AddDefaultPolicy(policy => policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()

            // The SignalR JS client sends credentials on its negotiate/WebSocket handshake by
            // default. CORS rejects a credentialed request against a wildcard origin, so this is
            // only valid paired with the explicit WithOrigins(...) above, never AllowAnyOrigin().
            .AllowCredentials()));
    }
}
