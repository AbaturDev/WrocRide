using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IDriverService
    {
        IEnumerable<DriverDto> GetAllAvailableDrivers();
        DriverDto GetById(int id);
        void UpdatePricing(int id, UpdateDriverPricingDto dto);
        void UpdateStatus(int id, UpdateDriverStatusDto dto);
    }


    public class DriverService : IDriverService
    {
        private readonly WrocRideDbContext _dbContext;
        public DriverService(WrocRideDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<DriverDto> GetAllAvailableDrivers()
        {
            var drivers = _dbContext.Drivers
                .Include(d => d.Car)
                .Include(d => d.User)
                //.Where(d => d.DriverStatus == DriverStatus.Available)
                .Select(d => new DriverDto()
                {
                    Name = d.User.Name,
                    Surename = d.User.Surename,
                    Rating = d.Rating,
                    Pricing = d.Pricing,
                    DriverStatus = d.DriverStatus
                })
                .ToList();

            // TODO:::Add pagination
            
            return drivers;
        }

        public DriverDto GetById(int id)
        {
            var driver = _dbContext.Drivers
                .Include(d => d.Car)
                .Include(d => d.User)
                .FirstOrDefault(d => d.Id == id);

            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var result = new DriverDto()
            {
                Name = driver.User.Name,
                Surename = driver.User.Surename,
                Rating = driver.Rating,
                Pricing = driver.Pricing,
                DriverStatus = driver.DriverStatus
            };

            return result;
        }

        public void UpdatePricing(int id, UpdateDriverPricingDto dto)
        {
            if(dto.Pricing <= 0)
            {
                throw new BadRequestException("Price value must be greater than 0");
            }

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == id);
            
            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            driver.Pricing = dto.Pricing;
            _dbContext.SaveChanges();
        }

        public void UpdateStatus(int id, UpdateDriverStatusDto dto)
        {
            if (!Enum.TryParse<DriverStatus>(dto.DriverStatus.ToString(), out _))
            {
                throw new BadRequestException("Invalid driver status value");
            }

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == id);
            
            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            driver.DriverStatus = dto.DriverStatus;
            _dbContext.SaveChanges();
        }
    }
}
