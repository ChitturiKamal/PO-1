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
    
    public partial class GetAllTEOffersExpiresByOfferID_Result
    {
        public int OfferExtensionID { get; set; }
        public Nullable<int> OfferID { get; set; }
        public Nullable<System.DateTime> CurrentExpiredate { get; set; }
        public Nullable<System.DateTime> RequestedExpiredate { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
}
