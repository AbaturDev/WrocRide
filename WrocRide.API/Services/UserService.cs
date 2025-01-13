namespace WrocRide.API.Services
{
    public interface IUserService
    {
        UserDto GetUser();
        void UpdateUser(UpdateUserDto dto);
        void AddCredits(AddCreditsDto dto);
        void DeactivateAccount();
    }
    public class UserService : IUserService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(WrocRideDbContext dbContext, IUserContextService userContextService, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _passwordHasher = passwordHasher;
        }

        public UserDto GetUser()
        {
            var userId = _userContextService.GetUserId;

            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == userId);

            if(user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result =  new UserDto()
            {
                Name = user.Name,
                Surename = user.Surename,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                JoinAt = user.JoinAt,
                Balance = user.Balance,
                Role = user.Role.Name,
                IsActive = user.IsActive,
            };

            return result;
        }

        public void UpdateUser(UpdateUserDto dto)
        {
            var userId = _userContextService.GetUserId;

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                user.Name = dto.Name;
            }

            if (!string.IsNullOrEmpty(dto.Surename))
            {
                user.Surename = dto.Surename;
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                user.PhoneNumber = dto.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
                user.PasswordHash = hashedPassword;
            }

            _dbContext.SaveChanges();
        }

        public void AddCredits(AddCreditsDto dto)
        {
            var userId = _userContextService.GetUserId;
            
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.Balance += dto.Credits;
            
            _dbContext.SaveChanges();
        }

        public void DeactivateAccount()
        {
            var userId = _userContextService.GetUserId;

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.IsActive = false;
            _dbContext.SaveChanges();
        }
    }
}