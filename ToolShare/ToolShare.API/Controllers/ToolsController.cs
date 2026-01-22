using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.Tool;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolService;
        private readonly IMapper _mapper;

        public ToolsController(IToolService toolService, IMapper mapper)
        {
            _toolService = toolService;
            _mapper = mapper;
        }

        // GET: api/Tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> GetAllTools()
        {
            try
            {
                var tools = await _toolService.GetAllToolsAsync();
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tools", error = ex.Message });
            }
        }

        // GET: api/Tools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToolResponseDTO>> GetToolById(int id)
        {
            try
            {
                var tool = await _toolService.GetToolWithDetailsAsync(id);
                if (tool == null)
                    return NotFound(new { message = $"Tool with ID {id} not found" });

                var toolDto = _mapper.Map<ToolResponseDTO>(tool);
                return Ok(toolDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the tool", error = ex.Message });
            }
        }

        // GET: api/Tools/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> GetAvailableTools()
        {
            try
            {
                var tools = await _toolService.GetAvailableToolsAsync();
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving available tools", error = ex.Message });
            }
        }

        // GET: api/Tools/category/1
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> GetToolsByCategory(int categoryId)
        {
            try
            {
                var tools = await _toolService.GetToolsByCategoryAsync(categoryId);
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tools by category", error = ex.Message });
            }
        }

        // GET: api/Tools/owner/3
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> GetToolsByOwner(int ownerId)
        {
            try
            {
                var tools = await _toolService.GetToolsByOwnerAsync(ownerId);
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tools by owner", error = ex.Message });
            }
        }

        // GET: api/Tools/search?term=drill
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> SearchTools([FromQuery] string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                    return BadRequest(new { message = "Search term is required" });

                var tools = await _toolService.SearchToolsAsync(term);
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching tools", error = ex.Message });
            }
        }

        // GET: api/Tools/filter?categoryId=1&location=Dhaka&minPrice=50&maxPrice=300&isAvailable=true
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> FilterTools(
            [FromQuery] int? categoryId,
            [FromQuery] string? location,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? isAvailable)
        {
            try
            {
                if(minPrice.HasValue && minPrice < 0)
                    return BadRequest(new { message = "Minimum price cannot be negative" });
                if(maxPrice.HasValue && maxPrice < 0)
                    return BadRequest(new { message = "Maximum price cannot be negative" });
                if(minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                    return BadRequest(new { message = "Minimum price cannot be greater than maximum price" });

                var tools = await _toolService.FilterToolsAsync(categoryId, location, minPrice, maxPrice, isAvailable);
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while filtering tools", error = ex.Message });
            }
        }

        // GET: api/Tools/most-borrowed?count=5
        [HttpGet("most-borrowed")]
        public async Task<ActionResult<IEnumerable<ToolResponseDTO>>> GetMostBorrowedTools([FromQuery] int count = 10)
        {
            try
            {
                if (count <= 0)
                    return BadRequest(new { message = "Count must be greater than zero" });

                var tools = await _toolService.GetMostBorrowedToolsAsync(count);
                var toolDtos = _mapper.Map<IEnumerable<ToolResponseDTO>>(tools);
                return Ok(toolDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving most borrowed tools", error = ex.Message });
            }
        }

        // POST: api/Tools?ownerId={id}
        [HttpPost]
        public async Task<ActionResult<ToolResponseDTO>> CreateTool([FromBody] CreateToolRequest request, [FromQuery] int ownerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var tool = _mapper.Map<Tool>(request);
                var createdTool = await _toolService.CreateToolAsync(tool, ownerId);
                var toolDto = _mapper.Map<ToolResponseDTO>(createdTool);

                return CreatedAtAction(nameof(GetToolById), new { id = toolDto.Id }, toolDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
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
                return StatusCode(500, new { message = "An error occurred while creating the tool", error = ex.Message });
            }
        }

        // PUT: api/Tools/5?ownerId=3
        [HttpPut("{id}")]
        public async Task<ActionResult<ToolResponseDTO>> UpdateTool(int id, [FromBody] UpdateToolRequest request, [FromQuery] int ownerId)
        {
            try
            {
                if (id != request.Id)
                    return BadRequest(new { message = "ID in URL does not match ID in request body" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var tool = _mapper.Map<Tool>(request);
                var updatedTool = await _toolService.UpdateToolAsync(tool, ownerId);
                var toolDto = _mapper.Map<ToolResponseDTO>(updatedTool);

                return Ok(toolDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the tool", error = ex.Message });
            }
        }

        // DELETE: api/Tools/5?userId=3&userRole=1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTool(int id, [FromQuery] int userId, [FromQuery] byte userRole)
        {
            try
            {
                var result = await _toolService.DeleteToolAsync(id, userId, userRole);
                if (!result)
                    return NotFound(new { message = $"Tool with ID {id} not found" });

                return Ok(new { message = "Tool deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the tool", error = ex.Message });
            }
        }
    }
}
