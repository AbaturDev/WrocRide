namespace WrocRide.API.Seeders
{
    public class SchedulesSeeder
    {
        public static void Seed(WrocRideDbContext dbContext, int scheduleCount)
        {
            if(dbContext.Schedules.Any())
            {
                return;
            }

            var sampleClients = dbContext.Clients.ToList();

            var faker = new Faker<Schedule>()
                .RuleFor(s => s.ClientId, f => f.PickRandom(sampleClients).Id)
                .RuleFor(s => s.PickUpLocation, f => f.Address.FullAddress())
                .RuleFor(s => s.Destination, f => f.Address.FullAddress())
                .RuleFor(s => s.StartTime, f => f.Date.Recent().TimeOfDay)
                .RuleFor(s => s.CreatedAt, f => f.Date.Past())
                .RuleFor(s => s.Distance, f => f.Random.Decimal(1, 5))
                .RuleFor(s => s.BudgetPerRide, f => f.Random.Decimal(30, 200));

            var schedules = faker.Generate(scheduleCount);

            dbContext.Schedules.AddRange(schedules);
            dbContext.SaveChanges();
        }
    }
}
