using Microsoft.AspNetCore.SignalR;
using WrocRide.Shared.PaginationHelpers;

namespace WrocRide.API.Services
{
    public interface IRideService
    {
        Task<int> CreateRide(CreateRideDto dto);
        Task<int> CreateRideReservation(CreateRideReservationDto dto);
        Task<PagedList<RideDto>> GetAll(RideQuery query);
        Task<RideDeatailsDto> GetById(int id);
        Task UpdateRideStatus(int id, UpdateRideStatusDto dto);
        Task DriverDecision(int id, UpdateRideStatusDto dto);
        Task CancelRide(int id);
        Task EndRide(int id);
    }
    public class RideService : IRideService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public RideService(WrocRideDbContext dbContext, IUserContextService userContext, IHubContext<NotificationHub> hubContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
            _hubContext = hubContext;
        }

        public async Task<int> CreateRide(CreateRideDto dto)
        {
            var driver = await _dbContext.Drivers
                .FirstOrDefaultAsync(d => d.Id == dto.DriverId
                && d.DriverStatus == DriverStatus.Available);
            
            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

            var client = await _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride");
            }

            var ride = new Ride()
            {
                ClientId = client.Id,
                RideStatus = RideStatus.Pending,
                DriverId = dto.DriverId,
                PickUpLocation = dto.PickUpLocation,
                Destination = dto.Destination,
                StartDate = DateTime.Now,
                Distance = 1,                    //will be replaced by google api
            };

            ride.Coast = ride.Distance * driver.Pricing;

            if (ride.Coast > client.User.Balance)
            {
                throw new BadRequestException("Client does not have enough money for the ride");
            }

            await _dbContext.Rides.AddAsync(ride);
            await _dbContext.SaveChangesAsync();

