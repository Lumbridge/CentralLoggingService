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
    
    public partial class Log
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Log()
        {
            this.AlertHistories = new HashSet<AlertHistory>();
        }
    
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SeverityId { get; set; }
        public int PublishingSystemId { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertHistory> AlertHistories { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Severity Severity { get; set; }
        public virtual PublishingSystem PublishingSystem { get; set; }
    }
}
