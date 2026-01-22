using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.Maintenance;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IMapper _mapper;

        public MaintenanceController(IMaintenanceService maintenanceService, IMapper mapper)
        {
            _maintenanceService = maintenanceService;
            _mapper = mapper;
        }

        // GET: api/Maintenance
        // get all maintenance records
        [HttpGet]
        public async Task<IActionResult> GetAllMaintenance()
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetAllMaintenanceAsync();
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    "Maintenance records retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance records",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/{id}
        // get maintenance by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaintenanceById(int id)
        {
            try
            {
                var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

                if (maintenance == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Maintenance not found",
                        new List<string> { $"Maintenance record with ID {id} does not exist" }
                    ));
                }

                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(maintenance);

                return Ok(ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                    maintenanceDto,
                    "Maintenance record retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance record",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/vehicle/{vehicleId}
        // get all maintenance records for a specific vehicle
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetMaintenanceByVehicleId(int vehicleId)
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetMaintenanceByVehicleIdAsync(vehicleId);
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    $"Maintenance records for vehicle {vehicleId} retrieved successfully"
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
                    "An error occurred while retrieving maintenance records",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/status/{status}
        // get maintenance records by status
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetMaintenanceByStatus(MaintenanceStatus status)
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetMaintenanceByStatusAsync(status);
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    $"{status} maintenance records retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance records by status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/type/{type}
        // get maintenance records by type
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetMaintenanceByType(MaintenanceType type)
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetMaintenanceByTypeAsync(type);
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    $"{type} maintenance records retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance records by type",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/upcoming/{daysAhead}
        // get upcoming maintenance within specified days
        [HttpGet("upcoming/{daysAhead}")]
        public async Task<IActionResult> GetUpcomingMaintenance(int daysAhead = 7)
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetUpcomingMaintenanceAsync(daysAhead);
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    $"Upcoming maintenance within {daysAhead} days retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid days ahead value",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving upcoming maintenance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/overdue
        // get overdue maintenance records
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueMaintenance()
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetOverdueMaintenanceAsync();
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    "Overdue maintenance records retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving overdue maintenance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/date-range
        // get maintenance records by date range
        [HttpGet("date-range")]
        public async Task<IActionResult> GetMaintenanceByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var maintenanceRecords = await _maintenanceService.GetMaintenanceByDateRangeAsync(startDate, endDate);
                var maintenanceDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(maintenanceRecords);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    maintenanceDto,
                    $"Maintenance records between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid date range",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance records by date range",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/alerts
        // get maintenance alerts (overdue + upcoming)
        [HttpGet("alerts")]
        public async Task<IActionResult> GetMaintenanceAlerts()
        {
            try
            {
                var alerts = await _maintenanceService.GetMaintenanceAlertsAsync();
                var alertsDto = _mapper.Map<IEnumerable<MaintenanceResponseDTO>>(alerts);

                return Ok(ApiResponseDTO<IEnumerable<MaintenanceResponseDTO>>.SuccessResponse(
                    alertsDto,
                    "Maintenance alerts retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance alerts",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Maintenance
        // create a new maintenance record
        [HttpPost]
        public async Task<IActionResult> CreateMaintenance([FromBody] CreateMaintenanceRequest request)
        {
            try
            {
                // Map request to entity
                var maintenance = _mapper.Map<Maintenance>(request);

                // Create maintenance
                var createdMaintenance = await _maintenanceService.CreateMaintenanceAsync(maintenance);

                // Map to response DTO
                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(createdMaintenance);

                return CreatedAtAction(
                    nameof(GetMaintenanceById),
                    new { id = createdMaintenance.MaintenanceId },
                    ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                        maintenanceDto,
                        "Maintenance record created successfully"
                    )
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating maintenance record",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Maintenance/schedule
        // schedule a new maintenance (alternative endpoint)
        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleMaintenance([FromBody] CreateMaintenanceRequest request)
        {
            try
            {
                var maintenance = await _maintenanceService.ScheduleMaintenanceAsync(
                    request.VehicleId,
                    request.MaintenanceType,
                    request.Description,
                    request.Cost,
                    request.ScheduledDate
                );

                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(maintenance);

                return Ok(ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                    maintenanceDto,
                    "Maintenance scheduled successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance scheduling failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance scheduling failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while scheduling maintenance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Maintenance/{id}
        // update an existing maintenance record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaintenance(int id, [FromBody] UpdateMaintenanceRequest request)
        {
            try
            {
                // Get existing maintenance
                var existingMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

                if (existingMaintenance == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Maintenance not found",
                        new List<string> { $"Maintenance record with ID {id} does not exist" }
                    ));
                }

                // Map request to entity
                _mapper.Map(request, existingMaintenance);
                existingMaintenance.MaintenanceId = id; // Ensure ID is not changed

                // Update maintenance
                await _maintenanceService.UpdateMaintenanceAsync(existingMaintenance);

                // Get updated maintenance
                var updatedMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(updatedMaintenance);

                return Ok(ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                    maintenanceDto,
                    "Maintenance record updated successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance update failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating maintenance record",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Maintenance/{id}
        // delete a maintenance record
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            try
            {
                var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

                if (maintenance == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Maintenance not found",
                        new List<string> { $"Maintenance record with ID {id} does not exist" }
                    ));
                }

                await _maintenanceService.DeleteMaintenanceAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Maintenance record deleted successfully",
                    "Maintenance deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting maintenance record",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Maintenance/{id}/status/{status}
        // update maintenance status
        [HttpPatch("{id}/status/{status}")]
        public async Task<IActionResult> UpdateMaintenanceStatus(int id, MaintenanceStatus status)
        {
            try
            {
                await _maintenanceService.UpdateMaintenanceStatusAsync(id, status);

                var updatedMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(updatedMaintenance);

                return Ok(ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                    maintenanceDto,
                    $"Maintenance status updated to {status} successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Status update failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating maintenance status",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Maintenance/{id}/complete
        // mark maintenance as completed
        [HttpPatch("{id}/complete")]
        public async Task<IActionResult> CompleteMaintenance(int id)
        {
            try
            {
                await _maintenanceService.CompleteMaintenanceAsync(id);

                var completedMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
                var maintenanceDto = _mapper.Map<MaintenanceResponseDTO>(completedMaintenance);

                return Ok(ApiResponseDTO<MaintenanceResponseDTO>.SuccessResponse(
                    maintenanceDto,
                    "Maintenance completed successfully"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "Maintenance not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Completion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while completing maintenance",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Maintenance/send-reminders
        // send maintenance reminders (manual trigger)
        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendMaintenanceReminders()
        {
            try
            {
                await _maintenanceService.SendMaintenanceRemindersAsync();

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Maintenance reminders sent successfully",
                    "Reminders sent"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while sending maintenance reminders",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/cost/vehicle/{vehicleId}
        // get total maintenance cost for a vehicle
        [HttpGet("cost/vehicle/{vehicleId}")]
        public async Task<IActionResult> GetTotalMaintenanceCostByVehicle(int vehicleId)
        {
            try
            {
                var totalCost = await _maintenanceService.GetTotalMaintenanceCostByVehicleAsync(vehicleId);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    totalCost,
                    $"Total maintenance cost for vehicle {vehicleId} retrieved successfully"
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
                    "An error occurred while retrieving maintenance cost",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/cost/total
        // get total maintenance cost across all vehicles
        [HttpGet("cost/total")]
        public async Task<IActionResult> GetTotalMaintenanceCost()
        {
            try
            {
                var totalCost = await _maintenanceService.GetTotalMaintenanceCostAsync();

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    totalCost,
                    "Total maintenance cost retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total maintenance cost",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Maintenance/cost/date-range
        // get maintenance cost by date range
        [HttpGet("cost/date-range")]
        public async Task<IActionResult> GetMaintenanceCostByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var cost = await _maintenanceService.GetMaintenanceCostByDateRangeAsync(startDate, endDate);

                return Ok(ApiResponseDTO<decimal>.SuccessResponse(
                    cost,
                    $"Maintenance cost between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd} retrieved successfully"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "Invalid date range",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving maintenance cost by date range",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}
