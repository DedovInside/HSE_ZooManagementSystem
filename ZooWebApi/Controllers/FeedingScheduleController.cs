using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ZooWebApi.DataTransferObjects;

namespace ZooWebApi.Controllers
{
    [ApiController]
    [Route("api/feedings")]
    public class FeedingScheduleController(FeedingOrganizationService feedingService) : ControllerBase
    {
        [HttpGet("schedules")]
        public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetFeedingSchedules()
        {
            IReadOnlyList<FeedingSchedule> schedules = await feedingService.GetFeedingSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("records")]
        public async Task<ActionResult<IEnumerable<FeedingRecord>>> GetFeedingRecords()
        {
            IReadOnlyList<FeedingRecord> records = await feedingService.GetFeedingRecordsAsync();
            return Ok(records);
        }

        [HttpPost("schedule")]
        public async Task<ActionResult> AddFeedingSchedule([FromBody] AddFeedingScheduleDto request)
        {
            try
            {
                await feedingService.AddFeedingScheduleAsync(
                    request.AnimalId,
                    request.FeedingTime,
                    request.FoodType,
                    request.IsRecurring);
                    
                return Ok("Кормление добавлено в расписание");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}