namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class BookingDetailResponseDto
    {
        public Guid BookingId { get; set; }
        public int SelectedPackageId { get; set; }
        public int EstimatedHours { get; set; }
        public bool IncludeCleaning { get; set; }
        public string? Notes { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // User Details
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Address Details
        public List<AddressDto> PickupLocations { get; set; } = new();
        public AddressDto? DropoffLocation { get; set; }


        public RouteResultDto routeResultDto { get; set; }
    }
}
