namespace WrocRide.Models
{
    public class UserQuery
    {
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public int? RoleId { get; set; }
    }
}
