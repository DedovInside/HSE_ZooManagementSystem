using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services
{
    public class EnclosureService
    {
        private readonly IEnclosureRepository _enclosureRepository;
        
        public EnclosureService(IEnclosureRepository enclosureRepository)
        {
            _enclosureRepository = enclosureRepository;
        }
        
        public async Task<Enclosure?> GetEnclosureByIdAsync(Guid id)
        {
            return await _enclosureRepository.GetByIdAsync(id);
        }
        
        public async Task<IReadOnlyList<Enclosure>> GetAllEnclosuresAsync()
        {
            return await _enclosureRepository.GetAllAsync();
        }
        
        public async Task<IReadOnlyList<Enclosure>> GetAvailableEnclosuresForAnimalAsync(AnimalType animalType)
        {
            return await _enclosureRepository.GetAvailableForAnimalAsync(animalType);
        }
        
        public async Task<Enclosure> CreateEnclosureAsync(EnclosureType type, int size, int capacity)
        {
            Enclosure enclosure = Enclosure.Create(type, size, capacity);
            await _enclosureRepository.AddAsync(enclosure);
            return enclosure;
        }
        
        public async Task UpdateEnclosureAsync(Enclosure enclosure)
        {
            await _enclosureRepository.UpdateAsync(enclosure);
        }
        
        public async Task DeleteEnclosureAsync(Guid id)
        {
            await _enclosureRepository.DeleteAsync(id);
        }
    }
}