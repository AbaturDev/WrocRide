
namespace WrocRide.API.Services
{
    public interface ICarService
    {
        void UpdateCar(int driverId, int carId, UpdateCarDto dto);
        CarDto GetById(int driverId, int carId);
    }

    public class CarService : ICarService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        public CarService(WrocRideDbContext dbContext, IUserContextService user)
        {
            _dbContext = dbContext;
            _userContextService = user;
        }

        public void UpdateCar(int driverId, int carId, UpdateCarDto dto)
        {
            var userId = _userContextService.GetUserId;

            var driver = _dbContext.Drivers
                .Include(d => d.Car)
                .FirstOrDefault(d => d.Id == driverId);

            if (driver == null)
            {
                throw new NotFoundException("Driver not found.");
            }

            if (driver.UserId != userId)
            {
                throw new BadRequestException("User is not a driver");
            }

            if (driver.CarId != carId)
            {
                throw new ForbidException("Driver is not the car owner.");
            }

            if (!string.IsNullOrEmpty(dto.Model))
            {
                driver.Car.Model = dto.Model;
            }

            if (!string.IsNullOrEmpty(dto.Brand))
            {
                driver.Car.Brand = dto.Brand;
            }

            if (!string.IsNullOrEmpty(dto.BodyColor))
            {
                driver.Car.BodyColor = dto.BodyColor;
            }

            if (!string.IsNullOrEmpty(dto.LicensePlate))
            {
                driver.Car.LicensePlate = dto.LicensePlate;
            }

            if(dto.YearProduced.HasValue)
            {
                driver.Car.YearProduced = dto.YearProduced.Value;
            }

            _dbContext.SaveChanges();
        }

        public CarDto GetById(int driverId, int carId)
        {
            var driver = _dbContext.Drivers
                .Include(d => d.Car)
                .FirstOrDefault(d => d.Id == driverId);

            if(driver == null)
            {
                throw new NotFoundException("Driver not found");
            }

            if(driver.CarId != carId)
            {
                throw new NotFoundException("Car not found");
            }

            var carDto = new CarDto()
            {
                Brand = driver.Car.Brand,
                Model = driver.Car.Model,
                LicensePlate = driver.Car.LicensePlate,
                BodyColor = driver.Car.BodyColor,
                YearProduced = driver.Car.YearProduced
            };

            return carDto;
        }
    }
}
