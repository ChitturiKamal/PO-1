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
    
    public partial class TEPayRollFFReport
    {
        public string Createdby { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public int FFReportID { get; set; }
        public Nullable<int> PayrollInputFileID { get; set; }
        public Nullable<int> Teempbasicinfo { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> ResignationDate { get; set; }
        public Nullable<System.DateTime> LastWorkingDate { get; set; }
        public string WorkLevel { get; set; }
        public string location { get; set; }
        public string FunctionName { get; set; }
        public string Entity { get; set; }
        public Nullable<decimal> LeaveBalance { get; set; }
        public Nullable<decimal> NoticePeriod { get; set; }
        public string Status { get; set; }
        public bool Isdeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedon { get; set; }
    
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEPayRollInputFile TEPayRollInputFile { get; set; }
    }
}