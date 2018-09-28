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
    
    public partial class TEContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEContact()
        {
            this.R_TEContactOrganisationRepresentatives = new HashSet<R_TEContactOrganisationRepresentatives>();
            this.R_TERelationshipsOfContact = new HashSet<R_TERelationshipsOfContact>();
            this.TEContactNotes = new HashSet<TEContactNote>();
            this.TEContactNotes1 = new HashSet<TEContactNote>();
            this.TEContactFamilyRelations = new HashSet<TEContactFamilyRelation>();
            this.TEContactGPAs = new HashSet<TEContactGPA>();
            this.TEContactFamilyRelations1 = new HashSet<TEContactFamilyRelation>();
            this.TEContactGPAs1 = new HashSet<TEContactGPA>();
            this.TEContactNotes2 = new HashSet<TEContactNote>();
            this.TEContactFamilyRelations2 = new HashSet<TEContactFamilyRelation>();
            this.TEContactGPAs2 = new HashSet<TEContactGPA>();
            this.TELeads = new HashSet<TELead>();
            this.TELeadActivityLogs = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs1 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs2 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs3 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs4 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs5 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs6 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs7 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs8 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs9 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs10 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs11 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs12 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs13 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs14 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs15 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs16 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs17 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs18 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs19 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs20 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs21 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs22 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs23 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs24 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs25 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs26 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs27 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs28 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs29 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs30 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs31 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs32 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs33 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs34 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs35 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs36 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs37 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs38 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs39 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs40 = new HashSet<TELeadActivityLog>();
            this.TELeadActivityLogs41 = new HashSet<TELeadActivityLog>();
            this.TENotes = new HashSet<TENote>();
            this.TETasks = new HashSet<TETask>();
            this.TEContactAddresses = new HashSet<TEContactAddress>();
            this.TEContactAlumnis = new HashSet<TEContactAlumni>();
            this.TEContactApprovers = new HashSet<TEContactApprover>();
            this.TEContactEducations = new HashSet<TEContactEducation>();
            this.TEContactEmails = new HashSet<TEContactEmail>();
            this.TEContactGPAs3 = new HashSet<TEContactGPA>();
            this.TEContactIdentifications = new HashSet<TEContactIdentification>();
            this.TEContactImportReports = new HashSet<TEContactImportReport>();
            this.TEContactInterests = new HashSet<TEContactInterest>();
            this.TEContactMobiles = new HashSet<TEContactMobile>();
            this.TEContactProfessions = new HashSet<TEContactProfession>();
            this.TEContactFamilyRelations3 = new HashSet<TEContactFamilyRelation>();
            this.TEContactSelfUpdationTrackings = new HashSet<TEContactSelfUpdationTracking>();
            this.TEContactSocials = new HashSet<TEContactSocial>();
            this.TEWmCGuestVisitsHistories = new HashSet<TEWmCGuestVisitsHistory>();
        }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string Contactid { get; set; }
        public Nullable<int> Projectid { get; set; }
        public Nullable<int> Towerid { get; set; }
        public string Unitid { get; set; }
        public string Mobile { get; set; }
        public string Emailid { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> DOM { get; set; }
        public string Type { get; set; }
        public string PrefferedContact { get; set; }
        public string PrefferedMode { get; set; }
        public bool ISProfileSPublic { get; set; }
        public bool IsAdvertise { get; set; }
        public string Photo { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Status { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
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
        public Nullable<int> TEOrganisation { get; set; }
        public string CountryCode { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ResidentStatus { get; set; }
        public Nullable<int> Value { get; set; }
        public string Organisation { get; set; }
        public string MotherTongue { get; set; }
        public bool IsVip { get; set; }
        public bool IsFriendofTE { get; set; }
        public bool IsBlackList { get; set; }
        public string PrefferedWaiter { get; set; }
        public string Loyalty { get; set; }
        public bool Cleaned { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public Nullable<int> CreatedBy_Id { get; set; }
        public Nullable<bool> IsExistingCustomer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_TEContactOrganisationRepresentatives> R_TEContactOrganisationRepresentatives { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_TERelationshipsOfContact> R_TERelationshipsOfContact { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactNote> TEContactNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactNote> TEContactNotes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactNote> TEContactNotes2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELead> TELeads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs8 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs9 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs10 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs11 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs12 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs13 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs14 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs15 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs16 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs17 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs18 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs19 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs20 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs21 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs22 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs23 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs24 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs25 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs26 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs27 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs28 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs29 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs30 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs31 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs32 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs33 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs34 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs35 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs36 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs37 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs38 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs39 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs40 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TELeadActivityLog> TELeadActivityLogs41 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TENote> TENotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETask> TETasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactAddress> TEContactAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactAlumni> TEContactAlumnis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactApprover> TEContactApprovers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactEducation> TEContactEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactEmail> TEContactEmails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactGPA> TEContactGPAs3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactIdentification> TEContactIdentifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactImportReport> TEContactImportReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactInterest> TEContactInterests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactMobile> TEContactMobiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactProfession> TEContactProfessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactFamilyRelation> TEContactFamilyRelations3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactSelfUpdationTracking> TEContactSelfUpdationTrackings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEContactSocial> TEContactSocials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEWmCGuestVisitsHistory> TEWmCGuestVisitsHistories { get; set; }
    }
}
