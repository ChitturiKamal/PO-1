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
    
    public partial class TEEmpSeparationQuestionNResponse
    {
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public System.DateTime LastModifiedOn { get; set; }
        public string ObjectId { get; set; }
        public string OldUniqueId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int UniqueId { get; set; }
        public Nullable<int> TEEmpSeparation { get; set; }
        public Nullable<int> TEQuestionMaster { get; set; }
        public Nullable<bool> IsSpecificToEmployee { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> TEBucketMaster { get; set; }
    
        public virtual TEEmpSeparation TEEmpSeparation1 { get; set; }
    }
}
