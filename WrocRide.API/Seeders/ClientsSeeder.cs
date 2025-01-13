namespace WrocRide.API.Seeders
{
    public class ClientsSeeder
    {
        public static void Seed(WrocRideDbContext dbContext)
        {
            if(dbContext.Clients.Any())
            {
                return;
            }

            var sampleClientUsers = dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "Client");

            var clients = sampleClientUsers.Select(u => new Client
            {
                UserId = u.Id
            });

            dbContext.Clients.AddRange(clients);
            dbContext.SaveChanges();
        }
    }
}
