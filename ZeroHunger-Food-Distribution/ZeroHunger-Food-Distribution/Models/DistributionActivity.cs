namespace ZeroHunger_Food_Distribution.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DistributionActivity
    {
        public int DistributionId { get; set; }
        public int CollectRequestId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime DistributedDate { get; set; }
        public string DistributionLocation { get; set; }
        public Nullable<int> NumberOfPeopleServed { get; set; }
        public string DistributionNotes { get; set; }
    
        public virtual CollectRequest CollectRequest { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
