using Gamana_Muttopalvelu_Backend.Enums;

namespace Gamana_Muttopalvelu_Backend.Data
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // Flag / Discriminator to identify if this is Pickup or Dropoff
        public AddressType Type { get; set; }

        public string Label { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Floor { get; set; }
        public bool HasElevator { get; set; }
    }
}
