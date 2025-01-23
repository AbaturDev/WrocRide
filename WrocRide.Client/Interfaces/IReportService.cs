using WrocRide.Shared.DTOs.Rating;
using WrocRide.Shared.DTOs.Report;

namespace WrocRide.Client.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> GetReport(int rideId);
        Task CreateReport(int rideId, CreateReportDto dto);
        Task DeleteReport(int rideId);
        Task UpdateReport(int rideId, CreateReportDto dto);
    }
}
