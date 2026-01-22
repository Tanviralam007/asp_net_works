using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IUserService
    {
        // curd signatures
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);

        // basic auth and validation signatures
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> ValidateUserCredentialsAsync(string email, string password);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phoneNumber);


        // user management signatures
        Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetInactiveUsersAsync();
        Task<User?> GetUserWithBookingsAsync(int userId);
        Task<User?> GetUserWithDriverDetailsAsync(int userId);
        Task<User?> GetUserWithDriverAsync(int userId);
        Task DeactivateUserAsync(int userId);
        Task ActivateUserAsync(int userId);

        // to observe statisitics and search signatures
        Task<int> GetTotalCustomersCountAsync();
        Task<int> GetTotalUsersCountAsync();
        Task<IEnumerable<User>> SearchUsersByNameAsync(string name);
    }
}
