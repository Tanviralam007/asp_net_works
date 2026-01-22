using System.ComponentModel.DataAnnotations;
using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Payment
{
    public class ProcessPaymentRequest
    {
        [Required(ErrorMessage = "Booking ID is required")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 10000, ErrorMessage = "Amount must be between 0.01 and 10000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
