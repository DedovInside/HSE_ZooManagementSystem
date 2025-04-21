using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ZooWebApi.DataTransferObjects;

namespace ZooWebApi.Controllers
{
    [ApiController]
    [Route("api/animal-transfers")]
    public class AnimalTransferController(AnimalTransferService transferService) : ControllerBase
    {
        [HttpPost("transfer")]
        public async Task<ActionResult> TransferAnimal([FromBody] TransferAnimalDto request)
        {
            try
            {
                await transferService.TransferAnimalAsync(request.AnimalId, request.NewEnclosureId);
                return Ok("Животное успешно перемещено");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        
        [HttpGet("records")]
        public async Task<ActionResult<IEnumerable<TransferRecord>>> GetFeedingRecords()
        {
            IReadOnlyList<TransferRecord> records = await transferService.GetTransfersAsync();
            return Ok(records);
        }
    }
}