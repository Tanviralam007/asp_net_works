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
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            // Primary Key
            builder.HasKey(v => v.VehicleId);

            // Unique Constraint
            builder.HasIndex(v => v.RegistrationNumber).IsUnique();

            // Vehicle-Maintenance relationship with CASCADE
            builder.HasMany(v => v.MaintenanceRecords)
                   .WithOne(m => m.Vehicle)
                   .HasForeignKey(m => m.VehicleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Vehicle-Booking relationship with SET NULL
            builder.HasMany(v => v.Bookings)
                   .WithOne(b => b.Vehicle)
                   .HasForeignKey(b => b.VehicleId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired(false);
        }
    }
}
