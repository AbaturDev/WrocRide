using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Models.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WrocRide.Services
{
    public interface IRideService
    {
        int CreateRide(CreateRideDto dto);
        PagedList<RideDto> GetAll(RideQuery query);
        RideDeatailsDto GetById(int id);
        void UpdateRideStatus(int id, UpdateRideStatusDto dto);
        public void DriverDecision(int id, RideDriverDecisionDto dto);

    }
    public class RideService : IRideService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContext;

        public RideService(WrocRideDbContext dbContext, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public int CreateRide(CreateRideDto dto)
        {
            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == dto.DriverId);
            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

            if (userId == null)
            {
                throw new NotLoggedException("User is not logged in");
            }

            var client = _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride");
            }

            var ride = new Ride()
            {
                ClientId = client.Id,
                RideStatus = RideStatus.WaitingForDriver,
                DriverId = dto.DriverId,
                PickUpLocation = dto.PickUpLocation,
                Destination = dto.Destination
            };

            _dbContext.Rides.Add(ride);
            _dbContext.SaveChanges();

            return ride.Id;
        }

        public PagedList<RideDto> GetAll(RideQuery query)
        {
            var userId = _userContext.GetUserId;

            if (userId == null)
            {
                throw new NotLoggedException("User is not logged in");
            }

            var client = _dbContext.Clients
                .FirstOrDefault(c => c.UserId == userId);

            var driver = _dbContext.Drivers
                .FirstOrDefault(d => d.UserId == userId);

            if (client == null && driver == null)
            {
                throw new BadRequestException("User is not a driver nor a client");
            }

            var baseQuery = _dbContext.Rides
                .Include(r => r.Client).ThenInclude(r => r.User)
                .Include(r => r.Driver).ThenInclude(r => r.User)
                .Include(r => r.Driver).ThenInclude(r => r.Car)
                .AsQueryable();

            if (client != null)
            {
                baseQuery = baseQuery.Where(r => r.ClientId == client.Id);
            }

            if (driver != null)
            {
                baseQuery = baseQuery.Where(r => r.DriverId == driver.Id);
            }

            var rides = baseQuery
                .Select(r => new RideDto()
                {
                    ClientName = r.Client.User.Name,
                    ClientSurename = r.Client.User.Surename,
                    DriverName = r.Driver.User.Name,
                    DriverSurename = r.Driver.User.Surename,
                    Destination = r.Destination,
                    PickUpLocation = r.PickUpLocation
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<RideDto>(rides, query.PageSize, query.PageNumber, rides.Count());

            return result;
        }

        public RideDeatailsDto GetById(int id)
        {
            var ride = _dbContext.Rides
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Include(r => r.Driver)
                    .ThenInclude(d => d.Car)
                .Include(r => r.Driver)
                    .ThenInclude(d => d.User)
                .Where(r => r.RideStatus == RideStatus.Ended || r.RideStatus == RideStatus.Canceled)
                .FirstOrDefault(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            if (ride.Client?.User == null || ride.Driver?.User == null || ride.Driver.Car == null)
            {
                throw new BadRequestException("Incomplete ride data");
            }

            var rideDeatails = new RideDeatailsDto()
            {
                ClientName = ride.Client.User.Name,
                ClientSurename = ride.Client.User.Surename,
                DriverName = ride.Driver.User.Name,
                DriverSurename = ride.Driver.User.Surename,
                PickUpLocation = ride.PickUpLocation,
                Destination = ride.Destination,
                Coast = ride.Coast,
                EndDate = ride.EndDate,
                StartDate = ride.StartDate,
                RideStatus = ride.RideStatus,
                ClientId = ride.ClientId,
                DriverId = ride.DriverId,
                CarModel = ride.Driver.Car.Model,
                CarBrand = ride.Driver.Car.Brand,
                Grade = 1                                       //to add review grade with validation (when reviews will be implemented)
            };

            return rideDeatails;
        }

        public void UpdateRideStatus(int id, UpdateRideStatusDto dto)
        {
            var ride = _dbContext.Rides.FirstOrDefault(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            ride.RideStatus = dto.RideStatus;
            _dbContext.SaveChanges();
        }

        public void DriverDecision(int id, RideDriverDecisionDto dto)
        {
            var ride = _dbContext.Rides
                .Include(r => r.Driver)
                .FirstOrDefault(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                UpdateRideStatus(id, dto);

                if (dto.RideStatus == RideStatus.Accepted)
                {
                    ride.Driver.DriverStatus = DriverStatus.Occupied;
                    ride.StartDate = DateTime.Now;
                    ride.Coast = dto.Coast;
                }

                else if (dto.RideStatus == RideStatus.Canceled)
                {
                    ride.StartDate = DateTime.Now;
                    ride.EndDate = DateTime.Now;
                }

                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }
        }
    }
}
