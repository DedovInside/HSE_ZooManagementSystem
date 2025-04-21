using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.Events;

namespace Application.EventHandling
{
    public class FeedingTimeEventHandler(
        IAnimalRepository animalRepository,
        IFeedingScheduleRepository scheduleRepository,
        IFeedingRecordRepository recordRepository)
        : IDomainEventHandler<FeedingTimeEvent>
    {
        public async Task HandleAsync(FeedingTimeEvent domainEvent)
        {
            // Получаем нужные данные
            FeedingSchedule? schedule = await scheduleRepository.GetByIdAsync(domainEvent.ScheduleId);
            Animal? animal = await animalRepository.GetByIdAsync(domainEvent.AnimalId);

            if (schedule == null || animal == null)
            {
                Console.WriteLine("Не удалось обработать событие кормления: расписание или животное не найдено");
                return;
            }

            try
            {
                // Кормление животного
                animal.Feed(schedule.Food);
                await animalRepository.UpdateAsync(animal);

                // Создание записи в журнале
                FeedingRecord record = FeedingRecord.Create(schedule);
                await recordRepository.AddAsync(record);

                // Обработка повторяющихся расписаний
                schedule.MarkAsCompleted();
                await scheduleRepository.UpdateAsync(schedule);

                Console.WriteLine($"Животное {animal.Id} успешно покормлено в {DateTime.Now} (обработано событием)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке события кормления: {ex.Message}");
            }
        }
    }
}