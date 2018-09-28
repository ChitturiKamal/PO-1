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
    
    public partial class TEAssetVariantValue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEAssetVariantValue()
        {
            this.TEAssetVariantMappings = new HashSet<TEAssetVariantMapping>();
            this.TEAssetVariantMappings1 = new HashSet<TEAssetVariantMapping>();
            this.TEAssetVariantMappings2 = new HashSet<TEAssetVariantMapping>();
        }
    
        public int Uniqueid { get; set; }
        public string Value { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public string SubmittedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public Nullable<int> TEAssetVariantTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEAssetVariantMapping> TEAssetVariantMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEAssetVariantMapping> TEAssetVariantMappings1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEAssetVariantMapping> TEAssetVariantMappings2 { get; set; }
        public virtual TEAssetVariantType TEAssetVariantType { get; set; }
        public virtual TEAssetVariantType TEAssetVariantType1 { get; set; }
        public virtual TEAssetVariantType TEAssetVariantType2 { get; set; }
    }
}
