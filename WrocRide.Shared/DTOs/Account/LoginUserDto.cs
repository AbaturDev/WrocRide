namespace WrocRide.Shared.DTOs.Account
{
    public record LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
