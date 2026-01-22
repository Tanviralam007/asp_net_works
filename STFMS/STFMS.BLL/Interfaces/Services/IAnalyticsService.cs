using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IAnalyticsService
    {
        // DASHBOARD STATISTICS signatures
        Task<(int TotalBookings, int CompletedRides, int ActiveBookings, int PendingBookings,
              decimal TotalRevenue, int TotalDrivers, int ActiveDrivers, int TotalVehicles,
              int ActiveVehicles, decimal AverageRating)> GetDashboardStatsAsync();

        Task<(int TotalUsers, int TotalCustomers, int TotalDrivers, decimal TodayRevenue,
              decimal MonthRevenue, int TodayBookings)> GetAdminDashboardStatsAsync();

        // REVENUE ANALYTICS signatures
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetRevenueByServiceTypeAsync(ServiceType serviceType);
        Task<decimal> GetTodayRevenueAsync();
        Task<decimal> GetMonthRevenueAsync(int year, int month);

        // Revenue Reports (Returns date, revenue, count) signatures
        Task<IEnumerable<(DateTime Date, decimal Revenue, int BookingCount)>>
            GetDailyRevenueReportAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<(int Month, string MonthName, decimal Revenue, int BookingCount)>>
            GetMonthlyRevenueReportAsync(int year);

        Task<IEnumerable<(PaymentMethod Method, decimal Revenue, int TransactionCount)>>
            GetRevenueByPaymentMethodAsync();

        // DRIVER PERFORMANCE ANALYTICS signatures
        Task<IEnumerable<Driver>> GetTopPerformingDriversAsync(int count);
        Task<IEnumerable<Driver>> GetDriversWithLowPerformanceAsync(decimal ratingThreshold);

        // Driver Performance Stats (Returns detailed performance metrics) signatures
        Task<(decimal Rating, int TotalRides, decimal TotalRevenue, decimal AverageRating,
              int CompletedRides, int CancelledRides)> GetDriverPerformanceAsync(int driverId);

        Task<IEnumerable<(Driver Driver, decimal TotalRevenue, int TotalRides, decimal AverageRating)>>
            GetAllDriverPerformanceAsync();

        // BOOKING ANALYTICS signatures
        Task<IEnumerable<(DateTime Date, int BookingCount)>>
            GetBookingTrendsAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<(string PickupLocation, string DropoffLocation, int TripCount, decimal AverageFare)>>
            GetPopularRoutesAsync(int topN);

        Task<IEnumerable<(ServiceType ServiceType, int BookingCount, decimal TotalRevenue, decimal AverageFare)>>
            GetServiceTypeDistributionAsync();

        Task<IEnumerable<(BookingStatus Status, int Count, decimal Percentage)>>
            GetBookingStatusDistributionAsync();

        Task<(int TotalBookings, int CompletedBookings, int CancelledBookings, decimal CompletionRate, decimal CancellationRate)>
            GetBookingStatisticsAsync();

        // VEHICLE ANALYTICS signatures
        Task<IEnumerable<(Vehicle Vehicle, int TotalTrips, decimal UtilizationRate)>>
            GetVehicleUtilizationReportAsync();

        Task<IEnumerable<(VehicleType Type, int VehicleCount, int TotalTrips, decimal TotalRevenue)>>
            GetVehicleTypePerformanceAsync();

        Task<decimal> GetAverageMaintenanceCostPerVehicleAsync();
        Task<decimal> GetTotalMaintenanceCostAsync();

        Task<IEnumerable<(Vehicle Vehicle, decimal MaintenanceCost, int MaintenanceCount)>>
            GetVehicleMaintenanceSummaryAsync();

        // CUSTOMER ANALYTICS signatures
        Task<IEnumerable<(User User, int TotalBookings, decimal TotalSpent, DateTime LastBooking)>>
            GetTopCustomersAsync(int count);

        Task<decimal> GetAverageBookingValueAsync();
        Task<decimal> GetCustomerLifetimeValueAsync(int userId);

        Task<int> GetNewCustomersCountAsync(DateTime startDate, DateTime endDate);
        Task<int> GetActiveCustomersCountAsync(DateTime startDate, DateTime endDate);

        Task<(int TotalCustomers, int ActiveCustomers, int NewThisMonth, decimal AverageSpending)>
            GetCustomerStatisticsAsync();

        // FEEDBACK & RATING ANALYTICS signatures
        Task<decimal> GetOverallAverageRatingAsync();
        Task<IEnumerable<(int Rating, int Count, decimal Percentage)>> GetRatingDistributionAsync();
        Task<IEnumerable<(Driver Driver, decimal AverageRating, int FeedbackCount)>> GetDriverRatingsAsync();

        // TIME-BASED ANALYTICS signatures
        Task<IEnumerable<(int Hour, int BookingCount)>> GetPeakHoursAsync(DateTime date);
        Task<IEnumerable<(string DayOfWeek, int BookingCount, decimal Revenue)>> GetWeeklyTrendsAsync();

        // GENERAL STATISTICS signatures
        Task<int> GetTotalBookingsCountAsync();
        Task<int> GetActiveBookingsCountAsync();
        Task<int> GetPendingBookingsCountAsync();
        Task<int> GetCompletedBookingsCountAsync();
        Task<int> GetTodayBookingsCountAsync();
    }
}
