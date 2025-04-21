using Domain.ValueObjects;
namespace ZooWebApi.DataTransferObjects
{
    public class CreateEnclosureDto
    {
        public EnclosureType Type { get; set; }
        public int Size { get; set; }
        public int Capacity { get; set; }
    }
}