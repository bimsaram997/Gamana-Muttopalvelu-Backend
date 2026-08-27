using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<RequestedService> RequestedServices => Set<RequestedService>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // One-to-Many: Booking -> Addresses
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Addresses)
                .WithOne(a => a.Booking)
                .HasForeignKey(a => a.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Offer>(entity =>
            {
                // Optional relationship with User (allows guest offer requests)
                entity.HasOne(o => o.User)
                       .WithMany(u => u.Offers)
                       .HasForeignKey(o => o.UserId);

                // One-to-Many: Offer -> Addresses
                entity.HasMany(o => o.Addresses)
                      .WithOne(a => a.Offer)
                      .HasForeignKey(a => a.OfferId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-Many: Offer -> RequestedServices
                entity.HasMany(o => o.RequestedServices)
                      .WithOne(rs => rs.Offer)
                      .HasForeignKey(rs => rs.OfferId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }

    }
}
