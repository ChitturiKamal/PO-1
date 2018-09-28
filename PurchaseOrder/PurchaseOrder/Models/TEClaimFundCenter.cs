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
    
    public partial class TEClaimFundCenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEClaimFundCenter()
        {
            this.TEClaimBooks = new HashSet<TEClaimBook>();
            this.TEClaimBooks1 = new HashSet<TEClaimBook>();
            this.TEClaimBooks2 = new HashSet<TEClaimBook>();
        }
    
        public int UniqueId { get; set; }
        public string FundCenterCode { get; set; }
        public string FundCenterDescription { get; set; }
        public string WBSCode { get; set; }
        public string WBSDescription { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<int> FundCenterOwner { get; set; }
        public string CompanyCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClaimBook> TEClaimBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClaimBook> TEClaimBooks1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEClaimBook> TEClaimBooks2 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo2 { get; set; }
    }
}