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
    
    public partial class TEApplicantMobileDetail
    {
        public int ApplicantMobileDetailID { get; set; }
        public Nullable<int> ApplicantID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public string CountryCode { get; set; }
        public string Mobile { get; set; }
        public string Type { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
