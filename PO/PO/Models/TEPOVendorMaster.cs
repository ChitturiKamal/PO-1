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
    
    public partial class TEPOVendorMaster
    {
        public int POVendorMasterId { get; set; }
        public Nullable<int> VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string Currency { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string ServiceTax { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
}
