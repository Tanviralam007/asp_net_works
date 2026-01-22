using System.ComponentModel.DataAnnotations;

namespace STFMS.API.DTOs.Driver
{
    public class UpdateDriverRequest
    {
        [Required(ErrorMessage = "License number is required")]
        [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters")]
        public required string LicenseNumber { get; set; }
    }
}
