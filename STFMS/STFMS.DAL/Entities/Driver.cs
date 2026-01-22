using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum DriverStatus
    {
        Available = 1,
        Busy = 2,
        Offline = 3
    }

    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR(50)")]
        public required string LicenseNumber { get; set; }

        [Column(TypeName = "DECIMAL(3,2)")]
        public decimal Rating { get; set; } = 0.00m;

        public int TotalRides { get; set; } = 0;

        [Column(TypeName = "TINYINT")]
        public DriverStatus Status { get; set; } = DriverStatus.Available;

        [Column(TypeName = "DATETIME2")]
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties (Many-to-One)
        public virtual User User { get; set; } = null!;

        // Navigation Properties (One-to-Many)
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
