namespace WrocRide.Shared.DTOs.Schedule;

public sealed record CreateScheduleDto
{
    public string PickUpLocation { get; set; }
    public string Destination { get; set; }
    public TimeSpan StartTime { get; set; }
    public List<int> DayOfWeekIds { get; set; }
    public decimal BudgetPerRide { get; set; }
}