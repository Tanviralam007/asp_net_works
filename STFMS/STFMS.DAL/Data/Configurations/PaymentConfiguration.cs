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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Primary Key
            builder.HasKey(p => p.PaymentId);

            // Unique Constraint
            builder.HasIndex(p => p.BookingId).IsUnique();
            builder.HasIndex(p => p.TransactionId).IsUnique();

            // Decimal precision
            builder.Property(p => p.Amount)
                   .HasPrecision(10, 2);
        }
    }
}
