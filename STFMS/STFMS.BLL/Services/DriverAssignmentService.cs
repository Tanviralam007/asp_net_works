using STFMS.BLL.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.BLL.Services
{
    public class DriverAssignmentService : IDriverAssignmentService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBookingRepository _bookingRepository;

        public DriverAssignmentService(
            IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IBookingRepository bookingRepository)
        {
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _bookingRepository = bookingRepository;
        }

        // automatic driver assignment
        public async Task<Driver?> AssignDriverToBookingAsync(int bookingId)
        {
            // Get booking details
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException("Only pending bookings can be assigned.");
            }

            // find best available driver
            var bestDriver = await FindBestAvailableDriverAsync(booking.PickupLocation);
            if (bestDriver == null)
            {
                return null; // No available drivers
            }

            // find suitable vehicle for driver based on service type
            var vehicle = await AssignVehicleToBookingAsync(bestDriver.DriverId, booking.ServiceType);
            if (vehicle == null)
            {
                return null; // No suitable vehicle available
            }

            // assign driver and vehicle to booking
            await _bookingRepository.AssignDriverAndVehicleAsync(bookingId, bestDriver.DriverId, vehicle.VehicleId);

            // update driver status to busy
            await _driverRepository.UpdateDriverStatusAsync(bestDriver.DriverId, DriverStatus.Busy);

            return bestDriver;
        }

        public async Task<Driver?> FindBestAvailableDriverAsync(string pickupLocation)
        {
            // get all available drivers
            var availableDrivers = await _driverRepository.GetAvailableDriversAsync();

            if (!availableDrivers.Any())
            {
                return null;
            }

            // priority-based selection algorithm:
            // 1. highest rated drivers
            // 2. balanced workload (fewer total rides preferred)
            // 3. drivers with active vehicles

            var driverScores = new List<(Driver Driver, double Score)>();

            foreach (var driver in availableDrivers)
            {
                // check if driver has at least one active vehicle
                var vehicles = await _vehicleRepository.GetVehiclesByDriverIdAsync(driver.DriverId);
                if (!vehicles.Any(v => v.Status == VehicleStatus.Active))
                {
                    continue; // Skip drivers without active vehicles
                }

                // calculate driver score
                double score = CalculateDriverScore(driver);
                driverScores.Add((driver, score));
            }

            // return driver with highest score
            return driverScores.OrderByDescending(ds => ds.Score)
                               .FirstOrDefault().Driver;
        }

        public async Task<IEnumerable<Driver>> FindNearbyDriversAsync(string location, int maxDistance)
        {
            // get all available drivers
            var availableDrivers = await _driverRepository.GetAvailableDriversAsync();

            // In real scenario, geolocation API has to be used to calculate actual distance
            // For now, simulate proximity based on location string matching
            var nearbyDrivers = availableDrivers.Where(d =>
            {
                // simulate proximity check (placeholder logic)
                // in production, lat/long coordinates and distance are to be used for calculation
                return true; // for now, return all available drivers
            }).ToList();

            return nearbyDrivers;
        }

        // ASSIGNMENT LOGIC
        public async Task<Driver?> GetHighestRatedAvailableDriverAsync()
        {
            var availableDrivers = await _driverRepository.GetAvailableDriversAsync();

            if (!availableDrivers.Any())
            {
                return null;
            }

            // return driver with highest rating
            return availableDrivers.OrderByDescending(d => d.Rating)
                                   .ThenByDescending(d => d.TotalRides)
                                   .FirstOrDefault();
        }

        public async Task<bool> IsDriverAvailableForBookingAsync(int driverId, DateTime bookingTime)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null || driver.Status != DriverStatus.Available)
            {
                return false;
            }

            // check if driver has any conflicting bookings at the requested time
            var driverBookings = await _bookingRepository.GetBookingsByDriverIdAsync(driverId);

            var conflictingBookings = driverBookings.Where(b =>
                (b.Status == BookingStatus.Assigned || b.Status == BookingStatus.InProgress) &&
                b.BookingTime <= bookingTime.AddHours(2) &&
                (b.CompletionTime == null || b.CompletionTime >= bookingTime)
            );

            return !conflictingBookings.Any();
        }

        // vehicle assignment
        public async Task<Vehicle?> AssignVehicleToBookingAsync(int driverId, ServiceType serviceType)
        {
            // get driver's vehicles
            var driverVehicles = await _vehicleRepository.GetVehiclesByDriverIdAsync(driverId);

            // filter active vehicles
            var activeVehicles = driverVehicles.Where(v => v.Status == VehicleStatus.Active).ToList();

            if (activeVehicles.Count == 0)
            {
                return null;
            }

            // select vehicle based on service type preference
            Vehicle? selectedVehicle = serviceType switch
            {
                ServiceType.Corporate => activeVehicles.FirstOrDefault(v => v.VehicleType == VehicleType.Luxury)
                                        ?? activeVehicles.FirstOrDefault(v => v.VehicleType == VehicleType.Sedan),

                ServiceType.Parcel => activeVehicles.FirstOrDefault(v => v.VehicleType == VehicleType.Van)
                                     ?? activeVehicles.FirstOrDefault(),

                ServiceType.Ride => activeVehicles.FirstOrDefault(v => v.VehicleType == VehicleType.Sedan)
                                   ?? activeVehicles.FirstOrDefault(),

                _ => activeVehicles.FirstOrDefault()
            };

            return selectedVehicle;
        }

        public async Task<Vehicle?> GetAvailableVehicleForDriverAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            var vehicles = await _vehicleRepository.GetVehiclesByDriverIdAsync(driverId);
            return vehicles.FirstOrDefault(v => v.Status == VehicleStatus.Active);
        }

        // optimization
        public async Task<int> GetDriverWorkloadAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            // get active and assigned bookings for driver
            var driverBookings = await _bookingRepository.GetBookingsByDriverIdAsync(driverId);

            var activeWorkload = driverBookings.Count(b =>
                b.Status == BookingStatus.Assigned ||
                b.Status == BookingStatus.InProgress);

            return activeWorkload;
        }

        public async Task BalanceDriverWorkloadAsync()
        {
            // get all drivers
            var allDrivers = await _driverRepository.GetAllAsync();

            var driverWorkloads = new List<(Driver Driver, int Workload)>();

            foreach (var driver in allDrivers)
            {
                int workload = await GetDriverWorkloadAsync(driver.DriverId);
                driverWorkloads.Add((driver, workload));
            }

            // identify overloaded and underutilized drivers
            var averageWorkload = driverWorkloads.Count == 0 ? driverWorkloads.Average(dw => dw.Workload) : 0;

            var overloadedDrivers = driverWorkloads.Where(dw => dw.Workload > averageWorkload + 2).ToList();
            var underutilizedDrivers = driverWorkloads.Where(dw => dw.Workload < averageWorkload - 2).ToList();

            // In real scenario, load balancing logic is to be implemented
            // For now, just log the information
            Console.WriteLine($"Average workload: {averageWorkload}");
            Console.WriteLine($"Overloaded drivers: {overloadedDrivers.Count}");
            Console.WriteLine($"Underutilized drivers: {underutilizedDrivers.Count}");

            await Task.CompletedTask;
        }
        
        // HELPER METHODS
        private static double CalculateDriverScore(Driver driver)
        {
            // multi-factor scoring algorithm
            double score = 0;

            // factor 1: Rating (0-5) - weighted 50%
            double ratingScore = (double)driver.Rating * 10; // 0-50 points
            score += ratingScore;

            // factor 2: Experience (total rides) - weighted 30%
            double experienceScore = Math.Min(driver.TotalRides / 10.0, 30); // 0-30 points (capped at 300 rides)
            score += experienceScore;

            // factor 3: Availability factor - weighted 20%
            // prefer drivers with lower workload (fewer recent rides)
            double availabilityScore = 20 - Math.Min(driver.TotalRides % 20, 20); // 0-20 points
            score += availabilityScore;

            return score;
        }
    }
}
