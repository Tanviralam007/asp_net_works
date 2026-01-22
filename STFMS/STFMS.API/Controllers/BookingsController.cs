using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Booking;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        // GET: api/Bookings
        // get all bookings
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    "Bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/{id}
        // get booking by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Booking not found",
                        new List<string> { $"Booking with ID {id} does not exist" }
                    ));
                }

                var bookingDto = _mapper.Map<BookingResponseDTO>(booking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Booking retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving booking",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/{id}/with-details
        // get booking with full details (user, driver, vehicle, payment, feedback)
        [HttpGet("{id}/with-details")]
        public async Task<IActionResult> GetBookingWithDetails(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingWithDetailsAsync(id);

                if (booking == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Booking not found",
                        new List<string> { $"Booking with ID {id} does not exist" }
                    ));
                }

                var bookingDto = _mapper.Map<BookingWithDetailsResponseDTO>(booking);

                return Ok(ApiResponseDTO<BookingWithDetailsResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Booking with details retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving booking details",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/user/{userId}
        // get all bookings for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    $"Bookings for user {userId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/driver/{driverId}
        // get all bookings for a specific driver
        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetBookingsByDriverId(int driverId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByDriverIdAsync(driverId);
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    $"Bookings for driver {driverId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/status/{status}
        // get bookings by status
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetBookingsByStatus(BookingStatus status)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByStatusAsync(status);
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    $"{status} bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving bookings by status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/service-type/{serviceType}
        // get bookings by service type
        [HttpGet("service-type/{serviceType}")]
        public async Task<IActionResult> GetBookingsByServiceType(ServiceType serviceType)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByServiceTypeAsync(serviceType);
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    $"{serviceType} bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving bookings by service type",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/pending
        // get all pending bookings
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingBookings()
        {
            try
            {
                var bookings = await _bookingService.GetPendingBookingsAsync();
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    "Pending bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving pending bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/active
        // get all active bookings (Assigned + InProgress)
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBookings()
        {
            try
            {
                var bookings = await _bookingService.GetActiveBookingsAsync();
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    "Active bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/completed
        // get all completed bookings
        [HttpGet("completed")]
        public async Task<IActionResult> GetCompletedBookings()
        {
            try
            {
                var bookings = await _bookingService.GetCompletedBookingsAsync();
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    "Completed bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving completed bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/date-range
        // get bookings by date range
        [HttpGet("date-range")]
        public async Task<IActionResult> GetBookingsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByDateRangeAsync(startDate, endDate);
                var bookingsDto = _mapper.Map<IEnumerable<BookingResponseDTO>>(bookings);

                return Ok(ApiResponseDTO<IEnumerable<BookingResponseDTO>>.SuccessResponse(
                    bookingsDto,
                    $"Bookings between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving bookings by date range",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Bookings
        // create a new booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                // Map request to entity
                var booking = _mapper.Map<Booking>(request);

                // Create booking
                var createdBooking = await _bookingService.CreateBookingAsync(booking);

                // Map to response DTO
                var bookingDto = _mapper.Map<BookingResponseDTO>(createdBooking);

                return CreatedAtAction(
                    nameof(GetBookingById),
                    new { id = createdBooking.BookingId },
                    ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                        bookingDto,
                        "Booking created successfully"
                    )
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Booking creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating booking",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Bookings/{id}
        // update an existing booking
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] CreateBookingRequest request)
        {
            try
            {
                // Get existing booking
                var existingBooking = await _bookingService.GetBookingByIdAsync(id);

                if (existingBooking == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Booking not found",
                        new List<string> { $"Booking with ID {id} does not exist" }
                    ));
                }

                // Map request to entity
                _mapper.Map(request, existingBooking);
                existingBooking.BookingId = id; // Ensure ID is not changed

                // Update booking
                await _bookingService.UpdateBookingAsync(existingBooking);

                // Get updated booking
                var updatedBooking = await _bookingService.GetBookingByIdAsync(id);
                var bookingDto = _mapper.Map<BookingResponseDTO>(updatedBooking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Booking updated successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating booking",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Bookings/{id}
        // delete a booking (only pending/cancelled bookings)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Booking not found",
                        new List<string> { $"Booking with ID {id} does not exist" }
                    ));
                }

                await _bookingService.DeleteBookingAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Booking deleted successfully",
                    "Booking deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Booking deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting booking",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Bookings/{id}/assign-driver
        // assign driver and vehicle to a pending booking
        [HttpPatch("{id}/assign-driver")]
        public async Task<IActionResult> AssignDriverToBooking(int id, [FromBody] AssignDriverRequest request)
        {
            try
            {
                await _bookingService.AssignDriverToBookingAsync(id, request.DriverId, request.VehicleId);

                var updatedBooking = await _bookingService.GetBookingByIdAsync(id);
                var bookingDto = _mapper.Map<BookingResponseDTO>(updatedBooking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Driver assigned to booking successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Assignment failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Assignment failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while assigning driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Bookings/{id}/start
        // start the ride (Assigned → InProgress)
        [HttpPatch("{id}/start")]
        public async Task<IActionResult> StartRide(int id)
        {
            try
            {
                await _bookingService.StartRideAsync(id);

                var updatedBooking = await _bookingService.GetBookingByIdAsync(id);
                var bookingDto = _mapper.Map<BookingResponseDTO>(updatedBooking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Ride started successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Start ride failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Start ride failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while starting ride",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Bookings/{id}/complete
        // complete the ride (InProgress → Completed)
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteRide(int id, [FromBody] CompleteBookingRequest request)
        {
            try
            {
                await _bookingService.CompleteRideAsync(id, request.ActualFare);

                var updatedBooking = await _bookingService.GetBookingByIdAsync(id);
                var bookingDto = _mapper.Map<BookingResponseDTO>(updatedBooking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Ride completed successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Complete ride failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Complete ride failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Complete ride failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while completing ride",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Bookings/{id}/cancel
        // cancel a booking
        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                await _bookingService.CancelBookingAsync(id);

                var updatedBooking = await _bookingService.GetBookingByIdAsync(id);
                var bookingDto = _mapper.Map<BookingResponseDTO>(updatedBooking);

                return Ok(ApiResponseDTO<BookingResponseDTO>.SuccessResponse(
                    bookingDto,
                    "Booking cancelled successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Cancel booking failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Cancel booking failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while cancelling booking",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Bookings/calculate-fare
        // calculate estimated fare for a trip
        [HttpPost("calculate-fare")]
        public async Task<IActionResult> CalculateFare([FromQuery] string pickupLocation, [FromQuery] string dropoffLocation, [FromQuery] ServiceType serviceType)
        {
            try
            {
                var estimatedFare = await _bookingService.CalculateFareAsync(pickupLocation, dropoffLocation, serviceType);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    estimatedFare,
                    "Fare calculated successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while calculating fare",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Bookings/estimate-fare
        // estimate fare based on distance
        [HttpPost("estimate-fare")]
        public async Task<IActionResult> EstimateFare([FromQuery] double distanceKm, [FromQuery] ServiceType serviceType)
        {
            try
            {
                var estimatedFare = await _bookingService.EstimateFareAsync(distanceKm, serviceType);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    estimatedFare,
                    "Fare estimated successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while estimating fare",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/statistics/total
        // get total booking count
        [HttpGet("statistics/total")]
        public async Task<IActionResult> GetTotalBookingCount()
        {
            try
            {
                var count = await _bookingService.GetTotalBookingsCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total booking count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total booking count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/statistics/user/{userId}
        // get total bookings by user
        [HttpGet("statistics/user/{userId}")]
        public async Task<IActionResult> GetTotalBookingsByUser(int userId)
        {
            try
            {
                var count = await _bookingService.GetTotalBookingsByUserAsync(userId);

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    $"Total bookings for user {userId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user booking count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Bookings/statistics/revenue
        // get total revenue by date range
        [HttpGet("statistics/revenue")]
        public async Task<IActionResult> GetTotalRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var revenue = await _bookingService.GetTotalRevenueByDateRangeAsync(startDate, endDate);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    revenue,
                    $"Total revenue between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving revenue",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}