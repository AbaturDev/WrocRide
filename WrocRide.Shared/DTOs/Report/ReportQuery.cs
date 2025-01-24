using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Report
{
    public class ReportQuery
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public ReportStatus? ReportStatus { get; set; }
    }
}
