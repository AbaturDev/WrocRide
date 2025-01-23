using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.Rating;

namespace WrocRide.Client.Services
{
    public class RatingService : IRatingService
    {
        private readonly IAddBearerTokenService _addBearerTokenService;
        private readonly HttpClient _httpClient;

        public RatingService(IAddBearerTokenService addBearerTokenService, HttpClient httpClient)
        {
            _addBearerTokenService = addBearerTokenService;
            _httpClient = httpClient;
        }
        public async Task CreateRating(int rideId, CreateRatingDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PostAsJsonAsync($"api/ride/{rideId}/rating", dto);
        }

        public async Task DeleteRating(int rideId)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.DeleteAsync($"api/ride/{rideId}/rating");
        }

        public async Task<RatingDto> GetRating(int rideId)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<RatingDto>($"api/ride/{rideId}/rating");

            if (response == null)
            {
                throw new Exception("Not found");
            }

            return response;

        }
        public async Task UpdateRating(int rideId, CreateRatingDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/ride/{rideId}/rating", dto);
        }
    }
}
