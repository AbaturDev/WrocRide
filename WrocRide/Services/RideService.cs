using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Helpers;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IRideService
    {
        int CreateRide(CreateRideDto dto);
        int CreateRideReservation(CreateRideReservationDto dto);
        PagedList<RideDto> GetAll(RideQuery query);
        RideDeatailsDto GetById(int id);
        void UpdateRideStatus(int id, UpdateRideStatusDto dto);
        void DriverDecision(int id, UpdateRideStatusDto dto);
        void CancelRide(int id);
        void EndRide(int id);
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

        public int CreateRide(CreateRideDto dto)
        {
            var driver = _dbContext.Drivers
                .FirstOrDefault(d => d.Id == dto.DriverId
                && d.DriverStatus == DriverStatus.Available);
            
            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

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

            _dbContext.Rides.Add(ride);
            _dbContext.SaveChanges();

            return ride.Id;
        }

        public int CreateRideReservation(CreateRideReservationDto dto)
        {
            var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == dto.DriverId);
            if (driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            var userId = _userContext.GetUserId;

            var client = _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride");
            }
            
            var hasReservation = _dbContext.Rides
                .Where(r => r.ClientId == client.Id)
                .Any(r => r.StartDate <= dto.StartDate && 
                          (r.EndDate == null || r.EndDate >= dto.StartDate));

            if (hasReservation)
            {
                throw new BadRequestException("Client already has a reservation at requested time");
            }
            
            var conflictingRides = _dbContext.Rides
                .Where(r => r.DriverId == driver.Id)
                .Where(r => r.RideStatus == RideStatus.Accepted || 
                            r.RideStatus == RideStatus.Ongoing || 
                            r.RideStatus == RideStatus.Reserved)
                .Any(r => r.StartDate <= dto.StartDate &&
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

            _dbContext.Rides.Add(ride);
            _dbContext.SaveChanges();

            return ride.Id;
        }

        public PagedList<RideDto> GetAll(RideQuery query)
        {
            var userId = _userContext.GetUserId;

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

            if (query.RideStatus != null)
            {
                baseQuery = baseQuery.Where(r => r.RideStatus == query.RideStatus);
            }

            var rides = baseQuery
                .Select(r => new RideDto()
                {
                    ClientName = r.Client.User.Name,
                    ClientSurename = r.Client.User.Surename,
                    DriverName = r.Driver.User.Name,
                    DriverSurename = r.Driver.User.Surename,
                    Destination = r.Destination,
                    Distance = r.Distance,
                    PickUpLocation = r.PickUpLocation,
                    RideStatus = r.RideStatus
                })
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var result = new PagedList<RideDto>(rides, query.PageSize, query.PageNumber, baseQuery.Count());

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
                .Include(r => r.Rating)
                .Where(r => r.RideStatus == RideStatus.Ended || r.RideStatus == RideStatus.Canceled)
                .FirstOrDefault(r => r.Id == id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }
    
            var rideDeatails = new RideDeatailsDto()
            {
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

        public void DriverDecision(int id, UpdateRideStatusDto dto)
        {
            var userId = _userContext.GetUserId;

            var driver = _dbContext.Drivers.FirstOrDefault(d => d.UserId == userId);

            if (driver == null)
            {
                throw new BadRequestException("User is not a driver");
            }
            
            var ride = _dbContext.Rides
                .Include(r => r.Driver)
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .Where(r => r.RideStatus == RideStatus.Pending 
                            || r.RideStatus == RideStatus.ReservationRequested)
                .FirstOrDefault(r => r.Id == id && r.DriverId == driver.Id);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
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
                
                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }
        }

        public void CancelRide(int id)
        {
            var userId = _userContext.GetUserId;

            var client = _dbContext.Clients
                .Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                throw new BadRequestException("This user is not a client. Failed request for a ride cancel");
            }

            var ride = _dbContext.Rides
                .Include(r => r.Driver)
                .FirstOrDefault(r => r.Id == id && 
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

            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                ride.EndDate = DateTime.Now;
                ride.RideStatus = RideStatus.Canceled;

                if (ride.Driver == null)
                {
                    throw new NotFoundException("Driver not found");
                }

                ride.Driver.DriverStatus = DriverStatus.Available;

                _dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch
            {
                dbContextTransaction.Rollback();
                throw new Exception();
            }
        }
        
        public void EndRide(int id)
        {
            var userId = _userContext.GetUserId;

            var driver = _dbContext.Drivers
                .Include(d => d.User)    
                .FirstOrDefault(d => d.UserId == userId);

            if(driver == null)
            {
                throw new BadRequestException("This user is not a driver");
            }

            var ride = _dbContext.Rides
                .Include(r => r.Client)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(r => r.DriverId == driver.Id && r.Id == id && r.RideStatus == RideStatus.Ongoing);

            if(ride == null)
            {
                throw new NotFoundException("Ride not found");
            }

            using var dbContextTransaction = _dbContext.Database.BeginTransaction();
            try
            {
                ride.EndDate = DateTime.Now;
                ride.RideStatus = RideStatus.Ended;

                driver.DriverStatus = DriverStatus.Available;
                driver.User.Balance += ride.Coast;

                ride.Client.User.Balance -= ride.Coast;
                
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
