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
    
    public partial class TEBroadcastDetail
    {
        public int UniqueID { get; set; }
        public int BroadcastID { get; set; }
        public Nullable<int> RoleID { get; set; }
    
        public virtual TEBroadcast TEBroadcast { get; set; }
    }
}
