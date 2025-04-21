using Domain.Entities;
using Domain.ValueObjects;
namespace Application.RepositoriesInterfaces
{
    public interface IEnclosureRepository
    {
        Task<Enclosure?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Enclosure>> GetAllAsync();
        Task<IReadOnlyList<Enclosure>> GetAvailableCapacityAsync(AnimalType animalType);
        Task<IReadOnlyList<Enclosure>> GetAvailableForAnimalAsync(AnimalType animalType);
        Task AddAsync(Enclosure enclosure);
        Task UpdateAsync(Enclosure enclosure);
        Task DeleteAsync(Guid id);
    }
}