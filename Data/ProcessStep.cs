namespace Gamana_Muttopalvelu_Backend.Data
{
    // --- 2. How It Works Steps ---
    public class ProcessStep
    {
        public int Id { get; set; }
        public string StepNumber { get; set; } = string.Empty; // "01", "02"
        public string Icon { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<ProcessStepTranslation> Translations { get; set; } = new List<ProcessStepTranslation>();
    }

    public class ProcessStepTranslation
    {
        public int Id { get; set; }
        public int ProcessStepId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProcessStep ProcessStep { get; set; } = null!;
    }
}
