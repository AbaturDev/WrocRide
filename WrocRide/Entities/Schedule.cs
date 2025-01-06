namespace WrocRide.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string PickUpLocation { get; set; }
    public string Destination { get; set; }
    public TimeSpan StartTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Distance { get; set; }
    public decimal BudgetPerRide { get; set; }
    
    public virtual ICollection<ScheduleDay> ScheduleDays { get; set; }
    public virtual Client Client { get; set; }
}