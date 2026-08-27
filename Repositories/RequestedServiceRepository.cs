using Gamana_Muttopalvelu_Backend.Data;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IRequestedServiceRepository
    {
        Task AddRangeAsync(IEnumerable<RequestedService> requestedServices);
    }

    public class RequestedServiceRepository : IRequestedServiceRepository
    {
        private readonly AppDbContext _context;

        public RequestedServiceRepository(AppDbContext context) => _context = context;

        public async Task AddRangeAsync(IEnumerable<RequestedService> requestedServices)
            => await _context.RequestedServices.AddRangeAsync(requestedServices);
    }
}
