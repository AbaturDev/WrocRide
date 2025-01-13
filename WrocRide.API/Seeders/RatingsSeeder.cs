namespace WrocRide.API.Seeders
{
    public class RatingsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if (dbContext.Ratings.Any())
            {
                return;
            }

            var sampleEndedRides = dbContext.Rides
                .Where(r => r.RideStatus == RideStatus.Ended)
                .ToList();

            var faker = new Faker<Rating>("pl")
                .RuleFor(r => r.Grade, f => f.Random.Int(1, 5))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence(30));

            var ratings = new List<Rating>();

            foreach (var ride in sampleEndedRides)
            {
                var rating = faker.Generate();

                if (ride.EndDate.HasValue)
                {
                    rating.CreatedAt = ride.EndDate.Value.AddMinutes(30);
                }
                else
                {
                    rating.CreatedAt = DateTime.Now;
                }

                rating.RideId = ride.Id;
                rating.CreatedByClientId = ride.ClientId;

                ratings.Add(rating);
            }

            dbContext.Ratings.AddRange(ratings);
            dbContext.SaveChanges();

            var drivers = dbContext.Drivers.ToList();

            foreach (var driver in drivers)
            {
                var driverGrade = dbContext.Rides
                    .Include(r => r.Rating)
                    .Where(r => r.DriverId == driver.Id && r.Rating != null)
                    .Average(r => r.Rating.Grade);

                driver.Rating = (float)driverGrade;
            }

            dbContext.SaveChanges();
        }
    }
}
