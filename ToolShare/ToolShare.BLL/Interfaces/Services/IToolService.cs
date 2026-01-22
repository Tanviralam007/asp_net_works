using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IToolService
    {
        Task<IEnumerable<Tool>> GetAllToolsAsync();
        Task<Tool?> GetToolByIdAsync(int id);
        Task<Tool?> GetToolWithDetailsAsync(int id);
        Task<IEnumerable<Tool>> GetAvailableToolsAsync();
        Task<IEnumerable<Tool>> GetToolsByCategoryAsync(int categoryId);
        Task<IEnumerable<Tool>> GetToolsByOwnerAsync(int ownerId);
        Task<IEnumerable<Tool>> SearchToolsAsync(string searchTerm);
        Task<IEnumerable<Tool>> FilterToolsAsync(int? categoryId, string? location,
            decimal? minPrice, decimal? maxPrice, bool? isAvailable);
        Task<Tool> CreateToolAsync(Tool tool, int ownerId);
        Task<Tool> UpdateToolAsync(Tool tool, int ownerId);
        Task<bool> DeleteToolAsync(int toolId, int userId, byte userRole);
        Task<IEnumerable<Tool>> GetMostBorrowedToolsAsync(int count);
    }
}
