using STFMS.BLL.Interfaces.Services;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // curd
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (await EmailExistsAsync(user.Email))
            {
                throw new InvalidOperationException($"Email '{user.Email}' is already registered.");
            }

            if (await PhoneExistsAsync(user.PhoneNumber))
            {
                throw new InvalidOperationException($"Phone number '{user.PhoneNumber}' is already registered.");
            }

            user.RegisteredDate = DateTime.UtcNow;
            user.IsActive = true;

            // password hash korum pore- apatoto development only

            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {user.UserId} not found.");
            }

            // check if email is being changed and if new email already exists
            if (existingUser.Email != user.Email && await EmailExistsAsync(user.Email))
            {
                throw new InvalidOperationException($"Email '{user.Email}' is already registered.");
            }

            // check if phone is being changed and if new phone already exists
            if (existingUser.PhoneNumber != user.PhoneNumber && await PhoneExistsAsync(user.PhoneNumber))
            {
                throw new InvalidOperationException($"Phone number '{user.PhoneNumber}' is already registered.");
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // don't allow deletion of users with active bookings
            var userWithBookings = await _userRepository.GetUserWithBookingsAsync(userId);
            if (userWithBookings?.Bookings.Any(b => b.Status == BookingStatus.InProgress || b.Status == BookingStatus.Assigned) == true)
            {
                throw new InvalidOperationException("Cannot delete user with active bookings.");
            }

            await _userRepository.DeleteAsync(userId);
        }

        // basic auth and validation
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            // password hash korum pore- apatoto development only
            if (user.PasswordHash == password)
            {
                return user;
            }

            return null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> PhoneExistsAsync(string phoneNumber)
        {
            return await _userRepository.PhoneExistsAsync(phoneNumber);
        }

        // user management
        public async Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType)
        {
            return await _userRepository.GetUsersByTypeAsync(userType);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _userRepository.GetActiveUsersAsync();
        }

        public async Task<User?> GetUserWithBookingsAsync(int userId)
        {
            return await _userRepository.GetUserWithBookingsAsync(userId);
        }

        public async Task<User?> GetUserWithDriverDetailsAsync(int userId)
        {
            return await _userRepository.GetUserWithDriverDetailsAsync(userId);
        }

        public async Task DeactivateUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }

        public async Task ActivateUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync()
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(x => !x.IsActive);
        }

        public async Task<User?> GetUserWithDriverAsync(int userId)
        {
            return await GetUserWithDriverDetailsAsync(userId);
        }

        // to observe stat
        public async Task<int> GetTotalCustomersCountAsync()
        {
            return await _userRepository.GetTotalCustomersCountAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _userRepository.CountAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _userRepository.GetAllAsync();
            }
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
