using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.BorrowRequest;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRequestsController : ControllerBase
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IMapper _mapper;
        public BorrowRequestsController(IBorrowRequestService borrowRequestService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _mapper = mapper;
        }

        // GET: api/BorrowRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRequestResponseDTO>>> GetAllRequests()
        {
            try
            {
                var requests = await _borrowRequestService.GetAllRequestsAsync();
                var requestDtos = _mapper.Map<IEnumerable<BorrowRequestResponseDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving borrow requests", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowRequestResponseDTO>> GetRequestById(int id)
        {
            try
            {
                var request = await _borrowRequestService.GetRequestByIdAsync(id);
                if (request == null)
                    return NotFound(new { message = $"Borrow request with ID {id} not found" });

                var requestDto = _mapper.Map<BorrowRequestResponseDTO>(request);
                return Ok(requestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the request", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/borrower/{id}
        [HttpGet("borrower/{borrowerId}")]
        public async Task<ActionResult<IEnumerable<BorrowRequestResponseDTO>>> GetRequestsByBorrower(int borrowerId)
        {
            try
            {
                var requests = await _borrowRequestService.GetRequestsByBorrowerAsync(borrowerId);
                var requestDtos = _mapper.Map<IEnumerable<BorrowRequestResponseDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving borrower requests", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/tool/{id}
        [HttpGet("tool/{toolId}")]
        public async Task<ActionResult<IEnumerable<BorrowRequestResponseDTO>>> GetRequestsByTool(int toolId)
        {
            try
            {
                var requests = await _borrowRequestService.GetRequestsByToolAsync(toolId);
                var requestDtos = _mapper.Map<IEnumerable<BorrowRequestResponseDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tool requests", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/owner/{id}
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<BorrowRequestResponseDTO>>> GetRequestsByOwner(int ownerId)
        {
            try
            {
                var requests = await _borrowRequestService.GetRequestsByOwnerAsync(ownerId);
                var requestDtos = _mapper.Map<IEnumerable<BorrowRequestResponseDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving owner requests", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/owner/{id}/pending
        [HttpGet("owner/{ownerId}/pending")]
        public async Task<ActionResult<IEnumerable<BorrowRequestResponseDTO>>> GetPendingRequestsForOwner(int ownerId)
        {
            try
            {
                var requests = await _borrowRequestService.GetPendingRequestsForOwnerAsync(ownerId);
                var requestDtos = _mapper.Map<IEnumerable<BorrowRequestResponseDTO>>(requests);
                return Ok(requestDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving pending requests", error = ex.Message });
            }
        }

        // GET: api/BorrowRequests/calculate-cost?toolId={id}&startDate=2026-01-15&endDate=2026-01-20
        [HttpGet("calculate-cost")]
        public async Task<ActionResult<decimal>> CalculateRentalCost(
            [FromQuery] int toolId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if(startDate < DateTime.Today)
                    return BadRequest(new { message = "Start date cannot be in the past" });
                if(endDate <= startDate)
                    return BadRequest(new { message = "End date must be after start date" });

                var cost = await _borrowRequestService.CalculateRentalCostAsync(toolId, startDate, endDate);
                return Ok(new { toolId, startDate, endDate, estimatedCost = cost });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating cost", error = ex.Message });
            }
        }

        // POST: api/BorrowRequests?borrowerId={id}
        [HttpPost]
        public async Task<ActionResult<BorrowRequestResponseDTO>> CreateRequest(
            [FromBody] CreateBorrowRequestRequest request,
            [FromQuery] int borrowerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var borrowRequest = _mapper.Map<BorrowRequest>(request);
                var createdRequest = await _borrowRequestService.CreateRequestAsync(borrowRequest, borrowerId);
                var requestDto = _mapper.Map<BorrowRequestResponseDTO>(createdRequest);

                return CreatedAtAction(nameof(GetRequestById), new { id = requestDto.Id }, requestDto);
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
                return StatusCode(500, new { message = "An error occurred while creating the request", error = ex.Message });
            }
        }

        // POST: api/BorrowRequests/{id}/approve?ownerId={id}
        [HttpPost("{requestId}/approve")]
        public async Task<ActionResult<BorrowRequestResponseDTO>> ApproveRequest(int requestId, [FromQuery] int ownerId)
        {
            try
            {
                var approvedRequest = await _borrowRequestService.ApproveRequestAsync(requestId, ownerId);
                var requestDto = _mapper.Map<BorrowRequestResponseDTO>(approvedRequest);
                return Ok(requestDto);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while approving the request", error = ex.Message });
            }
        }

        // POST: api/BorrowRequests/{id}/reject?ownerId={id}
        [HttpPost("{requestId}/reject")]
        public async Task<ActionResult<BorrowRequestResponseDTO>> RejectRequest(int requestId, [FromQuery] int ownerId)
        {
            try
            {
                var rejectedRequest = await _borrowRequestService.RejectRequestAsync(requestId, ownerId);
                var requestDto = _mapper.Map<BorrowRequestResponseDTO>(rejectedRequest);
                return Ok(requestDto);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while rejecting the request", error = ex.Message });
            }
        }

        // DELETE: api/BorrowRequests/1/cancel?borrowerId=1
        [HttpDelete("{requestId}/cancel")]
        public async Task<ActionResult> CancelRequest(int requestId, [FromQuery] int borrowerId)
        {
            try
            {
                var result = await _borrowRequestService.CancelRequestAsync(requestId, borrowerId);
                if (!result)
                    return NotFound(new { message = $"Request with ID {requestId} not found" });

                return Ok(new { message = "Request cancelled successfully" });
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while cancelling the request", error = ex.Message });
            }
        }
    }
}
