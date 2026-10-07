

namespace Gamana_Muttopalvelu_Backend.DTO
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
    }
}
