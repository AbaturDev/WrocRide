using WrocRide.Entities;
using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReporterId { get; set; }
        public int ReportedId { get; set; }
        public int RideId { get; set; }
        public int? AdminId { get; set; }
    }
}
