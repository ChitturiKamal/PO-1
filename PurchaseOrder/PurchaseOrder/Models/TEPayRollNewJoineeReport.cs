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
    
    public partial class TEPayRollNewJoineeReport
    {
        public string Createdby { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public int NewJoineeReportID { get; set; }
        public Nullable<int> PayrollInputFileID { get; set; }
        public Nullable<int> Teempbasicinfo { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string WorkLevel { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public string FunctionName { get; set; }
        public string Position { get; set; }
        public string PanNumber { get; set; }
        public string SalaryBankName { get; set; }
        public string SalaryIFSCCode { get; set; }
        public string SalaryAccountNumber { get; set; }
        public string ReimbursementBankName { get; set; }
        public string ReimbursementIFSCCode { get; set; }
        public string ReimbursementAccountNumber { get; set; }
        public string LegalEntity { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterName { get; set; }
        public Nullable<decimal> CTC { get; set; }
        public Nullable<decimal> Basic { get; set; }
        public Nullable<decimal> HRA_A { get; set; }
        public Nullable<decimal> LWF { get; set; }
        public Nullable<decimal> Conv { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> SpecialAllowance { get; set; }
        public Nullable<decimal> TotalFixedPay_A { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> Flexi { get; set; }
        public string Status { get; set; }
        public bool Isdeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedon { get; set; }
    
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEPayRollInputFile TEPayRollInputFile { get; set; }
    }
}
