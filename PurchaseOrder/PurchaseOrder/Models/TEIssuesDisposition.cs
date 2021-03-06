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
    
    public partial class TEIssuesDisposition
    {
        public int Uniqueid { get; set; }
        public string Disposition { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime RaisedDate { get; set; }
        public System.DateTime ResolvedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public Nullable<int> TEEmpBasicInfo { get; set; }
        public Nullable<int> TEIssueId { get; set; }
        public Nullable<int> LeaveId { get; set; }
    
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo2 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo3 { get; set; }
        public virtual TEEmpLeaveApplication TEEmpLeaveApplication { get; set; }
        public virtual TEEmpLeaveApplication TEEmpLeaveApplication1 { get; set; }
        public virtual TEIssue TEIssue { get; set; }
        public virtual TEIssue TEIssue1 { get; set; }
        public virtual TEIssue TEIssue2 { get; set; }
    }
}
