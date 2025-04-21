using Domain.ValueObjects;
namespace ZooWebApi.DataTransferObjects
{
    public class CreateAnimalDto
    {
        public string Name { get; set; } = string.Empty;
        public AnimalSpecies Species { get; set; }
        public AnimalType Type { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public FoodType Food { get; set; }
    }
}