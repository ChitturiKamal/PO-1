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
    
    public partial class TEEmailTemplateAttach
    {
        public int ETemplateAttachID { get; set; }
        public Nullable<int> EmailTemplateID { get; set; }
        public int DocumentID { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEEmailTemplate TEEmailTemplate { get; set; }
    }
}
