//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class AlertHistory
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public int SubscriberId { get; set; }
        public int AlertTriggerGroupId { get; set; }
        public System.DateTime Timestamp { get; set; }
    
        public virtual AlertTriggerGroup AlertTriggerGroup { get; set; }
        public virtual Log Log { get; set; }
        public virtual Subscriber Subscriber { get; set; }
    }
}
