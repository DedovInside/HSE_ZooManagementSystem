namespace Domain.Events
{
    public record AnimalMovedEvent(Guid AnimalId, Guid? FromEnclosureId, Guid ToEnclosureId) : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid? FromEnclosureId { get; } = FromEnclosureId;
        public Guid ToEnclosureId { get; } = ToEnclosureId;
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}