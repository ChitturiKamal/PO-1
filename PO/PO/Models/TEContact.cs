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
    
    public partial class TEContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEContact()
        {
            this.TEContactFamilyRelations = new HashSet<TEContactFamilyRelation>();
            this.TEContactGPAs = new HashSet<TEContactGPA>();
            this.TEContactNotes = new HashSet<TEContactNote>();
            this.TELeads = new HashSet<TELead>();
            this.TENotes = new HashSet<TENote>();
            this.TETasks = new HashSet<TETask>();
            this.TEContactAlumnis = new HashSet<TEContactAlumni>();
            this.TEContactGPAs1 = new HashSet<TEContactGPA>();
            this.TEContactIdentifications = new HashSet<TEContactIdentification>();
            this.TEContactInterests = new HashSet<TEContactInterest>();
            this.TEContactProfessions = new HashSet<TEContactProfession>();
            this.TEContactFamilyRelations1 = new HashSet<TEContactFamilyRelation>();
            this.TEContactSocials = new HashSet<TEContactSocial>();
        }
    
        public int Uniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> Towerid { get; set; }
        public string Unitid { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<int> Projectid { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Mobile { get; set; }
        public string Emailid { get; set; }
        public string Type { get; set; }
        public string PrefferedContact { get; set; }
        public string PrefferedMode { get; set; }
        public Nullable<bool> ISProfilesPublic { get; set; }
        public Nullable<bool> IsAdvertise { get; set; }
        public string Photo { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CallName { get; set; }
        public string ContactType { get; set; }
        public string Nationality { get; set; }
        public string CountryOfBirth { get; set; }
        public Nullable<decimal> Age { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Category { get; set; }
        public string Importance { get; set; }
        public string PrefferedSaleConsultant { get; set; }
        public string CountryCode { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ResidentStatus { get; set; }
        public Nullable<int> Value { get; set; }
        public string Organisation { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> DOM { get; set; }
        public string MiddleName { get; set; }
        public Nullable<int> TEOrganisation { get; set; }
        public string Contactid { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public Nullable<int> CreatedBy_Id { get; set; }
        public Nullable<bool> IsVip { get; set; }
        public Nullable<bool> IsFriendofTE { get; set; }
        public Nullable<bool> IsBlackList { get; set; }
        public string MotherTongue { get; set; }
        public Nullable<bool> IsExistingCustomer { get; set; }
        public string OldUniqueid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactNote> TEContactNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELead> TELeads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TENote> TENotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETask> TETasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactAlumni> TEContactAlumnis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactIdentification> TEContactIdentifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactInterest> TEContactInterests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactProfession> TEContactProfessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactSocial> TEContactSocials { get; set; }
    }
}
