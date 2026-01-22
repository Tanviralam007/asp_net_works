using System.ComponentModel.DataAnnotations;

namespace STFMS.API.DTOs.Booking
{
    public class AssignDriverRequest
    {
        [Required(ErrorMessage = "Driver ID is required")]
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Vehicle ID is required")]
        public int VehicleId { get; set; }
    }
}
