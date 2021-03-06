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
    
    public partial class TEEventList
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public Nullable<int> MCMID { get; set; }
        public Nullable<int> PSID { get; set; }
        public Nullable<int> SSMID { get; set; }
        public Nullable<decimal> EventCost { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public Nullable<int> CreativeAttachment { get; set; }
        public string Description { get; set; }
        public string CDN { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string LocationDetailes { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AgencyCode { get; set; }
        public string ExpectedResponse { get; set; }
        public string ActualResponse { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<int> EventType { get; set; }
    }
}
