namespace Elfland.Lake.Domain;

public interface IDomainEventBus
{
    Task DispatchDomainEventsAsync();
}
