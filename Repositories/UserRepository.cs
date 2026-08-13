using Gamana_Muttopalvelu_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);
    }
}
