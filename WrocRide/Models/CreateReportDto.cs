namespace WrocRide.Models
{
    public class CreateReportDto
    {
        public string Reason { get; set; }
        public int RideId { get; set; }
        public int ReportedId { get; set; }
    }
}
