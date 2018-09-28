using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace PurchaseOrder.BAL
{
    public class PurchaseOrders
    {
        RecordExceptions except = new RecordExceptions();
        TETechuvaDBContext1 db = new TETechuvaDBContext1();

        public string addTEPOApprovalCondition(TEPOApprovalCondition obj)
        {
            try
            {
                obj.CreatedOn = System.DateTime.Now;
                obj.LastModifiedOn = System.DateTime.Now;
                obj = db.TEPOApprovalConditions.Add(obj);
                db.SaveChanges();
                return obj.UniqueId.ToString();
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return e.Message;
            }
        }

        public string addTEPOApprover(List<TEPOMasterApprover> li, string approverUniqueID)
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
                        db.TEPOMasterApprovers.Add(obj);
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
                JToken type = x.SelectToken("headers");
                string typeStr = type.ToString().ToLower();

                int v = type.Count();
                MaterialDTO materials = new MaterialDTO();
                foreach (JToken jad in type)
                {

                    for (int j = 0; j <= 4; j++)
                    {
                        if (jad["name"].ToString() == "General")
                        {
                            foreach (JToken jack in jad["columns"])
                            {
                                for (int i = 1; i <= 16; i++)
                                {

                                    if (jack["key"].ToString() == "material_name")
                                    {
                                        materials.material_name = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "material_code")
                                    {
                                        materials.MaterialCode = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "short_description")
                                    {
                                        materials.short_description = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "shade_number")
                                    {
                                        materials.shade_number = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "shade_description")
                                    {
                                        materials.shade_description = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "unit_of_measure")
                                    {
                                        materials.unit_of_measure = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "hsn_code")
                                    {
                                        materials.hsn_code = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "sap_code")
                                    {
                                        materials.sap_code = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "wbs_code")
                                    {
                                        materials.wbs_code = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "part_of_edesign")
                                    {
                                        materials.part_of_edesign = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "edesign_description")
                                    {
                                        materials.edesign_description = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "generic")
                                    {
                                        materials.generic = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "manufactured")
                                    {
                                        materials.manufactured = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "can_be_used_as_an_asset")
                                    {
                                        materials.can_be_used_as_an_asset = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "material_status")
                                    {
                                        materials.material_status = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "available_till")
                                    {
                                        materials.available_till = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "Image")
                                    {
                                        if (jack["value"].ToString() != null)
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "url")
                                                {
                                                    materials.Image = im.Value.ToString();
                                                }
                                            }
                                    }
                                }
                            }
                        }
                        if (jad["name"].ToString() == "purchase")
                        {
                            foreach (JToken jack in jad)
                            {
                                for (int i = 1; i <= 10; i++)
                                {
                                    if (jack["key"].ToString() == "qty_evaluation_method")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "name")
                                                {
                                                    materials.qty_evaluation_method = im.Value.ToString();
                                                }
                                            }
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
                                                    materials.general_po_terms = im.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (jack["key"].ToString() == "special_po_terms")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "id")
                                                {
                                                    materials.special_po_terms = im.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    else if (jack["key"].ToString() == "last_purchase_rate")
                                    {
                                        materials.last_purchase_rate = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "weighted_average_purchase_rate")
                                    {
                                        materials.weighted_average_purchase_rate = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "purchase_rate_threshold")
                                    {
                                        materials.purchase_rate_threshold = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "gst_procurement")
                                    {
                                        materials.gst_procurement = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "gst_sales")
                                    {
                                        materials.gst_sales = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "approved_brands")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "id")
                                                {
                                                    materials.approved_brands = im.Value.ToString();
                                                }
                                            }
                                        }

                                    }
                                    else if (jack["key"].ToString() == "approved_vendors")
                                    {
                                        materials.approved_vendors = jack["value"].ToString();
                                    }
                                }

                            }
                        }
                        if (jad["name"].ToString() == "planning")
                        {
                            foreach (JToken jack in jad)
                            {
                                for (int i = 1; i <= 4; i++)
                                {

                                    if (jack["key"].ToString() == "po_lead_time")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "value")
                                                {
                                                    materials.po_lead_time = im.Value.ToString();
                                                }
                                            }
                                        }

                                    }
                                    else if (jack["key"].ToString() == "delivery_lead_time")
                                    {
                                        if (jack["value"].ToString() != null)
                                        {
                                            foreach (JProperty im in jack["value"])
                                            {
                                                if (im.Name == "value")
                                                {
                                                    materials.delivery_lead_time = im.Value.ToString();
                                                }
                                            }
                                        }

                                    }
                                    else if (jack["key"].ToString() == "min_order_qty")
                                    {
                                        materials.min_order_qty = jack["value"].ToString();
                                    }
                                    else if (jack["key"].ToString() == "maintain_lot")
                                    {
                                        materials.maintain_lot = jack["value"].ToString();
                                    }
                                }

                            }
                        }
                    }

                }
                if (materials.MaterialCode != null)
                {
                    list.Add(materials);
                }

            }
            return list;
        }
        
        public int SavePurchaseHeaderStructure(PurchaseHeaderStructure headerStructure)
        {
            TEPurchase_header_structure hsa = new TEPurchase_header_structure();
            int uniqueID = 0;
            TEPurchase_header_structure hs = new TEPurchase_header_structure();
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
            hs.CreatedOn = System.DateTime.Now.ToLongDateString();
            hs.LastModifiedOn= System.DateTime.Now.ToLongDateString();
            db.TEPurchase_header_structure.Add(hs);
            db.SaveChanges();
            if (hs.Uniqueid != 0)
                uniqueID = hs.Uniqueid;

            return uniqueID;
        }

        public int SavePurchaseItemStructure(PurchaseItemStructure itemStructure,int headerStructureID)
        {
            int uniqueID = 0;

            TEPurchase_Item_Structure itms = new TEPurchase_Item_Structure();
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
            itms.LastModifiedOn= System.DateTime.Now.ToString();
            itms.Status = itemStructure.Status;

            //itms.HSN_Code = itemStructure.HSN_Code;
   
            db.TEPurchase_Item_Structure.Add(itms);
            db.SaveChanges();
            if (itms.Uniqueid != 0)
                uniqueID = itms.Uniqueid;

            return uniqueID;
        }

        public int SavePurchaseService(PurchaseService purServ)
        {
            int uniqueID = 0;

            TEPurchase_Service serv = new TEPurchase_Service();
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
            db.TEPurchase_Service.Add(serv);
            db.SaveChanges();
            if (serv.Uniqueid != 0)
                uniqueID = serv.Uniqueid;

            return uniqueID;
        }

        public int PostPOMilestones(TEVendorPaymentMilestone value)
        {
            TEVendorPaymentMilestone result = value;

            if (!(value.UniqueId + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result.ModuleName = "PO";
                result = db.TEVendorPaymentMilestones.Add(value);
            }
            else
            {
                db.TEVendorPaymentMilestones.Attach(value);

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

            TEPurchase_Itemwise itm = new TEPurchase_Itemwise();
            itm.Purchasing_Order_Number = purItemCond.PurchasingOrderNumber;
            itm.Item_Number_of_Purchasing_Document = purItemCond.ItemNumberPurchase;
            itm.Condition_Type = purItemCond.ConditionType;
            itm.Condition_rate = Convert.ToDouble(purItemCond.ConditionRate);
            itm.VendorCode = purItemCond.VendorCode;//vendorCode
            db.TEPurchase_Itemwise.Add(itm);
            db.SaveChanges();
            if (itm.Uniqueid != 0)
                uniqueID = itm.Uniqueid;

            return uniqueID;
        }

        public HttpResponseMessage SavePO(PurchaseHeaderStructure headerStructure,
                                        PurchaseItemStructure itemStructure,
                                        PurchaseService purServ,
                                        TEVendorPaymentMilestone value,
                                        PurchaseItemwise purItemCond)

        {
            bool res = true;
            int headerStructureID = 0, itemID = 0, serviceID = 0,
                PaymntID = 0, ConditionID = 0;
            HttpResponseMessage hrm = new HttpResponseMessage();
            SuccessInfo sinfo = new SuccessInfo();
            FailInfo finfo = new FailInfo();

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
                finfo.errorcode = 1;
                finfo.errormessage = "Failed To Save";
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotAcceptable, Content = new JsonContent(new { info = finfo }) };
            }
        }
        public HttpResponseMessage SavePO(PurchaseHeaderStructure headerStructure,
                                       List<PurchaseItemStructure> itemStructure,
                                       List<PurchaseService> purServ,
                                       List<TEVendorPaymentMilestone> paymntMilestone,
                                       List<PurchaseItemwise> purItemCond)

        {
            bool res = true;
            int headerStructureID = 0, itemID = 0, serviceID = 0,
                PaymntID = 0, ConditionID = 0;
            HttpResponseMessage hrm = new HttpResponseMessage();
            SuccessInfo sinfo = new SuccessInfo();
            FailInfo finfo = new FailInfo();

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
                        foreach (TEVendorPaymentMilestone mstn in paymntMilestone)
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
                finfo.errorcode = 1;
                finfo.errormessage = "Failed To Save";
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotAcceptable, Content = new JsonContent(new { info = finfo }) };
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
                    obj.SequenceId=idd ;
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
                    if (obj.UniqueId > 0) { 
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
        public HttpResponseMessage UpdatePurchaseService(List<TEPurchase_Service> purServ)
        {
            string Message = "";
            if (purServ.Count > 0)
            {
                foreach (TEPurchase_Service purch in purServ)
                {
                    if (purch.Uniqueid > 0) { 
                    var back = db.TEPurchase_Service.Where(a => a.Uniqueid == purch.Uniqueid).SingleOrDefault();
                    purch.CreatedBy = back.CreatedBy;
                    purch.CreatedOn = back.CreatedOn;
                    purch.LastModifiedOn = System.DateTime.Now.ToString();
                        db.Entry(back).CurrentValues.SetValues(purch);
                    }
                    else
                    {
                        purch.CreatedOn = System.DateTime.Now.ToString();
                        purch.LastModifiedOn = System.DateTime.Now.ToString();
                        db.TEPurchase_Service.Add(purch);
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
        public HttpResponseMessage UpdatePurchaseItemStructure(List<TEPurchase_Item_Structure> itemStructure)
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
                foreach (TEPurchase_Item_Structure item in itemStructure)
                {
                    if (item.Uniqueid == 0)
                    {
                        var back = db.TEPurchase_Item_Structure.Where(a => a.Uniqueid == item.Uniqueid).SingleOrDefault();
                        item.CreatedOn = back.CreatedOn;
                        item.CreatedBy = back.CreatedBy;
                        item.LastModifiedOn = System.DateTime.Now.ToString();
                        db.Entry(back).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.CreatedOn = System.DateTime.Now.ToString() ;
                        item.LastModifiedOn = System.DateTime.Now.ToString();
                        db.TEPurchase_Item_Structure.Add(item);
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
    }
}
