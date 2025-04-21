using Application.RepositoriesInterfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services
{
    public class ZooStatisticsService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IEnclosureRepository _enclosureRepository;
        private readonly IFeedingRecordRepository _feedingRecordRepository;

        public ZooStatisticsService(
            IAnimalRepository animalRepository,
            IEnclosureRepository enclosureRepository,
            IFeedingRecordRepository feedingRecordRepository)
        {
            _animalRepository = animalRepository;
            _enclosureRepository = enclosureRepository;
            _feedingRecordRepository = feedingRecordRepository;
        }

        // Общая статистика
        public async Task<ZooStatistics> GetZooStatisticsAsync()
        {
            IReadOnlyList<Animal> animals = await _animalRepository.GetAllAsync();
            IReadOnlyList<Enclosure> enclosures = await _enclosureRepository.GetAllAsync();
            
            return new ZooStatistics
            {
                TotalAnimals = animals.Count,
                TotalEnclosures = enclosures.Count,
                AnimalsWithoutEnclosure = animals.Count(a => !a.EnclosureId.HasValue),
                AvailableEnclosuresCount = enclosures.Count(e => e.AnimalIds.Count < e.Capacity),
                AnimalsByType = animals.GroupBy(a => a.Type)
                    .ToDictionary(g => g.Key, g => g.Count()),
                EnclosuresByType = enclosures.GroupBy(e => e.Type)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        // Статистика кормлений
        public async Task<FeedingStatistics> GetFeedingStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime start = startDate ?? DateTime.Today.AddDays(-30);
            DateTime end = endDate ?? DateTime.Today;
            
            IReadOnlyList<FeedingRecord> feedingRecords = await _feedingRecordRepository.GetByDateRangeAsync(start, end);
            
            return new FeedingStatistics
            {
                TotalFeedings = feedingRecords.Count,
                FeedingsByAnimal = feedingRecords.GroupBy(r => r.AnimalId)
                    .ToDictionary(g => g.Key, g => g.Count()),
                FeedingsByFoodType = feedingRecords.GroupBy(r => r.Food)
                    .ToDictionary(g => g.Key, g => g.Count()),
                FeedingsByDay = feedingRecords.GroupBy(r => r.CompletedAt.Date)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
        
        // Статистика загруженности вольеров
        public async Task<EnclosureUsageStatistics> GetEnclosureUsageStatisticsAsync()
        {
            IReadOnlyList<Enclosure> enclosures = await _enclosureRepository.GetAllAsync();
            IReadOnlyList<Animal> animals = await _animalRepository.GetAllAsync();
            
            List<EnclosureUsage> enclosureUsage = enclosures.Select(e => new EnclosureUsage
            {
                EnclosureId = e.Id,
                Capacity = e.Capacity,
                CurrentOccupancy = e.AnimalIds.Count,
                UsagePercentage = (double)e.AnimalIds.Count / e.Capacity * 100
            }).ToList();
            
            return new EnclosureUsageStatistics
            {
                EnclosureUsages = enclosureUsage,
                AverageOccupancy = enclosureUsage.Any() 
                    ? enclosureUsage.Average(e => e.UsagePercentage) 
                    : 0,
                FullyOccupiedCount = enclosureUsage.Count(e => e.CurrentOccupancy == e.Capacity),
                EmptyEnclosuresCount = enclosureUsage.Count(e => e.CurrentOccupancy == 0)
            };
        }
        
        // Получение списка животных, у которых нет назначенных кормлений
        public async Task<IReadOnlyList<Animal>> GetAnimalsWithoutFeedingsAsync()
        {
            IReadOnlyList<Animal> animals = await _animalRepository.GetAllAsync();
            IReadOnlyList<FeedingRecord> feedingRecords = await _feedingRecordRepository.GetAllAsync();
            
            HashSet<Guid> animalIdsWithFeedings = feedingRecords
                .Select(r => r.AnimalId)
                .Distinct()
                .ToHashSet();
            
            return animals
                .Where(a => !animalIdsWithFeedings.Contains(a.Id))
                .ToList();
        }
    }

    // DataTransferObjects-классы для возвращения статистики
    public class ZooStatistics
    {
        public int TotalAnimals { get; set; }
        public int TotalEnclosures { get; set; }
        public int AnimalsWithoutEnclosure { get; set; }
        public int AvailableEnclosuresCount { get; set; }
        public Dictionary<AnimalType, int> AnimalsByType { get; set; } = new();
        public Dictionary<EnclosureType, int> EnclosuresByType { get; set; } = new();
    }
    
    public class FeedingStatistics
    {
        public int TotalFeedings { get; set; }
        public Dictionary<Guid, int> FeedingsByAnimal { get; set; } = new();
        public Dictionary<FoodType, int> FeedingsByFoodType { get; set; } = new();
        public Dictionary<DateTime, int> FeedingsByDay { get; set; } = new();
    }
    
    public class EnclosureUsageStatistics
    {
        public List<EnclosureUsage> EnclosureUsages { get; set; } = new();
        public double AverageOccupancy { get; set; }
        public int FullyOccupiedCount { get; set; }
        public int EmptyEnclosuresCount { get; set; }
    }
    
    public class EnclosureUsage
    {
        public Guid EnclosureId { get; set; }
        public int Capacity { get; set; }
        public int CurrentOccupancy { get; set; }
        public double UsagePercentage { get; set; }
    }
}