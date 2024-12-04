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
                .Where(d => d.DriverStatus == DriverStatus.Available)
                .ToList();

            var result = new List<DriverDto>();
            foreach (var driver in drivers)
            {
                var dto = new DriverDto()
                {
                    Name = driver.User.Name,
                    Surename = driver.User.Surename,
                    Rating = driver.Rating,
                    Pricing = driver.Pricing,
                    DriverStatus = driver.DriverStatus
                };
                result.Append(dto);
            }
            // TODO:::Add pagination
            
            return result;
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
    }
}
