using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PO.Models;
using PO.BAL;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace PO.Controllers
{
    public class PurchaseRequestController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        PurchaseOrders POBAL = new PurchaseOrders();

        public PurchaseRequestController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [HttpPost]
        public HttpResponseMessage SavePurchaseRequestBasicInfo(PurchaseRequest purRequest)
        {
            try
            {
                TEPurchaseRequest purObj = new TEPurchaseRequest();
                purObj.Active = true;
                purObj.CreatedBy = purRequest.CreatedByID;
                purObj.CreatedOn = DateTime.Now;
                purObj.LastModifiedBy = purRequest.LastModifiedByID;
                purObj.LastModifiedOn = DateTime.Now;
                purObj.PurchaseRequestDesc = purRequest.PRDescription;
                //purObj.PurchaseRequestId = purRequest.PRID;
                purObj.PurchaseRequestTitle = purRequest.PRTitle;
                purObj.FundCenterId = purRequest.FundCenterID;
                purObj.status = "Draft";
                db.TEPurchaseRequests.Add(purObj);
                db.SaveChanges();
                if (purObj.PurchaseRequestId > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, PRId = purObj.PurchaseRequestId }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        public int GetMat_Serv_Seq(int HeaderStructureid)
        {
            int Max_Seq = 1;
            List<string> materialNumber = new List<string>();
            List<int> materialNumber_int = new List<int>();
            List<string> serviceNumber = new List<string>();
            List<int> serviceNumber_int = new List<int>();
            int Mat_Item_Number = 0;
            int ServHead_Item_Number = 0;
            materialNumber = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == HeaderStructureid).Select(x => x.Item_Number).ToList();
            foreach (string val in materialNumber)
            {
                int temp = Convert.ToInt32(val);
                materialNumber_int.Add(temp);
            }
            if(materialNumber_int.Count() > 0)
                Mat_Item_Number = materialNumber_int.Max();

            serviceNumber = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == HeaderStructureid).Select(x => x.ItemNumber).ToList();
            foreach (string val in serviceNumber)
            {
                int temp = Convert.ToInt32(val);
                serviceNumber_int.Add(temp);
            }
            if(serviceNumber_int.Count() > 0)
                ServHead_Item_Number = serviceNumber_int.Max();

            //String Mat_Item_Number = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == HeaderStructureid).Max(x => x.Item_Number);
            //string ServHead_Item_Number = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == HeaderStructureid).Max(x => x.ItemNumber); ;

            if (Convert.ToInt32(Mat_Item_Number) > Convert.ToInt32(ServHead_Item_Number))
                return Convert.ToInt32(Mat_Item_Number) + 1;
            else if (Convert.ToInt32(ServHead_Item_Number) > Convert.ToInt32(Mat_Item_Number))
                return Convert.ToInt32(ServHead_Item_Number) + 1;
            else if (Convert.ToInt32(ServHead_Item_Number) == Convert.ToInt32(Mat_Item_Number))
                return Convert.ToInt32(ServHead_Item_Number) + 1;

            return Max_Seq;
        }


        [HttpPost]
        public HttpResponseMessage CopyCurrentPRLineItem(CopyPurchaseRequest copyData)
        {
            TEPRItemStructure exitItemLine = new TEPRItemStructure();
            int itemStructureId = 0;
            int serviceheaderId = 0;
            int prHeaderId = 0;
            int maxItemN0 = 0;

            try
            {
                itemStructureId = copyData.PRStructureId;
                exitItemLine = db.TEPRItemStructures.Where(r => r.Uniqueid == itemStructureId && r.IsDeleted == false).FirstOrDefault();
                prHeaderId = Convert.ToInt32(exitItemLine.PurchaseRequestId);
                serviceheaderId = Convert.ToInt32(exitItemLine.ServiceHeaderId);
                maxItemN0 = GetMat_Serv_Seq(prHeaderId);
                new PurchaseOrderBAL().PRItemsClone(serviceheaderId, prHeaderId, prHeaderId, exitItemLine, copyData.LastModifiedByID, true, maxItemN0);
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Copy Current Item";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

            sinfo.errorcode = 0;
            sinfo.errormessage = "Successfully Copy Line Item";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }

        [HttpPost]
        public HttpResponseMessage SavePurchaseRequestDetails(PurchaseRequestItemStructureList itemStructure)
        {
            try
            {
                int saveupdatecount = 1; string PlantStorageCode = string.Empty;
                //var itemduplicatecheck = (from item in db.TEPRItemStructures
                //                          where item.PurchaseRequestId == itemStructure.PR_ID
                //                          && itemStructure.Material_Number.Contains(item.Material_Number) && item.IsDeleted == false
                //                          select item).FirstOrDefault();
                //if (itemduplicatecheck != null)
                //{
                //    sinfo.errorcode = 1;
                //    sinfo.errormessage = "Unable to Save,Material/Service already exist";
                //    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                //}

                int LineItem_Seq = GetMat_Serv_Seq(itemStructure.PR_ID);

                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0" && itemStructure.Material_Number[cnt] != "0")
                    {
                        int tempItemStructureID = 0;
                        tempItemStructureID = itemStructure.ItemStructureID[cnt];
                       
                        TEPRItemStructure itms = new TEPRItemStructure();
                        itms.PurchaseRequestId = itemStructure.PR_ID;
                        itms.ServiceHeaderId = itemStructure.ServiceHeaderId[cnt];
                        itms.Assignment_Category = "";
                        //itms.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                        itms.Item_Number = itemStructure.Item_Number;
                        itms.Item_Category = itemStructure.Item_Category;
                        itms.Material_Number = itemStructure.Material_Number[cnt];//materialcode
                        itms.Short_Text = itemStructure.Short_Text[cnt];//shortDescription
                        itms.Long_Text = itemStructure.Long_Text[cnt];
                        itms.ItemType = itemStructure.ItemType[cnt];
                        itms.HSNCode = itemStructure.HSN_Code[cnt];
                        itms.WBSElementCode = itemStructure.WBSElementCode[cnt];
                        itms.Brand = itemStructure.Brand[cnt];
                        itms.Unit_Measure = itemStructure.Unit_Measure[cnt];//Unit                           
                                                                            //Levels Of material/Srvice
                        itms.Level1 = itemStructure.Level1[cnt];
                        itms.Level2 = itemStructure.Level2[cnt];
                        itms.Level3 = itemStructure.Level3[cnt];
                        itms.Level4 = itemStructure.Level4[cnt];
                        itms.Level5 = itemStructure.Level5[cnt];
                        itms.Level6 = itemStructure.Level6[cnt];
                        itms.Level7 = itemStructure.Level7[cnt];
                        //begining of calculations
                        itms.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                        itms.Balance_Qty = Convert.ToDecimal(itms.Order_Qty);
                        itms.OnHold_Qty = 0;

                        itms.Rate = itemStructure.Rate[cnt];
                        itms.TotalAmount = Convert.ToDecimal(itms.Order_Qty) * itms.Rate;

                        itms.IGSTRate = itemStructure.IGSTRate[cnt];
                        itms.IGSTAmount = (itms.TotalAmount * itms.IGSTRate) / 100;

                        itms.CGSTRate = itemStructure.CGSTRate[cnt];
                        itms.CGSTAmount = (itms.TotalAmount * itms.CGSTRate) / 100;

                        itms.SGSTRate = itemStructure.SGSTRate[cnt];
                        itms.SGSTAmount = (itms.TotalAmount * itms.SGSTRate) / 100;

                        itms.TotalTaxAmount = itemStructure.TotalTaxAmount[cnt];
                        itms.GrossAmount = itemStructure.GrossAmount[cnt];
                        //ending of calculations

                        itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                        itms.Material_Group = itemStructure.Material_Group;
                        itms.Plant = itemStructure.Plant;//Plant Code
                        itms.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code            
                        itms.CreatedOn = System.DateTime.Now;
                        //  itms.CreatedBy = itemStructure.CreatedBy;
                        itms.CreatedBy = Convert.ToInt32(itemStructure.LastModifiedBy);
                        itms.LastModifiedBy = Convert.ToInt32(itemStructure.LastModifiedBy);
                        itms.LastModifiedOn = System.DateTime.Now;
                        //  itms.LastModifiedBy = itemStructure.LastModifiedBy;
                        itms.Status = itemStructure.Status;
                        itms.IsDeleted = false;
                        if (itms.ItemType == "MaterialOrder")
                        {
                            itms.Item_Number = LineItem_Seq.ToString();
                            LineItem_Seq++;
                            if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                            {
                                itms.GLAccountNo = "110600";
                            }
                            else
                            {
                                itms.GLAccountNo = "110950";
                            }
                        }
                        if (itms.ItemType == "ServiceOrder")
                        {
                            int ServID = Convert.ToInt32(itemStructure.ServiceHeaderId[cnt]);
                            TEPRServiceHeader HeadServ = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == itemStructure.PR_ID && x.UniqueID == ServID).FirstOrDefault();
                            
                            var newt = HeadServ.ItemNumber;
                            if(HeadServ != null)
                                itms.Item_Number = HeadServ.ItemNumber;
                            if (String.IsNullOrEmpty(HeadServ.ItemNumber))
                            {
                                HeadServ.ItemNumber = LineItem_Seq.ToString();
                                itms.Item_Number = LineItem_Seq.ToString();
                                LineItem_Seq++;
                                db.Entry(HeadServ).CurrentValues.SetValues(HeadServ);
                                db.SaveChanges();
                            }
                            else
                                itms.Item_Number = HeadServ.ItemNumber.ToString();

                            itms.GLAccountNo = "515200";
                        }
                        if (itms.ItemType == "ExpenseOrder")
                        {
                            itms.GLAccountNo = "529900";
                        }
                        db.TEPRItemStructures.Add(itms);
                        db.SaveChanges();
                        tempItemStructureID = itms.Uniqueid;

                        if (itms.ItemType == "MaterialOrder")
                            TEPurchase_SaveMaterialSpecifications(itms.Material_Number, itemStructure.PR_ID, tempItemStructureID, Convert.ToInt32(itemStructure.LastModifiedBy));

                        saveupdatecount++;
                    }
                }
                if (saveupdatecount > 1)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        public void TEPurchase_SaveMaterialSpecifications(string materialcode, int headStructId, int itemStructId, int lastmodifiedby)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                List<columnsStruct> specColumnsList = new List<columnsStruct>();
                ObjFromComponentLib myList = new ObjFromComponentLib();
                //ObjFromComponentLib PO_SpecialTC = new ObjFromComponentLib();

                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // https://localhost:44380/materials/MFR023128?dataType=true
                var url = ComponentLibraryHost + "/materials/" + materialcode + "?dataType=true";

                var tokenResponse = client.GetAsync(url).Result;

                var onjs = tokenResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                myList = JsonConvert.DeserializeObject<ObjFromComponentLib>(onjs.Result, settings);

                if (myList != null)
                {
                    try
                    {
                        specColumnsList = myList.headers.Find(a => a.key.Equals("specifications")).columns.ToList();
                
                    }
                    catch (Exception ex)
                    {
                        ExceptionObj.RecordUnHandledException(ex);
                    }

                    foreach (var annex in specColumnsList)
                    {
                        TEPRSpecificationAnnexure Spc_annex = new TEPRSpecificationAnnexure();
                        Spc_annex.Name = annex.name;
                        Spc_annex.MaterialGroup = myList.group;
                        Spc_annex.CmpLibId = myList.id;
                        Spc_annex.MaterialCode = materialcode;
                        Spc_annex.PRHeaderStructureId = headStructId;
                        Spc_annex.PRItemStructureId = itemStructId;
                        Spc_annex.IsDeleted = false;
                        if (annex.dataType.subType != null)
                        {
                            SpecAnnexValue valObj = new SpecAnnexValue();
                            if (annex.value != null)
                            {
                                try
                                {
                                    valObj = JsonConvert.DeserializeObject<SpecAnnexValue>(annex.value.ToString());
                                    Spc_annex.Value = valObj.value;
                                    Spc_annex.ValueType = valObj.type;
                                }
                                catch (Exception ex)
                                {
                                    ExceptionObj.RecordUnHandledException(ex);

                                }
                            }
                        }
                        else
                        {
                            if (annex.value != null)
                                Spc_annex.Value = annex.value.ToString();
                        }
                        Spc_annex.LastModifiedBy = lastmodifiedby;
                        Spc_annex.LastModifiedOn = DateTime.Now;
                        db.TEPRSpecificationAnnexures.Add(Spc_annex);
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);

            }
        }

        public Token generateToken_ComponentLibrary()
        {
            Token tokenObj = new Token();
            try
            {
                HttpClient client = new HttpClient();
                string baseAddress = WebConfigurationManager.AppSettings["baseAddress"];
                string grant_type = WebConfigurationManager.AppSettings["grant_type"];
                string client_id = WebConfigurationManager.AppSettings["client_id"];
                string resource = WebConfigurationManager.AppSettings["resource"];
                string client_secret = WebConfigurationManager.AppSettings["client_secret"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];
                var form = new Dictionary<string, string>
                {
                   { "grant_type",grant_type},
                   {"client_id",client_id},
                   {"resource",resource},
                   { "client_secret",client_secret},
                };
                var tokenResponse = client.PostAsync(baseAddress + "/" + tenant + "/oauth2/token", new FormUrlEncodedContent(form)).Result;
                if (tokenResponse.StatusCode.Equals(200) || tokenResponse.ReasonPhrase.Equals("OK"))
                {
                    tokenObj = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }

            return tokenObj;
        }

        [HttpPost]
        public HttpResponseMessage GetPurchaseItemsByPOHeaderId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int phsId = json["POHeaderStructureID"].ToObject<int>();
                var taxDtls = GetPurchaseItemStructureByPOStructureId(phsId);

                if (taxDtls != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        public List<PurchaseItemStructure> GetPurchaseItemStructureByPOStructureId(int purchaseHeaderStructureId)
        {
            List<PurchaseItemStructure> purItemstructDtls = (from purHead in db.TEPOHeaderStructures
                                                             join itm in db.TEPRItemStructures on purHead.Uniqueid equals itm.POStructureId
                                                             where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                                             && itm.IsDeleted == false && purHead.IsDeleted == false
                                                             select new PurchaseItemStructure
                                                             {
                                                                 ItemStructureID = itm.Uniqueid,
                                                                 HeaderStructureID = itm.POStructureId,
                                                                 ItemType = itm.ItemType,
                                                                 //PurchasingOrderNumber = itm.Purchasing_Order_Number,
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
                                                                 IGSTAmount = itm.IGSTAmount,
                                                                 CGSTRate = itm.CGSTRate,
                                                                 CGSTAmount = itm.CGSTAmount,
                                                                 SGSTRate = itm.SGSTRate,
                                                                 SGSTAmount = itm.SGSTAmount,
                                                                 TotalTaxAmount = itm.TotalTaxAmount,
                                                                 GrossAmount = itm.GrossAmount,
                                                                 CreatedByID = itm.CreatedBy,
                                                                 LastModifiedByID = (int)itm.LastModifiedBy
                                                             }).OrderByDescending(a => a.ItemStructureID).ToList();

            return purItemstructDtls;
        }

        [HttpPost]
        public HttpResponseMessage GetPurchaseItemsByPRId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int prId = json["PurchaseRequestId"].ToObject<int>();
                var taxDtls = GetPurchaseItemStructureByPRId(prId);

                if (taxDtls != null && taxDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }


        //Start of Service Header

        [HttpPost]
        public HttpResponseMessage GetServiceHeadItemsbyHeadStructID(TEPRServiceHeader PO_StructID)
        {

            try
            {
                if (PO_StructID != null)
                {

                    var HeadData = this.PRSeq_Mat_Service(Convert.ToInt32(PO_StructID.PRHeaderStructureid));
                    sinfo.errorcode = 0;
                    // sinfo.totalrecords = HeadData;
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = HeadData }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex); 
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage AddServiceHeader(TEPRServiceHeader ItemsHead)
        {
            if (ItemsHead != null)
            {
                ItemsHead.IsDeleted = false;

                ItemsHead.LastModifiedBy = ItemsHead.LastModifiedBy;
                ItemsHead.LastModifiedOn = DateTime.Now;
                db.TEPRServiceHeaders.Add(ItemsHead);
                db.SaveChanges();
                List<TEPRServiceHeader> ItemHeadList = new List<Models.TEPRServiceHeader>();
                ItemHeadList.Add(ItemsHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Saved";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = ItemHeadList }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateServiceHeader(TEPRServiceHeader updateItemHead)
        {
            if (updateItemHead != null)
            {
                List<TEPRServiceHeader> listItemHeader = new List<TEPRServiceHeader>();
                TEPRServiceHeader itemHead = new TEPRServiceHeader();
                itemHead = db.TEPRServiceHeaders.Where(x => x.UniqueID == updateItemHead.UniqueID && x.IsDeleted == false).FirstOrDefault();
                itemHead.Title = updateItemHead.Title;
                itemHead.Description = updateItemHead.Description;
                itemHead.IsDeleted = false;
                itemHead.LastModifiedOn = updateItemHead.LastModifiedOn;
                db.Entry(itemHead).CurrentValues.SetValues(itemHead);
                db.SaveChanges();
                listItemHeader.Add(updateItemHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Header Updated";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = listItemHeader }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteServiceHeader(TEPRServiceHeader updateItemHead)
        {
            if (updateItemHead != null)
            {
                TEPRServiceHeader ItemHead = db.TEPRServiceHeaders.Where(x => x.UniqueID == updateItemHead.UniqueID && x.IsDeleted == false).FirstOrDefault();
                List<TEPRItemStructure> POItemStructList = db.TEPRItemStructures.Where(x => x.ServiceHeaderId == updateItemHead.UniqueID && x.PurchaseRequestId == updateItemHead.PRHeaderStructureid && x.IsDeleted == false).ToList();
                foreach (TEPRItemStructure POItemStruct in POItemStructList)
                {
                    POItemStruct.IsDeleted = true;
                    db.Entry(POItemStruct).CurrentValues.SetValues(POItemStruct);
                    db.SaveChanges();
                }

                ItemHead.IsDeleted = true;
                ItemHead.LastModifiedOn = DateTime.Now;
                db.Entry(ItemHead).CurrentValues.SetValues(ItemHead);
                db.SaveChanges();

                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Deleted";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

     

        //End of Service Header


        public List<PurchaseItemStructure> GetPurchaseItemStructureByPRId(int purchaseRequestId)
        {
            List<PurchaseItemStructure> purItemstructDtls = (from itm in db.TEPRItemStructures 
                                                             where itm.PurchaseRequestId == purchaseRequestId && itm.IsDeleted == false
                                                             && itm.IsDeleted == false
                                                             select new PurchaseItemStructure
                                                             {
                                                                 ItemStructureID = itm.Uniqueid,
                                                                 HeaderStructureID = itm.POStructureId,
                                                                 ItemType = itm.ItemType,
                                                                 //PurchasingOrderNumber = itm.Purchasing_Order_Number,
                                                                 Item_Number = itm.Item_Number,
                                                                 Assignment_Category = itm.Assignment_Category,
                                                                 Item_Category = itm.Item_Category,
                                                                 Material_Number = itm.Material_Number,
                                                                 Short_Text = itm.Short_Text,
                                                                 Long_Text = itm.Long_Text,
                                                                 Line_item = itm.Line_item,
                                                                 Order_Qty = itm.Order_Qty,
                                                                 Balance_Qty = itm.Balance_Qty,
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
                                                                 IGSTAmount = itm.IGSTAmount,
                                                                 CGSTRate = itm.CGSTRate,
                                                                 CGSTAmount = itm.CGSTAmount,
                                                                 SGSTRate = itm.SGSTRate,
                                                                 SGSTAmount = itm.SGSTAmount,
                                                                 TotalTaxAmount = itm.TotalTaxAmount,
                                                                 GrossAmount = itm.GrossAmount,
                                                                 CreatedByID = itm.CreatedBy,
                                                                 LastModifiedByID = (int)itm.LastModifiedBy
                                                             }).OrderBy(a => a.ItemStructureID).ToList();

            return purItemstructDtls;
        }

        [HttpPost]
        public HttpResponseMessage GetPRDetailsById(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int purReqId = json["PRId"].ToObject<int>();

                var prDetails = (from pr in db.TEPurchaseRequests
                                     join tpfc in db.TEPOFundCenters on pr.FundCenterId equals tpfc.Uniqueid
                                     join created in db.UserProfiles on pr.CreatedBy equals created.UserId
                                     join apprv in db.UserProfiles on pr.ApprovedBy equals apprv.UserId into tempApprover
                                     from approver in tempApprover.DefaultIfEmpty()
                                     where pr.PurchaseRequestId == purReqId && pr.Active == true
                                     select new
                                     {
                                         pr.PurchaseRequestId,
                                         pr.FundCenterId,
                                         tpfc.FundCenter_Code,
                                         tpfc.FundCenter_Description,
                                         tpfc.FundCenter_Owner,
                                         tpfc.ProjectCode,
                                         tpfc.ProjectName,
                                         pr.PurchaseRequestTitle,
                                         pr.PurchaseRequestDesc,
                                         pr.status,
                                         CreatedByID = pr.CreatedBy,
                                         pr.CreatedOn,
                                         CreatedBy = created.CallName,
                                         ApprovedBy = approver.CallName,
                                         ApprovedByID = pr.ApprovedBy,
                                         ApprovedOn = pr.ApprovedOn
                                     }).FirstOrDefault();

                if (prDetails != null && prDetails.PurchaseRequestId > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
                else
                {
                    prDetails = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //My PR Call
        [HttpPost]
        public HttpResponseMessage GetAllApprovedPRDetailsByLoginId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage(); string SearchKey = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            try
            {
                int loginId = json["LoginId"].ToObject<int>();
                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == loginId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Don't have PR Access";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = string.Empty, info = sinfo }) };
                }

                if (json["FilterBy"] != null)
                    SearchKey = json["FilterBy"].ToObject<string>();
                if (json["pageNumber"] != null)
                    pagenumber = json["pageNumber"].ToObject<int>();
                if (json["pagePerCount"] != null)
                    pagepercount = json["pagePerCount"].ToObject<int>();
                List<PRDetailsForSearch> prDetails = null;
                List<TEPurchaseHomeModel> posearchList = new List<TEPurchaseHomeModel>();
                if (!string.IsNullOrEmpty(SearchKey))
                {
                    prDetails = (from pr in db.TEPurchaseRequests
                                 join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                 from lftitem in g.DefaultIfEmpty()
                                 join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                 join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                 from createduser in tempcreator.DefaultIfEmpty()
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                 from approveduser in tempapprover.DefaultIfEmpty()
                                 where (pr.Active == true && pr.CreatedBy == loginId && pr.status == "Approved")
                                 &&
                                 (
                                 lftitem.Material_Number.Contains(SearchKey) ||
                                 lftitem.Short_Text.Contains(SearchKey) ||
                                 lftitem.Long_Text.Contains(SearchKey) ||
                                 lftitem.WBSElementCode.Contains(SearchKey) ||
                                 lftitem.Level1.Contains(SearchKey) ||
                                 lftitem.Level2.Contains(SearchKey) ||
                                 lftitem.Level3.Contains(SearchKey) ||
                                 lftitem.Level4.Contains(SearchKey) ||
                                 pr.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                 pr.PurchaseRequestTitle.Contains(SearchKey) ||
                                 pr.status.Contains(SearchKey) ||
                                 pr.PurchaseRequestDesc.Contains(SearchKey) ||
                                 createduser.CallName.Contains(SearchKey) ||
                                 approveduser.CallName.Contains(SearchKey) ||
                                 fund.FundCenter_Code.Contains(SearchKey) ||
                                 fund.FundCenter_Owner.Contains(SearchKey) ||
                                 fund.FundCenter_Description.Contains(SearchKey)
                                )
                                 select new PRDetailsForSearch
                                 {
                                     PurchaseRequestId = pr.PurchaseRequestId,
                                     FundCenterId = pr.FundCenterId,
                                     FundCenter_Code = fund.FundCenter_Code,
                                     FundCenter_Description = fund.FundCenter_Description,
                                     FundCenter_Owner = fund.FundCenter_Owner,
                                     PurchaseRequestTitle = pr.PurchaseRequestTitle,
                                     PurchaseRequestDesc = pr.PurchaseRequestDesc,
                                     status = pr.status,
                                     POStatus = pr.POStatus,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedBy = createduser.CallName,
                                     ApprovedBy = approveduser.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();

                    if (prDetails != null && prDetails.Count > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        var finalResult = prDetails.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = prDetails.Count;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                        sinfo.errormessage = "Successfully Got Records";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                    }
                    else
                    {
                        prDetails = null;
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Record Found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                    }
                }
                else { 
                var prDetailsList = (from pr in db.TEPurchaseRequests
                                 join tpfc in db.TEPOFundCenters on pr.FundCenterId equals tpfc.Uniqueid
                                 join created in db.UserProfiles on pr.CreatedBy equals created.UserId
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId
                                 where pr.CreatedBy == loginId && pr.Active == true && pr.status == "Approved"
                                 select new
                                 {
                                     pr.PurchaseRequestId,
                                     pr.FundCenterId,
                                     tpfc.FundCenter_Code,
                                     tpfc.FundCenter_Description,
                                     tpfc.FundCenter_Owner,
                                     pr.PurchaseRequestTitle,
                                     pr.PurchaseRequestDesc,
                                     pr.status,
                                     CreatedBy = created.CallName,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedOn = pr.CreatedOn,
                                     ApprovedBy = approver.CallName,
                                     ApprovedByID= pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn,
                                     pr.POStatus
                                 }).OrderByDescending(a => a.PurchaseRequestId).ToList();

                if (prDetailsList != null && prDetailsList.Count > 0)
                {
                    //sinfo.errorcode = 0;
                    //sinfo.errormessage = "Successfully Got Records";
                    //return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };

                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    var finalResult = prDetailsList.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = prDetailsList.Count;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = pagenumber.ToString();
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                }
                else
                {
                    prDetailsList = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetailsList }) };
                }
            }
                }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllPRDetails(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            int pagenumber = 0, pagepercount = 0;
            try
            {
                if (json["pageNumber"] != null)
                    pagenumber = json["pageNumber"].ToObject<int>();
                if (json["pagePerCount"] != null)
                    pagepercount = json["pagePerCount"].ToObject<int>();

                var prDetails = (from pr in db.TEPurchaseRequests
                                 join tpfc in db.TEPOFundCenters on pr.FundCenterId equals tpfc.Uniqueid
                                 join created in db.UserProfiles on pr.CreatedBy equals created.UserId
                                 join apprv in db.UserProfiles on pr.ApprovedBy equals apprv.UserId into tempApprover
                                 from approver in tempApprover.DefaultIfEmpty()
                                 where pr.Active == true
                                 select new
                                 {
                                     pr.PurchaseRequestId,
                                     pr.FundCenterId,
                                     tpfc.FundCenter_Code,
                                     tpfc.FundCenter_Description,
                                     tpfc.FundCenter_Owner,
                                     pr.PurchaseRequestTitle,
                                     pr.PurchaseRequestDesc,
                                     pr.status,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedBy = created.CallName,
                                     ApprovedBy = approver.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).OrderByDescending(a => a.PurchaseRequestId).ToList();

                if (prDetails != null && prDetails.Count > 0)
                {
                    //sinfo.errorcode = 0;
                    //sinfo.errormessage = "Successfully Got Records";
                    //return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };

                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    var finalResult = prDetails.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = prDetails.Count;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = pagenumber.ToString();
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                }
                else
                {
                    prDetails = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllDraftPRDetailsByLoginId(JObject json)
        {
            int pagenumber = 0, pagepercount = 0; string SearchKey = string.Empty;
            try
            {
                int loginId = json["LoginId"].ToObject<int>();

                if (json["FilterBy"] != null)
                    SearchKey = json["FilterBy"].ToObject<string>();
                if (json["pageNumber"] != null)
                    pagenumber = json["pageNumber"].ToObject<int>();
                if (json["pagePerCount"] != null)
                    pagepercount = json["pagePerCount"].ToObject<int>();

                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == loginId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Don't have PR Access";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = string.Empty, info = sinfo }) };
                }

                List<PRDetailsForSearch> prDetails = null;
                List<TEPurchaseHomeModel> posearchList = new List<TEPurchaseHomeModel>();
                if (!string.IsNullOrEmpty(SearchKey))
                {
                    prDetails = (from pr in db.TEPurchaseRequests
                                 join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                 from lftitem in g.DefaultIfEmpty()
                                 join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                 join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                 from createduser in tempcreator.DefaultIfEmpty()
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                 from approveduser in tempapprover.DefaultIfEmpty()
                                 where (pr.Active == true && pr.CreatedBy == loginId && pr.status == "Draft")
                                 &&
                                 (
                                 lftitem.Material_Number.Contains(SearchKey) ||
                                 lftitem.Short_Text.Contains(SearchKey) ||
                                 lftitem.Long_Text.Contains(SearchKey) ||
                                 lftitem.WBSElementCode.Contains(SearchKey) || 
                                 lftitem.Level1.Contains(SearchKey) ||
                                 lftitem.Level2.Contains(SearchKey) ||
                                 lftitem.Level3.Contains(SearchKey) ||
                                 lftitem.Level4.Contains(SearchKey) ||
                                 pr.status.Contains(SearchKey) ||
                                 pr.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                 pr.PurchaseRequestTitle.Contains(SearchKey) ||
                                 pr.PurchaseRequestDesc.Contains(SearchKey) ||
                                 createduser.CallName.Contains(SearchKey) ||
                                 approveduser.CallName.Contains(SearchKey) ||
                                 fund.FundCenter_Code.Contains(SearchKey) ||
                                 fund.FundCenter_Owner.Contains(SearchKey) ||
                                 fund.FundCenter_Description.Contains(SearchKey)
                                )
                                 select new PRDetailsForSearch
                                 {
                                     PurchaseRequestId = pr.PurchaseRequestId,
                                     FundCenterId = pr.FundCenterId,
                                     FundCenter_Code = fund.FundCenter_Code,
                                     FundCenter_Description = fund.FundCenter_Description,
                                     FundCenter_Owner = fund.FundCenter_Owner,
                                     PurchaseRequestTitle = pr.PurchaseRequestTitle,
                                     PurchaseRequestDesc = pr.PurchaseRequestDesc,
                                     status = pr.status,
                                     POStatus = pr.POStatus,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedBy = createduser.CallName,
                                     ApprovedBy = approveduser.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();

                    if (prDetails != null && prDetails.Count > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        var finalResult = prDetails.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = prDetails.Count;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                        sinfo.errormessage = "Successfully Got Records";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                    }
                    else
                    {
                        prDetails = null;
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Record Found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                    }
                }
                else
                {
                    var prDetailsList = (from pr in db.TEPurchaseRequests
                                     join tpfc in db.TEPOFundCenters on pr.FundCenterId equals tpfc.Uniqueid
                                     join created in db.UserProfiles on pr.CreatedBy equals created.UserId
                                     where pr.CreatedBy == loginId && pr.Active == true && pr.status == "Draft"
                                     select new
                                     {
                                         pr.PurchaseRequestId,
                                         pr.FundCenterId,
                                         tpfc.FundCenter_Code,
                                         tpfc.FundCenter_Description,
                                         tpfc.FundCenter_Owner,
                                         pr.PurchaseRequestTitle,
                                         pr.PurchaseRequestDesc,
                                         pr.status,
                                         CreatedBy = created.CallName,
                                         CreatedByID = pr.CreatedBy,
                                         CreatedOn = pr.CreatedOn
                                     }).OrderByDescending(a => a.PurchaseRequestId).ToList();

                    if (prDetailsList != null && prDetailsList.Count > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        var finalResult = prDetailsList.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = prDetailsList.Count;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                        sinfo.errormessage = "Successfully Got Records";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                    }
                    else
                    {
                        prDetailsList = null;
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Record Found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetailsList }) };
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage TEPRSearchForSearchTab(JObject json)
        {
            int loginId = 0;
            string SearchKey = string.Empty;
            if (json["LoginId"] != null)
                loginId = json["LoginId"].ToObject<int>();
            if (json["Search"] != null)
                SearchKey = json["Search"].ToObject<string>();

            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<PRDetailsForSearch> prDetails = null;
            try
            {
                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == loginId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    finfo.errorcode = 0;
                    finfo.errormessage = "Don't have PR Access";
                    finfo.listcount = 0;
                    hrm.Content = new JsonContent(new
                    {
                        result = string.Empty,
                        info = finfo
                    });
                    return hrm;
                }

                List<TEPurchaseHomeModel> posearchList = new List<TEPurchaseHomeModel>();
                var PRAdminroleCheck = (from userrole in db.webpages_UsersInRoles
                                        join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                        where userrole.UserId == loginId && webrole.RoleName.ToLower() == "pr admin"
                                        select new { webrole.RoleName }).FirstOrDefault();

                //Returning all the PR's information because he is Having PR Admin Role
                if (PRAdminroleCheck != null)
                {
                    prDetails = (from pr in db.TEPurchaseRequests
                                 join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                 from lftitem in g.DefaultIfEmpty()
                                 join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                 join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                 from createduser in tempcreator.DefaultIfEmpty()
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                 from approveduser in tempapprover.DefaultIfEmpty()
                                 where (pr.Active == true)
                                 &&
                                 (
                                 lftitem.Material_Number.Contains(SearchKey) ||
                                 lftitem.Short_Text.Contains(SearchKey) ||
                                 lftitem.Long_Text.Contains(SearchKey) ||
                                 lftitem.Level1.Contains(SearchKey) ||
                                 lftitem.Level2.Contains(SearchKey) ||
                                 lftitem.Level3.Contains(SearchKey) ||
                                 lftitem.Level4.Contains(SearchKey) ||
                                 pr.status.Contains(SearchKey) ||
                                 lftitem.WBSElementCode.Contains(SearchKey) ||
                                 pr.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                 pr.PurchaseRequestTitle.Contains(SearchKey) ||
                                 pr.PurchaseRequestDesc.Contains(SearchKey) ||
                                 createduser.CallName.Contains(SearchKey) ||
                                 approveduser.CallName.Contains(SearchKey) ||
                                 fund.FundCenter_Code.Contains(SearchKey) ||
                                 fund.FundCenter_Owner.Contains(SearchKey) ||
                                 fund.FundCenter_Description.Contains(SearchKey)
                                )
                                 select new PRDetailsForSearch
                                 {
                                     PurchaseRequestId = pr.PurchaseRequestId,
                                     FundCenterId = pr.FundCenterId,
                                     FundCenter_Code = fund.FundCenter_Code,
                                     FundCenter_Description = fund.FundCenter_Description,
                                     FundCenter_Owner = fund.FundCenter_Owner,
                                     PurchaseRequestTitle = pr.PurchaseRequestTitle,
                                     PurchaseRequestDesc = pr.PurchaseRequestDesc,
                                     status = pr.status,
                                     POStatus = pr.POStatus,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedBy = createduser.CallName,
                                     ApprovedBy = approveduser.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();
                }
                //Returning PO informations based on login user associated to the POs as a approver
                else
                {
                    prDetails = (from pr in db.TEPurchaseRequests
                                 join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                 from lftitem in g.DefaultIfEmpty()
                                 join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                 join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                 from createduser in tempcreator.DefaultIfEmpty()
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                 from approveduser in tempapprover.DefaultIfEmpty()
                                 where (pr.Active == true && pr.CreatedBy == loginId)
                                 &&
                                 (
                                 lftitem.Material_Number.Contains(SearchKey) ||
                                 lftitem.Short_Text.Contains(SearchKey) ||
                                 lftitem.Long_Text.Contains(SearchKey) ||
                                 lftitem.WBSElementCode.Contains(SearchKey) ||
                                 pr.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                 pr.PurchaseRequestTitle.Contains(SearchKey) ||
                                 pr.PurchaseRequestDesc.Contains(SearchKey) ||
                                 createduser.CallName.Contains(SearchKey) ||
                                 approveduser.CallName.Contains(SearchKey) ||
                                 fund.FundCenter_Code.Contains(SearchKey) ||
                                 fund.FundCenter_Owner.Contains(SearchKey) ||
                                 fund.FundCenter_Description.Contains(SearchKey)
                                )
                                 select new PRDetailsForSearch
                                 {
                                     PurchaseRequestId = pr.PurchaseRequestId,
                                     FundCenterId = pr.FundCenterId,
                                     FundCenter_Code = fund.FundCenter_Code,
                                     FundCenter_Description = fund.FundCenter_Description,
                                     FundCenter_Owner = fund.FundCenter_Owner,
                                     PurchaseRequestTitle = pr.PurchaseRequestTitle,
                                     PurchaseRequestDesc = pr.PurchaseRequestDesc,
                                     status = pr.status,
                                     POStatus = pr.POStatus,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedBy = createduser.CallName,
                                     ApprovedBy = approveduser.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();
                }
                if (prDetails != null && prDetails.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
                else
                {
                    prDetails = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePurchaseRequestBasicInfo(PurchaseRequest purRequest)
        {
            try
            {
                if (purRequest.PRID > 0)
                {
                    TEPurchaseRequest purObj = db.TEPurchaseRequests.Where(x => x.PurchaseRequestId == purRequest.PRID && x.Active == true).FirstOrDefault();

                    if (purObj != null && purObj.PurchaseRequestId > 0)
                    {
                        if (purObj.FundCenterId != purRequest.FundCenterID)
                        {
                            List<TEPRItemStructure> prItems = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == purRequest.PRID 
                                                                    && x.IsDeleted == false).ToList();
                            if (prItems != null && prItems.Count > 0)
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Failed to Update. To change the Fund Center please delete the Purchase Details first.";
                                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                            }
                            else
                            {
                                purObj.FundCenterId = purRequest.FundCenterID;
                                purObj.PurchaseRequestTitle = purRequest.PRTitle;
                                purObj.PurchaseRequestDesc = purRequest.PRDescription;
                                purObj.LastModifiedBy = purRequest.LastModifiedByID;
                                purObj.LastModifiedOn = DateTime.Now;
                                db.Entry(purObj).CurrentValues.SetValues(purObj);
                                db.SaveChanges();

                                sinfo.errorcode = 0;
                                sinfo.errormessage = "Successfully Updated";
                                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                            }
                        }
                        else
                        {
                            purObj.FundCenterId = purRequest.FundCenterID;
                            purObj.PurchaseRequestTitle = purRequest.PRTitle;
                            purObj.PurchaseRequestDesc = purRequest.PRDescription;
                            purObj.LastModifiedBy = purRequest.LastModifiedByID;
                            purObj.LastModifiedOn = DateTime.Now;
                            db.Entry(purObj).CurrentValues.SetValues(purObj);
                            db.SaveChanges();

                            sinfo.errorcode = 0;
                            sinfo.errormessage = "Successfully Updated";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to update. PR details not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to update. PR details not found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo}) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage ApprovePurchaseRequestBasicInfo(JObject json)
        {
            try
            {
                int purReqId = json["PRId"].ToObject<int>();
                int LastModifiedByID = json["LastModifiedByID"].ToObject<int>();

                TEPurchaseRequest purObj = db.TEPurchaseRequests.Where(x => x.PurchaseRequestId == purReqId && x.Active == true).FirstOrDefault();

                if (purObj != null && purObj.PurchaseRequestId > 0)
                {
                    List<TEPRItemStructure> prItemList = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == purReqId && x.IsDeleted == false).ToList();
                    if (prItemList != null && prItemList.Count > 0)
                    {
                        string materilaCode = IsAnyPRItemDontHaveDeliverSchedule(prItemList);
                        if (!string.IsNullOrEmpty(materilaCode))
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Submit for PO creation failed. Delivery Schedule Details not available for Material Number: " + materilaCode;
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                        else
                        {
                            string mismatchMaterilaCode = IsAnyPRItemQtyIsNotMatchingWithSumOfQtyOfDeliverSchedule(prItemList);
                            if (!string.IsNullOrEmpty(mismatchMaterilaCode))
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Submit for PO creation failed. Sum of Delivery Schedule Quantity is not matching with Item Quantity for Material Number: " + mismatchMaterilaCode;
                                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                            }
                            else
                            {
                                if (purObj.status == "Draft")
                                {
                                    purObj.status = "Approved";
                                    purObj.ApprovedBy = LastModifiedByID;
                                    purObj.ApprovedOn = DateTime.Now;
                                    purObj.LastModifiedBy = LastModifiedByID;
                                    purObj.LastModifiedOn = DateTime.Now;

                                    db.Entry(purObj).CurrentValues.SetValues(purObj);
                                    db.SaveChanges();

                                    sinfo.errorcode = 0;
                                    sinfo.errormessage = "Successfully Submitted for PO creation";
                                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                                }
                                else if (purObj.status == "Approved")
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "PR is already Submitted for PO creation";
                                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                                }
                                else
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Submit for PO creation failed. Invalid PR status: " + purObj.status;
                                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                        }
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Submit for PO creation failed. Purchase Details not available for this PR.";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Submit for PO creation failed. PR Details not Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Submit for PO creation";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        private string IsAnyPRItemDontHaveDeliverSchedule(List<TEPRItemStructure> prItemList)
        {
            string materialCode = string.Empty;
            foreach (TEPRItemStructure pr in prItemList)
            {
                var deliverySchedules = (from delv in db.TEPRItemsDeliverySchedules
                                         where delv.IsDeleted == false && delv.PRItemStructureId == pr.Uniqueid
                                         select new
                                         {
                                             delv.DeliveryScheduleId,
                                             delv.DeliveryQty,
                                             delv.DeliveryDate,
                                             delv.Remarks
                                         }).OrderBy(a => a.DeliveryScheduleId).ToList();
                if (deliverySchedules != null && deliverySchedules.Count > 0)
                {
                    materialCode = string.Empty;
                }
                else
                {
                    materialCode = pr.Material_Number;
                    break;
                }
            }
            return materialCode;
        }

        private string IsAnyPRItemQtyIsNotMatchingWithSumOfQtyOfDeliverSchedule(List<TEPRItemStructure> prItemList)
        {
            string materialCode = string.Empty;
            foreach (TEPRItemStructure pr in prItemList)
            {
                var deliverySchedules = (from delv in db.TEPRItemsDeliverySchedules
                                         where delv.IsDeleted == false && delv.PRItemStructureId == pr.Uniqueid
                                         select new
                                         {
                                             delv.DeliveryScheduleId,
                                             delv.DeliveryQty,
                                             delv.DeliveryDate,
                                             delv.Remarks
                                         }).OrderBy(a => a.DeliveryScheduleId).ToList();

                decimal deliverySchduleQty = deliverySchedules.Sum(a => a.DeliveryQty);
                decimal prOrderQty = Convert.ToDecimal(pr.Order_Qty);

                if (deliverySchduleQty != prOrderQty)
                {
                    materialCode = pr.Material_Number;
                    break;
                }
            }
            return materialCode;
        }

        [HttpPost]
        public HttpResponseMessage UpdatePurchaseRequestDetails(TEPRItemStructure prItem)
        {
            try
            {
                if (prItem != null && prItem.Uniqueid > 0)
                {
                    TEPRItemStructure prItemObj = db.TEPRItemStructures.Where(x => x.Uniqueid == prItem.Uniqueid && x.IsDeleted == false).FirstOrDefault();
                    if (prItemObj != null && prItemObj.PurchaseRequestId > 0)
                    {
                        prItemObj.Order_Qty = prItem.Order_Qty;
                        prItemObj.Balance_Qty = Convert.ToDecimal(prItemObj.Order_Qty);
                        prItemObj.OnHold_Qty = 0;
                        prItemObj.WBSElementCode = prItem.WBSElementCode;

                        prItemObj.LastModifiedBy = prItem.LastModifiedBy;
                        prItemObj.LastModifiedOn = DateTime.Now;
                        db.Entry(prItemObj).CurrentValues.SetValues(prItemObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Update. PurchaseDetails Details not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Update. Invalid PurchaseDetails ID Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllApprovedPRs()
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                var prDetails = (from pr in db.TEPurchaseRequests
                                 join tpfc in db.TEPOFundCenters on pr.FundCenterId equals tpfc.Uniqueid
                                 join created in db.UserProfiles on pr.CreatedBy equals created.UserId
                                 join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId
                                 where pr.Active == true && pr.status == "Approved"
                                 select new
                                 {
                                     pr.PurchaseRequestId,
                                     pr.FundCenterId,
                                     tpfc.FundCenter_Code,
                                     tpfc.FundCenter_Description,
                                     tpfc.FundCenter_Owner,
                                     pr.PurchaseRequestTitle,
                                     pr.PurchaseRequestDesc,
                                     pr.status,
                                     CreatedBy = created.CallName,
                                     CreatedByID = pr.CreatedBy,
                                     CreatedOn = pr.CreatedOn,
                                     ApprovedBy = approver.CallName,
                                     ApprovedByID = pr.ApprovedBy,
                                     ApprovedOn = pr.ApprovedOn
                                 }).OrderByDescending(a => a.PurchaseRequestId).ToList();

                if (prDetails != null && prDetails.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
                else
                {
                    prDetails = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllApprovedPRsForInitiatePO(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            int pagenumber = 0, pagepercount = 0, UserId = 0; string SearchKey = string.Empty;
            try
            {
                if (json["LoginId"] != null)
                    UserId = json["LoginId"].ToObject<int>();
                if (json["pageNumber"] != null)
                    pagenumber = json["pageNumber"].ToObject<int>();
                if (json["pagePerCount"] != null)
                    pagepercount = json["pagePerCount"].ToObject<int>();
                if (json["FilterBy"] != null)
                    SearchKey = json["FilterBy"].ToObject<string>();

                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == UserId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Don't have PR Access";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = string.Empty, info = sinfo }) };
                }

                if (!string.IsNullOrEmpty(SearchKey))
                {
                    var prDetails = (from pr in db.TEPurchaseRequests
                                     join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                     from lftitem in g.DefaultIfEmpty()
                                     join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                     join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                     from createduser in tempcreator.DefaultIfEmpty()
                                     join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                     from approveduser in tempapprover.DefaultIfEmpty()
                                     where pr.Active == true && pr.status == "Approved" && lftitem.Balance_Qty > 0 && lftitem.IsDeleted == false
                                     &&
                                     (
                                     lftitem.Material_Number.Contains(SearchKey) ||
                                     lftitem.Short_Text.Contains(SearchKey) ||
                                     lftitem.Long_Text.Contains(SearchKey) ||
                                     lftitem.WBSElementCode.Contains(SearchKey) ||
                                     lftitem.Level1.Contains(SearchKey) ||
                                     lftitem.Level2.Contains(SearchKey) ||
                                     lftitem.Level3.Contains(SearchKey) ||
                                     lftitem.Level4.Contains(SearchKey) ||
                                     pr.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                     pr.PurchaseRequestTitle.Contains(SearchKey) ||
                                     pr.status.Contains(SearchKey) ||
                                     pr.PurchaseRequestDesc.Contains(SearchKey) ||
                                     createduser.CallName.Contains(SearchKey) ||
                                     approveduser.CallName.Contains(SearchKey) ||
                                     fund.FundCenter_Code.Contains(SearchKey) ||
                                     fund.FundCenter_Owner.Contains(SearchKey) ||
                                     fund.FundCenter_Description.Contains(SearchKey)
                                    )
                                     select new
                                     {
                                         pr.PurchaseRequestId,
                                         pr.FundCenterId,
                                         fund.FundCenter_Code,
                                         fund.FundCenter_Description,
                                         fund.FundCenter_Owner,
                                         pr.PurchaseRequestTitle,
                                         pr.PurchaseRequestDesc,
                                         pr.status,
                                         pr.POStatus,
                                         CreatedBy = createduser.CallName,
                                         CreatedByID = pr.CreatedBy,
                                         CreatedOn = pr.CreatedOn,
                                         ApprovedBy = approveduser.CallName,
                                         ApprovedByID = pr.ApprovedBy,
                                         ApprovedOn = pr.ApprovedOn
                                     }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();
               

                if (prDetails != null && prDetails.Count > 0)
                {
                    //sinfo.errorcode = 0;
                    //sinfo.errormessage = "Successfully Got Records";
                    //return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };

                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    var finalResult = prDetails.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = prDetails.Count;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = pagenumber.ToString();
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                }
                else
                {
                    prDetails = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                }
                }
                else
                {
                    var prDetails = (from pr in db.TEPurchaseRequests
                                     join item in db.TEPRItemStructures on pr.PurchaseRequestId equals item.PurchaseRequestId into g
                                     from lftitem in g.DefaultIfEmpty()
                                     join fund in db.TEPOFundCenters on pr.FundCenterId equals fund.Uniqueid
                                     join creator in db.UserProfiles on pr.CreatedBy equals creator.UserId into tempcreator
                                     from createduser in tempcreator.DefaultIfEmpty()
                                     join approver in db.UserProfiles on pr.ApprovedBy equals approver.UserId into tempapprover
                                     from approveduser in tempapprover.DefaultIfEmpty()
                                     where pr.Active == true && pr.status == "Approved" && lftitem.Balance_Qty > 0 && lftitem.IsDeleted == false
                                     
                                     select new
                                     {
                                         pr.PurchaseRequestId,
                                         pr.FundCenterId,
                                         fund.FundCenter_Code,
                                         fund.FundCenter_Description,
                                         fund.FundCenter_Owner,
                                         pr.PurchaseRequestTitle,
                                         pr.PurchaseRequestDesc,
                                         pr.status,
                                         pr.POStatus,
                                         CreatedBy = createduser.CallName,
                                         CreatedByID = pr.CreatedBy,
                                         CreatedOn = pr.CreatedOn,
                                         ApprovedBy = approveduser.CallName,
                                         ApprovedByID = pr.ApprovedBy,
                                         ApprovedOn = pr.ApprovedOn
                                     }).Distinct().OrderByDescending(a => a.PurchaseRequestId).ToList();


                    if (prDetails != null && prDetails.Count > 0)
                    {
                        //sinfo.errorcode = 0;
                        //sinfo.errormessage = "Successfully Got Records";
                        //return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };

                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        var finalResult = prDetails.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = prDetails.Count;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                        sinfo.errormessage = "Successfully Got Records";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = finalResult }) };
                    }
                    else
                    {
                        prDetails = null;
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Record Found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = prDetails }) };
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPurchaseItemsByPRIdForInitiatePO(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int prId = json["PurchaseRequestId"].ToObject<int>();
                var taxDtls = GetPurchaseItemStructureByPRIdForInitiatePO(prId);

                if (taxDtls != null && taxDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        public List<PurchaseItemStructure> GetPurchaseItemStructureByPRIdForInitiatePO(int purchaseRequestId)
        {
            List<PurchaseItemStructure> purItemstructDtls = (from itm in db.TEPRItemStructures
                                                             join pr in db.TEPurchaseRequests on itm.PurchaseRequestId equals pr.PurchaseRequestId
                                                             where itm.PurchaseRequestId == purchaseRequestId && itm.IsDeleted == false
                                                             && itm.IsDeleted == false && itm.Balance_Qty > 0
                                                             &&  pr.Active == true && pr.POStatus != "Rejected"
                                                             select new PurchaseItemStructure
                                                             {
                                                                 ItemStructureID = itm.Uniqueid,
                                                                 HeaderStructureID = itm.POStructureId,
                                                                 ItemType = itm.ItemType,
                                                                 //PurchasingOrderNumber = itm.Purchasing_Order_Number,
                                                                 Item_Number = itm.Item_Number,
                                                                 Assignment_Category = itm.Assignment_Category,
                                                                 Item_Category = itm.Item_Category,
                                                                 Material_Number = itm.Material_Number,
                                                                 Short_Text = itm.Short_Text,
                                                                 Long_Text = itm.Long_Text,
                                                                 Line_item = itm.Line_item,
                                                                 Order_Qty = itm.Order_Qty,
                                                                 Balance_Qty = itm.Balance_Qty,
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
                                                                 IGSTAmount = itm.IGSTAmount,
                                                                 CGSTRate = itm.CGSTRate,
                                                                 CGSTAmount = itm.CGSTAmount,
                                                                 SGSTRate = itm.SGSTRate,
                                                                 SGSTAmount = itm.SGSTAmount,
                                                                 TotalTaxAmount = itm.TotalTaxAmount,
                                                                 GrossAmount = itm.GrossAmount,
                                                                 ServiceHeaderId = itm.ServiceHeaderId,
                                                                 CreatedByID = itm.CreatedBy,
                                                                 LastModifiedByID = (int)itm.LastModifiedBy
                                                             }).OrderByDescending(a => a.ItemStructureID).ToList();

            return purItemstructDtls;
        }

        [HttpPost]
        public HttpResponseMessage GetPODetailsByPRId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int prId = json["PurchaseRequestId"].ToObject<int>();
                var taxDtls = GetPurchaseOrderODetailsByPRId(prId);

                if (taxDtls != null && taxDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        public List<PODetails> GetPurchaseOrderODetailsByPRId(int purchaseRequestId)
        {
            List<PODetails> poItemstructDtls = (from po in db.TEPOHeaderStructures
                                                where po.PurchaseRequestId == purchaseRequestId && po.IsDeleted == false
                                                select new PODetails
                                                {
                                                    FugueId = po.Uniqueid,
                                                    POCreationDate = po.CreatedOn,
                                                    PONumber = po.Purchasing_Order_Number,
                                                    POTitle = po.PO_Title,
                                                    POStatus = po.Status,
                                                    PODocDate = po.Purchasing_Document_Date
                                                }).OrderBy(a => a.FugueId).ToList();
            return poItemstructDtls;
        }

        [HttpPost]
        public HttpResponseMessage UpdatePurchaseRequestPOStatus(JObject json)
        {
            try
            {
                int prId = json["PurchaseRequestId"].ToObject<int>();
                string PRPoStatus = json["PRPoStatus"].ToObject<string>();
                int LastModifiedByID = json["LastModifiedByID"].ToObject<int>();

                if(string.IsNullOrEmpty(PRPoStatus))
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Update. Invalid Status Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }

                if (prId > 0)
                {
                    TEPurchaseRequest purObj = db.TEPurchaseRequests.Where(x => x.PurchaseRequestId == prId && x.Active == true).FirstOrDefault();
                    if (purObj != null && purObj.PurchaseRequestId > 0)
                    {

                        TEPOHeaderStructure pohObj = db.TEPOHeaderStructures.Where(x => x.PurchaseRequestId == prId && x.IsDeleted == false).FirstOrDefault();

                        if (PRPoStatus == "Rejected" && pohObj != null && pohObj.Uniqueid > 0)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Update. Reason: Invalid status updation. PO already available for this PR.";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                        else if (PRPoStatus == "Pending Action" && pohObj != null && pohObj.Uniqueid > 0)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Update. Reason: Invalid status updation. PO already available for this PR.";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                        else if (PRPoStatus == "Partially Ordered" && pohObj == null)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Update. Reason: Invalid status updation. PO not available for this PR.";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                        else if (PRPoStatus == "Fully Ordered" && pohObj == null)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Update. Reason: Invalid status updation. PO not available for this PR.";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }

                        purObj.POStatus = PRPoStatus;
                        purObj.LastModifiedBy = LastModifiedByID;
                        purObj.LastModifiedOn = DateTime.Now;
                        db.Entry(purObj).CurrentValues.SetValues(purObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Update. PurchaseRequest Details not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Update. Invalid PurchaseRequestID Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePRItemDetails(JObject json)
        {
            try
            {
                int prItemId = json["PRItemId"].ToObject<int>();
                int LastModifiedByID = json["LastModifiedByID"].ToObject<int>();

                if (prItemId > 0)
                {
                    TEPRItemStructure prItemObj = db.TEPRItemStructures.Where(x => x.Uniqueid == prItemId && x.IsDeleted == false).FirstOrDefault();
                    if (prItemObj != null && prItemObj.Uniqueid > 0)
                    {
                        List<TEPRItemsDeliverySchedule> delSchedules = db.TEPRItemsDeliverySchedules.Where(x => x.PRItemStructureId == prItemObj.Uniqueid
                                     && x.IsDeleted == false).ToList();
                        if (delSchedules != null && delSchedules.Count > 0)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Delete. Please delete Delivery Schedules for this Item first.";
                            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                        }
                        prItemObj.IsDeleted = true;
                        prItemObj.LastModifiedBy = LastModifiedByID;
                        prItemObj.LastModifiedOn = DateTime.Now;
                        db.Entry(prItemObj).CurrentValues.SetValues(prItemObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Deleted";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Delete. PurchaseDetails not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Delete. Invalid PurchaseDetails ID Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveDeliveryScheduleByPRItemId(JObject json)
        {
            try
            {
                int prItemId = json["PRItemId"].ToObject<int>();
                int LastModifiedByID = json["LastModifiedByID"].ToObject<int>();
                decimal DeliveryQty = json["DeliveryQty"].ToObject<decimal>();
                DateTime DeliveryDate = json["DeliveryDate"].ToObject<DateTime>();
                string DeliveryRemarks = json["DeliveryRemarks"].ToObject<string>();

                if (prItemId > 0)
                {
                    TEPRItemStructure prItemObj = db.TEPRItemStructures.Where(x => x.Uniqueid == prItemId && x.IsDeleted == false).FirstOrDefault();
                    if (prItemObj != null && prItemObj.PurchaseRequestId > 0)
                    {
                        TEPRItemsDeliverySchedule delScheduleObj = new TEPRItemsDeliverySchedule();
                        delScheduleObj.CreatedBy = LastModifiedByID;
                        delScheduleObj.CreatedOn = DateTime.Now;
                        delScheduleObj.DeliveryDate = DeliveryDate;
                        delScheduleObj.DeliveryQty = DeliveryQty;
                        delScheduleObj.IsDeleted = false;
                        delScheduleObj.LastModifiedBy = LastModifiedByID;
                        delScheduleObj.LastModifiedOn = DateTime.Now;
                        delScheduleObj.PRItemStructureId = prItemId;
                        delScheduleObj.Remarks = DeliveryRemarks;
                        db.TEPRItemsDeliverySchedules.Add(delScheduleObj);
                        db.SaveChanges();
                        if (delScheduleObj.DeliveryScheduleId > 0)
                        {
                            sinfo.errorcode = 0;
                            sinfo.errormessage = "Successfully Saved";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo}) };
                        }
                        else
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Save";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Save Delivery Schedule. PurchaseDetails Details not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save Delivery Schedule. Invalid PurchaseDetails ID Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetAllDeliverySchedulesByPRItemId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int prItemId = json["PRItemId"].ToObject<int>();
                var deliverySchedules = (from pr in db.TEPRItemsDeliverySchedules
                                 where pr.IsDeleted == false && pr.PRItemStructureId == prItemId
                                 select new
                                 {
                                     pr.DeliveryScheduleId,
                                     pr.DeliveryQty,
                                     pr.DeliveryDate,
                                     pr.Remarks
                                 }).OrderBy(a => a.DeliveryScheduleId).ToList();

                if (deliverySchedules != null && deliverySchedules.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = deliverySchedules }) };
                }
                else
                {
                    deliverySchedules = null;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = deliverySchedules }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDeliverySchedule(JObject json)
        {
            try
            {
                int DeliveryScheduleId = json["DeliveryScheduleId"].ToObject<int>();
                int LastModifiedByID = json["LastModifiedByID"].ToObject<int>();

                if (DeliveryScheduleId > 0)
                {
                    TEPRItemsDeliverySchedule prItemObj = db.TEPRItemsDeliverySchedules.Where(x => x.DeliveryScheduleId == DeliveryScheduleId
                            && x.IsDeleted == false).FirstOrDefault();
                    if (prItemObj != null && prItemObj.DeliveryScheduleId > 0)
                    {
                        prItemObj.IsDeleted = true;
                        prItemObj.LastModifiedBy = LastModifiedByID;
                        prItemObj.LastModifiedOn = DateTime.Now;
                        db.Entry(prItemObj).CurrentValues.SetValues(prItemObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Deleted";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Delete. DeliverySchedule Details not found";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Delete. Invalid DeliverySchedule ID Received";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetDeliveryScheduleDetailsByPRId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int prId = json["PurchaseRequestId"].ToObject<int>();
                var deliverySchedules = GetDeliveryScheduleDetailsByPRId(prId);

                if (deliverySchedules != null && deliverySchedules.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = deliverySchedules }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = deliverySchedules }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        public List<DeliveryScheduleDetails> GetDeliveryScheduleDetailsByPRId(int purchaseRequestId)
        {
            List<DeliveryScheduleDetails> poItemstructDtls = (from delschedules in db.TEPRItemsDeliverySchedules
                                                join pritem in db.TEPRItemStructures on delschedules.PRItemStructureId equals pritem.Uniqueid
                                                join pr in db.TEPurchaseRequests on pritem.PurchaseRequestId equals pr.PurchaseRequestId
                                                where pr.PurchaseRequestId == purchaseRequestId && delschedules.IsDeleted == false
                                                && pritem.IsDeleted == false && pr.Active == true
                                                select new DeliveryScheduleDetails
                                                {
                                                    PRItemStructureId = delschedules.PRItemStructureId,
                                                    DeliveryScheduleId = delschedules.DeliveryScheduleId,
                                                    MaterialCode = pritem.Material_Number,
                                                    MaterialDescription = pritem.Short_Text,
                                                    Quantity = delschedules.DeliveryQty,
                                                    DeliveryDate = delschedules.DeliveryDate,
                                                    Remarks = delschedules.Remarks
                                                }).OrderBy(a => a.MaterialCode).ThenBy(a => a.DeliveryDate).ToList();

            return poItemstructDtls;
        }

        public List<PR_Mat_Serv_Seq> PRSeq_Mat_Service(int PRID)
        {
            PRItemClass PRDetails = this.ItemList(PRID);
            List<PR_Mat_Serv_Seq> SeqList = new List<PR_Mat_Serv_Seq>();
            int Mat_Seq = 0; int Service_Seq = 0;

            PR_Mat_Serv_Seq EmptyListSeq = new PR_Mat_Serv_Seq();
            TEPRServiceHeader TempServ = new TEPRServiceHeader();
            EmptyListSeq.ServHead = new TEPRServiceHeader();
            EmptyListSeq.ItemS = new List<PurchaseItems>();
            SeqList.Add(EmptyListSeq);
            //Piyush
            List<string> materialNumber = new List<string>();
            List<int> materialNumber_int = new List<int>();
            List<string> serviceNumber = new List<string>();
            List<int> serviceNumber_int = new List<int>();
            
            materialNumber = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == PRID).Select(x => x.Item_Number).ToList();
            if (materialNumber.Count() > 0)
            {
                foreach (string val in materialNumber)
                {
                    int temp = Convert.ToInt32(val);
                    materialNumber_int.Add(temp);
                }
                Mat_Seq = materialNumber_int.Max();
            }
            serviceNumber = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == PRID).Select(x => x.ItemNumber).ToList();
            if (serviceNumber.Count() > 0)
            {
                foreach (string val in serviceNumber)
                {
                    int temp = Convert.ToInt32(val);
                    serviceNumber_int.Add(temp);
                }
                Service_Seq = serviceNumber_int.Max();
            }
            //end Piyush
            //if (db.TEPRItemStructures.Where(x => x.Item_Number != null && (x.ServiceHeaderId == null || x.ServiceHeaderId == 0)).Count() > 0)
            //    Mat_Seq = Convert.ToInt32(db.TEPRItemStructures.Where(x => x.Item_Number != null && (x.ServiceHeaderId == null || x.ServiceHeaderId == 0)).Max(v => v.Item_Number));

            //if (db.TEPRServiceHeaders.Where(x => x.ItemNumber != null).Count() > 0)
            //    Service_Seq = Convert.ToInt32(db.TEPRServiceHeaders.Where(x => x.ItemNumber != null).Max(x => x.ItemNumber));

            if (Mat_Seq != 0 || Service_Seq != 0)
            {
                int MaxSeq = 0;

                if (Convert.ToInt32(Mat_Seq) > Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Mat_Seq);
                else if (Convert.ToInt32(Mat_Seq) < Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Service_Seq);
                else if (Convert.ToInt32(Mat_Seq) == Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Service_Seq);

                for (int i = 1; i <= MaxSeq; i++)
                {
                    String CountVal = i.ToString();

                    List<TEPRServiceHeader> ServHead = new List<TEPRServiceHeader>();
                    ServHead = PRDetails.POServiceHeader.Where(x => x.ItemNumber == CountVal).ToList();

                    List<PurchaseItems> POItems = new List<PurchaseItems>();
                    POItems = PRDetails.PurchaseItemStructureDetails.Where(x => x.Item_Number == CountVal && (x.ServiceHeaderId == 0 || x.ServiceHeaderId == null)).ToList();

                    // For Service Items whcich has sequence
                    foreach (TEPRServiceHeader ItemHead in ServHead)
                    {
                        PR_Mat_Serv_Seq ListNonSeq = new PR_Mat_Serv_Seq();

                        ListNonSeq.ServHead = ItemHead;
                        ListNonSeq.ItemS = PRDetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ItemHead.UniqueID).ToList();
                        SeqList.Add(ListNonSeq);
                    }

                    // For Material Items whcich has sequence
                    if (POItems.Count > 0)
                    {
                       
                        foreach (PurchaseItems PRItem in POItems)
                        {
                            PR_Mat_Serv_Seq ListMatSeq = new PR_Mat_Serv_Seq();
                            ListMatSeq.ItemS = new List<PurchaseItems>();
                            ListMatSeq.ItemS.Add(PRItem);
                            SeqList.Add(ListMatSeq);
                        }
                    }
                }
                // For Null Values whcich has no sequence
                List<TEPRServiceHeader> NullServHead = new List<TEPRServiceHeader>();
                NullServHead = PRDetails.POServiceHeader.Where(x => x.ItemNumber == null).ToList();

                List<PurchaseItems> NullPOItems = new List<PurchaseItems>();
                NullPOItems = PRDetails.PurchaseItemStructureDetails.Where(x => x.Item_Number == null && (x.ServiceHeaderId == 0 || x.ServiceHeaderId == null)).ToList();

                if (NullServHead != null && NullServHead.Count > 0)
                {
                    foreach (TEPRServiceHeader ServHead in NullServHead)
                    {
                        PR_Mat_Serv_Seq NullListNonSeq = new PR_Mat_Serv_Seq();

                        NullListNonSeq.ServHead = ServHead;
                        NullListNonSeq.ItemS = PRDetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ServHead.UniqueID).ToList();
                        SeqList.Add(NullListNonSeq);
                    }
                }

                if (NullPOItems.Count > 0)
                {
                    foreach (PurchaseItems PRItem in NullPOItems)
                    {
                        PR_Mat_Serv_Seq ListMatSeq = new PR_Mat_Serv_Seq();
                        ListMatSeq.ItemS = new List<PurchaseItems>();
                        ListMatSeq.ItemS.Add(PRItem);
                        SeqList.Add(ListMatSeq);
                    }
                   
                }

            }
            // Which are all the items Null Values
            else
            {
                foreach (TEPRServiceHeader ServHead in PRDetails.POServiceHeader)
                {
                    PR_Mat_Serv_Seq ListNonSeq = new PR_Mat_Serv_Seq();
                    ListNonSeq.ServHead = ServHead;
                    ListNonSeq.ItemS = PRDetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == ServHead.UniqueID).ToList();
                    SeqList.Add(ListNonSeq);
                }
                List<PurchaseItems> PRitems = PRDetails.PurchaseItemStructureDetails.Where(x => x.ServiceHeaderId == null).ToList();
                foreach (PurchaseItems PRItem in PRitems)
                {
                    PR_Mat_Serv_Seq ListNonSeqMat = new PR_Mat_Serv_Seq();
                    ListNonSeqMat.ItemS = new List<PurchaseItems>();
                    ListNonSeqMat.ItemS.Add(PRItem);
                if (ListNonSeqMat.ItemS.Count > 0)
                        SeqList.Add(ListNonSeqMat);
                }
            }
            return SeqList;
        }


        public PRItemClass ItemList(int PRID)
        {
            PRItemClass PRItems = new PRItemClass();
            PRItems.POServiceHeader = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == PRID && x.IsDeleted == false).ToList();
            PRItems.PurchaseItemStructureDetails = (from A in db.TEPRItemStructures
                                                    where A.PurchaseRequestId == PRID && A.IsDeleted == false
                                                    select new PurchaseItems
                                                    {
                                                        Uniqueid = A.Uniqueid,
                                                        PurchaseRequestId = A.PurchaseRequestId,
                                                        Item_Number = A.Item_Number,
                                                        Assignment_Category = A.Assignment_Category,
                                                        Item_Category = A.Item_Category,
                                                        Material_Number = A.Material_Number,
                                                        Short_Text = A.Short_Text,
                                                        Long_Text = A.Long_Text,
                                                        Line_item = A.Line_item,
                                                        Order_Qty = A.Order_Qty,
                                                        Unit_Measure = A.Unit_Measure,
                                                        Delivery_Date = A.Delivery_Date,
                                                        Net_Price = A.Net_Price,
                                                        Material_Group = A.Material_Group,
                                                        Plant = A.Plant,
                                                        Storage_Location = A.Storage_Location,
                                                        Requirement_Tracking_Number = A.Requirement_Tracking_Number,
                                                        Requisition_Number = A.Requisition_Number,
                                                        Item_Purchase_Requisition = A.Item_Purchase_Requisition,
                                                        Returns_Item = A.Returns_Item,
                                                        Tax_salespurchases_code = A.Tax_salespurchases_code,
                                                        Overall_limit = A.Overall_limit,
                                                        Expected_Value = A.Expected_Value,
                                                        Total_Value = A.Total_Value,
                                                        No_Limit = A.No_Limit,
                                                        Overdelivery_Tolerance = A.Overdelivery_Tolerance,
                                                        Underdelivery_Tolerance = A.Underdelivery_Tolerance,
                                                        Status = A.Status,
                                                        ApprovedBy = A.ApprovedBy,
                                                        ApprovedOn = A.ApprovedOn,
                                                        Version = A.Version,
                                                        teversion = A.teversion,
                                                        POStructureId = A.POStructureId,
                                                        HSNCode = A.HSNCode,
                                                        WBSElementCode = A.WBSElementCode,
                                                        InternalOrderNumber = A.InternalOrderNumber,
                                                        GLAccountNo = A.GLAccountNo,
                                                        Brand = A.Brand,
                                                        Rate = A.Rate,
                                                        TotalAmount = A.TotalAmount,
                                                        TaxRate = A.TaxRate,
                                                        IGSTRate = A.IGSTRate,
                                                        IGSTAmount = A.IGSTAmount,
                                                        CGSTRate = A.CGSTRate,
                                                        CGSTAmount = A.CGSTAmount,
                                                        SGSTRate = A.SGSTRate,
                                                        SGSTAmount = A.SGSTAmount,
                                                        TotalTaxAmount = A.TotalTaxAmount,
                                                        GrossAmount = A.GrossAmount,
                                                        ItemType = A.ItemType,
                                                        WBSElementCode2 = A.WBSElementCode2,
                                                        FugueItemSeqNo = A.FugueItemSeqNo,
                                                        SAPTransactionCode = A.SAPTransactionCode,
                                                        IsRecordInSAP = A.IsRecordInSAP,
                                                        SAPItemSeqNo = A.SAPItemSeqNo,
                                                        IsCLRateUpdated = A.IsCLRateUpdated,
                                                        LastModifiedOn = A.LastModifiedOn,
                                                        LastModifiedBy = A.LastModifiedBy,
                                                        CreatedOn = A.CreatedOn,
                                                        CreatedBy = A.CreatedBy,
                                                        IsDeleted = A.IsDeleted,
                                                        Balance_Qty = A.Balance_Qty,
                                                        OnHold_Qty = A.OnHold_Qty,
                                                        Level1 = A.Level1,
                                                        Level2 = A.Level2,
                                                        Level3 = A.Level3,
                                                        Level4 = A.Level4,
                                                        Level5 = A.Level5,
                                                        Level6 = A.Level6,
                                                        Level7 = A.Level7,
                                                        ServiceHeaderId = A.ServiceHeaderId




                                                    }).ToList();

            foreach (var data in PRItems.PurchaseItemStructureDetails)
            {
                data.PRTechnicalSpecifiactionList = db.TEPRSpecificationAnnexures.Where(a => a.PRItemStructureId == data.Uniqueid && a.IsDeleted == false).ToList();
            }

            return PRItems;
        }

        public class PRItemClass
        {
            public List<TEPRServiceHeader> POServiceHeader { get; set; }
            public List<PurchaseItems> PurchaseItemStructureDetails { get; set; }
        }



        public class PurchaseItems
        {
            public int Uniqueid { get; set; }
            public Nullable<int> PurchaseRequestId { get; set; }
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
            public string Total_Value { get; set; }
            public string No_Limit { get; set; }
            public string Overdelivery_Tolerance { get; set; }
            public string Underdelivery_Tolerance { get; set; }
            public string Status { get; set; }
            public string ApprovedBy { get; set; }
            public string ApprovedOn { get; set; }
            public string Version { get; set; }
            public string teversion { get; set; }
            public Nullable<int> POStructureId { get; set; }
            public string HSNCode { get; set; }
            public string WBSElementCode { get; set; }
            public string InternalOrderNumber { get; set; }
            public string GLAccountNo { get; set; }
            public string Brand { get; set; }
            public Nullable<decimal> Rate { get; set; }
            public Nullable<decimal> TotalAmount { get; set; }
            public Nullable<decimal> TaxRate { get; set; }
            public Nullable<decimal> IGSTRate { get; set; }
            public Nullable<decimal> IGSTAmount { get; set; }
            public Nullable<decimal> CGSTRate { get; set; }
            public Nullable<decimal> CGSTAmount { get; set; }
            public Nullable<decimal> SGSTRate { get; set; }
            public Nullable<decimal> SGSTAmount { get; set; }
            public Nullable<decimal> TotalTaxAmount { get; set; }
            public Nullable<decimal> GrossAmount { get; set; }
            public string ItemType { get; set; }
            public string WBSElementCode2 { get; set; }
            public Nullable<int> FugueItemSeqNo { get; set; }
            public string SAPTransactionCode { get; set; }
            public Nullable<bool> IsRecordInSAP { get; set; }
            public Nullable<int> SAPItemSeqNo { get; set; }
            public Nullable<bool> IsCLRateUpdated { get; set; }
            public Nullable<System.DateTime> LastModifiedOn { get; set; }
            public Nullable<int> LastModifiedBy { get; set; }
            public System.DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public bool IsDeleted { get; set; }
            public Nullable<decimal> Balance_Qty { get; set; }
            public Nullable<decimal> OnHold_Qty { get; set; }
            public string Level1 { get; set; }
            public string Level2 { get; set; }
            public string Level3 { get; set; }
            public string Level4 { get; set; }
            public string Level5 { get; set; }
            public string Level6 { get; set; }
            public string Level7 { get; set; }
            public Nullable<int> ServiceHeaderId { get; set; }

            public List<TEPRSpecificationAnnexure> PRTechnicalSpecifiactionList { get; set; }
        }

        public class PR_Mat_Serv_Seq
        {
            public TEPRServiceHeader ServHead { get; set; }

            public List<PurchaseItems> ItemS { get; set; }
        }

        public class PRList
        {
            List<PR_Mat_Serv_Seq> SeqList { get; set; }
        }

        public class CopyPurchaseRequest
        {
            public int PRStructureId { get; set; }
            public int LastModifiedByID { get; set; }
        }

        public class PurchaseRequest
        {
            public int PRID { get; set; }
            public int FundCenterID { get; set; }
            public string PRTitle { get; set; }
            public string PRDescription { get; set; }
            public int CreatedByID { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedByID { get; set; }
            public DateTime LastModifiedOn { get; set; }
        }

        public class PurchaseRequestForDisplay
        {
            public int PRID { get; set; }
            public int FundCenterID { get; set; }
            public string FundCenterDescription { get; set; }
            public string PRTitle { get; set; }
            public string PRDescription { get; set; }
            public int CreatedByID { get; set; }
            public string CreatedByName { get; set; }
            public DateTime CreatedOn { get; set; }
            public int LastModifiedByID { get; set; }
            public DateTime LastModifiedOn { get; set; }
        }

        public class PurchaseRequestItemStructureList
        {
            public int PR_ID { get; set; }
            //public string PR_Title { get; set; }
            //public string PR_Description { get; set; }
            public List<int> ItemStructureID { get; set; }
            public int HeaderStructureID { get; set; }
            public List<int> ServiceHeaderId { get; set; }
            public string PurchasingOrderNumber { get; set; }
            public string Item_Number { get; set; }
            public string Assignment_Category { get; set; }
            public string Item_Category { get; set; }
            public List<string> Material_Number { get; set; }
            public List<string> Short_Text { get; set; }
            public List<string> Long_Text { get; set; }
            public string Line_item { get; set; }
            public List<string> Order_Qty { get; set; }
            public List<string> Objectid { get; set; }
            public List<string> Unit_Measure { get; set; }
            public string Delivery_Date { get; set; }
            public string Net_Price { get; set; }
            public string Material_Group { get; set; }
            public string Plant { get; set; }
            public string Storage_Location { get; set; }
            public string Requirement_Tracking_Number { get; set; }
            public string Requisition_Number { get; set; }
            public string Item_Purchase_Requisition { get; set; }
            public string Returns_Item { get; set; }
            public List<string> Tax_salespurchases_code { get; set; }
            public string Overall_limit { get; set; }
            public string Expected_Value { get; set; }
            public string Actual_Value { get; set; }
            public string No_Limit { get; set; }
            public string Overdelivery_Tolerance { get; set; }
            public string Underdelivery_Tolerance { get; set; }
            public List<string> HSN_Code { get; set; }
            public List<string> ItemType { get; set; }
            public string Status { get; set; }
            public List<string> WBSElementCode { get; set; }
            public List<string> WBSElementCode2 { get; set; }
            public List<string> InternalOrderNumber { get; set; }
            public List<string> GLAccountNo { get; set; }
            public List<string> Brand { get; set; }
            public List<string> ManufactureCode { get; set; }
            public List<string> Level1 { get; set; }
            public List<string> Level2 { get; set; }
            public List<string> Level3 { get; set; }
            public List<string> Level4 { get; set; }
            public List<string> Level5 { get; set; }
            public List<string> Level6 { get; set; }
            public List<string> Level7 { get; set; }
            public List<decimal?> Rate { get; set; }
            public List<decimal?> TotalAmount { get; set; }
            public List<decimal?> IGSTRate { get; set; }
            public List<decimal?> IGSTAmount { get; set; }
            public List<decimal?> CGSTRate { get; set; }
            public List<decimal?> CGSTAmount { get; set; }
            public List<decimal?> SGSTRate { get; set; }
            public List<decimal?> SGSTAmount { get; set; }
            public List<decimal?> TotalTaxAmount { get; set; }
            public List<decimal?> GrossAmount { get; set; }
            public int CreatedByID { get; set; }
            public int LastModifiedByID { get; set; }
            public string CreatedBy { get; set; }
            public string LastModifiedBy { get; set; }
            public decimal PR_PurchaseItemsTotalAmount { get; set; }
        }

        public class PODetails
        {
            public int FugueId { get; set; }
            public string PONumber { get; set; }
            public string POTitle { get; set; }
            public string POStatus { get; set; }
            public string Short_Text { get; set; }
            public DateTime? PODocDate { get; set; }
            public DateTime? POCreationDate { get; set; }
            public int POItemUniqueId { get; set; }
        }

        public class DeliveryScheduleDetails
        {
            public int PRItemStructureId { get; set; }
            public int DeliveryScheduleId { get; set; }
            public string MaterialCode { get; set; }
            public string MaterialDescription { get; set; }
            public decimal Quantity { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public string Remarks { get; set; }
        }

        public class SpecAnnexValue
        {
            public string value { get; set; }
            public string type { get; set; }
        }


        public class ObjFromComponentLib
        {
            public List<columnsMain> headers { get; set; }
            public string group { get; set; }
            public string id { get; set; }

        }


        public class columnsStruct
        {
            public dataTypeStruct dataType { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public object value { get; set; }
            public bool isRequired { get; set; }
            public bool isSearchable { get; set; }
        }

        public class columnsMain
        {
            public List<columnsStruct> columns { get; set; }
            public string key { get; set; }
            public string name { get; set; }
        }
        
        public class dataTypeStruct
        {
            public string name { get; set; }
            public object subType { get; set; }
        }


        public class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
            [JsonProperty("error")]
            public string Error { get; set; }
        }

        public class PRDetailsForSearch
        {
            public int PurchaseRequestId { get; set; }
            public int FundCenterId { get; set; }
            public string FundCenter_Code { get; set; }
            public string FundCenter_Description { get; set; }
            public string FundCenter_Owner { get; set; }
            public string PurchaseRequestTitle { get; set; }
            public string PurchaseRequestDesc { get; set; }
            public string status { get; set; }
            public string POStatus { get; set; }
            public int CreatedByID { get; set; }
            public string CreatedBy { get; set; }
            public string ApprovedBy { get; set; }
            public int? ApprovedByID { get; set; }
            public DateTime? ApprovedOn { get; set; }
            public bool PRItemIsDeleted { get; set; }
        }
    }
}
