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
    
    public partial class TECarPark
    {
        public int CarParkID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> NoOfCarparks { get; set; }
        public Nullable<decimal> CostOfPerCarParkRate { get; set; }
        public Nullable<decimal> TotalCarParkPrice { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy_ID { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEOffer TEOffer { get; set; }
    }
}