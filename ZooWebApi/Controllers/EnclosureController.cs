using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using ZooWebApi.DataTransferObjects;

namespace ZooWebApi.Controllers
{
    [ApiController]
    [Route("api/enclosures")]
    public class EnclosureController(EnclosureService enclosureService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetAllEnclosures()
        {
            IReadOnlyList<Enclosure> enclosures = await enclosureService.GetAllEnclosuresAsync();
            return Ok(enclosures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosureById(Guid id)
        {
            Enclosure? enclosure = await enclosureService.GetEnclosureByIdAsync(id);
            
            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure);
        }

        [HttpGet("available/{animalType}")]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetAvailableEnclosures(AnimalType animalType)
        {
            IReadOnlyList<Enclosure> enclosures = await enclosureService.GetAvailableEnclosuresForAnimalAsync(animalType);
            return Ok(enclosures);
        }

        [HttpPost]
        public async Task<ActionResult<Enclosure>> CreateEnclosure([FromBody] CreateEnclosureDto request)
        {
            Enclosure enclosure = await enclosureService.CreateEnclosureAsync(
                request.Type,
                request.Size,
                request.Capacity);
                
            return CreatedAtAction(nameof(GetEnclosureById), new { id = enclosure.Id }, enclosure);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEnclosure(Guid id)
        {
            Enclosure? enclosure = await enclosureService.GetEnclosureByIdAsync(id);
            
            if (enclosure == null)
            {
                return NotFound();
            }

            await enclosureService.DeleteEnclosureAsync(id);
            return NoContent();
        }
    }
}