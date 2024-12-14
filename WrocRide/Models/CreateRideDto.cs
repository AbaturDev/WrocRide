namespace WrocRide.Models
{
    public class CreateRideDto
    {
        public string PickUpLocation { get; set; }
        public string Destination { get; set; }
        public int DriverId { get; set; }
    }
}
