using System.Net;

namespace Tests.Helpers;

public class OkAfterRetryTestHandler : DelegatingHandler
{
    private int _retryAttempt = 0;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(++_retryAttempt == 2
            ? new HttpResponseMessage(HttpStatusCode.OK)
            : new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
    }
}