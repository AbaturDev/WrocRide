namespace WrocRide.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }
        public int CreatedByClientId { get; set; }
    }
}
