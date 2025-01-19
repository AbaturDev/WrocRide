using WrocRide.Shared.DTOs.Account;

namespace WrocRide.Client.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginUserDto dto);
        Task Logout();
        Task RegisterDriver(RegisterDriverDto dto);
        Task Register(RegisterUserDto dto);
    }
}
