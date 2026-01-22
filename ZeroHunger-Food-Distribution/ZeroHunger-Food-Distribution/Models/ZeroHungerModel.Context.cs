namespace ZeroHunger_Food_Distribution.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ZeroHungerDBEntities : DbContext
    {
        public ZeroHungerDBEntities()
            : base("name=ZeroHungerDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CollectionActivity> CollectionActivities { get; set; }
        public virtual DbSet<CollectRequest> CollectRequests { get; set; }
        public virtual DbSet<DistributionActivity> DistributionActivities { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
