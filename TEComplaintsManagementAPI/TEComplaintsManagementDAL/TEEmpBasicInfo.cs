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
    
    public partial class TEEmpBasicInfo
    {
        public TEEmpBasicInfo()
        {
            this.TEEMPAddresses = new HashSet<TEEMPAddress>();
            this.TEEmpAssignmentDetails = new HashSet<TEEmpAssignmentDetail>();
            this.TEEmpAssignmentDetails1 = new HashSet<TEEmpAssignmentDetail>();
            this.TEEmpAttendences = new HashSet<TEEmpAttendence>();
            this.TEEmpAttendences1 = new HashSet<TEEmpAttendence>();
            this.TEEmpBankDetails = new HashSet<TEEmpBankDetail>();
            this.TEEmpEducationDetails = new HashSet<TEEmpEducationDetail>();
            this.TEEmpEmergencyContacts = new HashSet<TEEmpEmergencyContact>();
            this.TEEmpExperienceDetails = new HashSet<TEEmpExperienceDetail>();
            this.TEEmpFamilyDetails = new HashSet<TEEmpFamilyDetail>();
            this.TEEmpInsuranceDetails = new HashSet<TEEmpInsuranceDetail>();
            this.TEEmpLanguages = new HashSet<TEEmpLanguage>();
            this.TEEmpLeaveApplications = new HashSet<TEEmpLeaveApplication>();
            this.TEEmpLeavesSummaries = new HashSet<TEEmpLeavesSummary>();
            this.TEEmpOtherDetails = new HashSet<TEEmpOtherDetail>();
            this.TEEmpReferences = new HashSet<TEEmpReference>();
            this.TEEmpWorkSchedules = new HashSet<TEEmpWorkSchedule>();
            this.TEProjects = new HashSet<TEProject>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string EmployeeId { get; set; }
        public string UserId { get; set; }
        public string TEUser { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RelationOf { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<System.DateTime> DateOfMarriage { get; set; }
        public string LandlineNo { get; set; }
        public string Mobile { get; set; }
        public string PersonalEmail { get; set; }
        public string OfficialEmail { get; set; }
        public string BloodGroup { get; set; }
        public string Photo { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> DateOfConfirmation { get; set; }
        public Nullable<decimal> TEExperience { get; set; }
        public Nullable<decimal> PrevExperience { get; set; }
        public Nullable<decimal> TotalExperience { get; set; }
        public Nullable<System.DateTime> ProposedDOJ { get; set; }
        public Nullable<System.DateTime> ProposedDOL { get; set; }
        public Nullable<System.DateTime> DOL { get; set; }
        public string Reason { get; set; }
    
        public virtual ICollection<TEEMPAddress> TEEMPAddresses { get; set; }
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails { get; set; }
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails1 { get; set; }
        public virtual ICollection<TEEmpAttendence> TEEmpAttendences { get; set; }
        public virtual ICollection<TEEmpAttendence> TEEmpAttendences1 { get; set; }
        public virtual ICollection<TEEmpBankDetail> TEEmpBankDetails { get; set; }
        public virtual ICollection<TEEmpEducationDetail> TEEmpEducationDetails { get; set; }
        public virtual ICollection<TEEmpEmergencyContact> TEEmpEmergencyContacts { get; set; }
        public virtual ICollection<TEEmpExperienceDetail> TEEmpExperienceDetails { get; set; }
        public virtual ICollection<TEEmpFamilyDetail> TEEmpFamilyDetails { get; set; }
        public virtual ICollection<TEEmpInsuranceDetail> TEEmpInsuranceDetails { get; set; }
        public virtual ICollection<TEEmpLanguage> TEEmpLanguages { get; set; }
        public virtual ICollection<TEEmpLeaveApplication> TEEmpLeaveApplications { get; set; }
        public virtual ICollection<TEEmpLeavesSummary> TEEmpLeavesSummaries { get; set; }
        public virtual ICollection<TEEmpOtherDetail> TEEmpOtherDetails { get; set; }
        public virtual ICollection<TEEmpReference> TEEmpReferences { get; set; }
        public virtual ICollection<TEEmpWorkSchedule> TEEmpWorkSchedules { get; set; }
        public virtual ICollection<TEProject> TEProjects { get; set; }
    }
}