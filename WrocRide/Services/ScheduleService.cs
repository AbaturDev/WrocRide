using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services;

public interface IScheduleService
{
    int CreateSchedule(CreateScheduleDto dto);
    void DeleteSchedule(int id);
    ScheduleDto GetSchedule(int id);
    void GenerateRidesFromSchedules();
}

public class ScheduleService : IScheduleService
{
    private readonly WrocRideDbContext _dbContext;
    private readonly IUserContextService _userContext;
    
    public ScheduleService(WrocRideDbContext dbContext, IUserContextService userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public int CreateSchedule(CreateScheduleDto dto)
    {
        var userId = _userContext.GetUserId;

        var client = _dbContext.Clients.FirstOrDefault(c => c.UserId == userId);
        if (client == null)
        {
            throw new BadRequestException("User is not a client");
        }

        var conflictingSchedules = _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .Any(s => s.ClientId == client.Id 
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

        _dbContext.Schedules.Add(schedule);
        _dbContext.SaveChanges();

        foreach (var dayOfWeekId in dto.DayOfWeekIds)
        {
            var scheduleDay = new ScheduleDay()
            {
                DayOfWeekId = dayOfWeekId,
                ScheduleId = schedule.Id
            };
            _dbContext.ScheduleDays.Add(scheduleDay);
        }
        _dbContext.SaveChanges();
        
        return schedule.Id;
    }
    
    public void DeleteSchedule(int id)
    {
        var schedule = GetCurrentClientSchedule(id);

        _dbContext.Schedules.Remove(schedule);
        _dbContext.SaveChanges();
    }

    public ScheduleDto GetSchedule(int id)
    {
        var schedule = GetCurrentClientSchedule(id);

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

    private Schedule GetCurrentClientSchedule(int id)
    {
        var userId = _userContext.GetUserId;

        var client = _dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

        if (client == null)
        {
            throw new BadRequestException("User is not a client");
        }
        
        var schedule = _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .ThenInclude(s => s.DayOfWeek)
            .FirstOrDefault(s => s.Id == id);

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

    public void GenerateRidesFromSchedules()
    {
        var today = (int)DateTime.Today.DayOfWeek;

        var schedules = _dbContext.Schedules
            .Include(s => s.ScheduleDays)
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Where(s => s.ScheduleDays.Any(sd => sd.DayOfWeekId == today))
            .ToList();

        foreach (var schedule in schedules)
        {
            var rideAlreadyExist = _dbContext.Rides
                .Any(r => r.StartDate.Date == DateTime.Today.Date
                          && r.StartDate.TimeOfDay == schedule.StartTime
                          && r.ClientId == schedule.ClientId
                          && r.RideStatus != RideStatus.Canceled);

            if (rideAlreadyExist)
            {
                //signalR
                continue;
            }
            
            if (schedule.Client.User.Balance < schedule.BudgetPerRide)
            {
                //signalR
                continue;
            }
            
            var driver = _dbContext.Drivers
                .Where(d => d.DriverStatus == DriverStatus.Available
                                     && d.Pricing * schedule.Distance <= schedule.BudgetPerRide)
                .OrderBy(d => d.Rating)
                .FirstOrDefault();
            
            if (driver == null)
            {
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

            _dbContext.Rides.Add(ride);
            _dbContext.SaveChanges();
        }
    }
}