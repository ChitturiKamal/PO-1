//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PurchaseOrder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TELineOfBusiness
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TELineOfBusiness()
        {
            this.TEClearanceMasters = new HashSet<TEClearanceMaster>();
            this.TEClearanceMasters1 = new HashSet<TEClearanceMaster>();
            this.TEClearanceMasters2 = new HashSet<TEClearanceMaster>();
            this.TEDepartments = new HashSet<TEDepartment>();
            this.TEDepartments1 = new HashSet<TEDepartment>();
            this.TEDepartments2 = new HashSet<TEDepartment>();
            this.TEQueueDepartments = new HashSet<TEQueueDepartment>();
            this.TEQueueDepartments1 = new HashSet<TEQueueDepartment>();
            this.TEQueueDepartments2 = new HashSet<TEQueueDepartment>();
            this.TESubFunctions = new HashSet<TESubFunction>();
            this.TESubFunctions1 = new HashSet<TESubFunction>();
            this.TESubFunctions2 = new HashSet<TESubFunction>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string ObjectId { get; set; }
        public Nullable<int> TECompany { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> HeadUser { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClearanceMaster> TEClearanceMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClearanceMaster> TEClearanceMasters1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClearanceMaster> TEClearanceMasters2 { get; set; }
        public virtual TECompany TECompany1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEDepartment> TEDepartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEDepartment> TEDepartments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEDepartment> TEDepartments2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEQueueDepartment> TEQueueDepartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEQueueDepartment> TEQueueDepartments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEQueueDepartment> TEQueueDepartments2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESubFunction> TESubFunctions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESubFunction> TESubFunctions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESubFunction> TESubFunctions2 { get; set; }
    }
}
