using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace App.Handlers
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetry = 3;

        private readonly ILogger<RetryHandler> _logger;

        public RetryHandler(ILogger<RetryHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Calling {name}", nameof(RetryHandler));

            HttpResponseMessage response = null;

            for (var index = 1; index <= MaxRetry; index++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);
                    if (!IsTransientFailure(response)) return response;
                    await Task.Delay(GetDelay(index), cancellationToken);
                }
                catch (Exception ex) when (IsNetworkFailure(ex))
                {
                    await Task.Delay(GetDelay(index), cancellationToken);
                }
            }

            return response;
        }

        private static TimeSpan GetDelay(int retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

        private static bool IsTransientFailure(HttpResponseMessage response)
        {
            return response.StatusCode
                is HttpStatusCode.RequestTimeout
                or HttpStatusCode.GatewayTimeout
                or HttpStatusCode.TooManyRequests
                or HttpStatusCode.ServiceUnavailable;
        }

        private static bool IsNetworkFailure(Exception ex)
        {
            if (ex is null) return false;
            return ex is SocketException
                   || IsNetworkFailure(ex.InnerException);
        }
    }
}
