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
    
    public partial class TEEmpAssignmentDetail
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
        public Nullable<int> TEBandAndGrade { get; set; }
        public Nullable<int> TEWorkingDesignation { get; set; }
        public Nullable<int> TECompany { get; set; }
        public Nullable<int> TEDepartment { get; set; }
        public Nullable<int> TEWorkLocation { get; set; }
        public Nullable<int> TEEmpBasicInfo_ReportingTo { get; set; }
        public string NatureOfEmployment { get; set; }
        public string WorkGroup { get; set; }
        public string ProjectSite { get; set; }
        public string ProbationStatus { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public Nullable<int> TESubFunction { get; set; }
        public string ReasonForChange { get; set; }
        public Nullable<System.DateTime> NewEffectiveDate { get; set; }
        public Nullable<bool> AttendenceFlag { get; set; }
    
        public virtual TEBandsAndGrade TEBandsAndGrade { get; set; }
        public virtual TECompany TECompany1 { get; set; }
        public virtual TEDepartment TEDepartment1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo2 { get; set; }
        public virtual TEWorkingDesignation TEWorkingDesignation1 { get; set; }
        public virtual TEWorkLocation TEWorkLocation1 { get; set; }
        public virtual TESubFunction TESubFunction1 { get; set; }
    }
}
