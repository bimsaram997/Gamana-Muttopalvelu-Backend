namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class DetailedServiceTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class HighlightTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class AdminHighlightUpsertDto
    {
        public int DisplayOrder { get; set; } = 0;
        public List<HighlightTranslationDto> Translations { get; set; } = new();
    }

    public class AdminHighlightResponseDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public List<HighlightTranslationDto> Translations { get; set; } = new();
    }

    public class AdminDetailedServiceUpsertDto
    {
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<DetailedServiceTranslationDto> Translations { get; set; } = new();
        public List<AdminHighlightUpsertDto> Highlights { get; set; } = new();
    }

    public class AdminDetailedServiceResponseDto
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<DetailedServiceTranslationDto> Translations { get; set; } = new();
        public List<AdminHighlightResponseDto> Highlights { get; set; } = new();
    }
}
