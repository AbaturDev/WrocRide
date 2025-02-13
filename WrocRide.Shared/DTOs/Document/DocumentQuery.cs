using WrocRide.Shared.Enums;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.Shared.DTOs.Document;

public sealed record DocumentQuery(DocumentStatus? DocumentStatus) : PageQuery;
