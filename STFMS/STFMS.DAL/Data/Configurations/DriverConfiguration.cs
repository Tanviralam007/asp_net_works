using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.DAL.Data.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            // Primary Key
            builder.HasKey(d => d.DriverId);

            // Unique Constraints
            builder.HasIndex(d => d.UserId).IsUnique();
            builder.HasIndex(d => d.LicenseNumber).IsUnique();

            // User-Driver 1:1 relationship with RESTRICT
            builder.HasOne(d => d.User)
                   .WithOne(u => u.Driver)
                   .HasForeignKey<Driver>(d => d.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Driver-Vehicle relationship with RESTRICT
            builder.HasMany(d => d.Vehicles)
                   .WithOne(v => v.Driver)
                   .HasForeignKey(v => v.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Driver-Booking relationship with SET NULL
            builder.HasMany(d => d.Bookings)
                   .WithOne(b => b.Driver)
                   .HasForeignKey(b => b.DriverId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired(false);
        }
    }
}
