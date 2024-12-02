using WrocRide.Models.Enums;

namespace WrocRide.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public float Coast { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PickUpLocation { get; set; }
        public string Destination { get; set; }
        public RideStatus RideStatus { get; set; }
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
