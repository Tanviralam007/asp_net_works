using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepo.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(byte role)
        {
            return await _userRepo.GetUsersByRoleAsync(role);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email is required");

            if (!await _userRepo.IsEmailUniqueAsync(user.Email))
                throw new InvalidOperationException("Email already exists");

            user.CreatedAt = DateTime.Now;
            user.IsBlocked = false;

            return await _userRepo.AddAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");

            if (!await _userRepo.IsEmailUniqueAsync(user.Email, user.Id))
                throw new InvalidOperationException("Email already exists");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Location = user.Location;
            existingUser.PhoneNumber = user.PhoneNumber;

            await _userRepo.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _userRepo.DeleteAsync(id);
        }

        public async Task<bool> BlockUserAsync(int userId, int adminId)
        {
            var admin = await _userRepo.GetByIdAsync(adminId);
            if (admin == null || (byte)admin.Role != 2) // Admin role = 2
                throw new UnauthorizedAccessException("Only admins can block users");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.IsBlocked = true;
            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> UnblockUserAsync(int userId, int adminId)
        {
            var admin = await _userRepo.GetByIdAsync(adminId);
            if (admin == null || (byte)admin.Role != 2)
                throw new UnauthorizedAccessException("Only admins can unblock users");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.IsBlocked = false;
            await _userRepo.UpdateAsync(user);
            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            return await _userRepo.IsEmailUniqueAsync(email, excludeUserId);
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userRepo.GetTotalUsersCountAsync();
        }
    }
}
