using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ZooWebApi.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController(ZooStatisticsService statisticsService) : ControllerBase
    {
        [HttpGet("zoo")]
        public async Task<ActionResult> GetZooStatistics()
        {
            ZooStatistics statistics = await statisticsService.GetZooStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("feedings")]
        public async Task<ActionResult> GetFeedingStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            FeedingStatistics statistics = await statisticsService.GetFeedingStatisticsAsync(startDate, endDate);
            return Ok(statistics);
        }

        [HttpGet("enclosure-usage")]
        public async Task<ActionResult> GetEnclosureUsageStatistics()
        {
            EnclosureUsageStatistics statistics = await statisticsService.GetEnclosureUsageStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("animals-without-feedings")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimalsWithoutFeedings()
        {
            IReadOnlyList<Animal> animals = await statisticsService.GetAnimalsWithoutFeedingsAsync();
            return Ok(animals);
        }
    }
}