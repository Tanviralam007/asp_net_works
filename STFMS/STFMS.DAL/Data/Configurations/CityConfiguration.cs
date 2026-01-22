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
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            // Primary Key
            builder.HasKey(c => c.CityId);

            // Unique Constraint
            builder.HasIndex(c => c.CityName).IsUnique();

            // Decimal precision
            builder.Property(c => c.BaseFare)
                   .HasPrecision(10, 2);

            builder.Property(c => c.PerKmRate)
                   .HasPrecision(10, 2);
        }
    }
}
