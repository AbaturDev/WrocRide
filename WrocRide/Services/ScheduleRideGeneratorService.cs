namespace WrocRide.Services;

public class ScheduleRideGeneratorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScheduleRideGeneratorService> _logger;

    public ScheduleRideGeneratorService(IServiceScopeFactory scopeFactory, ILogger<ScheduleRideGeneratorService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Schedule ride generator service running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Schedule ride generator service working");
            using var scope = _scopeFactory.CreateScope();
            var scheduleService = scope.ServiceProvider.GetRequiredService<IScheduleService>();

            scheduleService.GenerateRidesFromSchedules();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}