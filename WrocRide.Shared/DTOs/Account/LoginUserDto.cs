namespace WrocRide.Shared.DTOs.Account
{
    public record LoginUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
