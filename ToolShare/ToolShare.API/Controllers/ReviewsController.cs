using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.Review;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET: api/Reviews/tool/1
        [HttpGet("tool/{toolId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByTool(int toolId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByToolAsync(toolId);
                var reviewDtos = _mapper.Map<IEnumerable<ReviewResponseDTO>>(reviews);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tool reviews", error = ex.Message });
            }
        }

        // GET: api/Reviews/user/1
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByUser(int userId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByUserAsync(userId);
                var reviewDtos = _mapper.Map<IEnumerable<ReviewResponseDTO>>(reviews);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user reviews", error = ex.Message });
            }
        }

        // GET: api/Reviews/tool/1/average-rating
        [HttpGet("tool/{toolId}/average-rating")]
        public async Task<ActionResult<object>> GetAverageRatingForTool(int toolId)
        {
            try
            {
                var averageRating = await _reviewService.GetAverageRatingForToolAsync(toolId);
                return Ok(new { toolId, averageRating });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating average rating", error = ex.Message });
            }
        }

        // POST: api/Reviews?reviewerId=1
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDTO>> CreateReview(
            [FromBody] CreateReviewRequest request,
            [FromQuery] int reviewerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var review = _mapper.Map<Review>(request);
                var createdReview = await _reviewService.CreateReviewAsync(review, reviewerId);
                var reviewDto = _mapper.Map<ReviewResponseDTO>(createdReview);

                return CreatedAtAction(nameof(GetReviewsByTool), new { toolId = reviewDto.ToolId }, reviewDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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
                return StatusCode(500, new { message = "An error occurred while creating the review", error = ex.Message });
            }
        }

        // DELETE: api/Reviews/5?userId=1&userRole=0
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id, [FromQuery] int userId, [FromQuery] byte userRole)
        {
            try
            {
                var result = await _reviewService.DeleteReviewAsync(id, userId, userRole);
                if (!result)
                    return NotFound(new { message = $"Review with ID {id} not found" });

                return Ok(new { message = "Review deleted successfully" });
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
                return StatusCode(500, new { message = "An error occurred while deleting the review", error = ex.Message });
            }
        }
    }
}
