namespace WrocRide.Models
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
    }
}
