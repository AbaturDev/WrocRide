using WrocRide.Shared.DTOs.Car;

namespace WrocRide.Client.Interfaces
{
    public interface ICarService
    {
        Task<CarDto> GetCar(int id);
        Task UpdateCar(int id, int carId, UpdateCarDto dto);
    }
}
