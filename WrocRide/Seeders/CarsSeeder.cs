using Bogus;
using Bogus.Extensions.UnitedKingdom;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;

namespace WrocRide.Seeders
{
    public class CarsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Cars.Any())
            {
                return;
            }

            var driverUsersCount = dbContext.Users
                .Include(u => u.Role)
                .Count(u => u.Role.Name == "Driver");

            var faker = new Faker<Car>()
                .RuleFor(c => c.Brand, f => f.Vehicle.Manufacturer())
                .RuleFor(c => c.Model, f => f.Vehicle.Model())
                .RuleFor(c => c.BodyColor, f => f.Commerce.Color())
                .RuleFor(c => c.YearProduced, f => f.Random.Int(2000, 2025))
                .RuleFor(c => c.LicensePlate, f => f.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ") + f.Random.Int(10000, 99999).ToString());

            var cars = faker.Generate(driverUsersCount);

            dbContext.Cars.AddRange(cars);
            dbContext.SaveChanges();
        }
    }
}
