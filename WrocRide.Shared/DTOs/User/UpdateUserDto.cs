namespace WrocRide.Shared.DTOs.User;

public sealed record UpdateUserDto
{
    public string? Name { get; set; }
    public string? Surename { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public bool? IsActive { get; set; }
}
