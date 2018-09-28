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
    
    public partial class TEBandsAndGrade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEBandsAndGrade()
        {
            this.TEEmpAssignmentDetails = new HashSet<TEEmpAssignmentDetail>();
            this.TEEmpAssignmentDetails1 = new HashSet<TEEmpAssignmentDetail>();
            this.TEEmpAssignmentDetails2 = new HashSet<TEEmpAssignmentDetail>();
        }
    
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> TECompany { get; set; }
        public string BandName { get; set; }
        public string GradeName { get; set; }
        public string Designation { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
    
        public virtual TECompany TECompany1 { get; set; }
        public virtual TECompany TECompany2 { get; set; }
        public virtual TECompany TECompany3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmpAssignmentDetail> TEEmpAssignmentDetails2 { get; set; }
    }
}
