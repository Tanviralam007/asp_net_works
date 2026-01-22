using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolShare.DAL.Entities
{
    public enum UserRole
    {
        Borrower = 1,
        ToolOwner = 2,
        Admin = 3
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public required string Name { get; set; }

        [StringLength(150)]
        [Column(TypeName = "NVARCHAR(150)")]
        public required string Email { get; set; }

        [StringLength(255)]
        [Column(TypeName = "NVARCHAR(255)")]
        public required string PasswordHash { get; set; }

        [Column(TypeName = "TINYINT")]
        public UserRole Role { get; set; } = UserRole.Borrower;

        [StringLength(200)]
        [Column(TypeName = "NVARCHAR(200)")]
        public required string Location { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR(20)")]
        public required string PhoneNumber { get; set; }

        public bool IsBlocked { get; set; } = false;

        [Column(TypeName = "DATETIME2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // navigation (One-to-Many)
        [InverseProperty("Owner")]
        public virtual ICollection<Tool> OwnedTools { get; set; } = new List<Tool>();

        [InverseProperty("Borrower")]
        public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
