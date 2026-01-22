using System.ComponentModel.DataAnnotations;
using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Vehicle
{
    public class UpdateVehicleRequest
    {
        [Required(ErrorMessage = "Registration number is required")]
        [StringLength(20, ErrorMessage = "Registration number cannot exceed 20 characters")]
        public required string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(100, ErrorMessage = "Model cannot exceed 100 characters")]
        public required string Model { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        public VehicleType VehicleType { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public VehicleStatus Status { get; set; }
    }
}
