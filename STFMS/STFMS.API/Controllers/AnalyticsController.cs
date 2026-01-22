using Microsoft.AspNetCore.Mvc;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Analytics;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/Analytics/dashboard
        // get comprehensive dashboard statistics
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _analyticsService.GetDashboardStatsAsync();

                var dashboardStats = new
                {
                    bookings = new
                    {
                        total = stats.TotalBookings,
                        completed = stats.CompletedRides,
                        active = stats.ActiveBookings,
                        pending = stats.PendingBookings
                    },
                    revenue = new
                    {
                        total = stats.TotalRevenue
                    },
                    drivers = new
                    {
                        total = stats.TotalDrivers,
                        active = stats.ActiveDrivers
                    },
                    vehicles = new
                    {
                        total = stats.TotalVehicles,
                        active = stats.ActiveVehicles
                    },
                    rating = new
                    {
                        average = stats.AverageRating
                    }
                };

                return Ok(ApiResponseDTO<object>.SuccessResponse(
                    dashboardStats,
                    "Dashboard statistics retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving dashboard statistics",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Analytics/dashboard/admin
        // get admin-specific dashboard statistics
        [HttpGet("dashboard/admin")]
        public async Task<IActionResult> GetAdminDashboardStats()
        {
            try
            {
                var stats = await _analyticsService.GetAdminDashboardStatsAsync();

                var adminStats = new
                {
                    users = new
                    {
                        total = stats.TotalUsers,
                        customers = stats.TotalCustomers,
                        drivers = stats.TotalDrivers
                    },
                    revenue = new
                    {
                        today = stats.TodayRevenue,
                        month = stats.MonthRevenue
                    },
                    bookings = new
                    {
                        today = stats.TodayBookings
                    }
                };

                return Ok(ApiResponseDTO<object>.SuccessResponse(
                    adminStats,
                    "Admin dashboard statistics retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving admin dashboard statistics",
                    new List<string> { ex.Message }
                ));
            }
        }

        // REVENUE ANALYTICS

        // get total revenue
        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var revenue = await _analyticsService.GetTotalRevenueAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    "Total revenue retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total revenue",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get today's revenue
        [HttpGet("revenue/today")]
        public async Task<IActionResult> GetTodayRevenue()
        {
            try
            {
                var revenue = await _analyticsService.GetTodayRevenueAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    "Today's revenue retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving today's revenue",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get revenue by date range
        [HttpGet("revenue/date-range")]
        public async Task<IActionResult> GetRevenueByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var revenue = await _analyticsService.GetRevenueByDateRangeAsync(startDate, endDate);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Revenue between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue by date range",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get monthly revenue
        [HttpGet("revenue/month/{year}/{month}")]
        public async Task<IActionResult> GetMonthRevenue(int year, int month)
        {
            try
            {
                var revenue = await _analyticsService.GetMonthRevenueAsync(year, month);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Revenue for {month:D2}/{year} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving month revenue",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get revenue by service type
        [HttpGet("revenue/service-type/{serviceType}")]
        public async Task<IActionResult> GetRevenueByServiceType(ServiceType serviceType)
        {
            try
            {
                var revenue = await _analyticsService.GetRevenueByServiceTypeAsync(serviceType);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Revenue for {serviceType} service retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue by service type",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get revenue by payment method
        [HttpGet("revenue/payment-methods")]
        public async Task<IActionResult> GetRevenueByPaymentMethod()
        {
            try
            {
                var revenueData = await _analyticsService.GetRevenueByPaymentMethodAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    revenueData.Select(r => new
                    {
                        paymentMethod = r.Method.ToString(),
                        revenue = r.Revenue,
                        transactionCount = r.TransactionCount
                    }),
                    "Revenue by payment method retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue by payment method",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get daily revenue report
        [HttpGet("revenue/daily-report")]
        public async Task<IActionResult> GetDailyRevenueReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _analyticsService.GetDailyRevenueReportAsync(startDate, endDate);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    report.Select(r => new
                    {
                        date = r.Date.ToString("yyyy-MM-dd"),
                        revenue = r.Revenue,
                        bookingCount = r.BookingCount
                    }),
                    "Daily revenue report retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving daily revenue report",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get monthly revenue report
        [HttpGet("revenue/monthly-report/{year}")]
        public async Task<IActionResult> GetMonthlyRevenueReport(int year)
        {
            try
            {
                var report = await _analyticsService.GetMonthlyRevenueReportAsync(year);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    report.Select(r => new
                    {
                        month = r.Month,
                        monthName = r.MonthName,
                        revenue = r.Revenue,
                        bookingCount = r.BookingCount
                    }),
                    $"Monthly revenue report for {year} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving monthly revenue report",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DRIVER PERFORMANCE ANALYTICS

        // get top performing drivers
        [HttpGet("drivers/top-performers/{count}")]
        public async Task<IActionResult> GetTopPerformingDrivers(int count = 10)
        {
            try
            {
                var drivers = await _analyticsService.GetTopPerformingDriversAsync(count);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    drivers.Select(d => new
                    {
                        driverId = d.DriverId,
                        name = d.User?.FullName ?? "N/A",
                        licenseNumber = d.LicenseNumber,
                        rating = d.Rating,
                        totalRides = d.TotalRides,
                        status = d.Status.ToString()
                    }),
                    $"Top {count} performing drivers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving top performing drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get drivers with low performance
        [HttpGet("drivers/low-performance/{ratingThreshold}")]
        public async Task<IActionResult> GetDriversWithLowPerformance(decimal ratingThreshold = 3.0m)
        {
            try
            {
                var drivers = await _analyticsService.GetDriversWithLowPerformanceAsync(ratingThreshold);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    drivers.Select(d => new
                    {
                        driverId = d.DriverId,
                        name = d.User?.FullName ?? "N/A",
                        licenseNumber = d.LicenseNumber,
                        rating = d.Rating,
                        totalRides = d.TotalRides,
                        status = d.Status.ToString()
                    }),
                    $"Drivers with rating below {ratingThreshold} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving low performance drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get driver performance
        [HttpGet("drivers/{driverId}/performance")]
        public async Task<IActionResult> GetDriverPerformance(int driverId)
        {
            try
            {
                var performance = await _analyticsService.GetDriverPerformanceAsync(driverId);

                var performanceData = new
                {
                    driverId,
                    rating = performance.Rating,
                    totalRides = performance.TotalRides,
                    totalRevenue = performance.TotalRevenue,
                    averageRating = performance.AverageRating,
                    completedRides = performance.CompletedRides,
                    cancelledRides = performance.CancelledRides,
                    completionRate = performance.TotalRides > 0
                        ? Math.Round((decimal)performance.CompletedRides / performance.TotalRides * 100, 2)
                        : 0
                };

                return Ok(ApiResponseDTO<object>.SuccessResponse(
                    performanceData,
                    $"Performance data for driver {driverId} retrieved successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver performance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get all driver performance
        [HttpGet("drivers/all-performance")]
        public async Task<IActionResult> GetAllDriverPerformance()
        {
            try
            {
                var allPerformance = await _analyticsService.GetAllDriverPerformanceAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    allPerformance.Select(p => new
                    {
                        driverId = p.Driver.DriverId,
                        name = p.Driver.User?.FullName ?? "N/A",
                        licenseNumber = p.Driver.LicenseNumber,
                        totalRevenue = p.TotalRevenue,
                        totalRides = p.TotalRides,
                        averageRating = p.AverageRating
                    }),
                    "All driver performance data retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving all driver performance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get driver ratings
        [HttpGet("drivers/ratings")]
        public async Task<IActionResult> GetDriverRatings()
        {
            try
            {
                var ratings = await _analyticsService.GetDriverRatingsAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    ratings.Select(r => new
                    {
                        driverId = r.Driver.DriverId,
                        name = r.Driver.User?.FullName ?? "N/A",
                        averageRating = r.AverageRating,
                        feedbackCount = r.FeedbackCount
                    }),
                    "Driver ratings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver ratings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // BOOKING ANALYTICS

        // get booking statistics
        [HttpGet("bookings/statistics")]
        public async Task<IActionResult> GetBookingStatistics()
        {
            try
            {
                var stats = await _analyticsService.GetBookingStatisticsAsync();

                var bookingStats = new
                {
                    totalBookings = stats.TotalBookings,
                    completedBookings = stats.CompletedBookings,
                    cancelledBookings = stats.CancelledBookings,
                    completionRate = stats.CompletionRate,
                    cancellationRate = stats.CancellationRate
                };

                return Ok(ApiResponseDTO<object>.SuccessResponse(
                    bookingStats,
                    "Booking statistics retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving booking statistics",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get booking status distribution
        [HttpGet("bookings/status-distribution")]
        public async Task<IActionResult> GetBookingStatusDistribution()
        {
            try
            {
                var distribution = await _analyticsService.GetBookingStatusDistributionAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    distribution.Select(d => new
                    {
                        status = d.Status.ToString(),
                        count = d.Count,
                        percentage = d.Percentage
                    }),
                    "Booking status distribution retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving booking status distribution",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get booking trends
        [HttpGet("bookings/trends")]
        public async Task<IActionResult> GetBookingTrends(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var trends = await _analyticsService.GetBookingTrendsAsync(startDate, endDate);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    trends.Select(t => new
                    {
                        date = t.Date.ToString("yyyy-MM-dd"),
                        bookingCount = t.BookingCount
                    }),
                    "Booking trends retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving booking trends",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get popular routes
        [HttpGet("bookings/popular-routes/{topN}")]
        public async Task<IActionResult> GetPopularRoutes(int topN = 10)
        {
            try
            {
                var routes = await _analyticsService.GetPopularRoutesAsync(topN);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    routes.Select(r => new
                    {
                        pickupLocation = r.PickupLocation,
                        dropoffLocation = r.DropoffLocation,
                        tripCount = r.TripCount,
                        averageFare = r.AverageFare
                    }),
                    $"Top {topN} popular routes retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving popular routes",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get service type distribution
        [HttpGet("bookings/service-type-distribution")]
        public async Task<IActionResult> GetServiceTypeDistribution()
        {
            try
            {
                var distribution = await _analyticsService.GetServiceTypeDistributionAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    distribution.Select(d => new
                    {
                        serviceType = d.ServiceType.ToString(),
                        bookingCount = d.BookingCount,
                        totalRevenue = d.TotalRevenue,
                        averageFare = d.AverageFare
                    }),
                    "Service type distribution retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving service type distribution",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get peak hours
        [HttpGet("bookings/peak-hours/{date}")]
        public async Task<IActionResult> GetPeakHours(DateTime date)
        {
            try
            {
                var peakHours = await _analyticsService.GetPeakHoursAsync(date);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    peakHours.Select(p => new
                    {
                        hour = p.Hour,
                        bookingCount = p.BookingCount
                    }),
                    $"Peak hours for {date:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving peak hours",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get weekly trends
        [HttpGet("bookings/weekly-trends")]
        public async Task<IActionResult> GetWeeklyTrends()
        {
            try
            {
                var trends = await _analyticsService.GetWeeklyTrendsAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    trends.Select(t => new
                    {
                        dayOfWeek = t.DayOfWeek,
                        bookingCount = t.BookingCount,
                        revenue = t.Revenue
                    }),
                    "Weekly trends retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving weekly trends",
                    new List<string> { ex.Message }
                ));
            }
        }

        // VEHICLE ANALYTICS

        // get vehicle utilization report
        [HttpGet("vehicles/utilization")]
        public async Task<IActionResult> GetVehicleUtilizationReport()
        {
            try
            {
                var utilization = await _analyticsService.GetVehicleUtilizationReportAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    utilization.Select(u => new
                    {
                        vehicleId = u.Vehicle.VehicleId,
                        registrationNumber = u.Vehicle.RegistrationNumber,
                        vehicleType = u.Vehicle.VehicleType.ToString(),
                        totalTrips = u.TotalTrips,
                        utilizationRate = u.UtilizationRate
                    }),
                    "Vehicle utilization report retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle utilization report",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get vehicle type performance
        [HttpGet("vehicles/type-performance")]
        public async Task<IActionResult> GetVehicleTypePerformance()
        {
            try
            {
                var performance = await _analyticsService.GetVehicleTypePerformanceAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    performance.Select(p => new
                    {
                        vehicleType = p.Type.ToString(),
                        vehicleCount = p.VehicleCount,
                        totalTrips = p.TotalTrips,
                        totalRevenue = p.TotalRevenue
                    }),
                    "Vehicle type performance retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle type performance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get vehicle maintenance summary
        [HttpGet("vehicles/maintenance-summary")]
        public async Task<IActionResult> GetVehicleMaintenanceSummary()
        {
            try
            {
                var summary = await _analyticsService.GetVehicleMaintenanceSummaryAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    summary.Select(s => new
                    {
                        vehicleId = s.Vehicle.VehicleId,
                        registrationNumber = s.Vehicle.RegistrationNumber,
                        vehicleType = s.Vehicle.VehicleType.ToString(),
                        maintenanceCost = s.MaintenanceCost,
                        maintenanceCount = s.MaintenanceCount
                    }),
                    "Vehicle maintenance summary retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle maintenance summary",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get total maintenance cost
        [HttpGet("vehicles/maintenance-cost/total")]
        public async Task<IActionResult> GetTotalMaintenanceCost()
        {
            try
            {
                var cost = await _analyticsService.GetTotalMaintenanceCostAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    cost,
                    "Total maintenance cost retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total maintenance cost",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get average maintenance cost per vehicle
        [HttpGet("vehicles/maintenance-cost/average")]
        public async Task<IActionResult> GetAverageMaintenanceCostPerVehicle()
        {
            try
            {
                var cost = await _analyticsService.GetAverageMaintenanceCostPerVehicleAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    cost,
                    "Average maintenance cost per vehicle retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving average maintenance cost",
                    new List<string> { ex.Message }
                ));
            }
        }

        // CUSTOMER ANALYTICS

        // get customer statistics
        [HttpGet("customers/statistics")]
        public async Task<IActionResult> GetCustomerStatistics()
        {
            try
            {
                var stats = await _analyticsService.GetCustomerStatisticsAsync();

                var customerStats = new
                {
                    totalCustomers = stats.TotalCustomers,
                    activeCustomers = stats.ActiveCustomers,
                    newThisMonth = stats.NewThisMonth,
                    averageSpending = stats.AverageSpending
                };

                return Ok(ApiResponseDTO<object>.SuccessResponse(
                    customerStats,
                    "Customer statistics retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving customer statistics",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get top customers
        [HttpGet("customers/top/{count}")]
        public async Task<IActionResult> GetTopCustomers(int count = 10)
        {
            try
            {
                var topCustomers = await _analyticsService.GetTopCustomersAsync(count);

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    topCustomers.Select(c => new
                    {
                        userId = c.User.UserId,
                        fullName = c.User.FullName,
                        email = c.User.Email,
                        phoneNumber = c.User.PhoneNumber,
                        totalBookings = c.TotalBookings,
                        totalSpent = c.TotalSpent,
                        lastBooking = c.LastBooking.ToString("yyyy-MM-dd")
                    }),
                    $"Top {count} customers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving top customers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get customer lifetime value
        [HttpGet("customers/{userId}/lifetime-value")]
        public async Task<IActionResult> GetCustomerLifetimeValue(int userId)
        {
            try
            {
                var lifetimeValue = await _analyticsService.GetCustomerLifetimeValueAsync(userId);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    lifetimeValue,
                    $"Customer lifetime value for user {userId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving customer lifetime value",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get average booking value
        [HttpGet("customers/average-booking-value")]
        public async Task<IActionResult> GetAverageBookingValue()
        {
            try
            {
                var averageValue = await _analyticsService.GetAverageBookingValueAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    averageValue,
                    "Average booking value retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving average booking value",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get new customers count
        [HttpGet("customers/new-count")]
        public async Task<IActionResult> GetNewCustomersCount(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var count = await _analyticsService.GetNewCustomersCountAsync(startDate, endDate);

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    $"New customers between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving new customers count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get active customers count
        [HttpGet("customers/active-count")]
        public async Task<IActionResult> GetActiveCustomersCount(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var count = await _analyticsService.GetActiveCustomersCountAsync(startDate, endDate);

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    $"Active customers between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active customers count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // FEEDBACK & RATING ANALYTICS

        // get overall average rating
        [HttpGet("ratings/overall-average")]
        public async Task<IActionResult> GetOverallAverageRating()
        {
            try
            {
                var rating = await _analyticsService.GetOverallAverageRatingAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    rating,
                    "Overall average rating retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving overall average rating",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get rating distribution
        [HttpGet("ratings/distribution")]
        public async Task<IActionResult> GetRatingDistribution()
        {
            try
            {
                var distribution = await _analyticsService.GetRatingDistributionAsync();

                return Ok(ApiResponseDTO<IEnumerable<object>>.SuccessResponse(
                    distribution.Select(d => new
                    {
                        rating = d.Rating,
                        count = d.Count,
                        percentage = d.Percentage
                    }),
                    "Rating distribution retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving rating distribution",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GENERAL STATISTICS

        // get total bookings count
        [HttpGet("statistics/bookings/total")]
        public async Task<IActionResult> GetTotalBookingsCount()
        {
            try
            {
                var count = await _analyticsService.GetTotalBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total bookings count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total bookings count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get active bookings count
        [HttpGet("statistics/bookings/active")]
        public async Task<IActionResult> GetActiveBookingsCount()
        {
            try
            {
                var count = await _analyticsService.GetActiveBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Active bookings count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active bookings count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get pending bookings count
        [HttpGet("statistics/bookings/pending")]
        public async Task<IActionResult> GetPendingBookingsCount()
        {
            try
            {
                var count = await _analyticsService.GetPendingBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Pending bookings count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving pending bookings count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get completed bookings count
        [HttpGet("statistics/bookings/completed")]
        public async Task<IActionResult> GetCompletedBookingsCount()
        {
            try
            {
                var count = await _analyticsService.GetCompletedBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Completed bookings count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving completed bookings count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // get today's bookings count
        [HttpGet("statistics/bookings/today")]
        public async Task<IActionResult> GetTodayBookingsCount()
        {
            try
            {
                var count = await _analyticsService.GetTodayBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Today's bookings count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving today's bookings count",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}
