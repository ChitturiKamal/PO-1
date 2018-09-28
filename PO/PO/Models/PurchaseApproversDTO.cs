using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.Models
{
    public class PurchaseApproversDTO
    {
        public List<POMasterApprover> MasterApproverlist { get; set; }
        public POApprovalCondition ApprovalCondition { get; set; }
        public POApprovalConditionDTO POApprovalConditionDTO { get; set; }
    }
    public class fundcenterDTO
    {
        public int uniqueID { get; set; }
        public int FundcenterUniqueID { get; set; }
        //public int? categoryid { get; set; }
        //public int? SUBCATEGORYID { get; set; }
        public string WBSCode { get; set; }
        public int? WbsUniqueID { get; set; }
        public string name { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string FundCentreCode { get; set; }
        public string FundCenter_Description { get; set; }
        public int? LastModifiedBy { get; set; }
        public string LastModifiedBy_Name { get; set; }
        public Nullable<DateTime> LastModifiedOn { get; set; }

    }
    public class CmpStaticFilesClass
    {
        public string url;
        public string name;
        public string id;
    }
    public class MaterialDTO
    {
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public string Level5 { get; set; }
        public string Level6 { get; set; }
        public string Level7 { get; set; }
        public string material_name { get; set; }
        public string MaterialCode { get; set; }
        public CmpStaticFilesClass Image { get; set; }
        public string short_description { get; set; }
        public string shade_number { get; set; }
        public string shade_description { get; set; }
        public string unit_of_measure { get; set; }
        public string hsn_code { get; set; }
        public string sap_code { get; set; }
        public string wbs_code { get; set; }
        public string part_of_edesign { get; set; }
        public string edesign_description { get; set; }
        public string generic { get; set; }
        public string manufactured { get; set; }
        public string can_be_used_as_an_asset { get; set; }
        public string material_status { get; set; }
        public string available_till { get; set; }
        public string weighted_average_purchase_rate { get; set; }
        public string qty_evaluation_method { get; set; }
        public string general_po_terms { get; set; }
        public string special_po_terms { get; set; }
        public string last_purchase_rate { get; set; }
        public string gst_procurement { get; set; }
        public string gst_sales { get; set; }
        public string approved_brands { get; set; }
        public string purchase_rate_threshold { get; set; }
        public string approved_vendors { get; set; }
        public string po_lead_time { get; set; }
        public string delivery_lead_time { get; set; }
        public string min_order_qty { get; set; }
        public string maintain_lot { get; set; }
        public PurchaseTaxDetails PurchaseTaxDetails { get; set; }
        public string ItemType { get; set; }
        public List<BrandDetail> BrandList { get; set; }
        public object MaterialInfo { get; set; }
        public object Mtl_Classific_Info { get; set; }
        public object Mtl_General_Info { get; set; }
        public object Mtl_Purchase_Info { get; set; }
        public object Mtl_Planning_Info { get; set; }
        public object Mtl_Quality_Info { get; set; }
        public object Mtl_Log_Info { get; set; }
        public object Mtl_Specs_Info { get; set; }
        public string annex_CheckListId { get; set; }
        public object Mtl_Definition_Info { get; set; }
    }
    public class ServiceDTO
    {
        public string ServiceLevel1 { get; set; }
        public string ServiceLevel2 { get; set; }
        public string ServiceLevel3 { get; set; }
        public string ServiceLevel4 { get; set; }
        public string ServiceLevel5 { get; set; }
        public string ServiceLevel6 { get; set; }
        public string ServiceLevel7 { get; set; }

        public string ServiceCode { get; set; }
        public string WBSCode { get; set; }
        public string ShortDescription { get; set; }
        public string EdesignDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public string ServiceStatus { get; set; }
        public string SAC { get; set; }
        public string MethodOfMesurement { get; set; }
        public string GeneraalSoTerms { get; set; }
        public string SpecialSOTerms { get; set; }
        public string LastPurchaseRate { get; set; }
        public string weightedAveragePurchaseRate { get; set; }
        public string PurchaseRateThreshold { get; set; }
        public string GSTApplicability { get; set; }

    }
    public class HSNTaxDetails
    {
        public string HSNCode { get; set; }
        public string Country { get; set; }
        public string VendorRegionCode { get; set; }
        public string VendorRegionDesc { get; set; }
        public string DeliveryPlantRegionCode { get; set; }
        public string DeliveryPlantRegionDesc { get; set; }
        public string GSTVendorClassificationCode { get; set; }
        public string GSTVendorClassificationDesc { get; set; }
        public int? VendorGSTApplicabilityCode { get; set; }
        public string VendorGSTApplicabilityDesc { get; set; }
        public string MaterialGSTApplicabilityCode { get; set; }
        public string MaterialGSTApplicabilityDesc { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string TaxCode { get; set; }
        public decimal? IGSTTaxRate { get; set; }
        public decimal? CGSTTaxRate { get; set; }
        public decimal? SGSTTaxRate { get; set; }
    }
    public class PurchaseTaxDetails
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string TaxCode { get; set; }
        public decimal? IGSTTaxRate { get; set; }
        public decimal? CGSTTaxRate { get; set; }
        public decimal? SGSTTaxRate { get; set; }
        public int? CheckDat { get; set; }
    }
    public class PurchaseTaxInput
    {
        public string HSNCode { get; set; }
        public string VendorRegionCode { get; set; }
        public string PlantRegionCode { get; set; }
        public string CountryCode { get; set; }        
        public int? VendorGSTApplicabilityId { get; set; }
        public string MaterialGSTApplicabilityId { get; set; }
        public string OrderType { get; set; }
    }
    public class WbsFundCenterInput
    {
        public int FundCentreID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
    }
    public class WbsCodeDtls
    {
        public int? FundCentreID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public int? wbsID { get; set; }
        public string wbsCode { get; set; }
        public string wbsDescription { get; set; }
        public string ProjectDescription { get; set; }
        public string fundcenterCode { get; set; }
        public string fundcenterDescription { get; set; }
        public string fundcenterOwner { get; set; }
    }
    public class SpecificationDTO
    {
        public string additional_features { get; set; }
        public string Configuration { get; set; }
        public string Depth { get; set; }
        public string finish { get; set; }
        public string GlassSpecification { get; set; }
        public string height { get; set; }
        public string length { get; set; }
        public string Polish_Coating { get; set; }
        public string Project { get; set; }
        public string SheenLevel { get; set; }
        public string Theme { get; set; }
        public string wall_thickness { get; set; }
        public string width_mm { get; set; }

    }

    public class Submitforapprovalreq
    {
        public int POUniqueId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string SubmitterComments { get; set; }
        public int UserId { get; set; }
        //public string shipTo { get; set; }           
    }
    public class ApprovalReq
    {
        public int POUniqueId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string SubmitterComments { get; set; }
        public int UserId { get; set; }
        //public string shipTo { get; set; }           
    }
    public class SearchMaterialGrpResult
    {
        public string GroupName { get; set; }
    }
    public class SearchMaterialGroup
    {
        public string searchQuery { get; set; }
        public int pageNumber { get; set; }
        public string groupname { get; set; }
        public bool details { get; set; }
        public int batchSize { get; set; }
        public List<string> filterDatas { get; set; }

    }
    public class TEPurchaseCountModel
    {
        public int PendingCount { get; set; }
        public int UpcomingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalCount { get; set; }

    }

    public class CopyPurchaseItemStructure
    {
        public int ItemStructureID { get; set; }
        public int LastModifiedById { get; set; }
    }

    public class PurchaseItemStructure
    {
        public int ItemStructureID { get; set; }
        public int? HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string Item_Number { get; set; }
        public string Assignment_Category { get; set; }
        public string Item_Category { get; set; }
        public string Material_Number { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Line_item { get; set; }
        public string Order_Qty { get; set; }
        public decimal? Balance_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string ItemType { get; set; }
        public string Delivery_Date { get; set; }
        public string Net_Price { get; set; }
        public string Material_Group { get; set; }
        public string Plant { get; set; }
        public string Storage_Location { get; set; }
        public string Requirement_Tracking_Number { get; set; }
        public string Requisition_Number { get; set; }
        public string Item_Purchase_Requisition { get; set; }
        public string Returns_Item { get; set; }
        public string Tax_salespurchases_code { get; set; }
        public string Overall_limit { get; set; }
        public string Expected_Value { get; set; }
        public string Actual_Value { get; set; }
        public string No_Limit { get; set; }
        public string Overdelivery_Tolerance { get; set; }
        public string Underdelivery_Tolerance { get; set; }
        public string HSN_Code { get; set; }
        public string Status { get; set; }
        public string WBSElementCode { get; set; }
        public string WBSElementCode2 { get; set; }
        public string InternalOrderNumber { get; set; }
        public string GLAccountNo { get; set; }
        public string Brand { get; set; }
        public decimal? Rate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? IGSTRate { get; set; }
        public decimal? IGSTAmount { get; set; }
        public decimal? CGSTRate { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTRate { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal? GrossAmount { get; set; }
        public int CreatedByID { get; set; }
        public int LastModifiedByID { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<int> ServiceHeaderId { get; set; }
        public Nullable<bool> IsRecordInSAP { get; set; }
        public List<TEPOSpecificationAnnexure> POTechnicalSpecifiactionList { get; set; }
    }
    public class PurchaseItemwise
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string ItemNumberPurchase { get; set; }
        public string ConditionType { get; set; }
        public decimal ConditionRate { get; set; }
        public string VendorCode { get; set; }
    }
    public class PurchaseAssignment
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string ItemNumber { get; set; }
        public string GLAccount { get; set; }
        public string WBSElement { get; set; }
        public string FundCenter { get; set; }
        public string CommitmentItem { get; set; }
        public string NetworkNumberAccountAssignment { get; set; }
    }

    public class TEPurchaseHomeModel
    {
        public string Purchasing_Order_Number { get; set; }
        public string Vendor_Account_Number { get; set; }
        public string VendorName { get; set; }
        public DateTime? Purchasing_Document_Date { get; set; }
        public string Fugue_Purchasing_Order_Number { get; set; }

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
        public List<TEPOApprover> Approvers { get; set; }
        public string ProjectCodes { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public string WbsHeads { get; set; }
        public string SubmitterName { get; set; }
        public string ManagerName { get; set; }
        public string OrderType { get; set; }
        public bool isCurrentApprover { get; set; }
        public bool? IsNewPO { get; set; }        
        public int? CreatedBy { get; set; }
        public int? PurchaseRequestId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool isRevisionAllowed { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }

    public class POPDFModel
    {
        public decimal? POTotalAmount;
        public decimal? POTotalTaxes;
        public decimal? POTotalGrossAmount;
        public double? POTotalPaymentTerms;
        public string WBSCode;

        public List<TEPOServiceHeader> POServiceHeader = new List<TEPOServiceHeader>();

        public PurchaseHeaderStructureDetail PurchaseHeaderStructureDetails = new PurchaseHeaderStructureDetail();

        public List<PurchaseItemStructureDetail> PurchaseItemStructureDetails = new List<PurchaseItemStructureDetail>();

        public List<PurchaseVendorPaymentMileStoneDetail> PurchaseVendorPaymentMileStoneDetails = new List<PurchaseVendorPaymentMileStoneDetail>();

        public List<PurchaseTermsAndConditionsDetail> SpecialTermsAndConditions = new List<PurchaseTermsAndConditionsDetail>();

        public List<POSpecificTCDeatailDTO> SpecificTermsAndConditions = new List<POSpecificTCDeatailDTO>();

        public List<PurchaseTermsAndConditionsDetail> GeneralTermsAndConditions = new List<PurchaseTermsAndConditionsDetail>();

        public List<POAnnexureSpecificatonDTO> AnnexureSpecifications = new List<POAnnexureSpecificatonDTO>();

        public List<POServiceAnnexureSpecificatonDTO> ServiceAnnexureSpecifications = new List<POServiceAnnexureSpecificatonDTO>();
    }

    public class PurchaseHeaderStructureDetail
    {
        public string CompanyName;
        public string CompanyAddress;
        public string CompanyCIN;
        public string CompanyGSTIN;
        public string CompanyLogo;
        public string ProjectOrFnc;
        public string WBSHead;
        public string PurchaseOrderNo;
        public string POManager;
        public DateTime? PODate;
        public string Revisioin;
        public string VendorName;
        public string VendorAddress;
        public string VendorCode;
        public string VendorCIN;
        public string VendorGSTIN;
        public string VendorCurrency;
        public string CompanyCode;
        public string POTitle;
        public string PODescripton;
        public string ShipTo;
        public string ShipFrom;
        public string POStatus;
        public string FundcenterName;
        //public TEPOPlantStorageDetail ShipTo { get; set; }       
        //public TEPOVendorShippingDetail ShipFrom { get; set; }
        //public TEPOPlantStorageDetail BillTo { get; set; }
        //public TEPOVendor BilledBy { get; set; }
        public string VendorBillingCity;
        public string VendorBillingPostalCode;
        public string VendorBillingState;
        public string VendorBillingCountry;
        public int? ShipToID;
    }

    public class PurchaseItemStructureDetail
    {
        public int ItemUniqueId;
        public string ItemNo;
        public string POItemName;
        public string Quantity;
        public string Unit;
        public decimal? Rate;
        public decimal? Amount;
        public decimal? IGSTRate;
        public decimal? SGSTRate;
        public decimal? CGSTRate;
        public decimal? TaxAmount;
        public decimal? GrossAmount;
        public string ItemType;
        public string WBSElementCode2;
        public Nullable<int> FugueItemSeqNo;
        public string SAPTransactionCode;
        public Nullable<bool> IsRecordInSAP;
        public Nullable<int> SAPItemSeqNo;
        public Nullable<bool> IsCLRateUpdated;
        public Nullable<int> PRRef;
        public string WBSCode;
        public string BrandName;
        public String Assignment_Category;
        public String Item_Category;
        public string ManufactureCode;
        public string POItemShortText;
        public string LINEITEMNUMBER;
        public string POItemLongText;
        public string POItemLineItem;
        public int? POHeaderStructureid;
        public List<TEPOSpecificationAnnexure> POTechnicalSpecifiactionList { get; set; }
    }
    public class PurchaseServiceBreakUp
    {
        public int ItemUniqueId;
        public string ItemNo;
        public string POItemShortText;
        public string POItemLongText;
        public string Quantity;
        public string Unit;
        public string Rate;
        public string Amount;
    }

    public class PurchaseVendorPaymentMileStoneDetail
    {
        public int MileStoneID;
        public string PaymentTerm;
        public DateTime? PaymentDate;
        public double? Percentage;
        public double? Amount;
    }

    public class PurchaseTermsAndConditionsDetail
    {
        public int SequenceId;
        public string Title;
        public string Condition;
        public int MasterTandCId;
    }
    public class BrandDetail
    {
        public int SequenceId;
        public string BrandSeries;
        public string BrandCode;
        public string ManufactureName;
        public string ManufactureCode;
    }

    public class PRDetails
    {
        public int PurchaseRequestId { get; set; }
        public int FundCenterId { get; set; }
        public string PurchaseRequestTitle { get; set; }
        public string PurchaseRequestDesc { get; set; }
        public List<PRItemStructureList> PurchaseItemList { get; set; }
    }
    public class PRItemStructureList
    {
        public int ItemUniqueid { get; set; }
        public int PurchaseRequestId { get; set; }
        public string Item_Number { get; set; }
        public string Material_Number { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
        public string HSNCode { get; set; }
        public string Brand { get; set; }
        public string ItemType { get; set; }
        public string WBSElementCode { get; set; }
        public string GLAccountNo { get; set; }
        public string HSN_Code { get; set; }
        public int serviceHeaderid { get; set; }

    }
    public class POSpecificTCDeatailDTO
    {
        public int SpecificTCTitleMasterId { get; set; }
        public string Title { get; set; }
        public List<POSpecificTCSubTitleDTO> SpecSubTitlesList { get; set; }
    }
    public class POSpecificTCSubTitleDTO
    {
        public int SpecificTCSubTitleMasterId { get; set; }
        public int SpecificTCTitleMasterId { get; set; }
        public string SubTitleDesc { get; set; }
        public List<TEPOSpecificTCDetail> SpecificTCList { get; set; }
    }
    public class SpecificTCDeatailDTO
    {
        public int SpecificTCTitleMasterId { get; set; }
        public string Title { get; set; }
        public List<SpecificTCDTO> SpecSubTitlesList { get; set; }
    }
    public class SpecificTCDTO
    {
        public int POHeaderStructureId { get; set; }
        public int? SpecificTCSubTitleMasterId { get; set; }
        public int? SpecificTCTitleMasterId { get; set; }
        public string SubTitleDesc { get; set; }
        public int SpecificTCId { get; set; }      
        public string Description { get; set; }
    }
    public class SpecificationSheetMain
    {
        public SpecificationSheetEntries content;
        public string title { get; set; }
        public string checkListId { get; set; }
        public string id { get; set; }
        public string template { get; set; }
    }
    public class SpecificationSheetEntries
    {
        public List<SpecificationSheetCells> entries { get; set; }
    }
    public class SpecificationSheetCells
    {
        public List<SpecificationCellValue> cells { get; set; }
    }
    public class SpecificationCellValue
    {
        public string value { get; set; }
    }
    public class POSpecificationSheet
    {
        public string TestObject { get; set; }
        public string TestMethod { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string Mandatory { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string MTCValue { get; set; }
        public string TestValue { get; set; }
        public string SourceOfTestValue { get; set; }
        public string PassOrFail { get; set; }
    }
    public class POAnnexureSpecificatonDTO
    {
        public int POItemStructureId { get; set; }
        public int POHeaderStructureId { get; set; }
        public int POAnnexureId { get; set; }
        public string Title { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialGroup { get; set; }
        public List<TEPOAnnexureSpecification> SpecsData { get; set; }
    }
    public class POServiceAnnexureDTO
    {
        public int POItemStructureId { get; set; }
        public int POHeaderStructureId { get; set; }
        public int POServiceAnnexureId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class POServiceAnnexureSpecificatonDTO
    {
        public int POItemStructureId { get; set; }
        public int POHeaderStructureId { get; set; }        
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceGroup { get; set; }
        public List<TEPOServiceAnnexure> SpecsData { get; set; }
    }
    public class SearchMaterialRate
    {
        public string materialId { get; set; }
        public string location { get; set; }
        public string currency { get; set; }
        public string onDate { get; set; }
        public string TransactionType { get; set; }
        public string NatureofTransaction { get; set; }
    }
    public class MaterailBaseRateInfo
    {
        public BaseLandedRate controlBaseRate;
        public BaseLandedRate landedRate;
        public string procurement_rate_threshold { get; set; }
        public string unitOfMeasure { get; set; }       
    }
    public class BaseLandedRate
    {       
        public string value { get; set; }
        public string currency { get; set; }
    }
    public class FinalItemBaseRate
    {
        public decimal ControlBaserate { get; set; }
        public decimal ThresholdValue { get; set; }
    }
    public class TEPOSpecificTCDetailDTO
    {
        public int SpecificTCId { get; set; }
        public int SpecificTCSubTitleMasterId { get; set; }
        public int POHeaderStructureId { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }       
    }
    public class POApprovalConditionDTO
    {
        public string LastModifiedBy { get; set; }
        public int UniqueId { get; set; }
        public Nullable<int> FundCenter { get; set; }
        public string FundCenterCode { get; set; }
        public string FundCenterDescription { get; set; }
        public Nullable<int> PurchasingGroup { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<double> MinAmount { get; set; }
        public Nullable<double> MaxAmount { get; set; }
        public Nullable<int> POManagerID { get; set; }
    }
    public class SAPResponse
    {
        public string ReturnCode { get; set; }
        public string PONumber { get; set; }
        public string Message { get; set; }
    }
}