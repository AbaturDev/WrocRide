using WrocRide.Models.Enums;
namespace WrocRide.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReporterUserId { get; set; }
        public virtual User Reporter { get; set; }
        public int ReportedUserId { get; set; }
        public virtual User Reported { get; set; }
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }
        public int? AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
