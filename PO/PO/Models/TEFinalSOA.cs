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
    
    public partial class TEFinalSOA
    {
        public int FinalSOAID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public string SAPCustomerID { get; set; }
        public string SAPOrderID { get; set; }
        public string Status { get; set; }
        public Nullable<int> InitiatedBy { get; set; }
        public Nullable<System.DateTime> InitiatedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsGroupOneApproved { get; set; }
        public Nullable<bool> IsGroupTwoApproved { get; set; }
        public Nullable<bool> IsGroupThreeApproved { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int SignedFSOADMSId { get; set; }
    }
}
