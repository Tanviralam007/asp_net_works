namespace ZeroHunger_Food_Distribution.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CollectionActivity
    {
        public int CollectionId { get; set; }
        public int CollectRequestId { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime CollectedDate { get; set; }
        public string ActualQuantity { get; set; }
        public string CollectionNotes { get; set; }
    
        public virtual CollectRequest CollectRequest { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
