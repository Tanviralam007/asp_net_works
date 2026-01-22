using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IToolCategoryRepository : IGenericRepository<ToolCategory>
    {
        Task<ToolCategory?> GetCategoryByNameAsync(string categoryName);
        Task<IEnumerable<ToolCategory>> GetCategoriesWithToolCountAsync();
        Task<bool> IsCategoryNameUniqueAsync(string categoryName, int? excludeCategoryId = null);
        Task<bool> HasAssociatedToolsAsync(int categoryId);
    }
}
