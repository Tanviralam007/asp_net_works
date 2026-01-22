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
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            // Primary Key
            builder.HasKey(f => f.FeedbackId);

            // Unique Constraint (one feedback per booking)
            builder.HasIndex(f => f.BookingId).IsUnique();
        }
    }
}
