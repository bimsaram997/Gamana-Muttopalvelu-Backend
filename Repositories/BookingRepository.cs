using Gamana_Muttopalvelu_Backend.Data;
using Gamana_Muttopalvelu_Backend.DTO;
using Gamana_Muttopalvelu_Backend.DTO.Filters;
using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IBookingRepository
    {

        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id);
        Task<PagedResponse<Booking>> GetAllAsync(BookingQueryParameters queryParams);
        Task SaveChangesAsync();
    }
    public class BookingRepository: IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Booking booking)
          => await _context.Bookings.AddAsync(booking);

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Addresses)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<PagedResponse<Booking>> GetAllAsync(BookingQueryParameters queryParams)
        {
            var query = _context.Bookings.AsNoTracking();

            
             if (!string.IsNullOrWhiteSpace(queryParams.Status))
            {
                query = query.Where(b => b.Status == queryParams.Status);
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var term = queryParams.SearchTerm.ToLower();
                query = query.Where(b =>
                    (b.User != null && (b.User.FullName.ToLower().Contains(term) || b.User.Email.ToLower().Contains(term))) ||
                    b.User != null && (b.User.Phone.ToLower().Contains(term)));
            }

            if(queryParams.SelectedPackageId.HasValue)
            {
                query = query.Where(b => b.SelectedPackageId == queryParams.SelectedPackageId.Value);
            }

            if (queryParams.ServiceDate.HasValue)
            {
                var startDateUtc = DateTime.SpecifyKind(queryParams.ServiceDate.Value.Date, DateTimeKind.Utc);
                var endDateUtc = startDateUtc.AddDays(1);

                query = query.Where(b => b.ServiceDate >= startDateUtc && b.ServiceDate < endDateUtc);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(b => b.User)
                .Include(b => b.Addresses)
                .Include(b => b.SelectedPackage!)
                    .ThenInclude(p => p.Translations)
                .OrderByDescending(b => b.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResponse<Booking>
            {
                Data = items,
                TotalCount = totalCount,
            };
        }
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
