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
    
    public partial class TEOfferPremium
    {
        public int PremiumId { get; set; }
        public Nullable<int> OfferID { get; set; }
        public string PremiumType { get; set; }
        public Nullable<decimal> PremiumMultiplier { get; set; }
        public Nullable<int> TotalPremiumPrice { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
