namespace WrocRide.Shared.PaginationHelpers;

public record PageQuery
{
    public int PageSize { get; set; } = 5;
    public int PageNumber { get; set; } = 1;
}
