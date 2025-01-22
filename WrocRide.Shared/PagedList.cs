namespace WrocRide.Shared
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedList(List<T> items, int pageSize, int pageNumber, int totalItemsCount)
        {
            Items = items;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalItemsCount /(double) pageSize);
            TotalItemsCount = totalItemsCount;
        }
    }
}
