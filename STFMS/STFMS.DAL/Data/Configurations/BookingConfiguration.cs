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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            // Primary Key
            builder.HasKey(b => b.BookingId);

            // Booking-Payment 1:1 relationship with CASCADE
            builder.HasOne(b => b.Payment)
                   .WithOne(p => p.Booking)
                   .HasForeignKey<Payment>(p => p.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Booking-Feedback 1:1 relationship with CASCADE
            builder.HasOne(b => b.Feedback)
                   .WithOne(f => f.Booking)
                   .HasForeignKey<Feedback>(f => f.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            builder.Property(b => b.EstimatedFare)
                   .HasPrecision(10, 2);

            builder.Property(b => b.ActualFare)
                   .HasPrecision(10, 2);
        }
    }
}
