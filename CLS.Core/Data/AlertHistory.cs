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
        public int AlertHistoryGroupId { get; set; }
        public int LogId { get; set; }
        public string UserId { get; set; }
        public int AlertTriggerGroupId { get; set; }
        public System.DateTime Timestamp { get; set; }
        public int SiblingCount { get; set; }
    
        public virtual AlertTriggerGroup AlertTriggerGroup { get; set; }
        public virtual Log Log { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
