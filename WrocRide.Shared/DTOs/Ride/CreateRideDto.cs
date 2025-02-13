namespace WrocRide.Shared.DTOs.Ride;
public record CreateRideDto
{
    public string PickUpLocation { get; set; }
    public string Destination { get; set; }
    public int DriverId { get; set; }
}
