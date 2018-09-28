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
    
    public partial class TEClaimCategoryGLMapping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEClaimCategoryGLMapping()
        {
            this.TEClaimItems = new HashSet<TEClaimItem>();
            this.TEClaimItems1 = new HashSet<TEClaimItem>();
        }
    
        public int UniqueId { get; set; }
        public Nullable<int> ClaimCategory { get; set; }
        public Nullable<int> Company { get; set; }
        public string Type { get; set; }
        public string GLCode { get; set; }
        public string GLDescription { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual TEClaimCategory TEClaimCategory { get; set; }
        public virtual TEClaimCategory TEClaimCategory1 { get; set; }
        public virtual TEClaimCategory TEClaimCategory2 { get; set; }
        public virtual TECompany TECompany { get; set; }
        public virtual TECompany TECompany1 { get; set; }
        public virtual TECompany TECompany2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClaimItem> TEClaimItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClaimItem> TEClaimItems1 { get; set; }
    }
}
