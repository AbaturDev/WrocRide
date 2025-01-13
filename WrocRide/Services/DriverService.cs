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
        void UpdatePricing(UpdateDriverPricingDto dto);
        void UpdateStatus(UpdateDriverStatusDto dto);
        PagedList<RatingDto> GetRatings(int id, DriverRatingsQuery query);
    }

    public class DriverService : IDriverService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContext;
        public DriverService(WrocRideDbContext dbContext, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
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

            var result = new PagedList<DriverDto>(drivers, query.PageSize, query.PageNumber, baseQuery.Count());
            
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

        public void UpdatePricing(UpdateDriverPricingDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.UserId == userId);
            
            if(driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }

            driver.Pricing = dto.Pricing;
            _dbContext.SaveChanges();
        }

        public void UpdateStatus(UpdateDriverStatusDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.UserId == userId);

            if (driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }

            driver.DriverStatus = dto.DriverStatus;
            _dbContext.SaveChanges();
        }

        public PagedList<RatingDto> GetRatings(int id, DriverRatingsQuery query)
        {
            var driver = _dbContext.Drivers
                .Include(d => d.Rides)
                .ThenInclude(d => d.Rating)
                .FirstOrDefault(d => d.Id == id);

            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var rides = _dbContext.Rides
                .Include(r => r.Rating)
                .Include(r => r.Driver)
                    .ThenInclude(r => r.User)
                .Include(r => r.Client)
                    .ThenInclude(r => r.User)
                .Where(r => r.DriverId == driver.Id && r.Rating != null);
            
            var ratings = rides
                .Select(r => new RatingDto()
                {
                    Grade = r.Rating.Grade,
                    Comment = r.Rating.Comment,
                    CreatedAt = r.Rating.CreatedAt,
                    ClientName = r.Client.User.Name,
                    ClientSurename = r.Client.User.Surename,
                    DriverName = r.Driver.User.Name,
                    DriverSurename = r.Driver.User.Surename
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<RatingDto>(ratings, query.PageSize, query.PageNumber, rides.Count());
         
            return result;
        }
    }
}
