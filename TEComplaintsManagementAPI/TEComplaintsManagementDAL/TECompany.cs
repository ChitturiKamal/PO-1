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
    
    public partial class TECompany
    {
        public TECompany()
        {
            this.TEBandsAndGrades = new HashSet<TEBandsAndGrade>();
            this.TEDepartments = new HashSet<TEDepartment>();
            this.TEEmpAssignmentDetails = new HashSet<TEEmpAssignmentDetail>();
            this.TESubFunctions = new HashSet<TESubFunction>();
            this.TELineOfBusinesses = new HashSet<TELineOfBusiness>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string CIN { get; set; }
        public string PF_Reg_No { get; set; }
        public string Address { get; set; }
        public Nullable<int> HeadUser { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<TEBandsAndGrade> TEBandsAndGrades { get; set; }
        public virtual ICollection<TEDepartment> TEDepartments { get; set; }
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails { get; set; }
        public virtual ICollection<TESubFunction> TESubFunctions { get; set; }
        public virtual ICollection<TELineOfBusiness> TELineOfBusinesses { get; set; }
    }
}