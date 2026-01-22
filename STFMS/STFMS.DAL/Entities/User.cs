using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STFMS.DAL.Entities
{
    public enum UserType
    {
        Customer = 1,
        Driver = 2,
        Admin = 3
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public required string FullName { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "NVARCHAR(150)")]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "NVARCHAR(255)")]
        public required string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public required string PhoneNumber { get; set; }

        [Column(TypeName = "TINYINT")]
        public UserType UserType { get; set; } = UserType.Customer;

        [Column(TypeName = "DATETIME2")]
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation Properties (One-to-One)
        public virtual Driver? Driver { get; set; }

        // Navigation Properties (One-to-Many)
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
