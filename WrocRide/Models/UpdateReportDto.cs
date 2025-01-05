using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class UpdateReportDto
    {
        public ReportStatus ReportStatus { get; set; }
        public int ReportedId { get; set; }
    }
}
