namespace WrocRide.API.Seeders
{
    public class ReportsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Reports.Any())
            {
                return;
            }

            var sampleRides = dbContext.Rides
                .Include(r => r.Driver)
                .Include(r => r.Client)
                .ToList();

            var sampleAdmins = dbContext.Admins.ToList();

            var faker = new Faker<Report>("pl")
                .RuleFor(r => r.Reason, f => f.Lorem.Sentence(40))
                .RuleFor(r => r.ReportStatus, f => f.PickRandomWithout<ReportStatus>(ReportStatus.Accepted))
                .RuleFor(r => r.AdminId, f => f.PickRandom(sampleAdmins).Id);


            var reports = new List<Report>();

            foreach(var ride in sampleRides)
            {
                var report = faker.Generate();

                if (ride.EndDate.HasValue)
                {
                    report.CreatedAt = ride.EndDate.Value.AddMinutes(5);
                }
                else
                {
                    report.CreatedAt = DateTime.Now;
                }

                report.RideId = ride.Id;

                bool isReporterClient = RandomBoolean();

                if (isReporterClient)
                {
                    report.ReporterUserId = ride.Client.UserId;
                    report.ReportedUserId = ride.Driver.UserId;
                }
                else
                {
                    report.ReporterUserId = ride.Driver.UserId;
                    report.ReportedUserId = ride.Client.UserId;
                }

                if(report.ReportStatus == ReportStatus.Pending)
                {
                    report.AdminId = null;
                }

                reports.Add(report);
            }

            dbContext.Reports.AddRange(reports);
            dbContext.SaveChanges();
        }

        private static bool RandomBoolean()
        {
            Random rand = new Random();

            return rand.Next(2) == 0;
        }
    }
}
