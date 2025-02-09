﻿namespace WrocRide.API.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReporterUserId { get; set; }
        public virtual User ReporterUser { get; set; }
        public int ReportedUserId { get; set; }
        public virtual User ReportedUser { get; set; }
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }
        public int? AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
