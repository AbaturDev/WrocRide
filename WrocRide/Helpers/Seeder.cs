using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WrocRide.Entities;
using DayOfWeek = WrocRide.Entities.DayOfWeek;

namespace WrocRide.Seeders
{
    public static class Seeder
    {
        private const int clientCount = 75;
        private const int adminCount = 5;
        private const int driverCount = 20;
        private const int ridesCount = 500;

        public static WebApplication Seed(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<WrocRideDbContext>();

                try
                {
                    context.Database.EnsureCreated();

                    RolesSeeder.Seed(context);
                    UsersSeeder.Seed(context, clientCount, adminCount, driverCount);
                    ClientsSeeder.Seed(context);
                    AdminsSeeder.Seed(context);

                    DocumentsSeeder.Seed(context);
                    CarsSeeder.Seed(context);
                    DriversSeeder.Seed(context);

                    RidesSeeder.Seed(context, ridesCount);
                    RatingsSeeder.Seed(context);

                    DayOfWeeksSeeder.Seed(context);
                    //schedule
                    //scheduleDay

                }
                catch(Exception)
                {
                    throw;
                }

            }

            return app;
        }

    }
}
