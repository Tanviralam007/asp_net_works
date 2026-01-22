using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        [Column(TypeName = "NVARCHAR(500)")]
        public string? Comments { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Booking Booking { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
