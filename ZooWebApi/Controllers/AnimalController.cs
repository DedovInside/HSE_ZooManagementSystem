using Application.Services;
using Domain.Entities;
using ZooWebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace ZooWebApi.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController(AnimalService animalService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAllAnimals()
        {
            IReadOnlyList<Animal> animals = await animalService.GetAllAnimalsAsync();
            return Ok(animals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimalById(Guid id)
        {
            Animal? animal = await animalService.GetAnimalByIdAsync(id);
            
            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        [HttpGet("enclosure/{enclosureId}")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetByEnclosure(Guid enclosureId)
        {
            IReadOnlyList<Animal> animals = await animalService.GetAnimalsByEnclosureIdAsync(enclosureId);
            return Ok(animals);
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> CreateAnimal([FromBody] CreateAnimalDto request)
        {
            Animal animal = await animalService.CreateAnimalAsync(
                request.Name,
                request.Species,
                request.Type,
                request.BirthDate,
                request.Gender,
                request.Food);
                
            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, animal);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(Guid id)
        {
            Animal? animal = await animalService.GetAnimalByIdAsync(id);
            
            if (animal == null)
            {
                return NotFound();
            }

            await animalService.DeleteAnimalAsync(id);
            return NoContent();
        }
    }
    
}