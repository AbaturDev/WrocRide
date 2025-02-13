namespace WrocRide.Shared.DTOs.Account
{
    public record RegisterDriverDto : RegisterUserDto
    {
        public decimal Pricing { get; set; }
        public string FileLocation { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string BodyColor { get; set; }
        public int YearProduced { get; set; }

    }
}
