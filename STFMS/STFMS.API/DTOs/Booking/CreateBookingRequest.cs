using System.ComponentModel.DataAnnotations;
using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Booking
{
    public class CreateBookingRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Pickup location is required")]
        [StringLength(200, ErrorMessage = "Pickup location cannot exceed 200 characters")]
        public required string PickupLocation { get; set; }

        [Required(ErrorMessage = "Dropoff location is required")]
        [StringLength(200, ErrorMessage = "Dropoff location cannot exceed 200 characters")]
        public required string DropoffLocation { get; set; }

        [Required(ErrorMessage = "Service type is required")]
        public ServiceType ServiceType { get; set; }
    }
}
