using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Shared.DTOs.User;

public sealed record UserQuery(int? RoleId) : PageQuery;