//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEComplaintsManagementDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TELead
    {
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEContact { get; set; }
        public string Saluation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CallName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string LeadOriginLocation { get; set; }
        public string LeadOriginState { get; set; }
        public string LeadOriginRegionon { get; set; }
        public string LeadOriginIpaddress { get; set; }
        public string CityChoice { get; set; }
        public string PrefferedLocation { get; set; }
        public Nullable<long> Budjet { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string FirstConnect { get; set; }
        public string PrimarySource { get; set; }
        public string SecondarySource { get; set; }
        public Nullable<int> TECampaign { get; set; }
        public Nullable<int> PrefferedContactid { get; set; }
        public string SalesEngine { get; set; }
        public string ResponseTeamStatus { get; set; }
        public string ResponseTeamLeadCategorisation { get; set; }
        public string ResponseTeamStatusReason { get; set; }
        public string ResponseTeamUdatedby { get; set; }
        public Nullable<System.DateTime> ResponseTeamUpdatedon { get; set; }
        public string SalesTeamUpdatedby { get; set; }
        public Nullable<System.DateTime> SalesTeamUpdatedon { get; set; }
        public string SalesTeamStatus { get; set; }
        public string SalesTeamLeadCategorisation { get; set; }
        public string SalesTeamSStatusReason { get; set; }
        public string LeadStatus { get; set; }
        public string Remarks { get; set; }
        public string InterestedProjects { get; set; }
        public string PrefferedProjects { get; set; }
        public string ResponseTeamby { get; set; }
        public string SaleTeamby { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
    
        public virtual TEContact TEContact1 { get; set; }
    }
}
