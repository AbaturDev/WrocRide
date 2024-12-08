namespace WrocRide.Models
{
    public record LoginUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
