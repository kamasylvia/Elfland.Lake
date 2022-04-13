using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DependencyAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    public DependencyAttribute() { }

    public DependencyAttribute(ServiceLifetime serviceLifetime)
    {
        Lifetime = serviceLifetime;
    }
}
