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
    
    public partial class TEPOSpecificationAnnexure
    {
        public int SpecAnnexureId { get; set; }
        public Nullable<int> POItemStructureId { get; set; }
        public Nullable<int> POHeaderStructureId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialGroup { get; set; }
        public string CmpLibId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
}
