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
    
    public partial class WebPageAudit
    {
        public int TransactionId { get; set; }
        public string EventURL { get; set; }
        public string EventGeneratedBy { get; set; }
        public Nullable<System.DateTime> EventDate { get; set; }
        public string EventRaisedByUser { get; set; }
        public string Remarks { get; set; }
        public string OldUniqueID { get; set; }
    }
}
