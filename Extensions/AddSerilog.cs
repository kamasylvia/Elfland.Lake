using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Elfland.Lake.Extensions;

public static partial class ProgramExtensions
{
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog(
            (hostingContext, loggerConfig) =>
                loggerConfig.ReadFrom.Configuration(hostingContext.Configuration)
        );
    }
}
