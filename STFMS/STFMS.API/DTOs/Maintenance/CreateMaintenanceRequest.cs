using System.ComponentModel.DataAnnotations;
using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Maintenance
{
    public class CreateMaintenanceRequest
    {
        [Required(ErrorMessage = "Vehicle ID is required")]
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Maintenance type is required")]
        public MaintenanceType MaintenanceType { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        [Range(0, 100000, ErrorMessage = "Cost must be between 0 and 100000")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }
    }
}
