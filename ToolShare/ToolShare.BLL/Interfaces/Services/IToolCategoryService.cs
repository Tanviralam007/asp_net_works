using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IToolCategoryService
    {
        Task<IEnumerable<ToolCategory>> GetAllCategoriesAsync();
        Task<ToolCategory?> GetCategoryByIdAsync(int id);
        Task<ToolCategory?> GetCategoryByNameAsync(string categoryName);
        Task<ToolCategory> CreateCategoryAsync(ToolCategory category);
        Task<ToolCategory> UpdateCategoryAsync(ToolCategory category);
        Task DeleteCategoryAsync(int id);
        Task<bool> IsCategoryNameUniqueAsync(string categoryName, int? excludeCategoryId = null);
    }
}
