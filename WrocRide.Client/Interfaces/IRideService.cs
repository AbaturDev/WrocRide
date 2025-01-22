using WrocRide.Shared.DTOs.Ride;

namespace WrocRide.Client.Interfaces
{
    public interface IRideService
    {
        Task CreateRide(CreateRideDto dto);
        Task CreateRideReservation(CreateRideReservationDto dto);
        //Task<RideDto> GetById(int id);
        Task UpdateRideStatus(int id, UpdateRideStatusDto dto);
        Task DriverDecision(int id, UpdateRideStatusDto dto);
        Task CancelRide(int id);
        Task EndRide(int id);
    }
}
