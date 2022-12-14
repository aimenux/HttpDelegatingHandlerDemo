using System.Net;
using App.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tests.Helpers;

namespace Tests;

public class RetryHandlerTests
{
    [Fact]
    public async Task Should_Get_200()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<RetryHandler>();
        using var handler = new RetryHandler(logger)
        {
            InnerHandler = new OkTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Get_Another_200()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<RetryHandler>();
        using var handler = new RetryHandler(logger)
        {
            InnerHandler = new OkAfterRetryTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Get_500()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<RetryHandler>();
        using var handler = new RetryHandler(logger)
        {
            InnerHandler = new KoTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Should_Get_Another_500()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<RetryHandler>();
        using var handler = new RetryHandler(logger)
        {
            InnerHandler = new KoAfterRetryTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}