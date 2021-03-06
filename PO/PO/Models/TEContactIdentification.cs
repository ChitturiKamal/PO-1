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
    
    public partial class TEContactIdentification
    {
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string IssuedBy { get; set; }
        public string Type { get; set; }
        public Nullable<int> TEContact { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string IdentityProof { get; set; }
        public string IdentityProoofNumber { get; set; }
        public string NameonIDProof { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }
        public Nullable<System.DateTime> ValidityofID { get; set; }
        public string AttachmentforIdProof { get; set; }
        public string AttachmentProofDocumentType { get; set; }
        public string AttachmentforAddressProof { get; set; }
        public string PANCardNumber { get; set; }
        public string Photo { get; set; }
        public Nullable<bool> GPAApplicable { get; set; }
        public string AadharNumber { get; set; }
    
        public virtual TEContact TEContact1 { get; set; }
    }
}
