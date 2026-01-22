using System.ComponentModel.DataAnnotations;

namespace STFMS.API.DTOs.Booking
{
    public class CompleteBookingRequest
    {
        [Required(ErrorMessage = "Actual fare is required")]
        [Range(0.01, 10000, ErrorMessage = "Actual fare must be between 0.01 and 10000")]
        public decimal ActualFare { get; set; }
    }
}
