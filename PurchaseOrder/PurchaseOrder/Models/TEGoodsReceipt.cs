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
    
    public partial class TEGoodsReceipt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEGoodsReceipt()
        {
            this.TEGoodsReceiptItems = new HashSet<TEGoodsReceiptItem>();
        }
    
        public int UniqueId { get; set; }
        public int POStructureID { get; set; }
        public string SAPGRNCode { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public string BillOfLading { get; set; }
        public string InvoiceNumber { get; set; }
        public string MovementTypes { get; set; }
        public string Comments { get; set; }
        public Nullable<int> DocumentYear { get; set; }
        public string GRNStatus { get; set; }
        public Nullable<bool> ISQAChecked { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
    
        public virtual TEPurchase_header_structure TEPurchase_header_structure { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEGoodsReceiptItem> TEGoodsReceiptItems { get; set; }
    }
}