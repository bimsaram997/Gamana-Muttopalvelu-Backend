namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class ReviewTranslationDto
    {
        public string LanguageCode { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string DateDisplay { get; set; } = string.Empty;
        public string ServiceUsed { get; set; } = string.Empty;
    }

    public class AdminReviewUpsertDto
    {
        public string Author { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public bool IsActive { get; set; } = true;
        public List<ReviewTranslationDto> Translations { get; set; } = new();
    }

    public class AdminReviewResponseDto
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsActive { get; set; }
        public List<ReviewTranslationDto> Translations { get; set; } = new();
    }
}
