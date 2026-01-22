using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phoneNumber);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<int> GetTotalCustomersCountAsync();
        Task<User?> GetUserWithDriverDetailsAsync(int userId);
        Task<User?> GetUserWithBookingsAsync(int userId);
    }
}
