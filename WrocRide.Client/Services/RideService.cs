using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared;
using WrocRide.Shared.DTOs.Ride;

namespace WrocRide.Client.Services
{
    public class RideService : IRideService
    {
        private readonly HttpClient _httpClient;
        private readonly IAddBearerTokenService _addBearerTokenService;
        public RideService(HttpClient httpClient, IAddBearerTokenService addBearerTokenService)
        {
            _httpClient = httpClient;
            _addBearerTokenService = addBearerTokenService;
        }

        public async Task CancelRide(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsync($"api/ride/{id}/cancel-ride", null);
        }

        public async Task CreateRide(CreateRideDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PostAsJsonAsync("api/ride", dto);

        }

        public async Task CreateRideReservation(CreateRideReservationDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PostAsJsonAsync("api/ride/reservation", dto);
        }

        public async Task DriverDecision(int id, UpdateRideStatusDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/ride/{id}/driver-decision", dto);
        }

        public async Task EndRide(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsync($"api/ride/{id}/end-ride", null);
        }

        public async Task UpdateRideStatus(int id, UpdateRideStatusDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/ride/{id}/ride-status", dto);
        }

        public async Task<PagedList<RideDto>> GetAll(int pageSize, int pageNumber)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<PagedList<RideDto>>($"api/ride?pageSize={pageSize}&pageNumber={pageNumber}");

            return response;
        }

        public async Task<RideDeatailsDto> GetById(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<RideDeatailsDto>($"api/ride/{id}");

            return response;
        }
    }
}
