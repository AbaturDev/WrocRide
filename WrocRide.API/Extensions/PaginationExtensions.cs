namespace WrocRide.API.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageSize, int pageNumber)
    {
        return source
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
    }
}
