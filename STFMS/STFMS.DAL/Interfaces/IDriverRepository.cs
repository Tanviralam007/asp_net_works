using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
        Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status);
        Task<Driver?> GetDriverByUserIdAsync(int userId);
        Task<Driver?> GetDriverByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Driver>> GetTopRatedDriversAsync(int count);
        Task<Driver?> GetDriverWithVehiclesAsync(int driverId);
        Task<Driver?> GetDriverWithBookingsAsync(int driverId);
        Task<IEnumerable<Driver>> GetDriversWithLowRatingAsync(decimal ratingThreshold);
        Task UpdateDriverRatingAsync(int driverId, decimal newRating);
        Task IncrementTotalRidesAsync(int driverId);
        Task UpdateDriverStatusAsync(int driverId, DriverStatus status);
    }
}
