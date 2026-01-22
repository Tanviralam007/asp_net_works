using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.BLL.Services
{
    public class ToolCategoryService : IToolCategoryService
    {
        private readonly IToolCategoryRepository _categoryRepo;

        public ToolCategoryService(IToolCategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<ToolCategory>> GetAllCategoriesAsync()
        {
            return await _categoryRepo.GetAllAsync();
        }

        public async Task<ToolCategory?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepo.GetByIdAsync(id);
        }

        public async Task<ToolCategory?> GetCategoryByNameAsync(string categoryName)
        {
            return await _categoryRepo.GetCategoryByNameAsync(categoryName);
        }

        public async Task<ToolCategory> CreateCategoryAsync(ToolCategory category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required");

            if (!await _categoryRepo.IsCategoryNameUniqueAsync(category.CategoryName))
                throw new InvalidOperationException("Category name already exists");

            return await _categoryRepo.AddAsync(category);
        }

        public async Task<ToolCategory> UpdateCategoryAsync(ToolCategory category)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(category.Id);
            if (existingCategory == null)
                throw new KeyNotFoundException("Category not found");

            if (!await _categoryRepo.IsCategoryNameUniqueAsync(category.CategoryName, category.Id))
                throw new InvalidOperationException("Category name already exists");

            // Update the existing tracked entity
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;

            // Save changes directly
            await _categoryRepo.SaveChangesAsync();

            return existingCategory;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            var hasTools = await _categoryRepo.HasAssociatedToolsAsync(id);
            if (hasTools)
                throw new InvalidOperationException("Cannot delete category because it has associated tools");

            await _categoryRepo.DeleteAsync(id);
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string categoryName, int? excludeCategoryId = null)
        {
            return await _categoryRepo.IsCategoryNameUniqueAsync(categoryName, excludeCategoryId);
        }
    }
}
