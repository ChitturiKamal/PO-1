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
    
    public partial class TEInvoiceApprover
    {
        public int UniqueId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> IsApprover { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApproverType { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
    }
}