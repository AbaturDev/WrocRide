namespace WrocRide.API.Services
{
    public interface IReportService
    {
        Task CreateReport(int rideId, CreateReportDto dto);
        Task<ReportDto> Get(int rideId);
        Task Delete(int rideId);
        Task Update(int rideId, CreateReportDto dto);
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

        public async Task CreateReport(int rideId, CreateReportDto dto)
        {
            var userId = _userContextService.GetUserId;

            var ride = await _dbContext.Rides
                .Include(r => r.Driver)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found.");
            }

            var reportExists = await _dbContext.Reports
                .FirstOrDefaultAsync(r => r.RideId == rideId && r.ReporterUserId == userId);

            if (reportExists != null)
            {
                throw new BadRequestException("Report already exists.");
            }

            if (ride.Driver.UserId == userId)
            {
                var report = new Report
                {
                    Reason = dto.Reason,
                    ReporterUserId = ride.Driver.UserId,
                    ReportedUserId = ride.Client.UserId,
                    RideId = rideId,
                    ReportStatus = ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                await _dbContext.Reports.AddAsync(report);
            }
            else if (ride.Client.UserId == userId)
            {
                var report = new Report
                {
                    Reason = dto.Reason,
                    ReporterUserId = ride.Client.UserId,
                    ReportedUserId = ride.Driver.UserId,
                    RideId = rideId,
                    ReportStatus = ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                await _dbContext.Reports.AddAsync(report);
            }
            else 
            {
                throw new ForbidException("User is not part of a ride.");
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReportDto> Get(int rideId)
        {
            var report = await GetReportByRideId(rideId);

            var result = new ReportDto
            {
                Id = report.Id,
                Reason = report.Reason,
                ReportStatus = report.ReportStatus,
                CreatedAt = report.CreatedAt,
                ReporterUserId = report.ReporterUserId,
                ReportedUserId = report.ReportedUserId,
                RideId = report.RideId
            };

            return result;
        }

        public async Task Delete(int rideId)
        {
            var report = await GetReportByRideId(rideId);

            _dbContext.Reports.Remove(report);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int rideId, CreateReportDto dto)
        {
            var report = await GetReportByRideId(rideId);

            report.Reason = dto.Reason;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<Report> GetReportByRideId(int rideId)
        {
            var userId = _userContextService.GetUserId;

            var report = await _dbContext.Reports
                .FirstOrDefaultAsync(r => r.RideId == rideId && r.ReporterUserId == userId);

            if (report == null)
            {
                throw new NotFoundException("Report not found");
            }

            return report;
        }
    }

}
