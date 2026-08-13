using Gamana_Muttopalvelu_Backend.Data;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IAddressRepository
    {
        Task AddRangeAsync(IEnumerable<Address> addresses);
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context) => _context = context;

        public async Task AddRangeAsync(IEnumerable<Address> addresses)
            => await _context.Addresses.AddRangeAsync(addresses);
    }
}
