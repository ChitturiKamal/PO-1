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
    
    public partial class TEProjectPremium
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEProjectPremium()
        {
            this.TEProjectUnitPremiums = new HashSet<TEProjectUnitPremium>();
        }
    
        public int ProjectPremiumID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> PremiumID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
        public Nullable<decimal> PremiumValue { get; set; }
        public string Quantity { get; set; }
        public string RateType { get; set; }
    
        public virtual TEPremiumMaster TEPremiumMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEProjectUnitPremium> TEProjectUnitPremiums { get; set; }
    }
}
