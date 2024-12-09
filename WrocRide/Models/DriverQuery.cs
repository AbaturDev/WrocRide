using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class DriverQuery
    {
        public DriverStatus? DriverStatus { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
