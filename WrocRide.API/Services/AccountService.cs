namespace WrocRide.API.Services
{
    public interface IAccountService
    {
        Task Register(RegisterUserDto dto);
        Task RegisterDriver(RegisterDriverDto dto);
        Task<string> Login(LoginUserDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtAuthentication _jwtAuthentication;

        public AccountService(WrocRideDbContext dbContext, IPasswordHasher<User> passwordHasher, JwtAuthentication jwtAuthentication)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtAuthentication = jwtAuthentication;
        }

        public async Task Register(RegisterUserDto dto)
        {
            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var newUser = new User()
                {
                    Name = dto.Name,
                    Surename = dto.Surename,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = dto.RoleId,
                    JoinAt = DateTime.Now,
                    Balance = 0,
                    IsActive = true
                };

                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
                if(role == null)
                {
                    throw new NotFoundException("Role not found");
                }
                
                if(role.Name == "Client")
                {
                    var client = new Client()
                    {
                        UserId = newUser.Id
                    };
                    await _dbContext.Clients.AddAsync(client);
                }
                else if(role.Name == "Admin")
                {
                    var admin = new Admin()
                    {
                        UserId = newUser.Id
                    };
                    await _dbContext.Admins.AddAsync(admin);
                }
                else
                {
                    throw new BadRequestException("Invalid role assigned");
                }

                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch(Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task RegisterDriver(RegisterDriverDto dto)
        {
            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var newUser = new User()
                {
                    Name = dto.Name,
                    Surename = dto.Surename,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = dto.RoleId,
                    JoinAt = DateTime.Now,
                    Balance = 0,
                    IsActive = false
                };

                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                var document = new Document()
                {
                    FileLocation = dto.FileLocation,
                    RequestDate = DateTime.Now,
                    DocumentStatus = DocumentStatus.UnderVerification
                };

                await _dbContext.Documents.AddAsync(document);

                var car = new Car()
                {
                    LicensePlate = dto.LicensePlate,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    BodyColor = dto.BodyColor,
                };

                await _dbContext.Cars.AddAsync(car);
                await _dbContext.SaveChangesAsync();

                var driver = new Driver()
                {
                    UserId = newUser.Id,
                    Pricing = dto.Pricing,
                    DriverStatus = DriverStatus.UnderVerification,
                    DocumentId = document.Id,
                    CarId = car.Id
                };

                await _dbContext.Drivers.AddAsync(driver);
                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }

            catch(Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task<string> Login(LoginUserDto dto)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if(user == null)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surename}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("IsActive", user.IsActive.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_jwtAuthentication.Expires);

            var tokenOptions = new JwtSecurityToken(issuer: _jwtAuthentication.Issuer,
                audience: _jwtAuthentication.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
