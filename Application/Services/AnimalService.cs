using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services
{
    public class AnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        
        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }
        
        public async Task<Animal?> GetAnimalByIdAsync(Guid id)
        {
            return await _animalRepository.GetByIdAsync(id);
        }
        
        public async Task<IReadOnlyList<Animal>> GetAllAnimalsAsync()
        {
            return await _animalRepository.GetAllAsync();
        }
        
        public async Task<IReadOnlyList<Animal>> GetAnimalsByEnclosureIdAsync(Guid enclosureId)
        {
            return await _animalRepository.GetByEnclosureIdAsync(enclosureId);
        }
        
        public async Task<Animal> CreateAnimalAsync(string name, AnimalSpecies species, AnimalType type, DateTime birthdate, Gender gender, FoodType food)
        {
            Animal animal = Animal.Create(name, species, type, birthdate, gender, food);
            await _animalRepository.AddAsync(animal);
            return animal;
        }
        
        public async Task UpdateAnimalAsync(Animal animal)
        {
            await _animalRepository.UpdateAsync(animal);
        }
        
        public async Task DeleteAnimalAsync(Guid id)
        {
            await _animalRepository.DeleteAsync(id);
        }
    }
}