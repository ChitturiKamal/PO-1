//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEComplaintsManagementDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TEEMPAddress
    {
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEEmpBasicInfo { get; set; }
        public string HouseNo { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Locality { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string LandMark { get; set; }
        public string TypeOfAddress { get; set; }
        public string Pincode { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
    }
}
