using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(int driverId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status);
        Task<IEnumerable<Booking>> GetBookingsByServiceTypeAsync(ServiceType serviceType);
        Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetPendingBookingsAsync();
        Task<IEnumerable<Booking>> GetCompletedBookingsAsync();
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<int> GetTotalBookingsByUserAsync(int userId);
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task UpdateBookingStatusAsync(int bookingId, BookingStatus status);
        Task AssignDriverAndVehicleAsync(int bookingId, int driverId, int vehicleId);
    }
}
