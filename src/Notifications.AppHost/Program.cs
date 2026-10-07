using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Notifications.AppHost;
using Notifications.Application;
using Notifications.Infrastructure;
using Notifications.Presentation;
using Serilog;
using Serilog.Events;

// Bootstrap logger: without it, anything that fails before the host is built - a missing
// connection string, a bad schema - is written nowhere and the container exits silently.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddObservability();

    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPresentation(builder.Configuration);

    var app = builder.Build();

    // First in the pipeline so its one-line-per-request summary times everything below it.
    app.UseSerilogRequestLogging();

    // Liveness: no checks registered, so it answers "the process is up" even while RabbitMQ
    // is unreachable - there is no readiness endpoint because there is no dependency to be
    // ready on (no DB, see NOTIFICATION_SERVICE_PLAN.md Phase 3).
    app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

    app.MapHub<MetricsHub>("/hubs/metrics");

    await app.RunAsync();
}
catch (Exception exception) when (exception is not HostAbortedException)
{
    Log.Fatal(exception, "The Notifications service terminated unexpectedly during startup.");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
