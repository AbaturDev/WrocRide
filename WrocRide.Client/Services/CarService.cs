using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.Car;

namespace WrocRide.Client.Services
{
    public class CarService : ICarService
    {
        private HttpClient _httpClient;
        private IAddBearerTokenService _addBearerTokenService;

        public CarService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
        {
            _httpClient = httpClient;
            _addBearerTokenService = addBearerTokenService;
        }

        public async Task<CarDto> GetCar(int driverId, int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<CarDto>($"api/driver/{driverId}/car/{id}");

            return response;
        }

        public async Task UpdateCar(int driverId, int id, UpdateCarDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/driver/{driverId}/car/{id}", dto);
        }
    }
}
