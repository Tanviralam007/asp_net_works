using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class ToolRepository : GenericRepository<Tool>, IToolRepository
    {
        public ToolRepository(AppDbContext context) : base(context) { }

        public override async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> GetAvailableToolsAsync()
        {
            return await _dbSet
                .Where(t => t.IsAvailable)
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> GetToolsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> GetToolsByOwnerAsync(int ownerId)
        {
            return await _dbSet
                .Where(t => t.OwnerId == ownerId)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> GetToolsByLocationAsync(string location)
        {
            return await _dbSet
                .Where(t => t.Location.Contains(location))
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> SearchToolsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(t => t.ToolName.Contains(searchTerm) ||
                           (t.Description != null && t.Description.Contains(searchTerm)))
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tool>> FilterToolsAsync(int? categoryId, string? location,
            decimal? minPrice, decimal? maxPrice, bool? isAvailable)
        {
            var query = _dbSet.AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(t => t.Location.Contains(location));

            if (minPrice.HasValue)
                query = query.Where(t => t.DailyRate >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(t => t.DailyRate <= maxPrice.Value);

            if (isAvailable.HasValue)
                query = query.Where(t => t.IsAvailable == isAvailable.Value);

            return await query
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .ToListAsync();
        }

        public async Task<Tool?> GetToolWithDetailsAsync(int toolId)
        {
            return await _dbSet
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.Id == toolId);
        }

        public async Task<IEnumerable<Tool>> GetMostBorrowedToolsAsync(int count)
        {
            return await _dbSet
                .Include(t => t.Category)
                .Include(t => t.Owner)
                .Include(t => t.BorrowRequests)
                .OrderByDescending(t => t.BorrowRequests.Count(br => (byte)br.Status == 3))
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetOwnerEarningsAsync(int ownerId)
        {
            return await _dbSet
                .Where(t => t.OwnerId == ownerId)
                .SelectMany(t => t.BorrowRequests)
                .Where(br => (byte)br.Status == 2)
                .Select(br => br.Payment != null ? br.Payment.Amount : 0) 
                .SumAsync();
        }
    }
}
