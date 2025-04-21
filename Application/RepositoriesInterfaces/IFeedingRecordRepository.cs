using Domain.Entities;

namespace Application.RepositoriesInterfaces
{
    public interface IFeedingRecordRepository
    {
        Task<FeedingRecord?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<FeedingRecord>> GetByScheduleIdAsync(Guid scheduleId);
        Task<IReadOnlyList<FeedingRecord>> GetByAnimalIdAsync(Guid animalId);
        Task<IReadOnlyList<FeedingRecord>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IReadOnlyList<FeedingRecord>> GetAllAsync();
        Task AddAsync(FeedingRecord record);
    }
}