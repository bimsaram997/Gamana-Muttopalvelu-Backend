namespace Gamana_Muttopalvelu_Backend.DTO.Filters
{
    public class PackageQueryParameters
    {
        public string? Title { get; set; }
        public string? Status { get; set; }
  
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
