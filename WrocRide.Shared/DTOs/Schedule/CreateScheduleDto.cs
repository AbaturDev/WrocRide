namespace WrocRide.Shared.DTOs.Schedule;

public class CreateScheduleDto
{
    public string PickUpLocation { get; set; }
    public string Destination { get; set; }
    public TimeSpan StartTime { get; set; }
    public List<int> DayOfWeekIds { get; set; }
    public decimal BudgetPerRide { get; set; }
}