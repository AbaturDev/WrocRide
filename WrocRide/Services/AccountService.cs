using Microsoft.AspNetCore.Identity;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IAccountService
    {
        void Register(RegisterUserDto dto);
        void RegisterDriver(RegisterDriverDto dto);
        void Login(LoginUserDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(WrocRideDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Register(RegisterUserDto dto)
        {
            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                var newUser = new User()
                {
                    Name = dto.Name,
                    Surename = dto.Surename,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = dto.RoleId
                };

                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                var role = _dbContext.Roles.FirstOrDefault(r => r.Id == dto.RoleId);
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
                    _dbContext.Clients.Add(client);
                }
                else if(role.Name == "Admin")
                {
                    var admin = new Admin()
                    {
                        UserId = newUser.Id
                    };
                    _dbContext.Admins.Add(admin);
                }
                else
                {
                    throw new InvalidRoleException("Invalid role assigned");
                }

                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch(Exception)
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }
        }

        public void RegisterDriver(RegisterDriverDto dto)
        {
            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                var newUser = new User()
                {
                    Name = dto.Name,
                    Surename = dto.Surename,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    RoleId = dto.RoleId
                };

                var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                var document = new Document()
                {
                    FileLocation = dto.FileLocation,
                    RequestDate = DateTime.Now,
                    DocumentStatus = DocumentStatus.UnderVerification
                };

                _dbContext.Documents.Add(document);

                var car = new Car()
                {
                    LicensePlate = dto.LicensePlate,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    BodyColor = dto.BodyColor,
                };

                _dbContext.Cars.Add(car);
                _dbContext.SaveChanges();

                var driver = new Driver()
                {
                    UserId = newUser.Id,
                    Pricing = dto.Pricing,
                    Rating = 0,
                    DriverStatus = DriverStatus.UnderVerification,
                    DocumentId = document.Id,
                    CarId = car.Id
                };

                _dbContext.Drivers.Add(driver);
                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }

            catch(Exception)
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }
        }

        public void Login(LoginUserDto dto)
        {
            //todo login + authentication
        }
    }
}
