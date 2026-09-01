namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class KeyServiceTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class AdminKeyServiceUpsertDto
    {
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<KeyServiceTranslationDto> Translations { get; set; } = new();
    }

    public class AdminKeyServiceResponseDto
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<KeyServiceTranslationDto> Translations { get; set; } = new();
    }
}
