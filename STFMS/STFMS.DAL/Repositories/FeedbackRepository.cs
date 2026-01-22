using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) 
            : base(context)
        {

        }

        public async Task<Feedback?> GetFeedbackByBookingIdAsync(int bookingId)
        {
            return await _dbSet
                .Include(f => f.User)
                .Include(f => f.Booking)
                .FirstOrDefaultAsync(f => f.BookingId == bookingId);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(f => f.Booking)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByDriverIdAsync(int driverId)
        {
            return await _dbSet
                .Include(f => f.User)
                .Include(f => f.Booking)
                .Where(f => f.Booking.DriverId == driverId)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByRatingAsync(int rating)
        {
            return await _dbSet
                .Include(f => f.User)
                .Include(f => f.Booking)
                .Where(f => f.Rating == rating)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetLowRatedFeedbacksAsync(int threshold)
        {
            return await _dbSet
                .Include(f => f.User)
                .Include(f => f.Booking)
                    .ThenInclude(b => b.Driver)
                        .ThenInclude(d => d!.User)
                .Where(f => f.Rating <= threshold)
                .OrderByDescending(f => f.SubmittedDate)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageRatingForDriverAsync(int driverId)
        {
            var feedbacks = await _dbSet
                .Where(f => f.Booking.DriverId == driverId)
                .ToListAsync();

            if (feedbacks.Count == 0)
                return 0;

            return (decimal)feedbacks.Average(f => f.Rating);
        }

        public async Task<IEnumerable<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            return await _dbSet
                .Include(f => f.User)
                .Include(f => f.Booking)
                .OrderByDescending(f => f.SubmittedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
