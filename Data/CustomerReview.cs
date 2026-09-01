namespace Gamana_Muttopalvelu_Backend.Data
{
    // --- 5. Customer Reviews ---
    public class CustomerReview
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public bool IsActive { get; set; } = true;
        public ICollection<CustomerReviewTranslation> Translations { get; set; } = new List<CustomerReviewTranslation>();
    }

    public class CustomerReviewTranslation
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string DateDisplay { get; set; } = string.Empty; // "Recent Move"
        public string ServiceUsed { get; set; } = string.Empty; // Text label
        public CustomerReview Review { get; set; } = null!;
    }

}
