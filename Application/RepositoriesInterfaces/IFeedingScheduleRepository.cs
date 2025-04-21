using Domain.Entities;
using Domain.ValueObjects;
namespace Application.RepositoriesInterfaces
{
    public interface IFeedingScheduleRepository
    {
        Task<FeedingSchedule?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<FeedingSchedule>> GetAllAsync();
        Task<IReadOnlyList<FeedingSchedule>> GetByTimeAsync(DateTime time);
        Task<IReadOnlyList<FeedingSchedule>> GetAvailableAsync(AnimalType animalType);
        Task<IReadOnlyList<FeedingSchedule>> GetPendingFeedingsAsync(DateTime before);
        Task AddAsync(FeedingSchedule enclosure);
        Task UpdateAsync(FeedingSchedule enclosure);
        Task DeleteAsync(Guid id);
    }
}