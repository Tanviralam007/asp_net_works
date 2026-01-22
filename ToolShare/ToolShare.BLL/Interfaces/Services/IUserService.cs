using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(byte role);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> BlockUserAsync(int userId, int adminId);
        Task<bool> UnblockUserAsync(int userId, int adminId);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<int> GetTotalUsersCountAsync();
    }
}
