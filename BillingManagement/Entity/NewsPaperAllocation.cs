//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BillingManagement.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class NewsPaperAllocation
    {
        public int Id { get; set; }
        public Nullable<int> NewspaperId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> NumberOfCopies { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual NewsPaper NewsPaper { get; set; }
        public virtual User User { get; set; }
    }
}
