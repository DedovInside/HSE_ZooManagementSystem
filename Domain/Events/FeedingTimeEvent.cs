namespace Domain.Events
{
    public record FeedingTimeEvent(Guid ScheduleId, Guid AnimalId, DateTime FeedingTime) : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public Guid ScheduleId { get; } = ScheduleId;
        public Guid AnimalId { get; } = AnimalId;
        public DateTime FeedingTime { get; } = FeedingTime;
    }
}