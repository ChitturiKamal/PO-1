using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PO.Models;
using PO.POSAPServiceInterface;
using PO.VendorSAPServiceInterface;
using AutoMapper;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using log4net;
using System.Xml.Serialization;

namespace PO.BAL
{
    public class PurchaseOrderBAL
    {
        TETechuvaDBContext context = new TETechuvaDBContext();
        RecordExceptions exception = new RecordExceptions();
        public PurchaseOrderBAL()
        {

            context.Configuration.ProxyCreationEnabled = false;
        }
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static int CustomIndexOf(string source, char toFind, int position)
        {
            int index = -1;
            for (int i = 0; i < position; i++)
            {
                index = source.IndexOf(toFind, index + 1);

                if (index == -1)
                    break;
            }

            return index;
        }

        public POPDFModel GetDataForPOPDF(int purchaseHeaderStructureId)
        {
            POPDFModel poPdf = new POPDFModel();

            poPdf.PurchaseHeaderStructureDetails = GetPurchaseHeaderStructureDetails(purchaseHeaderStructureId);
            poPdf.PurchaseItemStructureDetails = GetPurchaseItemStructureDetails(purchaseHeaderStructureId);
            poPdf.PurchaseVendorPaymentMileStoneDetails = GetPurchaseVendorPaymentMileStoneDetails(purchaseHeaderStructureId);
            poPdf.AnnexureSpecifications = GetPOAnnexureSpecificationsByHeaderStructureId(purchaseHeaderStructureId);
            poPdf.ServiceAnnexureSpecifications = GetPOServiceAnnexureByHeaderStructureId(purchaseHeaderStructureId);
            poPdf.POServiceHeader = context.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == purchaseHeaderStructureId && x.IsDeleted == false).ToList();

            int specialConditionPickListId = getPickListItemId("Special");

            if (specialConditionPickListId > 0)
                poPdf.SpecialTermsAndConditions = (from tc in context.TETermsAndConditions
                                                   where tc.IsDeleted == false && tc.IsActive == true &&
                                                    tc.PickListItemId == specialConditionPickListId && tc.ModuleName == "PO" && tc.POHeaderStructureId == purchaseHeaderStructureId
                                                   select new PurchaseTermsAndConditionsDetail { Title = tc.Title, MasterTandCId = tc.UniqueId, Condition = tc.Condition, SequenceId = tc.SequenceId }).OrderBy(a => a.SequenceId).ToList();
            else
                poPdf.SpecialTermsAndConditions = null;

            // int specificConditionPickListId = getPickListItemId("Specific");
            // if (specificConditionPickListId > 0)                
            poPdf.SpecificTermsAndConditions = GetPOSpecificTCDetailsByPOHeaderStructereId(purchaseHeaderStructureId);
            //  else
            //    poPdf.SpecificTermsAndConditions = null;

            int generalConditionPickListId = getPickListItemId("General");
            if (generalConditionPickListId > 0)
                poPdf.GeneralTermsAndConditions = GetPurchaseTermsAndConditionsByConditionType(purchaseHeaderStructureId, generalConditionPickListId);
            else
                poPdf.GeneralTermsAndConditions = null;

            if (poPdf.PurchaseItemStructureDetails != null && poPdf.PurchaseItemStructureDetails.Count > 0)
            {
                //poPdf.WBSCode = poPdf.PurchaseItemStructureDetails[0].WBSCode;
                List<PurchaseItemStructureDetail> POPDF = poPdf.PurchaseItemStructureDetails.Where(x => x.Assignment_Category == "P" && x.Item_Category == "D").ToList();
                if (POPDF.Count > 0)
                {
                    poPdf.POTotalAmount = POPDF.Sum(a => a.Amount);
                    poPdf.POTotalTaxes = POPDF.Sum(a => a.TaxAmount);
                    poPdf.POTotalGrossAmount = POPDF.Sum(a => a.GrossAmount);
                    poPdf.POTotalPaymentTerms = poPdf.PurchaseVendorPaymentMileStoneDetails.Sum(a => a.Amount);
                }
                else
                {
                    poPdf.POTotalAmount = poPdf.PurchaseItemStructureDetails.Where(a => a.ItemUniqueId > 0).Sum(a => a.Amount);
                    poPdf.POTotalTaxes = poPdf.PurchaseItemStructureDetails.Where(a => a.ItemUniqueId > 0).Sum(a => a.TaxAmount);
                    poPdf.POTotalGrossAmount = poPdf.PurchaseItemStructureDetails.Where(a => a.ItemUniqueId > 0).Sum(a => a.GrossAmount);
                    poPdf.POTotalPaymentTerms = poPdf.PurchaseVendorPaymentMileStoneDetails.Sum(a => a.Amount);
                }
            }
            if (poPdf.PurchaseHeaderStructureDetails != null)
            {
                poPdf.WBSCode = poPdf.PurchaseHeaderStructureDetails.FundcenterName;
            }

            return poPdf;
        }

        public PurchaseHeaderStructureDetail GetPurchaseHeaderStructureDetails(int purchaseHeaderStructureId)
        {
            PurchaseHeaderStructureDetail purHeadStructDtls = (from purHead in this.context.TEPOHeaderStructures
                                                               join prj in this.context.TEProjects on purHead.ProjectID equals prj.ProjectID
                                                               join plantStrg in this.context.TEPOPlantStorageDetails on prj.ProjectCode equals plantStrg.ProjectCode
                                                               join company in this.context.TECompanies on prj.CompanyID equals company.Uniqueid
                                                               join tpv in context.TEPOVendorMasterDetails on purHead.VendorID equals tpv.POVendorDetailId into temppv
                                                               from vndrmasterdatail in temppv.DefaultIfEmpty()
                                                               join vndMaster in context.TEPOVendorMasters on vndrmasterdatail.POVendorMasterId equals vndMaster.POVendorMasterId into tempvndMaster
                                                               from vndrmstr in tempvndMaster.DefaultIfEmpty()
                                                               join plant in this.context.TEPOPlantStorageDetails on purHead.ShippedToID equals plant.PlantStorageDetailsID

                                                               join fund in this.context.TEPOFundCenters on purHead.FundCenterID equals fund.Uniqueid
                                                               join manager in context.UserProfiles on purHead.POManagerID equals manager.UserId into tempmgr
                                                               from mnger in tempmgr.DefaultIfEmpty()
                                                               join StTemp in context.TEGSTNStateMasters on vndrmasterdatail.RegionId equals StTemp.StateID into stemp
                                                               from StateDet in stemp.DefaultIfEmpty()
                                                               join Ctemp in context.TEPOCountryMasters on vndrmasterdatail.CountryId equals Ctemp.UniqueID into ctemps
                                                               from CountryDet in ctemps.DefaultIfEmpty()
                                                               where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                                               //commented due to old po's not loading to view in pdf
                                                               //&& prj.IsDeleted == false && company.IsDeleted == false && vndrmasterdatail.IsDeleted == false
                                                               //&& plant.isdeleted == false && fund.IsDeleted == fals1e
                                                               select new PurchaseHeaderStructureDetail
                                                               {
                                                                   CompanyName = company.Name,
                                                                   CompanyAddress = company.Address,
                                                                   CompanyCIN = company.CIN,
                                                                   CompanyGSTIN = plantStrg.GSTIN, //company.GSTINCode,
                                                                   CompanyCode = company.CompanyCode,
                                                                   CompanyLogo = company.CompanyLogo,
                                                                   ProjectOrFnc = prj.ProjectName,
                                                                   PurchaseOrderNo = purHead.Purchasing_Order_Number,
                                                                   POManager = mnger.CallName,
                                                                   PODate = purHead.Purchasing_Document_Date,
                                                                   Revisioin = purHead.Version,
                                                                   VendorName = vndrmstr.VendorName,
                                                                   VendorAddress = vndrmasterdatail.BillingAddress,
                                                                   VendorCode = vndrmasterdatail.VendorCode,
                                                                   VendorCIN = vndrmstr.CIN,
                                                                   VendorGSTIN = vndrmasterdatail.GSTIN,
                                                                   VendorCurrency = vndrmstr.Currency,
                                                                   POTitle = purHead.PO_Title,
                                                                   PODescripton = purHead.PODescription,
                                                                   POStatus = purHead.ReleaseCode2Status,
                                                                   ShipTo = plant.Address,
                                                                   ShipFrom = vndrmasterdatail.ShippingAddress,
                                                                   FundcenterName = fund.FundCenter_Description,
                                                                   VendorBillingCity = vndrmasterdatail.BillingCity,
                                                                   VendorBillingPostalCode = vndrmasterdatail.BillingPostalCode,
                                                                   VendorBillingState = StateDet.StateName,
                                                                   VendorBillingCountry = CountryDet.Description,
                                                                   ShipToID = purHead.ShippedToID
                                                               }).FirstOrDefault();

            string tempName = string.Empty;
            if(purHeadStructDtls.VendorName.Contains(' '))
                tempName = purHeadStructDtls.VendorName.Substring(0, purHeadStructDtls.VendorName.IndexOf(' '));
            if (tempName.Equals("Mr") || tempName.Equals("Mrs") || tempName.Equals("Ms") || tempName.Equals("M/S") || tempName.Equals("Dr."))
            {
                purHeadStructDtls.VendorName = purHeadStructDtls.VendorName.Substring(purHeadStructDtls.VendorName.IndexOf(' ')).Trim();
            }

            var compGSTIN = context.TEPOPlantStorageDetails.Where(x => x.isdeleted == false && x.Type == "Shipping" && x.CompanyCode == purHeadStructDtls.CompanyCode && x.PlantStorageDetailsID == purHeadStructDtls.ShipToID).Select(a => a.GSTIN).FirstOrDefault();

            purHeadStructDtls.CompanyGSTIN = compGSTIN;

            return purHeadStructDtls;
        }

        public List<PurchaseItemStructureDetail> GetPurchaseItemStructureDetails(int purchaseHeaderStructureId)
        {
            List<PurchaseItemStructureDetail> purItemst = new List<PurchaseItemStructureDetail>();
            List<PurchaseItemStructureDetail> purItemstructDtls = new List<PurchaseItemStructureDetail>();
             String OrdType = (from HeadStruct in context.TEPOHeaderStructures
                                                join OrdTy in context.TEPurchase_OrderTypes on HeadStruct.PO_OrderTypeID equals OrdTy.UniqueId
                                                where HeadStruct.Uniqueid == purchaseHeaderStructureId
                               select OrdTy.Code).FirstOrDefault();
            if (OrdType == "NB")
            {
                purItemstructDtls = (from purHead in context.TEPOHeaderStructures
                                     join itm in context.TEPOItemStructures on purHead.Uniqueid equals itm.POStructureId
                                     where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                     && itm.IsDeleted == false 
                                     select new PurchaseItemStructureDetail
                                     {
                                         ItemUniqueId = itm.Uniqueid,
                                         ItemNo = itm.Item_Number,
                                         POItemName = itm.Long_Text,
                                         Assignment_Category = itm.Assignment_Category,
                                         Item_Category = itm.Item_Category,
                                         Quantity = itm.Order_Qty,
                                         Unit = itm.Unit_Measure,
                                         Rate = itm.Rate,
                                         Amount = itm.TotalAmount,
                                         IGSTRate = itm.IGSTRate,
                                         SGSTRate = itm.SGSTRate,
                                         CGSTRate = itm.CGSTRate,
                                         TaxAmount = itm.TotalTaxAmount,
                                         GrossAmount = itm.GrossAmount,
                                         //WBSCode = itm.WBSElementCode
                                         FugueItemSeqNo = itm.FugueItemSeqNo,
                                         SAPTransactionCode = itm.SAPTransactionCode,
                                         IsRecordInSAP = itm.IsRecordInSAP,
                                         SAPItemSeqNo = itm.SAPItemSeqNo,
                                         IsCLRateUpdated = itm.IsCLRateUpdated,
                                         BrandName = itm.Brand,
                                         LINEITEMNUMBER = itm.LINEITEMNUMBER,
                                         ItemType = itm.ItemType,
                                         ManufactureCode = itm.ManufactureCode,
                                         POItemShortText = itm.Short_Text,
                                         POItemLongText = itm.Long_Text,
                                         POHeaderStructureid = itm.ServiceHeaderId
                                     }).OrderBy(a => a.ItemUniqueId).ToList();
            }
            else
            {
                purItemstructDtls = (from purHead in context.TEPOHeaderStructures
                                                                       join itm in context.TEPOItemStructures on purHead.Uniqueid equals itm.POStructureId
                                                                       where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                                                       && itm.IsDeleted == false
                                                                       select new PurchaseItemStructureDetail
                                                                       {
                                                                           ItemUniqueId = itm.Uniqueid,
                                                                           ItemNo = itm.Item_Number,
                                                                           POItemName = itm.Long_Text,
                                                                           Quantity = itm.Order_Qty,
                                                                           Unit = itm.Unit_Measure,
                                                                           Rate = itm.Rate,
                                                                           Amount = itm.TotalAmount,
                                                                           IGSTRate = itm.IGSTRate,
                                                                           SGSTRate = itm.SGSTRate,
                                                                           CGSTRate = itm.CGSTRate,
                                                                           TaxAmount = itm.TotalTaxAmount,
                                                                           GrossAmount = itm.GrossAmount,
                                                                           //WBSCode = itm.WBSElementCode
                                                                           FugueItemSeqNo = itm.FugueItemSeqNo,
                                                                           BrandName = itm.Brand,
                                                                           ItemType = itm.ItemType,
                                                                           SAPTransactionCode = itm.SAPTransactionCode,
                                                                           IsRecordInSAP = itm.IsRecordInSAP,
                                                                           SAPItemSeqNo = itm.SAPItemSeqNo,
                                                                           ManufactureCode = itm.ManufactureCode,
                                                                           POItemShortText = itm.Short_Text,
                                                                           POItemLongText = itm.Long_Text,
                                                                           POHeaderStructureid = itm.ServiceHeaderId
                                                                       }).OrderBy(a => a.ItemUniqueId).ToList();
            }
            foreach (var data in purItemstructDtls)
            {
                data.POTechnicalSpecifiactionList = context.TEPOSpecificationAnnexures.Where(a => a.POItemStructureId == data.ItemUniqueId && a.IsDeleted == false).ToList();
                if (string.IsNullOrEmpty(data.POItemName))
                    data.POItemName = data.POItemShortText;
            }

            //List<PurchaseServiceBreakUp> purItemServDtls = (from brkp in context.TEPOServiceBreakUps
            //                                                where brkp.POStructureId == purchaseHeaderStructureId && brkp.IsDeleted == false
            //                                                select new PurchaseServiceBreakUp
            //                                                {
            //                                                    ItemUniqueId = brkp.Uniqueid,
            //                                                    ItemNo = brkp.Item_Number,
            //                                                    POItemShortText = brkp.Short_Text,
            //                                                    POItemLongText = brkp.LongText,
            //                                                    Quantity = brkp.Actual_Qty,
            //                                                    Unit = brkp.Unit_Measure,
            //                                                    Rate = brkp.Gross_Price,
            //                                                    Amount = brkp.Net_Price
            //                                                }).OrderBy(a => a.ItemUniqueId).ToList();
            //if (purItemstructDtls.Count > 0 && purItemServDtls.Count > 0)
            //{
            //    foreach (var purItem in purItemstructDtls)
            //    {
            //        var servbrkp = purItemServDtls.Where(a => a.ItemNo == purItem.ItemNo).OrderBy(b => b.ItemUniqueId).ToList();
            //        if (servbrkp.Count > 0)
            //        {
            //            purItemst.Add(purItem);
            //            int bseq = 1;
            //            foreach (var sbkp in servbrkp)
            //            {
            //                PurchaseItemStructureDetail strDtl = new PurchaseItemStructureDetail();
            //                strDtl.ItemNo = sbkp.ItemNo + "." + bseq.ToString();
            //                strDtl.POItemName = sbkp.POItemShortText;
            //                strDtl.POItemShortText = sbkp.POItemShortText;
            //                strDtl.Quantity = sbkp.Quantity;
            //                strDtl.Unit = sbkp.Unit;
            //                strDtl.Rate = string.IsNullOrEmpty(sbkp.Rate) ? 0 : Convert.ToDecimal(sbkp.Rate);
            //                strDtl.Amount = string.IsNullOrEmpty(sbkp.Amount) ? 0 : Convert.ToDecimal(sbkp.Amount);
            //                purItemst.Add(strDtl);
            //                bseq += 1;
            //            }
            //        }
            //        else
            //        {
            //            purItemst.Add(purItem);
            //        }
            //    }
            //}
            //else
            //{

            if (purItemstructDtls.Count > 0)
            {
                foreach (var itmst in purItemstructDtls)
                {
                    //if (string.IsNullOrEmpty(itmst.ItemNo))
                    //{
               //     itmst.ItemNo = itemseq.ToString();
                 //   itemseq += 1;
                    //}
                    purItemst.Add(itmst);
                }
            }

            //if (purItemstructDtls.Count > 0)
            //{
            //    int itemseq = 1;
            //    foreach (var itmst in purItemstructDtls)
            //    {
            //        //if (string.IsNullOrEmpty(itmst.ItemNo))
            //        //{
            //            itmst.ItemNo = itemseq.ToString();
            //            itemseq += 1;
            //        //}
            //        purItemst.Add(itmst);
            //    }
            //}
            // }
            return purItemst;
        }

        public List<PurchaseVendorPaymentMileStoneDetail> GetPurchaseVendorPaymentMileStoneDetails(int purchaseHeaderStructueId)
        {
            List<PurchaseVendorPaymentMileStoneDetail> purVendorPaymentDtls
                                    = (from purHead in context.TEPOHeaderStructures
                                       join payment in context.TEPOVendorPaymentMilestones on purHead.Uniqueid equals payment.POHeaderStructureId
                                       where purHead.Uniqueid == purchaseHeaderStructueId && purHead.IsDeleted == false
                                       && payment.IsDeleted == false
                                       select new PurchaseVendorPaymentMileStoneDetail
                                       {
                                           MileStoneID = payment.UniqueId,
                                           PaymentTerm = payment.PaymentTerm,
                                           PaymentDate = payment.Date,
                                           Percentage = payment.Percentage,
                                           Amount = payment.Amount
                                       }).OrderBy(a => a.MileStoneID).ToList();

            return purVendorPaymentDtls;
        }

        public List<PurchaseTermsAndConditionsDetail> GetPurchaseTermsAndConditionsByConditionType(int purchaseHeaderStructureId, int picklistItemId)
        {
            List<PurchaseTermsAndConditionsDetail> purTermsAndConditions = new List<PurchaseTermsAndConditionsDetail>();
            //List<PurchaseTermsAndConditionsDetail> purTermsAndConditions
            //                        = (from purHead in context.TEPOHeaderStructures
            //                           join terms in context.TETermsAndConditions on purHead.Uniqueid equals terms.POHeaderStructureId
            //                           where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
            //                           && terms.IsDeleted == false && terms.IsActive == true
            //                           && terms.PickListItemId == picklistItemId
            //                           group terms by new { terms.Title, terms.SequenceId, terms.Condition } into g
            //                           orderby g.Key.SequenceId ascending
            //                           select new PurchaseTermsAndConditionsDetail
            //                           {
            //                               SequenceId = g.Key.SequenceId,
            //                               Title = g.Key.Title,
            //                               Condition = g.Key.Condition
            //                           }).ToList();
            purTermsAndConditions = (from tc in context.TETermsAndConditions
                                     join mtc in context.TEMasterTermsConditions on tc.MasterTandCId equals mtc.UniqueId
                                     where tc.ModuleName == "PO" && tc.POHeaderStructureId == purchaseHeaderStructureId && tc.PickListItemId == picklistItemId
                                               && tc.IsDeleted == false && mtc.IsDeleted == false
                                     select new PurchaseTermsAndConditionsDetail { Title = mtc.Title, MasterTandCId = mtc.UniqueId, Condition = mtc.Condition, SequenceId = tc.SequenceId }).OrderBy(a => a.SequenceId).ToList();

            if (purTermsAndConditions != null && purTermsAndConditions.Count > 0)
                return purTermsAndConditions;
            else
                return null;
        }


        public List<PurchaseItemStructure> GetPurchaseItemStructureByPOStructureId(int purchaseHeaderStructureId)
        {
            var purItemstructDtls = (from purHead in context.TEPOHeaderStructures
                                                             join itm in context.TEPOItemStructures on purHead.Uniqueid equals itm.POStructureId
                                                             where purHead.Uniqueid == purchaseHeaderStructureId && purHead.IsDeleted == false
                                                             && itm.IsDeleted == false
                                                             select new PurchaseItemStructure
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
                                                                 //CreatedByID
                                                                 //LastModifiedByID
                                                                 CreatedBy = itm.CreatedBy,
                                                                 LastModifiedBy = itm.LastModifiedBy
                                                             }).OrderBy(a => a.ItemStructureID).ToList();
            foreach (var data in purItemstructDtls)
            {
                data.POTechnicalSpecifiactionList = context.TEPOSpecificationAnnexures.Where(a => a.POItemStructureId == data.ItemStructureID && a.IsDeleted == false).ToList();
            }

