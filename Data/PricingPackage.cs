namespace Gamana_Muttopalvelu_Backend.Data
{
    // --- 4. Pricing Packages ---
    public class PricingPackage
    {
        public int Id { get; set; }
        public decimal RatePerHour { get; set; }
        public bool IsPopular { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<PricingPackageTranslation> Translations { get; set; } = new List<PricingPackageTranslation>();
        public ICollection<PricingPackageFeature> Features { get; set; } = new List<PricingPackageFeature>();
    }

    public class PricingPackageTranslation
    {
        public int Id { get; set; }
        public int PricingPackageId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PriceDisplay { get; set; } = string.Empty; // "25€"
        public string Unit { get; set; } = string.Empty;         // "per hour" / "tunnissa"
        public string Description { get; set; } = string.Empty;
        public PricingPackage PricingPackage { get; set; } = null!;
    }

    public class PricingPackageFeature
    {
        public int Id { get; set; }
        public int PricingPackageId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public PricingPackage PricingPackage { get; set; } = null!;
        public ICollection<PricingPackageFeatureTranslation> Translations { get; set; } = new List<PricingPackageFeatureTranslation>();
    }

    public class PricingPackageFeatureTranslation
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public PricingPackageFeature Feature { get; set; } = null!;
    }
}
