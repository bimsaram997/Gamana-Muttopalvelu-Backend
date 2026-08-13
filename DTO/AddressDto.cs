namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class AddressDto
    {
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
