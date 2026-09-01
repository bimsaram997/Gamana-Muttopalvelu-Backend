namespace Gamana_Muttopalvelu_Backend.Data
{
    public class KeyService
    {
        public int Id { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<KeyServiceTranslation> Translations { get; set; } = new List<KeyServiceTranslation>();
    }

    public class KeyServiceTranslation
    {
        public int Id { get; set; }
        public int KeyServiceId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public KeyService KeyService { get; set; } = null!;
    }

   
}
