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
    
    public partial class SUBCATEGORYMASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBCATEGORYMASTER()
        {
            this.EBUILDPRICINGs = new HashSet<EBUILDPRICING>();
            this.RATEMASTERPRICINGs = new HashSet<RATEMASTERPRICING>();
            this.RATEMASTERPRICINGs1 = new HashSet<RATEMASTERPRICING>();
            this.WBSMASTERs = new HashSet<WBSMASTER>();
            this.WBSMASTERs1 = new HashSet<WBSMASTER>();
            this.WBSMASTERs2 = new HashSet<WBSMASTER>();
        }
    
        public int SUBCATEGORYID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int CATEGORYID { get; set; }
    
        public virtual CATEGORYMASTER CATEGORYMASTER { get; set; }
        public virtual CATEGORYMASTER CATEGORYMASTER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBUILDPRICING> EBUILDPRICINGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATEMASTERPRICING> RATEMASTERPRICINGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATEMASTERPRICING> RATEMASTERPRICINGs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WBSMASTER> WBSMASTERs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WBSMASTER> WBSMASTERs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WBSMASTER> WBSMASTERs2 { get; set; }
    }
}
