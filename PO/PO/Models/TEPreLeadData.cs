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
    
    public partial class TEPreLeadData
    {
        public int PreLeadID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<int> SourceType { get; set; }
        public string SourceName { get; set; }
        public string PrefferedProject { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<int> ActedBy { get; set; }
        public Nullable<System.DateTime> ActedOn { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CountryCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public Nullable<int> SecondarySourceType { get; set; }
        public Nullable<int> PConnect { get; set; }
        public Nullable<int> PConnectDetails { get; set; }
        public Nullable<int> SalesEngine { get; set; }
        public Nullable<int> CityOfChoice { get; set; }
        public Nullable<int> PreferredLocation { get; set; }
        public Nullable<int> Origin { get; set; }
    }
}
