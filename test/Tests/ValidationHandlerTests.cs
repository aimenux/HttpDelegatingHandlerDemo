using System.Net;
using App.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Tests.Helpers;

namespace Tests;

public class ValidationHandlerTests
{
    [Fact]
    public async Task When_ApiKey_Header_Exists_Then_Should_Get_200()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        request.Headers.Add("X-Api-Key", Guid.NewGuid().ToString("D"));
        var logger = new LoggerFactory().CreateLogger<ValidationHandler>();
        using var handler = new ValidationHandler(logger)
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
    public async Task When_ApiKey_Header_Not_Exists_Then_Should_Get_400()
    {
        // arrange
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.tests");
        var logger = new LoggerFactory().CreateLogger<ValidationHandler>();
        using var handler = new ValidationHandler(logger)
        {
            InnerHandler = new OkTestHandler()
        };
        using var invoker = new HttpMessageInvoker(handler);

        // act
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}