# Elfland.Lake
A tool box for WebAPI application.

# Library
## Automatic dependency injection.
Add `[Dependency]` to your service.

```cs
[Dependency(ServiceLifetime.Scope)] // Default life time is ServiceLifetime.Transient
public class CustomService
{
}
```

Add `builder.Services.AddDependencies()` to `Program.cs`
```cs
...

builder.Services.AddDependencies();

var app = builder.Build();
```