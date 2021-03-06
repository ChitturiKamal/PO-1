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
    
    public partial class TEPickListItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEPickListItem()
        {
            this.TELeaveTypes = new HashSet<TELeaveType>();
            this.TETaxDetails = new HashSet<TETaxDetail>();
            this.TETaxDetails1 = new HashSet<TETaxDetail>();
        }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TEPickList { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public string type { get; set; }
        public string colorcode { get; set; }
        public Nullable<int> Parentid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeaveType> TELeaveTypes { get; set; }
        public virtual TEPickList TEPickList1 { get; set; }
        public virtual TEPickList TEPickList2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETaxDetail> TETaxDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETaxDetail> TETaxDetails1 { get; set; }
    }
}
