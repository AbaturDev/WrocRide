using Bogus;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;
using WrocRide.Models.Enums;

namespace WrocRide.Seeders
{
    public class DriversSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Drivers.Any())
            {
                return;
            }

            var sampleDriverUsers = dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "Driver")
                .ToList();

            var sampleCars = dbContext.Cars.ToList();
            var sampleDocuments = dbContext.Documents.ToList();

            var faker = new Faker<Driver>()
                .RuleFor(d => d.Pricing, f => f.Random.Decimal(1, 50))
                .RuleFor(d => d.DriverStatus, f => f.PickRandomWithout<DriverStatus>(DriverStatus.UnderVerification, DriverStatus.Occupied));


            var drivers = new List<Driver>();

            for(int i=0; i<sampleDriverUsers.Count; i++)
            {
                var driver = faker.Generate();
                driver.UserId = sampleDriverUsers[i].Id;
                driver.CarId = sampleCars[i].Id;
                driver.DocumentId = sampleDocuments[i].Id;

                drivers.Add(driver);
            }

            //var driver = sampleDriverUsers.Select((user, index) =>
            //{
            //    var driver = faker.Generate();
            //    driver.UserId = index;
            //    driver.CarId = sampleCars[index].Id;
            //    driver.DocumentId = sampleDocuments[index].Id;
            //    return driver;
            //});

            dbContext.Drivers.AddRange(drivers);
            dbContext.SaveChanges();
        }

    }
}
