namespace WrocRide.API.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUser();
        Task UpdateUser(UpdateUserDto dto);
        Task AddCredits(AddCreditsDto dto);
        Task DeactivateAccount();
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

        public async Task<UserDto> GetUser()
        {
            var userId = _userContextService.GetUserId;

            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

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
                Id = user.Id
            };

            return result;
        }

        public async Task UpdateUser(UpdateUserDto dto)
        {
            var userId = _userContextService.GetUserId;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

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

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCredits(AddCreditsDto dto)
        {
            var userId = _userContextService.GetUserId;
            
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.Balance += dto.Credits;
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeactivateAccount()
        {
            var userId = _userContextService.GetUserId;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }
    }
}