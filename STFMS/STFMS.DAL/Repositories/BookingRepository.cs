using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) 
            : base(context)
        {

        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Include(b => b.Vehicle)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDriverIdAsync(int driverId)
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Vehicle)
                .Where(b => b.DriverId == driverId)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(BookingStatus status)
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Where(b => b.Status == status)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByServiceTypeAsync(ServiceType serviceType)
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.ServiceType == serviceType)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Where(b => b.BookingTime >= startDate && b.BookingTime <= endDate)
                .OrderBy(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Include(b => b.Vehicle)
                .Include(b => b.Payment)
                .Include(b => b.Feedback)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetPendingBookingsAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .Where(b => b.Status == BookingStatus.Pending)
                .OrderBy(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetCompletedBookingsAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Where(b => b.Status == BookingStatus.Completed)
                .OrderByDescending(b => b.CompletionTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await _dbSet
                .Include(b => b.User)
                .Include(b => b.Driver)
                    .ThenInclude(d => d!.User)
                .Include(b => b.Vehicle)
                .Where(b => b.Status == BookingStatus.Assigned || b.Status == BookingStatus.InProgress)
                .ToListAsync();
        }

        public async Task<int> GetTotalBookingsByUserAsync(int userId)
        {
            return await _dbSet
                .CountAsync(b => b.UserId == userId);
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(b => b.CompletionTime >= startDate &&
                           b.CompletionTime <= endDate &&
                           b.Status == BookingStatus.Completed)
                .SumAsync(b => b.ActualFare ?? 0);
        }

        public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _dbSet.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = status;

                if (status == BookingStatus.InProgress && booking.PickupTime == null)
                {
                    booking.PickupTime = DateTime.UtcNow;
                }
                else if (status == BookingStatus.Completed && booking.CompletionTime == null)
                {
                    booking.CompletionTime = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignDriverAndVehicleAsync(int bookingId, int driverId, int vehicleId)
        {
            var booking = await _dbSet.FindAsync(bookingId);
            if (booking != null)
            {
                booking.DriverId = driverId;
                booking.VehicleId = vehicleId;
                booking.Status = BookingStatus.Assigned;
                await _context.SaveChangesAsync();
            }
        }
    }
}
