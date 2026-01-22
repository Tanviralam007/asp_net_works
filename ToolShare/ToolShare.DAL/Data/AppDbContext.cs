using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // tables
        public DbSet<User> Users { get; set; }
        public DbSet<ToolCategory> ToolCategories { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ToolTransaction> ToolTransactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ToolImage> ToolImages { get; set; }
    }
}
