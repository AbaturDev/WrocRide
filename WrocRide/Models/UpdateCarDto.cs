using System.ComponentModel.DataAnnotations;

namespace WrocRide.Models
{
    public class UpdateCarDto
    {
        public string? LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? BodyColor { get; set; }
        public int? YearProduced { get; set; }
    }
}
