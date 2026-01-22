using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public enum TransactionStatus
    {
        Active = 1,
        Returned = 2,
        Overdue = 3
    }

    [Table("ToolTransactions")]
    public class ToolTransaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BorrowRequest")]
        [Column(TypeName = "INT")]
        public int BorrowRequestId { get; set; }

        [ForeignKey("Payment")]
        [Column(TypeName = "INT")]
        public int? PaymentId { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? HandoverDate { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? ReturnDate { get; set; }

        [Required]                          
        [Column(TypeName = "DATETIME2")]
        public DateTime ExpectedReturnDate { get; set; }

        public int LateDays { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        [Range(0, 9999.99)]                 
        public decimal FineAmount { get; set; }

        [Column(TypeName = "TINYINT")]
        public TransactionStatus Status { get; set; } = TransactionStatus.Active;

        public virtual BorrowRequest BorrowRequest { get; set; } = null!;
        public virtual Payment? Payment { get; set; }
    }
}
