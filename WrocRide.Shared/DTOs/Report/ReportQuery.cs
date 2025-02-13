using WrocRide.Shared.Enums;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Shared.DTOs.Report;
public sealed record ReportQuery(ReportStatus? ReportStatus) : PageQuery;