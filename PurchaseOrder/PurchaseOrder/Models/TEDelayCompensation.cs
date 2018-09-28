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
    
    public partial class TEDelayCompensation
    {
        public int DCID { get; set; }
        public Nullable<System.DateTime> CurrentHandoverDate { get; set; }
        public Nullable<int> Units { get; set; }
        public Nullable<decimal> CompensationValue { get; set; }
        public Nullable<int> GracePeriod { get; set; }
        public Nullable<decimal> CompensationPerMonth { get; set; }
        public Nullable<System.DateTime> ProsposedHandoverDate { get; set; }
        public Nullable<decimal> TotalCompensation { get; set; }
        public Nullable<decimal> TotalCompensationDuration { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public Nullable<System.DateTime> MontlyPayByDate { get; set; }
        public string PaidTo { get; set; }
        public Nullable<int> CompType { get; set; }
        public Nullable<int> ConditionType { get; set; }
        public string ConditionText { get; set; }
        public string SAPCustomerID { get; set; }
        public Nullable<int> OfferId { get; set; }
        public Nullable<int> LastModifiedById { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CurrentDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> RevisedHandOverDate { get; set; }
    }
}
