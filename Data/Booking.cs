using System.Net;

namespace Gamana_Muttopalvelu_Backend.Data
{
    public class Booking
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int SelectedPackageId { get; set; }
        public int EstimatedHours { get; set; }
        public bool IncludeCleaning { get; set; }

        // Unified collection storing all Pickup and Dropoff addresses for this booking
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        public string? Notes { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
