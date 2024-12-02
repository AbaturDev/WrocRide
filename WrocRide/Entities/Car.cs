namespace WrocRide.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string BodyColor { get; set; }
        public int YearProduced { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
