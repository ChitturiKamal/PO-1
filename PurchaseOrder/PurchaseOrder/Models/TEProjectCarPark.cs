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
    
    public partial class TEProjectCarPark
    {
        public int ProjectCarParkID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> CarParkID { get; set; }
        public Nullable<int> TotalCarParks { get; set; }
        public Nullable<decimal> CarParkRate { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<int> MinimumCarParks { get; set; }
        public Nullable<int> MaximumCarParks { get; set; }
    
        public virtual TECarParksMaster TECarParksMaster { get; set; }
        public virtual TEProject TEProject { get; set; }
    }
}
