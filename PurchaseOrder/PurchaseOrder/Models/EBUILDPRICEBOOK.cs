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
    
    public partial class EBUILDPRICEBOOK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EBUILDPRICEBOOK()
        {
            this.PRICEPUSHDETAILS = new HashSet<PRICEPUSHDETAIL>();
            this.EBUILDPRICINGs = new HashSet<EBUILDPRICING>();
        }
    
        public int PRICEBOOKID { get; set; }
        public string VERSIONNO { get; set; }
        public int CATEGORYID { get; set; }
        public string CREATEDBY { get; set; }
        public System.DateTime CREATEDDATE { get; set; }
        public string MODIFIEDBY { get; set; }
        public Nullable<System.DateTime> MODIFIEDDATE { get; set; }
        public string APPROVEDBY { get; set; }
        public Nullable<System.DateTime> APPROVEDDATE { get; set; }
        public string status { get; set; }
        public Nullable<int> isDeleted { get; set; }
        public Nullable<int> cityid { get; set; }
        public Nullable<int> productid { get; set; }
        public Nullable<int> projectid { get; set; }
    
        public virtual CATEGORYMASTER CATEGORYMASTER { get; set; }
        public virtual CATEGORYMASTER CATEGORYMASTER1 { get; set; }
        public virtual CITYMASTER CITYMASTER { get; set; }
        public virtual CITYMASTER CITYMASTER1 { get; set; }
        public virtual CITYMASTER CITYMASTER2 { get; set; }
        public virtual CITYMASTER CITYMASTER3 { get; set; }
        public virtual CITYMASTER CITYMASTER4 { get; set; }
        public virtual CITYMASTER CITYMASTER5 { get; set; }
        public virtual CITYMASTER CITYMASTER6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRICEPUSHDETAIL> PRICEPUSHDETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EBUILDPRICING> EBUILDPRICINGs { get; set; }
    }
}
