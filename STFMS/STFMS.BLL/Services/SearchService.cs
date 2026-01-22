using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using STFMS.BLL.Interfaces.Services;

namespace STFMS.BLL.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;

        public SearchService(
            IBookingRepository bookingRepository,
            IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            IFeedbackRepository feedbackRepository,
            IMaintenanceRepository maintenanceRepository)
        {
            _bookingRepository = bookingRepository;
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _feedbackRepository = feedbackRepository;
            _maintenanceRepository = maintenanceRepository;
        }

        // ADVANCED BOOKING SEARCH
        public async Task<IEnumerable<Booking>> SearchBookingsAsync(
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
            string? dropoffLocation = null)
        {
            var allBookings = await _bookingRepository.GetAllAsync();
            var query = allBookings.AsQueryable();

            if (userId.HasValue)
                query = query.Where(b => b.UserId == userId.Value);

            if (driverId.HasValue)
                query = query.Where(b => b.DriverId == driverId.Value);

            if (vehicleId.HasValue)
                query = query.Where(b => b.VehicleId == vehicleId.Value);

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            if (serviceType.HasValue)
                query = query.Where(b => b.ServiceType == serviceType.Value);

            if (startDate.HasValue)
                query = query.Where(b => b.BookingTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.BookingTime <= endDate.Value);

            if (minFare.HasValue)
                query = query.Where(b => b.EstimatedFare >= minFare.Value || (b.ActualFare.HasValue && b.ActualFare.Value >= minFare.Value));

            if (maxFare.HasValue)
                query = query.Where(b => b.EstimatedFare <= maxFare.Value || (b.ActualFare.HasValue && b.ActualFare.Value <= maxFare.Value));

            if (!string.IsNullOrWhiteSpace(pickupLocation))
                query = query.Where(b => b.PickupLocation.Contains(pickupLocation, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(dropoffLocation))
                query = query.Where(b => b.DropoffLocation.Contains(dropoffLocation, StringComparison.OrdinalIgnoreCase));

            return query.OrderByDescending(b => b.BookingTime).ToList();
        }

        public async Task<IEnumerable<Booking>> SearchBookingsByLocationAsync(string? location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return await _bookingRepository.GetAllAsync();
            }

            var allBookings = await _bookingRepository.GetAllAsync();
            return allBookings.Where(b =>
                b.PickupLocation.Contains(location, StringComparison.OrdinalIgnoreCase) ||
                b.DropoffLocation.Contains(location, StringComparison.OrdinalIgnoreCase)
            ).OrderByDescending(b => b.BookingTime).ToList();
        }

        public async Task<IEnumerable<Booking>> SearchBookingsByDateAsync(DateTime date)
        {
            return await _bookingRepository.GetBookingsByDateRangeAsync(date.Date, date.Date.AddDays(1));
        }

        // ADVANCED DRIVER SEARCH
        public async Task<IEnumerable<Driver>> SearchDriversAsync(
            DriverStatus? status = null,
            decimal? minRating = null,
            decimal? maxRating = null,
            int? minTotalRides = null,
            int? maxTotalRides = null,
            string? licenseNumber = null,
            DateTime? joinedAfter = null,
            DateTime? joinedBefore = null)
        {
            var allDrivers = await _driverRepository.GetAllAsync();
            var query = allDrivers.AsQueryable();

            if (status.HasValue)
                query = query.Where(d => d.Status == status.Value);

            if (minRating.HasValue)
                query = query.Where(d => d.Rating >= minRating.Value);

            if (maxRating.HasValue)
                query = query.Where(d => d.Rating <= maxRating.Value);

            if (minTotalRides.HasValue)
                query = query.Where(d => d.TotalRides >= minTotalRides.Value);

            if (maxTotalRides.HasValue)
                query = query.Where(d => d.TotalRides <= maxTotalRides.Value);

            if (!string.IsNullOrWhiteSpace(licenseNumber))
                query = query.Where(d => d.LicenseNumber.Contains(licenseNumber, StringComparison.OrdinalIgnoreCase));

            if (joinedAfter.HasValue)
                query = query.Where(d => d.JoinedDate >= joinedAfter.Value);

            if (joinedBefore.HasValue)
                query = query.Where(d => d.JoinedDate <= joinedBefore.Value);

            return query.OrderByDescending(d => d.Rating).ToList();
        }

        public async Task<IEnumerable<Driver>> SearchDriversByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await _driverRepository.GetAllAsync();
            }

            var allDrivers = await _driverRepository.GetAllAsync();
            // Note: Would need to load User navigation property for name search
            // This is a simplified version
            return allDrivers.OrderBy(d => d.DriverId).ToList();
        }

        public async Task<IEnumerable<Driver>> SearchDriversByRatingRangeAsync(decimal minRating, decimal maxRating)
        {
            var allDrivers = await _driverRepository.GetAllAsync();
            return allDrivers.Where(d => d.Rating >= minRating && d.Rating <= maxRating)
                            .OrderByDescending(d => d.Rating)
                            .ToList();
        }

        // ADVANCED VEHICLE SEARCH
        public async Task<IEnumerable<Vehicle>> SearchVehiclesAsync(
            int? driverId = null,
            VehicleStatus? status = null,
            VehicleType? vehicleType = null,
            string? registrationNumber = null,
            string? model = null,
            int? minCapacity = null,
            int? maxCapacity = null)
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var query = allVehicles.AsQueryable();

            if (driverId.HasValue)
                query = query.Where(v => v.DriverId == driverId.Value);

            if (status.HasValue)
                query = query.Where(v => v.Status == status.Value);

            if (vehicleType.HasValue)
                query = query.Where(v => v.VehicleType == vehicleType.Value);

            if (!string.IsNullOrWhiteSpace(registrationNumber))
                query = query.Where(v => v.RegistrationNumber.Contains(registrationNumber, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(v => v.Model.Contains(model, StringComparison.OrdinalIgnoreCase));

            if (minCapacity.HasValue)
                query = query.Where(v => v.Capacity >= minCapacity.Value);

            if (maxCapacity.HasValue)
                query = query.Where(v => v.Capacity <= maxCapacity.Value);

            return query.OrderBy(v => v.RegistrationNumber).ToList();
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesByModelAsync(string? model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                return await _vehicleRepository.GetAllAsync();
            }

            var allVehicles = await _vehicleRepository.GetAllAsync();
            return allVehicles.Where(v => v.Model.Contains(model, StringComparison.OrdinalIgnoreCase))
                             .OrderBy(v => v.Model)
                             .ToList();
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesByCapacityAsync(int minCapacity, int maxCapacity)
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            return allVehicles.Where(v => v.Capacity >= minCapacity && v.Capacity <= maxCapacity)
                             .OrderBy(v => v.Capacity)
                             .ToList();
        }

        // ADVANCED USER SEARCH
        public async Task<IEnumerable<User>> SearchUsersAsync(
            UserType? userType = null,
            bool? isActive = null,
            string? email = null,
            string? phoneNumber = null,
            string? name = null,
            DateTime? registeredAfter = null,
            DateTime? registeredBefore = null)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var query = allUsers.AsQueryable();

            if (userType.HasValue)
                query = query.Where(u => u.UserType == userType.Value);

            if (isActive.HasValue)
                query = query.Where(u => u.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(u => u.PhoneNumber.Contains(phoneNumber));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));

            if (registeredAfter.HasValue)
                query = query.Where(u => u.RegisteredDate >= registeredAfter.Value);

            if (registeredBefore.HasValue)
                query = query.Where(u => u.RegisteredDate <= registeredBefore.Value);

            return query.OrderBy(u => u.FullName).ToList();
        }

        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await _userRepository.GetAllAsync();
            }

            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                          .OrderBy(u => u.FullName)
                          .ToList();
        }

        public async Task<IEnumerable<User>> SearchUsersByEmailAsync(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return await _userRepository.GetAllAsync();
            }

            var allUsers = await _userRepository.GetAllAsync();
            return allUsers.Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
                          .OrderBy(u => u.Email)
                          .ToList();
        }

        // ADVANCED PAYMENT SEARCH
        public async Task<IEnumerable<Payment>> SearchPaymentsAsync(
            PaymentStatus? status = null,
            PaymentMethod? method = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minAmount = null,
            decimal? maxAmount = null,
            string? transactionId = null)
        {
            var allPayments = await _paymentRepository.GetAllAsync();
            var query = allPayments.AsQueryable();

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (method.HasValue)
                query = query.Where(p => p.PaymentMethod == method.Value);

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value);

            if (minAmount.HasValue)
                query = query.Where(p => p.Amount >= minAmount.Value);

            if (maxAmount.HasValue)
                query = query.Where(p => p.Amount <= maxAmount.Value);

            if (!string.IsNullOrWhiteSpace(transactionId))
                query = query.Where(p => p.TransactionId != null && p.TransactionId.Contains(transactionId, StringComparison.OrdinalIgnoreCase));

            return query.OrderByDescending(p => p.PaymentDate).ToList();
        }

        public async Task<IEnumerable<Payment>> SearchPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            var allPayments = await _paymentRepository.GetAllAsync();
            return allPayments.Where(p => p.Amount >= minAmount && p.Amount <= maxAmount)
                             .OrderByDescending(p => p.Amount)
                             .ToList();
        }

        // ADVANCED FEEDBACK SEARCH
        public async Task<IEnumerable<Feedback>> SearchFeedbacksAsync(
            int? userId = null,
            int? driverId = null,
            int? minRating = null,
            int? maxRating = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? commentKeyword = null)
        {
            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            var query = allFeedbacks.AsQueryable();

            if (userId.HasValue)
                query = query.Where(f => f.UserId == userId.Value);

            // Note: driverId search would require loading Booking navigation property
            // Simplified for now

            if (minRating.HasValue)
                query = query.Where(f => f.Rating >= minRating.Value);

            if (maxRating.HasValue)
                query = query.Where(f => f.Rating <= maxRating.Value);

            if (startDate.HasValue)
                query = query.Where(f => f.SubmittedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(f => f.SubmittedDate <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(commentKeyword))
                query = query.Where(f => f.Comments != null && f.Comments.Contains(commentKeyword, StringComparison.OrdinalIgnoreCase));

            return query.OrderByDescending(f => f.SubmittedDate).ToList();
        }

        public async Task<IEnumerable<Feedback>> SearchFeedbacksByRatingAsync(int rating)
        {
            return await _feedbackRepository.GetFeedbacksByRatingAsync(rating);
        }

        public async Task<IEnumerable<Feedback>> SearchFeedbacksByKeywordAsync(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await _feedbackRepository.GetAllAsync();
            }

            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            return allFeedbacks.Where(f => f.Comments != null && f.Comments.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                              .OrderByDescending(f => f.SubmittedDate)
                              .ToList();
        }

        // ADVANCED MAINTENANCE SEARCH
        public async Task<IEnumerable<Maintenance>> SearchMaintenanceAsync(
            int? vehicleId = null,
            MaintenanceStatus? status = null,
            MaintenanceType? maintenanceType = null,
            DateTime? scheduledAfter = null,
            DateTime? scheduledBefore = null,
            decimal? minCost = null,
            decimal? maxCost = null)
        {
            var allMaintenance = await _maintenanceRepository.GetAllAsync();
            var query = allMaintenance.AsQueryable();

            if (vehicleId.HasValue)
                query = query.Where(m => m.VehicleId == vehicleId.Value);

            if (status.HasValue)
                query = query.Where(m => m.Status == status.Value);

            if (maintenanceType.HasValue)
                query = query.Where(m => m.MaintenanceType == maintenanceType.Value);

            if (scheduledAfter.HasValue)
                query = query.Where(m => m.ScheduledDate >= scheduledAfter.Value);

            if (scheduledBefore.HasValue)
                query = query.Where(m => m.ScheduledDate <= scheduledBefore.Value);

            if (minCost.HasValue)
                query = query.Where(m => m.Cost >= minCost.Value);

            if (maxCost.HasValue)
                query = query.Where(m => m.Cost <= maxCost.Value);

            return query.OrderByDescending(m => m.ScheduledDate).ToList();
        }

        public async Task<IEnumerable<Maintenance>> SearchMaintenanceByCostRangeAsync(decimal minCost, decimal maxCost)
        {
            var allMaintenance = await _maintenanceRepository.GetAllAsync();
            return allMaintenance.Where(m => m.Cost >= minCost && m.Cost <= maxCost)
                                .OrderByDescending(m => m.Cost)
                                .ToList();
        }

        // COMBINED/COMPLEX SEARCHES
        public async Task<IEnumerable<Booking>> SearchBookingsWithDriverAndVehicleAsync(
            string? driverName = null,
            string? vehicleModel = null,
            BookingStatus? status = null)
        {
            var allBookings = await _bookingRepository.GetAllAsync();
            var query = allBookings.AsQueryable();

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            // Note: Would need to load Driver and Vehicle navigation properties
            // Simplified for now
            return query.OrderByDescending(b => b.BookingTime).ToList();
        }

        public async Task<IEnumerable<User>> SearchCustomersWithBookingHistoryAsync(int minBookings, decimal minTotalSpent)
        {
            var allCustomers = await _userRepository.GetUsersByTypeAsync(UserType.Customer);
            var qualifiedCustomers = new List<User>();

            foreach (var customer in allCustomers)
            {
                var customerBookings = await _bookingRepository.GetBookingsByUserIdAsync(customer.UserId);
                var completedBookings = customerBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

                var totalBookings = completedBookings.Count();
                var totalSpent = completedBookings.Sum(b => b.ActualFare!.Value);

                if (totalBookings >= minBookings && totalSpent >= minTotalSpent)
                {
                    qualifiedCustomers.Add(customer);
                }
            }

            return qualifiedCustomers;
        }

        public async Task<IEnumerable<Vehicle>> SearchVehiclesNeedingMaintenanceAsync(int daysThreshold)
        {
            return await _vehicleRepository.GetVehiclesDueForMaintenanceAsync(daysThreshold);
        }
    }
}