            return ride.Id;
        }

        public async Task<int> CreateRideReservation(CreateRideReservationDto dto)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == dto.DriverId);
            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

            var client = await _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride");
            }
            
            var hasReservation = await _dbContext.Rides
                .Where(r => r.ClientId == client.Id)
                .AnyAsync(r => r.StartDate <= dto.StartDate && 
                          (r.EndDate == null || r.EndDate >= dto.StartDate));

            if (hasReservation)
            {
                throw new BadRequestException("Client already has a reservation at requested time");
            }
            
            var conflictingRides = await _dbContext.Rides
                .Where(r => r.DriverId == driver.Id)
                .Where(r => r.RideStatus == RideStatus.Accepted || 
                            r.RideStatus == RideStatus.Ongoing || 
                            r.RideStatus == RideStatus.Reserved)
                .AnyAsync(r => r.StartDate <= dto.StartDate &&
                          (r.EndDate == null || r.EndDate >= dto.StartDate));

            if (conflictingRides)
            {
                throw new BadRequestException("Driver is not available at the requested time");
            }
            
            var ride = new Ride()
            {
                ClientId = client.Id,
                RideStatus = RideStatus.ReservationRequested,
                DriverId = dto.DriverId,
                PickUpLocation = dto.PickUpLocation,
                Destination = dto.Destination,
                StartDate = dto.StartDate,
                Distance = 1                            //will be replaced by google api
            };
            
            ride.Coast = ride.Distance * driver.Pricing;
            
            if (ride.Coast > client.User.Balance)
            {
                throw new BadRequestException("Client does not have enough money to reserve this ride");
            }

            await _dbContext.Rides.AddAsync(ride);
            await _dbContext.SaveChangesAsync();

            return ride.Id;
        }

        public async Task<PagedList<RideDto>> GetAll(RideQuery query)
        {
            var userId = _userContext.GetUserId;

            var client = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var driver = await _dbContext.Drivers
                .FirstOrDefaultAsync(d => d.UserId == userId);

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

            if (query.RideStatus != null)
            {
                baseQuery = baseQuery.Where(r => r.RideStatus == query.RideStatus);
            }

            var totalItemsCount = await baseQuery.CountAsync();

            var rides = await baseQuery
                .Select(r => new RideDto()
                {
                    Id = r.Id,
                    ClientName = r.Client.User.Name,
                    ClientSurename = r.Client.User.Surename,
                    DriverName = r.Driver.User.Name,
                    DriverSurename = r.Driver.User.Surename,
                    Destination = r.Destination,
                    Distance = r.Distance,
                    PickUpLocation = r.PickUpLocation,
                    RideStatus = r.RideStatus
                })
                .Paginate(query.PageSize, query.PageNumber)
                .ToListAsync();

            var result = new PagedList<RideDto>(rides, query.PageSize, query.PageNumber, totalItemsCount);

            return result;
        }

        public async Task<RideDeatailsDto> GetById(int id)
        {
            var ride = await _dbContext.Rides
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Include(r => r.Driver)
                    .ThenInclude(d => d.Car)
                .Include(r => r.Driver)
                    .ThenInclude(d => d.User)
                .Include(r => r.Rating)
                .Where(r => r.RideStatus == RideStatus.Ended || r.RideStatus == RideStatus.Canceled)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }
    
            var rideDeatails = new RideDeatailsDto()
            {
                Id = ride.Id,
                ClientName = ride.Client.User.Name,
                ClientSurename = ride.Client.User.Surename,
                DriverName = ride.Driver.User.Name,
                DriverSurename = ride.Driver.User.Surename,
                PickUpLocation = ride.PickUpLocation,
                Destination = ride.Destination,
                Distance = ride.Distance,
                Coast = ride.Coast,
                EndDate = ride.EndDate,
                StartDate = ride.StartDate,
                RideStatus = ride.RideStatus,
                ClientId = ride.ClientId,
                DriverId = ride.DriverId,
                CarModel = ride.Driver.Car.Model,
                CarBrand = ride.Driver.Car.Brand
            };

            rideDeatails.Grade = ride.Rating?.Grade;

            return rideDeatails;
        }

        public async Task UpdateRideStatus(int id, UpdateRideStatusDto dto)
        {
            var ride = await _dbContext.Rides.FirstOrDefaultAsync(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            ride.RideStatus = dto.RideStatus;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DriverDecision(int id, UpdateRideStatusDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }
            
            var ride = await _dbContext.Rides
                .Include(r => r.Driver)
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Where(r => r.RideStatus == RideStatus.Pending 
                            || r.RideStatus == RideStatus.ReservationRequested)
                .FirstOrDefaultAsync(r => r.Id == id && r.DriverId == driver.Id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                 if (ride.RideStatus == RideStatus.ReservationRequested && dto.RideStatus == RideStatus.Accepted)
                {
                    ride.RideStatus = RideStatus.Reserved;
                }
                else if (dto.RideStatus == RideStatus.Accepted)
                {
                    ride.RideStatus = dto.RideStatus;
                    ride.Driver.DriverStatus = DriverStatus.Occupied;
                }
                else if (dto.RideStatus == RideStatus.Canceled)
                {
                    ride.RideStatus = dto.RideStatus;
                    ride.EndDate = DateTime.Now;
                }
                
                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task CancelRide(int id)
        {
            var userId = _userContext.GetUserId;

            var client = await _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride cancel");
            }

            var ride = await _dbContext.Rides
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(r => r.Id == id && 
                    r.ClientId == client.Id && 
                    (r.RideStatus == RideStatus.Pending 
                     || r.RideStatus == RideStatus.Accepted
                     || r.RideStatus == RideStatus.ReservationRequested
                     || r.RideStatus == RideStatus.Reserved
                     ));
                
            if(ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                ride.EndDate = DateTime.Now;
                ride.RideStatus = RideStatus.Canceled;

                if (ride.Driver == null)
                {
                    throw new NotFoundException("Driver not found");
                }

                ride.Driver.DriverStatus = DriverStatus.Available;

                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }
        
        public async Task EndRide(int id)
        {
            var userId = _userContext.GetUserId;

            var driver = await _dbContext.Drivers
                .Include(d => d.User)    
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if(driver == null)
            {
                throw new BadRequestException("This user is not a driver");
            }

            var ride = await _dbContext.Rides
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.DriverId == driver.Id && r.Id == id && r.RideStatus == RideStatus.Ongoing);

            if(ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            await using var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                ride.EndDate = DateTime.Now;
                ride.RideStatus = RideStatus.Ended;

                driver.DriverStatus = DriverStatus.Available;
                driver.User.Balance += ride.Coast;

                ride.Client.User.Balance -= ride.Coast;
                
                await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch
            {
                await dbContextTransaction.RollbackAsync();
                throw new Exception();
            }
        }
    }
}
