namespace WrocRide.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public DateTime JoinAt { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Client Client { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
