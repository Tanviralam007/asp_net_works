using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.Payment;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponseDTO>> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound(new { message = $"Payment with ID {id} not found" });

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);
                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the payment", error = ex.Message });
            }
        }

        // GET: api/Payments/request/2
        [HttpGet("request/{requestId}")]
        public async Task<ActionResult<PaymentResponseDTO>> GetPaymentByRequestId(int requestId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByRequestIdAsync(requestId);
                if (payment == null)
                    return NotFound(new { message = $"Payment for request ID {requestId} not found" });

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);
                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the payment", error = ex.Message });
            }
        }

        // GET: api/Payments/user/1
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentResponseDTO>>> GetPaymentsByUser(int userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserAsync(userId);
                var paymentDtos = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);
                return Ok(paymentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user payments", error = ex.Message });
            }
        }

        // GET: api/Payments/owner/3/earnings
        [HttpGet("owner/{ownerId}/earnings")]
        public async Task<ActionResult<object>> GetOwnerEarnings(int ownerId)
        {
            try
            {
                var totalEarnings = await _paymentService.GetTotalPaymentsByOwnerAsync(ownerId);
                return Ok(new { ownerId, totalEarnings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating earnings", error = ex.Message });
            }
        }

        // POST: api/Payments?borrowerId=1
        [HttpPost]
        public async Task<ActionResult<PaymentResponseDTO>> ProcessPayment(
            [FromBody] CreatePaymentRequest request,
            [FromQuery] int borrowerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var payment = _mapper.Map<Payment>(request);
                var processedPayment = await _paymentService.ProcessPaymentAsync(payment, borrowerId);
                var paymentDto = _mapper.Map<PaymentResponseDTO>(processedPayment);

                return CreatedAtAction(nameof(GetPaymentById), new { id = paymentDto.Id }, paymentDto);
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
                return StatusCode(500, new { message = "An error occurred while processing payment", error = ex.Message });
            }
        }
    }
}
