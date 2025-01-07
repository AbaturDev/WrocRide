using Bogus;
using WrocRide.Entities;

namespace WrocRide.Seeders
{
    public class RatingsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Ratings.Any())
            {
                return;
            }

            var sampleEndedRides = dbContext.Rides
                .Where(r => r.RideStatus == Models.Enums.RideStatus.Ended)
                .ToList();

            var faker = new Faker<Rating>("pl")
                .RuleFor(r => r.Grade, f => f.Random.Int(1, 5))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence(30));

            var ratings = new List<Rating>();

            foreach(var ride in sampleEndedRides)
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
        }
    }
}
