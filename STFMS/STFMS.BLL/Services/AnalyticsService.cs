using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using STFMS.BLL.Interfaces.Services;
using System.Globalization;

namespace STFMS.BLL.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMaintenanceRepository _maintenanceRepository;

        public AnalyticsService(
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository,
            IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IUserRepository userRepository,
            IFeedbackRepository feedbackRepository,
            IMaintenanceRepository maintenanceRepository)
        {
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _feedbackRepository = feedbackRepository;
            _maintenanceRepository = maintenanceRepository;
        }

        // DASHBOARD STATISTICS
        public async Task<(int TotalBookings, int CompletedRides, int ActiveBookings, int PendingBookings,
            decimal TotalRevenue, int TotalDrivers, int ActiveDrivers, int TotalVehicles,
            int ActiveVehicles, decimal AverageRating)> GetDashboardStatsAsync()
        {
            var totalBookings = await _bookingRepository.CountAsync();
            var completedRides = await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Completed);
            var activeBookings = await _bookingRepository.CountAsync(b =>
                b.Status == BookingStatus.Assigned || b.Status == BookingStatus.InProgress);
            var pendingBookings = await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Pending);

            var totalRevenue = await _paymentRepository.GetTotalRevenueAsync();

            var totalDrivers = await _driverRepository.CountAsync();
            var activeDrivers = (await _driverRepository.GetAvailableDriversAsync()).Count();

            var totalVehicles = await _vehicleRepository.CountAsync();
            var activeVehicles = await _vehicleRepository.GetTotalActiveVehiclesCountAsync();

            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            var averageRating = allFeedbacks.Any() ? (decimal)allFeedbacks.Average(f => f.Rating) : 0m;

            return (totalBookings, completedRides, activeBookings, pendingBookings,
                totalRevenue, totalDrivers, activeDrivers, totalVehicles, activeVehicles, averageRating);
        }

        public async Task<(int TotalUsers, int TotalCustomers, int TotalDrivers, decimal TodayRevenue,
            decimal MonthRevenue, int TodayBookings)> GetAdminDashboardStatsAsync()
        {
            var totalUsers = await _userRepository.CountAsync();
            var totalCustomers = await _userRepository.GetTotalCustomersCountAsync();
            var totalDrivers = await _driverRepository.CountAsync();

            var today = DateTime.UtcNow.Date;
            var todayRevenue = await GetRevenueByDateRangeAsync(today, today.AddDays(1));

            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var monthRevenue = await GetRevenueByDateRangeAsync(monthStart, DateTime.UtcNow);

            var todayBookings = await _bookingRepository.CountAsync(b => b.BookingTime.Date == today);

            return (totalUsers, totalCustomers, totalDrivers, todayRevenue, monthRevenue, todayBookings);
        }

        // REVENUE ANALYTICS
        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _paymentRepository.GetTotalRevenueAsync();
        }

        public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate);
            return payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount);
        }

        public async Task<decimal> GetRevenueByServiceTypeAsync(ServiceType serviceType)
        {
            var bookings = await _bookingRepository.GetBookingsByServiceTypeAsync(serviceType);
            var completedBookings = bookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);
            return completedBookings.Sum(b => b.ActualFare!.Value);
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await GetRevenueByDateRangeAsync(today, today.AddDays(1));
        }

        public async Task<decimal> GetMonthRevenueAsync(int year, int month)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1);
            return await GetRevenueByDateRangeAsync(monthStart, monthEnd);
        }

        public async Task<IEnumerable<(DateTime Date, decimal Revenue, int BookingCount)>>
            GetDailyRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var completedBookings = bookings.Where(b =>
                b.Status == BookingStatus.Completed &&
                b.CompletionTime.HasValue &&
                b.ActualFare.HasValue);

            var dailyRevenue = completedBookings
                .GroupBy(b => b.CompletionTime!.Value.Date)
                .Select(g => (
                    Date: g.Key,
                    Revenue: g.Sum(b => b.ActualFare!.Value),
                    BookingCount: g.Count()
                ))
                .OrderBy(x => x.Date);

            return dailyRevenue;
        }

        public async Task<IEnumerable<(int Month, string MonthName, decimal Revenue, int BookingCount)>>
            GetMonthlyRevenueReportAsync(int year)
        {
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = new DateTime(year, 12, 31);

            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(yearStart, yearEnd);
            var completedBookings = bookings.Where(b =>
                b.Status == BookingStatus.Completed &&
                b.CompletionTime.HasValue &&
                b.ActualFare.HasValue);

            var monthlyRevenue = completedBookings
                .GroupBy(b => b.CompletionTime!.Value.Month)
                .Select(g => (
                    Month: g.Key,
                    MonthName: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Revenue: g.Sum(b => b.ActualFare!.Value),
                    BookingCount: g.Count()
                ))
                .OrderBy(x => x.Month);

            return monthlyRevenue;
        }

        public async Task<IEnumerable<(PaymentMethod Method, decimal Revenue, int TransactionCount)>>
            GetRevenueByPaymentMethodAsync()
        {
            var completedPayments = await _paymentRepository.GetPaymentsByStatusAsync(PaymentStatus.Completed);

            var revenueByMethod = completedPayments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => (
                    Method: g.Key,
                    Revenue: g.Sum(p => p.Amount),
                    TransactionCount: g.Count()
                ))
                .OrderByDescending(x => x.Revenue);

            return revenueByMethod;
        }

        // DRIVER PERFORMANCE ANALYTICS
        public async Task<IEnumerable<Driver>> GetTopPerformingDriversAsync(int count)
        {
            return await _driverRepository.GetTopRatedDriversAsync(count);
        }

        public async Task<IEnumerable<Driver>> GetDriversWithLowPerformanceAsync(decimal ratingThreshold)
        {
            return await _driverRepository.GetDriversWithLowRatingAsync(ratingThreshold);
        }

        public async Task<(decimal Rating, int TotalRides, decimal TotalRevenue, decimal AverageRating,
            int CompletedRides, int CancelledRides)> GetDriverPerformanceAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            var driverBookings = await _bookingRepository.GetBookingsByDriverIdAsync(driverId);
            var completedBookings = driverBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);
            var cancelledBookings = driverBookings.Where(b => b.Status == BookingStatus.Cancelled);

            var totalRevenue = completedBookings.Sum(b => b.ActualFare!.Value);
            var completedRides = completedBookings.Count();
            var cancelledRides = cancelledBookings.Count();

            var averageRating = await _feedbackRepository.GetAverageRatingForDriverAsync(driverId);

            return (driver.Rating, driver.TotalRides, totalRevenue, averageRating, completedRides, cancelledRides);
        }

        public async Task<IEnumerable<(Driver Driver, decimal TotalRevenue, int TotalRides, decimal AverageRating)>>
            GetAllDriverPerformanceAsync()
        {
            var allDrivers = await _driverRepository.GetAllAsync();
            List<(Driver, decimal, int, decimal)> driverPerformance = [];

            foreach (var driver in allDrivers)
            {
                var bookings = await _bookingRepository.GetBookingsByDriverIdAsync(driver.DriverId);
                var completedBookings = bookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

                var totalRevenue = completedBookings.Sum(b => b.ActualFare!.Value);
                var totalRides = completedBookings.Count();
                var averageRating = await _feedbackRepository.GetAverageRatingForDriverAsync(driver.DriverId);

                driverPerformance.Add((driver, totalRevenue, totalRides, averageRating));
            }

            return driverPerformance.OrderByDescending(dp => dp.Item2); // Order by revenue
        }

        // BOOKING ANALYTICS
        public async Task<IEnumerable<(DateTime Date, int BookingCount)>>
            GetBookingTrendsAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);

            var bookingTrends = bookings
                .GroupBy(b => b.BookingTime.Date)
                .Select(g => (Date: g.Key, BookingCount: g.Count()))
                .OrderBy(x => x.Date);

            return bookingTrends;
        }

        public async Task<IEnumerable<(string PickupLocation, string DropoffLocation, int TripCount, decimal AverageFare)>>
            GetPopularRoutesAsync(int topN)
        {
            var completedBookings = await _bookingRepository.GetBookingsByStatusAsync(BookingStatus.Completed);

            var popularRoutes = completedBookings
                .Where(b => b.ActualFare.HasValue)
                .GroupBy(b => new { b.PickupLocation, b.DropoffLocation })
                .Select(g => (
                    PickupLocation: g.Key.PickupLocation,
                    DropoffLocation: g.Key.DropoffLocation,
                    TripCount: g.Count(),
                    AverageFare: g.Average(b => b.ActualFare!.Value)
                ))
                .OrderByDescending(x => x.TripCount)
                .Take(topN);

            return popularRoutes;
        }

        public async Task<IEnumerable<(ServiceType ServiceType, int BookingCount, decimal TotalRevenue, decimal AverageFare)>>
            GetServiceTypeDistributionAsync()
        {
            var allBookings = await _bookingRepository.GetAllAsync();
            var completedBookings = allBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

            var serviceTypeStats = completedBookings
                .GroupBy(b => b.ServiceType)
                .Select(g => (
                    ServiceType: g.Key,
                    BookingCount: g.Count(),
                    TotalRevenue: g.Sum(b => b.ActualFare!.Value),
                    AverageFare: g.Average(b => b.ActualFare!.Value)
                ))
                .OrderByDescending(x => x.BookingCount);

            return serviceTypeStats;
        }

        public async Task<IEnumerable<(BookingStatus Status, int Count, decimal Percentage)>>
            GetBookingStatusDistributionAsync()
        {
            var allBookings = await _bookingRepository.GetAllAsync();
            var totalBookings = allBookings.Count();

            if (totalBookings == 0)
            {
                return Enumerable.Empty<(BookingStatus, int, decimal)>();
            }

            var statusDistribution = allBookings
                .GroupBy(b => b.Status)
                .Select(g => (
                    Status: g.Key,
                    Count: g.Count(),
                    Percentage: Math.Round((decimal)g.Count() / totalBookings * 100, 2)
                ))
                .OrderByDescending(x => x.Count);

            return statusDistribution;
        }

        public async Task<(int TotalBookings, int CompletedBookings, int CancelledBookings,
            decimal CompletionRate, decimal CancellationRate)> GetBookingStatisticsAsync()
        {
            var totalBookings = await _bookingRepository.CountAsync();
            var completedBookings = await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Completed);
            var cancelledBookings = await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Cancelled);

            var completionRate = totalBookings > 0 ? Math.Round((decimal)completedBookings / totalBookings * 100, 2) : 0;
            var cancellationRate = totalBookings > 0 ? Math.Round((decimal)cancelledBookings / totalBookings * 100, 2) : 0;

            return (totalBookings, completedBookings, cancelledBookings, completionRate, cancellationRate);
        }

        // VEHICLE ANALYTICS
        public async Task<IEnumerable<(Vehicle Vehicle, int TotalTrips, decimal UtilizationRate)>>
            GetVehicleUtilizationReportAsync()
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var vehicleUtilization = new List<(Vehicle, int, decimal)>();

            foreach (var vehicle in allVehicles)
            {
                var vehicleBookings = (await _bookingRepository.GetAllAsync())
                    .Where(b => b.VehicleId == vehicle.VehicleId && b.Status == BookingStatus.Completed);

                var totalTrips = vehicleBookings.Count();

                // calculate utilization rate (placeholder logic)
                // In real scenario: (actual hours used / available hours) * 100
                var utilizationRate = Math.Min(totalTrips * 2.5m, 100m); // Simple estimation

                vehicleUtilization.Add((vehicle, totalTrips, utilizationRate));
            }

            return vehicleUtilization.OrderByDescending(vu => vu.Item2);
        }

        public async Task<IEnumerable<(VehicleType Type, int VehicleCount, int TotalTrips, decimal TotalRevenue)>>
            GetVehicleTypePerformanceAsync()
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var allBookings = await _bookingRepository.GetAllAsync();

            var vehicleTypePerformance = allVehicles
                .GroupBy(v => v.VehicleType)
                .Select(g =>
                {
                    var vehicleIds = g.Select(v => v.VehicleId).ToList();
                    var typeBookings = allBookings.Where(b =>
                        b.VehicleId.HasValue &&
                        vehicleIds.Contains(b.VehicleId.Value) &&
                        b.Status == BookingStatus.Completed &&
                        b.ActualFare.HasValue);

                    return (
                        Type: g.Key,
                        VehicleCount: g.Count(),
                        TotalTrips: typeBookings.Count(),
                        TotalRevenue: typeBookings.Sum(b => b.ActualFare!.Value)
                    );
                })
                .OrderByDescending(x => x.TotalRevenue);

            return vehicleTypePerformance;
        }

        public async Task<decimal> GetAverageMaintenanceCostPerVehicleAsync()
        {
            var totalCost = await _maintenanceRepository.GetTotalMaintenanceCostAsync();
            var totalVehicles = await _vehicleRepository.CountAsync();

            return totalVehicles > 0 ? Math.Round(totalCost / totalVehicles, 2) : 0;
        }

        public async Task<decimal> GetTotalMaintenanceCostAsync()
        {
            return await _maintenanceRepository.GetTotalMaintenanceCostAsync();
        }

        public async Task<IEnumerable<(Vehicle Vehicle, decimal MaintenanceCost, int MaintenanceCount)>>
            GetVehicleMaintenanceSummaryAsync()
        {
            var allVehicles = await _vehicleRepository.GetAllAsync();
            var maintenanceSummary = new List<(Vehicle, decimal, int)>();

            foreach (var vehicle in allVehicles)
            {
                var maintenanceRecords = await _maintenanceRepository.GetMaintenanceByVehicleIdAsync(vehicle.VehicleId);
                var completedMaintenance = maintenanceRecords.Where(m => m.Status == MaintenanceStatus.Completed);

                var totalCost = completedMaintenance.Sum(m => m.Cost);
                var maintenanceCount = completedMaintenance.Count();

                maintenanceSummary.Add((vehicle, totalCost, maintenanceCount));
            }

            return maintenanceSummary.OrderByDescending(ms => ms.Item2);
        }

        // CUSTOMER ANALYTICS
        public async Task<IEnumerable<(User User, int TotalBookings, decimal TotalSpent, DateTime LastBooking)>>
            GetTopCustomersAsync(int count)
        {
            var allCustomers = await _userRepository.GetUsersByTypeAsync(UserType.Customer);
            var topCustomers = new List<(User, int, decimal, DateTime)>();

            foreach (var customer in allCustomers)
            {
                var customerBookings = await _bookingRepository.GetBookingsByUserIdAsync(customer.UserId);
                var completedBookings = customerBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

                if (!completedBookings.Any()) continue;

                var totalBookings = completedBookings.Count();
                var totalSpent = completedBookings.Sum(b => b.ActualFare!.Value);
                var lastBooking = completedBookings.Max(b => b.CompletionTime ?? b.BookingTime);

                topCustomers.Add((customer, totalBookings, totalSpent, lastBooking));
            }

            return topCustomers.OrderByDescending(tc => tc.Item3).Take(count);
        }

        public async Task<decimal> GetAverageBookingValueAsync()
        {
            var completedBookings = await _bookingRepository.GetBookingsByStatusAsync(BookingStatus.Completed);
            var bookingsWithFare = completedBookings.Where(b => b.ActualFare.HasValue);

            return bookingsWithFare.Any() ? Math.Round(bookingsWithFare.Average(b => b.ActualFare!.Value), 2) : 0;
        }

        public async Task<decimal> GetCustomerLifetimeValueAsync(int userId)
        {
            var customerBookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            var completedBookings = customerBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

            return completedBookings.Sum(b => b.ActualFare!.Value);
        }

        public async Task<int> GetNewCustomersCountAsync(DateTime startDate, DateTime endDate)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var newCustomers = allUsers.Where(u =>
                u.UserType == UserType.Customer &&
                u.RegisteredDate >= startDate &&
                u.RegisteredDate <= endDate);

            return newCustomers.Count();
        }

        public async Task<int> GetActiveCustomersCountAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var activeCustomerIds = bookings.Select(b => b.UserId).Distinct();

            return activeCustomerIds.Count();
        }

        public async Task<(int TotalCustomers, int ActiveCustomers, int NewThisMonth, decimal AverageSpending)>
            GetCustomerStatisticsAsync()
        {
            var totalCustomers = await _userRepository.GetTotalCustomersCountAsync();

            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var activeCustomers = await GetActiveCustomersCountAsync(monthStart, DateTime.UtcNow);
            var newThisMonth = await GetNewCustomersCountAsync(monthStart, DateTime.UtcNow);

            var averageSpending = await GetAverageBookingValueAsync();

            return (totalCustomers, activeCustomers, newThisMonth, averageSpending);
        }

        // FEEDBACK & RATING ANALYTICS
        public async Task<decimal> GetOverallAverageRatingAsync()
        {
            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            return allFeedbacks.Any() ? (decimal)allFeedbacks.Average(f => f.Rating) : 0m;
        }

        public async Task<IEnumerable<(int Rating, int Count, decimal Percentage)>> GetRatingDistributionAsync()
        {
            var allFeedbacks = await _feedbackRepository.GetAllAsync();
            var totalFeedbacks = allFeedbacks.Count();

            if (totalFeedbacks == 0)
            {
                return Enumerable.Empty<(int, int, decimal)>();
            }

            var ratingDistribution = allFeedbacks
                .GroupBy(f => f.Rating)
                .Select(g => (
                    Rating: g.Key,
                    Count: g.Count(),
                    Percentage: Math.Round((decimal)g.Count() / totalFeedbacks * 100, 2)
                ))
                .OrderByDescending(x => x.Rating);

            return ratingDistribution;
        }

        public async Task<IEnumerable<(Driver Driver, decimal AverageRating, int FeedbackCount)>> GetDriverRatingsAsync()
        {
            var allDrivers = await _driverRepository.GetAllAsync();
            var driverRatings = new List<(Driver, decimal, int)>();

            foreach (var driver in allDrivers)
            {
                var feedbacks = await _feedbackRepository.GetFeedbacksByDriverIdAsync(driver.DriverId);
                var feedbackCount = feedbacks.Count();
                var averageRating = feedbackCount > 0 ? (decimal)feedbacks.Average(f => f.Rating) : 0m;

                driverRatings.Add((driver, averageRating, feedbackCount));
            }

            return driverRatings.OrderByDescending(dr => dr.Item2);
        }

        // time based analytics
        public async Task<IEnumerable<(int Hour, int BookingCount)>> GetPeakHoursAsync(DateTime date)
        {
            var dayBookings = await _bookingRepository.GetBookingsByDateRangeAsync(date.Date, date.Date.AddDays(1));

            var peakHours = dayBookings
                .GroupBy(b => b.BookingTime.Hour)
                .Select(g => (Hour: g.Key, BookingCount: g.Count()))
                .OrderBy(x => x.Hour);

            return peakHours;
        }

        public async Task<IEnumerable<(string DayOfWeek, int BookingCount, decimal Revenue)>> GetWeeklyTrendsAsync()
        {
            var weekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            var weekBookings = await _bookingRepository.GetBookingsByDateRangeAsync(weekStart, weekEnd);
            var completedBookings = weekBookings.Where(b => b.Status == BookingStatus.Completed && b.ActualFare.HasValue);

            var weeklyTrends = completedBookings
                .GroupBy(b => b.CompletionTime!.Value.DayOfWeek)
                .Select(g => (
                    DayOfWeek: g.Key.ToString(),
                    BookingCount: g.Count(),
                    Revenue: g.Sum(b => b.ActualFare!.Value)
                ))
                .OrderBy(x => (int)Enum.Parse<DayOfWeek>(x.DayOfWeek));

            return weeklyTrends;
        }

        // statistics
        public async Task<int> GetTotalBookingsCountAsync()
        {
            return await _bookingRepository.CountAsync();
        }

        public async Task<int> GetActiveBookingsCountAsync()
        {
            return await _bookingRepository.CountAsync(b =>
                b.Status == BookingStatus.Assigned || b.Status == BookingStatus.InProgress);
        }

        public async Task<int> GetPendingBookingsCountAsync()
        {
            return await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Pending);
        }

        public async Task<int> GetCompletedBookingsCountAsync()
        {
            return await _bookingRepository.CountAsync(b => b.Status == BookingStatus.Completed);
        }

        public async Task<int> GetTodayBookingsCountAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _bookingRepository.CountAsync(b => b.BookingTime.Date == today);
        }
    }
}