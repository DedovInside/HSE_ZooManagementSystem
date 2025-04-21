using Domain.ValueObjects;

namespace Domain.Entities
{
    public class FeedingRecord
    {
        public Guid Id { get; private set; }
        public Guid ScheduleId { get; private set; }
        public Guid AnimalId { get; private set; }
        public FoodType Food { get; private set; }
        public DateTime CompletedAt { get; private set; }

        private FeedingRecord() { }

        public static FeedingRecord Create(FeedingSchedule schedule)
        {
            return new FeedingRecord
            {
                Id = Guid.NewGuid(),
                ScheduleId = schedule.Id,
                AnimalId = schedule.AnimalId,
                Food = schedule.Food,
                CompletedAt = DateTime.UtcNow
            };
        }
    }
}