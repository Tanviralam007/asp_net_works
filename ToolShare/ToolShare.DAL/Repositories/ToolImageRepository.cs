using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class ToolImageRepository : GenericRepository<ToolImage>, IToolImageRepository
    {
        public ToolImageRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ToolImage>> GetImagesByToolAsync(int toolId)
        {
            return await _dbSet
                .Where(ti => ti.ToolId == toolId)
                .OrderByDescending(ti => ti.IsPrimary)
                .ThenBy(ti => ti.CreatedAt)
                .ToListAsync();
        }

        public async Task<ToolImage?> GetPrimaryImageAsync(int toolId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ti => ti.ToolId == toolId && ti.IsPrimary);
        }

        public async Task<bool> DeleteImagesByToolAsync(int toolId)
        {
            var images = await _dbSet.Where(ti => ti.ToolId == toolId).ToListAsync();
            if (!images.Any()) return false;

            _dbSet.RemoveRange(images);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
