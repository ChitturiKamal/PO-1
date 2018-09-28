using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurchaseOrder.Models
{
    public class SuccessInfo
    {
        public int listcount { get; set; }
        public int errorcode { get; set; }
        public string errormessage { get; set; }
        public int fromrecords { get; set; }
        public int torecords { get; set; }
        public int totalrecords { get; set; }
        public string pages { get; set; }
    }
    public class PurchaseHeaderStructure
    {

        //public int UniqueId { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string FuguePurchasingOrderNumber { get; set; }
        public string PurchasingDocumentType { get; set; }
        public string VendorAccountNumber { get; set; }
        public string PurchasingDocumentDate { get; set; }
        public string PurchasingOrganization { get; set; }
        public string PurchasingGroup { get; set; }
        public string CompanyCode { get; set; }
        public string PaymentKey { get; set; }
        public string CurrencyKey { get; set; }
        public string ExchangeRate { get; set; }
        public string ManagedBy { get; set; }
        public string YouReference { get; set; }
        public string Telephone { get; set; }
        public string OurReference { get; set; }
        public string POTitle { get; set; }
        public string AgreementSignedDate { get; set; }
        public string Version { get; set; }
        public string ReasonChange { get; set; }
        public string RequestedBy { get; set; }
        public string ReleaseGroup { get; set; }
        public string ReleaseStrategy { get; set; }
        public string ReleaseCode1 { get; set; }
        public string ReleaseCode2 { get; set; }
        public string ReleaseCode3 { get; set; }
        public string ReleaseCode4 { get; set; }
        public string ReleaseStatus { get; set; }
        public string VersionTextField { get; set; }
        public string Path { get; set; }
        public string Statusversion { get; set; }
        public string ReleaseCodes { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterEmailID { get; set; }
        public string PartnerFunction1 { get; set; }
        public string PartnerFunction2 { get; set; }
        public string PartnerFunctionVendorCode1 { get; set; }
        public string PartnerFunctionVendorCode2 { get; set; }
    }

    public class PurchaseItemStructure
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string Item_Number { get; set; }
        public string Assignment_Category { get; set; }
        public string Item_Category { get; set; }
        public string Material_Number { get; set; }
        public string Short_Text { get; set; }
        public string Long_Text { get; set; }
        public string Line_item { get; set; }
        public string Order_Qty { get; set; }
        public string Unit_Measure { get; set; }
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

    public class PurchaseItemwise
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string ItemNumberPurchase { get; set; }
        public string ConditionType { get; set; }
        public decimal ConditionRate { get; set; }
        public string VendorCode { get; set; }
    }

    public class PurchaseService
    {
        public int HeaderStructureID { get; set; }
        public string PurchasingOrderNumber { get; set; }
        public string LongText { get; set; }
        public string ItemNumber { get; set; }
        public string LineItemNumber { get; set; }
        public string ActivityNumber { get; set; }
        public string ShortText { get; set; }
        public string OrderQuantity { get; set; }
        public string UnitMeasure { get; set; }
        public string GrossPrice { get; set; }
        public string NetPrice { get; set; }
        public string ActualQuantity { get; set; }
        public string LineItem { get; set; }

        public string WBS_Element { get; set; }
        public string Fund_Center { get; set; }
        public string Package_number { get; set; }
        public string Commitment_item { get; set; }
        public string Seq_No_Acc_Ass_ESKN { get; set; }
        public string Line_Number_INTROW { get; set; }
        public string SAC_Code { get; set; }
    }
}