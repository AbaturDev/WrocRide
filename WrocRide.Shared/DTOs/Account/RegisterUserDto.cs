﻿namespace WrocRide.Shared.DTOs.Account
{
    public record RegisterUserDto
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
