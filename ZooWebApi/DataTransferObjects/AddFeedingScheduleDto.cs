using Domain.ValueObjects;
namespace ZooWebApi.DataTransferObjects
{
    public class AddFeedingScheduleDto
    {
        public Guid AnimalId { get; set; }
        public DateTime FeedingTime { get; set; }
        public FoodType FoodType { get; set; }
        public bool IsRecurring { get; set; }
    }
}