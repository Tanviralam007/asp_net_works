using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum VehicleStatus
    {
        Active = 1,
        Maintenance = 2,
        Inactive = 3
    }

    public enum VehicleType
    {
        Sedan = 1,
        SUV = 2,
        Van = 3,
        Luxury = 4
    }

    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public required string RegistrationNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public required string Model { get; set; }

        [Column(TypeName = "TINYINT")]
        public VehicleType VehicleType { get; set; }

        [Range(1, 20)]
        public int Capacity { get; set; }

        [Column(TypeName = "TINYINT")]
        public VehicleStatus Status { get; set; } = VehicleStatus.Active;

        [Column(TypeName = "DATETIME2")]
        public DateTime LastServiceDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties (Many-to-One)
        public virtual Driver Driver { get; set; } = null!;

        // Navigation Properties (One-to-Many)
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Maintenance> MaintenanceRecords { get; set; } = new List<Maintenance>();
    }
}
