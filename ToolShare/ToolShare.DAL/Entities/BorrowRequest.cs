using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public enum RequestStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4,
        Completed = 5
    }

    public class BorrowRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tool")]
        [Column(TypeName = "INT")]
        public int ToolId { get; set; }

        [ForeignKey(nameof(Borrower))]
        [Column(TypeName = "INT")]
        public int? BorrowerId { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "TINYINT")]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [Column(TypeName = "DATETIME2")]
        public DateTime? ApprovalDate { get; set; }

        // navigation
        public virtual Tool Tool { get; set; } = null!;
        public virtual User? Borrower { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual ToolTransaction? Transaction { get; set; }
    }
}
