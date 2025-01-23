using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared;
using WrocRide.Shared.DTOs.Schedule;

namespace WrocRide.Client.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IAddBearerTokenService _addBearerTokenService;
        private readonly HttpClient _httpClient;

        public ScheduleService(IAddBearerTokenService addBearerTokenService, HttpClient httpClient)
        {
            _addBearerTokenService = addBearerTokenService;
            _httpClient = httpClient;
        }
        public async Task CreateSchedule(CreateScheduleDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PostAsJsonAsync("api/schedule", dto);
        }

        public async Task DeleteSchedule(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.DeleteAsync($"api/schedule/{id}");
        }

        public async Task<PagedList<ScheduleDto>> GetAllSchedules(int pageSize, int pageNumber)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<PagedList<ScheduleDto>>($"api/schedule?pageSize={pageSize}&pageNumber={pageNumber}");

            return response;
        }

        public async Task<ScheduleDto> GetScheduleById(int id)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<ScheduleDto>($"api/schedule/{id}");

            return response;
        }
    }
}
