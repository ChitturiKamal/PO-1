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
    
    public partial class TEEmpEducationDetail
    {
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEEmpBasicInfo { get; set; }
        public string Type { get; set; }
        public string Course { get; set; }
        public string MajorDiscipline { get; set; }
        public string Institution { get; set; }
        public string Location { get; set; }
        public string EducationMode { get; set; }
        public Nullable<System.DateTime> StartYear { get; set; }
        public Nullable<System.DateTime> EndYear { get; set; }
        public Nullable<decimal> PercentageGrade { get; set; }
        public Nullable<bool> IsHonors { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
    }
}
