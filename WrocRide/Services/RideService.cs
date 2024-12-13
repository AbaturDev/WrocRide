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
    }
}
