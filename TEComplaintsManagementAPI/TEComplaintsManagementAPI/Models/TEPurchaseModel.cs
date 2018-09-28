using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
   
    public class TEPurchaseModel
    {
        //Customization
       
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public string Purchasing_Order_Number { get; set; }
        public string Vendor_Account_Number { get; set; }
        public string Purchasing_Document_Date { get; set; }
        public string ReleaseGroup { get; set; }
        public string ReleaseStrategy { get; set; }
        public string ReleaseCode { get; set; }
        public string ReleaseCodeStatus { get; set; }
        public string fund_center { get; set; }
        public string FundCenter_Description { get; set; }
        public string FundCenter_Owner { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total{ get; set; }
        public string Release2CodeBy { get; set; }
        public string Release2Code { get; set; }
        public string ReleaseCode2Date { get; set; }        
        public string ReleaseCode2Status { get; set; }
        public string Release3CodeBy { get; set; }
        public string Release3Code { get; set; }
        public string ReleaseCode3Date { get; set; }
        public string ReleaseCode3Status { get; set; }
        public string Release4CodeBy { get; set; }
        public string Release4Code { get; set; }
        public string ReleaseCode4Date { get; set; }
        public string ReleaseCode4Status { get; set; }

        public string Material_Number { get; set; }
        public string Item_Category { get; set; }
        public int short_text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public decimal Net_Price { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal ItemTax { get; set; }
        public decimal itemTotal { get; set; }

       
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
       
    }
    public class TEPurchaseHomeModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Vendor_Account_Number { get; set; }
        public string VendorName { get; set; }
        public string Purchasing_Document_Date { get; set; }
      
        //public string fund_center { get; set; }
        public string FundCenter_Description { get; set; }
        public string Path { get; set; }
        //public string FundCenter_Owner { get; set; }
        public double? Amount { get; set; }
        //public decimal Tax { get; set; }
        //public decimal Total { get; set; }
        //public string ReleaseCode { get; set; }
        //public string ReleaseCodeBy { get; set; }
        public DateTime? Purchasing_Release_Date { get; set; }
        public string ReleaseCodeStatus { get; set; }
        public string POStatus { get; set; }

        //public string Release2CodeBy { get; set; }
        //public string Release2Code { get; set; }
        //public string ReleaseCode2Date { get; set; }
        //public string ReleaseCode2Status { get; set; }
        //public string Release3CodeBy { get; set; }
        //public string Release3Code { get; set; }
        //public string ReleaseCode3Date { get; set; }
        //public string ReleaseCode3Status { get; set; }
        //public string Release4CodeBy { get; set; }
        //public string Release4Code { get; set; }
        //public string ReleaseCode4Date { get; set; }
        //public string ReleaseCode4Status { get; set; }
        public string Currency_Key { get; set; }
      
        //public int ApproverUniqueId { get; set; }
        //public int SequenceNumber { get; set; }
        //public string ReleaseCode { get; set; }
        public int HeaderUniqueid { get; set; }
        public int PoCount { get; set; }
        public string PoTitle { get; set; }
        public List<TEPurchaseOrderApprover> Approvers { get; set; }
        public string ProjectCodes { get; set; }
        public string WbsHeads { get; set; }
        public string SubmitterName { get; set; }
        public string ManagerName { get; set; }
    }
    public class TEPurchaseItemModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Vendor_Account_Number { get; set; }
        public string Purchasing_Document_Date { get; set; }
       

        public DateTime? Purchasing_Release_Date { get; set; }
        public string ReleaseCodeStatus { get; set; }
        public string Path { get; set; }
        public string Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string ReleaseCode2By { get; set; }
        public int ReleaseCode2ByUserid { get; set; }
        public string ReleaseCode2 { get; set; }
        public DateTime? ReleaseCode2Date { get; set; }
        public string ReleaseCode2Status { get; set; }
        public string ReleaseCode3By { get; set; }
        public int ReleaseCode3ByUserid { get; set; }
        public string ReleaseCode3 { get; set; }
        public DateTime? ReleaseCode3Date { get; set; }
        public string ReleaseCode3Status { get; set; }
        public string ReleaseCode4By { get; set; }
        public int ReleaseCode4ByUserid { get; set; }
        public string ReleaseCode4 { get; set; }
        public DateTime? ReleaseCode4Date { get; set; }
        public string ReleaseCode4Status { get; set; }
         public string Currency_Key  { get; set; }
         public string OfficialEmail { get; set; }
         public string DocumentSubject { get; set; }
         public string DocumentBody { get; set; }
         public string SubmitterName { get; set; }
         public string AmountExclTax { get; set; }
         public string OtherCharges { get; set; }
         public string ItemTax { get; set; }


         public string CompanyName { get; set; }
         public string CompanyAddress { get; set; }
         
         public string FundCenterDescription { get; set; }
         public string WbsHeads { get; set; }
         public string OrderType { get; set; }
         public string RevisionNumber { get; set; }
         public string POManager { get; set; }
         public string PODate { get; set; }
         public string VendorName { get; set; }
         public string VendorAddress { get; set; }
         public string VendorPanNumber { get; set; }
         public string VendorServiceTax { get; set; }
         public string VendorVat { get; set; }
         public string FundCenterCode { get; set; }

         public string SubmitterComments { get; set; }
         public string POTitle { get; set; }

         public string ManagerName { get; set; }

         public string VenderGSTCode { get; set; }
         public string VenderRegionCode { get; set; }
         public string VenderRegionCodeDesc { get; set; }
         public string VenderCountry { get; set; }
         public string BillFromGSTCode { get; set; }
         public string BillFromAddress { get; set; }
         public string BillFromName { get; set; }
         public string BillFromCountry { get; set; }
         public string ShipFromGSTCode { get; set; }
         public string ShipFromAddress { get; set; }
         public string ShipFromName { get; set; }
         public string ShipFromCountry { get; set; }
         public string ShipToGSTCode { get; set; }
         public string ShipToAddress { get; set; }
         public string ShipToName { get; set; }
         public string ShipToCountry { get; set; }
         public string PlantCode { get; set; }
         public string PlantRegionCode { get; set; }
         public string PlantGSTIN { get; set; }
         public string PlantAddress { get; set; }
         public string PlantCountry{ get; set; }
         public string PlantName { get; set; }
         public string ShipToCode { get; set; }
         public string ShipFromCode { get; set; }
         public bool IsNewDocument { get; set; }
    }
    public class TEPurchaseItemlistModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Material_Number { get; set; }
        public string Item_Category { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string Net_Price { get; set; }
        public string ItemAmount { get; set; }
        public string ItemTax { get; set; }
        public string itemTotal { get; set; }
        public string WBS_Element { get; set; }
        public string Commitment_item { get; set; }
        public string ItemNumber { get; set; }
        public string FundCenter { get; set; }
        public string OtherCharges { get; set; }
        public string Amount { get; set; }
        public string TaxCode { get; set; }
        public string HSNCode { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string Plant { get; set; }
        
    }
    public class TEPurchaseItemlistDetailsModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Activity_Number { get; set; }
       // public string Item_Category { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string Net_Price { get; set; }
        public string Gross_Price { get; set; }
        public decimal ItemAmount { get; set; }
        //public decimal ItemTax { get; set; }
        public double? itemTotal { get; set; }
        public string WBS_Element { get; set; }
        public string Commitment_item { get; set; }
        public string ItemNumber { get; set; }
        public double? ItemTax { get; set; }
        public string FundCenter { get; set; }
        public string SACCode { get; set; }
        public string PlantCode { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
    }

    public class TEPurchaseUpdateModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Fugue_Purchasing_Order_Number { get; set; }
        public string ReleaseCodeBy { get; set; }
        public string ReleaseCode { get; set; }
        public DateTime? ReleaseCodeDate { get; set; }
        public string ReleaseCodeStatus { get; set; }   
    }

    public class TEPurchasetermsModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string LineNo { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }


    }

    public class TEPurchaseAproveModel
    {
        public string UserId { get; set; }
        public string Purchasing_Order_Number { get; set; }
        public string Fugue_Purchasing_Order_Number { get; set; }
        public string AproverUniqueid { get; set; }
        public string AproverName { get; set; }
        public string SequenceId { get; set; }
        public string ReleaseCode { get; set; }
        public string ReleaseCodeStatus { get; set; }
        public int POUniqueId { get; set; }
        public string Comments { get; set; }
    }

    public class TEPurchaseCountModel
    {
        public int PendingCount { get; set; }
        public int UpcomingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalCount { get; set; }

    }

    public class TEPurchaseFinalizeModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string FundCenter { get; set; }
        public string FundCenterDescription { get; set; }
        public string WbsHeads { get; set; }
        public string OrderType { get; set; }
        public int RevisionNumber { get; set; }
        public string POManager { get; set; }
        public DateTime? PODate { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorPanNumber { get; set; }
        public string VendorServiceTax { get; set; }
        public string VendorVat { get; set; }

    }
    public class TEMasterTerm
    {
        public string Condition { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string ModuleName { get; set; }
        public string ObjectId { get; set; }
        public string OldUniqueId { get; set; }
        public int SequenceId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int UniqueId { get; set; }
        public string TypeDesc { get; set; }
    }
    public class TEPOTerm
    {
        public string Condition { get; set; }
        public string ContextIdentifier { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string MasterId { get; set; }
        public string ModuleName { get; set; }
        public string ObjectId { get; set; }
        public string OldUniqueId { get; set; }
        public int SequenceId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int UniqueId { get; set; }
        public string TypeDesc { get; set; }
    }

    public class TEPurchaseItemListAnnexureModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Material_Number { get; set; }
        public string Item_Category { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string Net_Price { get; set; }
        public string ItemAmount { get; set; }
        public string ItemTax { get; set; }
        public string itemTotal { get; set; }
        public string WBS_Element { get; set; }
        public string Commitment_item { get; set; }
        public string ItemNumber { get; set; }
        public string FundCenter { get; set; }
        public string OtherCharges { get; set; }
        public string Amount { get; set; }
        public string TaxCode { get; set; }
        public string HSNCode { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string Plant { get; set; }

        public string PackingForwardingCondition { get; set; }
        public string PackingForwardingValue { get; set; }
        public string FreightCondition { get; set; }
        public string FreightValue { get; set; }
        public string OtherServicesType { get; set; }
        public string OtherServicesCondition { get; set; }
        public string OtherServicesValue { get; set; }
        public string ConditionTypeTotal { get; set; }
    }

    public class TEPurchaseItemlistDetailsAnnexureModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Activity_Number { get; set; }
        // public string Item_Category { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string Net_Price { get; set; }
        public string Gross_Price { get; set; }
        public decimal ItemAmount { get; set; }
        //public decimal ItemTax { get; set; }
        public double? itemTotal { get; set; }
        public string WBS_Element { get; set; }
        public string Commitment_item { get; set; }
        public string ItemNumber { get; set; }
        public double? ItemTax { get; set; }
        public string FundCenter { get; set; }
        public string SACCode { get; set; }
        public string PlantCode { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }

        public string PackingForwardingCondition { get; set; }
        public string PackingForwardingValue { get; set; }
        public string FreightCondition { get; set; }
        public string FreightValue { get; set; }
        public string OtherServicesType { get; set; }
        public string OtherServicesCondition { get; set; }
        public string OtherServicesValue { get; set; }
    }

    public class Submitforapprovalreq
    {
        public int POUniqueId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string SubmitterComments { get; set; }
        public int UserId { get; set; }
        public string shipTo { get; set; }
        public List<TEPurchaseItemListAnnexureModel> annexureModel { get; set; }
    }
}