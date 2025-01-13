namespace WrocRide.API.Entities;

public class ScheduleDay
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public int DayOfWeekId { get; set; }
    public virtual Schedule Schedule { get; set; }
    public virtual DayOfWeek DayOfWeek { get; set; }
}