using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum BookingStatus
    {
        Pending = 1,
        Assigned = 2,
        InProgress = 3,
        Completed = 4,
        Cancelled = 5
    }

    public enum ServiceType
    {
        Ride = 1,
        Corporate = 2,
        Parcel = 3
    }

    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }

        [ForeignKey("Vehicle")]
        public int? VehicleId { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR(200)")]
        public required string PickupLocation { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR(200)")]
        public required string DropoffLocation { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "DATETIME2")]
        public DateTime? PickupTime { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? CompletionTime { get; set; }

        [Column(TypeName = "TINYINT")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal EstimatedFare { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal? ActualFare { get; set; }

        [Column(TypeName = "TINYINT")]
        public ServiceType ServiceType { get; set; }

        // Navigation Properties (Many-to-One)
        public virtual User User { get; set; } = null!;
        public virtual Driver? Driver { get; set; }
        public virtual Vehicle? Vehicle { get; set; }

        // Navigation Properties (One-to-One)
        public virtual Payment? Payment { get; set; }
        public virtual Feedback? Feedback { get; set; }
    }
}
