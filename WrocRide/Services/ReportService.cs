using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;

namespace WrocRide.Services
{
    public interface IReportService
    {
        void reportUser(CreateReportDto dto);
    }
    public class ReportService : IReportService
    {
        private readonly WrocRideDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        public ReportService(WrocRideDbContext dbContext, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public void reportUser(CreateReportDto dto)
        {
            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new NotFoundException("User not found");
            }

            var report = new Report
            {
                Reason = dto.Reason,
                ReporterId = (int)userId,
                ReportedId = dto.ReportedId,
                RideId = dto.RideId,
                ReportStatus = Models.Enums.ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Reports.Add(report);
            _dbContext.SaveChanges();
        }
    }
}
