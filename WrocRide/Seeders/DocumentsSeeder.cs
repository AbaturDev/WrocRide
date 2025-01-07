using Bogus;
using Microsoft.EntityFrameworkCore;
using WrocRide.Entities;

namespace WrocRide.Seeders
{
    public class DocumentsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Documents.Any())
            {
                return;
            }

            var driverUsersCount = dbContext.Users
                .Include(u => u.Role)
                .Count(u => u.Role.Name == "Driver");

            var faker = new Faker<Document>()
                .RuleFor(d => d.DocumentStatus, f => Models.Enums.DocumentStatus.Accepted)
                .RuleFor(d => d.FileLocation, f => f.Internet.Url())
                .RuleFor(d => d.RequestDate, f => f.Date.Past());

            var documents = faker.Generate(driverUsersCount);

            dbContext.Documents.AddRange(documents);
            dbContext.SaveChanges();
        }
    }
}
