namespace Gamana_Muttopalvelu_Backend.DTO.Filters
{
    public class BookingQueryParameters
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public DateTime? ServiceDate { get; set; }
        public int? SelectedPackageId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
