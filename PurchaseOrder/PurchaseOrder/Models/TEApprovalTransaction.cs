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
    
    public partial class TEApprovalTransaction
    {
        public int AppTransctionID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string AppObjectName { get; set; }
        public Nullable<int> AppObjectReferenceID { get; set; }
        public Nullable<int> AppGroupID { get; set; }
        public Nullable<int> AppUserID { get; set; }
        public Nullable<int> AppUserLevel { get; set; }
        public Nullable<int> AppGroupMaxLevel { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string ApproverRemarks { get; set; }
        public string ApproverStatus { get; set; }
        public string AppObjectFinalStatus { get; set; }
        public string AppObjectCurrentStatus { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string AppObjectCode { get; set; }
    }
}
