using Domain.Events;
using Application.RepositoriesInterfaces;
using Domain.Entities;
namespace Application.EventHandling
{
    public class AnimalMovedEventHandler(
        IAnimalRepository animalRepository,
        ITransferRecordRepository transferRecordRepository) : IDomainEventHandler<AnimalMovedEvent>
    {
        public async Task HandleAsync(AnimalMovedEvent domainEvent)
        {
            Animal? animal = await animalRepository.GetByIdAsync(domainEvent.AnimalId);
            if (animal == null)
            {
                Console.WriteLine($"Не удалось найти животное с ID {domainEvent.AnimalId} при обработке события перемещения");
                return;
            }

            try
            {
                // Создаем запись в журнале перемещений
                TransferRecord record = TransferRecord.Create(
                    animal, 
                    domainEvent.FromEnclosureId, 
                    domainEvent.ToEnclosureId
                );
                
                await transferRecordRepository.AddAsync(record);
                
                Console.WriteLine($"Животное {animal.Name} ({animal.Species}) успешно перемещено " +
                                  $"{(domainEvent.FromEnclosureId.HasValue ? $"из вольера {domainEvent.FromEnclosureId}" : "")} " +
                                  $"в вольер {domainEvent.ToEnclosureId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке события перемещения: {ex.Message}");
            }
        }
    }
}