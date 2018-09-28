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
    
    public partial class TETaxDetail
    {
        public int TaxDetailsID { get; set; }
        public int ProjectID { get; set; }
        public int TaxConsiderationType { get; set; }
        public string SAPTaxCode { get; set; }
        public int TaxType { get; set; }
        public string TaxValueType { get; set; }
        public Nullable<decimal> TaxRate { get; set; }
        public Nullable<System.DateTime> SalesPeriodFromDate { get; set; }
        public Nullable<System.DateTime> SalesPeriodToDate { get; set; }
        public Nullable<System.DateTime> TaxationValidityFromDate { get; set; }
        public Nullable<System.DateTime> TaxationValidityToDate { get; set; }
        public Nullable<bool> IsTaxInclusive { get; set; }
        public string TaxationInApplicabilityEvent { get; set; }
        public int TowerID { get; set; }
        public string RevisionNo { get; set; }
        public string TaxBookID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    
        public virtual TEPickListItem TEPickListItem { get; set; }
        public virtual TEPickListItem TEPickListItem1 { get; set; }
        public virtual TEProject TEProject { get; set; }
        public virtual TETowerMaster TETowerMaster { get; set; }
    }
}
