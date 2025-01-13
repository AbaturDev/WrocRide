using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Driver
{
    public class DriverQuery
    {
        public DriverStatus? DriverStatus { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
