using Microsoft.Extensions.Logging;

namespace App.Handlers;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Calling {name}", nameof(LoggingHandler));

        try
        {
            _logger.LogInformation("Request: {request}", request);
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LogInformation("Response: {response}", response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get response: {ex}", ex);
            throw;
        }
    }
}