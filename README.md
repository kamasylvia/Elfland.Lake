# Elfland.Lake

A tool box for WebAPI application.

# Features

## Add Serilog

Install Prerequisites:

- `Serilog.Expressions`
- `Serilog.Sinks.Async`
- `Serilog.Sinks.Exceptionless`

Edit `Program.cs`
```cs
using System;
using Serilog;

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilog();

    ...

    var app = builder.Build();

    ...

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```

## Automatic dependency injection.

Add `[ApplicationService]` to your service.

```cs
[ApplicationService(ServiceLifetime.Scope)] // Default life time is ServiceLifetime.Transient
public class CustomService { }
```

Add `builder.Services.AddApplicationServices()` to `Program.cs`

```cs
...

builder.Services.AddApplicationServices();

var app = builder.Build();
...
```
