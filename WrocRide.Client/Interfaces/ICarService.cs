using WrocRide.Shared.DTOs.Car;

namespace WrocRide.Client.Interfaces
{
    public interface ICarService
    {
        Task<CarDto> GetCar(int driverId, int id);
        Task UpdateCar(int driverId, int id, UpdateCarDto dto);
    }
}
