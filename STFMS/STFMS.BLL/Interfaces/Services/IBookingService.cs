using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IBookingService
    {
        // curd signatures
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> CreateBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int bookingId);

        // booking lookup with details signatures
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(int driverId);

        // booking filtering signatures
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<IEnumerable<Booking>> GetBookingsByServiceTypeAsync(ServiceType serviceType);
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Booking>> GetPendingBookingsAsync();
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<IEnumerable<Booking>> GetCompletedBookingsAsync();

        // booking workflow signatures
        Task UpdateBookingStatusAsync(int bookingId, BookingStatus status);
        Task AssignDriverToBookingAsync(int bookingId, int driverId, int vehicleId);
        Task StartRideAsync(int bookingId);
        Task CompleteRideAsync(int bookingId, decimal actualFare);
        Task CancelBookingAsync(int bookingId);

        // fare calculation signatures
        Task<decimal> CalculateFareAsync(string pickupLocation, string dropoffLocation, ServiceType serviceType);
        Task<decimal> EstimateFareAsync(double distanceKm, ServiceType serviceType);

        // statistics signatures
        Task<int> GetTotalBookingsCountAsync();
        Task<int> GetTotalBookingsByUserAsync(int userId);
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
