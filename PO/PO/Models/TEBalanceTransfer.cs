//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TEBalanceTransfer
    {
        public int ID { get; set; }
        public int OfferId { get; set; }
        public string FromCustomerId { get; set; }
        public string ToCustomerId { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> TransferOn { get; set; }
        public Nullable<int> TransferBy { get; set; }
        public string SAPDocumentId { get; set; }
        public string Relationship { get; set; }
        public string status { get; set; }
        public string Description { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
    }
}
