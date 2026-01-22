using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByToolAsync(int toolId);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId);
        Task<Review> CreateReviewAsync(Review review, int reviewerId);
        Task<bool> DeleteReviewAsync(int reviewId, int userId, byte userRole);
        Task<double> GetAverageRatingForToolAsync(int toolId);
    }
}
