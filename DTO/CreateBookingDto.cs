namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class CreateBookingDto
    {
        public int SelectedPackageId { get; set; }
        public int EstimatedHours { get; set; }
        public bool IncludeCleaning { get; set; }

        public List<AddressDto> PickupLocations { get; set; } = new();
        public AddressDto DropoffLocation { get; set; } = new();

        public string? Notes { get; set; }
        public DateTime ServiceDate { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
    }
}
