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
    
    public partial class TEUnitPricingShareDetail
    {
        public int UnitPricingShareDetailsID { get; set; }
        public Nullable<int> ProjectShareID { get; set; }
        public string ElementType { get; set; }
        public Nullable<decimal> ElementMultiplier { get; set; }
        public Nullable<decimal> ElementPrice { get; set; }
        public string OwnerShipType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEShareDetail TEShareDetail { get; set; }
    }
}
