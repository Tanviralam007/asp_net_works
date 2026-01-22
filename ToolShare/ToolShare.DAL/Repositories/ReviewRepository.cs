using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsByToolAsync(int toolId)
        {
            return await _dbSet
                .Where(r => r.ToolId == toolId)
                .Include(r => r.User)
                .Include(r => r.Tool)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .Include(r => r.Tool)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByTypeAsync(byte reviewType)
        {
            return await _dbSet
                .Where(r => (byte)r.ReviewType == reviewType)
                .Include(r => r.Tool)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForToolAsync(int toolId)
        {
            var reviews = await _dbSet
                .Where(r => r.ToolId == toolId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        public async Task<double> GetAverageRatingForUserAsync(int userId)
        {
            var reviews = await _dbSet
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        public async Task<bool> HasUserReviewedToolAsync(int userId, int toolId)
        {
            return await _dbSet
                .AnyAsync(r => r.UserId == userId && r.ToolId == toolId);
        }
    }
}
