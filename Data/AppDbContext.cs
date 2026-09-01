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

        public DbSet<KeyService> KeyServices => Set<KeyService>();
        public DbSet<KeyServiceTranslation> KeyServiceTranslations => Set<KeyServiceTranslation>();
        public DbSet<ProcessStep> ProcessSteps => Set<ProcessStep>();
        public DbSet<ProcessStepTranslation> ProcessStepTranslations => Set<ProcessStepTranslation>();
        public DbSet<DetailedService> DetailedServices => Set<DetailedService>();
        public DbSet<DetailedServiceTranslation> DetailedServiceTranslations => Set<DetailedServiceTranslation>();
        public DbSet<DetailedServiceHighlight> DetailedServiceHighlights => Set<DetailedServiceHighlight>();
        public DbSet<DetailedServiceHighlightTranslation> DetailedServiceHighlightTranslations => Set<DetailedServiceHighlightTranslation>();
        public DbSet<PricingPackage> PricingPackages => Set<PricingPackage>();
        public DbSet<PricingPackageTranslation> PricingPackageTranslations => Set<PricingPackageTranslation>();
        public DbSet<PricingPackageFeature> PricingPackageFeatures => Set<PricingPackageFeature>();
        public DbSet<PricingPackageFeatureTranslation> PricingPackageFeatureTranslations => Set<PricingPackageFeatureTranslation>();
        public DbSet<CustomerReview> CustomerReviews => Set<CustomerReview>();
        public DbSet<CustomerReviewTranslation> CustomerReviewTranslations => Set<CustomerReviewTranslation>();
        public DbSet<FormServiceOption> FormServiceOptions => Set<FormServiceOption>();
        public DbSet<FormServiceOptionTranslation> FormServiceOptionTranslations => Set<FormServiceOptionTranslation>();

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

            // 1. Key Services
            modelBuilder.Entity<KeyService>(e => { e.ToTable("key_services"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<KeyServiceTranslation>(e =>
            {
                e.ToTable("key_service_translations");
                e.HasIndex(t => new { t.KeyServiceId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.KeyService).WithMany(k => k.Translations).HasForeignKey(t => t.KeyServiceId).OnDelete(DeleteBehavior.Cascade);
            });

            // 2. Process Steps
            modelBuilder.Entity<ProcessStep>(e => { e.ToTable("process_steps"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<ProcessStepTranslation>(e =>
            {
                e.ToTable("process_step_translations");
                e.HasIndex(t => new { t.ProcessStepId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.ProcessStep).WithMany(s => s.Translations).HasForeignKey(t => t.ProcessStepId).OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Detailed Services
            modelBuilder.Entity<DetailedService>(e => { e.ToTable("detailed_services"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<DetailedServiceTranslation>(e =>
            {
                e.ToTable("detailed_service_translations");
                e.HasIndex(t => new { t.DetailedServiceId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.DetailedService).WithMany(s => s.Translations).HasForeignKey(t => t.DetailedServiceId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<DetailedServiceHighlight>(e => { e.ToTable("detailed_service_highlights"); });
            modelBuilder.Entity<DetailedServiceHighlightTranslation>(e =>
            {
                e.ToTable("detailed_service_highlight_translations");
                e.HasIndex(t => new { t.HighlightId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.Highlight).WithMany(h => h.Translations).HasForeignKey(t => t.HighlightId).OnDelete(DeleteBehavior.Cascade);
            });

            // 4. Pricing Packages
            modelBuilder.Entity<PricingPackage>(e => { e.ToTable("pricing_packages"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<PricingPackageTranslation>(e =>
            {
                e.ToTable("pricing_package_translations");
                e.HasIndex(t => new { t.PricingPackageId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.PricingPackage).WithMany(p => p.Translations).HasForeignKey(t => t.PricingPackageId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<PricingPackageFeature>(e => { e.ToTable("pricing_package_features"); });
            modelBuilder.Entity<PricingPackageFeatureTranslation>(e =>
            {
                e.ToTable("pricing_package_feature_translations");
                e.HasIndex(t => new { t.FeatureId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.Feature).WithMany(f => f.Translations).HasForeignKey(t => t.FeatureId).OnDelete(DeleteBehavior.Cascade);
            });

            // 5. Customer Reviews
            modelBuilder.Entity<CustomerReview>(e => { e.ToTable("customer_reviews"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<CustomerReviewTranslation>(e =>
            {
                e.ToTable("customer_review_translations");
                e.HasIndex(t => new { t.ReviewId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.Review).WithMany(r => r.Translations).HasForeignKey(t => t.ReviewId).OnDelete(DeleteBehavior.Cascade);
            });

            // 6. Form Services
            modelBuilder.Entity<FormServiceOption>(e => { e.ToTable("form_service_options"); e.Property(p => p.Id).HasColumnName("id"); });
            modelBuilder.Entity<FormServiceOptionTranslation>(e =>
            {
                e.ToTable("form_service_option_translations");
                e.HasIndex(t => new { t.OptionId, t.LanguageCode }).IsUnique();
                e.HasOne(t => t.Option).WithMany(o => o.Translations).HasForeignKey(t => t.OptionId).OnDelete(DeleteBehavior.Cascade);
            });

        }

    }
}
