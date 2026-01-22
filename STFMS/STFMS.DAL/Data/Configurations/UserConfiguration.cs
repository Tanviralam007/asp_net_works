using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace STFMS.DAL.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(u => u.UserId);

            // Unique Constraints
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();

            // User-Booking relationship with RESTRICT
            builder.HasMany(u => u.Bookings)
                       .WithOne(b => b.User)
                       .HasForeignKey(b => b.UserId)
                       .OnDelete(DeleteBehavior.Restrict);

            // User-Feedback relationship
            builder.HasMany(u => u.Feedbacks)
                       .WithOne(f => f.User)
                       .HasForeignKey(f => f.UserId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
