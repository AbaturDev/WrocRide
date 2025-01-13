namespace WrocRide.Shared.DTOs.Rating
{
    public record RatingDto
    {
        public int Grade { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ClientName { get; set; }
        public string ClientSurename { get; set; }
        public string DriverName { get; set; }
        public string DriverSurename { get; set; }
    }
}
