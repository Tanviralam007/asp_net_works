using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Payment
{
    public class PaymentResponseDTO
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
