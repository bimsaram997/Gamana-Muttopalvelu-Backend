namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class OfferResponseDto
    {
        public Guid OfferId { get; set; }
        public Guid? UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public DateTime DesiredMovingDate { get; set; }
        public int TotalAddresses { get; set; }

        public List<int> ServiceIds { get; set; } = new();
        public string? AdditionalInfo { get; set; }
        public bool PrivacyAgreed { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class OfferDetailResponseDto
    {
        public Guid OfferId { get; set; }
        public Guid? UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public DateTime DesiredMovingDate { get; set; }
        public int TotalAddresses { get; set; }

        public List<int> ServiceIds { get; set; } = new();
        public string? AdditionalInfo { get; set; }
        public bool PrivacyAgreed { get; set; }

        public DateTime CreatedAt { get; set; }

        public AddressDto? DepartureAddress { get; set; } = new();
        public AddressDto? DestinationAddress { get; set; }
        public RouteResultDto routeResultDto { get; set; }
    }
}
