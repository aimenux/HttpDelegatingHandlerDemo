using System.Net;
using App.Handlers;
using App.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tests.Helpers;

namespace Tests;

public class CorrelationHandlerTests
{
    [Fact]
    public async Task Should_Add_Correlation_Id()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<CorrelationHandler>();
        var correlationService = new CorrelationService();
        using var handler = new CorrelationHandler(correlationService, logger)
        {
            InnerHandler = new OkTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        request.Headers.TryGetValues("X-Correlation-Id", out _).Should().BeTrue();
    }
}