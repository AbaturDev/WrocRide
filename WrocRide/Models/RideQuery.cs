using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class RideQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public RideStatus? RideStatus { get; set; }
    }
}
