using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared.DTOs.Report;

namespace WrocRide.Client.Services
{
    public class ReportService : IReportService
    {
        private readonly IAddBearerTokenService _addBearerTokenService;
        private readonly HttpClient _httpClient;

        public ReportService(IAddBearerTokenService addBearerTokenService, HttpClient httpClient)
        {
            _addBearerTokenService = addBearerTokenService;
            _httpClient = httpClient;
        }

        public async Task CreateReport(int rideId, CreateReportDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PostAsJsonAsync($"api/ride/{rideId}/report", dto);
        }

        public async Task DeleteReport(int rideId)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.DeleteAsync($"api/ride/{rideId}/report");
        }

        public async Task<ReportDto> GetReport(int rideId)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<ReportDto>($"api/ride/{rideId}/report");

            return response;
        }

        public async Task UpdateReport(int rideId, CreateReportDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/ride/{rideId}/report", dto);
        }
    }
}
