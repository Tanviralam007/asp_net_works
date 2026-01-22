using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Maintenance
{
    public class MaintenanceResponseDTO
    {
        public int MaintenanceId { get; set; }
        public int VehicleId { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public MaintenanceStatus Status { get; set; }
    }
}
