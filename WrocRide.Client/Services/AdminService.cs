using System.Net.Http.Json;
using WrocRide.Client.Interfaces;
using WrocRide.Shared;
using WrocRide.Shared.DTOs.Document;
using WrocRide.Shared.DTOs.Report;
using WrocRide.Shared.DTOs.User;
using WrocRide.Shared.Enums;

namespace WrocRide.Client.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAddBearerTokenService _addBearerTokenService;
        private readonly HttpClient _httpClient;

        public AdminService(IAddBearerTokenService addBearerTokenService, HttpClient httpClient)
        {
            _addBearerTokenService = addBearerTokenService;
            _httpClient = httpClient;
        }

        public async Task<PagedList<DocumentDto>> GetAllDocuments(int pageSize, int pageNumber, DocumentStatus? documentStatus)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var url = $"api/admin/documents?pageSize={pageSize}&pageNumber={pageNumber}";
            
            if(documentStatus != null)
            {
                url += $"&documentStatus={documentStatus}";
            }

            var response = await _httpClient.GetFromJsonAsync<PagedList<DocumentDto>>(url);
            return response;
        }

        public async Task<PagedList<ReportDto>> GetAllReports(int pageSize, int pageNumber, ReportStatus? reportStatus)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var url = $"api/admin/reports?pageSize={pageSize}&pageNumber={pageNumber}";

            if (reportStatus != null)
            {
                url += $"&reportStatus={reportStatus}";
            }

            var response = await _httpClient.GetFromJsonAsync<PagedList<ReportDto>>(url);
            return response;
        }

        public async Task<PagedList<UserDto>> GetAllUsers(int pageSize, int pageNumber, int? roleId)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            var url = $"api/admin/users?pageSize={pageSize}&pageNumber={pageNumber}";

            if (roleId != null)
            {
                url += $"&roleId={roleId}";
            }

            var response = await _httpClient.GetFromJsonAsync<PagedList<UserDto>>(url);
            return response;
        }

        public async Task UpdateDocument(int id, UpdateDocumentDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/admin/document/{id}", dto);
        }

        public async Task UpdateReport(int id, UpdateReportDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/admin/report/{id}", dto);
        }

        public async Task UpdateUser(int id, UpdateUserDto dto)
        {
            await _addBearerTokenService.AddBearerToken(_httpClient);
            await _httpClient.PutAsJsonAsync($"api/admin/user/{id}", dto);
        }
    }
}
