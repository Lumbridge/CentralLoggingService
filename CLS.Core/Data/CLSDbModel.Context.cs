﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CLS.Core.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CLSDbEntities : DbContext
    {
        public CLSDbEntities()
            : base("name=CLSDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlertHistory> AlertHistories { get; set; }
        public virtual DbSet<AlertTriggerGroup> AlertTriggerGroups { get; set; }
        public virtual DbSet<AlertTriggerNode> AlertTriggerNodes { get; set; }
        public virtual DbSet<AlertTriggerNodeOperator> AlertTriggerNodeOperators { get; set; }
        public virtual DbSet<AlertTriggerNodeType> AlertTriggerNodeTypes { get; set; }
        public virtual DbSet<AlertType> AlertTypes { get; set; }
        public virtual DbSet<DashboardMetadata> DashboardMetadatas { get; set; }
        public virtual DbSet<EnvironmentType> EnvironmentTypes { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<PublishingSystem> PublishingSystems { get; set; }
        public virtual DbSet<PublishingSystemType> PublishingSystemTypes { get; set; }
        public virtual DbSet<Severity> Severities { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
    }
}
