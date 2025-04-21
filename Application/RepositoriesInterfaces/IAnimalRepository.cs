using Domain.Entities;
namespace Application.RepositoriesInterfaces
{
    public interface IAnimalRepository
    {
        Task<Animal?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Animal>> GetAllAsync();
        Task<IReadOnlyList<Animal>> GetByEnclosureIdAsync(Guid enclosureId);
        Task AddAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task DeleteAsync(Guid id);
    }
}