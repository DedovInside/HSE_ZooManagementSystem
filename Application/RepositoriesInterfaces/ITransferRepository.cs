using Domain.Entities;
namespace Application.RepositoriesInterfaces
{
    public interface ITransferRecordRepository
    {
        Task<IReadOnlyList<TransferRecord>> GetAllAsync();
        Task<TransferRecord?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<TransferRecord>> GetByAnimalIdAsync(Guid animalId);
        Task AddAsync(TransferRecord transferRecord);
    }
}