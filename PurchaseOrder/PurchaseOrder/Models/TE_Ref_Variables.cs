//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PurchaseOrder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TE_Ref_Variables
    {
        public int RefVariableID { get; set; }
        public string Name { get; set; }
        public Nullable<int> LastModifiedByID { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> RefDocumentTypeID { get; set; }
        public Nullable<int> TemplateMethod { get; set; }
        public string Description { get; set; }
    
        public virtual TE_Ref_Document_Type TE_Ref_Document_Type { get; set; }
    }
}
