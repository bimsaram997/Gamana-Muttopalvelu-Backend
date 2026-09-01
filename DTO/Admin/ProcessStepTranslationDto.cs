namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class ProcessStepTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class AdminProcessStepUpsertDto
    {
        public string StepNumber { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public List<ProcessStepTranslationDto> Translations { get; set; } = new();
    }

    public class AdminProcessStepResponseDto
    {
        public int Id { get; set; }
        public string StepNumber { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<ProcessStepTranslationDto> Translations { get; set; } = new();
    }
}
