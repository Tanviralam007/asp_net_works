using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Driver;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;

        public DriversController(IDriverService driverService, IMapper mapper)
        {
            _driverService = driverService;
            _mapper = mapper;
        }

        // GET: api/Drivers
        // get all drivers
        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var drivers = await _driverService.GetAllDriversAsync();
                var driversDto = _mapper.Map<IEnumerable<DriverResponseDTO>>(drivers);

                return Ok(ApiResponseDTO<IEnumerable<DriverResponseDTO>>.SuccessResponse(
                    driversDto,
                    "Drivers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/user/{userId}
        // get driver by user id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetDriverByUserId(int userId)
        {
            try
            {
                var driver = await _driverService.GetDriverByUserIdAsync(userId);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver for user ID {userId} does not exist" }
                    ));
                }

                var driverDto = _mapper.Map<DriverResponseDTO>(driver);

                return Ok(ApiResponseDTO<DriverResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/license/{licenseNumber}
        // get driver by license number
        [HttpGet("license/{licenseNumber}")]
        public async Task<IActionResult> GetDriverByLicenseNumber(string licenseNumber)
        {
            try
            {
                var driver = await _driverService.GetDriverByLicenseNumberAsync(licenseNumber);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with license number {licenseNumber} does not exist" }
                    ));
                }

                var driverDto = _mapper.Map<DriverResponseDTO>(driver);

                return Ok(ApiResponseDTO<DriverResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/available
        // get all available drivers
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableDrivers()
        {
            try
            {
                var drivers = await _driverService.GetAvailableDriversAsync();
                var driversDto = _mapper.Map<IEnumerable<DriverResponseDTO>>(drivers);

                return Ok(ApiResponseDTO<IEnumerable<DriverResponseDTO>>.SuccessResponse(
                    driversDto,
                    "Available drivers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving available drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/status/{status}
        // get drivers by status (Available, Busy, Offline)
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetDriversByStatus(DriverStatus status)
        {
            try
            {
                var drivers = await _driverService.GetDriversByStatusAsync(status);
                var driversDto = _mapper.Map<IEnumerable<DriverResponseDTO>>(drivers);

                return Ok(ApiResponseDTO<IEnumerable<DriverResponseDTO>>.SuccessResponse(
                    driversDto,
                    $"{status} drivers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving drivers by status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/top-rated/{count}
        // Get top-rated drivers
        [HttpGet("top-rated/{count}")]
        public async Task<IActionResult> GetTopRatedDrivers(int count = 10)
        {
            try
            {
                var drivers = await _driverService.GetTopRatedDriversAsync(count);
                var driversDto = _mapper.Map<IEnumerable<DriverResponseDTO>>(drivers);

                return Ok(ApiResponseDTO<IEnumerable<DriverResponseDTO>>.SuccessResponse(
                    driversDto,
                    $"Top {count} rated drivers retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving top-rated drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/low-rated/{threshold}
        // get drivers with low ratings
        [HttpGet("low-rated/{threshold}")]
        public async Task<IActionResult> GetDriversWithLowRating(decimal threshold = 3.0m)
        {
            try
            {
                var drivers = await _driverService.GetDriversWithLowRatingAsync(threshold);
                var driversDto = _mapper.Map<IEnumerable<DriverResponseDTO>>(drivers);

                return Ok(ApiResponseDTO<IEnumerable<DriverResponseDTO>>.SuccessResponse(
                    driversDto,
                    $"Drivers with rating below {threshold} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving low-rated drivers",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/statistics/total
        // Get total driver count
        [HttpGet("statistics/total")]
        public async Task<IActionResult> GetTotalDriverCount()
        {
            try
            {
                var count = await _driverService.GetTotalDriversCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total driver count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total driver count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/statistics/available
        // Get available driver count
        [HttpGet("statistics/available")]
        public async Task<IActionResult> GetAvailableDriverCount()
        {
            try
            {
                var count = await _driverService.GetAvailableDriversCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Available driver count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving available driver count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/{id}
        // get driver by ID - MOVED TO AFTER SPECIFIC ROUTES
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            try
            {
                var driver = await _driverService.GetDriverByIdAsync(id);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with ID {id} does not exist" }
                    ));
                }

                var driverDto = _mapper.Map<DriverResponseDTO>(driver);

                return Ok(ApiResponseDTO<DriverResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/{id}/with-vehicles
        // get driver with their vehicles
        [HttpGet("{id}/with-vehicles")]
        public async Task<IActionResult> GetDriverWithVehicles(int id)
        {
            try
            {
                var driver = await _driverService.GetDriverWithVehiclesAsync(id);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with ID {id} does not exist" }
                    ));
                }

                var driverDto = _mapper.Map<DriverWithDetailsResponseDTO>(driver);

                return Ok(ApiResponseDTO<DriverWithDetailsResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver with vehicles retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver with vehicles",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Drivers/{id}/with-bookings
        // Get driver with their booking history
        [HttpGet("{id}/with-bookings")]
        public async Task<IActionResult> GetDriverWithBookings(int id)
        {
            try
            {
                var driver = await _driverService.GetDriverWithBookingsAsync(id);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with ID {id} does not exist" }
                    ));
                }

                var driverDto = _mapper.Map<DriverWithDetailsResponseDTO>(driver);

                return Ok(ApiResponseDTO<DriverWithDetailsResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver with bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving driver with bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Drivers
        // create a new driver
        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverRequest request)
        {
            try
            {
                // Map request to entity
                var driver = _mapper.Map<Driver>(request);

                // Create driver
                var createdDriver = await _driverService.CreateDriverAsync(driver);

                // Map to response DTO
                var driverDto = _mapper.Map<DriverResponseDTO>(createdDriver);

                return CreatedAtAction(
                    nameof(GetDriverById),
                    new { id = createdDriver.DriverId },
                    ApiResponseDTO<DriverResponseDTO>.SuccessResponse(
                        driverDto,
                        "Driver created successfully"
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Driver creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Drivers/{id}
        // update an existing driver
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] UpdateDriverRequest request)
        {
            try
            {
                // Get existing driver
                var existingDriver = await _driverService.GetDriverByIdAsync(id);

                if (existingDriver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with ID {id} does not exist" }
                    ));
                }

                // Map request to entity
                _mapper.Map(request, existingDriver);
                existingDriver.DriverId = id; // Ensure ID is not changed

                // Update driver
                await _driverService.UpdateDriverAsync(existingDriver);

                // Get updated driver
                var updatedDriver = await _driverService.GetDriverByIdAsync(id);
                var driverDto = _mapper.Map<DriverResponseDTO>(updatedDriver);

                return Ok(ApiResponseDTO<DriverResponseDTO>.SuccessResponse(
                    driverDto,
                    "Driver updated successfully"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Driver update failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Drivers/{id}
        // delete a driver
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                var driver = await _driverService.GetDriverByIdAsync(id);

                if (driver == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Driver not found",
                        new List<string> { $"Driver with ID {id} does not exist" }
                    ));
                }

                await _driverService.DeleteDriverAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver deleted successfully",
                    "Driver deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Driver deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting driver",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Drivers/{id}/status/available
        // set driver status to Available
        [HttpPatch("{id}/status/available")]
        public async Task<IActionResult> SetDriverAvailable(int id)
        {
            try
            {
                await _driverService.SetDriverAvailableAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver status updated to Available",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating driver status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Drivers/{id}/status/busy
        // Set driver status to Busy
        [HttpPatch("{id}/status/busy")]
        public async Task<IActionResult> SetDriverBusy(int id)
        {
            try
            {
                await _driverService.SetDriverBusyAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver status updated to Busy",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating driver status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Drivers/{id}/status/offline
        // set driver status to Offline
        [HttpPatch("{id}/status/offline")]
        public async Task<IActionResult> SetDriverOffline(int id)
        {
            try
            {
                await _driverService.SetDriverOfflineAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver status updated to Offline",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating driver status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Drivers/{id}/rating/update
        // recalculate and update driver rating from feedbacks
        [HttpPatch("{id}/rating/update")]
        public async Task<IActionResult> UpdateDriverRating(int id)
        {
            try
            {
                await _driverService.UpdateDriverRatingAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver rating updated successfully",
                    "Rating update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating driver rating",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Drivers/{id}/rides/increment
        // Increment driver's total rides count
        [HttpPatch("{id}/rides/increment")]
        public async Task<IActionResult> IncrementDriverRides(int id)
        {
            try
            {
                await _driverService.IncrementDriverRidesAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Driver total rides incremented successfully",
                    "Rides count update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Driver not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while incrementing driver rides",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}
