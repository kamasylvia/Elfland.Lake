# Elfland.Lake
A tool box for WebAPI application.

# Library
## Automatic dependency injection.
Add `[ApplicationService]` to your service.

```cs
[ApplicationService(ServiceLifetime.Scope)] // Default life time is ServiceLifetime.Transient
public class CustomService
{
}
```

Add `builder.Services.AddApplicationServices()` to `Program.cs`
```cs
...

builder.Services.AddApplicationServices();

var app = builder.Build();
...
```