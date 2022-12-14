using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace App.Handlers;

public class TimingHandler : DelegatingHandler
{
    private readonly ILogger<TimingHandler> _logger;

    public TimingHandler(ILogger<TimingHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Calling {name}", nameof(TimingHandler));

        var sw = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
        finally
        {
            _logger.LogInformation("Http call took {time}", sw.Elapsed.ToString("mm\\:ss\\.ff"));
        }
    }
}