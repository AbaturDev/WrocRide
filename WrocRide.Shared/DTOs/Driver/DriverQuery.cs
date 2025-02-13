using WrocRide.Shared.Enums;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Shared.DTOs.Driver;

public sealed record DriverQuery(DriverStatus? DriverStatus) : PageQuery;
