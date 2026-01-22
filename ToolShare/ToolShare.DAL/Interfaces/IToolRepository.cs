using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IToolRepository : IGenericRepository<Tool>
    {
        Task<IEnumerable<Tool>> GetAvailableToolsAsync();
        Task<IEnumerable<Tool>> GetToolsByCategoryAsync(int categoryId);
        Task<IEnumerable<Tool>> GetToolsByOwnerAsync(int ownerId);
        Task<IEnumerable<Tool>> GetToolsByLocationAsync(string location);
        Task<IEnumerable<Tool>> SearchToolsAsync(string searchTerm);
        Task<IEnumerable<Tool>> FilterToolsAsync(int? categoryId, string? location,
            decimal? minPrice, decimal? maxPrice, bool? isAvailable);
        Task<Tool?> GetToolWithDetailsAsync(int toolId); // Include Category, Owner, Images
        Task<IEnumerable<Tool>> GetMostBorrowedToolsAsync(int count);
        Task<decimal> GetOwnerEarningsAsync(int ownerId);
    }
}
