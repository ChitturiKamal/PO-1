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
    
    public partial class TEBandsAndGrade
    {
        public TEBandsAndGrade()
        {
            this.TEEmpAssignmentDetails = new HashSet<TEEmpAssignmentDetail>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TECompany { get; set; }
        public string BandName { get; set; }
        public string GradeName { get; set; }
        public string Designation { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        public virtual TECompany TECompany1 { get; set; }
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails { get; set; }
    }
}
