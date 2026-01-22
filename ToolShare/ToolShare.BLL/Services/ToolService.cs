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
    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepo;
        private readonly IUserRepository _userRepo;
        private readonly IToolCategoryRepository _categoryRepo;

        public ToolService(IToolRepository toolRepo, IUserRepository userRepo, IToolCategoryRepository categoryRepo)
        {
            _toolRepo = toolRepo;
            _userRepo = userRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Tool>> GetAllToolsAsync()
        {
            return await _toolRepo.GetAllAsync();
        }

        public async Task<Tool?> GetToolByIdAsync(int id)
        {
            return await _toolRepo.GetByIdAsync(id);
        }

        public async Task<Tool?> GetToolWithDetailsAsync(int id)
        {
            return await _toolRepo.GetToolWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Tool>> GetAvailableToolsAsync()
        {
            return await _toolRepo.GetAvailableToolsAsync();
        }

        public async Task<IEnumerable<Tool>> GetToolsByCategoryAsync(int categoryId)
        {
            return await _toolRepo.GetToolsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Tool>> GetToolsByOwnerAsync(int ownerId)
        {
            return await _toolRepo.GetToolsByOwnerAsync(ownerId);
        }

        public async Task<IEnumerable<Tool>> SearchToolsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Search term cannot be empty");

            return await _toolRepo.SearchToolsAsync(searchTerm);
        }

        public async Task<IEnumerable<Tool>> FilterToolsAsync(int? categoryId, string? location,
            decimal? minPrice, decimal? maxPrice, bool? isAvailable)
        {
            return await _toolRepo.FilterToolsAsync(categoryId, location, minPrice, maxPrice, isAvailable);
        }

        public async Task<Tool> CreateToolAsync(Tool tool, int ownerId)
        {
            // Validate owner
            var owner = await _userRepo.GetByIdAsync(ownerId);
            if (owner == null)
                throw new KeyNotFoundException("Owner not found");

            if ((byte)owner.Role != 2) // ToolOwner role = 2
                throw new UnauthorizedAccessException("Only tool owners can add tools");

            if (owner.IsBlocked)
                throw new InvalidOperationException("Blocked users cannot add tools");

            // Validate category
            var category = await _categoryRepo.GetByIdAsync(tool.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            // Validate tool data
            if (string.IsNullOrWhiteSpace(tool.ToolName))
                throw new ArgumentException("Tool name is required");

            if (tool.DailyRate <= 0)
                throw new ArgumentException("Daily rate must be greater than zero");

            tool.OwnerId = ownerId;
            tool.IsAvailable = true;
            tool.CreatedAt = DateTime.Now;

            return await _toolRepo.AddAsync(tool);
        }

        public async Task<Tool> UpdateToolAsync(Tool tool, int ownerId)
        {
            var existingTool = await _toolRepo.GetByIdAsync(tool.Id);
            if (existingTool == null)
                throw new KeyNotFoundException("Tool not found");

            // Authorization: Only owner can update
            if (existingTool.OwnerId != ownerId)
                throw new UnauthorizedAccessException("You can only update your own tools");

            // Validate category
            var category = await _categoryRepo.GetByIdAsync(tool.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            existingTool.ToolName = tool.ToolName;
            existingTool.Description = tool.Description;
            existingTool.DailyRate = tool.DailyRate;
            existingTool.Location = tool.Location;
            existingTool.CategoryId = tool.CategoryId;
            existingTool.IsAvailable = tool.IsAvailable;

            return await _toolRepo.UpdateAsync(existingTool);
        }

        public async Task<bool> DeleteToolAsync(int toolId, int userId, byte userRole)
        {
            var tool = await _toolRepo.GetByIdAsync(toolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            // Authorization: Owner or Admin can delete
            bool isAdmin = userRole == 3; // Admin role = 3
            bool isOwner = userRole == 2 && tool.OwnerId == userId; // ToolOwner role = 2

            if(isAdmin || isOwner)
            {
                return await _toolRepo.DeleteAsync(toolId);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this tool");
            }
        }

        public async Task<IEnumerable<Tool>> GetMostBorrowedToolsAsync(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero");

            return await _toolRepo.GetMostBorrowedToolsAsync(count);
        }
    }
}
