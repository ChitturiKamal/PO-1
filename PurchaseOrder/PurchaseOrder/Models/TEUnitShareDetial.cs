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
    
    public partial class TEUnitShareDetial
    {
        public int UnitShareDetailID { get; set; }
        public Nullable<int> ProjectShareID { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<decimal> PercentageofOwnershipOfUnit { get; set; }
        public Nullable<int> DefaultSpecificationCode { get; set; }
        public Nullable<int> DefaultNumberOfCarParks { get; set; }
        public string DefaultCarParkType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<int> AllocatedArea { get; set; }
    
        public virtual TEShareDetail TEShareDetail { get; set; }
    }
}