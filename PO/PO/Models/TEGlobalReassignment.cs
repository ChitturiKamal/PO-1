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
    
    public partial class TEGlobalReassignment
    {
        public int ReassignID { get; set; }
        public Nullable<int> ContextID { get; set; }
        public string ContextType { get; set; }
        public string Role { get; set; }
        public Nullable<int> FromUserID { get; set; }
        public Nullable<int> ToUserID { get; set; }
        public Nullable<int> AssignedBy { get; set; }
        public Nullable<System.DateTime> AssignedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
