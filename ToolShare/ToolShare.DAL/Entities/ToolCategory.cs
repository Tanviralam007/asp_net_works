using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public class ToolCategory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Column(TypeName = "NVARCHAR(50)")]
        public required string CategoryName { get; set; }

        [StringLength(500)]
        [Column(TypeName = "NVARCHAR(500)")]
        public string? Description { get; set; }

        // navigation (one-to-many with tool)
        public virtual ICollection<Tool> Tools { get; set; } = new List<Tool>();
    }
}
