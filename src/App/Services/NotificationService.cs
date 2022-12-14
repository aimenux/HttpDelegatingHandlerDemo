using System.Text;
using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;

namespace App.Services;

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly TelemetryClient _telemetryClient;
    private readonly ICorrelationService _correlationService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(HttpClient httpClient, TelemetryClient telemetryClient, ICorrelationService correlationService, ILogger<NotificationService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        _correlationService = correlationService ?? throw new ArgumentNullException(nameof(correlationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendAsync(Notification notification, CancellationToken cancellationToken)
    {
        var correlationId = _correlationService.GetCorrelationId();
        var operationName = $"Sending notification {correlationId}";
        using var operation = _telemetryClient.StartOperation<RequestTelemetry>(operationName);
        using var scope = _logger.BeginScope(GetCustomProperties());
        var json = JsonSerializer.Serialize(notification);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content, cancellationToken);
        var message = response.IsSuccessStatusCode
            ? "Succeeded to send notification {id}"
            : "Failed to send notification {id}";
        _logger.LogInformation(message, correlationId);
    }

    private static IDictionary<string, object> GetCustomProperties()
    {
        var customProperties = new Dictionary<string, object>
        {
            ["UserName"] = Environment.UserName,
            ["MachineName"] = Environment.MachineName
        };

        return customProperties;
    }
}