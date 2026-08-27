using Gamana_Muttopalvelu_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IOfferRepository
    {

        Task AddAsync(Offer offer);
        Task<Offer?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
    public class OfferRepository: IOfferRepository
    {
        private readonly AppDbContext _context;

        public OfferRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Offer offer)
          => await _context.Offers.AddAsync(offer);

        public async Task<Offer?> GetByIdAsync(Guid id)
        {
            return await _context.Offers
                .Include(b => b.User)
                .Include(b => b.Addresses)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
