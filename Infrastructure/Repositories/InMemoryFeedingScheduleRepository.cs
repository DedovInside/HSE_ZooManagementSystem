using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Repositories
{
    public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
    {
        private readonly Dictionary<Guid, FeedingSchedule> _feedingSchedules = new();

        public Task<FeedingSchedule?> GetByIdAsync(Guid id)
        {
            _feedingSchedules.TryGetValue(id, out FeedingSchedule? schedule);
            return Task.FromResult(schedule);
        }

        public Task<IReadOnlyList<FeedingSchedule>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<FeedingSchedule>>(_feedingSchedules.Values.ToList());
        }

        public Task<IReadOnlyList<FeedingSchedule>> GetByTimeAsync(DateTime time)
        {
            List<FeedingSchedule> schedules = _feedingSchedules.Values
                .Where(schedule => schedule.FeedingTime.Date == time.Date)
                .ToList();
            return Task.FromResult<IReadOnlyList<FeedingSchedule>>(schedules);
        }

        public Task<IReadOnlyList<FeedingSchedule>> GetAvailableAsync(AnimalType animalType)
        {
            List<FeedingSchedule> availableSchedules = _feedingSchedules.Values
                .Where(s => !s.IsCompleted)
                .ToList();

            return Task.FromResult<IReadOnlyList<FeedingSchedule>>(availableSchedules);
        }

        public Task<IReadOnlyList<FeedingSchedule>> GetPendingFeedingsAsync(DateTime before)
        {
            List<FeedingSchedule> pendingSchedules = _feedingSchedules.Values
                .Where(s => !s.IsCompleted && s.FeedingTime <= before)
                .OrderBy(s => s.FeedingTime)
                .ToList();

            return Task.FromResult<IReadOnlyList<FeedingSchedule>>(pendingSchedules);
        }

        public Task AddAsync(FeedingSchedule schedule)
        {
            _feedingSchedules[schedule.Id] = schedule;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(FeedingSchedule schedule)
        {
            _feedingSchedules[schedule.Id] = schedule;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _feedingSchedules.Remove(id);
            return Task.CompletedTask;
        }
    }
}