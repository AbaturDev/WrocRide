namespace WrocRide.Models;

public record ScheduleDto
{
    public required int Id { get; set; }
    public required int ClientId { get; set; }
    public required string PickUpLocation { get; set; }
    public required string Destination { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required List<String> DaysOfWeek { get; set; }
}