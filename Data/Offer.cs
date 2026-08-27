namespace Gamana_Muttopalvelu_Backend.Data
{
    public class Offer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public DateTime DesiredMovingDate { get; set; }

        // Unified collection for Departure and Destination addresses
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public ICollection<RequestedService> RequestedServices { get; set; } = new List<RequestedService>();
        public string? AdditionalInfo { get; set; }
        public bool PrivacyAgreed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
