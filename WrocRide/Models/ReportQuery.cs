using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class ReportQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int? ReportedId { get; set; } 
        public ReportStatus? ReportStatus { get; set; }
    }
}
