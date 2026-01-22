using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Feedback;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbacksController(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }

        // GET: api/Feedbacks
        // get all feedbacks
        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    "Feedbacks retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving feedbacks",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/{id}
        // get feedback by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);

                if (feedback == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Feedback not found",
                        new List<string> { $"Feedback with ID {id} does not exist" }
                    ));
                }

                var feedbackDto = _mapper.Map<FeedbackResponseDTO>(feedback);

                return Ok(ApiResponseDTO<FeedbackResponseDTO>.SuccessResponse(
                    feedbackDto,
                    "Feedback retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving feedback",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/booking/{bookingId}
        // get feedback by booking ID
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetFeedbackByBookingId(int bookingId)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByBookingIdAsync(bookingId);

                if (feedback == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Feedback not found",
                        new List<string> { $"Feedback for booking ID {bookingId} does not exist" }
                    ));
                }

                var feedbackDto = _mapper.Map<FeedbackResponseDTO>(feedback);

                return Ok(ApiResponseDTO<FeedbackResponseDTO>.SuccessResponse(
                    feedbackDto,
                    "Feedback retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving feedback",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/user/{userId}
        // get all feedbacks submitted by a user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFeedbacksByUserId(int userId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByUserIdAsync(userId);
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    $"Feedbacks by user {userId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user feedbacks",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/driver/{driverId}
        // get all feedbacks for a specific driver
        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetFeedbacksByDriverId(int driverId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByDriverIdAsync(driverId);
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    $"Feedbacks for driver {driverId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver feedbacks",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/rating/{rating}
        // get feedbacks by specific rating
        [HttpGet("rating/{rating}")]
        public async Task<IActionResult> GetFeedbacksByRating(int rating)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByRatingAsync(rating);
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    $"{rating}-star feedbacks retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid rating",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving feedbacks by rating",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/low-rated/{threshold}
        // get feedbacks below a certain rating threshold
        [HttpGet("low-rated/{threshold}")]
        public async Task<IActionResult> GetLowRatedFeedbacks(int threshold = 3)
        {
            try
            {
                var feedbacks = await _feedbackService.GetLowRatedFeedbacksAsync(threshold);
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    $"Feedbacks rated {threshold} or below retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid threshold",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving low-rated feedbacks",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/recent/{count}
        // get most recent feedbacks
        [HttpGet("recent/{count}")]
        public async Task<IActionResult> GetRecentFeedbacks(int count = 10)
        {
            try
            {
                var feedbacks = await _feedbackService.GetRecentFeedbacksAsync(count);
                var feedbacksDto = _mapper.Map<IEnumerable<FeedbackResponseDTO>>(feedbacks);

                return Ok(ApiResponseDTO<IEnumerable<FeedbackResponseDTO>>.SuccessResponse(
                    feedbacksDto,
                    $"Most recent {count} feedbacks retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid count",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving recent feedbacks",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Feedbacks
        // create a new feedback
        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            try
            {
                var feedback = _mapper.Map<Feedback>(request);
                var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedback);
                var feedbackDto = _mapper.Map<FeedbackResponseDTO>(createdFeedback);

                return CreatedAtAction(
                    nameof(GetFeedbackById),
                    new { id = createdFeedback.FeedbackId },
                    ApiResponseDTO<FeedbackResponseDTO>.SuccessResponse(
                        feedbackDto,
                        "Feedback submitted successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while submitting feedback",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Feedbacks/{id}
        // update an existing feedback
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] UpdateFeedbackRequest request)
        {
            try
            {
                var existingFeedback = await _feedbackService.GetFeedbackByIdAsync(id);

                if (existingFeedback == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Feedback not found",
                        new List<string> { $"Feedback with ID {id} does not exist" }
                    ));
                }

                _mapper.Map(request, existingFeedback);
                existingFeedback.FeedbackId = id;

                await _feedbackService.UpdateFeedbackAsync(existingFeedback);

                var updatedFeedback = await _feedbackService.GetFeedbackByIdAsync(id);
                var feedbackDto = _mapper.Map<FeedbackResponseDTO>(updatedFeedback);

                return Ok(ApiResponseDTO<FeedbackResponseDTO>.SuccessResponse(
                    feedbackDto,
                    "Feedback updated successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating feedback",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Feedbacks/{id}
        // delete a feedback
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);

                if (feedback == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Feedback not found",
                        new List<string> { $"Feedback with ID {id} does not exist" }
                    ));
                }

                await _feedbackService.DeleteFeedbackAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Feedback deleted successfully",
                    "Feedback deletion successful"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting feedback",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Feedbacks/statistics/total
        // get total feedback count
        [HttpGet("statistics/total")]
        public async Task<IActionResult> GetTotalFeedbackCount()
        {
            var count = await _feedbackService.GetTotalFeedbacksCountAsync();

            return Ok(ApiResponseDTO<int>.SuccessResponse(
                count,
                "Total feedback count retrieved successfully"
            ));
        }

        // GET: api/Feedbacks/statistics/overall-average
        // get overall average rating
        [HttpGet("statistics/overall-average")]
        public async Task<IActionResult> GetOverallAverageRating()
        {
            var averageRating = await _feedbackService.GetOverallAverageRatingAsync();

            return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                averageRating,
                "Overall average rating retrieved successfully"
            ));
        }
    }
}