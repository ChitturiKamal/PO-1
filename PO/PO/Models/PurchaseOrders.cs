using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using static PO.Models.PurchaseApproversDTO;

namespace PO.Models
{
    public class PurchaseOrders
    {
        RecordException except = new RecordException();
        TETechuvaDBContext db = new TETechuvaDBContext();

        public string addTEPOApprovalCondition(POApprovalCondition obj)
        {
            try
            {
                obj.CreatedOn = System.DateTime.Now;
                obj.LastModifiedOn = System.DateTime.Now;
                obj = db.POApprovalConditions.Add(obj);
                db.SaveChanges();
                return obj.UniqueId.ToString();
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return e.Message;
            }
        }

        public string addTEPOApprover(List<POMasterApprover> li, string approverUniqueID)
        {
            try
            {
                if (approverUniqueID != null && li != null)
                {
                    foreach (var obj in li)
                    {
                        obj.CreatedOn = System.DateTime.Now;
                        obj.LastModifiedOn = System.DateTime.Now;
                        obj.ApprovalConditionId = Convert.ToInt32(approverUniqueID);
                        db.POMasterApprovers.Add(obj);
                        db.SaveChanges();
                    }
                    return "success";
                }
                else
                    return "null";
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return e.Message;
            }
        }

        public List<MaterialDTO> ParseJobject(HttpResponseMessage tokenResponse)
        {
            var onjs = tokenResponse.Content.ReadAsStringAsync();
            JObject jo = JObject.Parse(onjs.Result);


            List<MaterialDTO> list = new List<MaterialDTO>();
            foreach (JObject x in jo.SelectToken("items"))
            {
                JToken headerstypeList = x.SelectToken("headers");
                int v = headerstypeList.Count();
                MaterialDTO mtlDTO = new MaterialDTO();
                mtlDTO.MaterialInfo = headerstypeList;
                foreach (JToken headertype in headerstypeList)
                {
                    if (headertype["name"].ToString() == "Classification")
                    {
                        mtlDTO.Mtl_Classific_Info = headertype["columns"];
                        foreach (JToken jack in headertype["columns"])
                        {
                            //for (int i = 1; i <= 7; i++)
                            //{

                                if (jack["key"].ToString() == "material_level_1")
                                {
                                    mtlDTO.Level1 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_2")
                                {
                                    mtlDTO.Level2 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_3")
                                {
                                    mtlDTO.Level3 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_4")
                                {
                                    mtlDTO.Level4 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_5")
                                {
                                    mtlDTO.Level5 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_6")
                                {
                                    mtlDTO.Level6 = jack["value"].ToString();
                                }
                                else if (jack["key"].ToString() == "material_level_7")
                                {
                                    mtlDTO.Level7 = jack["value"].ToString();
                                }
                            //    }
                        }
                    }
                    else if (headertype["name"].ToString() == "Planning")
                    {
                        mtlDTO.Mtl_Planning_Info = headertype["columns"];
                    }
                    else if (headertype["name"].ToString() == "Specifications")
                    {
                        mtlDTO.Mtl_Specs_Info = headertype["columns"];
                        //foreach (JToken jack in headertype["columns"])
                        //{
                        //    if (jack["name"].ToString() == "Specification Sheet")
                        //    {
                        //        mtlDTO.annex_CheckListId = jack["value"].ToString();
                        //    }
                        //}

                        foreach (JToken jack in headertype["columns"])
                        {
                            if (jack["key"].ToString() == "specification_sheet")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "id")
                                        {
                                            mtlDTO.annex_CheckListId = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (headertype["name"].ToString() == "System Logs")
                    {
                        mtlDTO.Mtl_Log_Info = headertype["columns"];
                    }
                    else if (headertype["name"].ToString() == "Quality")
                    {
                        mtlDTO.Mtl_Quality_Info = headertype["columns"];
                    }
                    else if (headertype["name"].ToString() == "General")
                    {
                        mtlDTO.Mtl_General_Info = headertype["columns"];
                        foreach (JToken jack in headertype["columns"])
                        {
                            if (jack["key"].ToString() == "material_name")
                            {
                                mtlDTO.material_name = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "material_code")
                            {
                                mtlDTO.MaterialCode = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "short_description")
                            {
                                mtlDTO.short_description = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "shade_number")
                            {
                                mtlDTO.shade_number = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "shade_description")
                            {
                                mtlDTO.shade_description = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "unit_of_measure")
                            {
                                mtlDTO.unit_of_measure = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "hsn_code")
                            {
                                mtlDTO.hsn_code = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "sap_code")
                            {
                                mtlDTO.sap_code = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "wbs_code")
                            {
                                mtlDTO.wbs_code = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "part_of_edesign")
                            {
                                mtlDTO.part_of_edesign = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "edesign_description")
                            {
                                mtlDTO.edesign_description = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "generic")
                            {
                                mtlDTO.generic = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "manufactured")
                            {
                                mtlDTO.manufactured = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "can_be_used_as_an_asset")
                            {
                                mtlDTO.can_be_used_as_an_asset = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "material_status")
                            {
                                mtlDTO.material_status = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "available_till")
                            {
                                mtlDTO.available_till = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "Image")
                            {
                                if (jack["value"].ToString() != null)
                                    try
                                    {
                                        foreach (JProperty im in jack["value"])
                                        {
                                            if (im.Name == "url")
                                            {
                                                var hi = im.Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { except.RecordUnHandledException(ex); }
                            }
                        }
                    }
                   else if (headertype["name"].ToString() == "Purchase")
                    {
                        mtlDTO.Mtl_Purchase_Info = headertype["columns"];
                        foreach (JToken jack in headertype["columns"])
                        {
                            if (jack["key"].ToString() == "qty_evaluation_method")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    try
                                    {
                                        foreach (JProperty im in jack["value"])
                                        {
                                            if (im.Name == "name")
                                            {
                                                mtlDTO.qty_evaluation_method = im.Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { except.RecordUnHandledException(ex); }
                                }
                            }
                            else if (jack["key"].ToString() == "general_po_terms")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "id")
                                        {
                                            mtlDTO.general_po_terms = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                            else if (jack["key"].ToString() == "special_po_terms")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    try
                                    {
                                        foreach (JProperty im in jack["value"])
                                        {
                                            if (im.Name == "id")
                                            {
                                                mtlDTO.special_po_terms = im.Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { except.RecordUnHandledException(ex); }
                                }
                            }
                            else if (jack["key"].ToString() == "last_purchase_rate")
                            {
                                mtlDTO.last_purchase_rate = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "weighted_average_purchase_rate")
                            {
                                mtlDTO.weighted_average_purchase_rate = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "purchase_rate_threshold")
                            {
                                mtlDTO.purchase_rate_threshold = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "gst_procurement")
                            {
                                mtlDTO.gst_procurement = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "gst_sales")
                            {
                                mtlDTO.gst_sales = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "approved_brands")
                            {
                                List<BrandDetail> BrandList = new List<BrandDetail>();
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JToken jx in jack["value"])
                                    {
                                        BrandDetail brandObj = new BrandDetail();
                                        foreach (JToken jd in jx["columns"])
                                        {
                                            if (jd["key"].ToString() == "brand_code")
                                            {
                                                brandObj.BrandSeries = jd["value"].ToString();
                                            }
                                            if (jd["key"].ToString() == "manufacturers_name")
                                            {
                                                brandObj.ManufactureName = jd["value"].ToString();
                                            }
                                            if (jd["key"].ToString() == "manufacturers_code")
                                            {
                                                brandObj.ManufactureCode = jd["value"].ToString();
                                            }
                                            if (jd["key"].ToString() == "brand_series")
                                            {
                                                brandObj.BrandSeries = jd["value"].ToString();
                                            }

                                        }
                                 //       if(!string.IsNullOrEmpty(brandObj.BrandSeries))
                                            BrandList.Add(brandObj);
                                    }
                                    mtlDTO.BrandList = BrandList;
                                }

                            }
                            else if (jack["key"].ToString() == "approved_vendors")
                            {
                                mtlDTO.approved_vendors = jack["value"].ToString();
                            }

                        }
                    }
                   else if (headertype["name"].ToString() == "Planning")
                    {
                        mtlDTO.Mtl_Planning_Info = headertype["columns"];
                        foreach (JToken jack in headertype["columns"])
                        {

                            if (jack["key"].ToString() == "po_lead_time")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    try
                                    {
                                        foreach (JProperty im in jack["value"])
                                        {
                                            if (im.Name == "value")
                                            {
                                                mtlDTO.po_lead_time = im.Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { except.RecordUnHandledException(ex); }
                                }

                            }
                            else if (jack["key"].ToString() == "delivery_lead_time")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    try
                                    {
                                        foreach (JProperty im in jack["value"])
                                        {
                                            if (im.Name == "value")
                                            {
                                                mtlDTO.delivery_lead_time = im.Value.ToString();
                                            }
                                        }
                                    }
                                    catch (Exception ex) { except.RecordUnHandledException(ex); }
                                }

                            }
                            else if (jack["key"].ToString() == "min_order_qty")
                            {
                                mtlDTO.min_order_qty = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "maintain_lot")
                            {
                                mtlDTO.maintain_lot = jack["value"].ToString();
                            }
                        }
                    }

                }
                if (mtlDTO.MaterialCode != null)
                {
                    mtlDTO.ItemType = "MaterialOrder";
                    list.Add(mtlDTO);
                }

            }
            return list;
        }
        public List<ServiceDTO> ParseServiceJobject_BackUp(HttpResponseMessage tokenResponse)
        {
            var onjs = tokenResponse.Content.ReadAsStringAsync();
            JObject jo = JObject.Parse(onjs.Result);


            List<ServiceDTO> list = new List<ServiceDTO>();
            foreach (JObject x in jo.SelectToken("items"))
            {
                JToken type = x.SelectToken("headers");
                string typeStr = type.ToString().ToLower();               
                int v = type.Count();
                ServiceDTO service = new ServiceDTO();               
                foreach (JToken jad in type)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (jad["name"].ToString() == "Classification")
                        {
                            foreach (JToken jack in jad["columns"])
                            {
                                for (int i = 1; i <= 7; i++)
                                {

                                    if (jack["key"].ToString() == "service_level_1")
                                    {
                                        service.ServiceLevel1 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_2")
                                    {
                                        service.ServiceLevel2 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_3")
                                    {
                                        service.ServiceLevel3 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_4")
                                    {
                                        service.ServiceLevel4 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_5")
                                    {
                                        service.ServiceLevel5 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_6")
                                    {
                                        service.ServiceLevel6 = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_level_7")
                                    {
                                        service.ServiceLevel7 = jack["value"].ToString();
                                    }
                                }
                            }
                        }
                        if (jad["name"].ToString() == "General")
                        {
                            foreach (JToken jack in jad["columns"])
                            {
                                for (int i = 1; i <= 7; i++)
                                {

                                    if (jack["key"].ToString() == "service_code")
                                    {
                                        service.ServiceCode = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "wbs_code")
                                    {
                                        service.WBSCode = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "short_description")
                                    {
                                        service.ShortDescription = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "edesign_description")
                                    {
                                        service.EdesignDescription = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "unit_of_measure")
                                    {
                                        service.UnitOfMeasure = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "service_status")
                                    {
                                        service.ServiceStatus = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "sac")
                                    {
                                        service.SAC = jack["value"].ToString();
                                    }
                                }
                            }
                        }
                        if (jad["name"].ToString() == "Purchase")
                        {
                            foreach (JToken jack in jad)
                            {
                                for (int i = 1; i <= 8; i++)
                                {
                                    if (jack["key"].ToString() == "method_of_measurement")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "name")
                                                {
                                                    service.MethodOfMesurement = im.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (jack["key"].ToString() == "general_so_terms")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "id")
                                                {
                                                    service.GeneraalSoTerms = im.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (jack["key"].ToString() == "special_so_terms")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "id")
                                                {
                                                    service.SpecialSOTerms = im.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (jack["key"].ToString() == "last_purchase_rate")
                                    {
                                        service.LastPurchaseRate = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "weighted_average_purchase_rate")
                                    {
                                        service.weightedAveragePurchaseRate = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "procurement_rate_threshold")
                                    {
                                        service.PurchaseRateThreshold = jack["value"].ToString();
                                    }
                                }

                            }
                        }
                    }

                }
                if (service.ServiceCode != null)
                {
                    list.Add(service);
                }

            }
            return list;
        }

        public List<MaterialDTO> ParseServiceJobject(HttpResponseMessage tokenResponse)
        {
            var onjs = tokenResponse.Content.ReadAsStringAsync();
            JObject jo = JObject.Parse(onjs.Result);
            List<MaterialDTO> list = new List<MaterialDTO>();
            foreach (JObject x in jo.SelectToken("items"))
            {
                JToken type = x.SelectToken("headers");
               // string typeStr = type.ToString().ToLower();
                int v = type.Count();
                MaterialDTO service = new MaterialDTO();
                service.MaterialInfo = type;
                foreach (JToken jad in type)
                {
                    //for (int j = 0; j <= 4; j++)
                    //{
                    if (jad["name"].ToString() == "Classification")
                    {
                        service.Mtl_Classific_Info = jad;
                        foreach (JToken jack in jad["columns"])
                        {
                            //for (int i = 1; i <= 7; i++)
                            //{

                            if (jack["key"].ToString() == "service_level_1")
                            {
                                service.Level1 = jack["value"].ToString();
                            }
                            else
                            if (jack["key"].ToString() == "service_level_2")
                            {
                                service.material_name = jack["value"].ToString();
                                service.Level2 = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_level_3")
                            {
                                service.Level3 = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_level_4")
                            {
                                service.Level4 = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_level_5")
                            {
                                service.Level5 = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_level_6")
                            {
                                service.Level6 = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_level_7")
                            {
                                service.Level7 = jack["value"].ToString();
                            }
                            //}
                        }
                    }
                    else if (jad["name"].ToString() == "General")
                    {
                        service.Mtl_General_Info = jad;
                        foreach (JToken jack in jad["columns"])
                        {
                            if (jack["key"].ToString() == "service_code")
                            {
                                service.MaterialCode = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "wbs_code")
                            {
                                service.wbs_code = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "short_description")
                            {
                                service.short_description = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "edesign_description")
                            {
                                service.edesign_description = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "unit_of_measure")
                            {
                                service.unit_of_measure = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "service_status")
                            {
                                service.material_status = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "sac")
                            {
                                service.hsn_code = jack["value"].ToString();
                            }
                        }
                    }
                    else if (jad["name"].ToString() == "Purchase")
                    {
                        service.Mtl_Purchase_Info = jad;
                        foreach (JToken jack in jad["columns"])
                        {
                            if (jack["key"].ToString() == "method_of_measurement")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "name")
                                        {
                                            service.qty_evaluation_method = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                            else if (jack["key"].ToString() == "general_so_terms")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "id")
                                        {
                                            service.general_po_terms = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                            else if (jack["key"].ToString() == "special_so_terms")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "id")
                                        {
                                            service.special_po_terms = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                            else if (jack["key"].ToString() == "last_purchase_rate")
                            {
                                service.last_purchase_rate = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "weighted_average_purchase_rate")
                            {
                                service.weighted_average_purchase_rate = jack["value"].ToString();
                            }
                            else if (jack["key"].ToString() == "procurement_rate_threshold")
                            {
                                service.gst_procurement = jack["value"].ToString();
                            }
                        }
                    }
                   else if (jad["name"].ToString() == "Planning")
                    {
                        service.Mtl_Planning_Info = jad;
                    }
                    else if (jad["name"].ToString() == "Quality")
                    {
                        service.Mtl_Quality_Info = jad;
                    }
                    else if (jad["name"].ToString() == "System Logs")
                    {
                        service.Mtl_Log_Info = jad;
                    }
                    else if (jad["name"].ToString() == "Classification Definition")
                    {
                        service.Mtl_Definition_Info = jad;
                    }
                }
                if (service.MaterialCode != null)
                {
                    service.ItemType = "ServiceOrder";
                    list.Add(service);
                }

            }
            return list;
        }

        public void ParseServiceAnnexureJobject(HttpResponseMessage tokenResponse, int headerid, int ItemId)
        {
            var onjs = tokenResponse.Content.ReadAsStringAsync();
            JObject jo = JObject.Parse(onjs.Result);
            List<POServiceAnnexureDTO> list = new List<POServiceAnnexureDTO>();
            JToken type = jo.SelectToken("headers");
            int v = type.Count();
            POServiceAnnexureDTO service = new POServiceAnnexureDTO();
            foreach (JToken jad in type)
            {
                if (jad["key"].ToString() == "classification_definition")
                {
                    foreach (JToken jack in jad["columns"])
                    {
                        TEPOServiceAnnexure annexure = new TEPOServiceAnnexure();
                        annexure.Name = jack["name"].ToString();
                        annexure.Value = jack["value"].ToString();
                        annexure.PO_HeaderStructureID = headerid;
                        annexure.PO_ItemStructureID = ItemId;
                        annexure.IsDeleted = false;
                        db.TEPOServiceAnnexures.Add(annexure);
                        db.SaveChanges();
                    }
                }
            }
        }

        public int SavePurchaseHeaderStructure(PurchaseHeaderStructure headerStructure)
        {
            TEPOHeaderStructure hsa = new TEPOHeaderStructure();
            int uniqueID = 0;
            TEPOHeaderStructure hs = new TEPOHeaderStructure();
            hs.Objectid = "TEPurchaseHeaderService";
            hs.Status = "Active";
            hs.Purchasing_Order_Number = headerStructure.PurchasingOrderNumber;
            hs.Fugue_Purchasing_Order_Number = headerStructure.FuguePurchasingOrderNumber;
            hs.Purchasing_Document_Type = headerStructure.PurchasingDocumentType;
            hs.Vendor_Account_Number = headerStructure.VendorAccountNumber;
            hs.Purchasing_Document_Date = headerStructure.PurchasingDocumentDate;
            hs.Purchasing_Organization = headerStructure.CompanyCode;
            hs.Purchasing_Group = headerStructure.PurchasingGroup;
            hs.Company_Code = headerStructure.CompanyCode;
            hs.Payment_Key = "0001";
            hs.Currency_Key = headerStructure.CurrencyKey;
            hs.Exchange_Rate = headerStructure.ExchangeRate;
            hs.Managed_by = headerStructure.ManagedBy;
            hs.You_Reference = headerStructure.YouReference;
            hs.Telephone = headerStructure.Telephone;
            hs.Our_Reference = headerStructure.OurReference;
            //
            hs.PO_Title = headerStructure.POTitle;
            hs.Agreement_signed_date = headerStructure.AgreementSignedDate;
            hs.Version = headerStructure.Version;
            hs.Reason_change = headerStructure.ReasonChange;
            hs.Requested_By = System.DateTime.Now.ToLongDateString();
            hs.ReleaseGroup = headerStructure.ReleaseGroup;
            hs.ReleaseStrategy = headerStructure.ReleaseStrategy;
            hs.ReleaseCode1 = headerStructure.ReleaseCode1;
            hs.ReleaseCode2 = headerStructure.ReleaseCode2;
            hs.ReleaseCode3 = headerStructure.ReleaseCode3;
            hs.ReleaseCode4 = headerStructure.ReleaseCode4;
            hs.ReleaseStatus = headerStructure.ReleaseStatus;
            hs.VersionTextField = headerStructure.VersionTextField;
            hs.path = headerStructure.Path;
            hs.Statusversion = headerStructure.Statusversion;
            hs.ReleaseCodes = headerStructure.ReleaseCodes;
            hs.SubmitterName = headerStructure.SubmitterName;
            hs.SubmitterEmailID = headerStructure.SubmitterEmailID;
            hs.PartnerFunction1 = headerStructure.PartnerFunction1;
            hs.PartnerFunction2 = headerStructure.PartnerFunction2;
            hs.PartnerFunctionVendorCode1 = headerStructure.PartnerFunctionVendorCode1;
            hs.PartnerFunctionVendorCode2 = headerStructure.PartnerFunctionVendorCode2;
            hs.CreatedOn = System.DateTime.Now;
            hs.LastModifiedOn = System.DateTime.Now;
           db.TEPOHeaderStructures.Add(hs);
            db.SaveChanges();
            if (hs.Uniqueid != 0)
                uniqueID = hs.Uniqueid;

            return uniqueID;
        }

        public int SavePurchaseItemStructure(PurchaseItemStructure itemStructure, int headerStructureID)
        {
            int uniqueID = 0;

            TEPOItemStructure itms = new TEPOItemStructure();
            itms.Purchasing_Order_Number = headerStructureID.ToString();
            itms.Item_Number = itemStructure.Item_Number;
            itms.Assignment_Category = itemStructure.Assignment_Category;
            itms.Item_Category = itemStructure.Item_Category;
            itms.Material_Number = itemStructure.Material_Number;//materialcode
            itms.Short_Text = itemStructure.Short_Text;//shortDescription
            itms.Long_Text = itemStructure.Long_Text;
            itms.Line_item = itemStructure.Line_item;
            itms.Order_Qty = itemStructure.Order_Qty;//Quantity
            itms.Unit_Measure = itemStructure.Unit_Measure;//Unit
            itms.Delivery_Date = itemStructure.Delivery_Date;
            itms.Net_Price = itemStructure.Net_Price;
            itms.Material_Group = itemStructure.Material_Group;

            itms.Plant = itemStructure.Plant;//companyCode
            itms.Storage_Location = itemStructure.Storage_Location;//company Code
            itms.IsDeleted = false;
            itms.Requirement_Tracking_Number = itemStructure.Requirement_Tracking_Number;
            itms.Requisition_Number = itemStructure.Requisition_Number;
            itms.Item_Purchase_Requisition = itemStructure.Item_Purchase_Requisition;
            itms.Returns_Item = itemStructure.Returns_Item;
            itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code;
            itms.Overall_limit = itemStructure.Overall_limit;
            itms.Expected_Value = itemStructure.Expected_Value;
            //itms.Actual_Value = itemStructure.Actual_Value;
            itms.No_Limit = itemStructure.No_Limit;
            itms.Overdelivery_Tolerance = itemStructure.Overdelivery_Tolerance;
            itms.Underdelivery_Tolerance = itemStructure.Underdelivery_Tolerance;
            itms.CreatedOn = System.DateTime.Now.ToString();
            itms.LastModifiedOn = System.DateTime.Now.ToString();
            itms.Status = itemStructure.Status;

            //itms.HSN_Code = itemStructure.HSN_Code;

            db.TEPOItemStructures.Add(itms);
            db.SaveChanges();
            if (itms.Uniqueid != 0)
                uniqueID = itms.Uniqueid;

            return uniqueID;
        }

        public int SavePurchaseService(PurchaseService purServ)
        {
            int uniqueID = 0;

            TEPOService serv = new TEPOService();
            serv.Purchasing_Order_Number = purServ.PurchasingOrderNumber;
            serv.LongText = purServ.LongText;
            serv.Item_Number = purServ.ItemNumber;
            serv.Line_item_number = purServ.LineItemNumber;
            serv.Activity_Number = purServ.ActivityNumber;
            serv.Short_Text = purServ.ShortText;//shortdescription
            //serv.OrderQuantity = purServ.OrderQuantity;
            serv.Unit_Measure = purServ.UnitMeasure;
            serv.Gross_Price = purServ.GrossPrice;
            serv.Net_Price = purServ.NetPrice;
            serv.Actual_Qty = purServ.ActualQuantity;
            serv.line_item = purServ.LineItem;
            serv.WBS_Element = purServ.WBS_Element;//materialWBS
            serv.Fund_Center = purServ.Fund_Center;//FundcenterID
            serv.Package_number = purServ.Package_number;
            serv.Commitment_item = purServ.Commitment_item;
            serv.Seq_No_Acc_Ass_ESKN = purServ.Seq_No_Acc_Ass_ESKN;
            serv.Line_Number_INTROW = purServ.Line_Number_INTROW;
            //.SAC_Code = purServ.SAC_Code;
            db.TEPOServices.Add(serv);
            db.SaveChanges();
            if (serv.Uniqueid != 0)
                uniqueID = serv.Uniqueid;

            return uniqueID;
        }

        public int PostPOMilestones(TEPOVendorPaymentMilestone value)
        {
            TEPOVendorPaymentMilestone result = value;

            if (!(value.UniqueId + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result.ModuleName = "PO";
                result = db.TEPOVendorPaymentMilestones.Add(value);
            }
            else
            {
                db.TEPOVendorPaymentMilestones.Attach(value);

                foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                {
                    string propname = item.Name;
                    if (propname.ToLower() == "createdon")
                        continue;
                    object propValue = item.GetValue(value);
                    if (propValue != null || Convert.ToString(propValue).Length != 0)
                        db.Entry(value).Property(propname).IsModified = true;
                }

                value.LastModifiedOn = System.DateTime.Now;
                db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
            }

            db.SaveChanges();

            return value.UniqueId;
        }

        public int SavePurchaseItemWiseCondition(PurchaseItemwise purItemCond)
        {
            int uniqueID = 0;

            TEPOItemwise itm = new TEPOItemwise();
            itm.Purchasing_Order_Number = purItemCond.PurchasingOrderNumber;
            itm.Item_Number_of_Purchasing_Document = purItemCond.ItemNumberPurchase;
            itm.Condition_Type = purItemCond.ConditionType;
            itm.Condition_rate = Convert.ToDouble(purItemCond.ConditionRate);
            itm.VendorCode = purItemCond.VendorCode;//vendorCode
            db.TEPOItemwises.Add(itm);
            db.SaveChanges();
            if (itm.Uniqueid != 0)
                uniqueID = itm.Uniqueid;

            return uniqueID;
        }

        public HttpResponseMessage SavePO(PurchaseHeaderStructure headerStructure,
                                        PurchaseItemStructure itemStructure,
                                        PurchaseService purServ,
                                        TEPOVendorPaymentMilestone value,
                                        PurchaseItemwise purItemCond)

        {
            bool res = true;
            int headerStructureID = 0, serviceID = 0, PaymntID = 0;
            HttpResponseMessage hrm = new HttpResponseMessage();
            SuccessInfo sinfo = new SuccessInfo();
            try
            {
                headerStructureID = SavePurchaseHeaderStructure(headerStructure);
                if (headerStructureID > 0)
                {
                    itemStructure.HeaderStructureID = headerStructureID;
                    purServ.HeaderStructureID = headerStructureID;
                    value.ContextIdentifier = value.ContextIdentifier;
                    purItemCond.HeaderStructureID = headerStructureID;

                    //itemID = SavePurchaseItemStructure(itemStructure);
                    serviceID = SavePurchaseService(purServ);
                    PaymntID = PostPOMilestones(value);
                    //ConditionID = SavePurchaseItemWiseCondition(purItemCond);
                }
            }
            catch (Exception ex)
            {
                new RecordException().RecordUnHandledException(ex);
                res = false;
            }
            if (res)
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(new { info = sinfo })
                };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Save";
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotAcceptable, Content = new JsonContent(new { info = sinfo }) };
            }
        }
        public HttpResponseMessage SavePO(PurchaseHeaderStructure headerStructure,
                                       List<PurchaseItemStructure> itemStructure,
                                       List<PurchaseService> purServ,
                                       List<TEPOVendorPaymentMilestone> paymntMilestone,
                                       List<PurchaseItemwise> purItemCond)

        {
            bool res = true;
            int headerStructureID = 0,serviceID = 0, PaymntID = 0;
            HttpResponseMessage hrm = new HttpResponseMessage();
            SuccessInfo sinfo = new SuccessInfo();
            try
            {
                headerStructureID = SavePurchaseHeaderStructure(headerStructure);
                if (headerStructureID > 0)
                {
                    if (itemStructure.Count > 0)
                    {
                        foreach (PurchaseItemStructure item in itemStructure)
                        {
                            item.HeaderStructureID = headerStructureID;
                            // itemID = SavePurchaseItemStructure(item);
                        }
                    }
                    if (purServ.Count > 0)
                    {
                        foreach (PurchaseService purch in purServ)
                        {
                            purch.HeaderStructureID = headerStructureID;
                            serviceID = SavePurchaseService(purch);
                        }
                    }
                    if (paymntMilestone.Count > 0)
                    {
                        foreach (TEPOVendorPaymentMilestone mstn in paymntMilestone)
                        {
                            mstn.ContextIdentifier = headerStructureID.ToString();
                            PaymntID = PostPOMilestones(mstn);
                        }
                    }
                    if (purItemCond.Count > 0)
                    {
                        foreach (PurchaseItemwise purchItem in purItemCond)
                        {
                            purchItem.HeaderStructureID = headerStructureID;
                            //ConditionID = SavePurchaseItemWiseCondition(purchItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              new  RecordException().RecordUnHandledException(ex);
                res = false;
            }
            if (res)
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
                return new HttpResponseMessage()
                {
                    Content = new JsonContent(new { info = sinfo })
                };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Save";
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotAcceptable, Content = new JsonContent(new { info = sinfo }) };
            }
        }
        //Create calls
        public HttpResponseMessage SaveTermsAndConditions(List<TETermsAndCondition> listOfConditions, int headerStrutureID)
        {
            string Message = "";
            try
            {
                if (listOfConditions == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,

                    };
                }
                foreach (var obj in listOfConditions)
                {
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.ContextIdentifier = headerStrutureID.ToString();
                    obj.CreatedOn = System.DateTime.Now;
                    obj.LastModifiedOn = System.DateTime.Now;
                    obj.Title = "General";
                    db.TETermsAndConditions.Add(obj);
                    db.SaveChanges();
                }
                Message = "success";
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }

        }
        public HttpResponseMessage SaveSpecialTermsAndConditions(List<TETermsAndCondition> listOfConditions, int headerStrutureID)
        {
            string Message = "";
            try
            {
                if (listOfConditions == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,

                    };
                }
                int idd = 1;
                foreach (var obj in listOfConditions)
                {

                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    obj.ContextIdentifier = headerStrutureID.ToString();
                    obj.CreatedOn = System.DateTime.Now;
                    obj.LastModifiedOn = System.DateTime.Now;
                    obj.Title = "special";
                    obj.SequenceId = idd;
                    db.TETermsAndConditions.Add(obj);
                    db.SaveChanges();
                    idd++;
                }
                Message = "success";
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }

        }
        public HttpResponseMessage SavePurchaseService(List<PurchaseService> purServ)
        {
            string Message = "";
            if (purServ.Count > 0)
            {
                foreach (PurchaseService purch in purServ)
                {
                    SavePurchaseService(purch);
                }
                Message = "success";
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent(Message)
                };
            }
        }
        public HttpResponseMessage SavePurchaseItemStructure(List<PurchaseItemStructure> itemStructure)
        {
            string Message = "";
            try
            {
                if (itemStructure.Count > 0)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(Message)
                    };
                }
                foreach (PurchaseItemStructure item in itemStructure)
                {

                    //SavePurchaseItemStructure(item);
                }
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };

            }
            catch (Exception e)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }


        //Update Calls
        public HttpResponseMessage UpdateTermsAndConditions(List<TETermsAndCondition> listOfConditions)
        {
            string Message = "";
            try
            {
                if (listOfConditions == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,

                    };
                }
                foreach (var obj in listOfConditions)
                {
                    if (obj.UniqueId > 0)
                    {
                        var back = db.TETermsAndConditions.Where(a => a.UniqueId == obj.UniqueId).SingleOrDefault();
                        obj.IsActive = true;
                        obj.IsDeleted = false;
                        obj.CreatedBy = back.CreatedBy;
                        obj.CreatedOn = back.CreatedOn;
                        obj.LastModifiedOn = System.DateTime.Now;
                        obj.Title = "General";
                        db.Entry(back).CurrentValues.SetValues(obj);
                    }
                    else
                    {
                        obj.CreatedOn = System.DateTime.Now;
                        obj.LastModifiedOn = System.DateTime.Now;
                        db.TETermsAndConditions.Add(obj);
                    }
                }
                Message = "success";
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }

        }
        public HttpResponseMessage UpdatePurchaseService(List<TEPOService> purServ)
        {
            string Message = "";
            if (purServ.Count > 0)
            {
                foreach (TEPOService purch in purServ)
                {
                    if (purch.Uniqueid > 0)
                    {
                        var back = db.TEPOServices.Where(a => a.Uniqueid == purch.Uniqueid).SingleOrDefault();
                        purch.CreatedBy = back.CreatedBy;
                        purch.CreatedOn = back.CreatedOn;
                        purch.LastModifiedOn = System.DateTime.Now.ToString();
                        db.Entry(back).CurrentValues.SetValues(purch);
                    }
                    else
                    {
                        purch.CreatedOn = System.DateTime.Now.ToString();
                        purch.LastModifiedOn = System.DateTime.Now.ToString();
                        db.TEPOServices.Add(purch);
                    }
                }
                Message = "success";
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent(Message)
                };
            }
        }
        public HttpResponseMessage UpdatePurchaseItemStructure(List<TEPOItemStructure> itemStructure)
        {
            string Message = "";
            try
            {
                if (itemStructure.Count > 0)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(Message)
                    };
                }
                foreach (TEPOItemStructure item in itemStructure)
                {
                    if (item.Uniqueid == 0)
                    {
                        var back = db.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).SingleOrDefault();
                        item.CreatedOn = back.CreatedOn;
                        item.CreatedBy = back.CreatedBy;
                        item.LastModifiedOn = System.DateTime.Now.ToString();
                        db.Entry(back).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.CreatedOn = System.DateTime.Now.ToString();
                        item.LastModifiedOn = System.DateTime.Now.ToString();
                        db.TEPOItemStructures.Add(item);
                    }

                }
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Message)
                };

            }
            catch (Exception e)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }


        public List<Mat_Serv_Seq> GetPOHeadItemStructure(int? HeadStructureID)
        {
            List<Mat_Serv_Seq> PoStructItem = new List<Mat_Serv_Seq>();

           

            BAL.PurchaseOrderBAL POBAL = new BAL.PurchaseOrderBAL();
            PODetails PODetails = new PODetails();
            PODetails.PurchaseItemStructureDetails = this.GetPurchaseItemStructureByPOStructureId(HeadStructureID);
            PODetails.POServiceHeader = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == HeadStructureID && x.IsDeleted == false).ToList();
            PoStructItem = Seq_Mat_Service(PODetails, Convert.ToInt32(HeadStructureID));

            return PoStructItem;
        }



        public POHeadItemStructure GetPOHeadItemStructureWorkOrd(int? HeadStructureID)
        {
            POHeadItemStructure PoStructItem = new POHeadItemStructure();
            PoStructItem.UniqueID = HeadStructureID;
            List<POSubItemStructure> PoSubItem = new List<POSubItemStructure>();

            var HeadOrderType = (from Head in db.TEPOHeaderStructures
                                 join OrdType in db.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == HeadStructureID
                                 select OrdType.Code).FirstOrDefault();
                POSubItemStructure TempItemStructureFirst = new POSubItemStructure();


            //PoSubItem.Add(TempItemStructureFirst);
            List<PurchaseSubItemStructure> ItemStruct = this.GetPurchaseItemStructureByPOStructureId(HeadStructureID);

            POSubItemStructure TempItemStructure = new POSubItemStructure();
            //TempItemStructure.HeadTitle = ;
            //TempItemStructure.HeadDescription = ;
            TempItemStructure.Itemstructure = new List<PurchaseSubItemStructure>();
            PoSubItem.Add(TempItemStructure);


            List<TEPOServiceHeader> POItemsHeads = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == HeadStructureID && x.IsDeleted == false).OrderBy(x => x.UniqueID).ToList();
            if (POItemsHeads.Count > 0)
            {
                foreach (TEPOServiceHeader POServiceLoopHead in POItemsHeads)
                {
                    List<PurchaseSubItemStructure> ItemStructVar2 = ItemStruct.Where(x => x.ServiceHeaderId == POServiceLoopHead.UniqueID && x.Assignment_Category != "P" && x.Item_Category != "D").OrderBy(x => x.ItemStructureID).ToList();

                    POSubItemStructure TempItemStructure3 = new POSubItemStructure();
                    TempItemStructure3.HeadTitle = POServiceLoopHead.Title;
                    TempItemStructure3.HeadDescription = POServiceLoopHead.Description;
                    TempItemStructure3.HeadID = POServiceLoopHead.UniqueID;
                    TempItemStructure3.Itemstructure = ItemStructVar2;
                    PoSubItem.Add(TempItemStructure3);
                }

                
                List<PurchaseSubItemStructure> ItemStructVar = ItemStruct.Where(x => x.ServiceHeaderId == null || x.ServiceHeaderId == 0).ToList();
                if (ItemStructVar.Count > 0)
                {
                    foreach (PurchaseSubItemStructure Items in ItemStructVar)
                    {
                        POSubItemStructure TempItemStructure2 = new POSubItemStructure();
                        List<PurchaseSubItemStructure> ItemStructVal = new List<PurchaseSubItemStructure>();
                        ItemStructVal.Add(Items);
                        TempItemStructure2.Itemstructure = ItemStructVal;
                        PoSubItem.Add(TempItemStructure2);
                    }
                }
            }
            else
            {
                List<PurchaseSubItemStructure> ItemStructVar = ItemStruct.Where(x => x.ServiceHeaderId == null || x.ServiceHeaderId == 0).ToList();
                if (ItemStructVar.Count > 0)
                {
                    foreach (PurchaseSubItemStructure Items in ItemStructVar)
                    {
                        POSubItemStructure TempItemStructure2 = new POSubItemStructure();
                        List<PurchaseSubItemStructure> ItemStructVal = new List<PurchaseSubItemStructure>();
                        ItemStructVal.Add(Items);
                        TempItemStructure2.Itemstructure = ItemStructVal;
                        PoSubItem.Add(TempItemStructure2);
                    }
                }
            }
           



            PoStructItem.ItemStructureList = PoSubItem;
            return PoStructItem;
        }

        public List<PurchaseSubItemStructure> GetPurchaseItemStructureByPOStructureId(int? purchaseHeaderStructureId)
        {
            var purItemstructDtls = (from purHead in db.TEPOHeaderStructures
                                     join itm in db.TEPOItemStructures on purHead.Uniqueid equals itm.POStructureId
                                     join OrderTyp in db.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals OrderTyp.UniqueId
                                     where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                     && itm.IsDeleted == false 
                                     select new PurchaseSubItemStructure
                                     {

                                         ItemStructureID = itm.Uniqueid,
                                         HeaderStructureID = itm.POStructureId,
                                         ItemType = itm.ItemType,
                                         PurchasingOrderNumber = itm.Purchasing_Order_Number,
                                         Item_Number = itm.Item_Number,
                                         Assignment_Category = itm.Assignment_Category,
                                         Item_Category = itm.Item_Category,
                                         Material_Number = itm.Material_Number,
                                         Short_Text = itm.Short_Text,
                                         Long_Text = itm.Long_Text,
                                         Line_item = itm.Line_item,
                                         Order_Qty = itm.Order_Qty,
                                         Unit_Measure = itm.Unit_Measure,
                                         Delivery_Date = itm.Delivery_Date,
                                         Net_Price = itm.Net_Price,
                                         Material_Group = itm.Material_Group,
                                         Plant = itm.Plant,
                                         Storage_Location = itm.Storage_Location,
                                         Requirement_Tracking_Number = itm.Requirement_Tracking_Number,
                                         Requisition_Number = itm.Requisition_Number,
                                         Item_Purchase_Requisition = itm.Item_Purchase_Requisition,
                                         Returns_Item = itm.Returns_Item,
                                         Tax_salespurchases_code = itm.Tax_salespurchases_code,
                                         Overall_limit = itm.Overall_limit,
                                         Expected_Value = itm.Expected_Value,
                                         //Actual_Value = string.Empty,
                                         No_Limit = itm.No_Limit,
                                         Overdelivery_Tolerance = itm.Overdelivery_Tolerance,
                                         Underdelivery_Tolerance = itm.Underdelivery_Tolerance,
                                         HSN_Code = itm.HSNCode,
                                         Status = itm.Status,
                                         WBSElementCode = itm.WBSElementCode,
                                         WBSElementCode2 = itm.WBSElementCode2,
                                         InternalOrderNumber = itm.InternalOrderNumber,
                                         GLAccountNo = itm.GLAccountNo,
                                         Brand = itm.Brand,
                                         Rate = itm.Rate,
                                         TotalAmount = itm.TotalAmount,
                                         IGSTRate = itm.IGSTRate,
                                         IGSTAmount = itm.IGSTAmount,
                                         CGSTRate = itm.CGSTRate,
                                         CGSTAmount = itm.CGSTAmount,
                                         SGSTRate = itm.SGSTRate,
                                         SGSTAmount = itm.SGSTAmount,
                                         TotalTaxAmount = itm.TotalTaxAmount,
                                         GrossAmount = itm.GrossAmount,
                                         IsRecordInSAP = itm.IsRecordInSAP,
                                         CreatedByID = purHead.CreatedBy,
                                         //LastModifiedByID
                                         ManufactureCode = itm.ManufactureCode,
                                         ServiceHeaderId = itm.ServiceHeaderId,
                                         CreatedBy = itm.CreatedBy,
                                         LastModifiedBy = itm.LastModifiedBy
                                     }).OrderBy(a => a.ItemStructureID).ToList();
            foreach (var data in purItemstructDtls)
            {
                data.POTechnicalSpecifiactionList = db.TEPOSpecificationAnnexures.Where(a => a.POItemStructureId == data.ItemStructureID && a.IsDeleted == false).ToList();
            }

            return purItemstructDtls;
        }

        //Required Classes

        public List<Mat_Serv_Seq> Seq_Mat_Service(PODetails PODetails, int POID)
        {
            List<Mat_Serv_Seq> SeqList = new List<Mat_Serv_Seq>();
            Mat_Serv_Seq Emptymat = new Mat_Serv_Seq();

            Emptymat.ServHead = new TEPOServiceHeader();
            Emptymat.ItemS = new List<PurchaseSubItemStructure>();
            SeqList.Add(Emptymat);

            List<String> Mat_SeqList = PODetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == 0 || x.ServiceHeaderId == null).Select(x => x.Item_Number).ToList();
            int Mat_Seq = 0;
            foreach (String MatList in Mat_SeqList)
            {
                int Temp_Mat_Seq = Convert.ToInt32(MatList);
                if (Temp_Mat_Seq > Mat_Seq)
                    Mat_Seq = Temp_Mat_Seq;
            }
            List<String> Service_SeqList = PODetails.POServiceHeader.Select(x => x.ItemNumber).ToList();
            int Service_Seq = 0;
            foreach (String ServList in Service_SeqList)
            {
                int Temp_Mat_Seq = Convert.ToInt32(ServList);
                if (Temp_Mat_Seq > Service_Seq)
                    Service_Seq = Temp_Mat_Seq;
            }


            if (Mat_Seq != 0 || Service_Seq != 0)

            {
                int MaxSeq = 0;

                if (Mat_Seq > Service_Seq)
                    MaxSeq = Convert.ToInt32(Mat_Seq);
                if (Mat_Seq < Service_Seq)
                    MaxSeq = Convert.ToInt32(Service_Seq);
                else if (Mat_Seq == Service_Seq)
                    MaxSeq = Convert.ToInt32(Service_Seq);

                for (int i = 1; i <= MaxSeq; i++)
                {
                    String CountVal = i.ToString();

                    List<TEPOServiceHeader> ServHead = new List<TEPOServiceHeader>();
                    ServHead = PODetails.POServiceHeader.Where(x => x.ItemNumber == CountVal).ToList();

                    List<PurchaseSubItemStructure> POItems = new List<PurchaseSubItemStructure>();
                    POItems = PODetails.PurchaseItemStructureDetails.Where(x => x.Item_Number == CountVal && (x.ServiceHeaderId == 0 || x.ServiceHeaderId == null)).ToList();

                    // For Service Items whcich has sequence
                    foreach (TEPOServiceHeader ItemHead in ServHead)
                    {
                        Mat_Serv_Seq ListNonSeq = new Mat_Serv_Seq();
                        TEPOServiceHeader TempServ = new TEPOServiceHeader();
                        ListNonSeq.ServHead = ItemHead;
                        ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ItemHead.UniqueID).ToList();
                        SeqList.Add(ListNonSeq);
                    }

                    // For Material Items whcich has sequence
                    if (POItems.Count > 0)
                    {
                        foreach (PurchaseSubItemStructure TempPO in POItems)
                        {
                            Mat_Serv_Seq ListMatSeq = new Mat_Serv_Seq();
                            ListMatSeq.ItemS = new List<PurchaseSubItemStructure>();
                            ListMatSeq.ItemS.Add(TempPO);
                            SeqList.Add(ListMatSeq);
                        }
                    }
                }
                // For Null Values whcich has no sequence
                List<TEPOServiceHeader> NullServHead = new List<TEPOServiceHeader>();
                NullServHead = PODetails.POServiceHeader.Where(x => x.ItemNumber == null).ToList();

                List<PurchaseSubItemStructure> NullPOItems = new List<PurchaseSubItemStructure>();
                NullPOItems = PODetails.PurchaseItemStructureDetails.Where(x => x.Item_Number == null && (x.ServiceHeaderId == 0 || x.ServiceHeaderId == null)).ToList();

                if (NullServHead != null && NullServHead.Count > 0)
                {
                    foreach (TEPOServiceHeader ServHead in NullServHead)
                    {
                        Mat_Serv_Seq NullListNonSeq = new Mat_Serv_Seq();
                        TEPOServiceHeader TempServ = new TEPOServiceHeader();
                        NullListNonSeq.ServHead = ServHead;
                        NullListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ServHead.UniqueID).ToList();
                        SeqList.Add(NullListNonSeq);
                    }
                }

                if (NullPOItems.Count > 0)
                {
                    foreach (PurchaseSubItemStructure TempPO in NullPOItems)
                    {
                        Mat_Serv_Seq NullListMatSeq = new Mat_Serv_Seq();
                        NullListMatSeq.ItemS = new List<PurchaseSubItemStructure>();
                        NullListMatSeq.ItemS.Add(TempPO);
                        SeqList.Add(NullListMatSeq);
                    }
                }

            }
            // Which are all the items Null Values
            else
            {
                foreach (TEPOServiceHeader ServHead in PODetails.POServiceHeader)
                {
                    Mat_Serv_Seq ListNonSeq = new Mat_Serv_Seq();
                    TEPOServiceHeader TempServ = new TEPOServiceHeader();
                    ListNonSeq.ServHead = ServHead;
                    ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ServHead.UniqueID).ToList();
                    SeqList.Add(ListNonSeq);
                }
                List<PurchaseSubItemStructure> NULLPO = PODetails.PurchaseItemStructureDetails.Where(x => (x.ServiceHeaderId == null || x.ServiceHeaderId == 0)).ToList();
                if (NULLPO.Count > 0)
                {
                    foreach (PurchaseSubItemStructure TempPo in NULLPO)
                    {
                        Mat_Serv_Seq ListNonSeqMat = new Mat_Serv_Seq();
                        ListNonSeqMat.ItemS = new List<PurchaseSubItemStructure>();
                        ListNonSeqMat.ItemS.Add(TempPo);
                        SeqList.Add(ListNonSeqMat);
                    }
                }
            }
            return SeqList;
        }

        public class PODetails
        {
            public List<TEPOServiceHeader> POServiceHeader { get; set; }
            public List<PurchaseSubItemStructure> PurchaseItemStructureDetails { get; set; }
        }

         public class Mat_Serv_Seq
    {
        public TEPOServiceHeader ServHead { get; set; }

        public List<PurchaseSubItemStructure> ItemS { get; set; }
    }

      

      

        public class POHeadItemStructure
        {
            public int? UniqueID { get; set; }
            public List<POSubItemStructure> ItemStructureList { get; set; }
        }

        public class POHeadItemStructureOrd
        {
            public int? UniqueID { get; set; }
            public List<POSubItemStructureOrd> ItemStructureList { get; set; }
        }

        public class POSubItemStructure
        {
            public String HeadTitle { get; set; }
            public Nullable<int> HeadID { get; set; }
            public String HeadDescription { get; set; }
            public List<PurchaseSubItemStructure> Itemstructure { get; set; }
        }

        public class POSubItemStructureOrd
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
            public int? CreatedByID { get; set; }
            public int LastModifiedByID { get; set; }
            public string CreatedBy { get; set; }
            public string LastModifiedBy { get; set; }
            public Nullable<bool> IsRecordInSAP { get; set; }
            public List<TEPOSpecificationAnnexure> POTechnicalSpecifiactionList { get; set; }
            public List<TEPOServiceBreakUp> ServiceBreakup { get; set; }
           
        }

        public class PurchaseSubItemStructure
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
            public int? CreatedByID { get; set; }
            public int LastModifiedByID { get; set; }
            public string CreatedBy { get; set; }
            public string LastModifiedBy { get; set; }
            public Nullable<int> ServiceHeaderId { get; set; }
            public string ManufactureCode { get; set; }
            public Nullable<bool> IsRecordInSAP { get; set; }
            public List<TEPOSpecificationAnnexure> POTechnicalSpecifiactionList { get; set; }
        }

        public class PurchaseHeaderStructure
        {

            //public int UniqueId { get; set; }
            public string PurchasingOrderNumber { get; set; }
            public string FuguePurchasingOrderNumber { get; set; }
            public string PurchasingDocumentType { get; set; }
            public string VendorAccountNumber { get; set; }
            public DateTime? PurchasingDocumentDate { get; set; }
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
}