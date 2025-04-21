using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Infrastructure.Repositories
{
    public class InMemoryEnclosureRepository : IEnclosureRepository
    {
        private readonly Dictionary<Guid, Enclosure> _enclosures = new();
        public Task<Enclosure?> GetByIdAsync(Guid id)
        {
            _enclosures.TryGetValue(id, out Enclosure? enclosure);
            return Task.FromResult(enclosure);
        }
        
        public Task<IReadOnlyList<Enclosure>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Enclosure>>(_enclosures.Values.ToList());
        }
        
        public Task<IReadOnlyList<Enclosure>> GetAvailableCapacityAsync(AnimalType animalType)
        {
            List<Enclosure> enclosuresWithCapacity = _enclosures.Values
                .Where(x => x.AnimalIds.Count < x.Capacity)
                .ToList();

            return Task.FromResult<IReadOnlyList<Enclosure>>(enclosuresWithCapacity);
        }
        
        public Task<IReadOnlyList<Enclosure>> GetAvailableForAnimalAsync(AnimalType animalType)
        {
            List<Enclosure> availableEnclosures = _enclosures.Values
                .Where(x => IsCompatibleType(x.Type, animalType) && x.AnimalIds.Count < x.Capacity)
                .ToList();

            return Task.FromResult<IReadOnlyList<Enclosure>>(availableEnclosures);
        }

        private bool IsCompatibleType(EnclosureType enclosureType, AnimalType animalType)
        {
            // Считаем типы совместимыми, если их числовые значения совпадают
            return (int)enclosureType == (int)animalType;
        }
        
        public Task AddAsync(Enclosure enclosure)
        {
            _enclosures[enclosure.Id] = enclosure;
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Enclosure enclosure)
        {
            _enclosures[enclosure.Id] = enclosure;
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(Guid id)
        {
            _enclosures.Remove(id);
            return Task.CompletedTask;
        }
    }
}