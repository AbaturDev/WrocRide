using WrocRide.Shared;
using WrocRide.Shared.DTOs.Document;
using WrocRide.Shared.DTOs.Report;
using WrocRide.Shared.DTOs.User;
using WrocRide.Shared.Enums;

namespace WrocRide.Client.Interfaces
{
    public interface IAdminService
    {
        Task<PagedList<DocumentDto>> GetAllDocuments(int pageSize, int pageNumber, DocumentStatus? documentStatus);
        Task UpdateDocument(int id, UpdateDocumentDto dto);
        Task<PagedList<UserDto>> GetAllUsers(int pageSize, int pageNumber, int? roleId);
        Task UpdateUser(int id, UpdateUserDto dto);
        Task<PagedList<ReportDto>> GetAllReports(int pageSize, int pageNumber, ReportStatus? reportStatus);
        Task UpdateReport(int id, UpdateReportDto dto);
    }
}
