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
    
    public partial class TEApproveGroupUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEApproveGroupUser()
        {
            this.TEApprovalProcesses = new HashSet<TEApprovalProcess>();
        }
    
        public int TRID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> UserLevel { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<int> ProjectID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEApprovalProcess> TEApprovalProcesses { get; set; }
        public virtual TEApproveGroup TEApproveGroup { get; set; }
    }
}