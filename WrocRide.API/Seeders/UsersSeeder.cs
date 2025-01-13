namespace WrocRide.API.Seeders
{
    public class UsersSeeder
    {
        public static void Seed(WrocRideDbContext dbContext, int clientCount, int adminCount, int driverCount)
        {
            if(dbContext.Users.Any())
            {
                return;
            }

            var clientRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Client");
            var driverRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Driver");
            var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");

            if(clientRole == null || driverRole == null || adminRole == null)
            {
                throw new Exception("Roles must be seeded before users");
            }

            var passwordHasher = new PasswordHasher<User>();
            var faker = new Faker<User>("pl")
                .RuleFor(u => u.Name, f => f.Name.FirstName())
                .RuleFor(u => u.Surename, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.IsActive, f => true)
                .RuleFor(u => u.JoinAt, f => f.Date.Past())
                .RuleFor(u => u.Balance, f => f.Random.Decimal(0, 10000));

            var users = new List<User>();

            var clients = faker.Generate(clientCount)
                .Select(u =>
                {
                    u.RoleId = clientRole.Id;
                    u.PasswordHash = passwordHasher.HashPassword(u, "haslohaslo");
                    return u;
                });

            users.AddRange(clients);

            var drivers = faker.Generate(driverCount)
                .Select(u =>
                {
                    u.RoleId = driverRole.Id;
                    u.PasswordHash = passwordHasher.HashPassword(u, "haslohaslo");
                    return u;
                });

            users.AddRange(drivers);

            var admins = faker.Generate(adminCount)
                .Select(u =>
                {
                    u.RoleId = adminRole.Id;
                    u.PasswordHash = passwordHasher.HashPassword(u, "haslohaslo");
                    return u;
                });

            users.AddRange(admins);

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}
