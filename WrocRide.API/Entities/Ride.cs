﻿namespace WrocRide.API.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public decimal Coast { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PickUpLocation { get; set; }
        public string Destination { get; set; }
        public decimal Distance { get; set; }
        public RideStatus RideStatus { get; set; }
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual Rating Rating { get; set; }
    }
}
