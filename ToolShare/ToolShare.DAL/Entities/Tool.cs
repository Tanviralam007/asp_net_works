using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public class Tool
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "INT")]
        public int OwnerId { get; set; }

        [ForeignKey("Category")]   
        [Column(TypeName = "INT")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "NVARCHAR(150)")]
        public string ToolName { get; set; } = string.Empty;

        [StringLength(1000)]
        [Column(TypeName = "NVARCHAR(1000)")]
        public string? Description { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        [Range(0.01, 9999.99)]
        public decimal DailyRate { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR(200)")]
        public string Location { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        [Column(TypeName = "DATETIME2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // navigation 
        public virtual User? Owner { get; set; }
        public virtual ToolCategory Category { get; set; } = null!;
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
        public virtual ICollection<ToolImage> Images { get; set; } = new List<ToolImage>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
