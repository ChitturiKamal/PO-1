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
    
    public partial class TEOrgRule
    {
        public int OrgnizationRuleID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> TowerID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public Nullable<int> EDesignSyncStatus { get; set; }
    }
}
