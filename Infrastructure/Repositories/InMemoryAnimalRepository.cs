using Application.RepositoriesInterfaces;
using Domain.Entities;
namespace Infrastructure.Repositories
{
    public class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly Dictionary<Guid, Animal> _animals = new();
        public Task<Animal?> GetByIdAsync(Guid id)
        {
            _animals.TryGetValue(id, out Animal? animal);
            return Task.FromResult(animal);
        }
        
        public Task<IReadOnlyList<Animal>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Animal>>(_animals.Values.ToList());
        }
        
        public Task<IReadOnlyList<Animal>> GetByEnclosureIdAsync(Guid enclosureId)
        {
            List<Animal> result = _animals.Values.Where(x => x.EnclosureId == enclosureId).ToList();
            return Task.FromResult<IReadOnlyList<Animal>>(result);
        }
        
        public Task AddAsync(Animal animal)
        {
            _animals[animal.Id] = animal;
            return Task.CompletedTask;
        }
        
        public Task UpdateAsync(Animal animal)
        {
            _animals[animal.Id] = animal;
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(Guid id)
        {
            _animals.Remove(id);
            return Task.CompletedTask;
        }
    }
}