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
    
    public partial class TECategory_UOMMapping
    {
        public int UniqueId { get; set; }
        public Nullable<int> ClaimCategory { get; set; }
        public Nullable<int> ClaimLimitUOM { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual TEClaimCategory TEClaimCategory { get; set; }
        public virtual TEClaimLimitUOM TEClaimLimitUOM { get; set; }
        public virtual TEClaimCategory TEClaimCategory1 { get; set; }
        public virtual TEClaimLimitUOM TEClaimLimitUOM1 { get; set; }
        public virtual TEClaimCategory TEClaimCategory2 { get; set; }
        public virtual TEClaimLimitUOM TEClaimLimitUOM2 { get; set; }
    }
}
