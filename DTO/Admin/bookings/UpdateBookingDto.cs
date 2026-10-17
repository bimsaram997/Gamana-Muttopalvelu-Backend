namespace Gamana_Muttopalvelu_Backend.DTO.Admin.bookings
{
    public class UpdateBookingDto
    {
        public string Status { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public int SelectedPackageId { get; set; }
        public int EstimatedHours { get; set; }
        public bool IncludeCleaning { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }

        // User Info
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Addresses
        public List<AddressDto> PickupLocations { get; set; } = new();
        public AddressDto? DropoffLocation { get; set; }
        public string? AdminNotes { get; set; }
    }
}
