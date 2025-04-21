using Application.RepositoriesInterfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InMemoryTransferRecordRepository : ITransferRecordRepository
    {
        private readonly Dictionary<Guid, TransferRecord> _records = new();

        public Task<IReadOnlyList<TransferRecord>> GetAllAsync()
        {
            return Task.FromResult<IReadOnlyList<TransferRecord>>(_records.Values.ToList());
        }

        public Task<TransferRecord?> GetByIdAsync(Guid id)
        {
            _records.TryGetValue(id, out TransferRecord? record);
            return Task.FromResult(record);
        }

        public Task<IReadOnlyList<TransferRecord>> GetByAnimalIdAsync(Guid animalId)
        {
            List<TransferRecord> filteredRecords = _records.Values
                .Where(record => record.AnimalId == animalId)
                .ToList();
            
            return Task.FromResult<IReadOnlyList<TransferRecord>>(filteredRecords);
        }

        public Task AddAsync(TransferRecord transferRecord)
        {
            _records[transferRecord.Id] = transferRecord;
            return Task.CompletedTask;
        }
    }
}