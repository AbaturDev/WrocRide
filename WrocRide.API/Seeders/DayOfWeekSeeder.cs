using DayOfWeek = WrocRide.API.Entities.DayOfWeek;

namespace WrocRide.API.Seeders
{
    public class DayOfWeekSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if (dbContext.DayOfWeeks.Any())
            {
                return;
            }

            var days = new List<DayOfWeek>()
            {
                new DayOfWeek()
                {
                    Day = "Monday"
                },
                new DayOfWeek()
                {
                    Day = "Tuesday"
                },
                new DayOfWeek()
                {
                    Day = "Wednesday"
                },
                new DayOfWeek()
                {
                    Day = "Thursday"
                },
                new DayOfWeek()
                {
                    Day = "Friday"
                },
                new DayOfWeek()
                {
                    Day = "Saturday"
                },
                new DayOfWeek()
                {
                    Day = "Sunday"
                },

            };

            dbContext.DayOfWeeks.AddRange(days);
            dbContext.SaveChanges();
        }      
    }
}
