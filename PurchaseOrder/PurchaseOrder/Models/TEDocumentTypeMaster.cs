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
    
    public partial class TEDocumentTypeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEDocumentTypeMaster()
        {
            this.TEDocumentSubTypeMasters = new HashSet<TEDocumentSubTypeMaster>();
        }
    
        public int TypeId { get; set; }
        public int CategoryID { get; set; }
        public string DocumentType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEDcoumentCatMaster TEDcoumentCatMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEDocumentSubTypeMaster> TEDocumentSubTypeMasters { get; set; }
    }
}
