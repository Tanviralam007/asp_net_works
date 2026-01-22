using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public class ToolImage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tool")]
        [Column(TypeName = "INT")]
        public int ToolId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "NVARCHAR(500)")]
        [Url]  // validates it's a proper URL
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; } = false;

        [Column(TypeName = "DATETIME2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Tool Tool { get; set; } = null!;
    }
}
