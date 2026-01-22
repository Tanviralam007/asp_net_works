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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository)
        {
            _bookingRepository = bookingRepository;
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
        }

        // curd
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetByIdAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            // Business validation
            if (string.IsNullOrWhiteSpace(booking.PickupLocation))
            {
                throw new ArgumentException("Pickup location is required.");
            }

            if (string.IsNullOrWhiteSpace(booking.DropoffLocation))
            {
                throw new ArgumentException("Dropoff location is required.");
            }

            // Set default values
            booking.BookingTime = DateTime.UtcNow;
            booking.Status = BookingStatus.Pending;
            booking.PickupTime = null;
            booking.CompletionTime = null;
            booking.ActualFare = null;

            // Calculate estimated fare if not provided
            if (booking.EstimatedFare <= 0)
            {
                booking.EstimatedFare = await CalculateFareAsync(
                    booking.PickupLocation,
                    booking.DropoffLocation,
                    booking.ServiceType);
            }

            return await _bookingRepository.AddAsync(booking);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(booking.BookingId);
            if (existingBooking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {booking.BookingId} not found.");
            }

            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            // Business rule: Only allow deletion of pending or cancelled bookings
            if (booking.Status == BookingStatus.InProgress || booking.Status == BookingStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete in-progress or completed bookings.");
            }

            await _bookingRepository.DeleteAsync(bookingId);
        }

        // booking lookup with details
        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepository.GetBookingsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(int driverId)
        {
            return await _bookingRepository.GetBookingsByDriverIdAsync(driverId);
        }

        // booking filtering
        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _bookingRepository.GetBookingsByStatusAsync(status);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByServiceTypeAsync(ServiceType serviceType)
        {
            return await _bookingRepository.GetBookingsByServiceTypeAsync(serviceType);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Booking>> GetPendingBookingsAsync()
        {
            return await _bookingRepository.GetPendingBookingsAsync();
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await _bookingRepository.GetActiveBookingsAsync();
        }

        public async Task<IEnumerable<Booking>> GetCompletedBookingsAsync()
        {
            return await _bookingRepository.GetCompletedBookingsAsync();
        }

        //booking workflow
        public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            await _bookingRepository.UpdateBookingStatusAsync(bookingId, status);
        }

        public async Task AssignDriverToBookingAsync(int bookingId, int driverId, int vehicleId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException("Only pending bookings can be assigned.");
            }

            // Validate driver exists and is available
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            if (driver.Status != DriverStatus.Available)
            {
                throw new InvalidOperationException("Driver is not available.");
            }

            // Validate vehicle exists and is active
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            if (vehicle.Status != VehicleStatus.Active)
            {
                throw new InvalidOperationException("Vehicle is not active.");
            }

            // Validate vehicle belongs to driver
            if (vehicle.DriverId != driverId)
            {
                throw new InvalidOperationException("Vehicle does not belong to the selected driver.");
            }

            // Assign driver and vehicle
            await _bookingRepository.AssignDriverAndVehicleAsync(bookingId, driverId, vehicleId);

            // Update driver status to busy
            await _driverRepository.UpdateDriverStatusAsync(driverId, DriverStatus.Busy);
        }

        public async Task StartRideAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status != BookingStatus.Assigned)
            {
                throw new InvalidOperationException("Only assigned bookings can be started.");
            }

            await _bookingRepository.UpdateBookingStatusAsync(bookingId, BookingStatus.InProgress);
        }

        public async Task CompleteRideAsync(int bookingId, decimal actualFare)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status != BookingStatus.InProgress)
            {
                throw new InvalidOperationException("Only in-progress bookings can be completed.");
            }

            if (actualFare <= 0)
            {
                throw new ArgumentException("Actual fare must be greater than zero.");
            }

            // Update booking
            booking.ActualFare = actualFare;
            booking.CompletionTime = DateTime.UtcNow;
            booking.Status = BookingStatus.Completed;
            await _bookingRepository.UpdateAsync(booking);

            // Update driver status back to available and increment ride count
            if (booking.DriverId.HasValue)
            {
                await _driverRepository.UpdateDriverStatusAsync(booking.DriverId.Value, DriverStatus.Available);
                await _driverRepository.IncrementTotalRidesAsync(booking.DriverId.Value);
            }
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status == BookingStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel completed bookings.");
            }

            // If booking was assigned, make driver available again
            if (booking.Status == BookingStatus.Assigned && booking.DriverId.HasValue)
            {
                await _driverRepository.UpdateDriverStatusAsync(booking.DriverId.Value, DriverStatus.Available);
            }

            await _bookingRepository.UpdateBookingStatusAsync(bookingId, BookingStatus.Cancelled);
        }

        // fare calculation
        public async Task<decimal> CalculateFareAsync(string pickupLocation, string dropoffLocation, ServiceType serviceType)
        {
            // Simple fare calculation logic
            // In real scenario, you'd use distance calculation API (Google Maps, etc.)

            // Base fare calculation (placeholder logic)
            decimal baseFare = serviceType switch
            {
                ServiceType.Ride => 5.00m,
                ServiceType.Corporate => 10.00m,
                ServiceType.Parcel => 3.00m,
                _ => 5.00m
            };

            // Simulated distance calculation (in real app, use actual distance)
            // For now, estimate based on string length difference (placeholder)
            int estimatedDistance = Math.Abs(pickupLocation.Length - dropoffLocation.Length) + 5;
            decimal perKmRate = 1.50m;

            decimal totalFare = baseFare + (estimatedDistance * perKmRate);

            // Apply service type multiplier
            decimal multiplier = serviceType switch
            {
                ServiceType.Corporate => 1.5m,
                ServiceType.Parcel => 0.8m,
                _ => 1.0m
            };

            return await Task.FromResult(Math.Round(totalFare * multiplier, 2));
        }

        public async Task<decimal> EstimateFareAsync(double distanceKm, ServiceType serviceType)
        {
            decimal baseFare = serviceType switch
            {
                ServiceType.Ride => 5.00m,
                ServiceType.Corporate => 10.00m,
                ServiceType.Parcel => 3.00m,
                _ => 5.00m
            };

            decimal perKmRate = 1.50m;
            decimal totalFare = baseFare + ((decimal)distanceKm * perKmRate);

            // Apply service type multiplier
            decimal multiplier = serviceType switch
            {
                ServiceType.Corporate => 1.5m,
                ServiceType.Parcel => 0.8m,
                _ => 1.0m
            };

            return await Task.FromResult(Math.Round(totalFare * multiplier, 2));
        }

    // statistics
    public async Task<int> GetTotalBookingsCountAsync()
        {
            return await _bookingRepository.CountAsync();
        }

        public async Task<int> GetTotalBookingsByUserAsync(int userId)
        {
            return await _bookingRepository.GetTotalBookingsByUserAsync(userId);
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _bookingRepository.GetTotalRevenueByDateRangeAsync(startDate, endDate);
        }
    }
}
