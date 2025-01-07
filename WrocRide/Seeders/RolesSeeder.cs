using WrocRide.Entities;

namespace WrocRide.Seeders
{
    public class RolesSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Roles.Any())
            {
                return;
            }

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

            dbContext.Roles.AddRange(roles);
            dbContext.SaveChanges();
        }
    }
}
