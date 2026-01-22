using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Vehicle;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        // GET: api/Vehicles
        // get all vehicles
        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    "Vehicles retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicles",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/{id}
        // get vehicle by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

                if (vehicle == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Vehicle not found",
                        new List<string> { $"Vehicle with ID {id} does not exist" }
                    ));
                }

                var vehicleDto = _mapper.Map<VehicleResponseDTO>(vehicle);

                return Ok(ApiResponseDTO<VehicleResponseDTO>.SuccessResponse(
                    vehicleDto,
                    "Vehicle retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/registration/{registrationNumber}
        // get vehicle by registration number
        [HttpGet("registration/{registrationNumber}")]
        public async Task<IActionResult> GetVehicleByRegistrationNumber(string registrationNumber)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByRegistrationNumberAsync(registrationNumber);

                if (vehicle == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Vehicle not found",
                        new List<string> { $"Vehicle with registration number {registrationNumber} does not exist" }
                    ));
                }

                var vehicleDto = _mapper.Map<VehicleResponseDTO>(vehicle);

                return Ok(ApiResponseDTO<VehicleResponseDTO>.SuccessResponse(
                    vehicleDto,
                    "Vehicle retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/driver/{driverId}
        // get all vehicles for a specific driver
        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetVehiclesByDriverId(int driverId)
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByDriverIdAsync(driverId);
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    $"Vehicles for driver {driverId} retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicles",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/{id}/with-maintenance
        // get vehicle with maintenance history
        [HttpGet("{id}/with-maintenance")]
        public async Task<IActionResult> GetVehicleWithMaintenanceHistory(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleWithMaintenanceHistoryAsync(id);

                if (vehicle == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Vehicle not found",
                        new List<string> { $"Vehicle with ID {id} does not exist" }
                    ));
                }

                var vehicleDto = _mapper.Map<VehicleWithDetailsResponseDTO>(vehicle);

                return Ok(ApiResponseDTO<VehicleWithDetailsResponseDTO>.SuccessResponse(
                    vehicleDto,
                    "Vehicle with maintenance history retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicle with maintenance history",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/status/{status}
        // Get vehicles by status (Active, Maintenance, Inactive)
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetVehiclesByStatus(VehicleStatus status)
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByStatusAsync(status);
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    $"{status} vehicles retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicles by status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/type/{vehicleType}
        // Get vehicles by type (Sedan, SUV, Van, Bike)
        [HttpGet("type/{vehicleType}")]
        public async Task<IActionResult> GetVehiclesByType(VehicleType vehicleType)
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByTypeAsync(vehicleType);
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    $"{vehicleType} vehicles retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicles by type",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/active
        // Get all active vehicles
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetActiveVehiclesAsync();
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    "Active vehicles retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active vehicles",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/maintenance/due/{daysThreshold}
        // Get vehicles due for maintenance
        [HttpGet("maintenance/due/{daysThreshold}")]
        public async Task<IActionResult> GetVehiclesDueForMaintenance(int daysThreshold = 30)
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiclesDueForMaintenanceAsync(daysThreshold);
                var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

                return Ok(ApiResponseDTO<IEnumerable<VehicleResponseDTO>>.SuccessResponse(
                    vehiclesDto,
                    $"Vehicles due for maintenance (last service > {daysThreshold} days) retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving vehicles due for maintenance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Vehicles
        // Create a new vehicle
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            try
            {
                // Map request to entity
                var vehicle = _mapper.Map<Vehicle>(request);

                // Create vehicle
                var createdVehicle = await _vehicleService.CreateVehicleAsync(vehicle);

                // Map to response DTO
                var vehicleDto = _mapper.Map<VehicleResponseDTO>(createdVehicle);

                return CreatedAtAction(
                    nameof(GetVehicleById),
                    new { id = createdVehicle.VehicleId },
                    ApiResponseDTO<VehicleResponseDTO>.SuccessResponse(
                        vehicleDto,
                        "Vehicle created successfully"
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating vehicle",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Vehicles/{id}
        // Update an existing vehicle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleRequest request)
        {
            try
            {
                // Get existing vehicle
                var existingVehicle = await _vehicleService.GetVehicleByIdAsync(id);

                if (existingVehicle == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Vehicle not found",
                        new List<string> { $"Vehicle with ID {id} does not exist" }
                    ));
                }

                // Map request to entity
                _mapper.Map(request, existingVehicle);
                existingVehicle.VehicleId = id; // Ensure ID is not changed

                // Update vehicle
                await _vehicleService.UpdateVehicleAsync(existingVehicle);

                // Get updated vehicle
                var updatedVehicle = await _vehicleService.GetVehicleByIdAsync(id);
                var vehicleDto = _mapper.Map<VehicleResponseDTO>(updatedVehicle);

                return Ok(ApiResponseDTO<VehicleResponseDTO>.SuccessResponse(
                    vehicleDto,
                    "Vehicle updated successfully"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle update failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating vehicle",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Vehicles/{id}
        // Delete a vehicle
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

                if (vehicle == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Vehicle not found",
                        new List<string> { $"Vehicle with ID {id} does not exist" }
                    ));
                }

                await _vehicleService.DeleteVehicleAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Vehicle deleted successfully",
                    "Vehicle deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting vehicle",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Vehicles/{id}/status/active
        // Set vehicle status to Active
        [HttpPatch("{id}/status/active")]
        public async Task<IActionResult> SetVehicleActive(int id)
        {
            try
            {
                await _vehicleService.SetVehicleActiveAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Vehicle status updated to Active",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating vehicle status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Vehicles/{id}/status/maintenance
        // Set vehicle status to Maintenance
        [HttpPatch("{id}/status/maintenance")]
        public async Task<IActionResult> SetVehicleInMaintenance(int id)
        {
            try
            {
                await _vehicleService.SetVehicleInMaintenanceAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Vehicle status updated to Maintenance",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating vehicle status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Vehicles/{id}/status/inactive
        // Set vehicle status to Inactive
        [HttpPatch("{id}/status/inactive")]
        public async Task<IActionResult> SetVehicleInactive(int id)
        {
            try
            {
                await _vehicleService.SetVehicleInactiveAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Vehicle status updated to Inactive",
                    "Status update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating vehicle status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Vehicles/{id}/service-date
        // Update vehicle last service date
        [HttpPatch("{id}/service-date")]
        public async Task<IActionResult> UpdateLastServiceDate(int id, [FromBody] DateTime serviceDate)
        {
            try
            {
                await _vehicleService.UpdateLastServiceDateAsync(id, serviceDate);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Vehicle last service date updated successfully",
                    "Service date update successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Vehicle not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating service date",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/statistics/total
        // Get total vehicle count
        [HttpGet("statistics/total")]
        public async Task<IActionResult> GetTotalVehicleCount()
        {
            try
            {
                var count = await _vehicleService.GetTotalVehiclesCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total vehicle count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total vehicle count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Vehicles/statistics/active
        // Get active vehicle count
        [HttpGet("statistics/active")]
        public async Task<IActionResult> GetActiveVehicleCount()
        {
            try
            {
                var count = await _vehicleService.GetActiveVehiclesCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Active vehicle count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active vehicle count",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}