namespace WrocRide.API.Services;

public interface IScheduleService
{
    Task<int> CreateSchedule(CreateScheduleDto dto);
    Task DeleteSchedule(int id);
    Task<ScheduleDto> GetSchedule(int id);
    Task GenerateRidesFromSchedules();
}

public class ScheduleService : IScheduleService
{
    private readonly WrocRideDbContext _dbContext;
    private readonly IUserContextService _userContext;
    private readonly ILogger<ScheduleService> _logger;

    public ScheduleService(WrocRideDbContext dbContext, IUserContextService userContext, ILogger<ScheduleService> logger)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<int> CreateSchedule(CreateScheduleDto dto)
    {
        var userId = _userContext.GetUserId;

        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
        if (client == null)
        {
            throw new BadRequestException("User is not a client");
        }

        var conflictingSchedules = await _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .AnyAsync(s => s.ClientId == client.Id 
                      && s.ScheduleDays.Any(sd => dto.DayOfWeekIds.Contains(sd.DayOfWeekId) 
                      && s.StartTime == dto.StartTime));

        if (conflictingSchedules)
        {
            throw new BadRequestException("Client already has a schedule at one of the terms");
        }
        
        var schedule = new Schedule()
        {
            ClientId = client.Id,
            PickUpLocation = dto.PickUpLocation,
            Destination = dto.Destination,
            StartTime = dto.StartTime,
            CreatedAt = DateTime.Now,
            BudgetPerRide = dto.BudgetPerRide,
            Distance = 1                            //will be replaced by google api
        };

        await _dbContext.Schedules.AddAsync(schedule);
        await _dbContext.SaveChangesAsync();

        foreach (var dayOfWeekId in dto.DayOfWeekIds)
        {
            var scheduleDay = new ScheduleDay()
            {
                DayOfWeekId = dayOfWeekId,
                ScheduleId = schedule.Id
            };
            await _dbContext.ScheduleDays.AddAsync(scheduleDay);
        }
        await _dbContext.SaveChangesAsync();
        
        return schedule.Id;
    }
    
    public async Task DeleteSchedule(int id)
    {
        var schedule = await GetCurrentClientSchedule(id);

        _dbContext.Schedules.Remove(schedule);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ScheduleDto> GetSchedule(int id)
    {
        var schedule = await GetCurrentClientSchedule(id);

        var result = new ScheduleDto()
        {
            Id = schedule.Id,
            ClientId = schedule.ClientId,
            PickUpLocation = schedule.PickUpLocation,
            Destination = schedule.Destination,
            Distance = schedule.Distance,
            StartTime = schedule.StartTime,
            CreatedAt = schedule.CreatedAt,
            BudgetPerRide = schedule.BudgetPerRide,
            DaysOfWeek = schedule.ScheduleDays.Select(s => s.DayOfWeek.Day).ToList()
        };

        return result;
    }

    private async Task<Schedule> GetCurrentClientSchedule(int id)
    {
        var userId = _userContext.GetUserId;

        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null)
        {
            throw new BadRequestException("User is not a client");
        }
        
        var schedule = await _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .ThenInclude(s => s.DayOfWeek)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (schedule == null)
        {
            throw new NotFoundException("Schedule not found");
        }

        if (schedule.ClientId != client.Id)
        {
            throw new ForbidException("Client is not schedule creator");
        }

        return schedule;
    }

    public async Task GenerateRidesFromSchedules()
    {
        var today = (int)DateTime.Today.DayOfWeek;

        var schedules = await _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Where(s => s.ScheduleDays.Any(sd => sd.DayOfWeekId == today))
            .ToListAsync();

        foreach (var schedule in schedules)
        {
            _logger.LogInformation($"Proccessing schedule ID {schedule.Id}");

            try
            {
                var rideAlreadyExist = await _dbContext.Rides
                    .AnyAsync(r => r.StartDate.Date == DateTime.Today.Date
                              && r.StartDate.TimeOfDay == schedule.StartTime
                              && r.ClientId == schedule.ClientId
                              && r.RideStatus != RideStatus.Canceled);

                if (rideAlreadyExist)
                {
                    _logger.LogWarning($"Todays ride already exist for schedule ID {schedule.Id}");
                    //signalR
                    continue;
                }

                if (schedule.Client.User.Balance < schedule.BudgetPerRide)
                {
                    _logger.LogWarning($"Client Id {schedule.ClientId} does not have enoguh money for schedule Id {schedule.Id} ride creation");
                    //signalR
                    continue;
                }

                var driver = await _dbContext.Drivers
                    .Where(d => d.DriverStatus == DriverStatus.Available
                                         && d.Pricing * schedule.Distance <= schedule.BudgetPerRide)
                    .OrderBy(d => d.Rating)
                    .FirstOrDefaultAsync();

                if (driver == null)
                {
                    _logger.LogWarning($"No avaliable driver found for schedule Id {schedule.Id}");
                    //signalR
                    continue;
                }

                var ride = new Ride()
                {
                    StartDate = DateTime.Today + schedule.StartTime,
                    PickUpLocation = schedule.PickUpLocation,
                    Destination = schedule.Destination,
                    DriverId = driver.Id,
                    ClientId = schedule.ClientId,
                    RideStatus = RideStatus.Pending,
                    Distance = schedule.Distance,
                    Coast = driver.Pricing * schedule.Distance
                };

                await _dbContext.Rides.AddAsync(ride);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Succesfully created todays ride id {ride.Id} for schedule Id {schedule.Id}");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}