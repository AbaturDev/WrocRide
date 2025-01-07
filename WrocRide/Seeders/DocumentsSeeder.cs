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

            var sampleAdmins = dbContext.Admins.ToList();

            var faker = new Faker<Document>()
                .RuleFor(d => d.DocumentStatus, f => Models.Enums.DocumentStatus.Accepted)
                .RuleFor(d => d.FileLocation, f => f.Internet.Url())
                .RuleFor(d => d.RequestDate, f => f.Date.Past())
                .RuleFor(d => d.AdminId, f => f.PickRandom(sampleAdmins).Id);

            var documents = faker.Generate(driverUsersCount)
                .Select(d => 
                {
                    d.ExaminationDate = d.RequestDate.AddHours(3);
                    return d;
                });

            dbContext.Documents.AddRange(documents);
            dbContext.SaveChanges();
        }
    }
}
