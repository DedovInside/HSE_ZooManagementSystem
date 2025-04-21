using Domain.ValueObjects;
namespace Domain.Entities
{
    public class TransferRecord
    {
        public Guid Id { get; private set; }
        public Guid AnimalId { get; private set; }
        public string AnimalName { get; private set; }
        public AnimalSpecies AnimalSpecies { get; private set; }
        public AnimalType AnimalType { get; private set; }
        public Guid? FromEnclosureId { get; private set; }
        public Guid ToEnclosureId { get; private set; }
        public DateTime TransferTime { get; private set; }

        private TransferRecord(Guid id, Guid animalId, string animalName, AnimalSpecies species, AnimalType type, 
            Guid? fromEnclosureId, Guid toEnclosureId, DateTime transferTime)
        {
            Id = id;
            AnimalId = animalId;
            AnimalName = animalName;
            AnimalSpecies = species;
            AnimalType = type;
            FromEnclosureId = fromEnclosureId;
            ToEnclosureId = toEnclosureId;
            TransferTime = transferTime;
        }

        public static TransferRecord Create(Animal animal, Guid? fromEnclosureId, Guid toEnclosureId)
        {
            return new TransferRecord(
                Guid.NewGuid(),
                animal.Id,
                animal.Name,
                animal.Species,
                animal.Type,
                fromEnclosureId,
                toEnclosureId,
                DateTime.Now
            );
        }
    }
}