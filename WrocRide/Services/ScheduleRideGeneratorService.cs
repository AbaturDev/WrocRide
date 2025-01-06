namespace WrocRide.Services;

public class ScheduleRideGeneratorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScheduleRideGeneratorService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var scheduleService = scope.ServiceProvider.GetRequiredService<IScheduleService>();

            scheduleService.GenerateRidesFromSchedules();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}