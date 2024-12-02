using WrocRide.Entities;

namespace WrocRide.Services
{
    public class RideService
    {
        private readonly WrocRideDbContext _dbContext;
        public RideService(WrocRideDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
