using Application.RepositoriesInterfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InMemoryFeedingRecordRepository : IFeedingRecordRepository
    {
        private readonly Dictionary<Guid, FeedingRecord> _feedingRecords = new();

        public Task<FeedingRecord?> GetByIdAsync(Guid id)
        {
            _feedingRecords.TryGetValue(id, out FeedingRecord? record);
            return Task.FromResult(record);
        }

        public Task<IReadOnlyList<FeedingRecord>> GetByScheduleIdAsync(Guid scheduleId)
        {
            List<FeedingRecord> records = _feedingRecords.Values
                .Where(r => r.ScheduleId == scheduleId)
                .ToList();
            return Task.FromResult<IReadOnlyList<FeedingRecord>>(records);
        }

        public Task<IReadOnlyList<FeedingRecord>> GetByAnimalIdAsync(Guid animalId)
        {
            List<FeedingRecord> records = _feedingRecords.Values
                .Where(r => r.AnimalId == animalId)
                .ToList();
            return Task.FromResult<IReadOnlyList<FeedingRecord>>(records);
        }

        public Task<IReadOnlyList<FeedingRecord>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            List<FeedingRecord> records = _feedingRecords.Values
                .Where(r => r.CompletedAt >= start && r.CompletedAt <= end)
                .ToList();
            return Task.FromResult<IReadOnlyList<FeedingRecord>>(records);
        }

        public Task<IReadOnlyList<FeedingRecord>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<FeedingRecord>>(_feedingRecords.Values.ToList());
        }

        public Task AddAsync(FeedingRecord record)
        {
            _feedingRecords[record.Id] = record;
            return Task.CompletedTask;
        }
    }
}