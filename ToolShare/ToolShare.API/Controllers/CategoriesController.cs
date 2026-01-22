using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.Category;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IToolCategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(IToolCategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
                return Ok(categoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving categories", error = ex.Message });
            }
        }

        // GET: api/Categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                    return NotFound(new { message = $"Category with ID {id} not found" });

                var categoryDto = _mapper.Map<CategoryResponseDTO>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category", error = ex.Message });
            }
        }

        // GET: api/Categories/name/Power Tools
        [HttpGet("name/{categoryName}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryByName(string categoryName)
        {
            try
            {
                var category = await _categoryService.GetCategoryByNameAsync(categoryName);
                if (category == null)
                    return NotFound(new { message = $"Category '{categoryName}' not found" });

                var categoryDto = _mapper.Map<CategoryResponseDTO>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the category", error = ex.Message });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = _mapper.Map<ToolCategory>(request);
                var createdCategory = await _categoryService.CreateCategoryAsync(category);
                var categoryDto = _mapper.Map<CategoryResponseDTO>(createdCategory);

                return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the category", error = ex.Message });
            }
        }

        // PUT: api/Categories/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            try
            {
                if (id != request.Id)
                    return BadRequest(new { message = "ID in URL does not match ID in request body" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = _mapper.Map<ToolCategory>(request);
                var updatedCategory = await _categoryService.UpdateCategoryAsync(category);
                var categoryDto = _mapper.Map<CategoryResponseDTO>(updatedCategory);

                return Ok(categoryDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the category", error = ex.Message });
            }
        }

        // DELETE: api/Categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the category", error = ex.Message });
            }
        }

        // GET: api/Categories/check-name?name=Power Tools
        [HttpGet("check-name")]
        public async Task<ActionResult<bool>> CheckCategoryNameUnique([FromQuery] string name, [FromQuery] int? excludeCategoryId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest(new { message = "Category name is required" });

                var isUnique = await _categoryService.IsCategoryNameUniqueAsync(name, excludeCategoryId);
                return Ok(new { categoryName = name, isUnique });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking category name", error = ex.Message });
            }
        }
    }
}
