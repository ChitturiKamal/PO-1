using PurchaseOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurchaseOrder.BAL
{
    public class PurchaseApproversDTO
    {
        public List<TEPOMasterApprover> MasterApproverlist { get; set; }
        public TEPOApprovalCondition ApprovalCondition { get; set; }
    }
    public class fundcenterDTO
    {
        public int uniqueID { get; set; }
        public int FundcenterUniqueID { get; set; }
        public int? categoryid { get; set; }
        public int? SUBCATEGORYID { get; set; }
        public string WBSCode { get; set; }
        public int? WbsUniqueID { get; set; }
        public string name { get; set; }
        public string FundCentreCode { get; set; }
        public string FundCenter_Description { get; set; }
        public int? LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedOn { get; set; }

    }
    public class MaterialDTO
    {
        public string material_name { get; set; }
        public string MaterialCode { get; set; }
        public string Image { get; set; }
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
}