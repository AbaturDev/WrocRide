﻿using WrocRide.Models.Enums;

namespace WrocRide.Models
{
    public class DriverDto
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public float? Rating { get; set; }
        public decimal Pricing { get; set; }
        public DriverStatus DriverStatus { get; set; }
    }
}

