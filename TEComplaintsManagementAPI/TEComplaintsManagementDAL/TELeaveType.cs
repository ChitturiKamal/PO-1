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
    
    public partial class TELeaveType
    {
        public TELeaveType()
        {
            this.TEEmpLeavesSummaries = new HashSet<TEEmpLeavesSummary>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> AnnualNumOfLeave { get; set; }
        public Nullable<decimal> Accrual { get; set; }
        public Nullable<decimal> DurationOfAccrual { get; set; }
        public Nullable<bool> IsAccrueAtBegining { get; set; }
        public Nullable<bool> IsCarryForward { get; set; }
        public Nullable<decimal> MaxCarryForward { get; set; }
        public Nullable<bool> EnCashable { get; set; }
        public Nullable<float> MaxEnCashable { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public string EligibleGender { get; set; }
        public string EmployeeGroup { get; set; }
        public Nullable<System.DateTime> ApplicableFromDate { get; set; }
        public Nullable<System.DateTime> ApplicableToDate { get; set; }
        public Nullable<decimal> MinBalanceAllowed { get; set; }
        public Nullable<decimal> MaxAllowedAtATime { get; set; }
        public string colorcode { get; set; }
        public Nullable<int> TEPicklistitemid { get; set; }
    
        public virtual ICollection<TEEmpLeavesSummary> TEEmpLeavesSummaries { get; set; }
        public virtual TEPickListItem TEPickListItem { get; set; }
    }
}