using System.Reflection;
using Elfland.Lake.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Extensions;

public static partial class ProgramExtensions
{
    /// <summary>
    /// Register all attributed services in the assembly to .NET IoC container.
    /// These services must be decorated with [ApplicationService] attribute.
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.RegisterServicesByAttribute(ServiceLifetime.Transient);
        services.RegisterServicesByAttribute(ServiceLifetime.Scoped);
        services.RegisterServicesByAttribute(ServiceLifetime.Singleton);
    }

    private static void RegisterServicesByAttribute(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime
    ) =>
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(
                implementationType =>
                    implementationType.GetCustomAttribute<ApplicationServiceAttribute>()?.Lifetime
                        == serviceLifetime
                    && implementationType.IsClass
                    && !implementationType.IsAbstract
            )
            .ToList()
            .ForEach(
                implementationType =>
                    implementationType
                        .GetInterfaces()
                        .ToList()
                        .ForEach(
                            serviceType =>
                            {
                                switch (serviceLifetime)
                                {
                                    case ServiceLifetime.Transient:
                                        services.AddTransient(serviceType, implementationType);
                                        break;
                                    case ServiceLifetime.Scoped:
                                        services.AddScoped(serviceType, implementationType);
                                        break;
                                    case ServiceLifetime.Singleton:
                                        services.AddSingleton(serviceType, implementationType);
                                        break;
                                }
                            }
                        )
            );
}
