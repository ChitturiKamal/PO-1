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
    
    public partial class TECampaignMaster
    {
        public int MCMID { get; set; }
        public string Name { get; set; }
        public string AgencyCode { get; set; }
        public Nullable<decimal> OverHeadCost { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> MediaID { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    }
}
