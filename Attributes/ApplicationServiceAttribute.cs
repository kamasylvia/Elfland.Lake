using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApplicationServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    public ApplicationServiceAttribute() { }

    public ApplicationServiceAttribute(ServiceLifetime serviceLifetime) =>
        Lifetime = serviceLifetime;
}
