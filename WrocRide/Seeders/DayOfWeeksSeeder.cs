using WrocRide.Entities;
using DayOfWeek = WrocRide.Entities.DayOfWeek;

namespace WrocRide.Seeders
{
    public class DayOfWeeksSeeder
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
