using STFMS.API.DTOs.Driver;
using STFMS.API.DTOs.Maintenance;

namespace STFMS.API.DTOs.Vehicle
{
    public class VehicleWithDetailsResponseDTO : VehicleResponseDTO
    {
        public DriverWithDetailsResponseDTO? Driver { get; set; }
        public List<MaintenanceResponseDTO>? MaintenanceRecords { get; set; }
    }
}
