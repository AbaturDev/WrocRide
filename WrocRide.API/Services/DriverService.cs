
namespace WrocRide.API.Services
{
    public interface IDriverService
    {
        Task<PagedList<DriverDto>> GetAll(DriverQuery query);
        Task<DriverDto> GetById(int id);
        Task UpdatePricing(UpdateDriverPricingDto dto);
        Task UpdateStatus(UpdateDriverStatusDto dto);
        Task<PagedList<RatingDto>> GetRatings(int id, DriverRatingsQuery query);
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

        public async Task<PagedList<DriverDto>> GetAll(DriverQuery query)
        {
            IQueryable<Driver> baseQuery = _dbContext.Drivers
                .Include(d => d.Car)
                .Include(d => d.User);

            if(query.DriverStatus != null)
            {
                baseQuery = baseQuery.Where(d => d.DriverStatus == query.DriverStatus);
            }

            var totalItemsCount = await baseQuery.CountAsync();

            var drivers = await baseQuery
                .Select(d => new DriverDto()
                {
                    Id = d.Id,
                    Name = d.User.Name,
                    Surename = d.User.Surename,
                    Rating = d.Rating,
                    Pricing = d.Pricing,
                    DriverStatus = d.DriverStatus,
                    CarId = d.CarId
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedList<DriverDto>(drivers, query.PageSize, query.PageNumber, totalItemsCount);
            
            return result;
        }

        public async Task<DriverDto> GetById(int id)
        {
            var driver = await _dbContext.Drivers
                .Include(d => d.Car)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var result = new DriverDto()
            {
                Id = driver.Id,
                Name = driver.User.Name,
                Surename = driver.User.Surename,
                Rating = driver.Rating,
                Pricing = driver.Pricing,
                DriverStatus = driver.DriverStatus,
                CarId = driver.CarId
            };

            return result;
        }

        public async Task UpdatePricing(UpdateDriverPricingDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            
            if(driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }

            driver.Pricing = dto.Pricing;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatus(UpdateDriverStatusDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }

            driver.DriverStatus = dto.DriverStatus;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedList<RatingDto>> GetRatings(int id, DriverRatingsQuery query)
        {
            var driver = await _dbContext.Drivers
                .Include(d => d.Rides)
                .ThenInclude(d => d.Rating)
                .FirstOrDefaultAsync(d => d.Id == id);

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

            var totalItemsCount = await rides.CountAsync();
            
            var ratings = await rides
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
                .ToListAsync();

            var result = new PagedList<RatingDto>(ratings, query.PageSize, query.PageNumber, totalItemsCount);
         
            return result;
        }
    }
}
