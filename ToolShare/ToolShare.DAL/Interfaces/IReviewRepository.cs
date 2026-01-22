using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByToolAsync(int toolId);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId);
        Task<IEnumerable<Review>> GetReviewsByTypeAsync(byte reviewType);
        Task<double> GetAverageRatingForToolAsync(int toolId);
        Task<double> GetAverageRatingForUserAsync(int userId);
        Task<bool> HasUserReviewedToolAsync(int userId, int toolId);
    }
}
