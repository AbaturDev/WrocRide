using WrocRide.Shared.Enums;

namespace WrocRide.Shared.DTOs.Driver;
public sealed record DriverDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surename { get; set; }
    public float? Rating { get; set; }
    public decimal Pricing { get; set; }
    public DriverStatus DriverStatus { get; set; }
    public int CarId { get; set; }
}

