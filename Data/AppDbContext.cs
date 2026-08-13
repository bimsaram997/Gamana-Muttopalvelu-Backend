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


        }

    }
}
