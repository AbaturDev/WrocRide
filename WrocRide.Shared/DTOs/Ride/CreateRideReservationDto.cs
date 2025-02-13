namespace WrocRide.Shared.DTOs.Ride;

public sealed record CreateRideReservationDto : CreateRideDto
{
    public DateTime StartDate { get; set; }
}