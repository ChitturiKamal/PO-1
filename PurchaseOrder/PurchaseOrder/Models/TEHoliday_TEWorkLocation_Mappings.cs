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
    
    public partial class TEHoliday_TEWorkLocation_Mappings
    {
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEWorkLocation { get; set; }
        public string WorkGroup { get; set; }
        public Nullable<int> TEHoliday { get; set; }
        public Nullable<bool> Applicable { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public Nullable<int> TESubFunction { get; set; }
    
        public virtual TEHoliday TEHoliday1 { get; set; }
        public virtual TEHoliday TEHoliday2 { get; set; }
        public virtual TEHoliday TEHoliday3 { get; set; }
        public virtual TEWorkLocation TEWorkLocation1 { get; set; }
        public virtual TEWorkLocation TEWorkLocation2 { get; set; }
        public virtual TEWorkLocation TEWorkLocation3 { get; set; }
        public virtual TESubFunction TESubFunction1 { get; set; }
    }
}
