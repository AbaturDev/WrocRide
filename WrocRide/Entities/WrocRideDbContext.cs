using Microsoft.EntityFrameworkCore;

namespace WrocRide.Entities
{
    public class WrocRideDbContext : DbContext
    {
        public WrocRideDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<DayOfWeek> DayOfWeeks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ride>()
                .HasOne(r => r.Driver)
                .WithMany(d => d.Rides)
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Driver>()
                .HasOne(r => r.Document)
                .WithOne()
                .HasForeignKey<Driver>(r => r.DocumentId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Ride>()
                .Property(u => u.Coast)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Driver>()
                .Property(u => u.Pricing)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Schedule>()
                .Property(u => u.BudgetPerRide)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Schedule>()
                .Property(u => u.Distance)
                .HasPrecision(8, 3);
            
            modelBuilder.Entity<Ride>()
                .Property(u => u.Distance)
                .HasPrecision(8, 3);

            base.OnModelCreating(modelBuilder);
        }
    }
}
 