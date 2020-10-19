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
    
    public partial class PublishingSystem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PublishingSystem()
        {
            this.Logs = new HashSet<Log>();
            this.Subscriptions = new HashSet<Subscription>();
        }
    
        public int Id { get; set; }
        public int EnvironmentTypeId { get; set; }
        public int PublishingSystemTypeId { get; set; }
        public string Name { get; set; }
    
        public virtual EnvironmentType EnvironmentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log> Logs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual PublishingSystemType PublishingSystemType { get; set; }
    }
}
