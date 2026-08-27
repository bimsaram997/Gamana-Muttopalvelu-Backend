namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class CreateOfferDto
    {

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DesiredMovingDate { get; set; }
        public AddressDto DepartureAddress { get; set; } = new();
        public AddressDto DestinationAddress { get; set; } = new();
        public List<int> ServiceIds { get; set; } = new();
        public string? AdditionalInfo { get; set; }
        public bool PrivacyAgreed { get; set; }

    }
}
