using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Exceptions;
using WrocRide.Models;
using WrocRide.Models.Enums;

namespace WrocRide.Services
{
    public interface IReportService
    {
        void CreateReport(int rideId, CreateReportDto dto);
        ReportDto Get(int rideId);
        void Delete(int rideId);
        void Update(int rideId, CreateReportDto dto);
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

        public void CreateReport(int rideId, CreateReportDto dto)
        {
            var userId = _userContextService.GetUserId;

            var ride = _dbContext.Rides
                .Include(r => r.Driver)
                .Include(r => r.Client)
                .FirstOrDefault(r => r.Id == rideId && r.RideStatus == RideStatus.Ended);

            if (ride == null)
            {
                throw new NotFoundException("Ride not found.");
            }

            var reportExists = _dbContext.Reports.FirstOrDefault(r => r.RideId == rideId && r.ReporterUserId == userId);

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
                    ReportStatus = Models.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Reports.Add(report);
            }
            else if (ride.Client.UserId == userId)
            {
                var report = new Report
                {
                    Reason = dto.Reason,
                    ReporterUserId = ride.Client.UserId,
                    ReportedUserId = ride.Driver.UserId,
                    RideId = rideId,
                    ReportStatus = Models.Enums.ReportStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Reports.Add(report);
            }
            else 
            {
                throw new ForbidException("User is not part of a ride.");
            }

            _dbContext.SaveChanges();
        }

        public ReportDto Get(int rideId)
        {
            var report = GetReportByRideId(rideId);

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

        public void Delete(int rideId)
        {
            var report = GetReportByRideId(rideId);

            _dbContext.Reports.Remove(report);
            _dbContext.SaveChanges();
        }

        public void Update(int rideId, CreateReportDto dto)
        {
            var report = GetReportByRideId(rideId);

            report.Reason = dto.Reason;

            _dbContext.SaveChanges();
        }

        private Report GetReportByRideId(int rideId)
        {
            var userId = _userContextService.GetUserId;

            var report = _dbContext.Reports
                .FirstOrDefault(r => r.RideId == rideId && r.ReporterUserId == userId);

            if (report == null)
            {
                throw new NotFoundException("Report not found");
            }

            return report;
        }
    }

}
