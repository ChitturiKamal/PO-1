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
    
    public partial class TEAssetLoss
    {
        public int Uniqueid { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public string SubmittedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DateOfLoss { get; set; }
        public string TypeOfLoss { get; set; }
        public string LossRemarks { get; set; }
        public string PoliceComplaintDetails { get; set; }
        public Nullable<decimal> RecoveryAmount { get; set; }
        public Nullable<int> LostFrom { get; set; }
        public Nullable<int> LostFromResponsiblePreson { get; set; }
        public Nullable<int> LostFromLocation { get; set; }
        public Nullable<int> TEAssetID { get; set; }
        public string RejectReason { get; set; }
    
        public virtual TEAsset TEAsset { get; set; }
        public virtual TEAsset TEAsset1 { get; set; }
        public virtual TEAsset TEAsset2 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo1 { get; set; }
        public virtual TEWorkLocation TEWorkLocation { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo2 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo3 { get; set; }
        public virtual TEWorkLocation TEWorkLocation1 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo4 { get; set; }
        public virtual TEEmpBasicInfo TEEmpBasicInfo5 { get; set; }
        public virtual TEWorkLocation TEWorkLocation2 { get; set; }
    }
}