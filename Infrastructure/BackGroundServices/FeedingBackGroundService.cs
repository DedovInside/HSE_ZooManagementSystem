using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class FeedingBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = scopeFactory.CreateScope())
                {
                    FeedingOrganizationService feedingService = scope.ServiceProvider.GetRequiredService<FeedingOrganizationService>();
                    await feedingService.CheckScheduledFeedings();
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}