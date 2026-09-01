namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class FormOptionTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class AdminFormOptionUpsertDto
    {
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<FormOptionTranslationDto> Translations { get; set; } = new();
    }

    public class AdminFormOptionResponseDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<FormOptionTranslationDto> Translations { get; set; } = new();
    }
}
