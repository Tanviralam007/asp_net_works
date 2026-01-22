using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(byte role)
        {
            return await _dbSet
                .Where(u => (byte)u.Role == role)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetBlockedUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsBlocked)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByLocationAsync(string location)
        {
            return await _dbSet
                .Where(u => u.Location.Contains(location))
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            var query = _dbSet.Where(u => u.Email.ToLower() == email.ToLower());

            if (excludeUserId.HasValue)
                query = query.Where(u => u.Id != excludeUserId.Value);

            return !await query.AnyAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
