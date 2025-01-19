using WrocRide.Shared.DTOs.User;

namespace WrocRide.Client.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUser();
        Task UpdateProfile(UpdateUserDto dto);
        Task AddCredits(AddCreditsDto dto);
        Task DeactivateAccount();
    }
}
