using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IFeedbackService
    {
        // curd signatures
        Task<Feedback?> GetFeedbackByIdAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> CreateFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int feedbackId);

        // feedback lookup signatures
        Task<Feedback?> GetFeedbackByBookingIdAsync(int bookingId);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId);
        Task<IEnumerable<Feedback>> GetFeedbacksByDriverIdAsync(int driverId);

        // feedback filtering signatures
        Task<IEnumerable<Feedback>> GetFeedbacksByRatingAsync(int rating);
        Task<IEnumerable<Feedback>> GetLowRatedFeedbacksAsync(int threshold);
        Task<IEnumerable<Feedback>> GetRecentFeedbacksAsync(int count);

        // feedback submission signatures
        Task<Feedback> SubmitFeedbackAsync(int bookingId, int userId, int rating, string? comments);

        // rating calculations signatures
        Task<decimal> GetAverageRatingForDriverAsync(int driverId);
        Task UpdateDriverRatingAfterFeedbackAsync(int driverId);

        // statistics signatures
        Task<int> GetTotalFeedbacksCountAsync();
        Task<decimal> GetOverallAverageRatingAsync();
    }
}
