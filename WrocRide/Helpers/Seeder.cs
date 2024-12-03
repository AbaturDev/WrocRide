using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WrocRide.Entities;

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
