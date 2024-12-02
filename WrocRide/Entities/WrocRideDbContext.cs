using Microsoft.EntityFrameworkCore;

namespace WrocRide.Entities
{
    public class WrocRideDbContext : DbContext
    {
        public WrocRideDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<User> Users { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<Document> Documents { get; set; }
        DbSet<Driver> Drivers { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<Ride> Rides { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ride>()
                .HasOne(r => r.Driver)
                .WithMany()
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
                .HasOne(r => r.Client)
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
