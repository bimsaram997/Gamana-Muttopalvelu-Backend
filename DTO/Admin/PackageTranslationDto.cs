namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class PackageTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PriceDisplay { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class FeatureTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class AdminFeatureUpsertDto
    {
        public int DisplayOrder { get; set; } = 0;
        public List<FeatureTranslationDto> Translations { get; set; } = new();
    }

    public class AdminFeatureResponseDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public List<FeatureTranslationDto> Translations { get; set; } = new();
    }

    public class AdminPackageUpsertDto
    {
        public decimal RatePerHour { get; set; }
        public bool IsPopular { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<PackageTranslationDto> Translations { get; set; } = new();
        public List<AdminFeatureUpsertDto> Features { get; set; } = new();
    }

    public class AdminPackageResponseDto
    {
        public int Id { get; set; }
        public decimal RatePerHour { get; set; }
        public bool IsPopular { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<PackageTranslationDto> Translations { get; set; } = new();
        public List<AdminFeatureResponseDto> Features { get; set; } = new();
    }
}
