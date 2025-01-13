namespace WrocRide.Shared.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoinAt { get; set; }
        public int RoleId { get; set; }
    }
}
