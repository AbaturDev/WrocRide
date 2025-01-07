using WrocRide.Entities;

namespace WrocRide.Seeders
{
    public class ScheduleDaySeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.ScheduleDays.Any())
            {
                return;
            }

            var sampleSchedules = dbContext.Schedules.ToList();
            var sampleDaysOfWeek = dbContext.DayOfWeeks.ToList();

            var rand = new Random();

            foreach(var schedule in sampleSchedules)
            {
                var randomDaysOfWeek = sampleDaysOfWeek.OrderBy(x => rand.Next()).Take(rand.Next(1, sampleDaysOfWeek.Count)).ToList();

                var scheduleDays = randomDaysOfWeek
                    .Select(sd => new ScheduleDay
                    {
                        ScheduleId = schedule.Id,
                        DayOfWeekId = sd.Id
                    });

                dbContext.ScheduleDays.AddRange(scheduleDays);
            }

            dbContext.SaveChanges();
        }
        
    }
}
