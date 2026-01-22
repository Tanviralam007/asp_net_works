using System.ComponentModel.DataAnnotations;

namespace ToolShare.API.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "Borrow request ID is required")]
        public int BorrowRequestId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 1000000, ErrorMessage = "Amount must be between 1 and 1000000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [Range(0, 3, ErrorMessage = "Payment method must be 0 (Cash), 1 (BKash), 2 (Nagad), or 3 (Card)")]
        public byte PaymentMethod { get; set; }

        [Required(ErrorMessage = "Transaction reference is required")]
        [StringLength(100)]
        public string TransactionReference { get; set; } = string.Empty;
    }
}
