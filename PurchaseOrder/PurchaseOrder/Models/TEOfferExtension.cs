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
    
    public partial class TEOfferExtension
    {
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> OfferExtensionId { get; set; }
        public Nullable<int> TEOffer { get; set; }
        public Nullable<System.DateTime> CurrentExpireDate { get; set; }
        public Nullable<System.DateTime> RequestedExpireDate { get; set; }
        public string CreatedComments { get; set; }
        public string ApprovedComments { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
    }
}
