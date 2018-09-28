using Newtonsoft.Json.Linq;
using PO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PO.Controllers
{
    public class PlantStorageDetailsAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        public PlantStorageDetailsAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [HttpPost]
        public HttpResponseMessage SavePlantStorageDetails(TEPOPlantStorageDetail plntstoragedtl)
        {
            try
            {
                TEProject proj = db.TEProjects.Where(a => a.ProjectID == plntstoragedtl.ProjectID && a.IsDeleted == false).FirstOrDefault();
                plntstoragedtl.LastModifiedOn = DateTime.Now;
                plntstoragedtl.isdeleted = false;
                if (proj != null)
                {
                    plntstoragedtl.ProjectName = proj.ProjectName;
                    plntstoragedtl.ProjectCode = proj.ProjectCode;
                }
                TEGSTNStateMaster state = db.TEGSTNStateMasters.Where(a => a.StateID == plntstoragedtl.StateID && a.IsDeleted == false).FirstOrDefault();
                if (state != null)
                {
                    plntstoragedtl.StateCode = state.StateCode;
                    plntstoragedtl.StateCodeDescription = state.StateName;
                }
                db.TEPOPlantStorageDetails.Add(plntstoragedtl);
                db.SaveChanges();

                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdatePlantStorageDetails(TEPOPlantStorageDetail plntstoragedtl)
        {
            try
            {
                TEPOPlantStorageDetail plntObj = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == plntstoragedtl.PlantStorageDetailsID && a.isdeleted == false).FirstOrDefault();
                if (plntObj != null)
                {
                    TEProject proj = db.TEProjects.Where(a => a.ProjectID == plntstoragedtl.ProjectID && a.IsDeleted == false).FirstOrDefault();
                    plntObj.LastModifiedOn = DateTime.Now;
                    plntObj.isdeleted = false;
                    if (proj != null)
                    {
                        plntObj.ProjectName = proj.ProjectName;
                        plntObj.ProjectCode = proj.ProjectCode;
                    }
                    TEGSTNStateMaster state = db.TEGSTNStateMasters.Where(a => a.StateID == plntstoragedtl.StateID && a.IsDeleted == false).FirstOrDefault();
                    if (state != null)
                    {
                        plntObj.StateCode = state.StateCode;
                        plntObj.StateCodeDescription = state.StateName;
                    }
                    plntObj.Address = plntstoragedtl.Address;
                    plntObj.CompanyCode = plntstoragedtl.CompanyCode;
                    plntObj.CountryCode = plntstoragedtl.CountryCode;
                    plntObj.GSTIN = plntstoragedtl.GSTIN;
                    plntObj.PlantStorageCode = plntstoragedtl.PlantStorageCode;
                    plntObj.Type = plntstoragedtl.Type;
                    plntObj.StateID = plntstoragedtl.StateID;
                    plntObj.ProjectID = plntstoragedtl.ProjectID;
                    db.Entry(plntObj).CurrentValues.SetValues(plntObj);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Updated";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Unable to Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage DeletePlantStorageDetails(TEPOPlantStorageDetail plntstoragedtl)
        {
            try
            {
                TEPOPlantStorageDetail plntObj = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == plntstoragedtl.PlantStorageDetailsID && a.isdeleted == false).FirstOrDefault();
                if (plntObj != null)
                {
                    plntObj.LastModifiedOn = DateTime.Now;
                    plntObj.isdeleted = true;
                    db.Entry(plntObj).CurrentValues.SetValues(plntObj);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Unable to Delete";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPlantStorageDetailsByID(JObject json)
        {
            int plantUniqueId = json["PlantStorageDetailsID"].ToObject<int>();
            try
            {
                var plntStorageDetails = (from plnt in db.TEPOPlantStorageDetails
                                          join user in db.UserProfiles on plnt.LastModifiedBy equals user.UserId into tempuser
                                          from prof in tempuser.DefaultIfEmpty()
                                          where plnt.isdeleted == false && plnt.PlantStorageDetailsID == plantUniqueId
                                          select new
                                          {
                                              plnt.Address,
                                              plnt.CompanyCode,
                                              plnt.CountryCode,
                                              plnt.GSTIN,
                                              plnt.LastModifiedBy,
                                              plnt.LastModifiedOn,
                                              plnt.PlantStorageCode,
                                              plnt.PlantStorageDetailsID,
                                              plnt.ProjectCode,
                                              plnt.ProjectID,
                                              plnt.ProjectName,
                                              plnt.StateCode,
                                              plnt.StateCodeDescription,
                                              plnt.StateID,
                                              plnt.StorageLocationCode,
                                              plnt.Type,
                                              prof.UserName,
                                              prof.CallName
                                          }).FirstOrDefault();
                if (plntStorageDetails != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = plntStorageDetails, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = plntStorageDetails, info = sinfo }) };
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

        [HttpPost]
        public HttpResponseMessage GetPlantStorageDetails_Pagination(JObject json)
        {
            int pagenumber = json["page_number"].ToObject<int>();
            int pagepercount = json["pagepercount"].ToObject<int>();
            int count = 0;
            try
            {
                var plntStorageList = (from plnt in db.TEPOPlantStorageDetails
                                       join user in db.UserProfiles on plnt.LastModifiedBy equals user.UserId into tempuser
                                       from prof in tempuser.DefaultIfEmpty()
                                       where plnt.isdeleted == false
                                       select new
                                       {
                                           plnt.Address,
                                           plnt.CompanyCode,
                                           plnt.CountryCode,
                                           plnt.GSTIN,
                                           plnt.LastModifiedBy,
                                           plnt.LastModifiedOn,
                                           plnt.PlantStorageCode,
                                           plnt.PlantStorageDetailsID,
                                           plnt.ProjectCode,
                                           plnt.ProjectID,
                                           plnt.ProjectName,
                                           plnt.StateCode,
                                           plnt.StateCodeDescription,
                                           plnt.StateID,
                                           plnt.StorageLocationCode,
                                           plnt.Type,
                                           prof.UserName,
                                           prof.CallName
                                       }).ToList();

                count = plntStorageList.Count;
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
                    var finalResult = plntStorageList.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = count;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = "1";

                    if (finalResult.Count > 0)
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
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = plntStorageList, info = sinfo }) };
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

        [HttpPost]
        public HttpResponseMessage GetStates(JObject json)
        {
            try
            {
                var stateList = (from state in db.TEGSTNStateMasters
                                 where state.IsDeleted == false
                                 select new
                                 {
                                     StateID = state.StateID,
                                     StateCode = state.StateCode,
                                     StateName = state.StateName,
                                     SAPCode = state.SAPCode,
                                     LastModifiedBy = state.LastModifiedBy,
                                     LastModifiedOn = state.LastModifiedOn
                                 }).OrderBy(x => x.StateName).ToList();
                int count = stateList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = stateList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = stateList, info = sinfo }) };
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

        [HttpPost]
        public HttpResponseMessage GetCurrencies(JObject json)
        {
            try
            {
                var currencyList = (from currency in db.TECurrencyMasters
                                    where currency.IsDeleted == false
                                    select new
                                    {
                                        UniqueId = currency.UniqueId,
                                        Country = currency.Country,
                                        Currency = currency.Currency,
                                        LastModifiedBy = currency.LastModifiedBy,
                                        LastModifiedOn = currency.LastModifiedOn
                                    }).ToList();
                int count = currencyList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = currencyList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = currencyList, info = sinfo }) };
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

        [HttpPost]
        public HttpResponseMessage GetCountries(JObject json)
        {
            try
            {
                var countryList = (from currency in db.TEPOCountryMasters
                                   where currency.IsDeleted == false
                                   select new
                                   {
                                       UniqueID = currency.UniqueID,
                                       CountryCode = currency.CountryCode,
                                       CountryPhoneCode = currency.CountryPhoneCode,
                                       Description = currency.Description,
                                       LastModifiedBy = currency.LastModifiedBy,
                                       LastModifiedOn = currency.LastModifiedOn
                                   }).OrderBy(x => x.Description).ToList();
                int count = countryList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = countryList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = countryList, info = sinfo }) };
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

    }
}
