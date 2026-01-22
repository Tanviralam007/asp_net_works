using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4
    }

    public enum PaymentMethod
    {
        Card = 1,
        Cash = 2,
        Wallet = 3
    }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "TINYINT")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column(TypeName = "TINYINT")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Column(TypeName = "DATETIME2")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string? TransactionId { get; set; }

        // Navigation Properties (One-to-One)
        public virtual Booking Booking { get; set; } = null!;
    }
}
