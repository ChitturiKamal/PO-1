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
    
    public partial class TEOfferSpecification
    {
        public int OfferSpecsID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public Nullable<int> SpecsID { get; set; }
        public string SpecsColourCode { get; set; }
        public string SpecsCategory { get; set; }
        public string SpecsSubCategory { get; set; }
        public string SpecsDescription { get; set; }
        public Nullable<int> SEQNO { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEOffer TEOffer { get; set; }
    }
}
