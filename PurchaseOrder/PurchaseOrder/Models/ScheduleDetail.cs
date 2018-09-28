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
    
    public partial class ScheduleDetail
    {
        public int ScheduleDetailID { get; set; }
        public Nullable<int> ScheduleMasterID { get; set; }
        public Nullable<int> MilestoneID { get; set; }
        public Nullable<int> SequenceOrder { get; set; }
        public Nullable<System.DateTime> BaseLineDate { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }
        public Nullable<System.DateTime> TodayOfferDate { get; set; }
        public Nullable<decimal> RevenueRatio { get; set; }
        public Nullable<decimal> SchemeRevenueRatio { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> RelativeMileStone { get; set; }
        public Nullable<int> RelativeInterval { get; set; }
        public string MileStoneType { get; set; }
        public Nullable<int> LastModifiedBy_ID { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string OldUniqueID { get; set; }
        public string MilestoneInterval { get; set; }
        public string OfferDateRule { get; set; }
        public string Status { get; set; }
        public Nullable<bool> displaydateinagreement { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string CompletionStatus { get; set; }
        public string RelativeMilestoneCode { get; set; }
        public string Remarks { get; set; }
        public string ApprovalStatus { get; set; }
        public string ModifiedProjectionStatus { get; set; }
        public Nullable<System.DateTime> ModifiedProjectionDate { get; set; }
    }
}
