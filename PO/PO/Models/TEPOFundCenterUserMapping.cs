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
    
    public partial class TEPOFundCenterUserMapping
    {
        public int FundCenterUserMappingId { get; set; }
        public int FundCenterId { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual TEPOFundCenter TEPOFundCenter { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}