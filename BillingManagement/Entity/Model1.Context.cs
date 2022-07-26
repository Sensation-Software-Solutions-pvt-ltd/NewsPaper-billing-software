﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BillMgmtEntities : DbContext
    {
        public BillMgmtEntities()
            : base("name=BillMgmtEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<NewsPaper> NewsPapers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<NewsPaperRate> NewsPaperRates { get; set; }
        public virtual DbSet<NewsPaperAllocation> NewsPaperAllocations { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<ManualBulkSupply> ManualBulkSupplies { get; set; }
        public virtual DbSet<NoSupplyDate> NoSupplyDates { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
    }
}
