using Gamana_Muttopalvelu_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IBookingRepository
    {

        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id);
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
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
