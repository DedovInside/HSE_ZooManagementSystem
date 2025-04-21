using Domain.Events;
using Domain.ValueObjects;
namespace Domain.Entities
{
    public class Animal : IHasDomainEvents
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        
        public AnimalSpecies Species { get; private set; }
        public AnimalType Type { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }
        public FoodType Food { get; private set; }
        public HealthStatus HealthStatus { get; private set; }
        public Guid? EnclosureId { get; private set; }
        
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        private Animal(Guid id, string name, AnimalSpecies species, AnimalType type, DateTime birthDate,
            Gender gender, FoodType food, HealthStatus healthStatus, Guid? enclosureId)
        {
            Id = id;
            Name = name;
            Species = species;
            Type = type;
            BirthDate = birthDate;
            Gender = gender;
            Food = food;
            HealthStatus = healthStatus;
            EnclosureId = enclosureId;
        }

        public static Animal Create(string name, AnimalSpecies species, AnimalType type,
            DateTime birthDate, Gender gender, FoodType food)
        {
            return new Animal(
                Guid.NewGuid(),
                name,
                species,
                type,
                birthDate,
                gender,
                food,
                HealthStatus.Healthy,
                null);
        }
        
        
        public void Feed(FoodType food)
        {
            if (food != Food)
            {
                throw new InvalidOperationException("Animal cannot eat this type of food.");
            }
            
            // Логика кормления животного
            Console.WriteLine($"{Name} {Id} is being fed with {food}.");
        }


        public void Treat()
        {
            HealthStatus = HealthStatus.Healthy;
        }
        
        public void MoveToEnclosure(Guid newEnclosureId)
        {
            
            Guid? oldEnclosure = EnclosureId;
            if (oldEnclosure == newEnclosureId)
            {
                throw new InvalidOperationException("Animal is already in this enclosure.");
            }
            EnclosureId = newEnclosureId;
            
            // Добавляем событие о перемещении животного
            _domainEvents.Add(new AnimalMovedEvent(Id, oldEnclosure, newEnclosureId));
            
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    
    }
}