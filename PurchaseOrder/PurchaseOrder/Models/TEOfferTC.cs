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
    
    public partial class TEOfferTC
    {
        public int OfferTCID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public string TCName { get; set; }
        public Nullable<int> SEQNO { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> BlockId { get; set; }
        public string TypeName { get; set; }
    
        public virtual TEOffer TEOffer { get; set; }
    }
}
