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
    
    public partial class TESpecificationDetailed
    {
        public int SpecificationDetailedID { get; set; }
        public string TemplateName { get; set; }
        public Nullable<int> SeqNo { get; set; }
        public Nullable<int> SpecificationCategoryID { get; set; }
        public string SubCategoryID { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ApplicableSpecificationID { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    }
}
