using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;

namespace WrocRide.Services
{
    public interface IUserService
    {
        UserDto GetUser();
    }
    public class UserService : IUserService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        public UserService(WrocRideDbContext dbContext, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public UserDto GetUser()
        {
            int? userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new NotFoundException("User not found"); // Change it to NotLoggedIn error
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found"); // Change it to NotLoggedIn error
            }

            var result =  new UserDto()
            {
                Name = user.Name,
                Surename = user.Surename,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                JoinAt = user.JoinAt
            };

            return result;
        }

    }
}
