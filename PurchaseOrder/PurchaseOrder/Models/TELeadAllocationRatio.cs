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
    
    public partial class TELeadAllocationRatio
    {
        public int UniqueId { get; set; }
        public Nullable<int> SalesConsultantId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> Ratio { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
}