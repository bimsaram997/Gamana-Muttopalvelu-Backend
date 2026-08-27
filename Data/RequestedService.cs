namespace Gamana_Muttopalvelu_Backend.Data
{
    public class RequestedService
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Foreign Key & Navigation for Offer
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        // Service Details
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}
