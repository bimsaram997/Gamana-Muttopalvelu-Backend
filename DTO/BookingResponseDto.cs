namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class BookingResponseDto
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalAddresses { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
