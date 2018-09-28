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
using System.Globalization;
//using System.Web.Cors;

namespace PO.Controllers
{
    public class POHSNRateAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        POHSNRate HSNBAL = new POHSNRate();

        public POHSNRateAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        
        [HttpPost]
        public HttpResponseMessage GetHSNTaxRate(JObject json)
        {
            int pagenumber = json["page_number"].ToObject<int>();
            int pagepercount = json["pagepercount"].ToObject<int>();
            int count = 0;
            try {


                var HSNTaxCodeList = (from HSNTx in db.TEPOHSNTaxCodeMappings
                                      //join VenCatMast in db.TEPOVendorCategoryMasters on
                                      //HSNTx.GSTVendorClassification equals VenCatMast.UniqueID into temp
                                      //from tempVenCat in temp.DefaultIfEmpty()
                                      join GSTAppMast in db.TEPOGSTApplicabilityMasters on
                                      HSNTx.MaterialGSTApplicability equals GSTAppMast.UniqueID into tempMat
                                      from tempMatApp in tempMat.DefaultIfEmpty()
                                      join GSTVenAppMast in db.TEPOGSTApplicabilityMasters on
                                      HSNTx.VendorGSTApplicability equals GSTVenAppMast.UniqueID into tempVen
                                      from tempVenApp in tempVen.DefaultIfEmpty()
                                      join user in db.UserProfiles on HSNTx.LastModifiedBy equals user.UserId into tempuser
                                      from prof in tempuser.DefaultIfEmpty()
                                      where HSNTx.IsDeleted == false
                                      select new
                                      {
                                          HSNTx.ApplicableTo,
                                          HSNTx.DestinationCountry,
                                          HSNTx.VendorRegionDescription,
                                          HSNTx.DeliveryPlantRegionDescription,
                                          HSNTx.HSNCode,
                                          MaterialGSTApplicability = tempMatApp.Description,
                                          VendorGSTApplicability = tempVenApp.Description,
                                          HSNTx.ValidFrom,
                                          HSNTx.ValidTo,
                                          HSNTx.TaxType,
                                          HSNTx.TaxCode,
                                          HSNTx.TaxRate,
                                          HSNTx.UniqueID,
                                          HSNTx.LastModifiedOn,
                                          prof.UserName,
                                          HSNTx.IsDeleted
                                      }).OrderByDescending(x => x.UniqueID).ToList();

                count = HSNTaxCodeList.Count();
            if (count > 0)
            {
                if (pagenumber == 0)
                {
                    pagenumber = 1;
                }
                int iPageNum = pagenumber;
                int iPageSize = pagepercount;
                int start = iPageNum - 1;
                start = start * iPageSize;
                var finalResult = HSNTaxCodeList.Skip(start).Take(iPageSize).ToList();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Success";
                sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                sinfo.torecords = finalResult.Count() + start;
                sinfo.totalrecords = count;
                sinfo.listcount = finalResult.Count();
                sinfo.pages = "1";

                if (finalResult.Count() > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = finalResult, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = finalResult, info = sinfo }) };
                }
            }
            else
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "No Records";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNTaxCodeList, info = sinfo }) };
            }
        }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
    }
        }

        //HSN Codes From Model For the Dropdown
        [HttpPost]
        public HttpResponseMessage GetHSNCode(JObject json)
        {
            try
            {
                var HSNList = (from HSN in db.TEPOHSNMasters
                                   where HSN.IsDeleted == false
                                   select new
                                   {
                                       UniqueID = HSN.UniqueID,
                                       CountryCode = HSN.CountryCode,
                                       HSNSACCode = HSN.HSNSACCode,
                                       LastModifiedBy = HSN.LastModifiedBy,
                                       LastModifiedOn = HSN.LastModifiedOn
                                   }).OrderBy(x => x.HSNSACCode).ToList();
                int count = HSNList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //Material GST From Model For the Dropdown -- NOT IN USE--
        [HttpPost]
        public HttpResponseMessage GetMatGstAppl(JObject json)

        {
            try
            {
                var HSNList = (from GSTAppl in db.TEPOGSTApplicabilityMasters
                               where GSTAppl.IsDeleted == false
                               select new
                               {
                                   UniqueID = GSTAppl.UniqueID,
                                   AppCode = GSTAppl.GSTApplicabilityCode,
                                   Description = GSTAppl.Description,
                                   LastModifiedBy = GSTAppl.LastModifiedBy,
                                   LastModifiedOn = GSTAppl.LastModifiedOn
                               }).ToList();
                int count = HSNList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //GST Vendor Classification From Model For the Dropdown
        [HttpPost]
        public HttpResponseMessage GetVenCatMast(JObject json)
        {
            try
            {
                var HSNList = db.TEPOVendorCategoryMasters.Where(x => x.IsDeleted == false).OrderBy(x => x.Description).ToList();
                int count = HSNList.Count();
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //Material GST From Model For the Dropdown
        [HttpPost]
        public HttpResponseMessage GetMatAppl(JObject json)
        {
            try
            {
                String ApplicableTo = json["Applicable_To"].ToObject<String>();
                var HSNList = (from GSTAppMap in db.TEPOGSTApplicabilityMappings
                               join GSTAppMast in db.TEPOGSTApplicabilityMasters on
                               GSTAppMap.GSTApplicabilityMasterId equals GSTAppMast.UniqueID into tempuser
                               from Tempdat in tempuser.DefaultIfEmpty()
                               where GSTAppMap.IsDeleted == false && GSTAppMap.ApplicableTo == ApplicableTo
                               select new
                               {
                                   Tempdat.UniqueID,
                                   Tempdat.Description

                               }).OrderBy(x =>x.Description).ToList();


                int count = HSNList.Count();
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //Select by HSN Unique ID from Table for Edit at front end
        [HttpPost]
        public HttpResponseMessage GettHSNCodeByID(JObject json)
        {
            int HSNUniqueId = json["HsnTaxCodeID"].ToObject<int>();
            try
            {
                var HSNTaxDetails = (from HSNTx in db.TEPOHSNTaxCodeMappings
                                     join user in db.UserProfiles on HSNTx.LastModifiedBy equals user.UserId 
                                     into tempuser
                                     from prof in tempuser.DefaultIfEmpty()
                                     where HSNTx.IsDeleted == false && HSNTx.UniqueID == HSNUniqueId
                                     select new
                                     {
                                         HSNTx.ApplicableTo,
                                         HSNTx.DestinationCountry,
                                         HSNTx.VendorRegionCode,
                                         HSNTx.DeliveryPlantRegionCode,
                                         //HSNTx.GSTVendorClassification,
                                         HSNTx.HSNCode,
                                         HSNTx.MaterialGSTApplicability,
                                         HSNTx.VendorGSTApplicability,
                                         HSNTx.ValidFrom,
                                         HSNTx.ValidTo,
                                         HSNTx.TaxType,
                                         HSNTx.TaxCode,
                                         HSNTx.TaxRate,
                                         HSNTx.UniqueID,
                                         HSNTx.IsDeleted,
                                         prof.UserName,
                                         prof.CallName
                                     }).FirstOrDefault();
                if (HSNTaxDetails != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNTaxDetails, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = HSNTaxDetails, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //To Save the Record in Table
        [HttpPost]
        public HttpResponseMessage SaveHSNCodeDetails(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            try
            {
                HSNTaxCode.LastModifiedBy = GetLogInUserId();
                return HSNBAL.SavePOHSNRate(HSNTaxCode);
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //To Update the Record
        [HttpPost]
        public HttpResponseMessage UpdateHSNCodeDetails(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            try
            {
                HSNTaxCode.LastModifiedBy = GetLogInUserId();
                return HSNBAL.UpdatePOHSNRate(HSNTaxCode);
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //To Virtually Delete the Record
        [HttpPost]
        public HttpResponseMessage DeleteHSNCodeDetails(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            try
            {
                return HSNBAL.DeletePOHSNRate(HSNTaxCode);
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        private int GetLogInUserId()
        {
            var re = Request;
            var header = re.Headers;
            int authuser = 0;
            if (header.Contains("authUser"))
            {
                authuser = Convert.ToInt32(header.GetValues("authUser").First());
            }
            return authuser;
        }
    }
}
