namespace Gamana_Muttopalvelu_Backend.Data
{
    // --- 6. Form Option Services (Offer Request Wizard Dropdown) ---
    public class FormServiceOption
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<FormServiceOptionTranslation> Translations { get; set; } = new List<FormServiceOptionTranslation>();
    }

    public class FormServiceOptionTranslation
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public FormServiceOption Option { get; set; } = null!;
    }
}
