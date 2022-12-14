using App.Handlers;
using App.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencies(this IServiceCollection services, HostBuilderContext context)
    {
        var apiKey = context.Configuration.GetValue<string>("Settings:ApiKey");
        var webHookUrl = context.Configuration.GetValue<string>("Settings:WebHookUrl");
        services
            .AddHttpClient<INotificationService, NotificationService>((_, client) =>
            {
                client.BaseAddress = new Uri(webHookUrl);
                client.DefaultRequestHeaders.Add("User-Agent", "HttpLoggingDemo");
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                }
            })
            .AddHttpMessageHandler<TimingHandler>()
            .AddHttpMessageHandler<LoggingHandler>()
            .AddHttpMessageHandler<ValidationHandler>()
            .AddHttpMessageHandler<CorrelationHandler>()
            .AddHttpMessageHandler<RetryHandler>();

        services.AddTransient<RetryHandler>();
        services.AddTransient<TimingHandler>();
        services.AddTransient<LoggingHandler>();
        services.AddTransient<ValidationHandler>();
        services.AddTransient<CorrelationHandler>();
        services.AddSingleton(GetTelemetryClient(context));
        services.AddSingleton<ICorrelationService, CorrelationService>();
        services.Configure<Settings>(context.Configuration.GetSection(nameof(Settings)));
    }

    private static TelemetryClient GetTelemetryClient(HostBuilderContext context)
    {
        var connectionString = context.Configuration["Serilog:WriteTo:2:Args:connectionString"];
        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        telemetryConfiguration.ConnectionString = connectionString;
        var telemetryClient = new TelemetryClient(telemetryConfiguration);
        return telemetryClient;
    }
}