using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;

namespace WrocRide.Services;

public interface IScheduleService
{
    int CreateSchedule(CreateScheduleDto dto);
    void DeleteSchedule(int id);
    ScheduleDto GetSchedule(int id);
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

        var schedule = new Schedule()
        {
            ClientId = client.Id,
            PickUpLocation = dto.PickUpLocation,
            Destination = dto.Destination,
            StartTime = dto.StartTime,
            CreatedAt = DateTime.Now
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
            StartTime = schedule.StartTime,
            CreatedAt = schedule.CreatedAt,
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
    
}