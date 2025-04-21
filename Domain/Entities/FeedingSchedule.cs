using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class FeedingSchedule : IHasDomainEvents
    {
        public Guid Id { get; private set; }
        public Guid AnimalId { get; private set; }
        public DateTime FeedingTime { get; private set; }
        public FoodType Food { get; private set; }
        public bool IsCompleted { get; private set; }
        private bool IsRecurring { get; set; } // Добавлено для повторяющихся кормлений
        private TimeSpan TimeOfDay => FeedingTime.TimeOfDay; // Только время суток для ежедневных кормлений
        
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        private FeedingSchedule() { }
        
        public static FeedingSchedule Create(Guid animalId, DateTime feedingTime, FoodType food, bool isRecurring = false)
        {
            FeedingSchedule schedule = new()
            {
                Id = Guid.NewGuid(),
                AnimalId = animalId,
                FeedingTime = feedingTime,
                Food = food,
                IsCompleted = false,
                IsRecurring = isRecurring
            };
            
            return schedule;
        }
        
        public void Reschedule(DateTime newFeedingTime)
        {
            FeedingTime = newFeedingTime;
            IsCompleted = false; // При перепланировании сбрасываем статус выполнения
        }
        
        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                
                // Если расписание повторяющееся, перепланируем на следующий день
                if (IsRecurring)
                {
                    DateTime tomorrow = DateTime.Today.AddDays(1).Add(TimeOfDay);
                    FeedingTime = tomorrow;
                    IsCompleted = false; // Сбрасываем флаг для нового периода
                    
                }
            }
        }

        public void RaiseFeedingTimeEvent()
        {
            // Для повторяющихся кормлений можно генерировать событие даже если IsCompleted=true
            if (IsCompleted && !IsRecurring)
            {
                throw new InvalidOperationException("Нельзя создать событие кормления для уже выполненного неповторяющегося расписания.");
            }
            
            _domainEvents.Add(new FeedingTimeEvent(Id, AnimalId, FeedingTime));
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}