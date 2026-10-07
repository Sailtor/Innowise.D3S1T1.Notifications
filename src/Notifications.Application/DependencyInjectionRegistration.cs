using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Notifications.Application;

public static class DependencyInjectionRegistration
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IThresholdRuleEvaluator, ThresholdRuleEvaluator>();
    }
}
