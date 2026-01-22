using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;

namespace STFMS.BLL.Interfaces.Services
{
    public interface ISearchService
    {
        // ADVANCED BOOKING SEARCH signatures
        Task<IEnumerable<Booking>> SearchBookingsAsync(
            int? userId = null,
            int? driverId = null,
            int? vehicleId = null,
            BookingStatus? status = null,
            ServiceType? serviceType = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minFare = null,
            decimal? maxFare = null,
            string? pickupLocation = null,
            string? dropoffLocation = null);

        Task<IEnumerable<Booking>> SearchBookingsByLocationAsync(string location);
        Task<IEnumerable<Booking>> SearchBookingsByDateAsync(DateTime date);

        // ADVANCED DRIVER SEARCH signatures
        Task<IEnumerable<Driver>> SearchDriversAsync(
            DriverStatus? status = null,
            decimal? minRating = null,
            decimal? maxRating = null,
            int? minTotalRides = null,
            int? maxTotalRides = null,
            string? licenseNumber = null,
            DateTime? joinedAfter = null,
            DateTime? joinedBefore = null);

        Task<IEnumerable<Driver>> SearchDriversByNameAsync(string name);
        Task<IEnumerable<Driver>> SearchDriversByRatingRangeAsync(decimal minRating, decimal maxRating);

        // ADVANCED VEHICLE SEARCH signatures
        Task<IEnumerable<Vehicle>> SearchVehiclesAsync(
            int? driverId = null,
            VehicleStatus? status = null,
            VehicleType? vehicleType = null,
            string? registrationNumber = null,
            string? model = null,
            int? minCapacity = null,
            int? maxCapacity = null);

        Task<IEnumerable<Vehicle>> SearchVehiclesByModelAsync(string model);
        Task<IEnumerable<Vehicle>> SearchVehiclesByCapacityAsync(int minCapacity, int maxCapacity);

        // ADVANCED USER SEARCH signatures
        Task<IEnumerable<User>> SearchUsersAsync(
            UserType? userType = null,
            bool? isActive = null,
            string? email = null,
            string? phoneNumber = null,
            string? name = null,
            DateTime? registeredAfter = null,
            DateTime? registeredBefore = null);

        Task<IEnumerable<User>> SearchUsersByNameAsync(string name);
        Task<IEnumerable<User>> SearchUsersByEmailAsync(string email);

        // ADVANCED PAYMENT SEARCH signatures
        Task<IEnumerable<Payment>> SearchPaymentsAsync(
            PaymentStatus? status = null,
            PaymentMethod? method = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            string? transactionId = null);

        Task<IEnumerable<Payment>> SearchPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount);

        // ADVANCED FEEDBACK SEARCH signatures
        Task<IEnumerable<Feedback>> SearchFeedbacksAsync(
            int? userId = null,
            int? driverId = null,
            int? minRating = null,
            int? maxRating = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? commentKeyword = null);

        Task<IEnumerable<Feedback>> SearchFeedbacksByRatingAsync(int rating);
        Task<IEnumerable<Feedback>> SearchFeedbacksByKeywordAsync(string keyword);

        // ADVANCED MAINTENANCE SEARCH signatures
        Task<IEnumerable<Maintenance>> SearchMaintenanceAsync(
            int? vehicleId = null,
            MaintenanceStatus? status = null,
            MaintenanceType? maintenanceType = null,
            DateTime? scheduledAfter = null,
            DateTime? scheduledBefore = null,
            decimal? minCost = null,
            decimal? maxCost = null);

        Task<IEnumerable<Maintenance>> SearchMaintenanceByCostRangeAsync(decimal minCost, decimal maxCost);

        // COMBINED/COMPLEX SEARCHES signatures
        Task<IEnumerable<Booking>> SearchBookingsWithDriverAndVehicleAsync(
            string? driverName = null,
            string? vehicleModel = null,
            BookingStatus? status = null);

        Task<IEnumerable<User>> SearchCustomersWithBookingHistoryAsync(
            int minBookings,
            decimal minTotalSpent);

        Task<IEnumerable<Vehicle>> SearchVehiclesNeedingMaintenanceAsync(int daysThreshold);
    }
}

