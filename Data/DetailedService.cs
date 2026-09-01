namespace Gamana_Muttopalvelu_Backend.Data
{
    // --- 3. Main Detailed Services ---
    public class DetailedService
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<DetailedServiceTranslation> Translations { get; set; } = new List<DetailedServiceTranslation>();
        public ICollection<DetailedServiceHighlight> Highlights { get; set; } = new List<DetailedServiceHighlight>();
    }

    public class DetailedServiceTranslation
    {
        public int Id { get; set; }
        public int DetailedServiceId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DetailedService DetailedService { get; set; } = null!;
    }

    public class DetailedServiceHighlight
    {
        public int Id { get; set; }
        public int DetailedServiceId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public DetailedService DetailedService { get; set; } = null!;
        public ICollection<DetailedServiceHighlightTranslation> Translations { get; set; } = new List<DetailedServiceHighlightTranslation>();
    }

    public class DetailedServiceHighlightTranslation
    {
        public int Id { get; set; }
        public int HighlightId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DetailedServiceHighlight Highlight { get; set; } = null!;
    }
}
