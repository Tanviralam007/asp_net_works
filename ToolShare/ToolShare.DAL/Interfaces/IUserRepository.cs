using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(byte role);
        Task<IEnumerable<User>> GetBlockedUsersAsync();
        Task<IEnumerable<User>> GetUsersByLocationAsync(string location);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<int> GetTotalUsersCountAsync();
    }
}
