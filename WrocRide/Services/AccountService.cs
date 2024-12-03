using Microsoft.AspNetCore.Identity;
using WrocRide.Entities;
using WrocRide.Models;

namespace WrocRide.Services
{
    public interface IAccountService
    {
        void Register(RegisterUserDto dto);
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
            var newUser = new User()
            {
                Name = dto.Name,
                Surename = dto.Surename,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = dto.RoleId
            };

            //TODO::: - handle different roles cases -> (especialy the driver case) + validator
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public void Login(LoginUserDto dto)
        {
            //todo login + authentication
        }
    }
}
