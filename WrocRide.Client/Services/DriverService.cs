using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared;
using WrocRide.Shared.DTOs.Driver;
using WrocRide.Shared.DTOs.Rating;

namespace WrocRide.Client.Services
{
    public class DriverService : IDriverService
    {
        private readonly HttpClient _httpClient;
        private readonly IAddBearerTokenService _addBearerTokenService;

        public DriverService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
        {
            _httpClient = httpClient;
            _addBearerTokenService = addBearerTokenService;
        }

        public async Task<PagedList<DriverDto>> GetAll(int pageSize, int pageNumber)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<PagedList<DriverDto>>($"api/driver?pageSize={pageSize}&pageNumber={pageNumber}");

            return response;
        }

        public async Task<DriverDto> GetById(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<DriverDto>($"api/driver/{id}");

            return response;
        }

        public async Task<PagedList<RatingDto>> GetRatings(int id, int pageSize, int pageNumber)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<PagedList<RatingDto>>($"api/driver/{id}/ratings?pageSize={pageSize}&pageNumber={pageNumber}");

            return response;
        }

        public async Task UpdatePricing(UpdateDriverPricingDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync("api/driver/pricing", dto);
        }

        public async Task UpdateDriverStatus(UpdateDriverStatusDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync("api/driver/status", dto);
        }
    }
}
