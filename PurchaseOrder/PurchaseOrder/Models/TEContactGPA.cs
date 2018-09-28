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
    
    public partial class TEContactGPA
    {
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEContact { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string House_ { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Locality { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string LandMark { get; set; }
        public string RelationOf { get; set; }
        public string RelationName { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public Nullable<int> TEContactOfNominee { get; set; }
        public string Title { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string CurrentProfession { get; set; }
        public string Industry { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
    
        public virtual TEContact TEContact1 { get; set; }
        public virtual TEContact TEContact2 { get; set; }
        public virtual TEContact TEContact3 { get; set; }
        public virtual TEContact TEContact4 { get; set; }
    }
}
