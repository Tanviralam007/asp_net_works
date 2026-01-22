using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class ToolCategoryRepository : GenericRepository<ToolCategory>, IToolCategoryRepository
    {
        public ToolCategoryRepository(AppDbContext context) : base(context) { }

        public async Task<ToolCategory?> GetCategoryByNameAsync(string categoryName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == categoryName.ToLower());
        }

        public async Task<IEnumerable<ToolCategory>> GetCategoriesWithToolCountAsync()
        {
            return await _dbSet
                .Include(c => c.Tools)
                .OrderByDescending(c => c.Tools.Count)
                .ToListAsync();
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string categoryName, int? excludeCategoryId = null)
        {
            var query = _dbSet.Where(c => c.CategoryName.ToLower() == categoryName.ToLower());

            if (excludeCategoryId.HasValue)
                query = query.Where(c => c.Id != excludeCategoryId.Value);

            return !await query.AnyAsync();
        }

        public async Task<bool> HasAssociatedToolsAsync(int categoryId)
        {
            return await _context.Tools.AnyAsync(t => t.CategoryId == categoryId);
        }
    }
}
