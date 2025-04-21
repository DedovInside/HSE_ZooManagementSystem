using Domain.Events;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.EventHandling
{
    public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
    {
        public async Task DispatchEventsAsync(IHasDomainEvents aggregateRoot)
        {
            List<IDomainEvent> events = aggregateRoot.DomainEvents.ToList();
            if (!events.Any())
            {
                return;
            }

            foreach (IDomainEvent domainEvent in events)
            {
                Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                IEnumerable<object?> handlers = serviceProvider.GetServices(handlerType);

                foreach (object? handler in handlers)
                {
                    if (handler == null)
                    {
                        continue;
                    }
                    MethodInfo? method = handlerType.GetMethod("HandleAsync");
                    if (method != null)
                    {
                        object? result = method.Invoke(handler, [domainEvent]);
                        if (result != null)
                        {
                            await (Task)result;
                        }
                    }
                }
            }

            aggregateRoot.ClearDomainEvents();
        }
    }
}