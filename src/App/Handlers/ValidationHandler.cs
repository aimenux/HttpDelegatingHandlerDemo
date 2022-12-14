using System.Net;
using Microsoft.Extensions.Logging;

namespace App.Handlers;

public class ValidationHandler : DelegatingHandler
{
    private const string ApiKeyName = "X-Api-Key";

    private readonly ILogger<ValidationHandler> _logger;

    public ValidationHandler(ILogger<ValidationHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Calling {name}", nameof(ValidationHandler));

        if (!request.Headers.Contains(ApiKeyName))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent($"Missing api key header {ApiKeyName}!")
            };
        }

        return await base.SendAsync(request, cancellationToken);
    }
}