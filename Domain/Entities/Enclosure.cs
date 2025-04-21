using Domain.ValueObjects;
namespace Domain.Entities
{
    public class Enclosure
    {
        public Guid Id { get; private set; }
        public EnclosureType Type { get; private set; }
        public int Size { get; private set; }
        public int Capacity { get; private set; }
        private readonly List<Guid> _animalIds = new();
        
        public IReadOnlyList<Guid> AnimalIds => _animalIds;
        
        private Enclosure() { }
        
        public static Enclosure Create(EnclosureType enclosureType, int size, int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.");
            }

            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than zero.");
            }

            return new Enclosure
            {
                Id = Guid.NewGuid(),
                Type = enclosureType,
                Size = size,
                Capacity = capacity
            };
        }
        
        public void AddAnimal(Animal animal)
        {
            if (!IsCompatibleAnimalType(animal.Type))
            {
                throw new InvalidOperationException($"Cannot place {animal.Type} in {Type} enclosure.");
            }
            
            if (_animalIds.Count >= Capacity)
            {
                throw new InvalidOperationException("Enclosure is full.");
            }

            _animalIds.Add(animal.Id);
        }
        
        
        private bool IsCompatibleAnimalType(AnimalType animalType)
        {
            // Основной принцип: типы животного и вольера должны совпадать
            return (int)animalType == (int)Type;
        }

        public void RemoveAnimal(Guid animalId)
        {
            _animalIds.Remove(animalId);
        }

        public void Clean()
        {
            // Логика уборки
        }
        
    }
}