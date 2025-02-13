namespace WrocRide.Shared.DTOs.Car
{
    public sealed record CarDto
    {
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string BodyColor { get; set; }
        public int YearProduced { get; set; }
    }
}
