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
    
    public partial class TEDocumentSubTypeMaster
    {
        public int SubTypeID { get; set; }
        public int TypeID { get; set; }
        public string DocumentSubType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEDocumentTypeMaster TEDocumentTypeMaster { get; set; }
    }
}
