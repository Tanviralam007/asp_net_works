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
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        public DriverService(IDriverRepository driverRepository, IFeedbackRepository feedbackRepository)
        {
            _driverRepository = driverRepository;
            _feedbackRepository = feedbackRepository;
        }

        // curd 
        public async Task<Driver?> GetDriverByIdAsync(int driverId)
        {
            return await _driverRepository.GetByIdAsync(driverId);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _driverRepository.GetAllAsync();
        }

        public async Task<Driver> CreateDriverAsync(Driver driver)
        {
            // Business validation
            var existingDriver = await _driverRepository.GetDriverByLicenseNumberAsync(driver.LicenseNumber);
            if (existingDriver != null)
            {
                throw new InvalidOperationException($"License number '{driver.LicenseNumber}' is already registered.");
            }

            // Check if user already has a driver profile
            var existingDriverForUser = await _driverRepository.GetDriverByUserIdAsync(driver.UserId);
            if (existingDriverForUser != null)
            {
                throw new InvalidOperationException("User already has a driver profile.");
            }

            // Set default values
            driver.Rating = 0.00m;
            driver.TotalRides = 0;
            driver.Status = DriverStatus.Available;
            driver.JoinedDate = DateTime.UtcNow;

            var createdDriver = await _driverRepository.AddAsync(driver);

            return await _driverRepository.GetByIdAsync(createdDriver.DriverId) ?? createdDriver;
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            var existingDriver = await _driverRepository.GetByIdAsync(driver.DriverId);
            if (existingDriver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driver.DriverId} not found.");
            }

            // Check if license number is being changed and if new license already exists
            if (existingDriver.LicenseNumber != driver.LicenseNumber)
            {
                var driverWithLicense = await _driverRepository.GetDriverByLicenseNumberAsync(driver.LicenseNumber);
                if (driverWithLicense != null)
                {
                    throw new InvalidOperationException($"License number '{driver.LicenseNumber}' is already registered.");
                }
            }

            await _driverRepository.UpdateAsync(driver);
        }

        public async Task DeleteDriverAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            // Business rule: Don't allow deletion of drivers with active bookings
            var driverWithBookings = await _driverRepository.GetDriverWithBookingsAsync(driverId);
            if (driverWithBookings?.Bookings.Any(b => b.Status == BookingStatus.InProgress || b.Status == BookingStatus.Assigned) == true)
            {
                throw new InvalidOperationException("Cannot delete driver with active bookings.");
            }

            await _driverRepository.DeleteAsync(driverId);
        }

        // driver lookup
        public async Task<Driver?> GetDriverByUserIdAsync(int userId)
        {
            return await _driverRepository.GetDriverByUserIdAsync(userId);
        }

        public async Task<Driver?> GetDriverByLicenseNumberAsync(string licenseNumber)
        {
            return await _driverRepository.GetDriverByLicenseNumberAsync(licenseNumber);
        }

        public async Task<Driver?> GetDriverWithVehiclesAsync(int driverId)
        {
            return await _driverRepository.GetDriverWithVehiclesAsync(driverId);
        }

        public async Task<Driver?> GetDriverWithBookingsAsync(int driverId)
        {
            return await _driverRepository.GetDriverWithBookingsAsync(driverId);
        }

        // driver availability and status
        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync()
        {
            return await _driverRepository.GetAvailableDriversAsync();
        }

        public async Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status)
        {
            return await _driverRepository.GetDriversByStatusAsync(status);
        }

        public async Task UpdateDriverStatusAsync(int driverId, DriverStatus status)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            await _driverRepository.UpdateDriverStatusAsync(driverId, status);
        }

        public async Task SetDriverAvailableAsync(int driverId)
        {
            await UpdateDriverStatusAsync(driverId, DriverStatus.Available);
        }

        public async Task SetDriverBusyAsync(int driverId)
        {
            await UpdateDriverStatusAsync(driverId, DriverStatus.Busy);
        }

        public async Task SetDriverOfflineAsync(int driverId)
        {
            await UpdateDriverStatusAsync(driverId, DriverStatus.Offline);
        }

        // driver performance
        public async Task<IEnumerable<Driver>> GetTopRatedDriversAsync(int count)
        {
            return await _driverRepository.GetTopRatedDriversAsync(count);
        }

        public async Task<IEnumerable<Driver>> GetDriversWithLowRatingAsync(decimal ratingThreshold)
        {
            return await _driverRepository.GetDriversWithLowRatingAsync(ratingThreshold);
        }

        public async Task UpdateDriverRatingAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found");
            }

            var averageRating = await _feedbackRepository.GetAverageRatingForDriverAsync(driverId);

            if (averageRating > 0)
            {
                await _driverRepository.UpdateDriverRatingAsync(driverId, averageRating);
            }
        }

        public async Task IncrementDriverRidesAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            await _driverRepository.IncrementTotalRidesAsync(driverId);
        }

        // statistics
        public async Task<int> GetTotalDriversCountAsync()
        {
            return await _driverRepository.CountAsync();
        }

        public async Task<int> GetAvailableDriversCountAsync()
        {
            var availableDrivers = await _driverRepository.GetAvailableDriversAsync();
            return availableDrivers.Count();
        }
    }
}
