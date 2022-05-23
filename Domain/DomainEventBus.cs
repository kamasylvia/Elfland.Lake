using Elfland.Lake.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Elfland.Lake.Domain;

[ApplicationService(ServiceLifetime.Scoped)]
public class DomainEventBus : IDomainEventBus
{
    private readonly IMediator _mediator;
    public List<INotification>? DomainEvents { get; set; }

    public DomainEventBus(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task DispatchDomainEventsAsync()
    {
        foreach (var domainEvent in DomainEvents!)
        {
            await _mediator.Publish(domainEvent);
        }
        DomainEvents.Clear();
    }
}
