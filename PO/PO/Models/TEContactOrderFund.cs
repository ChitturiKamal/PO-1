//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TEContactOrderFund
    {
        public int UniqueID { get; set; }
        public string SAPCustomerID { get; set; }
        public string SAPOrderID { get; set; }
        public Nullable<int> TypeofFunding { get; set; }
        public Nullable<decimal> AmountOfBankFunding { get; set; }
        public Nullable<bool> PreApprovedLoan { get; set; }
        public string LoanAccountNumber { get; set; }
        public Nullable<decimal> ApprovedAmount { get; set; }
        public string BankContactPerson { get; set; }
        public string BankContactNumber { get; set; }
        public string BankEmailID { get; set; }
        public string BankAddress { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<int> TypeOfAccount { get; set; }
        public Nullable<bool> PreferredBankForLoan { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
