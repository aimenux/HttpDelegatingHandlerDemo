using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;

namespace App.Extensions;

public static class LoggingExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
    {
        return builder.UseSerilog((hostingContext, _, loggerConfiguration) =>
        {
            SelfLog.Enable(Console.Error);
            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration);
        });
    }
}