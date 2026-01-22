using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Payment;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
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

        // GET: api/Payments
        // get all payments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

                return Ok(ApiResponseDTO<IEnumerable<PaymentResponseDTO>>.SuccessResponse(
                    paymentsDto,
                    "Payments retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payments",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/{id}
        // Get payment by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);

                if (payment == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Payment not found",
                        new List<string> { $"Payment with ID {id} does not exist" }
                    ));
                }

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/booking/{bookingId}
        // Get payment by booking ID
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);

                if (payment == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Payment not found",
                        new List<string> { $"Payment for booking ID {bookingId} does not exist" }
                    ));
                }

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/transaction/{transactionId}
        // get payment by transaction ID
        [HttpGet("transaction/{transactionId}")]
        public async Task<IActionResult> GetPaymentByTransactionId(string transactionId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByTransactionIdAsync(transactionId);

                if (payment == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Payment not found",
                        new List<string> { $"Payment with transaction ID {transactionId} does not exist" }
                    ));
                }

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/status/{status}
        // Get payments by status
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPaymentsByStatus(PaymentStatus status)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStatusAsync(status);
                var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

                return Ok(ApiResponseDTO<IEnumerable<PaymentResponseDTO>>.SuccessResponse(
                    paymentsDto,
                    $"{status} payments retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payments by status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/method/{method}
        // get payments by payment method
        [HttpGet("method/{method}")]
        public async Task<IActionResult> GetPaymentsByMethod(PaymentMethod method)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByMethodAsync(method);
                var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

                return Ok(ApiResponseDTO<IEnumerable<PaymentResponseDTO>>.SuccessResponse(
                    paymentsDto,
                    $"{method} payments retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payments by method",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/date-range
        // get payments by date range
        [HttpGet("date-range")]
        public async Task<IActionResult> GetPaymentsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);
                var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

                return Ok(ApiResponseDTO<IEnumerable<PaymentResponseDTO>>.SuccessResponse(
                    paymentsDto,
                    $"Payments between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving payments by date range",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/failed
        // Get all failed payments
        [HttpGet("failed")]
        public async Task<IActionResult> GetFailedPayments()
        {
            try
            {
                var payments = await _paymentService.GetFailedPaymentsAsync();
                var paymentsDto = _mapper.Map<IEnumerable<PaymentResponseDTO>>(payments);

                return Ok(ApiResponseDTO<IEnumerable<PaymentResponseDTO>>.SuccessResponse(
                    paymentsDto,
                    "Failed payments retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving failed payments",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Payments
        // create a new payment (manual creation)
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] ProcessPaymentRequest request)
        {
            try
            {
                // Map request to entity
                var payment = _mapper.Map<Payment>(request);

                // Create payment
                var createdPayment = await _paymentService.CreatePaymentAsync(payment);

                // Map to response DTO
                var paymentDto = _mapper.Map<PaymentResponseDTO>(createdPayment);

                return CreatedAtAction(
                    nameof(GetPaymentById),
                    new { id = createdPayment.PaymentId },
                    ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                        paymentDto,
                        "Payment created successfully"
                    )
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Payment creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Payment creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Payment creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Payments/process
        // process payment for a completed booking (recommended method)
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(
                    request.BookingId,
                    request.Amount,
                    request.PaymentMethod
                );

                var paymentDto = _mapper.Map<PaymentResponseDTO>(payment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    payment.Status == PaymentStatus.Completed
                        ? "Payment processed successfully"
                        : "Payment processing failed"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Payment processing failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Payment processing failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Payment processing failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while processing payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Payments/{id}
        // update an existing payment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] ProcessPaymentRequest request)
        {
            try
            {
                // Get existing payment
                var existingPayment = await _paymentService.GetPaymentByIdAsync(id);

                if (existingPayment == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Payment not found",
                        new List<string> { $"Payment with ID {id} does not exist" }
                    ));
                }

                // Map request to entity
                _mapper.Map(request, existingPayment);
                existingPayment.PaymentId = id; // Ensure ID is not changed

                // Update payment
                await _paymentService.UpdatePaymentAsync(existingPayment);

                // Get updated payment
                var updatedPayment = await _paymentService.GetPaymentByIdAsync(id);
                var paymentDto = _mapper.Map<PaymentResponseDTO>(updatedPayment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment updated successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Payments/{id}
        // delete a payment (only pending/failed)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);

                if (payment == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Payment not found",
                        new List<string> { $"Payment with ID {id} does not exist" }
                    ));
                }

                await _paymentService.DeletePaymentAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Payment deleted successfully",
                    "Payment deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Payment deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Payments/{id}/complete
        // mark payment as completed
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> MarkPaymentAsCompleted(int id, [FromBody] string transactionId)
        {
            try
            {
                await _paymentService.MarkPaymentAsCompletedAsync(id, transactionId);

                var updatedPayment = await _paymentService.GetPaymentByIdAsync(id);
                var paymentDto = _mapper.Map<PaymentResponseDTO>(updatedPayment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment marked as completed successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Payment not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Operation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while marking payment as completed",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Payments/{id}/fail
        // mark payment as failed
        [HttpPatch("{id}/fail")]
        public async Task<IActionResult> MarkPaymentAsFailed(int id)
        {
            try
            {
                await _paymentService.MarkPaymentAsFailedAsync(id);

                var updatedPayment = await _paymentService.GetPaymentByIdAsync(id);
                var paymentDto = _mapper.Map<PaymentResponseDTO>(updatedPayment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment marked as failed"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Payment not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while marking payment as failed",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Payments/{id}/refund
        // refund a completed payment
        [HttpPatch("{id}/refund")]
        public async Task<IActionResult> RefundPayment(int id)
        {
            try
            {
                await _paymentService.RefundPaymentAsync(id);

                var updatedPayment = await _paymentService.GetPaymentByIdAsync(id);
                var paymentDto = _mapper.Map<PaymentResponseDTO>(updatedPayment);

                return Ok(ApiResponseDTO<PaymentResponseDTO>.SuccessResponse(
                    paymentDto,
                    "Payment refunded successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Payment not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Refund failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while refunding payment",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/revenue/total
        // get total revenue
        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            try
            {
                var revenue = await _paymentService.GetTotalRevenueAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    "Total revenue retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total revenue",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/revenue/method/{method}
        // get revenue by payment method
        [HttpGet("revenue/method/{method}")]
        public async Task<IActionResult> GetRevenueByMethod(PaymentMethod method)
        {
            try
            {
                var revenue = await _paymentService.GetRevenueByMethodAsync(method);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Revenue from {method} payments retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue by method",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Payments/revenue/date-range
        // get revenue by date range
        [HttpGet("revenue/date-range")]
        public async Task<IActionResult> GetRevenueByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var revenue = await _paymentService.GetRevenueByDateRangeAsync(startDate, endDate);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Revenue between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue by date range",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}
