namespace WrocRide.API.Seeders
{
    public class RidesSeeder
    {
        public static void Seed(WrocRideDbContext dbContext, int ridesCount)
        {
            if(dbContext.Rides.Any())
            {
                return;
            }

            var sampleDrivers = dbContext.Drivers.ToList();
            var sampleClients = dbContext.Clients.ToList();

            var faker = new Faker<Ride>("pl")
                .RuleFor(r => r.Coast, f => f.Random.Decimal(10, 1000))
                .RuleFor(r => r.PickUpLocation, f => f.Address.FullAddress())
                .RuleFor(r => r.Destination, f => f.Address.FullAddress())
                .RuleFor(r => r.Distance, f => f.Random.Decimal(1, 200))
                .RuleFor(r => r.RideStatus, f => f.PickRandomWithout<RideStatus>(RideStatus.Reserved, RideStatus.Ongoing, RideStatus.Accepted))
                .RuleFor(r => r.ClientId, f => f.PickRandom(sampleClients).Id)
                .RuleFor(r => r.DriverId, f => f.PickRandom(sampleDrivers).Id)
                .RuleFor(r => r.StartDate, f => f.Date.Past());

            var rides = faker.Generate(ridesCount)
                .Select(r =>
                {
                    if (r.RideStatus == RideStatus.Canceled || r.RideStatus == RideStatus.Ended)
                    {
                        r.EndDate = r.StartDate.AddMinutes(30);
                    }

                    return r;
                });

            dbContext.Rides.AddRange(rides);
            dbContext.SaveChanges();
        }
    }
}
