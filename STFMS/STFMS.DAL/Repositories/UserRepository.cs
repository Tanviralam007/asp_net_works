using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) 
            : base(context)
        {

        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.RegisteredDate)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }

        public async Task<int> GetTotalCustomersCountAsync()
        {
            return await _dbSet.CountAsync(u => u.UserType == UserType.Customer);
        }

        public async Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType)
        {
            return await _dbSet
                .Where(u => u.UserType == userType)
                .OrderByDescending (u => u.RegisteredDate)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithBookingsAsync(int userId) /////////// -> UN-ref
        {
            return await _dbSet
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Driver)
                .ThenInclude(d => d!.User)
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Vehicle)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserWithDriverDetailsAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.Driver)
                .FirstOrDefaultAsync (u => u.UserId == userId);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _dbSet.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