            return purItemstructDtls;
        }

        public int getPickListItemId(string description)
        {
            int picklistitemid = 0;
            picklistitemid = (from pickmaster in context.TEPickLists
                              join pl in context.TEPickListItems on pickmaster.Uniqueid equals pl.TEPickList
                              where pickmaster.IsDeleted == false && pickmaster.Description == "TermsAndConditionType" && pl.Description == description
                              select pl.Uniqueid).FirstOrDefault();
            return picklistitemid;
        }

        public string getloginUsernamebyId(int userid)
        {
            string username = string.Empty;
            username = (from user in context.UserProfiles
                        where user.IsDeleted == false && user.UserId == userid
                        select user.UserName).FirstOrDefault();
            return username;
        }

        //public string SendPODetailsToSAP(int purchaseHeaderID)
        //{
        //    string PONumber = string.Empty;
        //    string CompanyCode= string.Empty;
        //    string VendorAccountNo= string.Empty;
        //    string DocumentTypeCode= string.Empty;
        //    string Currency= string.Empty;
        //    string ProjectCode= string.Empty;
        //    int wbsParts = 0;
        //    string wbsPartOne= string.Empty;

        //    PurchaseOrderReqHeader poHeaderStructure = new PurchaseOrderReqHeader();
        //    PurchaseOrderReq poReq = new PurchaseOrderReq();
        //    context.Configuration.ProxyCreationEnabled = true;
        //    TEPOHeaderStructure headerStructure = new TEPOHeaderStructure();
        //    List<TEPOItemStructure> itemList = new List<TEPOItemStructure>();

        //    var POWBSPartValue= context.TEMasterRules.Where(a => a.RuleName.Contains("POWBSPart1") && a.IsDeleted == false).FirstOrDefault();
        //    if(POWBSPartValue!=null)
        //    {
        //        wbsParts = Convert.ToInt32(POWBSPartValue.RuleValue);
        //    }

        //    headerStructure = context.TEPOHeaderStructures.Where(a => a.Uniqueid == purchaseHeaderID && a.IsDeleted == false).FirstOrDefault();
        //    if (headerStructure != null)
        //    {
        //        var CmpnyVendCode = (from purHead in  context.TEPOHeaderStructures
        //                             join proj in context.TEProjects on purHead.ProjectID equals proj.ProjectID
        //                         join cmpny in context.TECompanies on proj.CompanyID equals cmpny.Uniqueid
        //                         join vend  in context.TEPOVendors on purHead.VendorID equals vend.Uniqueid
        //                         join orderType in context.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
        //                         where proj.IsDeleted == false && cmpny.IsDeleted == false
        //                         && vend.IsDeleted == false && orderType.IsDeleted == false
        //                         select new
        //                         {
        //                             cmpny.CompanyCode,
        //                             vend.Vendor_Code,
        //                             vend.Currency,
        //                             orderType.Code,
        //                             proj.ProjectCode
        //                         }).FirstOrDefault();
        //        if (CmpnyVendCode != null)
        //        {
        //            CompanyCode = CmpnyVendCode.CompanyCode;
        //            VendorAccountNo = CmpnyVendCode.Vendor_Code;
        //            DocumentTypeCode = CmpnyVendCode.Code;
        //            Currency = CmpnyVendCode.Currency;
        //        }
        //    }
        //    var pomanagerObj = context.UserProfiles.Where(a => a.UserId == headerStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
        //    var plantStorage= context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
        //    var shippingLocation = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
        //    var fundCenter=context.TEPOFundCenters.Where(a => a.Uniqueid == headerStructure.FundCenterID && a.IsDeleted == false).FirstOrDefault();

        //    itemList = context.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderID && a.IsDeleted == false).ToList();

        //    DateTime docDate = Convert.ToDateTime(headerStructure.Purchasing_Document_Date);
        //    poHeaderStructure.PO_NUMBER = "";
        //    poHeaderStructure.F_PONUMBER = headerStructure.Fugue_Purchasing_Order_Number;
        //    poHeaderStructure.COMP_CODE = CompanyCode;
        //    poHeaderStructure.DOC_TYPE= DocumentTypeCode;
        //    poHeaderStructure.VENDOR = VendorAccountNo;
        //    poHeaderStructure.PURCH_ORG = "1000";
        //    poHeaderStructure.PUR_GROUP = "001";
        //    poHeaderStructure.CURRENCY = Currency;
        //    poHeaderStructure.DOC_DATE = string.Format("{0:dd.MM.yyyy}", docDate);
        //    poHeaderStructure.PMNTTRMS = "T041";
        //    poHeaderStructure.REF_1 = ".";
        //    poHeaderStructure.OUR_REF = ".";
        //    poHeaderStructure.TELEPHONE = ".";
        //    poHeaderStructure.COMPLETED = "";//second time onwards "X" has to send
        //    poHeaderStructure.REASON = "";//second time onwards "003" has to send
        //    poHeaderStructure.EXCH_RATE = "";// headerStructure.Exchange_Rate;
        //    poHeaderStructure.SALES_PERS = pomanagerObj.UserName;
        //    poHeaderStructure.ZPOTITLE = headerStructure.PO_Title;
        //    poHeaderStructure.ZAGSIGNDT = "";// string.Format("{0:dd.MM.yyyy}", docDate);
        //    poHeaderStructure.FUGUE_ID = headerStructure.Uniqueid.ToString();
        //    poHeaderStructure.TEXT_LINE = "";
        //    poHeaderStructure.ZPROJECT = ProjectCode;            

        //    List<PurchaseOrderReqItem> itemStructureList = new List<PurchaseOrderReqItem>();
        //    List<PurchaseOrderReqItem1> itemConditionList = new List<PurchaseOrderReqItem1>();
        //    List<PurchaseOrderReqItem2> item2List = new List<PurchaseOrderReqItem2>();
        //    if (itemList.Count > 0)
        //    {
        //        int count = 1;
        //        foreach (TEPOItemStructure item in itemList)
        //        {     
        //            var commitmentItem= context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
        //            wbsPartOne = wbsResult(item.WBSElementCode, wbsParts + 1);
        //            PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();                                       
        //            itemStructure.PO_NUMBER = "";
        //            itemStructure.PO_ITEM = count.ToString();//item.Item_Number;
        //            itemStructure.DEL_IND = "";//if we delete item mark it as X
        //            itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"
        //            if (item.ItemType == "ServiceOrder" || item.ItemType == "ExpenseOrder")
        //            {
        //                itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
        //                itemStructure.MATERIAL = "";
        //                itemStructure.SHORT_TEXT = item.Short_Text.Substring(0, 40);
        //                itemStructure.QUANTITY = "1";
        //                itemStructure.PO_UNIT = "AU";
        //                itemStructure.MATL_GROUP = "17";
        //            }                   
        //            else
        //            {
        //                itemStructure.ACCTASSCAT = "Q";//material "Q", Service & Expense "P"
        //                itemStructure.MATERIAL = item.Material_Number;
        //                itemStructure.SHORT_TEXT = "";
        //                itemStructure.QUANTITY = item.Order_Qty;
        //                itemStructure.PO_UNIT = "";
        //                itemStructure.MATL_GROUP = "";
        //            } 
        //            itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
        //            itemStructure.NET_PRICE = "";

        //            itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
        //            itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;
        //            if (item.ItemType == "ServiceOrder")
        //            {
        //                itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
        //                itemStructure.WBS_ELEMENT = "";
        //                itemStructure.FUNDS_CTR = "";
        //                itemStructure.CMMT_ITEM_LONG = "";
        //            }
        //            else
        //            {
        //                itemStructure.GL_ACCOUNT = item.GLAccountNo;
        //                itemStructure.WBS_ELEMENT = wbsPartOne;// item.wbselementcode part1;
        //                itemStructure.FUNDS_CTR = fundCenter.FundCenter_Code;
        //                itemStructure.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
        //            }
        //            itemStructure.ASSET_NO = "";                    
        //            itemStructure.ORDER = "";//item.Purchasing_Order_Number
        //            itemStructure.TAX_CODE = "G3";// item.Tax_salespurchases_code;
        //            itemStructure.PREQ_NO = "";
        //            itemStructure.PREQ_ITEM = "";
        //            itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
        //            itemStructure.SERVICE = "";
        //            itemStructure.SQUANTITY = "";
        //            itemStructure.GR_PRICE = "";
        //            itemStructure.PRE_VENDOR = "";
        //            if (item.ItemType == "ExpenseOrder")
        //            {
        //                itemStructure.LIMIT = item.TotalAmount.ToString();
        //                itemStructure.EXP_VALUE = item.TotalAmount.ToString();
        //                itemStructure.STEUC = item.HSNCode;//HSNCode
        //            }
        //            else
        //            {
        //                itemStructure.LIMIT = "";
        //                itemStructure.EXP_VALUE = "";
        //                itemStructure.STEUC = "";
        //            }

        //            itemStructure.NO_LIMIT = "";
        //            itemStructure.EXT_LINE = "";
        //            itemStructure.BASE_UOM = "";
        //            itemStructure.LINE_TEXT = "";
        //            itemStructure.NETWORK = "";
        //            itemStructure.OVF_TOL = "";
        //            itemStructure.OVF_UNLIM = "";
        //            itemStructure.NO_MORE_GR = "";
        //            itemStructure.RET_ITEM = ""; 
        //            itemStructure.TEXT_LINE = "";//more description
        //            itemStructure.KOSTL = "";

        //            itemStructureList.Add(itemStructure);

        //            if (item.ItemType == "MaterialOrder")
        //            {
        //                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
        //                itemCondition.KBETR = item.Rate.ToString();
        //                itemCondition.KPOSN = "";// "1";
        //                itemCondition.KSCHL = "P001";
        //                itemCondition.LIFNR = "";
        //                itemCondition.PO_ITEM = count.ToString();//item.Item_Number;
        //                itemConditionList.Add(itemCondition);
        //            }
        //            if (item.ItemType == "ServiceOrder")
        //            {
        //                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
        //                item2.PO_ITEM = count.ToString();//item.Item_Number;
        //                item2.LINES = "";
        //                item2.SERVICE = item.Material_Number;
        //                item2.SQUANTITY = item.Order_Qty;
        //                item2.GR_PRICE = item.Rate.ToString();
        //                item2.BASE_UOM = "";
        //                item2.LINE_TEXT = "";//"" if we delete then "X"
        //                item2.OVF_TOL = "";
        //                item2.OVF_UNLIM1 = "";
        //                item2.MATKL = "";
        //                item2.WBS_ELEMENT = wbsPartOne;
        //                item2.GL_ACCOUNT = item.GLAccountNo;
        //                item2.ORDER = "";
        //                item2.FUNDS_CTR = fundCenter.FundCenter_Code;
        //                item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
        //                item2.LONG_TEXT = item.Long_Text.Substring(0, 130);                        
        //                item2List.Add(item2);
        //            }
        //            count++;
        //        }
        //    }
        //    poReq.Header = poHeaderStructure;
        //    poReq.item = itemStructureList.ToArray();
        //    if (itemConditionList.Count == 0)
        //    {
        //        PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
        //        itemCondition.KBETR = "";
        //        itemCondition.KPOSN = "";
        //        itemCondition.KSCHL = "";
        //        itemCondition.LIFNR = "";
        //        itemCondition.PO_ITEM = "";
        //        itemConditionList.Add(itemCondition);
        //    }
        //    poReq.item1 = itemConditionList.ToArray();
        //    if (item2List.Count == 0)
        //    {
        //        PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
        //        item2.BASE_UOM = "";
        //        item2.CMMT_ITEM_LONG = "";
        //        item2.FUNDS_CTR = "";
        //        item2.GL_ACCOUNT = "";
        //        item2.GR_PRICE = "";
        //        item2.LINES = "";
        //        item2.LINE_TEXT = "";
        //        item2.LONG_TEXT = "";
        //        item2.MATKL = "";
        //        item2.ORDER = "";
        //        item2.OVF_TOL = "";
        //        item2.OVF_UNLIM1 = "";
        //        item2.PO_ITEM = "";
        //        item2.SERVICE = "";
        //        item2.SQUANTITY = "";
        //        item2.WBS_ELEMENT = "";
        //        item2List.Add(item2);
        //    }
        //    poReq.Item2= item2List.ToArray();
        //    SAPCallConnector sapCall = new SAPCallConnector();
        //    IList<PurchaseOrderResItem> RespList = new List<PurchaseOrderResItem>();
        //    RespList = sapCall.CreatePO(poReq);
        //    foreach (PurchaseOrderResItem item in RespList)
        //    {
        //        if (item.RETCODE == "0")
        //        {
        //            PONumber = item.PO_NUMBER;
        //            headerStructure.Purchasing_Order_Number = PONumber;
        //            context.Entry(headerStructure).CurrentValues.SetValues(headerStructure);
        //            context.SaveChanges();
        //            //Purchasing Document Number PO_NUMBER
        //            //Fugue Purchasing Order Number   F_PONUMBER
        //            //Item Number of Purchasing Document  PO_ITEM
        //            //PO Item Net Value NETWR
        //            //PO Item Gross Value BRTWR
        //            //PO Item Tax Value NAVNW
        //            //Return Code RETCODE
        //            //Return Message MESSAGE
        //            //Fugue Reference ID FUGUE_ID
        //        }
        //        else
        //        {
        //            ApplicationErrorLog errorlog = new ApplicationErrorLog();
        //            errorlog.InnerException = headerStructure.Uniqueid.ToString();
        //            errorlog.Stacktrace = item.MESSAGE;
        //            errorlog.Source = "PO Posting";
        //            errorlog.Error = item.MESSAGE;
        //            errorlog.ExceptionDateTime = DateTime.Now;
        //            context.ApplicationErrorLogs.Add(errorlog);
        //            context.SaveChanges();

        //            //if (log.IsDebugEnabled)
        //            //    log.Debug("Failed To Generate Invoice:" + item.MESSAGE);
        //        }
        //    }
        //    return PONumber;
        //}

        #region Author[Kamal] For Service Order

        public SAPResponse SendPODetailsToSAP(int purchaseHeaderID, bool Validation_Check = false)
        {
            string PONumber = string.Empty;
            string CompanyCode = string.Empty;
            string VendorAccountNo = string.Empty;
            string DocumentTypeCode = string.Empty;
            string Currency = string.Empty;
            string ProjectCode = string.Empty;
            int wbsParts = 0;
            string wbsPartOne = string.Empty;
            SAPResponse sapRes = new SAPResponse();

            var HeadOrderType = (from Head in context.TEPOHeaderStructures
                                 join OrdType in context.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == purchaseHeaderID
                                 select OrdType.Code).FirstOrDefault();


            PurchaseOrderReqHeader poHeaderStructure = new PurchaseOrderReqHeader();
            PurchaseOrderReq poReq = new PurchaseOrderReq();
            context.Configuration.ProxyCreationEnabled = true;
            TEPOHeaderStructure headerStructure = new TEPOHeaderStructure();
            List<TEPOItemStructure> itemList = new List<TEPOItemStructure>();

            var POWBSPartValue = context.TEMasterRules.Where(a => a.RuleName.Contains("POWBSPart1") && a.IsDeleted == false).FirstOrDefault();
            if (POWBSPartValue != null)
            {
                wbsParts = Convert.ToInt32(POWBSPartValue.RuleValue);
            }

            headerStructure = context.TEPOHeaderStructures.Where(a => a.Uniqueid == purchaseHeaderID && a.IsDeleted == false).FirstOrDefault();
            if (headerStructure != null)
            {
                var CmpnyVendCode = (from purHead in context.TEPOHeaderStructures
                                     join proj in context.TEProjects on purHead.ProjectID equals proj.ProjectID
                                     join cmpny in context.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                                     join vendordtl in this.context.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                                     join vendor in this.context.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                     join orderType in context.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                                     where
                                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                                      //&& vendor.IsDeleted == false && vendordtl.IsDeleted == false && orderType.IsDeleted == false &&
                                      purHead.Uniqueid == headerStructure.Uniqueid
                                     select new
                                     {
                                         cmpny.CompanyCode,
                                         vendordtl.VendorCode,
                                         vendor.Currency,
                                         orderType.Code,
                                         proj.ProjectCode
                                     }).FirstOrDefault();
                if (CmpnyVendCode != null)
                {
                    CompanyCode = CmpnyVendCode.CompanyCode;
                    VendorAccountNo = CmpnyVendCode.VendorCode;
                    DocumentTypeCode = CmpnyVendCode.Code;
                    Currency = CmpnyVendCode.Currency;
                    ProjectCode = CmpnyVendCode.ProjectCode;
                }
            }
            var pomanagerObj = context.UserProfiles.Where(a => a.UserId == headerStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
            var plantStorage = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
            var shippingLocation = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
            var fundCenter = context.TEPOFundCenters.Where(a => a.Uniqueid == headerStructure.FundCenterID && a.IsDeleted == false).FirstOrDefault();

           

            //DateTime docDate = Convert.ToDateTime(headerStructure.Purchasing_Document_Date);
            DateTime docDate = DateTime.Today;
            if (Validation_Check)
                poHeaderStructure.TESTRUN = "X";
            else
                poHeaderStructure.TESTRUN = "";

            poHeaderStructure.PO_NUMBER = "";
            poHeaderStructure.F_PONUMBER = headerStructure.Fugue_Purchasing_Order_Number;
            poHeaderStructure.COMP_CODE = CompanyCode;
            poHeaderStructure.DOC_TYPE = DocumentTypeCode;
            poHeaderStructure.VENDOR = VendorAccountNo;
            poHeaderStructure.PURCH_ORG = "1000";
            poHeaderStructure.PUR_GROUP = "001";
            poHeaderStructure.CURRENCY = Currency;
            poHeaderStructure.DOC_DATE = string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.PMNTTRMS = "T041";
            poHeaderStructure.REF_1 = ".";
            poHeaderStructure.OUR_REF = ".";
            poHeaderStructure.TELEPHONE = ".";
            poHeaderStructure.COMPLETED = "";//second time onwards "X" has to send
            poHeaderStructure.REASON = "";//second time onwards "003" has to send
            poHeaderStructure.EXCH_RATE = "";// headerStructure.Exchange_Rate;
            if (pomanagerObj != null)
                poHeaderStructure.SALES_PERS = pomanagerObj.UserName;
            else
                poHeaderStructure.SALES_PERS = "";
            poHeaderStructure.ZPOTITLE = headerStructure.PO_Title;
            poHeaderStructure.ZAGSIGNDT = "";// string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.FUGUE_ID = headerStructure.Uniqueid.ToString();
            poHeaderStructure.TEXT_LINE = "";
            poHeaderStructure.ZPROJECT = ProjectCode;

            List<PurchaseOrderReqItem> itemStructureList = new List<PurchaseOrderReqItem>();
            List<PurchaseOrderReqItem1> itemConditionList = new List<PurchaseOrderReqItem1>();
            List<PurchaseOrderReqItem2> item2List = new List<PurchaseOrderReqItem2>();

            itemList = context.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderID && a.IsDeleted == false).OrderBy(x => x.Uniqueid).ToList();
            
            #region Service Material & Expense

            if (itemList.Count > 0)
                {
                    int count = 1;
                Dictionary<int?, String> ServicOrdList = new Dictionary<int?, String>();

                List<int?> ServiceNumber = new List<int?>();
                List<TEPOItemStructure> SeqItems1 = new List<TEPOItemStructure>();
                List<TEPOItemStructure> SeqItems = new List<TEPOItemStructure>();
                int Mat_Seq = 1;
                #region Service Material and Expense Order

                foreach (String itemNum in itemList.Select(x => x.Item_Number))
                {
                    int IntNum = Convert.ToInt32(itemNum);
                    if (Mat_Seq < IntNum)
                        Mat_Seq = IntNum;
                }

                for(int i = 1; i <= Mat_Seq; i++)
                {
                    List<TEPOItemStructure> TempItems = itemList.Where(x => x.Item_Number == i.ToString()).ToList();
                    SeqItems1.AddRange(TempItems);
                }

                foreach (TEPOItemStructure TEItems in SeqItems1)
                {
                    if (TEItems.ItemType.Equals("ServiceOrder"))
                    {

                        int? HeadID = TEItems.ServiceHeaderId;
                        if (!ServiceNumber.Contains(HeadID))
                        {
                            List<TEPOItemStructure> TempList = context.TEPOItemStructures.Where(x => x.ServiceHeaderId == HeadID && x.IsDeleted == false).OrderBy(x => x.Uniqueid).ToList();
                            SeqItems.AddRange(TempList);
                            ServiceNumber.Add(HeadID);
                        }
                    }
                    else
                        SeqItems.Add(TEItems);
                }

                foreach (TEPOItemStructure item in SeqItems)
                    {
                    TEPOServiceHeader ServHeadList = new TEPOServiceHeader();
                    String Short_Text = String.Empty;
                    String Long_Text = String.Empty;

                    var commitmentItem = context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
                       
                        if (!string.IsNullOrEmpty(item.WBSElementCode))
                        {
                            if (item.WBSElementCode.Length > wbsParts)
                                wbsPartOne = item.WBSElementCode.Substring(0, wbsParts);
                            else
                                wbsPartOne = item.WBSElementCode;
                        }
                    bool ListCheck = true;
                   
                        if(ServicOrdList.Count > 0 && item.ItemType == "ServiceOrder")
                        {
                            if (ServicOrdList.ContainsKey(item.ServiceHeaderId))
                                ListCheck = false;
                            else
                                ListCheck = true;
                        }

                    if (ListCheck || item.ItemType != "ServiceOrder")
                    {
                        PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
                        if (item.ItemType == "ServiceOrder")
                        {
                            ServHeadList = context.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == purchaseHeaderID && x.UniqueID == item.ServiceHeaderId).FirstOrDefault();
                            ServicOrdList.Add(item.ServiceHeaderId, item.Item_Number);
                            Short_Text = ServHeadList.Title;
                            Long_Text = ServHeadList.Description;
                            itemStructure.PO_ITEM = item.Item_Number.ToString();//item.Item_Number;
                        }
                        else
                        {
                            Short_Text = item.Short_Text;
                            Long_Text = item.Long_Text;
                            itemStructure.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                        }

                      
                        itemStructure.PO_NUMBER = "";
                        
                        itemStructure.DEL_IND = "";//if we delete item mark it as X
                        itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"
                        if (item.ItemType == "ServiceOrder" || item.ItemType == "ExpenseOrder")
                        {
                            itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
                            itemStructure.MATERIAL = "";
                            if (!string.IsNullOrEmpty(Short_Text) && Short_Text.Length > 40)
                                itemStructure.SHORT_TEXT = Short_Text.Substring(0, 40);
                            else
                                itemStructure.SHORT_TEXT = string.IsNullOrEmpty(Short_Text) ? "" : Short_Text;
                            itemStructure.QUANTITY = "1";
                            itemStructure.PO_UNIT = "AU";
                            itemStructure.MATL_GROUP = "17";
                        }
                        else
                        {
                            itemStructure.ACCTASSCAT = "Q";//material "Q", Service & Expense "P"
                            itemStructure.MATERIAL = item.Material_Number;
                            itemStructure.SHORT_TEXT = "";
                            itemStructure.QUANTITY = item.Order_Qty;
                            itemStructure.PO_UNIT = "";
                            itemStructure.MATL_GROUP = "";
                        }
                        itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
                        itemStructure.NET_PRICE = "";
                        itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
                        itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;

                        if (item.ItemType == "ServiceOrder")
                        {
                            itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
                            itemStructure.WBS_ELEMENT = "";
                            itemStructure.FUNDS_CTR = "";
                            itemStructure.CMMT_ITEM_LONG = "";
                            itemStructure.ITEM_CAT = "D";
                        }
                        else
                        {
                            itemStructure.GL_ACCOUNT = item.GLAccountNo;
                            itemStructure.WBS_ELEMENT = wbsPartOne;// item.wbselementcode part1;
                            itemStructure.FUNDS_CTR = fundCenter.FundCenter_Code;
                            if (commitmentItem != null)
                                itemStructure.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                            else
                                itemStructure.CMMT_ITEM_LONG = "";
                        }
                        itemStructure.ASSET_NO = "";
                        itemStructure.ORDER = "";//item.Purchasing_Order_Number
                        itemStructure.TAX_CODE = item.Tax_salespurchases_code;// item.Tax_salespurchases_code;
                        itemStructure.PREQ_NO = "";
                        itemStructure.PREQ_ITEM = "";
                        itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
                        itemStructure.SERVICE = "";
                        itemStructure.SQUANTITY = "";
                        itemStructure.GR_PRICE = "";
                        itemStructure.PRE_VENDOR = "";
                        if (item.ItemType == "ExpenseOrder")
                        {
                            itemStructure.LIMIT = item.TotalAmount.ToString();
                            itemStructure.EXP_VALUE = item.TotalAmount.ToString();
                            itemStructure.STEUC = item.HSNCode;//HSNCode
                            itemStructure.ITEM_CAT = "B";
                        }
                        else
                        {
                            itemStructure.LIMIT = "";
                            itemStructure.EXP_VALUE = "";
                            itemStructure.STEUC = "";
                        }

                        itemStructure.NO_LIMIT = "";
                        itemStructure.EXT_LINE = "";
                        itemStructure.BASE_UOM = "";
                        itemStructure.LINE_TEXT = "";
                        itemStructure.NETWORK = "";
                        itemStructure.OVF_TOL = "";
                        itemStructure.OVF_UNLIM = "";
                        itemStructure.NO_MORE_GR = "";
                        itemStructure.RET_ITEM = "";
                        itemStructure.TEXT_LINE = Long_Text;//more description
                        itemStructure.KOSTL = "";

                        itemStructureList.Add(itemStructure);
                    }
                        if (item.ItemType == "MaterialOrder")
                        {
                            PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                            itemCondition.KBETR = item.Rate.ToString();
                            itemCondition.KPOSN = "";// "1";
                            itemCondition.KSCHL = "P001";
                            itemCondition.LIFNR = "";
                            itemCondition.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                            itemConditionList.Add(itemCondition);
                        }
                       else if (item.ItemType == "ServiceOrder")
                        {
                            PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                            item2.PO_ITEM = item.Item_Number.ToString();//item.Item_Number;
                            item2.LINES = item.SAPItemSeqNo.ToString();
                            item2.SERVICE = item.Material_Number;
                            item2.SQUANTITY = item.Order_Qty;
                            item2.GR_PRICE = item.Rate.ToString();
                            item2.BASE_UOM = "";
                            item2.LINE_TEXT = "";//"" if we delete then "X"
                            item2.OVF_TOL = "";
                            item2.OVF_UNLIM1 = "";
                            item2.MATKL = "";
                            item2.WBS_ELEMENT = wbsPartOne;
                            item2.GL_ACCOUNT = item.GLAccountNo;
                            item2.ORDER = "";
                            item2.FUNDS_CTR = fundCenter.FundCenter_Code;
                            if (commitmentItem != null)
                                item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                            else
                                item2.CMMT_ITEM_LONG = "";
                            if (!string.IsNullOrEmpty(item.Long_Text) && item.Long_Text.Length > 130)
                                item2.LONG_TEXT = item.Long_Text.Substring(0, 130);
                            else
                                item2.LONG_TEXT = string.IsNullOrEmpty(item.Long_Text) ? "" : item.Long_Text;
                            item2List.Add(item2);
                        }
                        count++;
                    }
                }
            //}
            #endregion

            poReq.Header = poHeaderStructure;
            poReq.item = itemStructureList.ToArray();
            if (itemConditionList.Count == 0)
            {
                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                itemCondition.KBETR = "";
                itemCondition.KPOSN = "";
                itemCondition.KSCHL = "";
                itemCondition.LIFNR = "";
                itemCondition.PO_ITEM = "";
                itemConditionList.Add(itemCondition);
            }
            poReq.item1 = itemConditionList.ToArray();
            if (item2List.Count == 0)
            {
                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                item2.BASE_UOM = "";
                item2.CMMT_ITEM_LONG = "";
                item2.FUNDS_CTR = "";
                item2.GL_ACCOUNT = "";
                item2.GR_PRICE = "";
                item2.LINES = "";
                item2.LINE_TEXT = "";
                item2.LONG_TEXT = "";
                item2.MATKL = "";
                item2.ORDER = "";
                item2.OVF_TOL = "";
                item2.OVF_UNLIM1 = "";
                item2.PO_ITEM = "";
                item2.SERVICE = "";
                item2.SQUANTITY = "";
                item2.WBS_ELEMENT = "";
                item2List.Add(item2);
            }
            poReq.Item2 = item2List.ToArray();
            SAPCallConnector sapCall = new SAPCallConnector();
            IList<PurchaseOrderResItem> RespList = new List<PurchaseOrderResItem>();
            RespList = sapCall.CreatePO(poReq);
            int Count = 0; bool CheckValid = true; String ValidMessage = String.Empty;
            int[] ValidVal = { 0, 0, 0 };
            foreach (PurchaseOrderResItem item in RespList)
            {
                Count++;
                //For Validation Check 
                if (Validation_Check)
                {
                    if(item.RETCODE == "1" || !CheckValid)
                    {
                        CheckValid = false;
                        sapRes.ReturnCode = "1";
                        sapRes.PONumber = "";
                        if (Count > 1)
                            sapRes.Message += "<br/> ";
                        sapRes.Message += Count + "." + item.MESSAGE;

                        if ((item.MESSAGE.Contains("Vendor") && item.MESSAGE.Contains("blocked")) && ValidVal[0] != 1)
                        {
                            ValidVal[0] = 1;
                            ValidMessage += "For Vendor blocked--> contact Accounts Team<br/>";
                        }

                        if ((item.MESSAGE.Contains("<script>")) && ValidVal[1] != 1)
                        {
                            ValidVal[1] = 1;
                            sapRes.Message = "There is communication failure between FUGUE and SAP. Please try after sometime.<br/>";
                        }

                        if ((item.MESSAGE.Contains("material") && item.MESSAGE.Contains("does not exist")) && ValidVal[2] != 1)
                        {
                            ValidVal[2] = 1;
                            ValidMessage += "Material / Service not available in SAP please contact IT Team.<br/>";
                        }

                        if (log.IsDebugEnabled)
                            log.Debug("Leaving From PO SAP Call: Failed " + item.MESSAGE);

                    }
                    else
                    {
                        sapRes.ReturnCode = "0";
                        sapRes.PONumber = "";
                       
                    }
                }
                else
                {
                    if (item.RETCODE == "0" && CheckValid)
                    {
                        PONumber = item.PO_NUMBER;
                        headerStructure.Purchasing_Order_Number = PONumber;
                        context.Entry(headerStructure).CurrentValues.SetValues(headerStructure);
                        context.SaveChanges();
                        foreach (TEPOItemStructure itmStrc in itemList)
                        {
                            var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                            itemStrcSAP.IsRecordInSAP = true;
                            itemStrcSAP.SAPTransactionCode = "P";
                            itemStrcSAP.Purchasing_Order_Number = PONumber;
                            context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                            context.SaveChanges();
                        }
                        sapRes.ReturnCode = item.RETCODE;
                        sapRes.PONumber = item.PO_NUMBER;
                        sapRes.Message = "Success";
                        if (log.IsDebugEnabled)
                            log.Debug("Leaving From PO SAP Call: Success " + PONumber);
                    }
                    else
                    {
                        CheckValid = false;
                        foreach (TEPOItemStructure itmStrc in itemList)
                        {
                            var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                            if (itemStrcSAP.IsRecordInSAP == false)
                            {
                                itemStrcSAP.SAPItemSeqNo = 0;
                                itemStrcSAP.Item_Number = "0";
                                context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                                context.SaveChanges();
                            }
                        }
                        ApplicationErrorLog errorlog = new ApplicationErrorLog();
                        errorlog.InnerException = headerStructure.Uniqueid.ToString();
                        errorlog.Stacktrace = item.MESSAGE;
                        errorlog.Source = "PO Posting";
                        errorlog.Error = item.MESSAGE;
                        errorlog.ExceptionDateTime = DateTime.Now;
                        context.ApplicationErrorLogs.Add(errorlog);
                        context.SaveChanges();

                        sapRes.ReturnCode = "1";
                        sapRes.PONumber = "";
                        if (Count > 1)
                            sapRes.Message += "<br/> ";
                        sapRes.Message += Count + "." + item.MESSAGE;

                        if ((item.MESSAGE.Contains("Vendor") && item.MESSAGE.Contains("blocked")) && ValidVal[0] != 1)
                        {
                            ValidVal[0] = 1;
                            ValidMessage += "For Vendor blocked--> contact Accounts Team<br/>";
                        }

                        if ((item.MESSAGE.Contains("<script>")) && ValidVal[1] != 1)
                        {
                            ValidVal[1] = 1;
                            sapRes.Message = "There is communication failure between FUGUE and SAP. Please try after sometime.<br/>";
                        }

                        if ((item.MESSAGE.Contains("material") && item.MESSAGE.Contains("does not exist")) && ValidVal[2] != 1)
                        {
                            ValidVal[2] = 1;
                            ValidMessage += "Material / Service not available in SAP please contact IT Team.<br/>";
                        }

                        if (log.IsDebugEnabled)
                            log.Debug("Leaving From PO SAP Call: Failed " + item.MESSAGE);
                    }
                }
            }
            String TempString = sapRes.Message;
            sapRes.Message = ValidMessage + TempString;

            return sapRes;
        }

        public SAPResponse UpdatePODetailsToSAP(int purchaseHeaderID, bool Validation_Check = false)
        {
            string PONumber = string.Empty;
            string CompanyCode = string.Empty;
            string VendorAccountNo = string.Empty;
            string DocumentTypeCode = string.Empty;
            string Currency = string.Empty;
            string ProjectCode = string.Empty;
            int wbsParts = 0;
            string wbsPartOne = string.Empty;
            SAPResponse sapRes = new SAPResponse();

            var HeadOrderType = (from Head in context.TEPOHeaderStructures
                                 join OrdType in context.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == purchaseHeaderID
                                 select OrdType.Code).FirstOrDefault();

            PurchaseOrderReqHeader poHeaderStructure = new PurchaseOrderReqHeader();
            PurchaseOrderReq poReq = new PurchaseOrderReq();
            context.Configuration.ProxyCreationEnabled = true;
            TEPOHeaderStructure headerStructure = new TEPOHeaderStructure();
            List<TEPOItemStructure> itemList = new List<TEPOItemStructure>();
            List<TEPOItemStructure> itemListDel = new List<TEPOItemStructure>();

            var POWBSPartValue = context.TEMasterRules.Where(a => a.RuleName.Contains("POWBSPart1") && a.IsDeleted == false).FirstOrDefault();
            if (POWBSPartValue != null)
            {
                wbsParts = Convert.ToInt32(POWBSPartValue.RuleValue);
            }

            headerStructure = context.TEPOHeaderStructures.Where(a => a.Uniqueid == purchaseHeaderID && a.IsDeleted == false).FirstOrDefault();
            if (headerStructure != null)
            {
                var CmpnyVendCode = (from purHead in context.TEPOHeaderStructures
                                     join proj in context.TEProjects on purHead.ProjectID equals proj.ProjectID
                                     join cmpny in context.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                                     join vendordtl in this.context.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                                     join vendor in this.context.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                     join orderType in context.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                                     where
                                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                                      //&& vendordtl.IsDeleted == false && vendor.IsDeleted == false && orderType.IsDeleted == false &&
                                      purHead.Uniqueid == headerStructure.Uniqueid
                                     select new
                                     {
                                         cmpny.CompanyCode,
                                         vendordtl.VendorCode,
                                         vendor.Currency,
                                         orderType.Code,
                                         proj.ProjectCode
                                     }).FirstOrDefault();
                if (CmpnyVendCode != null)
                {
                    CompanyCode = CmpnyVendCode.CompanyCode;
                    VendorAccountNo = CmpnyVendCode.VendorCode;
                    DocumentTypeCode = CmpnyVendCode.Code;
                    Currency = CmpnyVendCode.Currency;
                    ProjectCode = CmpnyVendCode.ProjectCode;
                }
            }
            var pomanagerObj = context.UserProfiles.Where(a => a.UserId == headerStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
            var plantStorage = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
            var shippingLocation = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
            var fundCenter = context.TEPOFundCenters.Where(a => a.Uniqueid == headerStructure.FundCenterID && a.IsDeleted == false).FirstOrDefault();

           
            DateTime docDate = Convert.ToDateTime(headerStructure.Purchasing_Document_Date);
            //DateTime docDate = DateTime.Today;

            if (Validation_Check)
                poHeaderStructure.TESTRUN = "X";
            else
                poHeaderStructure.TESTRUN = "";

            poHeaderStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            poHeaderStructure.F_PONUMBER = headerStructure.Fugue_Purchasing_Order_Number;
            poHeaderStructure.COMP_CODE = CompanyCode;
            poHeaderStructure.DOC_TYPE = DocumentTypeCode;
            poHeaderStructure.VENDOR = VendorAccountNo;
            poHeaderStructure.PURCH_ORG = "1000";
            poHeaderStructure.PUR_GROUP = "001";
            poHeaderStructure.CURRENCY = Currency;
            poHeaderStructure.DOC_DATE = string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.PMNTTRMS = "T041";
            poHeaderStructure.REF_1 = ".";
            poHeaderStructure.OUR_REF = ".";
            poHeaderStructure.TELEPHONE = ".";
            poHeaderStructure.COMPLETED = "X";//second time onwards "X" has to send
            poHeaderStructure.REASON = "0003";//second time onwards "0003" has to send
            poHeaderStructure.EXCH_RATE = "";// headerStructure.Exchange_Rate;
            if (pomanagerObj != null)
                poHeaderStructure.SALES_PERS = pomanagerObj.UserName;
            else
                poHeaderStructure.SALES_PERS = "Admin";
            poHeaderStructure.ZPOTITLE = headerStructure.PO_Title;
            poHeaderStructure.ZAGSIGNDT = "";// string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.FUGUE_ID = headerStructure.Uniqueid.ToString();
            poHeaderStructure.TEXT_LINE = "";
            poHeaderStructure.ZPROJECT = ProjectCode;

            List<PurchaseOrderReqItem> itemStructureList = new List<PurchaseOrderReqItem>();
            List<PurchaseOrderReqItem1> itemConditionList = new List<PurchaseOrderReqItem1>();
            List<PurchaseOrderReqItem2> item2List = new List<PurchaseOrderReqItem2>();


            #region Old Service Order Code -- Author:Kamal Commented by: kamal
            //if (ServHeadList.Count() > 0 && HeadOrderType == "NB" )
            //{
            //    int HeadCount = 0;
            //    foreach (TEPOServiceHeader ServHead in ServHeadList)
            //    {
            //        HeadCount++;
            //        if (ServHead.IsDeleted == false)
            //        { 
            //        itemList = context.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderID && a.ServiceHeaderId == ServHead.UniqueID
            //                                                      && a.IsDeleted == false).OrderBy(x => x.FugueItemSeqNo).ToList();
            //        //itemListDel = context.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderID && a.ServiceHeaderId == ServHead.UniqueID
            //        //                                 && a.IsDeleted == true && a.SAPTransactionCode == "X").ToList();
            //        //if (itemList.Count > 0)
            //        //{
            //        //    if (itemListDel.Count > 0)
            //        //        itemList.AddRange(itemListDel);
            //        //}

            //        if (itemList.Count > 0)
            //        {
            //            int count = 1;
            //            foreach (TEPOItemStructure item in itemList)
            //            {
            //                var commitmentItem = context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
            //                //wbsPartOne = wbsResult(item.WBSElementCode, wbsParts + 1);
            //                if (!string.IsNullOrEmpty(item.WBSElementCode))
            //                {
            //                    if (item.WBSElementCode.Length > wbsParts)
            //                    {
            //                        wbsPartOne = item.WBSElementCode.Substring(0, wbsParts);
            //                    }
            //                    else
            //                    {
            //                        wbsPartOne = item.WBSElementCode;
            //                    }
            //                }

            //                if (count == 1)
            //                {
            //                    PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
            //                    itemStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            //                    itemStructure.PO_ITEM = HeadCount.ToString();//item.Item_Number;


            //                    //Its in Pending..

            //                    if (item.SAPTransactionCode == "X")
            //                        itemStructure.DEL_IND = "X";//if we delete item mark it as X
            //                    else if (item.SAPTransactionCode == "C" || item.IsRecordInSAP == false)
            //                        itemStructure.DEL_IND = "C";
            //                    else
            //                        itemStructure.DEL_IND = "";

            //                    //if (ServHead.IsDeleted == false)
            //                    //    itemStructure.DEL_IND = "C";
            //                    //else if(ServHead.IsDeleted == true)
            //                    //    itemStructure.DEL_IND = "X";

            //                    //Its in Pending..

            //                    itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"

            //                    itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
            //                    itemStructure.MATERIAL = "";
            //                    if (!string.IsNullOrEmpty(ServHead.Title) && ServHead.Title.Length > 40)
            //                        itemStructure.SHORT_TEXT = ServHead.Title.Substring(0, 40);
            //                    else
            //                        itemStructure.SHORT_TEXT = string.IsNullOrEmpty(ServHead.Title) ? "" : ServHead.Title;
            //                    itemStructure.QUANTITY = "1";
            //                    itemStructure.PO_UNIT = "AU";
            //                    itemStructure.MATL_GROUP = "17";

            //                    itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
            //                    itemStructure.NET_PRICE = "";

            //                    itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
            //                    itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;

            //                    itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
            //                    itemStructure.WBS_ELEMENT = "";
            //                    itemStructure.FUNDS_CTR = "";
            //                    itemStructure.CMMT_ITEM_LONG = "";
            //                    itemStructure.ITEM_CAT = "D";

            //                    itemStructure.ASSET_NO = "";
            //                    itemStructure.ORDER = "";//item.Purchasing_Order_Number
            //                    itemStructure.TAX_CODE = item.Tax_salespurchases_code;
            //                    itemStructure.PREQ_NO = "";
            //                    itemStructure.PREQ_ITEM = "";
            //                    itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
            //                    itemStructure.SERVICE = "";
            //                    itemStructure.SQUANTITY = "";
            //                    itemStructure.GR_PRICE = "";
            //                    itemStructure.PRE_VENDOR = "";

            //                    itemStructure.LIMIT = "";
            //                    itemStructure.EXP_VALUE = "";
            //                    itemStructure.STEUC = item.HSNCode;


            //                    itemStructure.NO_LIMIT = "";
            //                    itemStructure.EXT_LINE = "";
            //                    itemStructure.BASE_UOM = "";
            //                    itemStructure.LINE_TEXT = "";
            //                    itemStructure.NETWORK = "";
            //                    itemStructure.OVF_TOL = "";
            //                    itemStructure.OVF_UNLIM = "";
            //                    itemStructure.NO_MORE_GR = "";
            //                    itemStructure.RET_ITEM = "";
            //                    itemStructure.TEXT_LINE = ServHead.Description;//more description
            //                    itemStructure.KOSTL = "";

            //                    itemStructureList.Add(itemStructure);


            //                }
            //                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
            //                item2.PO_ITEM = HeadCount.ToString();//item.Item_Number;
            //                item2.LINES = item.SAPItemSeqNo.ToString();
            //                item2.SERVICE = item.Material_Number;
            //                item2.SQUANTITY = item.Order_Qty;
            //                item2.GR_PRICE = item.Rate.ToString();
            //                item2.BASE_UOM = "";
            //                if (item.SAPTransactionCode == "X")
            //                    item2.LINE_TEXT = "X";//"" if we delete then "X"
            //                else
            //                    item2.LINE_TEXT = "";
            //                item2.OVF_TOL = "";
            //                item2.OVF_UNLIM1 = "";
            //                item2.MATKL = "";
            //                item2.WBS_ELEMENT = wbsPartOne;
            //                item2.GL_ACCOUNT = item.GLAccountNo;
            //                item2.ORDER = "";
            //                item2.FUNDS_CTR = fundCenter.FundCenter_Code;
            //                if (commitmentItem != null)
            //                    item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
            //                else
            //                    item2.CMMT_ITEM_LONG = "";
            //                if (!string.IsNullOrEmpty(item.Long_Text) && item.Long_Text.Length > 130)
            //                    item2.LONG_TEXT = item.Long_Text.Substring(0, 130);
            //                else
            //                    item2.LONG_TEXT = string.IsNullOrEmpty(item.Long_Text) ? "" : item.Long_Text;
            //                item2List.Add(item2);
            //                count++;
            //            }
            //        }

            //        else
            //        {
            //            PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
            //            itemStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            //            itemStructure.PO_ITEM = HeadCount.ToString();//item.Item_Number;




            //            if (ServHead.IsDeleted == false)
            //                itemStructure.DEL_IND = "C";
            //            else if (ServHead.IsDeleted == true)
            //                itemStructure.DEL_IND = "X";

            //            itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"

            //            itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
            //            itemStructure.MATERIAL = "";
            //            if (!string.IsNullOrEmpty(ServHead.Title) && ServHead.Title.Length > 40)
            //                itemStructure.SHORT_TEXT = ServHead.Title.Substring(0, 40);
            //            else
            //                itemStructure.SHORT_TEXT = string.IsNullOrEmpty(ServHead.Title) ? "" : ServHead.Title;
            //            itemStructure.QUANTITY = "1";
            //            itemStructure.PO_UNIT = "AU";
            //            itemStructure.MATL_GROUP = "17";

            //            itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
            //            itemStructure.NET_PRICE = "";

            //            itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
            //            itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;

            //            itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
            //            itemStructure.WBS_ELEMENT = "";
            //            itemStructure.FUNDS_CTR = "";
            //            itemStructure.CMMT_ITEM_LONG = "";
            //            itemStructure.ITEM_CAT = "D";

            //            itemStructure.ASSET_NO = "";
            //            itemStructure.ORDER = "";//item.Purchasing_Order_Number
            //            itemStructure.TAX_CODE = "";
            //            itemStructure.PREQ_NO = "";
            //            itemStructure.PREQ_ITEM = "";
            //            itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
            //            itemStructure.SERVICE = "";
            //            itemStructure.SQUANTITY = "";
            //            itemStructure.GR_PRICE = "";
            //            itemStructure.PRE_VENDOR = "";

            //            itemStructure.LIMIT = "";
            //            itemStructure.EXP_VALUE = "";
            //            itemStructure.STEUC = "";


            //            itemStructure.NO_LIMIT = "";
            //            itemStructure.EXT_LINE = "";
            //            itemStructure.BASE_UOM = "";
            //            itemStructure.LINE_TEXT = "";
            //            itemStructure.NETWORK = "";
            //            itemStructure.OVF_TOL = "";
            //            itemStructure.OVF_UNLIM = "";
            //            itemStructure.NO_MORE_GR = "";
            //            itemStructure.RET_ITEM = "";
            //            itemStructure.TEXT_LINE = ServHead.Description;//more description
            //            itemStructure.KOSTL = "";

            //            itemStructureList.Add(itemStructure);

            //        }
            //    }
            //    }
            //}
            //else
            //{
            #endregion
            Dictionary<int?, String> ServicOrdList = new Dictionary<int?, String>();
            List<int?> ServiceNumber = new List<int?>();
            List<TEPOItemStructure> SeqItems = new List<TEPOItemStructure>();
            List<TEPOItemStructure> SeqItems1 = new List<TEPOItemStructure>();
            int Mat_Seq = 1;
            #region Service Material and Expense Order
            itemList = context.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderID && a.IsDeleted == false).ToList();


            foreach (String itemNum in itemList.Select(x => x.Item_Number))
            {
                int IntNum = Convert.ToInt32(itemNum);
                if (Mat_Seq < IntNum)
                    Mat_Seq = IntNum;
            }

            for (int i = 1; i <= Mat_Seq; i++)
            {
                List<TEPOItemStructure> TempItems = itemList.Where(x => x.Item_Number == i.ToString()).ToList();
                SeqItems1.AddRange(TempItems);
            }

            foreach (TEPOItemStructure TEItems in SeqItems1)
            {
                if(TEItems.ItemType.Equals("ServiceOrder"))
                {
                    int? HeadID = TEItems.ServiceHeaderId;
                    if (!ServiceNumber.Contains(HeadID))
                    {
                        List<TEPOItemStructure> TempList = context.TEPOItemStructures.Where(x => x.ServiceHeaderId == HeadID && x.IsDeleted == false).OrderBy(x => x.Uniqueid).ToList();
                        SeqItems.AddRange(TempList);
                        ServiceNumber.Add(HeadID);
                    }
                }
                else
                    SeqItems.Add(TEItems);

            }

            if (SeqItems.Count > 0)
            {
                int count = 1;
                foreach (TEPOItemStructure item in SeqItems)
                {
                    TEPOServiceHeader ServHeadList = new TEPOServiceHeader();
                    String Short_Text = String.Empty;
                    String Long_Text = String.Empty;
                    bool ServContain = true;
                    var commitmentItem = context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
                    //wbsPartOne = wbsResult(item.WBSElementCode, wbsParts + 1);
                    if (!string.IsNullOrEmpty(item.WBSElementCode))
                    {
                        if (item.WBSElementCode.Length > wbsParts)
                            wbsPartOne = item.WBSElementCode.Substring(0, wbsParts);
                        else
                            wbsPartOne = item.WBSElementCode;
                    }

                    if(ServicOrdList.Count > 0)
                    {
                        if (ServicOrdList.ContainsKey(item.ServiceHeaderId))
                            ServContain = false;
                        else
                            ServContain = true;
                    }

                    if (ServContain || item.ItemType != "ServiceOrder")
                    {
                        PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
                        if (item.ItemType == "ServiceOrder")
                        {
                            ServHeadList = context.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == purchaseHeaderID && x.UniqueID == item.ServiceHeaderId).FirstOrDefault();
                            var TEPOItem = context.TEPOItemStructures.Where(x => x.ServiceHeaderId == ServHeadList.UniqueID  && (x.IsRecordInSAP == true)).ToList();

                            if (TEPOItem.Count == 0)
                                itemStructure.DEL_IND = "C";
                            else
                                itemStructure.DEL_IND = "";

                            ServicOrdList.Add(item.ServiceHeaderId, item.Item_Number);
                            Short_Text = ServHeadList.Title;
                            Long_Text = ServHeadList.Description;
                            itemStructure.PO_ITEM = item.Item_Number.ToString();//item.Item_Number;

                        }
                        else
                        {
                            Short_Text = item.Short_Text;
                            Long_Text = item.Long_Text;
                            itemStructure.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;

                            if (item.SAPTransactionCode == "X")
                                itemStructure.DEL_IND = "X";//if we delete item mark it as X
                            else if (item.SAPTransactionCode == "C")
                                itemStructure.DEL_IND = "C";
                            else
                                itemStructure.DEL_IND = "";

                        }

                        itemStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
                        //itemStructure.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                       
                        itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"
                        if (item.ItemType == "ServiceOrder" || item.ItemType == "ExpenseOrder")
                        {
                            itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
                            itemStructure.MATERIAL = "";
                            if (!string.IsNullOrEmpty(Short_Text) && Short_Text.Length > 40)
                                itemStructure.SHORT_TEXT = Short_Text.Substring(0, 40);
                            else
                                itemStructure.SHORT_TEXT = string.IsNullOrEmpty(Short_Text) ? "" : Short_Text;
                            itemStructure.QUANTITY = "1";
                            itemStructure.PO_UNIT = "AU";
                            itemStructure.MATL_GROUP = "17";
                        }
                        else
                        {
                            itemStructure.ACCTASSCAT = "Q";//material "Q", Service & Expense "P"
                            itemStructure.MATERIAL = item.Material_Number;
                            itemStructure.SHORT_TEXT = "";
                            itemStructure.QUANTITY = item.Order_Qty;
                            itemStructure.PO_UNIT = "";
                            itemStructure.MATL_GROUP = "";
                        }
                        itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
                        itemStructure.NET_PRICE = "";

                        itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
                        itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;
                        if (item.ItemType == "ServiceOrder")
                        {
                            itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
                            itemStructure.WBS_ELEMENT = "";
                            itemStructure.FUNDS_CTR = "";
                            itemStructure.CMMT_ITEM_LONG = "";
                            itemStructure.ITEM_CAT = "D";
                        }
                        else
                        {
                            itemStructure.GL_ACCOUNT = item.GLAccountNo;
                            itemStructure.WBS_ELEMENT = wbsPartOne;// item.wbselementcode part1;
                            itemStructure.FUNDS_CTR = fundCenter.FundCenter_Code;
                            if (commitmentItem != null)
                                itemStructure.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                            else
                                itemStructure.CMMT_ITEM_LONG = "";
                        }
                        itemStructure.ASSET_NO = "";
                        itemStructure.ORDER = "";//item.Purchasing_Order_Number
                        itemStructure.TAX_CODE = item.Tax_salespurchases_code;
                        itemStructure.PREQ_NO = "";
                        itemStructure.PREQ_ITEM = "";
                        itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
                        itemStructure.SERVICE = "";
                        itemStructure.SQUANTITY = "";
                        itemStructure.GR_PRICE = "";
                        itemStructure.PRE_VENDOR = "";
                        if (item.ItemType == "ExpenseOrder")
                        {
                            itemStructure.LIMIT = item.TotalAmount.ToString();
                            itemStructure.EXP_VALUE = item.TotalAmount.ToString();
                            itemStructure.STEUC = item.HSNCode;//HSNCode
                            itemStructure.ITEM_CAT = "B";
                        }
                        else
                        {
                            itemStructure.LIMIT = "";
                            itemStructure.EXP_VALUE = "";
                            itemStructure.STEUC = "";
                        }

                        itemStructure.NO_LIMIT = "";
                        itemStructure.EXT_LINE = "";
                        itemStructure.BASE_UOM = "";
                        itemStructure.LINE_TEXT = "";
                        itemStructure.NETWORK = "";
                        itemStructure.OVF_TOL = "";
                        itemStructure.OVF_UNLIM = "";
                        itemStructure.NO_MORE_GR = "";
                        itemStructure.RET_ITEM = "";
                        itemStructure.TEXT_LINE = Long_Text;//more description
                        itemStructure.KOSTL = "";

                        itemStructureList.Add(itemStructure);

                        if (item.ItemType == "MaterialOrder")
                        {
                            PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                            itemCondition.KBETR = item.Rate.ToString();
                            itemCondition.KPOSN = "";// "1";
                            itemCondition.KSCHL = "P001";
                            itemCondition.LIFNR = "";
                            itemCondition.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                            itemConditionList.Add(itemCondition);
                        }
                    }
                        if (item.ItemType == "ServiceOrder")
                        {
                            PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                            item2.PO_ITEM = item.Item_Number.ToString();//item.Item_Number;
                            item2.LINES = item.SAPItemSeqNo.ToString();
                            item2.SERVICE = item.Material_Number;
                            item2.SQUANTITY = item.Order_Qty;
                            item2.GR_PRICE = item.Rate.ToString();
                            item2.BASE_UOM = "";
                            if (item.SAPTransactionCode == "X")
                                item2.LINE_TEXT = "X";//"" if we delete then "X"
                            else
                                item2.LINE_TEXT = "";
                            item2.OVF_TOL = "";
                            item2.OVF_UNLIM1 = "";
                            item2.MATKL = "";
                            item2.WBS_ELEMENT = wbsPartOne;
                            item2.GL_ACCOUNT = item.GLAccountNo;
                            item2.ORDER = "";
                            item2.FUNDS_CTR = fundCenter.FundCenter_Code;
                            if (commitmentItem != null)
                                item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                            else
                                item2.CMMT_ITEM_LONG = "";
                            if (!string.IsNullOrEmpty(item.Long_Text) && item.Long_Text.Length > 130)
                                item2.LONG_TEXT = item.Long_Text.Substring(0, 130);
                            else
                                item2.LONG_TEXT = string.IsNullOrEmpty(item.Long_Text) ? "" : item.Long_Text;
                            item2List.Add(item2);
                        }
                        count++;
                    
                }
            }
                #endregion
            //}
            poReq.Header = poHeaderStructure;
            poReq.item = itemStructureList.ToArray();
            if (itemConditionList.Count == 0)
            {
                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                itemCondition.KBETR = "";
                itemCondition.KPOSN = "";
                itemCondition.KSCHL = "";
                itemCondition.LIFNR = "";
                itemCondition.PO_ITEM = "";
                itemConditionList.Add(itemCondition);
            }
            poReq.item1 = itemConditionList.ToArray();
            if (item2List.Count == 0)
            {
                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                item2.BASE_UOM = "";
                item2.CMMT_ITEM_LONG = "";
                item2.FUNDS_CTR = "";
                item2.GL_ACCOUNT = "";
                item2.GR_PRICE = "";
                item2.LINES = "";
                item2.LINE_TEXT = "";
                item2.LONG_TEXT = "";
                item2.MATKL = "";
                item2.ORDER = "";
                item2.OVF_TOL = "";
                item2.OVF_UNLIM1 = "";
                item2.PO_ITEM = "";
                item2.SERVICE = "";
                item2.SQUANTITY = "";
                item2.WBS_ELEMENT = "";
                item2List.Add(item2);
            }
            poReq.Item2 = item2List.ToArray();
            SAPCallConnector sapCall = new SAPCallConnector();
            IList<PurchaseOrderResItem> RespList = new List<PurchaseOrderResItem>();

            RespList = sapCall.CreatePO(poReq);
            bool Check_Valid = true;
            String PONUmber = String.Empty;
            String RETCODE = String.Empty;
            String MESSAGE = String.Empty;
            int count_1 = 0;
            String ValidMessage = String.Empty;
            int[] ValidVal = { 0, 0, 0};
            foreach (PurchaseOrderResItem item in RespList)
            {
                count_1++;
                //For Validation Check 
                if (Validation_Check)
                {
                    if (item.RETCODE == "1" || !Check_Valid)
                    {
                        Check_Valid = false;
                        sapRes.ReturnCode = "1";
                        sapRes.PONumber = "";
                        if (count_1 > 1)
                            sapRes.Message += "<br/>";
                        sapRes.Message += count_1 + "."+ item.MESSAGE;
                        if ((item.MESSAGE.Contains("Vendor") && item.MESSAGE.Contains("blocked")) && ValidVal[0] != 1)
                        {
                            ValidVal[0] = 1;
                            ValidMessage += "For Vendor blocked--> contact Accounts Team <br/>";
                        }

                        if ((item.MESSAGE.Contains("<script>")) && ValidVal[1] != 1)
                        {
                            ValidVal[1] = 1;
                            sapRes.Message = "There is communication failure between FUGUE and SAP. Please try after sometime. <br/>";
                        }

                        if ((item.MESSAGE.Contains("material") && item.MESSAGE.Contains("does not exist")) && ValidVal[2] != 1)
                        {
                            ValidVal[2] = 1;
                            ValidMessage += "Material / Service not available in SAP please contact IT Team.<br/>";
                        }

                        if (log.IsDebugEnabled)
                            log.Debug("Leaving From PO SAP Call: Failed " + item.MESSAGE);
                    }
                    else
                    {
                        sapRes.ReturnCode = "0";
                        sapRes.PONumber = "";
                        
                    }
                }
                else
                {
                    //Success
                    if (item.RETCODE == "0" && Check_Valid)
                    {
                        RETCODE = item.RETCODE;
                        PONUmber = item.PO_NUMBER;
                        Check_Valid = true;
                    }
                    //Unsuccessfull
                    else
                    {
                        Check_Valid = false;
                        RETCODE = item.RETCODE;

                        if (count_1 > 1)
                            MESSAGE += "<br/>";
                        MESSAGE += count_1 + "." + item.MESSAGE;

                        if ((item.MESSAGE.Contains("Vendor") && item.MESSAGE.Contains("blocked")) && ValidVal[0] != 1)
                        {
                            ValidVal[0] = 1;
                            ValidMessage += "For Vendor blocked--> contact Accounts Team<br/>";
                        }

                        if ((item.MESSAGE.Contains("<script>")) && ValidVal[1] != 1)
                        {
                            ValidVal[1] = 1;
                            sapRes.Message = "There is communication failure between FUGUE and SAP. Please try after sometime.<br/>";
                        }

                        if ((item.MESSAGE.Contains("material") && item.MESSAGE.Contains("does not exist")) && ValidVal[2] != 1)
                        {
                            ValidVal[2] = 1;
                            ValidMessage += "Material / Service not available in SAP please contact IT Team.<br/>";
                        }

                        if (log.IsDebugEnabled)
                            log.Debug("Leaving From PO SAP Call: Failed " + item.MESSAGE);
                    }
                }
            }

            if (!Validation_Check)
            {
                if (Check_Valid)
                {
                    
                    headerStructure.Purchasing_Order_Number = PONUmber;
                    context.Entry(headerStructure).CurrentValues.SetValues(headerStructure);
                    context.SaveChanges();

                    foreach (TEPOItemStructure itmStrc in itemList)
                    {
                        var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                        itemStrcSAP.IsRecordInSAP = true;
                        if (string.IsNullOrEmpty(itemStrcSAP.SAPTransactionCode))
                            itemStrcSAP.SAPTransactionCode = "P";
                        itemStrcSAP.Purchasing_Order_Number = PONUmber;
                        context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                        context.SaveChanges();
                    }

                    sapRes.ReturnCode = RETCODE;
                    sapRes.PONumber = PONUmber;
                    sapRes.Message = "Success";

                    if (log.IsDebugEnabled)
                        log.Debug("Leaving From PO SAP Call: Success " + PONumber);

                }
                else
                {
                    foreach (TEPOItemStructure itmStrc in itemList)
                    {
                        var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                        if (itemStrcSAP.IsRecordInSAP != true)
                        {
                            itemStrcSAP.SAPItemSeqNo = 0;
                            itemStrcSAP.Item_Number = "0";
                            itemStrcSAP.IsRecordInSAP = false;
                            context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                            context.SaveChanges();
                        }
                    }
                    ApplicationErrorLog errorlog = new ApplicationErrorLog();
                    errorlog.InnerException = headerStructure.Uniqueid.ToString();
                    errorlog.Stacktrace = MESSAGE;
                    errorlog.Source = "PO Posting";
                    errorlog.Error = MESSAGE;
                    errorlog.ExceptionDateTime = DateTime.Now;
                    context.ApplicationErrorLogs.Add(errorlog);
                    context.SaveChanges();

                    sapRes.ReturnCode = "1";
                    sapRes.PONumber = "";
                    sapRes.Message = MESSAGE;


                } 
                  
            }
            String TempString = sapRes.Message;
            sapRes.Message = ValidMessage + TempString;

            return sapRes;
        }



        #endregion

        #region Delete Option
        public SAPResponse DeletePODetailsToSAP(TEPOItemStructure existItem)
        {
            string PONumber = string.Empty;
            string CompanyCode = string.Empty;
            string VendorAccountNo = string.Empty;
            string DocumentTypeCode = string.Empty;
            string Currency = string.Empty;
            string ProjectCode = string.Empty;
            int wbsParts = 0;
            string wbsPartOne = string.Empty;
            SAPResponse sapRes = new SAPResponse();

            var HeadOrderType = (from Head in context.TEPOHeaderStructures
                                 join OrdType in context.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == existItem.POStructureId
                                 select OrdType.Code).FirstOrDefault();

            PurchaseOrderReqHeader poHeaderStructure = new PurchaseOrderReqHeader();
            PurchaseOrderReq poReq = new PurchaseOrderReq();
            context.Configuration.ProxyCreationEnabled = true;
            TEPOHeaderStructure headerStructure = new TEPOHeaderStructure();

            List<TEPOItemStructure> itemListDel = new List<TEPOItemStructure>();

            var POWBSPartValue = context.TEMasterRules.Where(a => a.RuleName.Contains("POWBSPart1") && a.IsDeleted == false).FirstOrDefault();
            if (POWBSPartValue != null)
            {
                wbsParts = Convert.ToInt32(POWBSPartValue.RuleValue);
            }

            headerStructure = context.TEPOHeaderStructures.Where(a => a.Uniqueid == existItem.POStructureId && a.IsDeleted == false).FirstOrDefault();
            if (headerStructure != null)
            {
                var CmpnyVendCode = (from purHead in context.TEPOHeaderStructures
                                     join proj in context.TEProjects on purHead.ProjectID equals proj.ProjectID
                                     join cmpny in context.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                                     join vendordtl in this.context.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                                     join vendor in this.context.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                     join orderType in context.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                                     where
                                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                                      //&& vendordtl.IsDeleted == false && vendor.IsDeleted == false && orderType.IsDeleted == false &&
                                      purHead.Uniqueid == headerStructure.Uniqueid
                                     select new
                                     {
                                         cmpny.CompanyCode,
                                         vendordtl.VendorCode,
                                         vendor.Currency,
                                         orderType.Code,
                                         proj.ProjectCode
                                     }).FirstOrDefault();
                if (CmpnyVendCode != null)
                {
                    CompanyCode = CmpnyVendCode.CompanyCode;
                    VendorAccountNo = CmpnyVendCode.VendorCode;
                    DocumentTypeCode = CmpnyVendCode.Code;
                    Currency = CmpnyVendCode.Currency;
                    ProjectCode = CmpnyVendCode.ProjectCode;
                }
            }
            var pomanagerObj = context.UserProfiles.Where(a => a.UserId == headerStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
            var plantStorage = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
            var shippingLocation = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
            var fundCenter = context.TEPOFundCenters.Where(a => a.Uniqueid == headerStructure.FundCenterID && a.IsDeleted == false).FirstOrDefault();


            DateTime docDate = Convert.ToDateTime(headerStructure.Purchasing_Document_Date);
            //DateTime docDate = DateTime.Today;
            poHeaderStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            poHeaderStructure.F_PONUMBER = headerStructure.Fugue_Purchasing_Order_Number;
            poHeaderStructure.COMP_CODE = CompanyCode;
            poHeaderStructure.DOC_TYPE = DocumentTypeCode;
            poHeaderStructure.VENDOR = VendorAccountNo;
            poHeaderStructure.PURCH_ORG = "1000";
            poHeaderStructure.PUR_GROUP = "001";
            poHeaderStructure.CURRENCY = Currency;
            poHeaderStructure.DOC_DATE = string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.PMNTTRMS = "T041";
            poHeaderStructure.REF_1 = ".";
            poHeaderStructure.OUR_REF = ".";
            poHeaderStructure.TELEPHONE = ".";
            poHeaderStructure.COMPLETED = "X";//second time onwards "X" has to send
            poHeaderStructure.REASON = "0003";//second time onwards "0003" has to send
            poHeaderStructure.EXCH_RATE = "";// headerStructure.Exchange_Rate;
            if (pomanagerObj != null)
                poHeaderStructure.SALES_PERS = pomanagerObj.UserName;
            else
                poHeaderStructure.SALES_PERS = "Admin";
            poHeaderStructure.ZPOTITLE = headerStructure.PO_Title;
            poHeaderStructure.ZAGSIGNDT = "";// string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.FUGUE_ID = headerStructure.Uniqueid.ToString();
            poHeaderStructure.TEXT_LINE = "";
            poHeaderStructure.ZPROJECT = ProjectCode;

            List<PurchaseOrderReqItem> itemStructureList = new List<PurchaseOrderReqItem>();
            List<PurchaseOrderReqItem1> itemConditionList = new List<PurchaseOrderReqItem1>();
            List<PurchaseOrderReqItem2> item2List = new List<PurchaseOrderReqItem2>();


            #region Service Material and Expense Order
            TEPOItemStructure item = new TEPOItemStructure();
            item = context.TEPOItemStructures.Where(a => a.POStructureId == existItem.POStructureId && a.Uniqueid == existItem.Uniqueid).FirstOrDefault();

            TEPOServiceHeader HeadItem = new Models.TEPOServiceHeader();

            if (item.ItemType == "ServiceOrder")
                HeadItem = context.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == existItem.POStructureId && x.UniqueID == existItem.ServiceHeaderId).FirstOrDefault();

            var commitmentItem = context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
            //wbsPartOne = wbsResult(item.WBSElementCode, wbsParts + 1);

            if (!string.IsNullOrEmpty(item.WBSElementCode))
            {
                if (item.WBSElementCode.Length > wbsParts)
                {
                    wbsPartOne = item.WBSElementCode.Substring(0, wbsParts);
                }
                else
                {
                    wbsPartOne = item.WBSElementCode;
                }
            }

            PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
            itemStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            if (item.ItemType == "ServiceOrder")
            {
                itemStructure.PO_ITEM = item.Item_Number;
                itemStructure.DEL_IND = "";
                
                //if(HeadItem.IsDeleted == true)
                //else
                //    itemStructure.DEL_IND = "C";
            }
            else
            {
                itemStructure.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                itemStructure.DEL_IND = "X";//if we delete item mark it as X
            }
            itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"
            if (item.ItemType == "ServiceOrder" || item.ItemType == "ExpenseOrder")
            {
                itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
                itemStructure.MATERIAL = "";
                if (item.ItemType == "ServiceOrder")
                {
                    if (!string.IsNullOrEmpty(HeadItem.Title) && HeadItem.Title.Length > 40)
                        itemStructure.SHORT_TEXT = HeadItem.Title.Substring(0, 40);
                    else
                        itemStructure.SHORT_TEXT = string.IsNullOrEmpty(HeadItem.Title) ? "" : HeadItem.Title;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.Short_Text) && item.Short_Text.Length > 40)
                        itemStructure.SHORT_TEXT = item.Short_Text.Substring(0, 40);
                    else
                        itemStructure.SHORT_TEXT = string.IsNullOrEmpty(item.Short_Text) ? "" : item.Short_Text;
                }
                itemStructure.QUANTITY = "1";
                itemStructure.PO_UNIT = "AU";
                itemStructure.MATL_GROUP = "17";
            }
            else
            {
                itemStructure.ACCTASSCAT = "Q";//material "Q", Service & Expense "P"
                itemStructure.MATERIAL = item.Material_Number;
                itemStructure.SHORT_TEXT = "";
                itemStructure.QUANTITY = item.Order_Qty;
                itemStructure.PO_UNIT = "";
                itemStructure.MATL_GROUP = "";
            }
            itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
            itemStructure.NET_PRICE = "";

            itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
            itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;
            if (item.ItemType == "ServiceOrder")
            {
                itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
                itemStructure.WBS_ELEMENT = "";
                itemStructure.FUNDS_CTR = "";
                itemStructure.CMMT_ITEM_LONG = "";
                itemStructure.ITEM_CAT = "D";
            }
            else
            {
                itemStructure.GL_ACCOUNT = item.GLAccountNo;
                itemStructure.WBS_ELEMENT = wbsPartOne;// item.wbselementcode part1;
                itemStructure.FUNDS_CTR = fundCenter.FundCenter_Code;
                if (commitmentItem != null)
                    itemStructure.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                else
                    itemStructure.CMMT_ITEM_LONG = "";
            }
            itemStructure.ASSET_NO = "";
            itemStructure.ORDER = "";//item.Purchasing_Order_Number
            itemStructure.TAX_CODE = item.Tax_salespurchases_code;
            itemStructure.PREQ_NO = "";
            itemStructure.PREQ_ITEM = "";
            itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
            itemStructure.SERVICE = "";
            itemStructure.SQUANTITY = "";
            itemStructure.GR_PRICE = "";
            itemStructure.PRE_VENDOR = "";
            if (item.ItemType == "ExpenseOrder")
            {
                itemStructure.LIMIT = item.TotalAmount.ToString();
                itemStructure.EXP_VALUE = item.TotalAmount.ToString();
                itemStructure.STEUC = item.HSNCode;//HSNCode
                itemStructure.ITEM_CAT = "B";
            }
            else
            {
                itemStructure.LIMIT = "";
                itemStructure.EXP_VALUE = "";
                itemStructure.STEUC = "";
            }

            itemStructure.NO_LIMIT = "";
            itemStructure.EXT_LINE = "";
            itemStructure.BASE_UOM = "";
            itemStructure.LINE_TEXT = "";
            itemStructure.NETWORK = "";
            itemStructure.OVF_TOL = "";
            itemStructure.OVF_UNLIM = "";
            itemStructure.NO_MORE_GR = "";
            itemStructure.RET_ITEM = "";
            if (item.ItemType == "ServiceOrder")
                itemStructure.TEXT_LINE = HeadItem.Description;//more description
            else
                itemStructure.TEXT_LINE = "";//more description
            itemStructure.KOSTL = "";

            itemStructureList.Add(itemStructure);

            if (item.ItemType == "MaterialOrder")
            {
                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                itemCondition.KBETR = item.Rate.ToString();
                itemCondition.KPOSN = "";// "1";
                itemCondition.KSCHL = "P001";
                itemCondition.LIFNR = "";
                itemCondition.PO_ITEM = item.SAPItemSeqNo.ToString();//item.Item_Number;
                itemConditionList.Add(itemCondition);
            }
            if (item.ItemType == "ServiceOrder")
            {
                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                item2.PO_ITEM = item.Item_Number;//item.Item_Number;
                item2.LINES = item.SAPItemSeqNo.ToString();
                item2.SERVICE = item.Material_Number;
                item2.SQUANTITY = item.Order_Qty;
                item2.GR_PRICE = item.Rate.ToString();
                item2.BASE_UOM = "";

                item2.LINE_TEXT = "X";//"" if we delete then "X"

                item2.OVF_TOL = "";
                item2.OVF_UNLIM1 = "";
                item2.MATKL = "";
                item2.WBS_ELEMENT = wbsPartOne;
                item2.GL_ACCOUNT = item.GLAccountNo;
                item2.ORDER = "";
                item2.FUNDS_CTR = fundCenter.FundCenter_Code;
                if (commitmentItem != null)
                    item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                else
                    item2.CMMT_ITEM_LONG = "";
                if (!string.IsNullOrEmpty(item.Long_Text) && item.Long_Text.Length > 130)
                    item2.LONG_TEXT = item.Long_Text.Substring(0, 130);
                else
                    item2.LONG_TEXT = string.IsNullOrEmpty(item.Long_Text) ? "" : item.Long_Text;
                item2List.Add(item2);
            }

            #endregion

            poReq.Header = poHeaderStructure;
            poReq.item = itemStructureList.ToArray();
            if (itemConditionList.Count == 0)
            {
                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                itemCondition.KBETR = "";
                itemCondition.KPOSN = "";
                itemCondition.KSCHL = "";
                itemCondition.LIFNR = "";
                itemCondition.PO_ITEM = "";
                itemConditionList.Add(itemCondition);
            }
            poReq.item1 = itemConditionList.ToArray();
            if (item2List.Count == 0)
            {
                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                item2.BASE_UOM = "";
                item2.CMMT_ITEM_LONG = "";
                item2.FUNDS_CTR = "";
                item2.GL_ACCOUNT = "";
                item2.GR_PRICE = "";
                item2.LINES = "";
                item2.LINE_TEXT = "";
                item2.LONG_TEXT = "";
                item2.MATKL = "";
                item2.ORDER = "";
                item2.OVF_TOL = "";
                item2.OVF_UNLIM1 = "";
                item2.PO_ITEM = "";
                item2.SERVICE = "";
                item2.SQUANTITY = "";
                item2.WBS_ELEMENT = "";
                item2List.Add(item2);
            }
            poReq.Item2 = item2List.ToArray();
            SAPCallConnector sapCall = new SAPCallConnector();
            IList<PurchaseOrderResItem> RespList = new List<PurchaseOrderResItem>();

            RespList = sapCall.CreatePO(poReq);
            foreach (PurchaseOrderResItem Repitem in RespList)
            {
                if (Repitem.RETCODE == "0")
                {
                    PONumber = Repitem.PO_NUMBER;
                    headerStructure.Purchasing_Order_Number = PONumber;
                    context.Entry(headerStructure).CurrentValues.SetValues(headerStructure);
                    context.SaveChanges();

                    var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
                    itemStrcSAP.IsRecordInSAP = true;
                    if (string.IsNullOrEmpty(itemStrcSAP.SAPTransactionCode))
                        itemStrcSAP.SAPTransactionCode = "P";
                    context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                    context.SaveChanges();

                    sapRes.ReturnCode = Repitem.RETCODE;
                    sapRes.PONumber = Repitem.PO_NUMBER;
                    sapRes.Message = "Success";

                    if (log.IsDebugEnabled)
                        log.Debug("Leaving From PO SAP Call: Success " + PONumber);

                    //Purchasing Document Number PO_NUMBER
                    //Fugue Purchasing Order Number   F_PONUMBER
                    //Item Number of Purchasing Document  PO_ITEM
                    //PO Item Net Value NETWR
                    //PO Item Gross Value BRTWR
                    //PO Item Tax Value NAVNW
                    //Return Code RETCODE
                    //Return Message MESSAGE
                    //Fugue Reference ID FUGUE_ID
                }
                else
                {

                    var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
                    if (itemStrcSAP.IsRecordInSAP != true)
                    {
                        itemStrcSAP.SAPItemSeqNo = 0;
                        itemStrcSAP.IsRecordInSAP = false;
                        context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                        context.SaveChanges();
                    }

                    ApplicationErrorLog errorlog = new ApplicationErrorLog();
                    errorlog.InnerException = headerStructure.Uniqueid.ToString();
                    errorlog.Stacktrace = Repitem.MESSAGE;
                    errorlog.Source = "PO Posting";
                    errorlog.Error = Repitem.MESSAGE;
                    errorlog.ExceptionDateTime = DateTime.Now;
                    context.ApplicationErrorLogs.Add(errorlog);
                    context.SaveChanges();

                    sapRes.ReturnCode = "1";
                    sapRes.PONumber = "";
                    sapRes.Message = Repitem.MESSAGE;

                    if (log.IsDebugEnabled)
                        log.Debug("Leaving From PO SAP Call: Failed " + Repitem.MESSAGE);
                }
            }
            return sapRes;
        }


        public SAPResponse DeleteHeadPODetailsToSAP(TEPOServiceHeader UpdtIemHead)
        {
            string PONumber = string.Empty;
            string CompanyCode = string.Empty;
            string VendorAccountNo = string.Empty;
            string DocumentTypeCode = string.Empty;
            string Currency = string.Empty;
            string ProjectCode = string.Empty;
            int wbsParts = 0;
            string wbsPartOne = string.Empty;
            SAPResponse sapRes = new SAPResponse();

            PurchaseOrderReqHeader poHeaderStructure = new PurchaseOrderReqHeader();
            PurchaseOrderReq poReq = new PurchaseOrderReq();
            context.Configuration.ProxyCreationEnabled = true;
            TEPOHeaderStructure headerStructure = new TEPOHeaderStructure();
            List<TEPOItemStructure> itemList = new List<TEPOItemStructure>();
            List<TEPOItemStructure> itemListDel = new List<TEPOItemStructure>();

            var POWBSPartValue = context.TEMasterRules.Where(a => a.RuleName.Contains("POWBSPart1") && a.IsDeleted == false).FirstOrDefault();
            if (POWBSPartValue != null)
            {
                wbsParts = Convert.ToInt32(POWBSPartValue.RuleValue);
            }

            headerStructure = context.TEPOHeaderStructures.Where(a => a.Uniqueid == UpdtIemHead.POHeaderStructureid && a.IsDeleted == false).FirstOrDefault();
            if (headerStructure != null)
            {
                var CmpnyVendCode = (from purHead in context.TEPOHeaderStructures
                                     join proj in context.TEProjects on purHead.ProjectID equals proj.ProjectID
                                     join cmpny in context.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                                     join vendordtl in this.context.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                                     join vendor in this.context.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                     join orderType in context.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                                     where
                                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                                      //&& vendordtl.IsDeleted == false && vendor.IsDeleted == false && orderType.IsDeleted == false &&
                                      purHead.Uniqueid == headerStructure.Uniqueid
                                     select new
                                     {
                                         cmpny.CompanyCode,
                                         vendordtl.VendorCode,
                                         vendor.Currency,
                                         orderType.Code,
                                         proj.ProjectCode
                                     }).FirstOrDefault();
                if (CmpnyVendCode != null)
                {
                    CompanyCode = CmpnyVendCode.CompanyCode;
                    VendorAccountNo = CmpnyVendCode.VendorCode;
                    DocumentTypeCode = CmpnyVendCode.Code;
                    Currency = CmpnyVendCode.Currency;
                    ProjectCode = CmpnyVendCode.ProjectCode;
                }
            }
            var pomanagerObj = context.UserProfiles.Where(a => a.UserId == headerStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
            var plantStorage = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
            var shippingLocation = context.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == headerStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
            var fundCenter = context.TEPOFundCenters.Where(a => a.Uniqueid == headerStructure.FundCenterID && a.IsDeleted == false).FirstOrDefault();


            DateTime docDate = Convert.ToDateTime(headerStructure.Purchasing_Document_Date);
            //DateTime docDate = DateTime.Today;
            poHeaderStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;
            poHeaderStructure.F_PONUMBER = headerStructure.Fugue_Purchasing_Order_Number;
            poHeaderStructure.COMP_CODE = CompanyCode;
            poHeaderStructure.DOC_TYPE = DocumentTypeCode;
            poHeaderStructure.VENDOR = VendorAccountNo;
            poHeaderStructure.PURCH_ORG = "1000";
            poHeaderStructure.PUR_GROUP = "001";
            poHeaderStructure.CURRENCY = Currency;
            poHeaderStructure.DOC_DATE = string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.PMNTTRMS = "T041";
            poHeaderStructure.REF_1 = ".";
            poHeaderStructure.OUR_REF = ".";
            poHeaderStructure.TELEPHONE = ".";
            poHeaderStructure.COMPLETED = "X";//second time onwards "X" has to send
            poHeaderStructure.REASON = "0003";//second time onwards "0003" has to send
            poHeaderStructure.EXCH_RATE = "";// headerStructure.Exchange_Rate;
            if (pomanagerObj != null)
                poHeaderStructure.SALES_PERS = pomanagerObj.UserName;
            else
                poHeaderStructure.SALES_PERS = "Admin";
            poHeaderStructure.ZPOTITLE = headerStructure.PO_Title;
            poHeaderStructure.ZAGSIGNDT = "";// string.Format("{0:dd.MM.yyyy}", docDate);
            poHeaderStructure.FUGUE_ID = headerStructure.Uniqueid.ToString();
            poHeaderStructure.TEXT_LINE = "";
            poHeaderStructure.ZPROJECT = ProjectCode;

            List<PurchaseOrderReqItem> itemStructureList = new List<PurchaseOrderReqItem>();
            List<PurchaseOrderReqItem1> itemConditionList = new List<PurchaseOrderReqItem1>();
            List<PurchaseOrderReqItem2> item2List = new List<PurchaseOrderReqItem2>();


            #region Service Material and Expense Order
            int HeadCount = 0;
            List<TEPOServiceHeader> ServHeadList = context.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == UpdtIemHead.POHeaderStructureid).OrderBy(x => x.UniqueID).ToList();

            foreach (TEPOServiceHeader HeadItem in ServHeadList)
            {
                HeadCount++;
                if (HeadItem.UniqueID == UpdtIemHead.UniqueID)
                {
                    itemList = context.TEPOItemStructures.Where(a => a.ServiceHeaderId == UpdtIemHead.UniqueID).ToList();

                    if (itemList.Count > 0)
                    {
                        int count = 1;
                        foreach (TEPOItemStructure item in itemList)
                        {

                            var commitmentItem = context.TEPOGLCodeMasters.Where(a => a.GLAccountCode == item.GLAccountNo && a.IsDeleted == false).FirstOrDefault();
                            //wbsPartOne = wbsResult(item.WBSElementCode, wbsParts + 1);
                            if (!string.IsNullOrEmpty(item.WBSElementCode))
                            {
                                if (item.WBSElementCode.Length > wbsParts)
                                {
                                    wbsPartOne = item.WBSElementCode.Substring(0, wbsParts);
                                }
                                else
                                {
                                    wbsPartOne = item.WBSElementCode;
                                }
                            }
                            if (count == 1)
                            {
                                PurchaseOrderReqItem itemStructure = new PurchaseOrderReqItem();
                                itemStructure.PO_NUMBER = headerStructure.Purchasing_Order_Number;

                                itemStructure.PO_ITEM = HeadCount.ToString();
                                itemStructure.DEL_IND = "X";

                                itemStructure.ITEM_CAT = "";//for material it is Blank, for service lineitem it is "D", for expense order it is "B"

                                itemStructure.ACCTASSCAT = "P";//material "Q", Service & Expense "P"
                                itemStructure.MATERIAL = "";

                                if (!string.IsNullOrEmpty(HeadItem.Title) && HeadItem.Title.Length > 40)
                                    itemStructure.SHORT_TEXT = HeadItem.Title.Substring(0, 40);
                                else
                                    itemStructure.SHORT_TEXT = string.IsNullOrEmpty(HeadItem.Title) ? "" : HeadItem.Title;

                                itemStructure.QUANTITY = "1";
                                itemStructure.PO_UNIT = "AU";
                                itemStructure.MATL_GROUP = "17";


                                itemStructure.ACCTASSCAT = "Q";//material "Q", Service & Expense "P"
                                itemStructure.MATERIAL = item.Material_Number;
                                
                                itemStructure.QUANTITY = item.Order_Qty;
                                itemStructure.PO_UNIT = "";
                                itemStructure.MATL_GROUP = "";

                                itemStructure.DELIVERY_DATE = string.Format("{0:dd.MM.yyyy}", DateTime.Today);
                                itemStructure.NET_PRICE = "";

                                itemStructure.PLANT = plantStorage.PlantStorageCode;//item.Plant;
                                itemStructure.TRACKINGNO = "";// item.Requirement_Tracking_Number;

                                itemStructure.GL_ACCOUNT = ""; //item.GLAccountNo;//need to add one column of this
                                itemStructure.WBS_ELEMENT = "";
                                itemStructure.FUNDS_CTR = "";
                                itemStructure.CMMT_ITEM_LONG = "";
                                itemStructure.ITEM_CAT = "D";

                                itemStructure.ASSET_NO = "";
                                itemStructure.ORDER = "";//item.Purchasing_Order_Number
                                itemStructure.TAX_CODE = item.Tax_salespurchases_code;
                                itemStructure.PREQ_NO = "";
                                itemStructure.PREQ_ITEM = "";
                                itemStructure.STGE_LOC = shippingLocation.StorageLocationCode;//item.Storage_Location;
                                itemStructure.SERVICE = "";
                                itemStructure.SQUANTITY = "";
                                itemStructure.GR_PRICE = "";
                                itemStructure.PRE_VENDOR = "";

                                itemStructure.LIMIT = "";
                                itemStructure.EXP_VALUE = "";
                                itemStructure.STEUC = "";


                                itemStructure.NO_LIMIT = "";
                                itemStructure.EXT_LINE = "";
                                itemStructure.BASE_UOM = "";
                                itemStructure.LINE_TEXT = "";
                                itemStructure.NETWORK = "";
                                itemStructure.OVF_TOL = "";
                                itemStructure.OVF_UNLIM = "";
                                itemStructure.NO_MORE_GR = "";
                                itemStructure.RET_ITEM = "";

                                itemStructure.TEXT_LINE = HeadItem.Description;//more description

                                itemStructure.KOSTL = "";

                                itemStructureList.Add(itemStructure);
                            }
                           
                                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                                item2.PO_ITEM = HeadCount.ToString();//item.Item_Number;
                                item2.LINES = item.SAPItemSeqNo.ToString();
                                item2.SERVICE = item.Material_Number;
                                item2.SQUANTITY = item.Order_Qty;
                                item2.GR_PRICE = item.Rate.ToString();
                                item2.BASE_UOM = "";

                                item2.LINE_TEXT = "X";//"" if we delete then "X"

                                item2.OVF_TOL = "";
                                item2.OVF_UNLIM1 = "";
                                item2.MATKL = "";
                                item2.WBS_ELEMENT = wbsPartOne;
                                item2.GL_ACCOUNT = item.GLAccountNo;
                                item2.ORDER = "";
                                item2.FUNDS_CTR = fundCenter.FundCenter_Code;
                                if (commitmentItem != null)
                                    item2.CMMT_ITEM_LONG = commitmentItem.CommitmentItemCode;
                                else
                                    item2.CMMT_ITEM_LONG = "";
                                if (!string.IsNullOrEmpty(item.Long_Text) && item.Long_Text.Length > 130)
                                    item2.LONG_TEXT = item.Long_Text.Substring(0, 130);
                                else
                                    item2.LONG_TEXT = string.IsNullOrEmpty(item.Long_Text) ? "" : item.Long_Text;
                                item2List.Add(item2);
                           
                            count++;
                        }
                    }
                }
            }

           
            #endregion

            poReq.Header = poHeaderStructure;
            poReq.item = itemStructureList.ToArray();
            if (itemConditionList.Count == 0)
            {
                PurchaseOrderReqItem1 itemCondition = new PurchaseOrderReqItem1();
                itemCondition.KBETR = "";
                itemCondition.KPOSN = "";
                itemCondition.KSCHL = "";
                itemCondition.LIFNR = "";
                itemCondition.PO_ITEM = "";
                itemConditionList.Add(itemCondition);
            }
            poReq.item1 = itemConditionList.ToArray();
            if (item2List.Count == 0)
            {
                PurchaseOrderReqItem2 item2 = new PurchaseOrderReqItem2();
                item2.BASE_UOM = "";
                item2.CMMT_ITEM_LONG = "";
                item2.FUNDS_CTR = "";
                item2.GL_ACCOUNT = "";
                item2.GR_PRICE = "";
                item2.LINES = "";
                item2.LINE_TEXT = "";
                item2.LONG_TEXT = "";
                item2.MATKL = "";
                item2.ORDER = "";
                item2.OVF_TOL = "";
                item2.OVF_UNLIM1 = "";
                item2.PO_ITEM = "";
                item2.SERVICE = "";
                item2.SQUANTITY = "";
                item2.WBS_ELEMENT = "";
                item2List.Add(item2);
            }
            poReq.Item2 = item2List.ToArray();
            SAPCallConnector sapCall = new SAPCallConnector();
            IList<PurchaseOrderResItem> RespList = new List<PurchaseOrderResItem>();

            RespList = sapCall.CreatePO(poReq);
            foreach (PurchaseOrderResItem item in RespList)
            {
                if (item.RETCODE == "0")
                {
                    PONumber = item.PO_NUMBER;
                    headerStructure.Purchasing_Order_Number = PONumber;
                    context.Entry(headerStructure).CurrentValues.SetValues(headerStructure);
                    context.SaveChanges();

                    foreach (TEPOItemStructure itmStrc in itemList)
                    {
                        var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                        itemStrcSAP.IsRecordInSAP = true;
                        if (string.IsNullOrEmpty(itemStrcSAP.SAPTransactionCode))
                            itemStrcSAP.SAPTransactionCode = "X";
                        context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                        context.SaveChanges();
                    }

                    sapRes.ReturnCode = item.RETCODE;
                    sapRes.PONumber = item.PO_NUMBER;
                    sapRes.Message = "Success";

                    if (log.IsDebugEnabled)
                        log.Debug("Leaving From PO SAP Call: Success " + PONumber);

                    //Purchasing Document Number PO_NUMBER
                    //Fugue Purchasing Order Number   F_PONUMBER
                    //Item Number of Purchasing Document  PO_ITEM
                    //PO Item Net Value NETWR
                    //PO Item Gross Value BRTWR
                    //PO Item Tax Value NAVNW
                    //Return Code RETCODE
                    //Return Message MESSAGE
                    //Fugue Reference ID FUGUE_ID
                }
                else
                {
                    foreach (TEPOItemStructure itmStrc in itemList)
                    {
                        var itemStrcSAP = context.TEPOItemStructures.Where(a => a.Uniqueid == itmStrc.Uniqueid).FirstOrDefault();
                        if (itemStrcSAP.IsRecordInSAP == true)
                        { }
                        else
                        {
                            itemStrcSAP.SAPItemSeqNo = 0;
                            itemStrcSAP.IsRecordInSAP = false;
                            context.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                            context.SaveChanges();
                        }
                    }
                    ApplicationErrorLog errorlog = new ApplicationErrorLog();
                    errorlog.InnerException = headerStructure.Uniqueid.ToString();
                    errorlog.Stacktrace = item.MESSAGE;
                    errorlog.Source = "PO Posting";
                    errorlog.Error = item.MESSAGE;
                    errorlog.ExceptionDateTime = DateTime.Now;
                    context.ApplicationErrorLogs.Add(errorlog);
                    context.SaveChanges();

                    sapRes.ReturnCode = "1";
                    sapRes.PONumber = "";
                    sapRes.Message = item.MESSAGE;

                    if (log.IsDebugEnabled)
                        log.Debug("Leaving From PO SAP Call: Failed " + item.MESSAGE);
                }
            }
            return sapRes;
        }

        #endregion

        public string wbsResult(string input, int number)
        {
            try
            {
                int words = number;
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '-')
                    {
                        words--;
                    }
                    if (words == 0)
                    {
                        return input.Substring(0, i);
                    }
                }
                return input;
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }


        #region Author[Kamal] For Service

        public int ClonePO(int headerID, int loginId)
        {
            int uniqueId = 0;

            var HeadOrderType = (from Head in context.TEPOHeaderStructures
                                 join OrdType in context.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == headerID
                                 select OrdType.Code).FirstOrDefault();

            UserProfile UserObj = context.UserProfiles.Where(a => a.UserId == loginId && a.IsDeleted == false).FirstOrDefault();

            var POHeaderStructure = (from header in context.TEPOHeaderStructures
                                     where header.Uniqueid == headerID && header.IsDeleted == false
                                     && header.Status == "Active" && header.ReleaseCode2Status == "Approved"
                                     select header).FirstOrDefault();

            var POItemStructure = (from item in context.TEPOItemStructures
                                   where item.POStructureId == headerID && item.IsDeleted == false
                                   select item).ToList();

            var POPaymentTerms = (from milestone in context.TEPOVendorPaymentMilestones
                                  where milestone.POHeaderStructureId == headerID && milestone.IsDeleted == false
                                  select milestone).ToList();

            var SpecificTandCDtls = (from specificDtl in context.TEPOSpecificTCDetails
                                     where specificDtl.IsDeleted == false && specificDtl.POHeaderStructureId == headerID
                                     select specificDtl).ToList();

            var LinkedPOList = (from linkPO in context.TEPOLinkedPOes
                                where linkPO.MainPOID == headerID && linkPO.IsDeleted == false
                                select linkPO).ToList();

            var LinkedChildPOList = (from linkchPO in context.TEPOLinkedPOes
                                     where linkchPO.LinkedPOID == headerID && linkchPO.IsDeleted == false
                                     select linkchPO).ToList();

            if (POHeaderStructure != null)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<TEPOHeaderStructure, TEPOHeaderStructure>()
                 .ForMember(a => a.Uniqueid, a => a.Ignore()));

                var newObject = Mapper.Map<TEPOHeaderStructure>(POHeaderStructure);
                if (!string.IsNullOrEmpty(newObject.Version))
                {
                    int ver = 0;
                    ver = Convert.ToInt32(newObject.Version);
                    ver += 1;
                    newObject.Version = ver.ToString();
                }
                newObject.ReleaseCode2Status = "Draft";
                newObject.CreatedBy = loginId;
                newObject.CreatedOn = DateTime.Now;
                newObject.LastModifiedBy = loginId;
                newObject.LastModifiedOn = DateTime.Now;
                context.TEPOHeaderStructures.Add(newObject);
                context.SaveChanges();
                uniqueId = newObject.Uniqueid;

                POHeaderStructure.Status = "Superseded";
                context.Entry(POHeaderStructure).CurrentValues.SetValues(POHeaderStructure);
                context.SaveChanges();

                var TandC = (from terms in context.TETermsAndConditions
                             where terms.POHeaderStructureId == headerID && terms.IsDeleted == false
                             select terms).ToList();

                if (TandC.Count > 0)
                {
                    foreach (TETermsAndCondition tcObj in TandC)
                    {
                        Mapper.Reset();
                        Mapper.Initialize(cfg => cfg.CreateMap<TETermsAndCondition, TETermsAndCondition>()
                        .ForMember(a => a.UniqueId, a => a.Ignore()));

                        var newTCObject = Mapper.Map<TETermsAndCondition>(tcObj);
                        newTCObject.POHeaderStructureId = uniqueId;
                        newTCObject.ContextIdentifier = uniqueId.ToString();
                        newTCObject.LastModifiedBy = UserObj.UserName;
                        newTCObject.LastModifiedOn = DateTime.Now;
                        context.TETermsAndConditions.Add(newTCObject);
                        context.SaveChanges();
                    }
                }
                int managerID = 0;
                if (newObject.POManagerID > 0)
                    managerID = Convert.ToInt32(newObject.POManagerID);
                if (managerID > 0 && loginId > 0)
                    SavePurchaseOrderApproverForDraft(uniqueId, loginId, managerID);
                else
                    SavePOSubmitterForDraft(uniqueId, loginId);
                // int draftAppr= SavePurchaseOrderApproverForDraft(uniqueId, loginId, managerID);
            }

            var ServHead = context.TEPOServiceHeaders.Where(x => x.IsDeleted == false && x.POHeaderStructureid == headerID).OrderBy(x => x.UniqueID).ToList();
            #region Service
            //if (ServHead.Count > 0)
            //{
            //    // Start of the Service Header Line Items
            //    foreach (TEPOServiceHeader ServHeadID in ServHead)
            //    {

            //        var POItemStruct = (from item in context.TEPOItemStructures
            //                            where item.POStructureId == headerID && item.IsDeleted == false && item.ServiceHeaderId == ServHeadID.UniqueID
            //                            select item).OrderBy(x => x.Uniqueid).ToList();
            //        //To Clone the Head
            //        Mapper.Reset();
            //        Mapper.Initialize(cfg => cfg.CreateMap<TEPOServiceHeader, TEPOServiceHeader>()
            //         .ForMember(a => a.UniqueID, a => a.Ignore()));
            //        var newHeadItemObject = Mapper.Map<TEPOServiceHeader>(ServHeadID);
            //        newHeadItemObject.POHeaderStructureid = uniqueId;
            //        context.TEPOServiceHeaders.Add(newHeadItemObject);
            //        context.SaveChanges();
            //        int ServiceHeadUniqueID = newHeadItemObject.UniqueID;

            //        if (POItemStruct.Count > 0)
            //        {
            //            POItemsClone(ServiceHeadUniqueID, headerID, uniqueId, POItemStruct, UserObj);
            //        }
            //    }
            //    // End of the Service Header Line Items

            //    // Start of the Material Line Items 
            //    var POItemStruct_Mat = (from item in context.TEPOItemStructures
            //                        where item.POStructureId == headerID && item.IsDeleted == false && (item.ServiceHeaderId == null || item.ServiceHeaderId == 0)
            //                        select item).ToList();

            //    if (POItemStruct_Mat.Count > 0)
            //        POItemsClone(0, headerID, uniqueId, POItemStruct_Mat, UserObj);
            //    // End of the Material Line Items 
            //}
            //else
            //{
                #endregion
                #region
                if (POItemStructure.Count > 0)
                {
                Dictionary<int?, int> HeaderCloneValues = new Dictionary<int?, int>();
                foreach (TEPOItemStructure itemStructure in POItemStructure)
                {
                    int ServiceHeadUniqueID = 0;
                    if (itemStructure.ServiceHeaderId != 0 && itemStructure.ServiceHeaderId != null)
                    {
                        if (!HeaderCloneValues.ContainsKey(itemStructure.ServiceHeaderId))
                        {
                            TEPOServiceHeader ServHead_Val = context.TEPOServiceHeaders.Where(x => x.UniqueID == itemStructure.ServiceHeaderId).FirstOrDefault();
                            Mapper.Reset();
                            Mapper.Initialize(cfg => cfg.CreateMap<TEPOServiceHeader, TEPOServiceHeader>().ForMember(a => a.UniqueID, a => a.Ignore()));
                            var newHeadItemObject = Mapper.Map<TEPOServiceHeader>(ServHead_Val);
                            newHeadItemObject.POHeaderStructureid = uniqueId;
                            context.TEPOServiceHeaders.Add(newHeadItemObject);
                            context.SaveChanges();
                            ServiceHeadUniqueID = newHeadItemObject.UniqueID;
                            HeaderCloneValues.Add(itemStructure.ServiceHeaderId, ServiceHeadUniqueID);
                        }
                        else
                        {
                           
                            ServiceHeadUniqueID = HeaderCloneValues[itemStructure.ServiceHeaderId];
                        }
                    }
                    POItemsClone(ServiceHeadUniqueID, headerID, uniqueId, itemStructure, UserObj);
                }
                    #region--------------------------Delete If Everythig Works-------------------------//
                    //foreach (TEPOItemStructure itemStructure in POItemStructure)
                    //{
                    //    Mapper.Reset();
                    //    Mapper.Initialize(cfg => cfg.CreateMap<TEPOItemStructure, TEPOItemStructure>()
                    //     .ForMember(a => a.Uniqueid, a => a.Ignore()));

                    //    var newItemObject = Mapper.Map<TEPOItemStructure>(itemStructure);
                    //    newItemObject.POStructureId = uniqueId;
                    //    newItemObject.LastModifiedBy = UserObj.UserName;
                    //    newItemObject.LastModifiedOn = DateTime.Now.ToString();
                    //    context.TEPOItemStructures.Add(newItemObject);
                    //    context.SaveChanges();

                    //    var POSpecificAnnex = (from annex in context.TEPOAnnexures
                    //                           where annex.PO_HeaderStructureID == headerID
                    //                           && annex.PO_ItemStructureID == itemStructure.Uniqueid
                    //                           && annex.IsDeleted == false
                    //                           select annex).ToList();
                    //    if (POSpecificAnnex.Count > 0)
                    //    {
                    //        foreach (TEPOAnnexure spAnnex in POSpecificAnnex)
                    //        {
                    //            Mapper.Reset();
                    //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexure, TEPOAnnexure>()
                    //             .ForMember(a => a.POAnnexureId, a => a.Ignore()));

                    //            var newSpecObject = Mapper.Map<TEPOAnnexure>(spAnnex);
                    //            newSpecObject.PO_HeaderStructureID = uniqueId;
                    //            newSpecObject.PO_ItemStructureID = newItemObject.Uniqueid;
                    //            newSpecObject.LastModifiedBy = UserObj.UserId;
                    //            newSpecObject.LastModifiedOn = DateTime.Now;
                    //            context.TEPOAnnexures.Add(newSpecObject);
                    //            context.SaveChanges();

                    //            var annexSpec = context.TEPOAnnexureSpecifications.Where(a => a.POAnnexureId == spAnnex.POAnnexureId && a.IsDeleted == false)
                    //                            .OrderBy(b => b.POAnnexureSpecId).ToList();
                    //            if (annexSpec.Count > 0)
                    //            {
                    //                foreach (TEPOAnnexureSpecification poAnnexSpec in annexSpec)
                    //                {
                    //                    Mapper.Reset();
                    //                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexureSpecification, TEPOAnnexureSpecification>()
                    //                     .ForMember(a => a.POAnnexureSpecId, a => a.Ignore()));

                    //                    var newAnnexSpecObject = Mapper.Map<TEPOAnnexureSpecification>(poAnnexSpec);
                    //                    newAnnexSpecObject.POAnnexureId = newSpecObject.POAnnexureId;
                    //                    context.TEPOAnnexureSpecifications.Add(newAnnexSpecObject);
                    //                    context.SaveChanges();
                    //                }
                    //            }
                    //        }
                    //    }
                    //    var POServiceAnnex = (from servannex in context.TEPOServiceAnnexures
                    //                          where servannex.PO_HeaderStructureID == headerID
                    //                           && servannex.PO_ItemStructureID == itemStructure.Uniqueid
                    //                           && servannex.IsDeleted == false
                    //                          select servannex).ToList();
                    //    if (POServiceAnnex.Count > 0)
                    //    {
                    //        foreach (TEPOServiceAnnexure serAnnex in POServiceAnnex)
                    //        {
                    //            Mapper.Reset();
                    //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOServiceAnnexure, TEPOServiceAnnexure>()
                    //             .ForMember(a => a.POServiceAnnexureId, a => a.Ignore()));

                    //            var newServAnnexObject = Mapper.Map<TEPOServiceAnnexure>(serAnnex);
                    //            newServAnnexObject.PO_HeaderStructureID = uniqueId;
                    //            newServAnnexObject.PO_ItemStructureID = newItemObject.Uniqueid;
                    //            context.TEPOServiceAnnexures.Add(newServAnnexObject);
                    //            context.SaveChanges();
                    //        }
                    //    }
                    //}
                    #endregion --------------------------Delete If Everythig Works-------------------------//
                }
            //}
            #endregion

            if (POPaymentTerms.Count > 0)
            {
                foreach (TEPOVendorPaymentMilestone pymntMileStone in POPaymentTerms)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOVendorPaymentMilestone, TEPOVendorPaymentMilestone>()
                     .ForMember(a => a.UniqueId, a => a.Ignore()));

                    var newMSObject = Mapper.Map<TEPOVendorPaymentMilestone>(pymntMileStone);
                    newMSObject.POHeaderStructureId = uniqueId;
                    newMSObject.ContextIdentifier = uniqueId.ToString();
                    newMSObject.LastModifiedBy = UserObj.UserName;
                    newMSObject.LastModifiedOn = DateTime.Now;
                    newMSObject.IsDeleted = false;
                    context.TEPOVendorPaymentMilestones.Add(newMSObject);
                    context.SaveChanges();
                }
            }

            if (SpecificTandCDtls.Count > 0)
            {
                List<TEPOSpecificTCDetailDTO> specDtoList = new List<TEPOSpecificTCDetailDTO>();
                List<TEPOSpecificTCDetail> speciDtoList = new List<TEPOSpecificTCDetail>();
                speciDtoList.AddRange(SpecificTandCDtls);
                foreach (TEPOSpecificTCDetail spTCDtl in SpecificTandCDtls)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOSpecificTCDetail, TEPOSpecificTCDetail>()
                     .ForMember(a => a.SpecificTCId, a => a.Ignore()));
                    //.ForMember(a => a.POHeaderStructureId, a => a.Ignore()));
                    var newSpecTCDtlObject = Mapper.Map<TEPOSpecificTCDetail>(spTCDtl);
                    newSpecTCDtlObject.POHeaderStructureId = uniqueId;
                    newSpecTCDtlObject.LastModifiedBy = UserObj.UserId;
                    newSpecTCDtlObject.LastModifiedOn = DateTime.Now;
                    context.TEPOSpecificTCDetails.Add(newSpecTCDtlObject);
                    context.SaveChanges();
                }
                foreach (TEPOSpecificTCDetail spTCDtl in speciDtoList)
                {
                    var specDtl = (from sub in context.TEPOSpecificTCDetails
                                   where sub.SpecificTCId == spTCDtl.SpecificTCId
                                   select sub).FirstOrDefault();
                    specDtl.POHeaderStructureId = headerID;
                    context.Entry(specDtl).CurrentValues.SetValues(specDtl);
                    context.SaveChanges();
                }
            }


            if (LinkedPOList.Count > 0)
            {
                foreach (TEPOLinkedPO mainPO in LinkedPOList)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOLinkedPO, TEPOLinkedPO>()
                     .ForMember(a => a.UniqueID, a => a.Ignore()));

                    var newMainPOObject = Mapper.Map<TEPOLinkedPO>(mainPO);
                    newMainPOObject.MainPOID = uniqueId;
                    newMainPOObject.LastModifiedBy = UserObj.UserId;
                    newMainPOObject.LastModifiedOn = DateTime.Now;
                    context.TEPOLinkedPOes.Add(newMainPOObject);
                    context.SaveChanges();
                }
            }

            if (LinkedChildPOList.Count > 0)
            {
                foreach (TEPOLinkedPO childPO in LinkedChildPOList)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOLinkedPO, TEPOLinkedPO>()
                     .ForMember(a => a.UniqueID, a => a.Ignore()));

                    var newChildPOObject = Mapper.Map<TEPOLinkedPO>(childPO);
                    newChildPOObject.LinkedPOID = uniqueId;
                    newChildPOObject.LastModifiedBy = UserObj.UserId;
                    newChildPOObject.LastModifiedOn = DateTime.Now;
                    context.TEPOLinkedPOes.Add(newChildPOObject);
                    context.SaveChanges();
                }
            }

            return uniqueId;
        }

        public void POItemsClone(int ServiceHeadUniqueID, int headerID, int New_HeadID, TEPOItemStructure itemStructure, UserProfile UserObj, bool NewSAP = false, int maxItemNo = 0)
        {
            
                Mapper.Reset();
                Mapper.Initialize(cfg => cfg.CreateMap<TEPOItemStructure, TEPOItemStructure>().ForMember(a => a.Uniqueid, a => a.Ignore()));

                var newItemObject = Mapper.Map<TEPOItemStructure>(itemStructure);
                newItemObject.ServiceHeaderId = ServiceHeadUniqueID;
                newItemObject.ItemType = itemStructure.ItemType;

                //Cloning Taxes Manually
                newItemObject.Material_Number = itemStructure.Material_Number;
                newItemObject.Short_Text = itemStructure.Short_Text;
                newItemObject.Long_Text = itemStructure.Long_Text;
                newItemObject.IGSTRate = itemStructure.IGSTRate;
                newItemObject.IGSTAmount = itemStructure.IGSTAmount;
                newItemObject.CGSTRate = itemStructure.CGSTRate;
                newItemObject.CGSTAmount = itemStructure.CGSTAmount;
                newItemObject.SGSTRate = itemStructure.SGSTRate;
                newItemObject.SGSTAmount = itemStructure.SGSTAmount;
                newItemObject.TotalTaxAmount = itemStructure.TotalTaxAmount;
                newItemObject.GrossAmount = itemStructure.GrossAmount;
            newItemObject.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code;
                newItemObject.CGSTRate = itemStructure.CGSTRate;
            if (NewSAP)
            {
                newItemObject.Item_Number = maxItemNo.ToString();
                newItemObject.SAPTransactionCode = String.Empty;
                newItemObject.IsRecordInSAP = false;

                    }
                //Cloning Taxes End

                newItemObject.POStructureId = New_HeadID;
                newItemObject.LastModifiedBy = UserObj.UserName;
                newItemObject.LastModifiedOn = DateTime.Now.ToString();
                context.TEPOItemStructures.Add(newItemObject);
                context.SaveChanges();

            var SpecifAnnex = context.TEPOSpecificationAnnexures.Where(a => a.POHeaderStructureId == headerID && a.POItemStructureId == itemStructure.Uniqueid && a.IsDeleted == false).ToList();

            if (SpecifAnnex.Count > 0)
            {
                foreach (TEPOSpecificationAnnexure poSpecAnnex in SpecifAnnex)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOSpecificationAnnexure, TEPOSpecificationAnnexure>()
                     .ForMember(a => a.SpecAnnexureId, a => a.Ignore()));

                    var newAnnexSpecObject = Mapper.Map<TEPOSpecificationAnnexure>(poSpecAnnex);
                    poSpecAnnex.POHeaderStructureId = New_HeadID;
                    poSpecAnnex.POItemStructureId = newItemObject.Uniqueid;
                    poSpecAnnex.IsDeleted = false;
                    poSpecAnnex.LastModifiedBy = UserObj.UserId;
                    poSpecAnnex.LastModifiedOn = DateTime.Now;
                    context.TEPOSpecificationAnnexures.Add(poSpecAnnex);
                    context.SaveChanges();

                }
            }

            var POSpecificAnnex = (from annex in context.TEPOAnnexures
                                       where annex.PO_HeaderStructureID == headerID
                                       && annex.PO_ItemStructureID == itemStructure.Uniqueid
                                       && annex.IsDeleted == false
                                       select annex).ToList();
                if (POSpecificAnnex.Count > 0)
                {
                    foreach (TEPOAnnexure spAnnex in POSpecificAnnex)
                    {
                        Mapper.Reset();
                        Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexure, TEPOAnnexure>()
                         .ForMember(a => a.POAnnexureId, a => a.Ignore()));

                        var newSpecObject = Mapper.Map<TEPOAnnexure>(spAnnex);
                        newSpecObject.PO_HeaderStructureID = New_HeadID;
                        newSpecObject.PO_ItemStructureID = newItemObject.Uniqueid;
                        newSpecObject.LastModifiedBy = UserObj.UserId;
                        newSpecObject.LastModifiedOn = DateTime.Now;
                        context.TEPOAnnexures.Add(newSpecObject);
                        context.SaveChanges();
                   

                            var annexSpec = context.TEPOAnnexureSpecifications.Where(a => a.POAnnexureId == spAnnex.POAnnexureId && a.IsDeleted == false)
                                        .OrderBy(b => b.POAnnexureSpecId).ToList();
                        if (annexSpec.Count > 0)
                        {
                            foreach (TEPOAnnexureSpecification poAnnexSpec in annexSpec)
                            {
                                Mapper.Reset();
                                Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexureSpecification, TEPOAnnexureSpecification>()
                                 .ForMember(a => a.POAnnexureSpecId, a => a.Ignore()));

                                var newAnnexSpecObject = Mapper.Map<TEPOAnnexureSpecification>(poAnnexSpec);
                                
                                newAnnexSpecObject.POAnnexureId = newSpecObject.POAnnexureId;
                                context.TEPOAnnexureSpecifications.Add(newAnnexSpecObject);
                                context.SaveChanges();
                            }
                        }
                    }
                }
                var POServiceAnnex = (from servannex in context.TEPOServiceAnnexures
                                      where servannex.PO_HeaderStructureID == headerID
                                       && servannex.PO_ItemStructureID == itemStructure.Uniqueid
                                       && servannex.IsDeleted == false
                                      select servannex).ToList();
                if (POServiceAnnex.Count > 0)
                {
                    foreach (TEPOServiceAnnexure serAnnex in POServiceAnnex)
                    {
                        Mapper.Reset();
                        Mapper.Initialize(cfg => cfg.CreateMap<TEPOServiceAnnexure, TEPOServiceAnnexure>()
                         .ForMember(a => a.POServiceAnnexureId, a => a.Ignore()));

                        var newServAnnexObject = Mapper.Map<TEPOServiceAnnexure>(serAnnex);
                        newServAnnexObject.PO_HeaderStructureID = New_HeadID;
                        newServAnnexObject.PO_ItemStructureID = newItemObject.Uniqueid;
                        context.TEPOServiceAnnexures.Add(newServAnnexObject);
                        context.SaveChanges();
                    }
                }
            
        }

        public void PRItemsClone(int ServiceHeadUniqueID, int headerID, int New_HeadID, TEPRItemStructure itemStructure, int userId, bool NewSAP = false, int maxItemNo = 0)
        {

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<TEPRItemStructure, TEPRItemStructure>().ForMember(a => a.Uniqueid, a => a.Ignore()));

            var newItemObject = Mapper.Map<TEPRItemStructure>(itemStructure);
            newItemObject.ServiceHeaderId = ServiceHeadUniqueID;
            newItemObject.ItemType = itemStructure.ItemType;

            //Cloning Taxes Manually
            newItemObject.Material_Number = itemStructure.Material_Number;
            newItemObject.Short_Text = itemStructure.Short_Text;
            newItemObject.Long_Text = itemStructure.Long_Text;
            newItemObject.IGSTRate = itemStructure.IGSTRate;
            newItemObject.IGSTAmount = itemStructure.IGSTAmount;
            newItemObject.CGSTRate = itemStructure.CGSTRate;
            newItemObject.CGSTAmount = itemStructure.CGSTAmount;
            newItemObject.SGSTRate = itemStructure.SGSTRate;
            newItemObject.SGSTAmount = itemStructure.SGSTAmount;
            newItemObject.TotalTaxAmount = itemStructure.TotalTaxAmount;
            newItemObject.GrossAmount = itemStructure.GrossAmount;
            newItemObject.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code;
            newItemObject.CGSTRate = itemStructure.CGSTRate;
            if (NewSAP)
            {
                newItemObject.Balance_Qty = Convert.ToDecimal(itemStructure.Order_Qty);
                newItemObject.Item_Number = maxItemNo.ToString();
            }
         
            newItemObject.POStructureId = itemStructure.POStructureId;
            newItemObject.LastModifiedBy = userId;
            newItemObject.LastModifiedOn = Convert.ToDateTime(DateTime.Now.ToString());
            context.TEPRItemStructures.Add(newItemObject);
            context.SaveChanges();

            var POSpecificAnnex = (from annex in context.TEPRSpecificationAnnexures
                                   where annex.PRHeaderStructureId == headerID
                                   && annex.PRItemStructureId == itemStructure.Uniqueid
                                   && annex.IsDeleted == false
                                   select annex).ToList();
            if (POSpecificAnnex.Count > 0)
            {
                foreach (TEPRSpecificationAnnexure spAnnex in POSpecificAnnex)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg => cfg.CreateMap<TEPRSpecificationAnnexure, TEPRSpecificationAnnexure>()
                     .ForMember(a => a.SpecAnnexureId, a => a.Ignore()));

                    var newSpecObject = Mapper.Map<TEPRSpecificationAnnexure>(spAnnex);
                    newSpecObject.PRHeaderStructureId = New_HeadID;
                    newSpecObject.PRItemStructureId = newItemObject.Uniqueid;
                    newSpecObject.LastModifiedBy = userId;
                    newSpecObject.LastModifiedOn = DateTime.Now;
                    context.TEPRSpecificationAnnexures.Add(newSpecObject);
                    context.SaveChanges();
                }
            }
        }

        //public int ClonePO(int headerID, int loginId)
        //{
        //    int uniqueId = 0;

        //    var UserObj = context.UserProfiles.Where(a => a.UserId == loginId && a.IsDeleted == false).FirstOrDefault();

        //    var POHeaderStructure = (from header in context.TEPOHeaderStructures
        //                             where header.Uniqueid == headerID && header.IsDeleted == false
        //                             && header.Status == "Active" && header.ReleaseCode2Status == "Approved"
        //                             select header).FirstOrDefault();

        //    var POItemStructure = (from item in context.TEPOItemStructures
        //                           where item.POStructureId == headerID && item.IsDeleted == false
        //                           select item).ToList();

        //    var POPaymentTerms = (from milestone in context.TEPOVendorPaymentMilestones
        //                          where milestone.POHeaderStructureId == headerID && milestone.IsDeleted == false
        //                          select milestone).ToList();

        //    var SpecificTandCDtls = (from specificDtl in context.TEPOSpecificTCDetails
        //                             where specificDtl.IsDeleted == false && specificDtl.POHeaderStructureId == headerID
        //                             select specificDtl).ToList();

        //    var LinkedPOList = (from linkPO in context.TEPOLinkedPOes
        //                        where linkPO.MainPOID == headerID && linkPO.IsDeleted == false
        //                        select linkPO).ToList();

        //    var LinkedChildPOList = (from linkchPO in context.TEPOLinkedPOes
        //                             where linkchPO.LinkedPOID == headerID && linkchPO.IsDeleted == false
        //                             select linkchPO).ToList();

        //    if (POHeaderStructure != null)
        //    {
        //        Mapper.Reset();
        //        Mapper.Initialize(cfg => cfg.CreateMap<TEPOHeaderStructure, TEPOHeaderStructure>()
        //         .ForMember(a => a.Uniqueid, a => a.Ignore()));

        //        var newObject = Mapper.Map<TEPOHeaderStructure>(POHeaderStructure);
        //        if (!string.IsNullOrEmpty(newObject.Version))
        //        {
        //            int ver = 0;
        //            ver = Convert.ToInt32(newObject.Version);
        //            ver += 1;
        //            newObject.Version = ver.ToString();
        //        }
        //        newObject.ReleaseCode2Status = "Draft";
        //        newObject.CreatedBy = loginId;
        //        newObject.CreatedOn = DateTime.Now;
        //        newObject.LastModifiedBy = loginId;
        //        newObject.LastModifiedOn = DateTime.Now;
        //        context.TEPOHeaderStructures.Add(newObject);
        //        context.SaveChanges();
        //        uniqueId = newObject.Uniqueid;

        //        var TandC = (from terms in context.TETermsAndConditions
        //                     where terms.POHeaderStructureId == headerID && terms.IsDeleted == false
        //                     select terms).ToList();

        //        if (TandC.Count > 0)
        //        {
        //            foreach (TETermsAndCondition tcObj in TandC)
        //            {
        //                Mapper.Reset();
        //                Mapper.Initialize(cfg => cfg.CreateMap<TETermsAndCondition, TETermsAndCondition>()
        //                .ForMember(a => a.UniqueId, a => a.Ignore()));

        //                var newTCObject = Mapper.Map<TETermsAndCondition>(tcObj);
        //                newTCObject.POHeaderStructureId = uniqueId;
        //                newTCObject.ContextIdentifier = uniqueId.ToString();
        //                newTCObject.LastModifiedBy = UserObj.UserName;
        //                newTCObject.LastModifiedOn = DateTime.Now;
        //                context.TETermsAndConditions.Add(newTCObject);
        //                context.SaveChanges();
        //            }
        //        }
        //        int managerID = 0;
        //        if (newObject.POManagerID > 0)
        //            managerID = Convert.ToInt32(newObject.POManagerID);
        //        if (managerID > 0 && loginId > 0)
        //            SavePurchaseOrderApproverForDraft(uniqueId, loginId, managerID);
        //        else
        //            SavePOSubmitterForDraft(uniqueId, loginId);
        //        // int draftAppr= SavePurchaseOrderApproverForDraft(uniqueId, loginId, managerID);
        //    }
        //    if (POItemStructure.Count > 0)
        //    {
        //        foreach (TEPOItemStructure itemStructure in POItemStructure)
        //        {
        //            Mapper.Reset();
        //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOItemStructure, TEPOItemStructure>()
        //             .ForMember(a => a.Uniqueid, a => a.Ignore()));

        //            var newItemObject = Mapper.Map<TEPOItemStructure>(itemStructure);
        //            newItemObject.POStructureId = uniqueId;
        //            newItemObject.LastModifiedBy = UserObj.UserName;
        //            newItemObject.LastModifiedOn = DateTime.Now.ToString();
        //            context.TEPOItemStructures.Add(newItemObject);
        //            context.SaveChanges();

        //            var POSpecificAnnex = (from annex in context.TEPOAnnexures
        //                                   where annex.PO_HeaderStructureID == headerID
        //                                   && annex.PO_ItemStructureID == itemStructure.Uniqueid
        //                                   && annex.IsDeleted == false
        //                                   select annex).ToList();
        //            if (POSpecificAnnex.Count > 0)
        //            {
        //                foreach (TEPOAnnexure spAnnex in POSpecificAnnex)
        //                {
        //                    Mapper.Reset();
        //                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexure, TEPOAnnexure>()
        //                     .ForMember(a => a.POAnnexureId, a => a.Ignore()));

        //                    var newSpecObject = Mapper.Map<TEPOAnnexure>(spAnnex);
        //                    newSpecObject.PO_HeaderStructureID = uniqueId;
        //                    newSpecObject.PO_ItemStructureID = newItemObject.Uniqueid;
        //                    newSpecObject.LastModifiedBy = UserObj.UserId;
        //                    newSpecObject.LastModifiedOn = DateTime.Now;
        //                    context.TEPOAnnexures.Add(newSpecObject);
        //                    context.SaveChanges();

        //                    var annexSpec = context.TEPOAnnexureSpecifications.Where(a => a.POAnnexureId == spAnnex.POAnnexureId && a.IsDeleted == false)
        //                                    .OrderBy(b => b.POAnnexureSpecId).ToList();
        //                    if (annexSpec.Count > 0)
        //                    {
        //                        foreach (TEPOAnnexureSpecification poAnnexSpec in annexSpec)
        //                        {
        //                            Mapper.Reset();
        //                            Mapper.Initialize(cfg => cfg.CreateMap<TEPOAnnexureSpecification, TEPOAnnexureSpecification>()
        //                             .ForMember(a => a.POAnnexureSpecId, a => a.Ignore()));

        //                            var newAnnexSpecObject = Mapper.Map<TEPOAnnexureSpecification>(poAnnexSpec);
        //                            newAnnexSpecObject.POAnnexureId = newSpecObject.POAnnexureId;
        //                            context.TEPOAnnexureSpecifications.Add(newAnnexSpecObject);
        //                            context.SaveChanges();
        //                        }
        //                    }
        //                }
        //            }
        //            var POServiceAnnex = (from servannex in context.TEPOServiceAnnexures
        //                                  where servannex.PO_HeaderStructureID == headerID
        //                                   && servannex.PO_ItemStructureID == itemStructure.Uniqueid
        //                                   && servannex.IsDeleted == false
        //                                  select servannex).ToList();
        //            if (POServiceAnnex.Count > 0)
        //            {
        //                foreach (TEPOServiceAnnexure serAnnex in POServiceAnnex)
        //                {
        //                    Mapper.Reset();
        //                    Mapper.Initialize(cfg => cfg.CreateMap<TEPOServiceAnnexure, TEPOServiceAnnexure>()
        //                     .ForMember(a => a.POServiceAnnexureId, a => a.Ignore()));

        //                    var newServAnnexObject = Mapper.Map<TEPOServiceAnnexure>(serAnnex);
        //                    newServAnnexObject.PO_HeaderStructureID = uniqueId;
        //                    newServAnnexObject.PO_ItemStructureID = newItemObject.Uniqueid;
        //                    context.TEPOServiceAnnexures.Add(newServAnnexObject);
        //                    context.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    if (POPaymentTerms.Count > 0)
        //    {
        //        foreach (TEPOVendorPaymentMilestone pymntMileStone in POPaymentTerms)
        //        {
        //            Mapper.Reset();
        //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOVendorPaymentMilestone, TEPOVendorPaymentMilestone>()
        //             .ForMember(a => a.UniqueId, a => a.Ignore()));

        //            var newMSObject = Mapper.Map<TEPOVendorPaymentMilestone>(pymntMileStone);
        //            newMSObject.POHeaderStructureId = uniqueId;
        //            newMSObject.ContextIdentifier = uniqueId.ToString();
        //            newMSObject.LastModifiedBy = UserObj.UserName;
        //            newMSObject.LastModifiedOn = DateTime.Now;
        //            newMSObject.IsDeleted = false;
        //            context.TEPOVendorPaymentMilestones.Add(newMSObject);
        //            context.SaveChanges();
        //        }
        //    }

        //    if (SpecificTandCDtls.Count > 0)
        //    {
        //        List<TEPOSpecificTCDetailDTO> specDtoList = new List<TEPOSpecificTCDetailDTO>();
        //        List<TEPOSpecificTCDetail> speciDtoList = new List<TEPOSpecificTCDetail>();
        //        speciDtoList.AddRange(SpecificTandCDtls);
        //        foreach (TEPOSpecificTCDetail spTCDtl in SpecificTandCDtls)
        //        {
        //            Mapper.Reset();
        //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOSpecificTCDetail, TEPOSpecificTCDetail>()
        //             .ForMember(a => a.SpecificTCId, a => a.Ignore()));
        //            //.ForMember(a => a.POHeaderStructureId, a => a.Ignore()));
        //            var newSpecTCDtlObject = Mapper.Map<TEPOSpecificTCDetail>(spTCDtl);
        //            newSpecTCDtlObject.POHeaderStructureId = uniqueId;
        //            newSpecTCDtlObject.LastModifiedBy = UserObj.UserId;
        //            newSpecTCDtlObject.LastModifiedOn = DateTime.Now;
        //            context.TEPOSpecificTCDetails.Add(newSpecTCDtlObject);
        //            context.SaveChanges();
        //        }
        //        foreach (TEPOSpecificTCDetail spTCDtl in speciDtoList)
        //        {
        //            var specDtl = (from sub in context.TEPOSpecificTCDetails
        //                           where sub.SpecificTCId == spTCDtl.SpecificTCId
        //                           select sub).FirstOrDefault();
        //            specDtl.POHeaderStructureId = headerID;
        //            context.Entry(specDtl).CurrentValues.SetValues(specDtl);
        //            context.SaveChanges();
        //        }
        //    }


        //    if (LinkedPOList.Count > 0)
        //    {
        //        foreach (TEPOLinkedPO mainPO in LinkedPOList)
        //        {
        //            Mapper.Reset();
        //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOLinkedPO, TEPOLinkedPO>()
        //             .ForMember(a => a.UniqueID, a => a.Ignore()));

        //            var newMainPOObject = Mapper.Map<TEPOLinkedPO>(mainPO);
        //            newMainPOObject.MainPOID = uniqueId;
        //            newMainPOObject.LastModifiedBy = UserObj.UserId;
        //            newMainPOObject.LastModifiedOn = DateTime.Now;
        //            context.TEPOLinkedPOes.Add(newMainPOObject);
        //            context.SaveChanges();
        //        }
        //    }
        //    if (LinkedChildPOList.Count > 0)
        //    {
        //        foreach (TEPOLinkedPO childPO in LinkedChildPOList)
        //        {
        //            Mapper.Reset();
        //            Mapper.Initialize(cfg => cfg.CreateMap<TEPOLinkedPO, TEPOLinkedPO>()
        //             .ForMember(a => a.UniqueID, a => a.Ignore()));

        //            var newChildPOObject = Mapper.Map<TEPOLinkedPO>(childPO);
        //            newChildPOObject.LinkedPOID = uniqueId;
        //            newChildPOObject.LastModifiedBy = UserObj.UserId;
        //            newChildPOObject.LastModifiedOn = DateTime.Now;
        //            context.TEPOLinkedPOes.Add(newChildPOObject);
        //            context.SaveChanges();
        //        }
        //    }
        //    return uniqueId;
        //}

        #endregion
        public void SavePurchaseOrderApproverForDraft(int poID, int loginuserId, int mangerid)
        {
            string loginUsername = string.Empty;
            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(loginuserId);
            TEPOApprover submitter = new TEPOApprover();
            try
            {
                submitter.CreatedOn = System.DateTime.Now;
                // submitter.LastModifiedOn = System.DateTime.Now;
                submitter.CreatedBy = loginUsername;
                // submitter.LastModifiedBy = user.UserName;
                submitter.SequenceNumber = 0;
                submitter.POStructureId = poID;
                submitter.PurchaseOrderNumber = poID.ToString();
                submitter.ApproverName = loginUsername;
                submitter.Status = "Pending For Approval";
                submitter.ApproverId = loginuserId;
                context.TEPOApprovers.Add(submitter);
                context.SaveChanges();

                TEPOApprover poManager = new TEPOApprover();
                UserProfile managerInfo = context.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == mangerid).FirstOrDefault();
                if (managerInfo != null)
                {
                    poManager.CreatedOn = System.DateTime.Now;
                    //poManager.LastModifiedOn = System.DateTime.Now;
                    poManager.CreatedBy = loginUsername;
                    //poManager.LastModifiedBy = user.UserName;
                    poManager.SequenceNumber = 1;
                    poManager.POStructureId = poID;
                    poManager.PurchaseOrderNumber = poID.ToString();
                    poManager.ApproverName = managerInfo.CallName;
                    poManager.Status = "Draft";
                    poManager.ApproverId = mangerid;
                    context.TEPOApprovers.Add(poManager);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
        }
        public void SavePOSubmitterForDraft(int poID, int loginuserId)
        {
            string loginUsername = string.Empty;
            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(loginuserId);
            TEPOApprover submitter = new TEPOApprover();
            try
            {
                submitter.CreatedOn = System.DateTime.Now;
                // submitter.LastModifiedOn = System.DateTime.Now;
                submitter.CreatedBy = loginUsername;
                // submitter.LastModifiedBy = user.UserName;
                submitter.SequenceNumber = 0;
                submitter.POStructureId = poID;
                submitter.PurchaseOrderNumber = poID.ToString();
                submitter.ApproverName = loginUsername;
                submitter.Status = "Pending For Approval";
                submitter.ApproverId = loginuserId;
                context.TEPOApprovers.Add(submitter);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
        }
        public void SavePOManagerForDraft(int poID, int loginuserId, int mangerid)
        {
            string loginUsername = string.Empty;
            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(loginuserId);
            TEPOApprover submitter = new TEPOApprover();
            try
            {
                TEPOApprover poManager = new TEPOApprover();
                UserProfile managerInfo = context.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == mangerid).FirstOrDefault();
                if (managerInfo != null)
                {
                    poManager.CreatedOn = System.DateTime.Now;
                    //poManager.LastModifiedOn = System.DateTime.Now;
                    poManager.CreatedBy = loginUsername;
                    //poManager.LastModifiedBy = user.UserName;
                    poManager.SequenceNumber = 1;
                    poManager.POStructureId = poID;
                    poManager.PurchaseOrderNumber = poID.ToString();
                    poManager.ApproverName = managerInfo.CallName;
                    poManager.Status = "Draft";
                    poManager.ApproverId = mangerid;
                    context.TEPOApprovers.Add(poManager);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
        }

        public int SavePOSubmitterForDraft_old(int poID, int userID)
        {
            UserProfile user = context.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == userID).FirstOrDefault();

            var MasterApprover = context.POMasterApprovers.Where(x => x.IsDeleted == false && x.Type == "Submitter" && x.ApproverId == userID).FirstOrDefault();

            int uniqueID = 0;
            TEPOApprover submitter = new TEPOApprover();
            try
            {
                if (MasterApprover != null)
                {
                    submitter.CreatedOn = System.DateTime.Now;
                    submitter.LastModifiedOn = System.DateTime.Now;
                    submitter.CreatedBy = user.UserName;
                    submitter.LastModifiedBy = user.UserName;
                    submitter.SequenceNumber = 0;
                    submitter.POStructureId = poID;
                    submitter.PurchaseOrderNumber = poID.ToString();
                    submitter.ApproverName = user.CallName;
                    submitter.Status = "Pending For Approval";
                    submitter.ApproverId = MasterApprover.ApproverId;
                    context.TEPOApprovers.Add(submitter);
                    context.SaveChanges();
                    uniqueID = submitter.UniqueId;
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return uniqueID;
        }

        public int SavePO(TEPOHeaderStructure pur_head_struct_obj, int loginId)
        {
            int resultphsUniqueid = 0;
            try
            {
                pur_head_struct_obj.Objectid = "TEPurchaseHeaderService";
                pur_head_struct_obj.Status = "Active";
                pur_head_struct_obj.ReleaseCode2Status = "Draft";
                pur_head_struct_obj.IsNewPO = true;
                string FuguePurchaseOrderNumber = "Fugue_" + DateTime.Now.ToString("yyyymmdd_hhmmss");
                pur_head_struct_obj.Fugue_Purchasing_Order_Number = FuguePurchaseOrderNumber;
                //pur_head_struct_obj.Purchasing_Document_Date = DateTime.Now;
                pur_head_struct_obj.CreatedOn = DateTime.Now;
                pur_head_struct_obj.LastModifiedOn = DateTime.Now;
                pur_head_struct_obj.Version = "0";
                pur_head_struct_obj.IsDeleted = false;
                pur_head_struct_obj.Objectid = "TEPurchaseHeaderService";
                context.TEPOHeaderStructures.Add(pur_head_struct_obj);

                context.SaveChanges();
                resultphsUniqueid = pur_head_struct_obj.Uniqueid;

                if (resultphsUniqueid > 0)
                {
                    if (pur_head_struct_obj.IsPRPO == true)
                    {
                        new PurchaseOrderBAL().SavePOSubmitterForDraft(resultphsUniqueid, loginId);
                    }
                    else
                    {
                        int magaerId = Convert.ToInt32(pur_head_struct_obj.POManagerID);
                        new PurchaseOrderBAL().SavePurchaseOrderApproverForDraft(resultphsUniqueid, loginId, magaerId);
                    }
                    AutoSavePOGenTermConditions(resultphsUniqueid, pur_head_struct_obj.LastModifiedBy.ToString());
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return resultphsUniqueid;
        }
        public void AutoSavePOGenTermConditions(int poHeaderId, string lastmodifiedby)
        {
            try
            {
                int picklistitemitmId = 0;
                string picklistitemType = string.Empty;
                picklistitemitmId = getPickListItemId("General");
                if (picklistitemitmId > 0)
                    picklistitemType = picklistitemitmId.ToString();

                var masterTandClist = (from mtc in context.TEMasterTermsConditions
                                       where mtc.ModuleName == "PO" && mtc.Type == picklistitemType && mtc.IsDeleted == false
                                       && mtc.IsActive == true
                                       select new { mtc.Condition, mtc.SequenceId, mtc.UniqueId, mtc.Title }).Distinct().ToList();
                int cnt = 1;
                foreach (var mstrTC in masterTandClist)
                {
                    TETermsAndCondition term = new TETermsAndCondition();
                    term.CreatedBy = lastmodifiedby;
                    term.ContextIdentifier = poHeaderId.ToString();
                    term.Condition = mstrTC.Condition;
                    term.LastModifiedOn = DateTime.Now;
                    term.LastModifiedBy = lastmodifiedby;
                    term.CreatedBy = lastmodifiedby;
                    term.CreatedOn = DateTime.Now;
                    term.IsActive = true;
                    term.IsDeleted = false;
                    term.MasterId = mstrTC.UniqueId.ToString();
                    term.MasterTandCId = mstrTC.UniqueId;
                    term.SequenceId = mstrTC.SequenceId;
                    term.ModuleName = "PO";
                    term.PickListItemId = picklistitemitmId;
                    term.POHeaderStructureId = poHeaderId;
                    term.SequenceId = cnt;
                    term.Title = mstrTC.Title;
                    term.Type = picklistitemitmId.ToString();
                    context.TETermsAndConditions.Add(term);
                    context.SaveChanges();
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
        }

        public List<POSpecificTCDeatailDTO> GetPOSpecificTCDetailsByPOHeaderStructereId(int headerId)
        {
            List<POSpecificTCDeatailDTO> specDtlList = new List<POSpecificTCDeatailDTO>();
            List<POSpecificTermsDetail> spcTClist = new List<POSpecificTermsDetail>();
            try
            {
                spcTClist = (from
                               tcd in context.TEPOSpecificTCDetails
                                 join subTitle in context.TEPOSpecificTCSubTitleMasters on tcd.SpecificTCSubTitleMasterId equals subTitle.SpecificTCSubTitleMasterId into tempSub
                                 from SubTitleDetail in tempSub.DefaultIfEmpty()
                                 join title in context.TEPOSpecificTCTitleMasters on tcd.SpecificTCTitleMasterId equals title.SpecificTCTitleMasterId
                                 where tcd.POHeaderStructureId == headerId && tcd.IsDeleted == false
                                 select new POSpecificTermsDetail
                                 {
                                     Title = title.Title,
                                     SpecificTCTitleMasterId = title.SpecificTCTitleMasterId,
                                     SpecificTCSubTitleMasterId = SubTitleDetail.SpecificTCSubTitleMasterId,
                                     SubTitleDesc = SubTitleDetail.SubTitleDesc,
                                     SpecificTCId = tcd.SpecificTCId,
                                     Description = tcd.Description
                                 }).Distinct().ToList();
                if (spcTClist.Count > 0)
                {
                    var titleList = spcTClist.Select(a => new { a.SpecificTCTitleMasterId, a.Title }).Distinct().ToList();
                    foreach (var titleSpec in titleList)
                    {
                        POSpecificTCDeatailDTO specDtlDto = new POSpecificTCDeatailDTO();
                        specDtlDto.SpecificTCTitleMasterId = Convert.ToInt32(titleSpec.SpecificTCTitleMasterId);
                        specDtlDto.Title = titleSpec.Title;
                        var subTitles = spcTClist.Where(b => b.SpecificTCTitleMasterId == titleSpec.SpecificTCTitleMasterId).Select(a => new { a.SpecificTCSubTitleMasterId, a.SubTitleDesc }).Distinct().ToList();
                        List<POSpecificTCSubTitleDTO> specSubTitleDtoList = new List<POSpecificTCSubTitleDTO>();
                        foreach (var obj in subTitles)
                        {
                            var specDtls = spcTClist.Where(a => a.SpecificTCSubTitleMasterId == obj.SpecificTCSubTitleMasterId && a.SpecificTCTitleMasterId == titleSpec.SpecificTCTitleMasterId).ToList();
                            List<TEPOSpecificTCDetail> specDtlDtoList = new List<TEPOSpecificTCDetail>();
                            POSpecificTCSubTitleDTO subTitleDTO = new POSpecificTCSubTitleDTO();
                            subTitleDTO.SpecificTCSubTitleMasterId = Convert.ToInt32(obj.SpecificTCSubTitleMasterId);
                            subTitleDTO.SubTitleDesc = obj.SubTitleDesc;
                            foreach (var dtlObj in specDtls)
                            {
                                TEPOSpecificTCDetail specDtlObj = new TEPOSpecificTCDetail();
                                specDtlObj.SpecificTCId = dtlObj.SpecificTCId;
                                specDtlObj.Description = dtlObj.Description;
                                specDtlObj.SpecificTCSubTitleMasterId = Convert.ToInt32(dtlObj.SpecificTCSubTitleMasterId);
                                specDtlDtoList.Add(specDtlObj);
                            }
                            subTitleDTO.SpecificTCList = specDtlDtoList;
                            specSubTitleDtoList.Add(subTitleDTO);
                        }
                        specDtlDto.SpecSubTitlesList = specSubTitleDtoList;
                        if (specDtlDto != null)
                        {
                            specDtlList.Add(specDtlDto);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return specDtlList;
        }

        public List<POAnnexureSpecificatonDTO> GetPOAnnexureSpecificationsByHeaderStructureId(int headerId)
        {
            List<POAnnexureSpecificatonDTO> annexSpcList = new List<POAnnexureSpecificatonDTO>();
            try
            {
                annexSpcList = (from itm in context.TEPOItemStructures
                                join anex in context.TEPOAnnexures on itm.Uniqueid equals anex.PO_ItemStructureID
                                where itm.POStructureId == headerId && itm.IsDeleted == false && itm.Short_Text != String.Empty
                                select new POAnnexureSpecificatonDTO
                                {
                                    POHeaderStructureId = headerId,
                                    POItemStructureId = itm.Uniqueid,
                                    MaterialCode = itm.Material_Number,
                                    MaterialName = itm.Short_Text,
                                    POAnnexureId = anex.POAnnexureId,
                                    Title = anex.Title
                                }).Distinct().ToList();
                foreach (var data in annexSpcList)
                {
                    data.SpecsData = context.TEPOAnnexureSpecifications.Where(anex => anex.POAnnexureId == data.POAnnexureId && anex.IsDeleted == false).OrderBy(a => a.POAnnexureSpecId).ToList();
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return annexSpcList;
        }
        public List<POServiceAnnexureSpecificatonDTO> GetPOServiceAnnexureByHeaderStructureId(int headerId)
        {
            List<POServiceAnnexureSpecificatonDTO> annexSpcList = new List<POServiceAnnexureSpecificatonDTO>();
            try
            {
                annexSpcList = (from itm in context.TEPOItemStructures
                                join anex in context.TEPOServiceAnnexures on itm.Uniqueid equals anex.PO_ItemStructureID
                                where itm.POStructureId == headerId && itm.IsDeleted == false && (itm.Short_Text != String.Empty)
                                select new POServiceAnnexureSpecificatonDTO
                                {
                                    POHeaderStructureId = headerId,
                                    POItemStructureId = itm.Uniqueid,
                                    ServiceCode = itm.Material_Number,
                                    ServiceName = itm.Short_Text
                                }).Distinct().ToList();
                foreach (var data in annexSpcList)
                {
                    data.SpecsData = context.TEPOServiceAnnexures.Where(anex => anex.PO_ItemStructureID == data.POItemStructureId && anex.Value != "" && anex.IsDeleted == false).OrderBy(a => a.POServiceAnnexureId).ToList();
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return annexSpcList;
        }

        public int SaveVendor(VendorDTO vendorDto, int loginId)
        {
            int resultVendorMasterUniqueid = 0;
            try
            {
                if (vendorDto.POVendorMasterId == 0)
                {
                    TEPOVendorMaster vendorMaster = new TEPOVendorMaster();
                    vendorMaster.CIN = vendorDto.CIN;
                    vendorMaster.LastModifiedBy = loginId;
                    vendorMaster.LastModifiedOn = DateTime.Now;
                    vendorMaster.Currency = vendorDto.Currency;
                    vendorMaster.PAN = vendorDto.PAN;
                    vendorMaster.ServiceTax = vendorDto.ServiceTax;
                    vendorMaster.VendorContactId = vendorDto.VendorContactId;
                    vendorMaster.VendorName = vendorDto.VendorName;
                    vendorMaster.IsDeleted = false;
                    vendorMaster.IsActive = true;
                    context.TEPOVendorMasters.Add(vendorMaster);
                    context.SaveChanges();
                    resultVendorMasterUniqueid = vendorMaster.POVendorMasterId;
                    if (resultVendorMasterUniqueid > 0)
                    {
                        for (int cnt = 1; cnt < vendorDto.BillingAddress.Count(); cnt++)
                        {
                            string vendorcode = string.Empty;
                            TEPOVendorMasterDetail vendorMasterDetail = new TEPOVendorMasterDetail();
                            vendorMasterDetail.BankAccountName = vendorDto.BankAccountName[cnt];
                            vendorMasterDetail.BankAccountNumber = vendorDto.BankAccountNumber[cnt];
                            vendorMasterDetail.BankName = vendorDto.BankName[cnt];
                            vendorMasterDetail.BillingAddress = vendorDto.BillingAddress[cnt];
                            vendorMasterDetail.BillingCity = vendorDto.BillingCity[cnt];
                            vendorMasterDetail.BillingPostalCode = vendorDto.BillingPostalCode[cnt];
                            vendorMasterDetail.CountryId = vendorDto.CountryId[cnt];
                            vendorMasterDetail.GLAccountId = vendorDto.GLAccountId[cnt];
                            vendorMasterDetail.GSTApplicabilityId = vendorDto.GSTApplicabilityId[cnt];
                            vendorMasterDetail.GSTIN = vendorDto.GSTIN[cnt];
                            vendorMasterDetail.IFSCCode = vendorDto.IFSCCode[cnt];
                            vendorMasterDetail.IsDeleted = false;
                            vendorMasterDetail.LastModifiedBy = loginId;
                            vendorMasterDetail.LastModifiedOn = DateTime.Now;
                            vendorMasterDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.RegionId[cnt]))
                                vendorMasterDetail.RegionId = Convert.ToInt32(vendorDto.RegionId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.SchemaGroupId[cnt]))
                                vendorMasterDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.SchemaGroupId[cnt]);
                            vendorMasterDetail.ShippingAddress = vendorDto.ShippingAddress[cnt];
                            vendorMasterDetail.ShippingCity = vendorDto.ShippingCity[cnt];
                            vendorMasterDetail.ShippingPostalCode = vendorDto.ShippingPostalCode[cnt];
                            vendorMasterDetail.VendorAccountGroupId = vendorDto.VendorAccountGroupId[cnt];
                            vendorMasterDetail.VendorCategoryId = vendorDto.VendorCategoryId[cnt];
                            //vendorMasterDetail.VendorCode = vendorDto.VendorCode[cnt];
                            vendorMasterDetail.WithholdApplicability = vendorDto.WithholdApplicability[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxCodeId[cnt]))
                                vendorMasterDetail.WithholdTaxCodeId = Convert.ToInt32(vendorDto.WithholdTaxCodeId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxTypeId[cnt]))
                                vendorMasterDetail.WithholdTaxTypeId = Convert.ToInt32(vendorDto.WithholdTaxTypeId[cnt]);
                            vendorMasterDetail.ContactNumber = vendorDto.RepresentContactNumber[cnt];
                            vendorMasterDetail.EmailID = vendorDto.RepresentEmailID[cnt];
                            // vendorMasterDetail.Designation = vendorDto.Designation[cnt];
                            vendorMasterDetail.RepresentName = vendorDto.RepresentName[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.RepresentContactId[cnt]))
                                vendorMasterDetail.RepresentContactId = Convert.ToInt32(vendorDto.RepresentContactId[cnt]);
                            //vendorMasterDetail.CancelledChequeRef = vendorDto.CancelledChequeRef[cnt];
                            //vendorMasterDetail.GSTNRegnCertificateRef = vendorDto.GSTNRegnCertificateRef[cnt];
                            //vendorMasterDetail.IncorporationCertificateRef = vendorDto.IncorporationCertificateRef[cnt];
                            context.TEPOVendorMasterDetails.Add(vendorMasterDetail);
                            context.SaveChanges();
                            vendorcode = SaveVendorDetailsToSAP(vendorMasterDetail.POVendorDetailId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return resultVendorMasterUniqueid;
        }

        public int SaveVendorMaster(VendorMasterDto vendorDto, int loginId)
        {
            int vendorMasterUniqueid = 0, vendorMasterDetailUniqueid = 0;
            try
            {
                if (vendorDto.POVendorMasterId == 0)
                {
                    TEPOVendorMaster vendorMaster = new TEPOVendorMaster();
                    vendorMaster.CIN = vendorDto.CIN;
                    vendorMaster.LastModifiedBy = loginId;
                    vendorMaster.LastModifiedOn = DateTime.Now;
                    vendorMaster.Currency = vendorDto.Currency;
                    vendorMaster.PAN = vendorDto.PAN;
                    vendorMaster.ServiceTax = vendorDto.ServiceTax;
                    vendorMaster.VendorContactId = vendorDto.VendorContactId;
                    vendorMaster.VendorName = vendorDto.VendorName;
                    vendorMaster.IsDeleted = false;
                    vendorMaster.IsActive = true;
                    context.TEPOVendorMasters.Add(vendorMaster);
                    context.SaveChanges();
                    vendorMasterUniqueid = vendorMaster.POVendorMasterId;
                    if (vendorMasterUniqueid > 0 && vendorDto.VendorMasterDetails != null && vendorDto.VendorMasterDetails.Count > 0)
                    {
                        for (int cnt = 0; cnt < vendorDto.VendorMasterDetails.Count(); cnt++)
                        {
                            string vendorcode = string.Empty;
                            TEPOVendorMasterDetail vendorMasterDetail = new TEPOVendorMasterDetail();
                            vendorMasterDetail.BankAccountName = vendorDto.VendorMasterDetails[cnt].BankAccountName;
                            vendorMasterDetail.BankAccountNumber = vendorDto.VendorMasterDetails[cnt].BankAccountNumber;
                            vendorMasterDetail.BankName = vendorDto.VendorMasterDetails[cnt].BankName;
                            vendorMasterDetail.BillingAddress = vendorDto.VendorMasterDetails[cnt].BillingAddress;
                            vendorMasterDetail.BillingCity = vendorDto.VendorMasterDetails[cnt].BillingCity;
                            vendorMasterDetail.BillingPostalCode = vendorDto.VendorMasterDetails[cnt].BillingPostalCode;
                            vendorMasterDetail.CountryId = vendorDto.VendorMasterDetails[cnt].CountryId;
                            vendorMasterDetail.GLAccountId = vendorDto.VendorMasterDetails[cnt].GLAccountId;
                            vendorMasterDetail.GSTApplicabilityId = vendorDto.VendorMasterDetails[cnt].GSTApplicabilityId;
                            vendorMasterDetail.GSTIN = vendorDto.VendorMasterDetails[cnt].GSTIN;
                            vendorMasterDetail.IFSCCode = vendorDto.VendorMasterDetails[cnt].IFSCCode;
                            vendorMasterDetail.IsDeleted = false;
                            vendorMasterDetail.IsActive = true;
                            vendorMasterDetail.LastModifiedBy = loginId;
                            vendorMasterDetail.LastModifiedOn = DateTime.Now;
                            vendorMasterDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].RegionId))
                                vendorMasterDetail.RegionId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].RegionId);
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].SchemaGroupId))
                                vendorMasterDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].SchemaGroupId);
                            vendorMasterDetail.ShippingAddress = vendorDto.VendorMasterDetails[cnt].ShippingAddress;
                            vendorMasterDetail.ShippingCity = vendorDto.VendorMasterDetails[cnt].ShippingCity;
                            vendorMasterDetail.ShippingPostalCode = vendorDto.VendorMasterDetails[cnt].ShippingPostalCode;
                            vendorMasterDetail.VendorAccountGroupId = vendorDto.VendorMasterDetails[cnt].VendorAccountGroupId;
                            vendorMasterDetail.VendorCategoryId = vendorDto.VendorMasterDetails[cnt].VendorCategoryId;
                            vendorMasterDetail.ContactNumber = vendorDto.VendorMasterDetails[cnt].RepresentContactNumber;
                            vendorMasterDetail.EmailID = vendorDto.VendorMasterDetails[cnt].RepresentEmailID;
                            vendorMasterDetail.RepresentName = vendorDto.VendorMasterDetails[cnt].RepresentName;
                            vendorMasterDetail.RepresentContactId = vendorDto.VendorMasterDetails[cnt].RepresentContactId;
                            context.TEPOVendorMasterDetails.Add(vendorMasterDetail);
                            context.SaveChanges();
                            vendorMasterDetailUniqueid = vendorMasterDetail.POVendorDetailId;
                            if (vendorMasterDetailUniqueid > 0 && vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails != null
                                                && vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count > 0)
                            {
                                for (int i = 0; i < vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count(); i++)
                                {
                                    TEPOVendorWithHoldApplicabilityDetail vendorWithHoldDetail = new TEPOVendorWithHoldApplicabilityDetail();
                                    vendorWithHoldDetail.WithHoldingApplicability =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingApplicability;
                                    vendorWithHoldDetail.WithHoldingCodeId =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingCodeId;
                                    vendorWithHoldDetail.WithHoldingTaxTypeId =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingTaxTypeId;
                                    vendorWithHoldDetail.IsDeleted = false;
                                    vendorWithHoldDetail.LastModifiedBy = loginId;
                                    vendorWithHoldDetail.LastModifiedOn = DateTime.Now;
                                    vendorWithHoldDetail.POVendorDetailId = vendorMasterDetailUniqueid;
                                    context.TEPOVendorWithHoldApplicabilityDetails.Add(vendorWithHoldDetail);
                                    context.SaveChanges();
                                }
                            }
                            vendorcode = SaveVendorDetailsToSAP(vendorMasterDetail.POVendorDetailId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
            }
            return vendorMasterUniqueid;
        }

        public bool UpdateVendor(VendorDTO vendorDto, int loginId)
        {
            bool res = true;
            string vendorcode = string.Empty;
            try
            {
                if (vendorDto.POVendorMasterId > 0)
                {
                    var vendorMaster = context.TEPOVendorMasters.Where(a => a.POVendorMasterId == vendorDto.POVendorMasterId
                                        && a.IsDeleted == false).FirstOrDefault();
                    vendorMaster.CIN = vendorDto.CIN;
                    vendorMaster.LastModifiedBy = loginId;
                    vendorMaster.LastModifiedOn = DateTime.Now;
                    vendorMaster.Currency = vendorDto.Currency;
                    if (vendorDto.IsActive != null)
                        vendorMaster.IsActive = vendorDto.IsActive;
                    vendorMaster.PAN = vendorDto.PAN;
                    vendorMaster.ServiceTax = vendorDto.ServiceTax;
                    vendorMaster.VendorContactId = vendorDto.VendorContactId;
                    vendorMaster.VendorName = vendorDto.VendorName;
                    context.Entry(vendorMaster).CurrentValues.SetValues(vendorMaster);
                    context.SaveChanges();
                    for (int cnt = 1; cnt < vendorDto.POVendorDetailId.Count(); cnt++)
                    {
                        int tempDetlId = vendorDto.POVendorDetailId[cnt];
                        var vendorDetail = context.TEPOVendorMasterDetails.Where(a => a.POVendorDetailId == tempDetlId
                                          && a.IsDeleted == false).FirstOrDefault();
                        if (vendorDetail != null)
                        {
                            vendorDetail.BankAccountName = vendorDto.BankAccountName[cnt];
                            vendorDetail.BankAccountNumber = vendorDto.BankAccountNumber[cnt];
                            vendorDetail.BankName = vendorDto.BankName[cnt];
                            vendorDetail.BillingAddress = vendorDto.BillingAddress[cnt];
                            vendorDetail.BillingCity = vendorDto.BillingCity[cnt];
                            vendorDetail.BillingPostalCode = vendorDto.BillingPostalCode[cnt];
                            vendorDetail.CountryId = vendorDto.CountryId[cnt];
                            vendorDetail.GLAccountId = vendorDto.GLAccountId[cnt];
                            vendorDetail.GSTApplicabilityId = vendorDto.GSTApplicabilityId[cnt];
                            vendorDetail.GSTIN = vendorDto.GSTIN[cnt];
                            vendorDetail.IFSCCode = vendorDto.IFSCCode[cnt];
                            //vendorDetail.IsDeleted = false;
                            vendorDetail.LastModifiedBy = loginId;
                            vendorDetail.LastModifiedOn = DateTime.Now;
                            vendorDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.RegionId[cnt]))
                                vendorDetail.RegionId = Convert.ToInt32(vendorDto.RegionId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.SchemaGroupId[cnt]))
                                vendorDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.SchemaGroupId[cnt]);
                            vendorDetail.ShippingAddress = vendorDto.ShippingAddress[cnt];
                            vendorDetail.ShippingCity = vendorDto.ShippingCity[cnt];
                            vendorDetail.ShippingPostalCode = vendorDto.ShippingPostalCode[cnt];
                            vendorDetail.VendorAccountGroupId = vendorDto.VendorAccountGroupId[cnt];
                            vendorDetail.VendorCategoryId = vendorDto.VendorCategoryId[cnt];
                            //vendorDetail.VendorCode = vendorDto.VendorCode[cnt];
                            vendorDetail.WithholdApplicability = vendorDto.WithholdApplicability[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxCodeId[cnt]))
                                vendorDetail.WithholdTaxCodeId = Convert.ToInt32(vendorDto.WithholdTaxCodeId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxTypeId[cnt]))
                                vendorDetail.WithholdTaxTypeId = Convert.ToInt32(vendorDto.WithholdTaxTypeId[cnt]);
                            vendorDetail.ContactNumber = vendorDto.RepresentContactNumber[cnt];
                            vendorDetail.EmailID = vendorDto.RepresentEmailID[cnt];
                            // vendorDetail.Designation = vendorDto.Designation[cnt];
                            vendorDetail.RepresentName = vendorDto.RepresentName[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.RepresentContactId[cnt]))
                                vendorDetail.RepresentContactId = Convert.ToInt32(vendorDto.RepresentContactId[cnt]);
                            context.Entry(vendorDetail).CurrentValues.SetValues(vendorDetail);
                            context.SaveChanges();
                            vendorcode = SaveVendorDetailsToSAP(vendorDetail.POVendorDetailId);
                        }
                        else
                        {
                            TEPOVendorMasterDetail vendorMasterDetail = new TEPOVendorMasterDetail();
                            vendorMasterDetail.BankAccountName = vendorDto.BankAccountName[cnt];
                            vendorMasterDetail.BankAccountNumber = vendorDto.BankAccountNumber[cnt];
                            vendorMasterDetail.BankName = vendorDto.BankName[cnt];
                            vendorMasterDetail.BillingAddress = vendorDto.BillingAddress[cnt];
                            vendorMasterDetail.BillingCity = vendorDto.BillingCity[cnt];
                            vendorMasterDetail.BillingPostalCode = vendorDto.BillingPostalCode[cnt];
                            vendorMasterDetail.CountryId = vendorDto.CountryId[cnt];
                            vendorMasterDetail.GLAccountId = vendorDto.GLAccountId[cnt];
                            vendorMasterDetail.GSTApplicabilityId = vendorDto.GSTApplicabilityId[cnt];
                            vendorMasterDetail.GSTIN = vendorDto.GSTIN[cnt];
                            vendorMasterDetail.IFSCCode = vendorDto.IFSCCode[cnt];
                            vendorMasterDetail.IsDeleted = false;
                            vendorMasterDetail.LastModifiedBy = loginId;
                            vendorMasterDetail.LastModifiedOn = DateTime.Now;
                            vendorMasterDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.RegionId[cnt]))
                                vendorMasterDetail.RegionId = Convert.ToInt32(vendorDto.RegionId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.SchemaGroupId[cnt]))
                                vendorMasterDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.SchemaGroupId[cnt]);
                            vendorMasterDetail.ShippingAddress = vendorDto.ShippingAddress[cnt];
                            vendorMasterDetail.ShippingCity = vendorDto.ShippingCity[cnt];
                            vendorMasterDetail.ShippingPostalCode = vendorDto.ShippingPostalCode[cnt];
                            vendorMasterDetail.VendorAccountGroupId = vendorDto.VendorAccountGroupId[cnt];
                            vendorMasterDetail.VendorCategoryId = vendorDto.VendorCategoryId[cnt];
                            //vendorMasterDetail.VendorCode = vendorDto.VendorCode[cnt];
                            vendorMasterDetail.WithholdApplicability = vendorDto.WithholdApplicability[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxCodeId[cnt]))
                                vendorMasterDetail.WithholdTaxCodeId = Convert.ToInt32(vendorDto.WithholdTaxCodeId[cnt]);
                            if (!string.IsNullOrEmpty(vendorDto.WithholdTaxTypeId[cnt]))
                                vendorMasterDetail.WithholdTaxTypeId = Convert.ToInt32(vendorDto.WithholdTaxTypeId[cnt]);
                            vendorMasterDetail.ContactNumber = vendorDto.RepresentContactNumber[cnt];
                            vendorMasterDetail.EmailID = vendorDto.RepresentEmailID[cnt];
                            // vendorMasterDetail.Designation = vendorDto.Designation[cnt];
                            vendorMasterDetail.RepresentName = vendorDto.RepresentName[cnt];
                            if (!string.IsNullOrEmpty(vendorDto.RepresentContactId[cnt]))
                                vendorMasterDetail.RepresentContactId = Convert.ToInt32(vendorDto.RepresentContactId[cnt]);
                            //vendorMasterDetail.CancelledChequeRef = vendorDto.CancelledChequeRef[cnt];
                            //vendorMasterDetail.GSTNRegnCertificateRef = vendorDto.GSTNRegnCertificateRef[cnt];
                            //vendorMasterDetail.IncorporationCertificateRef = vendorDto.IncorporationCertificateRef[cnt];
                            context.Entry(vendorMasterDetail).CurrentValues.SetValues(vendorMasterDetail);
                            context.SaveChanges();
                            vendorcode = SaveVendorDetailsToSAP(vendorMasterDetail.POVendorDetailId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
                res = false;
            }
            return res;
        }

        public bool UpdateVendorMaster(VendorMasterDto vendorDto, int loginId)
        {
            bool res = true;
            string vendorcode = string.Empty;
            try
            {
                if (vendorDto.POVendorMasterId > 0)
                {
                    var vendorMaster = context.TEPOVendorMasters.Where(a => a.POVendorMasterId == vendorDto.POVendorMasterId && a.IsDeleted == false).FirstOrDefault();
                    vendorMaster.CIN = vendorDto.CIN;
                    vendorMaster.LastModifiedBy = loginId;
                    vendorMaster.LastModifiedOn = DateTime.Now;
                    vendorMaster.Currency = vendorDto.Currency;
                    if (vendorDto.IsActive != null) vendorMaster.IsActive = vendorDto.IsActive;
                    vendorMaster.PAN = vendorDto.PAN;
                    vendorMaster.ServiceTax = vendorDto.ServiceTax;
                    vendorMaster.VendorContactId = vendorDto.VendorContactId;
                    vendorMaster.VendorName = vendorDto.VendorName;
                    //vendorMaster.IsActive = vendorDto.IsActive;
                    context.Entry(vendorMaster).CurrentValues.SetValues(vendorMaster);
                    context.SaveChanges();

                    //Done By Piyush for Vendor TEContactDetails
                    if (vendorMaster.VendorContactId != null)
                    {
                        var vendorContact = context.TEContacts.Where(a => a.Uniqueid == vendorMaster.VendorContactId && a.IsDeleted == false).FirstOrDefault();
                        vendorContact.FirstName = vendorDto.VendorName;
                        vendorContact.CallName = vendorDto.VendorName;
                        context.Entry(vendorContact).CurrentValues.SetValues(vendorContact);
                        context.SaveChanges();
                    }


                    //for (int cnt = 1; cnt < vendorDto.POVendorDetailId.Count(); cnt++)
                    for (int cnt = 0; cnt < vendorDto.VendorMasterDetails.Count(); cnt++)
                    {
                        int tempDetlId = vendorDto.VendorMasterDetails[cnt].POVendorDetailId;
                        var vendorDetail = context.TEPOVendorMasterDetails.Where(a => a.POVendorDetailId == tempDetlId
                                          && a.IsDeleted == false).FirstOrDefault();
                        if (vendorDetail != null && vendorDetail.POVendorDetailId > 0)
                        {
                            vendorDetail.BankAccountName = vendorDto.VendorMasterDetails[cnt].BankAccountName;
                            vendorDetail.BankAccountNumber = vendorDto.VendorMasterDetails[cnt].BankAccountNumber;
                            vendorDetail.BankName = vendorDto.VendorMasterDetails[cnt].BankName;
                            vendorDetail.BillingAddress = vendorDto.VendorMasterDetails[cnt].BillingAddress;
                            vendorDetail.BillingCity = vendorDto.VendorMasterDetails[cnt].BillingCity;
                            vendorDetail.BillingPostalCode = vendorDto.VendorMasterDetails[cnt].BillingPostalCode;
                            vendorDetail.CountryId = vendorDto.VendorMasterDetails[cnt].CountryId;
                            vendorDetail.GLAccountId = vendorDto.VendorMasterDetails[cnt].GLAccountId;
                            vendorDetail.GSTApplicabilityId = vendorDto.VendorMasterDetails[cnt].GSTApplicabilityId;
                            vendorDetail.GSTIN = vendorDto.VendorMasterDetails[cnt].GSTIN;
                            vendorDetail.IFSCCode = vendorDto.VendorMasterDetails[cnt].IFSCCode;
                            //vendorDetail.IsDeleted = false;
                            vendorDetail.IsActive = vendorDto.VendorMasterDetails[cnt].IsActive;
                            vendorDetail.LastModifiedBy = loginId;
                            vendorDetail.LastModifiedOn = DateTime.Now;
                            vendorDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].RegionId))
                                vendorDetail.RegionId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].RegionId);
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].SchemaGroupId))
                                vendorDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].SchemaGroupId);
                            vendorDetail.ShippingAddress = vendorDto.VendorMasterDetails[cnt].ShippingAddress;
                            vendorDetail.ShippingCity = vendorDto.VendorMasterDetails[cnt].ShippingCity;
                            vendorDetail.ShippingPostalCode = vendorDto.VendorMasterDetails[cnt].ShippingPostalCode;
                            vendorDetail.VendorAccountGroupId = vendorDto.VendorMasterDetails[cnt].VendorAccountGroupId;
                            vendorDetail.VendorCategoryId = vendorDto.VendorMasterDetails[cnt].VendorCategoryId;
                            vendorDetail.ContactNumber = vendorDto.VendorMasterDetails[cnt].RepresentContactNumber;
                            vendorDetail.EmailID = vendorDto.VendorMasterDetails[cnt].RepresentEmailID;
                            vendorDetail.RepresentName = vendorDto.VendorMasterDetails[cnt].RepresentName;
                            vendorDetail.RepresentContactId = vendorDto.VendorMasterDetails[cnt].RepresentContactId;
                            context.Entry(vendorDetail).CurrentValues.SetValues(vendorDetail);
                            context.SaveChanges();
                            if (vendorDto.VendorMasterDetails[cnt].RepresentName != null)
                            {
                                int representUniqueid = vendorDto.VendorMasterDetails[cnt].RepresentContactId;
                                var representContact = context.TEContacts.Where(a => a.Uniqueid == representUniqueid && a.IsDeleted == false).FirstOrDefault();
                                representContact.FirstName = vendorDto.VendorMasterDetails[cnt].RepresentName;
                                representContact.CallName = vendorDto.VendorMasterDetails[cnt].RepresentName;
                                context.Entry(representContact).CurrentValues.SetValues(representContact);
                                context.SaveChanges();
                            }
                            if (vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails != null
                                                && vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count > 0)
                            {
                                for (int i = 0; i < vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count(); i++)
                                {
                                    int tempWithHoldDetlId = vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].POVendorWithHoldApplicabilityDetailId;
                                    if (tempWithHoldDetlId != 0)
                                    {
                                        TEPOVendorWithHoldApplicabilityDetail VendorWithHoldApplicability = new TEPOVendorWithHoldApplicabilityDetail();
                                        VendorWithHoldApplicability = context.TEPOVendorWithHoldApplicabilityDetails.
                                                        Where(a => a.VendorWithHoldApplicabilityId == tempWithHoldDetlId && a.IsDeleted == false).FirstOrDefault();
                                        VendorWithHoldApplicability.LastModifiedBy = loginId;
                                        VendorWithHoldApplicability.LastModifiedOn = DateTime.Now;
                                        VendorWithHoldApplicability.POVendorDetailId = tempDetlId;
                                        VendorWithHoldApplicability.WithHoldingApplicability = vendorDto.VendorMasterDetails[cnt].
                                                                                                        WithHoldApplicabilityDetails[i].WithHoldingApplicability;
                                        VendorWithHoldApplicability.WithHoldingCodeId = vendorDto.VendorMasterDetails[cnt].
                                                                                                        WithHoldApplicabilityDetails[i].WithHoldingCodeId;
                                        VendorWithHoldApplicability.WithHoldingTaxTypeId = vendorDto.VendorMasterDetails[cnt].
                                                                                                        WithHoldApplicabilityDetails[i].WithHoldingTaxTypeId;
                                        context.Entry(VendorWithHoldApplicability).CurrentValues.SetValues(VendorWithHoldApplicability);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        TEPOVendorWithHoldApplicabilityDetail vendorWithHoldDetail = new TEPOVendorWithHoldApplicabilityDetail();
                                        vendorWithHoldDetail.WithHoldingApplicability =
                                                                vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingApplicability;
                                        vendorWithHoldDetail.WithHoldingCodeId =
                                                                vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingCodeId;
                                        vendorWithHoldDetail.WithHoldingTaxTypeId =
                                                                vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingTaxTypeId;
                                        vendorWithHoldDetail.IsDeleted = false;
                                        vendorWithHoldDetail.LastModifiedBy = loginId;
                                        vendorWithHoldDetail.LastModifiedOn = DateTime.Now;
                                        vendorWithHoldDetail.POVendorDetailId = tempDetlId;
                                        context.TEPOVendorWithHoldApplicabilityDetails.Add(vendorWithHoldDetail);
                                        context.SaveChanges();
                                    }
                                }
                            }

                            vendorcode = SaveVendorDetailsToSAP(vendorDetail.POVendorDetailId);
                        }
                        else
                        {
                            TEPOVendorMasterDetail vendorMasterDetail = new TEPOVendorMasterDetail();
                            int vendorMasterDetailUniqueid = 0;
                            vendorMasterDetail.BankAccountName = vendorDto.VendorMasterDetails[cnt].BankAccountName;
                            vendorMasterDetail.BankAccountNumber = vendorDto.VendorMasterDetails[cnt].BankAccountNumber;
                            vendorMasterDetail.BankName = vendorDto.VendorMasterDetails[cnt].BankName;
                            vendorMasterDetail.BillingAddress = vendorDto.VendorMasterDetails[cnt].BillingAddress;
                            vendorMasterDetail.BillingCity = vendorDto.VendorMasterDetails[cnt].BillingCity;
                            vendorMasterDetail.BillingPostalCode = vendorDto.VendorMasterDetails[cnt].BillingPostalCode;
                            vendorMasterDetail.CountryId = vendorDto.VendorMasterDetails[cnt].CountryId;
                            vendorMasterDetail.GLAccountId = vendorDto.VendorMasterDetails[cnt].GLAccountId;
                            vendorMasterDetail.GSTApplicabilityId = vendorDto.VendorMasterDetails[cnt].GSTApplicabilityId;
                            vendorMasterDetail.GSTIN = vendorDto.VendorMasterDetails[cnt].GSTIN;
                            vendorMasterDetail.IFSCCode = vendorDto.VendorMasterDetails[cnt].IFSCCode;
                            vendorMasterDetail.IsDeleted = false;
                            vendorMasterDetail.IsActive = vendorDto.VendorMasterDetails[cnt].IsActive;
                            vendorMasterDetail.LastModifiedBy = loginId;
                            vendorMasterDetail.LastModifiedOn = DateTime.Now;
                            vendorMasterDetail.POVendorMasterId = vendorMaster.POVendorMasterId;
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].RegionId))
                                vendorMasterDetail.RegionId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].RegionId);
                            if (!string.IsNullOrEmpty(vendorDto.VendorMasterDetails[cnt].SchemaGroupId))
                                vendorMasterDetail.ScehmaGroupId = Convert.ToInt32(vendorDto.VendorMasterDetails[cnt].SchemaGroupId);
                            vendorMasterDetail.ShippingAddress = vendorDto.VendorMasterDetails[cnt].ShippingAddress;
                            vendorMasterDetail.ShippingCity = vendorDto.VendorMasterDetails[cnt].ShippingCity;
                            vendorMasterDetail.ShippingPostalCode = vendorDto.VendorMasterDetails[cnt].ShippingPostalCode;
                            vendorMasterDetail.VendorAccountGroupId = vendorDto.VendorMasterDetails[cnt].VendorAccountGroupId;
                            vendorMasterDetail.VendorCategoryId = vendorDto.VendorMasterDetails[cnt].VendorCategoryId;
                            vendorMasterDetail.ContactNumber = vendorDto.VendorMasterDetails[cnt].RepresentContactNumber;
                            vendorMasterDetail.EmailID = vendorDto.VendorMasterDetails[cnt].RepresentEmailID;
                            vendorMasterDetail.RepresentName = vendorDto.VendorMasterDetails[cnt].RepresentName;
                            vendorMasterDetail.RepresentContactId = vendorDto.VendorMasterDetails[cnt].RepresentContactId;
                            context.TEPOVendorMasterDetails.Add(vendorMasterDetail);
                            context.SaveChanges();
                            vendorMasterDetailUniqueid = (int)vendorMasterDetail.POVendorMasterId;
                            if (vendorMasterDetailUniqueid > 0 && vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails != null
                                                && vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count > 0)
                            {
                                for (int i = 0; i < vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails.Count(); i++)
                                {
                                    TEPOVendorWithHoldApplicabilityDetail vendorWithHoldDetail = new TEPOVendorWithHoldApplicabilityDetail();
                                    vendorWithHoldDetail.WithHoldingApplicability =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingApplicability;
                                    vendorWithHoldDetail.WithHoldingCodeId =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingCodeId;
                                    vendorWithHoldDetail.WithHoldingTaxTypeId =
                                                            vendorDto.VendorMasterDetails[cnt].WithHoldApplicabilityDetails[i].WithHoldingTaxTypeId;
                                    vendorWithHoldDetail.IsDeleted = false;
                                    vendorWithHoldDetail.LastModifiedBy = loginId;
                                    vendorWithHoldDetail.LastModifiedOn = DateTime.Now;
                                    vendorWithHoldDetail.POVendorDetailId = vendorMasterDetailUniqueid;
                                    context.TEPOVendorWithHoldApplicabilityDetails.Add(vendorWithHoldDetail);
                                    context.SaveChanges();
                                }
                            }
                            vendorcode = SaveVendorDetailsToSAP(vendorMasterDetail.POVendorDetailId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
                res = false;
            }
            return res;
        }

        public string SaveVendorDetailsToSAP_Old(int VendorMasterDetailID)
        {
            string VendorCode = string.Empty;
            try
            {
                string CompanyCode = string.Empty;
                string VendorName = string.Empty, VendorName1 = string.Empty, VendorName2 = string.Empty,
                       VendorName3 = string.Empty, VendorName4 = string.Empty;
                string BillingAddress = string.Empty, BillingAddress1 = string.Empty, BillingAddress2 = string.Empty,
                      BillingAddress3 = string.Empty;
                string PostalCode = string.Empty, City = string.Empty, CountryCode = string.Empty,
                        RegionCode = string.Empty, TelephoneNo = string.Empty, Email = string.Empty,
                        GLAccntNo = string.Empty, Currency = string.Empty, Category = string.Empty,
                        FugueId = string.Empty, ContactPerson = string.Empty, PAN = string.Empty,
                         GSTN = string.Empty, GSTClassification = string.Empty, WithHoldTaxType = string.Empty,
                         WithHoldTaxCode = string.Empty, WithholdApplicability = string.Empty;
                int VendorNameLength = 0, BillingAddressLength = 0;

                var vendorMasterDetail = context.TEPOVendorMasterDetails.Where(a => a.IsDeleted == false && a.POVendorDetailId == VendorMasterDetailID).FirstOrDefault();
                var vendorMaster = context.TEPOVendorMasters.Where(a => a.IsDeleted == false && a.POVendorMasterId == vendorMasterDetail.POVendorMasterId).FirstOrDefault();
                var cntry = context.TEPOCountryMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.CountryId).FirstOrDefault();
                var gstclass = context.TEPOGSTApplicabilityMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.GSTApplicabilityId).FirstOrDefault();
                var glAccnt = context.TEPOGLCodeMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.GLAccountId).FirstOrDefault();
                var withholdType = context.TEPOWithholdingTaxMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.WithholdTaxTypeId).FirstOrDefault();
                var withholdTaxCod = context.TEPOWithholdingTaxMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.WithholdTaxCodeId).FirstOrDefault();
                
                if (vendorMaster != null && vendorMasterDetail != null)
                {
                    #region fetch info
                    if (!string.IsNullOrEmpty(vendorMasterDetail.VendorCode))
                    {
                        VendorCode = vendorMasterDetail.VendorCode;
                    }
                    if (!string.IsNullOrEmpty(vendorMaster.VendorName))
                    {
                        VendorName = vendorMaster.VendorName;
                        VendorNameLength = VendorName.Length;
                        List<string> a = new List<string>();
                        if (VendorNameLength > 0)
                        {
                            string x = VendorName;
                            for (int i = 0; i < x.Length; i += 35)
                            {
                                if ((i + 35) < x.Length)
                                    a.Add(x.Substring(i, 35));
                                else
                                    a.Add(x.Substring(i));
                            }
                        }
                        if (a.Count > 0 && a.Count == 1)
                        {
                            VendorName1 = a[0];
                        }
                        else if (a.Count > 0 && a.Count == 2)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                        }
                        else if (a.Count > 0 && a.Count == 3)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                            VendorName3 = a[2];
                        }
                        else if (a.Count > 0 && a.Count >= 4)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                            VendorName3 = a[2];
                            VendorName4 = a[3];
                        }
                    }
                    if (!string.IsNullOrEmpty(vendorMasterDetail.BillingAddress))
                    {
                        BillingAddress = vendorMasterDetail.BillingAddress;
                        BillingAddressLength = BillingAddress.Length;
                        List<string> a = new List<string>();
                        if (BillingAddressLength > 0)
                        {
                            string x = BillingAddress;
                            for (int i = 0; i < x.Length; i += 60)
                            {
                                if ((i + 60) < x.Length)
                                    a.Add(x.Substring(i, 60));
                                else
                                    a.Add(x.Substring(i));
                            }
                        }
                        if (a.Count > 0 && a.Count == 1)
                        {
                            BillingAddress1 = a[0];
                        }
                        else if (a.Count > 0 && a.Count == 2)
                        {
                            BillingAddress1 = a[0];
                            BillingAddress2 = a[1];
                        }
                        else if (a.Count > 0 && a.Count >= 3)
                        {
                            BillingAddress1 = a[0];
                            BillingAddress2 = a[1];
                            BillingAddress3 = a[2];
                        }
                    }
                    #endregion
                    PostalCode = vendorMasterDetail.BillingPostalCode;
                    City = vendorMasterDetail.BillingCity;
                    if (cntry != null)
                        CountryCode = cntry.CountryCode;
                    TelephoneNo = vendorMasterDetail.ContactNumber;
                    Email = vendorMasterDetail.EmailID;
                    if (glAccnt != null)
                        GLAccntNo = glAccnt.GLAccountCode;
                    Currency = vendorMaster.Currency;
                    //Currency = "INR";
                    var vendrCtg = context.TEPOVendorCategoryMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.VendorCategoryId).FirstOrDefault();
                    Category = vendrCtg.TEVendorCategoryCode;
                    FugueId = vendorMasterDetail.POVendorDetailId.ToString();
                    ContactPerson = vendorMasterDetail.RepresentName;
                    if (vendorMasterDetail.RepresentName.Length > 30)
                        ContactPerson = vendorMasterDetail.RepresentName.Substring(0, 29);
                    PAN = vendorMaster.PAN;
                    GSTN = vendorMasterDetail.GSTIN;
                    if (gstclass != null)
                        GSTClassification = gstclass.GSTApplicabilityCode.ToString();
                    if (withholdType != null)
                        WithHoldTaxType = withholdType.WithholdingTaxCode;
                    if (withholdTaxCod != null)
                        WithHoldTaxCode = withholdType.WithholdingTaxCode;
                    WithholdApplicability = vendorMasterDetail.WithholdApplicability;
                }

                VendorMasterReqItem vendorReq = new VendorMasterReqItem();
                vendorReq.AKONT = GLAccntNo;
                vendorReq.BUKRS = "1000";//CompanyCode
                vendorReq.BUSAB = Category;
                vendorReq.CITY1 = City;
                vendorReq.COUNTRY = CountryCode;
                vendorReq.EKORG = "1000";
                vendorReq.FUGUE_ID = FugueId;
                vendorReq.J_1ICSTNO = "";
                vendorReq.J_1IEXRN = "";
                vendorReq.J_1ILSTNO = "";
                vendorReq.J_1IPANNO = PAN;
                vendorReq.J_1ISERN = "";
                if (Currency == "INR")
                    vendorReq.KALSK = "01";
                else
                    vendorReq.KALSK = "02";
                vendorReq.KTOKK = "ZVEN";
                vendorReq.LEBRE = "X";
                vendorReq.LIFNR = VendorCode;
                vendorReq.LNRZA = "";
                vendorReq.NAME1 = VendorName1;
                vendorReq.NAME2 = VendorName2;
                vendorReq.NAME3 = VendorName3;
                vendorReq.NAME4 = VendorName4;
                vendorReq.PARNR = "";
                vendorReq.PARVW = "";
                vendorReq.POST_CODE1 = PostalCode;
                if (CountryCode == "IN")
                    vendorReq.QLAND = "IN";
                else
                    vendorReq.QLAND = "";
                vendorReq.REGION = RegionCode;
                vendorReq.REPRF = "X";
                vendorReq.SMTP_ADDR = Email;
                vendorReq.SORT1 = "Fugue_Vendor_" + FugueId;
                vendorReq.STCD3 = GSTN;
                vendorReq.STREET = BillingAddress1;
                vendorReq.STREET2 = BillingAddress2;
                vendorReq.STREET3 = BillingAddress3;
                vendorReq.TEL_NUMBER = TelephoneNo;
                vendorReq.VEN_CLASS = GSTClassification;
                vendorReq.VERKF = ContactPerson;
                vendorReq.WAERS = Currency;
                vendorReq.WEBRE = "X";
                vendorReq.XZEMP = "";
                vendorReq.ZTERM = "T033";
                vendorReq.ZWELS = "C";

                List<VendorMasterReqItemItem> vendorReqItemList = new List<VendorMasterReqItemItem>();
                VendorMasterReqItemItem vendorReqItem = new VendorMasterReqItemItem();
                vendorReqItem.LIFNR = VendorCode;
                vendorReqItem.WITHT = WithHoldTaxType;
                vendorReqItem.WT_WITHCD = WithHoldTaxCode;
                if (WithholdApplicability.ToLower() == "true"
                    || WithholdApplicability.ToLower() == "yes"
                    || WithholdApplicability.ToLower() == "1")
                    vendorReqItem.WT_SUBJCT = "X";
                else
                    vendorReqItem.WT_SUBJCT = "";
                vendorReqItemList.Add(vendorReqItem);
                vendorReq.item = vendorReqItemList.ToArray();

                List<VendorMasterReqItem> vendorReqList = new List<VendorMasterReqItem>();
                vendorReqList.Add(vendorReq);

                SAPCallConnector sapCall = new SAPCallConnector();
                IList<VendorMasterResItem> RespList = new List<VendorMasterResItem>();
                RespList = sapCall.CreateVendor(vendorReqList);
                foreach (VendorMasterResItem item in RespList)
                {
                    if (item.RETCODE == "0")
                    {
                        VendorCode = item.LIFNR;
                        vendorMasterDetail.VendorCode = VendorCode;
                        context.Entry(vendorMasterDetail).CurrentValues.SetValues(vendorMasterDetail);
                        context.SaveChanges();
                        if (log.IsDebugEnabled)
                            log.Debug("Vendor Details Posted Successfully:" + VendorCode);
                    }
                    else
                    {
                        ApplicationErrorLog errorlog = new ApplicationErrorLog();
                        errorlog.InnerException = vendorMasterDetail.POVendorDetailId.ToString();
                        errorlog.Stacktrace = item.MESSAGE;
                        errorlog.Source = "Vendor Posting";
                        errorlog.Error = item.MESSAGE;
                        errorlog.ExceptionDateTime = DateTime.Now;
                        context.ApplicationErrorLogs.Add(errorlog);
                        context.SaveChanges();

                        if (log.IsDebugEnabled)
                            log.Debug("Failed To Post Vendor:" + item.MESSAGE);
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
                if (log.IsDebugEnabled)
                    log.Debug("Failed To Post Vendor:" + ex.Message);
            }
            return VendorCode;
        }

        public string SaveVendorDetailsToSAP(int VendorMasterDetailID)
        {
            string VendorCode = string.Empty;
            try
            {
                string CompanyCode = string.Empty;
                string VendorName = string.Empty, VendorName1 = string.Empty, VendorName2 = string.Empty,
                       VendorName3 = string.Empty, VendorName4 = string.Empty;
                string BillingAddress = string.Empty, BillingAddress1 = string.Empty, BillingAddress2 = string.Empty,
                      BillingAddress3 = string.Empty;
                string PostalCode = string.Empty, City = string.Empty, CountryCode = string.Empty,
                        RegionCode = string.Empty, TelephoneNo = string.Empty, Email = string.Empty,
                        GLAccntNo = string.Empty, Currency = string.Empty, Category = string.Empty,
                        FugueId = string.Empty, ContactPerson = string.Empty, PAN = string.Empty,
                         GSTN = string.Empty, GSTClassification = string.Empty;
                int VendorNameLength = 0, BillingAddressLength = 0; string vendorAccGroupID = string.Empty;
                string vendorAccGroupName = string.Empty;

                var vendorMasterDetail = context.TEPOVendorMasterDetails.Where(a => a.IsDeleted == false && a.POVendorDetailId == VendorMasterDetailID).FirstOrDefault();
                if(vendorMasterDetail.VendorAccountGroupId != null)
                {
                    vendorAccGroupID = context.TEPOVendorAccountGroupMasters.Where(x => x.UniqueID == vendorMasterDetail.VendorAccountGroupId).Select(a => a.VendorAccountGroupCode).FirstOrDefault();
                }
                vendorAccGroupName = context.TEPOVendorAccountGroupMasters.Where(x => x.IsDeleted == false && x.VendorAccountGroupCode == vendorAccGroupID).Select(x => x.Description).FirstOrDefault();
                var vendorMaster = context.TEPOVendorMasters.Where(a => a.IsDeleted == false && a.POVendorMasterId == vendorMasterDetail.POVendorMasterId).FirstOrDefault();
                var cntry = context.TEPOCountryMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.CountryId).FirstOrDefault();
                var gstclass = context.TEPOGSTApplicabilityMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.GSTApplicabilityId).FirstOrDefault();
                var glAccnt = context.TEPOGLCodeMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.GLAccountId).FirstOrDefault();
                var vendorWithHoldDetail = context.TEPOVendorWithHoldApplicabilityDetails.Where(a => a.IsDeleted == false
                                                                     && a.POVendorDetailId == vendorMasterDetail.POVendorDetailId).ToList();

                if (!string.IsNullOrEmpty(vendorMasterDetail.RegionId.ToString()))
                {
                    RegionCode = context.TEGSTNStateMasters.Where(x => x.StateID == vendorMasterDetail.RegionId && x.IsDeleted == false).Select(x => x.StateCode).FirstOrDefault().ToString();
                }

                if (vendorMaster != null && vendorMasterDetail != null)
                {
                    #region fetch info
                    if (!string.IsNullOrEmpty(vendorMasterDetail.VendorCode))
                    {
                        VendorCode = vendorMasterDetail.VendorCode;
                    }
                    if (!string.IsNullOrEmpty(vendorMaster.VendorName))
                    {
                        string tempName = string.Empty;
                        if(vendorMaster.VendorName.Contains(' '))
                            tempName = vendorMaster.VendorName.Substring(0, vendorMaster.VendorName.IndexOf(' '));
                        if(tempName.Equals("Mr") || tempName.Equals("Mrs") || tempName.Equals("Ms") || tempName.Equals("M/S") || tempName.Equals("Dr."))
                        {
                            vendorMaster.VendorName = vendorMaster.VendorName.Substring(vendorMaster.VendorName.IndexOf(' '));
                        }
                        VendorName = vendorMaster.VendorName.Trim();
                        VendorNameLength = VendorName.Length;
                        List<string> a = new List<string>();
                        if (VendorNameLength > 0)
                        {
                            string x = VendorName;
                            for (int i = 0; i < x.Length; i += 35)
                            {
                                if ((i + 35) < x.Length)
                                    a.Add(x.Substring(i, 35));
                                else
                                    a.Add(x.Substring(i));
                            }
                        }
                        if (a.Count > 0 && a.Count == 1)
                        {
                            VendorName1 = a[0];
                        }
                        else if (a.Count > 0 && a.Count == 2)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                        }
                        else if (a.Count > 0 && a.Count == 3)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                            VendorName3 = a[2];
                        }
                        else if (a.Count > 0 && a.Count >= 4)
                        {
                            VendorName1 = a[0];
                            VendorName2 = a[1];
                            VendorName3 = a[2];
                            VendorName4 = a[3];
                        }
                    }
                    if (!string.IsNullOrEmpty(vendorMasterDetail.BillingAddress))
                    {
                        BillingAddress = vendorMasterDetail.BillingAddress;
                        BillingAddressLength = BillingAddress.Length;
                        List<string> a = new List<string>();
                        if (BillingAddressLength > 0)
                        {
                            string x = BillingAddress;
                            for (int i = 0; i < x.Length; i += 60)
                            {
                                if ((i + 60) < x.Length)
                                    a.Add(x.Substring(i, 60));
                                else
                                    a.Add(x.Substring(i));
                            }
                        }
                        if (a.Count > 0 && a.Count == 1)
                        {
                            BillingAddress1 = a[0];
                        }
                        else if (a.Count > 0 && a.Count == 2)
                        {
                            BillingAddress1 = a[0];
                            BillingAddress2 = a[1];
                        }
                        else if (a.Count > 0 && a.Count >= 3)
                        {
                            BillingAddress1 = a[0];
                            BillingAddress2 = a[1];
                            BillingAddress3 = a[2];
                        }
                    }
                    #endregion
                    PostalCode = vendorMasterDetail.BillingPostalCode;
                    City = vendorMasterDetail.BillingCity;
                    if (cntry != null)
                        CountryCode = cntry.CountryCode;
                    TelephoneNo = vendorMasterDetail.ContactNumber;
                    Email = vendorMasterDetail.EmailID;
                    if (glAccnt != null)
                        GLAccntNo = glAccnt.GLAccountCode;
                    Currency = vendorMaster.Currency;
                    //Currency = "INR";
                    var vendrCtg = context.TEPOVendorCategoryMasters.Where(a => a.IsDeleted == false && a.UniqueID == vendorMasterDetail.VendorCategoryId).FirstOrDefault();
                    Category = vendrCtg.TEVendorCategoryCode;
                    FugueId = vendorMasterDetail.POVendorDetailId.ToString();
                    ContactPerson = vendorMasterDetail.RepresentName;
                    if (vendorMasterDetail.RepresentName.Length > 30)
                        ContactPerson = vendorMasterDetail.RepresentName.Substring(0, 29);
                    PAN = vendorMaster.PAN;
                    GSTN = vendorMasterDetail.GSTIN;
                    if (gstclass != null)
                    {
                        if(gstclass.GSTApplicabilityCode == 5)
                        {
                            GSTClassification = string.Empty;
                        }
                        else
                        {
                            GSTClassification = gstclass.GSTApplicabilityCode.ToString();
                        }
                    }
                        
                }

                VendorMasterReqItem vendorReq = new VendorMasterReqItem();
                vendorReq.AKONT = GLAccntNo;
                vendorReq.BUKRS = "1000";//CompanyCode
                vendorReq.BUSAB = Category;
                vendorReq.CITY1 = City;
                vendorReq.COUNTRY = CountryCode;
                vendorReq.EKORG = "1000";
                vendorReq.FUGUE_ID = FugueId;
                vendorReq.J_1ICSTNO = "";
                vendorReq.J_1IEXRN = "";
                vendorReq.J_1ILSTNO = "";
                vendorReq.J_1IPANNO = PAN;
                vendorReq.J_1ISERN = "";
                if (Currency == "INR")
                    vendorReq.KALSK = "01";
                else
                    vendorReq.KALSK = "02";
                vendorReq.KTOKK = vendorAccGroupID;
                vendorReq.LEBRE = "X";
                vendorReq.LIFNR = VendorCode;
                vendorReq.LNRZA = "";
                vendorReq.NAME1 = VendorName1;
                vendorReq.NAME2 = VendorName2;
                vendorReq.NAME3 = VendorName3;
                vendorReq.NAME4 = VendorName4;
                vendorReq.PARNR = "";
                vendorReq.PARVW = "";
                vendorReq.POST_CODE1 = PostalCode;
                if (CountryCode == "IN")
                    vendorReq.QLAND = "IN";
                else
                    vendorReq.QLAND = "";
                vendorReq.REGION = RegionCode;
                vendorReq.REPRF = "X";
                vendorReq.SMTP_ADDR = Email;
                vendorReq.SORT1 = "Fugue_Vendor_" + FugueId;
                vendorReq.STCD3 = GSTN;
                vendorReq.STREET = BillingAddress1;
                vendorReq.STREET2 = BillingAddress2;
                vendorReq.STREET3 = BillingAddress3;
                vendorReq.TEL_NUMBER = TelephoneNo;
                vendorReq.VEN_CLASS = GSTClassification;
                vendorReq.VERKF = ContactPerson;
                vendorReq.WAERS = Currency;
                vendorReq.WEBRE = "X";
                vendorReq.XZEMP = "";
                vendorReq.ZTERM = "T033";
                vendorReq.ZWELS = "C";

                List<VendorMasterReqItemItem> vendorReqItemList = new List<VendorMasterReqItemItem>();
                if (vendorWithHoldDetail != null && vendorWithHoldDetail.Count > 0)
                {
                    for (int i = 0; i < vendorWithHoldDetail.Count(); i++)
                    {
                        string WithHoldTaxType = string.Empty,
                        WithHoldTaxCode = string.Empty, WithholdApplicability = string.Empty;
                        int uniqueidTax = Convert.ToInt32(vendorWithHoldDetail[i].WithHoldingTaxTypeId);
                        var withholdType = context.TEPOWithholdingTaxMasters.Where(a => a.IsDeleted == false &&
                                            a.UniqueID == uniqueidTax).FirstOrDefault();
                        int uniqueidCode = Convert.ToInt32(vendorWithHoldDetail[i].WithHoldingCodeId);
                        var withholdTaxCod = context.TEPOWithholdingTaxMasters.Where(a => a.IsDeleted == false &&
                                            a.UniqueID == uniqueidCode).FirstOrDefault();
                        if (withholdType != null)
                            WithHoldTaxType = withholdType.WithholdingTaxCode;
                        if (withholdTaxCod != null)
                            WithHoldTaxCode = withholdType.WithholdingTaxCode;
                        WithholdApplicability = vendorWithHoldDetail[i].WithHoldingApplicability.ToString();

                        VendorMasterReqItemItem vendorReqItem = new VendorMasterReqItemItem();
                        vendorReqItem.LIFNR = VendorCode;
                        vendorReqItem.WITHT = WithHoldTaxType;
                        vendorReqItem.WT_WITHCD = WithHoldTaxCode;
                        if (WithholdApplicability.ToLower() == "true"
                            || WithholdApplicability.ToLower() == "yes"
                            || WithholdApplicability.ToLower() == "1")
                            vendorReqItem.WT_SUBJCT = "X";
                        else
                            vendorReqItem.WT_SUBJCT = "";
                        vendorReqItemList.Add(vendorReqItem);
                    }
                }

                vendorReq.item = vendorReqItemList.ToArray();

                List<VendorMasterReqItem> vendorReqList = new List<VendorMasterReqItem>();
                vendorReqList.Add(vendorReq);

                SAPCallConnector sapCall = new SAPCallConnector();
                IList<VendorMasterResItem> RespList = new List<VendorMasterResItem>();
                RespList = sapCall.CreateVendor(vendorReqList);
                foreach (VendorMasterResItem item in RespList)
                {
                    if (item.RETCODE == "0")
                    {
                        VendorCode = item.LIFNR;
                        VendorCode = VendorCode.ToString().TrimStart('0');
                        vendorMasterDetail.VendorCode = VendorCode.ToString();
                        context.Entry(vendorMasterDetail).CurrentValues.SetValues(vendorMasterDetail);
                        context.SaveChanges();
                        if (log.IsDebugEnabled)
                            log.Debug("Vendor Details Posted Successfully:" + VendorCode);
                    }
                    else
                    {
                        ApplicationErrorLog errorlog = new ApplicationErrorLog();
                        errorlog.InnerException = vendorMasterDetail.POVendorDetailId.ToString();
                        errorlog.Stacktrace = item.MESSAGE;
                        errorlog.Source = "Vendor Posting";
                        errorlog.Error = item.MESSAGE;
                        errorlog.ExceptionDateTime = DateTime.Now;
                        context.ApplicationErrorLogs.Add(errorlog);
                        context.SaveChanges();

                        if (log.IsDebugEnabled)
                            log.Debug("Failed To Post Vendor:" + item.MESSAGE);
                    }
                }
            }
            catch (Exception ex)
            {
                exception.RecordUnHandledException(ex);
                if (log.IsDebugEnabled)
                    log.Debug("Failed To Post Vendor:" + ex.Message);
            }
            return VendorCode;
        }

        public static string ObjectToSOAP(object Object)
        {
            if (Object == null)
            {
                throw new ArgumentException("Object can not be null");
            }
            using (MemoryStream Stream = new MemoryStream())
            {
                SoapFormatter Serializer = new SoapFormatter();
                Serializer.Serialize(Stream, Object);
                Stream.Flush();
                //return UTF8Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
                string strObject = "";
                strObject =
                Encoding.ASCII.GetString(Stream.GetBuffer());

                //Check for the null terminator character
                int index = strObject.IndexOf("\0");
                if (index > 0)
                {
                    strObject = strObject.Substring(0, index);
                }
                return strObject;
            }
        }

        public class POSpecificTermsDetail
        {
            public string Title { get; set; }
            public int? SpecificTCTitleMasterId { get; set; }
            public int? SpecificTCSubTitleMasterId { get; set; }
            public string SubTitleDesc { get; set; }
            public int SpecificTCId { get; set; }
            public string Description { get; set; }
        }
    }
  

}

#endregion