using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public enum PaymentMethod
    {
        Cash = 1,
        Bkash = 2,
        Nagad = 3
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BorrowRequest")]
        [Column(TypeName = "INT")]
        public int BorrowRequestId { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        [Range(0.01, 999999.99)]
        public decimal Amount { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "TINYINT")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string TransactionReference { get; set; } = string.Empty;

        // navigation (one to one with BorrowRequestId)
        public virtual BorrowRequest BorrowRequest { get; set; } = null!;
    }
}
