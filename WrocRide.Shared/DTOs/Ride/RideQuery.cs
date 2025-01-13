using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Ride
{
    public class RideQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public RideStatus? RideStatus { get; set; }
    }
}
