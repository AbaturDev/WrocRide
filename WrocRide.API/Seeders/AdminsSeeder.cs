namespace WrocRide.API.Seeders
{
    public class AdminsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Admins.Any())
            {
                return;
            }

            var sampleAdminUsers = dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "Admin");

            var admins = sampleAdminUsers.Select(u => new Admin
            {
                UserId = u.Id
            });

            dbContext.Admins.AddRange(admins);
            dbContext.SaveChanges();
        }
    }
}
