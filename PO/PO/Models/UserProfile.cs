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
    
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            this.TEApproveGroupUsers = new HashSet<TEApproveGroupUser>();
            this.TEDcoumentCatMasters = new HashSet<TEDcoumentCatMaster>();
            this.TENotes = new HashSet<TENote>();
            this.TEPOFundCenterPOMgrMappings = new HashSet<TEPOFundCenterPOMgrMapping>();
            this.TEPOFundCenterUserMappings = new HashSet<TEPOFundCenterUserMapping>();
            this.TESalesPlanandTargets = new HashSet<TESalesPlanandTarget>();
            this.TESalesPlanandTargets1 = new HashSet<TESalesPlanandTarget>();
            this.TETargetAllocations = new HashSet<TETargetAllocation>();
            this.TETargetAllocations1 = new HashSet<TETargetAllocation>();
            this.TEUserRoleThresholdMappings = new HashSet<TEUserRoleThresholdMapping>();
            this.TEUsersRoles = new HashSet<TEUsersRole>();
            this.TEUsersThresholds = new HashSet<TEUsersThreshold>();
        }
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }
        public string Phone { get; set; }
        public string House_no { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Area { get; set; }
        public string city { get; set; }
        public Nullable<int> pincode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Nullable<bool> Status { get; set; }
        public string AndroidToken { get; set; }
        public string IosToken { get; set; }
        public string Password { get; set; }
        public string facebookid { get; set; }
        public string googleid { get; set; }
        public string CallName { get; set; }
        public string photo { get; set; }
        public Nullable<int> roleid { get; set; }
        public string BizAppID { get; set; }
        public string CallCentreID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string AuthKey { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEApproveGroupUser> TEApproveGroupUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEDcoumentCatMaster> TEDcoumentCatMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TENote> TENotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPOFundCenterPOMgrMapping> TEPOFundCenterPOMgrMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPOFundCenterUserMapping> TEPOFundCenterUserMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESalesPlanandTarget> TESalesPlanandTargets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TESalesPlanandTarget> TESalesPlanandTargets1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETargetAllocation> TETargetAllocations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TETargetAllocation> TETargetAllocations1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEUserRoleThresholdMapping> TEUserRoleThresholdMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEUsersRole> TEUsersRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEUsersThreshold> TEUsersThresholds { get; set; }
        public virtual UserProfile UserProfile1 { get; set; }
        public virtual UserProfile UserProfile2 { get; set; }
    }
}
