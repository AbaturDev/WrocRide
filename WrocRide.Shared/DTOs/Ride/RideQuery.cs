using WrocRide.Shared.Enums;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Shared.DTOs.Ride;

public sealed record RideQuery(RideStatus? RideStatus) : PageQuery;