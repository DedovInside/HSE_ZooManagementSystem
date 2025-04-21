using Domain.Events;
namespace Application.EventHandling
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    { 
        Task HandleAsync(TEvent domainEvent);
    }
}