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
    
    public partial class TEEmpLeaveApplication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEEmpLeaveApplication()
        {
            this.TEEmpAttendences = new HashSet<TEEmpAttendence>();
            this.TEEmpSpecialLeaveApplications = new HashSet<TEEmpSpecialLeaveApplication>();
            this.TEEmpSpecialLeaveApplications1 = new HashSet<TEEmpSpecialLeaveApplication>();
            this.TEEmpSpecialLeaveApplications2 = new HashSet<TEEmpSpecialLeaveApplication>();
            this.TEIssuesDispositions = new HashSet<TEIssuesDisposition>();
            this.TEIssuesDispositions1 = new HashSet<TEIssuesDisposition>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEEmpBasicInfo { get; set; }
        public Nullable<int> TEEmpLeaveType { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<bool> IsHalfday { get; set; }
        public Nullable<decimal> NoOfDays { get; set; }
        public string SubmittedByTEEmpBasicInfo { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public string Reason { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public Nullable<bool> FromDateIsHalfday { get; set; }
        public Nullable<bool> ToDateIsHalfday { get; set; }
        public string ReasonForRejection { get; set; }
        public string LeaveName { get; set; }
        public string LeaveType { get; set; }
        public Nullable<int> LeaveSummary { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpAttendence> TEEmpAttendences { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo2 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpSpecialLeaveApplication> TEEmpSpecialLeaveApplications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpSpecialLeaveApplication> TEEmpSpecialLeaveApplications1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpSpecialLeaveApplication> TEEmpSpecialLeaveApplications2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEIssuesDisposition> TEIssuesDispositions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEIssuesDisposition> TEIssuesDispositions1 { get; set; }
        public virtual TEEmpLeavesSummary TEEmpLeavesSummary { get; set; }
    }
}