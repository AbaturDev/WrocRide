
namespace WrocRide.API.Services
{
    public interface IRatingService
    {
        Task<int> CreateRating(int rideId, CreateRatingDto dto);
        Task<RatingDto> Get(int rideId);
        Task Update(int rideId, CreateRatingDto dto);
        Task Delete(int rideId);
    }

    public class RatingService : IRatingService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContext;
        public RatingService(WrocRideDbContext dbContext, IUserContextService userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public async Task<int> CreateRating(int rideId, CreateRatingDto dto)
        {
            var ride = await GetCurrentClientRide(rideId);

            if(ride.Rating != null)
            {
                throw new BadRequestException("Ride already have an rating");
            }

            var rating = new Rating()
            {
                CreatedAt = DateTime.Now,
                Grade = dto.Grade,
                Comment = dto.Comment,
                RideId = ride.Id,
                CreatedByClientId = ride.ClientId
            };

            await _dbContext.Ratings.AddAsync(rating);
            await _dbContext.SaveChangesAsync();

            await UpdateDriverAverageRating(ride.DriverId);

            return rating.Id;
        }

        public async Task Update(int rideId, CreateRatingDto dto)
        {
            var ride = await GetCurrentClientRide(rideId);

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            if(ride.Rating.CreatedByClientId != ride.ClientId)
            {
                throw new ForbidException("Client is not a rating creator");
            }

            ride.Rating.Grade = dto.Grade;
            ride.Rating.Comment = dto.Comment;

            await _dbContext.SaveChangesAsync();

            await UpdateDriverAverageRating(ride.DriverId);
        }

        public async Task Delete(int rideId)
        {
            var ride = await GetCurrentClientRide(rideId);

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            if (ride.Rating.CreatedByClientId != ride.ClientId)
            {
                throw new ForbidException("Client is not a rating creator");
            }

            _dbContext.Ratings.Remove(ride.Rating);
            await _dbContext.SaveChangesAsync();

            await UpdateDriverAverageRating(ride.DriverId);
        }

        public async Task<RatingDto> Get(int rideId)
        {
            var ride = await _dbContext.Rides
                .Include(r => r.Rating)
                .Include(r => r.Driver)
                    .ThenInclude(r => r.User)
                .Include(r => r.Client)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            if(ride.Rating == null)
            {
                throw new BadRequestException("Ride does not have a rating");
            }

            var result = new RatingDto()
            {
                Grade = ride.Rating.Grade,
                Comment = ride.Rating.Comment,
                CreatedAt = ride.Rating.CreatedAt,
                ClientName = ride.Client.User.Name,
                ClientSurename = ride.Client.User.Surename,
                DriverName = ride.Driver.User.Name,
                DriverSurename = ride.Driver.User.Surename
            };

            return result;
        }

        private async Task<Ride> GetCurrentClientRide(int rideId)
        {
            var userId = _userContext.GetUserId;

            var client = await _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("User is not a client");
            }

            var ride = await _dbContext.Rides
                .Include(r => r.Rating)
                .FirstOrDefaultAsync(r => r.ClientId == client.Id && r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            return ride;
        }

        private async Task UpdateDriverAverageRating(int driverId)
        {
            var driver = await _dbContext.Drivers
                .FirstOrDefaultAsync(r => r.Id == driverId);

            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var driverRatings = _dbContext.Rides
                .Include(r => r.Rating)
                .Where(r => r.DriverId == driverId && r.Rating != null);

            if (!await driverRatings.AnyAsync())
            {
                driver.Rating = null;
            }
            else
            {
                driver.Rating = (float)await driverRatings.AverageAsync(r => r.Rating.Grade);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
