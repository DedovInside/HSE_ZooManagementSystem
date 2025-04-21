using Application.EventHandling;
using Application.RepositoriesInterfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AnimalTransferService(
        IAnimalRepository animalRepository,
        IEnclosureRepository feedingScheduleRepository,
        ITransferRecordRepository transferRecordRepository,
        IDomainEventDispatcher domainEventDispatcher)
    {
        public async Task<IReadOnlyList<TransferRecord>> GetTransfersAsync()
        {
            return await transferRecordRepository.GetAllAsync();
        }
        public async Task TransferAnimalAsync(Guid animalId, Guid newEnclosureId)
        {
            Animal? animal = await animalRepository.GetByIdAsync(animalId);
            if (animal == null)
            {
                throw new ArgumentException($"Животное с ID {animalId} не найдено");
            }

            Enclosure? enclosure = await feedingScheduleRepository.GetByIdAsync(newEnclosureId);
            if (enclosure == null)
            {
                throw new ArgumentException($"Вольер с ID {newEnclosureId} не найден");
            }

            // Если животное уже в вольере, удаляем его оттуда
            if (animal.EnclosureId.HasValue)
            {
                Enclosure? currentEnclosure = await feedingScheduleRepository.GetByIdAsync(animal.EnclosureId.Value);
                currentEnclosure?.RemoveAnimal(animalId);
                if (currentEnclosure != null)
                {
                    await feedingScheduleRepository.UpdateAsync(currentEnclosure);
                }
            }

            // Перемещаем животное в новый вольер
            animal.MoveToEnclosure(newEnclosureId);
            enclosure.AddAnimal(animal);

            // Сохраняем изменения
            await animalRepository.UpdateAsync(animal);
            await feedingScheduleRepository.UpdateAsync(enclosure);

            // Публикуем события
            await domainEventDispatcher.DispatchEventsAsync(animal);
            animal.ClearDomainEvents();
        }
    }
}