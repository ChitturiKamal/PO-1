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
    
    public partial class TEOfferScheme
    {
        public int OfferSchemeId { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> OfferId { get; set; }
        public string SchemeName { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public Nullable<System.DateTime> lastModifiedOn { get; set; }
    
        public virtual TEOffer TEOffer { get; set; }
    }
}
