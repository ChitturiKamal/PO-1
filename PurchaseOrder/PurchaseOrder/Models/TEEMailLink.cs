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
    
    public partial class TEEMailLink
    {
        public int UniqueID { get; set; }
        public string EMailLink { get; set; }
        public string SAPCustomerID { get; set; }
        public string EncryptValue { get; set; }
        public string EMailCode { get; set; }
        public Nullable<bool> IsExpired { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> AccessedStatus { get; set; }
        public string SourceFrom { get; set; }
        public Nullable<int> ReferenceID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    }
}
