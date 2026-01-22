using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public required string CityName { get; set; }

        public bool IsActive { get; set; } = true;

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal BaseFare { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal PerKmRate { get; set; }
    }
}
