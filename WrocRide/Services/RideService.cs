using WrocRide.Entities;

namespace WrocRide.Services
{
    public interface IRideService
    {
        void createRide(int driverId, int clientId, string pickUpLocation, string destination);
    }
    public class RideService
    {
        private readonly WrocRideDbContext _dbContext;
        public RideService(WrocRideDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
