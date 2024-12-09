using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IDriverService
    {
        PagedList<DriverDto> GetAll(DriverQuery query);
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

        public PagedList<DriverDto> GetAll(DriverQuery query)
        {
            IQueryable<Driver> baseQuery = _dbContext.Drivers
                .Include(d => d.Car)
                .Include(d => d.User);

            if(query.DriverStatus != null)
            {
                baseQuery = baseQuery.Where(d => d.DriverStatus == query.DriverStatus);
            }

            var drivers = baseQuery
                .Select(d => new DriverDto()
                {
                    Name = d.User.Name,
                    Surename = d.User.Surename,
                    Rating = d.Rating,
                    Pricing = d.Pricing,
                    DriverStatus = d.DriverStatus
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<DriverDto>(drivers, query.PageSize, query.PageNumber, drivers.Count());
            
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
