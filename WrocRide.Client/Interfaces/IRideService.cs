using WrocRide.Shared;
using WrocRide.Shared.DTOs.Ride;
using WrocRide.Shared.Enums;

namespace WrocRide.Client.Interfaces
{
    public interface IRideService
    {
        Task CreateRide(CreateRideDto dto);
        Task CreateRideReservation(CreateRideReservationDto dto);
        Task<PagedList<RideDto>> GetAll(int pageSize, int pageNumber, RideStatus? rideStatus);
        Task<RideDeatailsDto> GetById(int id);
        Task UpdateRideStatus(int id, UpdateRideStatusDto dto);
        Task DriverDecision(int id, UpdateRideStatusDto dto);
        Task CancelRide(int id);
        Task EndRide(int id);
    }
}
