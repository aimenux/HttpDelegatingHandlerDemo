using App.Services;
using Microsoft.Extensions.Logging;

namespace App.Handlers;

public class CorrelationHandler : DelegatingHandler
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    private readonly ICorrelationService _correlationService;
    private readonly ILogger<CorrelationHandler> _logger;

    public CorrelationHandler(ICorrelationService correlationService, ILogger<CorrelationHandler> logger)
    {
        _correlationService = correlationService ?? throw new ArgumentNullException(nameof(correlationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Calling {name}", nameof(CorrelationHandler));
        request.Headers.Add(CorrelationIdHeaderName, _correlationService.GetCorrelationId());
        return await base.SendAsync(request, cancellationToken);
    }
}