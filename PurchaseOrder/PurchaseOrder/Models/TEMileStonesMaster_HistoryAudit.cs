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
    
    public partial class TEMileStonesMaster_HistoryAudit
    {
        public int TRID { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public string ScheduleCode { get; set; }
        public string ScheduleType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> OldLastModifiedBy { get; set; }
        public Nullable<System.DateTime> OldLastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
        public string Status { get; set; }
    }
}
