using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IRideService
    {
        int CreateRide(CreateRideDto dto);
        //List<RideDto> GetAll();
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
            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

            if(userId == null)
            {
                throw new NotLoggedException("User is not logged in");
            }

            var client = _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);
            
            if(client == null)
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

        //public List<RideDto> GetAll()
        //{
        //    var userId = _userContext.GetUserId;

        //    if(userId == null)
        //    {
        //        throw new NotLoggedException("User is not logged in");
        //    }

        //    var userClient = _dbContext.Clients
        //                            .Include(c => c.User)
        //                            .FirstOrDefault(c => c.UserId == userId);

        //}

        public RideDeatailsDto GetById(int id)
        {
            var ride = _dbContext.Rides
                            .Include(r => r.Client)
                                .ThenInclude(c => c.User)
                            .Include(r => r.Driver)
                                .ThenInclude(d => d.Car)
                            .Include(r => r.Driver)
                                .ThenInclude(d => d.User)
                            //.Where(r => r.RideStatus == RideStatus.Ended || r.RideStatus == RideStatus.Canceled )
                            .FirstOrDefault(r => r.Id == id);

            if(ride == null)
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
                Grade = 1
            };

            //to add review grade with validation (when reviews will be implemented)

            return rideDeatails;
        }

        public void UpdateRideStatus(int id, UpdateRideStatusDto dto)
        {
            var ride = _dbContext.Rides.FirstOrDefault(r => r.Id == id);

            if(ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            ride.RideStatus = dto.RideStatus;
            _dbContext.SaveChanges();
        }

        public void DriverDecision(int id, RideDriverDecisionDto dto)
        {
            //transaction
        }
    }
}
