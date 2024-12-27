using WrocRide.Models.Enums;

namespace WrocRide.Entities
{
    public class Driver
    {
        public int Id { get; set; }
        public float? Rating { get; set; }
        public decimal Pricing { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }
        public virtual List<Ride> Rides { get; set; }
    }
}
