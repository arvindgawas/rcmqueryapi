﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTWebApi.Repository
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TMSEntitiesnew : DbContext
    {
        Type providerService = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        public TMSEntitiesnew()
            : base("name=TMSEntitiesnew")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<ErrorType> ErrorTypes { get; set; }
        public virtual DbSet<KeyGen> KeyGens { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketDetail> TicketDetails { get; set; }
        public virtual DbSet<BankMaster> BankMasters { get; set; }
        public virtual DbSet<QueryTypeMaster> QueryTypeMasters { get; set; }
        public virtual DbSet<userbankmap> userbankmaps { get; set; }
        public virtual DbSet<usermaster> usermasters { get; set; }
        public virtual DbSet<LocationEmailHistory> LocationEmailHistories { get; set; }
    }
}
