using Application.EventHandling;
using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services
{
    public class FeedingOrganizationService(
        IAnimalRepository animalRepository,
        IFeedingScheduleRepository feedingScheduleRepository,
        IFeedingRecordRepository feedingRecordRepository,
        IDomainEventDispatcher eventDispatcher)
    {
        // Получение расписания кормлений
        public async Task<IReadOnlyList<FeedingSchedule>> GetFeedingSchedulesAsync()
        {
            return await feedingScheduleRepository.GetAllAsync();
        }

        // Получение журнала кормлений
        public async Task<IReadOnlyList<FeedingRecord>> GetFeedingRecordsAsync()
        {
            return await feedingRecordRepository.GetAllAsync();
        }

        // Добавление нового расписания кормления
        public async Task AddFeedingScheduleAsync(Guid animalId, DateTime feedingTime, FoodType foodType, bool isRecurring = false)
        {
            Animal? animal = await animalRepository.GetByIdAsync(animalId);

            if (animal == null)
            {
                throw new ArgumentException("Животное не найдено", nameof(animalId));
            }

            if (animal.Food != foodType)
            {
                throw new ArgumentException("Тип корма не соответствует потребностям животного", nameof(foodType));
            }

            FeedingSchedule schedule = FeedingSchedule.Create(animalId, feedingTime, foodType, isRecurring);
            await feedingScheduleRepository.AddAsync(schedule);
        }

        // Проверка и выполнение запланированных кормлений
        public async Task CheckScheduledFeedings()
        {
            DateTime now = DateTime.Now;
            IReadOnlyList<FeedingSchedule> schedules = await feedingScheduleRepository.GetPendingFeedingsAsync(now);

            foreach (FeedingSchedule schedule in schedules)
            {
                if (schedule.FeedingTime <= now && !schedule.IsCompleted)
                {
                    try
                    {
                        // Генерируем событие
                        schedule.RaiseFeedingTimeEvent();
                
                        // Отправляем события через диспетчер (обработчик сделает всё остальное)
                        await eventDispatcher.DispatchEventsAsync(schedule);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при обработке расписания {schedule.Id}: {ex.Message}");
                    }
                }
            }
        }
    }
}