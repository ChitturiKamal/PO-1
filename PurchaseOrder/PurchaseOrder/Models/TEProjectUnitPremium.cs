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
    
    public partial class TEProjectUnitPremium
    {
        public int ProjectUnitPremiumID { get; set; }
        public Nullable<int> ProjectPremiumID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<decimal> PremiumValue { get; set; }
        public string MulRateType { get; set; }
        public string MulQuantity { get; set; }
        public string Mulrate { get; set; }
    
        public virtual TEProjectPremium TEProjectPremium { get; set; }
    }
}
