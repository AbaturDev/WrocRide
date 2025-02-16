﻿using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Report;
public sealed record ReportDto
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public ReportStatus ReportStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ReporterUserId { get; set; }
    public int ReportedUserId { get; set; }
    public int RideId { get; set; }
    public int? AdminId { get; set; }
}
