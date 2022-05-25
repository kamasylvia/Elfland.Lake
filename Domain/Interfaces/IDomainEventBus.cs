namespace Elfland.Lake.Domain.Interfaces;

public interface IDomainEventBus
{
    Task DispatchDomainEventsAsync();
}
