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
    
    public partial class TECarParkSummary
    {
        public int CarParkID { get; set; }
        public string CarParkName { get; set; }
        public Nullable<decimal> RatePerCarPark { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LostModifiedBy { get; set; }
        public Nullable<System.DateTime> LostModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
    }
}
