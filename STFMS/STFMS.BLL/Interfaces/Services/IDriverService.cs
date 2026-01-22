using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IDriverService
    {
        // curd signatures
        Task<Driver?> GetDriverByIdAsync(int driverId);
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<Driver> CreateDriverAsync(Driver driver);
        Task UpdateDriverAsync(Driver driver);
        Task DeleteDriverAsync(int driverId);

        // driver lookup signatures
        Task<Driver?> GetDriverByUserIdAsync(int userId);
        Task<Driver?> GetDriverByLicenseNumberAsync(string licenseNumber);
        Task<Driver?> GetDriverWithVehiclesAsync(int driverId);
        Task<Driver?> GetDriverWithBookingsAsync(int driverId);

        // driver availability and status signatures
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
        Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status);
        Task UpdateDriverStatusAsync(int driverId, DriverStatus status);
        Task SetDriverAvailableAsync(int driverId);
        Task SetDriverBusyAsync(int driverId);
        Task SetDriverOfflineAsync(int driverId);

        // driver performance signatures
        Task<IEnumerable<Driver>> GetTopRatedDriversAsync(int count);
        Task<IEnumerable<Driver>> GetDriversWithLowRatingAsync(decimal ratingThreshold);
        Task UpdateDriverRatingAsync(int driverId);
        Task IncrementDriverRidesAsync(int driverId);

        // statistics signaturess
        Task<int> GetTotalDriversCountAsync();
        Task<int> GetAvailableDriversCountAsync();
    }
}
