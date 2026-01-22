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
    public class MaintenanceConfiguration : IEntityTypeConfiguration<Maintenance>
    {
        public void Configure(EntityTypeBuilder<Maintenance> builder)
        {
            // Primary Key
            builder.HasKey(m => m.MaintenanceId);

            // Decimal precision
            builder.Property(m => m.Cost)
                   .HasPrecision(10, 2);
        }
    }
}
