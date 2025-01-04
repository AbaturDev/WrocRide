using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WrocRide.Entities;
using DayOfWeek = WrocRide.Entities.DayOfWeek;

namespace WrocRide.Seeders
{
    public static class Seeder
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<WrocRideDbContext>();

                try
                {
                    context.Database.EnsureCreated();

                    if(!context.Roles.Any())
                    {
                        var roles = new List<Role>()
                        {
                            new Role()
                            {
                                Name = "Client"
                            },
                            new Role()
                            {
                                Name = "Driver"
                            },
                            new Role()
                            {
                                Name = "Admin"
                            }
                        };

                        context.Roles.AddRange(roles);
                        context.SaveChanges();
                    }

                    if (!context.DayOfWeeks.Any())
                    {
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
                        
                        context.DayOfWeeks.AddRange(days);
                        context.SaveChanges();
                    }
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
