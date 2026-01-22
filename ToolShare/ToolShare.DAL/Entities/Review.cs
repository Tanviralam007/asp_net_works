using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public enum ReviewType
    {
        ToolReview = 1,
        UserReview = 2
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tool")]
        [Column(TypeName = "INT")]
        public int ToolId { get; set; }

        [ForeignKey(nameof(User))]
        [Column(TypeName = "INT")]
        public int? UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        [Column(TypeName = "NVARCHAR(500)")]
        public string? Comment { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "TINYINT")]
        public ReviewType ReviewType { get; set; }

        public virtual Tool Tool { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
    }
}