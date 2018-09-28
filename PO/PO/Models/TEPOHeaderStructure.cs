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
    
    public partial class TEPOHeaderStructure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEPOHeaderStructure()
        {
            this.TEPOSpecificTCDetails = new HashSet<TEPOSpecificTCDetail>();
        }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string Purchasing_Order_Number { get; set; }
        public string Fugue_Purchasing_Order_Number { get; set; }
        public string Purchasing_Document_Type { get; set; }
        public string Vendor_Account_Number { get; set; }
        public Nullable<System.DateTime> Purchasing_Document_Date { get; set; }
        public string Purchasing_Organization { get; set; }
        public string Purchasing_Group { get; set; }
        public string Company_Code { get; set; }
        public string Payment_Key { get; set; }
        public string Currency_Key { get; set; }
        public string Exchange_Rate { get; set; }
        public string Managed_by { get; set; }
        public string You_Reference { get; set; }
        public string Telephone { get; set; }
        public string Our_Reference { get; set; }
        public string PO_Title { get; set; }
        public string Agreement_signed_date { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string Reason_change { get; set; }
        public string Requested_By { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ReleaseGroup { get; set; }
        public string ReleaseStrategy { get; set; }
        public string ReleaseCode1 { get; set; }
        public string ReleaseCode2 { get; set; }
        public string ReleaseCode3 { get; set; }
        public string ReleaseCode4 { get; set; }
        public string ReleaseStatus { get; set; }
        public string VersionTextField { get; set; }
        public string path { get; set; }
        public string Statusversion { get; set; }
        public string ReleaseCode1By { get; set; }
        public string ReleaseCode1Status { get; set; }
        public Nullable<System.DateTime> ReleaseCode1Date { get; set; }
        public string ReleaseCode2By { get; set; }
        public string ReleaseCode2Status { get; set; }
        public Nullable<System.DateTime> ReleaseCode2Date { get; set; }
        public string ReleaseCode3By { get; set; }
        public string ReleaseCode3Status { get; set; }
        public Nullable<System.DateTime> ReleaseCode3Date { get; set; }
        public string ReleaseCode4By { get; set; }
        public string ReleaseCode4Status { get; set; }
        public Nullable<System.DateTime> ReleaseCode4Date { get; set; }
        public string ReleaseCodes { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmailID { get; set; }
        public Nullable<int> SubmittedBy { get; set; }
        public string SubmitterComments { get; set; }
        public string PartnerFunction1 { get; set; }
        public string PartnerFunction2 { get; set; }
        public string PartnerFunctionVendorCode1 { get; set; }
        public string PartnerFunctionVendorCode2 { get; set; }
        public string ShipTpCode { get; set; }
        public Nullable<int> FundCenterID { get; set; }
        public Nullable<int> VendorID { get; set; }
        public Nullable<int> ShippedFromID { get; set; }
        public Nullable<int> ShippedToID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> BilledByID { get; set; }
        public Nullable<int> BilledToID { get; set; }
        public string PODescription { get; set; }
        public Nullable<int> PO_OrderTypeID { get; set; }
        public Nullable<bool> IsNewPO { get; set; }
        public Nullable<int> POManagerID { get; set; }
        public Nullable<bool> IsPRPO { get; set; }
        public Nullable<int> PurchaseRequestId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPOSpecificTCDetail> TEPOSpecificTCDetails { get; set; }
    }
}
