using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum MaintenanceStatus
    {
        Scheduled = 1,
        InProgress = 2,
        Completed = 3
    }

    public enum MaintenanceType
    {
        Service = 1,
        Repair = 2,
        Inspection = 3
    }

    public class Maintenance
    {
        [Key]
        public int MaintenanceId { get; set; }

        [Required]
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }

        [Column(TypeName = "TINYINT")]
        public MaintenanceType MaintenanceType { get; set; }

        [Required]
        [StringLength(300)]
        [Column(TypeName = "NVARCHAR(300)")]
        public required string Description { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Cost { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime ScheduledDate { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? CompletedDate { get; set; }

        [Column(TypeName = "TINYINT")]
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;

        // Navigation Properties (Many-to-One)
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
