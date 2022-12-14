[![.NET](https://github.com/aimenux/HttpDelegatingHandlerDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/HttpDelegatingHandlerDemo/actions/workflows/ci.yml)

# HttpDelegatingHandlerDemo
```
Using http delegating handlers
```

> In this repo, i m using [http delegation handlers](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.delegatinghandler) in various ways :
>
> - `TimingHandler` : log request processing time
>
> - `LoggingHandler` : log request/response body
>
> - `ValidationHandler` : validate required request headers
>
> - `CorrelationHandler` : add correlation id to request headers
>
> - `RetryHandler` : retry request when transient errors occurs
>
>

**`Tools`** : vs22, net 6.0, console-app, app-insights, xunit, fluent-assertions