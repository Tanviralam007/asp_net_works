using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<Feedback?> GetFeedbackByBookingIdAsync(int bookingId);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId);
        Task<IEnumerable<Feedback>> GetFeedbacksByDriverIdAsync(int driverId);
        Task<IEnumerable<Feedback>> GetFeedbacksByRatingAsync(int rating);
        Task<IEnumerable<Feedback>> GetLowRatedFeedbacksAsync(int threshold);
        Task<decimal> GetAverageRatingForDriverAsync(int driverId);
        Task<IEnumerable<Feedback>> GetRecentFeedbacksAsync(int count);
    }
}
