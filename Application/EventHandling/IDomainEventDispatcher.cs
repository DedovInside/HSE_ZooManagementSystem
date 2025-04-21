using Domain.Events;
namespace Application.EventHandling
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IHasDomainEvents domainEntity);
    }
}