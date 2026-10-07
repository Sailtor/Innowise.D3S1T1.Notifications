using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notifications.Infrastructure.Messaging;
using Notifications.Infrastructure.Options;
using RabbitMQ.Client;

namespace Notifications.Infrastructure;

public static class DependencyInjectionRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddMessaging(services, configuration);
    }

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<MetricReadingsBatchConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                RabbitMqOptions options = ctx
                    .GetRequiredService<IOptions<RabbitMqOptions>>()
                    .Value;

                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                cfg.ReceiveEndpoint("metric-readings-notifications", e =>
                {
                    e.Bind("metric-readings", b => b.ExchangeType = ExchangeType.Fanout);
                    e.ConfigureConsumer<MetricReadingsBatchConsumer>(ctx);
                    e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(5)));
                });
            });
        });
    }
}
