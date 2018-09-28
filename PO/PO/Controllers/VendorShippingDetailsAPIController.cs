using Newtonsoft.Json.Linq;
using PO.BAL;
using PO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PO.Controllers
{
    public class VendorShippingDetailsAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        public VendorShippingDetailsAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        //[HttpPost]
        //public HttpResponseMessage SavePurchaseVendorDetails(Purchase_Vendor_Mapping_List vendorMapList)
        //{
        //    try
        //    {
        //        if (vendorMapList.Uniqueid > 0)
        //        {
        //            TEPOVendor vendorExist = db.TEPOVendors.Where(a => a.Uniqueid == vendorMapList.Uniqueid && a.IsDeleted == false).FirstOrDefault();

        //            if (vendorExist != null)
        //            {
        //                string loginUsername = string.Empty;
        //                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(vendorMapList.LastModifiedBy));
        //                vendorExist.LastModifiedBy = loginUsername;
        //                vendorExist.Address = vendorMapList.Address;
        //                vendorExist.CIN = vendorMapList.CIN;
        //                vendorExist.GLAccountID= vendorMapList.GLAccountID;
        //                vendorExist.Country = vendorMapList.Country;
        //                vendorExist.GSTApplicability= vendorMapList.GSTApplicability;
        //                vendorExist.LastModifiedOn = DateTime.Now.ToShortDateString();
        //                vendorExist.Currency = vendorMapList.Currency;
        //                vendorExist.GSTIN = vendorMapList.GSTIN;
        //                vendorExist.IsDeleted = false;
        //                vendorExist.IsActive = true;
        //                vendorExist.PanNumber = vendorMapList.PanNumber;
        //                vendorExist.RegionCode = vendorMapList.RegionCode;
        //                TEGSTNStateMaster statedata = GetStateInfo(vendorMapList.RegionCode);
        //                if (statedata != null)
        //                {
        //                    vendorExist.RegionCodeDesc = statedata.StateName;
        //                }
        //                vendorExist.ServiceTax = vendorMapList.ServiceTax;
        //                vendorExist.Status = "Draft";
        //                vendorExist.Vat = vendorMapList.Vat;
        //                vendorExist.Vendor_Code = vendorMapList.Vendor_Code;
        //                vendorExist.Vendor_Owner = vendorMapList.Vendor_Owner;
        //                db.Entry(vendorExist).CurrentValues.SetValues(vendorExist);
        //                db.SaveChanges();
        //            }

        //            for (int cnt = 0; cnt < vendorMapList.Address_Ship.Count; cnt++)
        //            {
        //                if (vendorMapList.Address_Ship[cnt] != "0")
        //                {
        //                    var allVendorShippingIds = db.TEPOVendorShippingDetails.Where(a => a.VendorID == vendorMapList.Uniqueid && a.IsDeleted == false).Select(a => a.VendorShippingID).ToList();
        //                    var nonintersectVendorShippingIds = vendorMapList.VendorShippingID.Except(allVendorShippingIds).Union(allVendorShippingIds.Except(vendorMapList.VendorShippingID));
        //                    //soft deleting non-selected shipping details
        //                    foreach (var deletevendorShipId in nonintersectVendorShippingIds)
        //                    {
        //                        TEPOVendorShippingDetail deletevendorShip = db.TEPOVendorShippingDetails.Where(a => a.VendorShippingID == deletevendorShipId).FirstOrDefault();
        //                        if (deletevendorShip != null)
        //                        {
        //                            deletevendorShip.IsDeleted = true;
        //                            deletevendorShip.LastModifiedBy = Convert.ToInt32(vendorMapList.LastModifiedBy);
        //                            deletevendorShip.LastModifiedOn = DateTime.Now;
        //                            db.Entry(deletevendorShip).CurrentValues.SetValues(deletevendorShip);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                    if (vendorMapList.VendorShippingID[cnt] > 0)
        //                    {
        //                        int tempshippingId = 0;
        //                        tempshippingId = vendorMapList.VendorShippingID[cnt];
        //                        TEPOVendorShippingDetail existvendorShip = db.TEPOVendorShippingDetails.Where(a => a.VendorShippingID == tempshippingId).FirstOrDefault();
        //                        if (existvendorShip != null)
        //                        {
        //                            existvendorShip.Address = vendorMapList.Address_Ship[cnt];
        //                            existvendorShip.CountryCode = vendorMapList.CountryCode_Ship[cnt];
        //                            existvendorShip.GSTIN = vendorMapList.GSTIN_Ship[cnt];
        //                            existvendorShip.VendorID = vendorMapList.Uniqueid;
        //                            existvendorShip.StateCode = vendorMapList.StateCode_Ship[cnt];
        //                            TEGSTNStateMaster statedata = GetStateInfo(vendorMapList.StateCode_Ship[cnt]);
        //                            if (statedata != null)
        //                            {
        //                                existvendorShip.StateCodeDescription = statedata.StateName;
        //                            }
        //                            existvendorShip.LastModifiedBy = Convert.ToInt32(vendorMapList.LastModifiedBy);
        //                            existvendorShip.LastModifiedOn = DateTime.Now;
        //                            db.Entry(existvendorShip).CurrentValues.SetValues(existvendorShip);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TEPOVendorShippingDetail vendorShip = new TEPOVendorShippingDetail();
        //                        vendorShip.Address = vendorMapList.Address_Ship[cnt];
        //                        vendorShip.CountryCode = vendorMapList.CountryCode_Ship[cnt];
        //                        vendorShip.GSTIN = vendorMapList.GSTIN_Ship[cnt];
        //                        vendorShip.VendorID = vendorMapList.Uniqueid;
        //                        vendorShip.IsDeleted = false;
        //                        vendorShip.StateCode = vendorMapList.StateCode_Ship[cnt];
        //                        TEGSTNStateMaster statedata = GetStateInfo(vendorMapList.StateCode_Ship[cnt]);
        //                        if (statedata != null)
        //                        {
        //                            vendorShip.StateCodeDescription = statedata.StateName;
        //                        }
        //                        vendorShip.LastModifiedBy = Convert.ToInt32(vendorMapList.LastModifiedBy);
        //                        vendorShip.LastModifiedOn = DateTime.Now;
        //                        db.TEPOVendorShippingDetails.Add(vendorShip);
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Updated";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            string loginUsername = string.Empty;
        //            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(vendorMapList.LastModifiedBy));
        //            TEPOVendor vendor = new TEPOVendor();
        //            vendor.Address = vendorMapList.Address;
        //            vendor.CIN = vendorMapList.CIN;
        //            vendor.GLAccountID = vendorMapList.GLAccountID;
        //            vendor.Country = vendorMapList.Country;
        //            vendor.GSTApplicability = vendorMapList.GSTApplicability;
        //            vendor.CreatedBy = loginUsername;
        //            vendor.CreatedOn = DateTime.Now.ToShortDateString();
        //            vendor.LastModifiedBy = loginUsername;
        //            vendor.LastModifiedOn = DateTime.Now.ToShortDateString();
        //            vendor.Currency = vendorMapList.Currency;
        //            vendor.GSTIN = vendorMapList.GSTIN;
        //            vendor.IsDeleted = false;
        //            vendor.IsActive = true;
        //            vendor.PanNumber = vendorMapList.PanNumber;
        //            vendor.RegionCode = vendorMapList.RegionCode;
        //            TEGSTNStateMaster statedata = GetStateInfo(vendorMapList.RegionCode);
        //            if (statedata != null)
        //            {
        //                vendor.RegionCodeDesc = statedata.StateName;
        //            }
        //            vendor.ServiceTax = vendorMapList.ServiceTax;
        //            vendor.Status = "Draft";
        //            vendor.Vat = vendorMapList.Vat;
        //            vendor.Vendor_Code = vendorMapList.Vendor_Code;
        //            vendor.Vendor_Owner = vendorMapList.Vendor_Owner;
        //            db.TEPOVendors.Add(vendor);
        //            db.SaveChanges();

        //            int resuniqueId = vendor.Uniqueid;
        //            if (resuniqueId > 0)
        //            {
        //                for (int cnt = 0; cnt < vendorMapList.Address_Ship.Count; cnt++)
        //                {
        //                    if (vendorMapList.Address_Ship[cnt] != "0")
        //                    {
        //                        TEPOVendorShippingDetail vendorShip = new TEPOVendorShippingDetail();
        //                        vendorShip.Address = vendorMapList.Address_Ship[cnt];
        //                        vendorShip.CountryCode = vendorMapList.CountryCode_Ship[cnt];
        //                        vendorShip.GSTIN = vendorMapList.GSTIN_Ship[cnt];
        //                        vendorShip.VendorID = resuniqueId;
        //                        vendorShip.IsDeleted = false;
        //                        vendorShip.StateCode = vendorMapList.StateCode_Ship[cnt];
        //                        TEGSTNStateMaster statedata_ship = GetStateInfo(vendorMapList.StateCode_Ship[cnt]);
        //                        if (statedata != null)
        //                        {
        //                            vendorShip.StateCodeDescription = statedata_ship.StateName;
        //                        }
        //                        vendorShip.LastModifiedBy = Convert.ToInt32(vendorMapList.LastModifiedBy);
        //                        vendorShip.LastModifiedOn = DateTime.Now;
        //                        db.TEPOVendorShippingDetails.Add(vendorShip);
        //                        db.SaveChanges();
        //                    }
        //                }
        //            }
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Saved";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail To Save";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}
        //[HttpPost]
        //public HttpResponseMessage DeletePurchaseVendor(TEPOVendor vendor)
        //{
        //    try
        //    {
        //        TEPOVendor vendorExist = db.TEPOVendors.Where(a => a.Uniqueid == vendor.Uniqueid && a.IsDeleted == false).FirstOrDefault();
        //        if (vendorExist != null)
        //        {
        //            string loginUsername = string.Empty;
        //            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(vendor.LastModifiedBy));
        //            vendorExist.LastModifiedBy = loginUsername;
        //            vendorExist.LastModifiedOn = DateTime.Now.ToShortDateString();
        //            vendorExist.IsDeleted = true;
        //            db.Entry(vendorExist).CurrentValues.SetValues(vendorExist);
        //            db.SaveChanges();
        //            var vendorchildList = db.TEPOVendorShippingDetails.Where(a => a.VendorID == vendor.Uniqueid && a.IsDeleted == false).ToList();
        //            foreach (var shipdata in vendorchildList)
        //            {
        //                TEPOVendorShippingDetail existvendorShip = db.TEPOVendorShippingDetails.Where(a => a.VendorShippingID == vendor.Uniqueid).FirstOrDefault();
        //                if (existvendorShip != null)
        //                {
        //                    existvendorShip.LastModifiedBy = Convert.ToInt32(vendor.LastModifiedBy);
        //                    existvendorShip.LastModifiedOn = DateTime.Now;
        //                    existvendorShip.IsDeleted = true;
        //                    db.Entry(existvendorShip).CurrentValues.SetValues(existvendorShip);
        //                    db.SaveChanges();
        //                }
        //            }
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Deleted";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Unable to Delete";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail To Delete";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}
        //[HttpPost]
        //public HttpResponseMessage DeletePurchaseShippingDetail(TEPOVendorShippingDetail shippingDetail)
        //{
        //    try
        //    {
        //        TEPOVendorShippingDetail existvendorShip = db.TEPOVendorShippingDetails.Where(a => a.VendorShippingID == shippingDetail.VendorShippingID).FirstOrDefault();
        //        if (existvendorShip != null)
        //        {
        //            existvendorShip.LastModifiedBy = shippingDetail.LastModifiedBy;
        //            existvendorShip.LastModifiedOn = DateTime.Now;
        //            existvendorShip.IsDeleted = true;
        //            db.Entry(existvendorShip).CurrentValues.SetValues(existvendorShip);
        //            db.SaveChanges();

        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Deleted";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Unable to Delete";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail To Delete";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage GetVendorDetailsByID(JObject json)
        //{
        //    Purchase_VendorDTO vendorDetails = new Purchase_VendorDTO();
        //    try
        //    {
        //        int vendoruniqueId = json["Uniqueid"].ToObject<int>();
        //        vendorDetails = (from vndr in db.TEPOVendors
        //                         join ship in db.TEPOVendorShippingDetails on vndr.Uniqueid equals ship.VendorID
        //                         join gl in db.TEPOGLCodeMasters on vndr.GLAccountID equals gl.UniqueID into tempgl
        //                         from gldata in tempgl.DefaultIfEmpty()
        //                         where vndr.IsDeleted == false && vndr.IsActive == true && vndr.Uniqueid== vendoruniqueId
        //                         select new Purchase_VendorDTO
        //                         {
        //                             Address = vndr.Address,
        //                             Country = vndr.Country,
        //                             CIN = vndr.CIN,
        //                             GSTApplicability = vndr.GSTApplicability,
        //                             Currency = vndr.Currency,
        //                             GSTIN = vndr.GSTIN,
        //                             IsActive = vndr.IsActive,
        //                             Vendor_Owner = vndr.Vendor_Owner,
        //                             Vendor_Code = vndr.Vendor_Code,
        //                             Vat = vndr.Vat,
        //                             PanNumber = vndr.PanNumber,
        //                             Status = vndr.Status,
        //                             RegionCode = vndr.RegionCode,
        //                             RegionCodeDesc = vndr.RegionCodeDesc,
        //                             Uniqueid = vndr.Uniqueid,
        //                             ServiceTax = vndr.ServiceTax,
        //                             GLAccountID= vndr.GLAccountID,
        //                             GLAccountCode=gldata.GLAccountCode,
        //                             GLAccountShortName = gldata.GLAccountShortName,
        //                             LastModifiedBy = vndr.LastModifiedBy,
        //                             LastModifiedOn = vndr.LastModifiedOn
        //                         }).FirstOrDefault();

        //        if (vendorDetails != null)
        //        {
        //            vendorDetails.VendorShippingDetail = db.TEPOVendorShippingDetails.Where(a => a.VendorID == vendoruniqueId && a.IsDeleted == false).ToList();
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Success";
        //            sinfo.fromrecords = 1;
        //            sinfo.torecords = 1;
        //            sinfo.totalrecords = 1;
        //            sinfo.listcount = 1;
        //            sinfo.pages = "1";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetails, info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "No Records";
        //            sinfo.listcount = 0;
        //            return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetails, info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail";
        //        sinfo.listcount = 0;
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage GetVendorDetails_Pagination(JObject json)
        //{
        //    List<Purchase_VendorDTO> vendorList = new List<Purchase_VendorDTO>();
        //    List<Purchase_VendorDTO> finalvendorList = new List<Purchase_VendorDTO>();
        //    int pagenumber = json["page_number"].ToObject<int>();
        //    int pagepercount = json["pagepercount"].ToObject<int>();
        //    int count = 0;
        //    try
        //    {
        //        vendorList = (from vndr in db.TEPOVendors
        //                      join gl in db.TEPOGLCodeMasters on vndr.GLAccountID equals gl.UniqueID into tempgl
        //                      from gldata in tempgl.DefaultIfEmpty()
        //                      where vndr.IsDeleted == false && vndr.IsActive == true
        //                      select new Purchase_VendorDTO
        //                      {
        //                          Address = vndr.Address,
        //                          Country = vndr.Country,
        //                          CIN = vndr.CIN,
        //                          Currency = vndr.Currency,
        //                          GSTIN = vndr.GSTIN,
        //                          GSTApplicability=vndr.GSTApplicability,
        //                          GLAccountID = vndr.GLAccountID,
        //                          GLAccountCode = gldata.GLAccountCode,
        //                          GLAccountShortName = gldata.GLAccountShortName,
        //                          //IsActive =vndr.IsActive,
        //                          Vendor_Owner = vndr.Vendor_Owner,
        //                          Vendor_Code = vndr.Vendor_Code,
        //                          Vat = vndr.Vat,
        //                          PanNumber = vndr.PanNumber,
        //                          Status = vndr.Status,
        //                          RegionCode = vndr.RegionCode,
        //                          RegionCodeDesc = vndr.RegionCodeDesc,
        //                          Uniqueid = vndr.Uniqueid,
        //                          ServiceTax = vndr.ServiceTax,
        //                          LastModifiedBy = vndr.LastModifiedBy,
        //                          LastModifiedOn = vndr.LastModifiedOn
        //                      }).ToList();
        //        count = vendorList.Count;
        //        if (count > 0)
        //        {
        //            if (pagenumber == 0)
        //            {
        //                pagenumber = 1;
        //            }
        //            int iPageNum = pagenumber;
        //            int iPageSize = pagepercount;
        //            int start = iPageNum - 1;
        //            start = start * iPageSize;
        //            finalvendorList = vendorList.Skip(start).Take(iPageSize).ToList();
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Success";
        //            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
        //            sinfo.torecords = finalvendorList.Count + start;
        //            sinfo.totalrecords = count;
        //            sinfo.listcount = finalvendorList.Count;
        //            sinfo.pages = "1";
        //            if (finalvendorList.Count > 0)
        //            {
        //                foreach (var shipdata in finalvendorList)
        //                {
        //                    shipdata.VendorShippingDetail = db.TEPOVendorShippingDetails.Where(a => a.VendorID == shipdata.Uniqueid && a.IsDeleted == false).ToList();
        //                }
        //                sinfo.errorcode = 0;
        //                sinfo.errormessage = "Successfully";
        //                return new HttpResponseMessage() { Content = new JsonContent(new { result = finalvendorList, info = sinfo }) };
        //            }
        //            else
        //            {
        //                sinfo.errorcode = 0;
        //                sinfo.errormessage = "No Records";
        //                sinfo.listcount = 0;
        //                return new HttpResponseMessage() { Content = new JsonContent(new { result = finalvendorList, info = sinfo }) };
        //            }
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "No Records";
        //            sinfo.listcount = 0;
        //            return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail";
        //        sinfo.listcount = 0;
        //        return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
        //    }
        //}
        public TEGSTNStateMaster GetStateInfo(string statecode)
        {
            TEGSTNStateMaster stateDetails = new TEGSTNStateMaster();
            try
            {
                stateDetails = db.TEGSTNStateMasters.Where(a => a.IsDeleted == false && a.StateCode == statecode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
            return stateDetails;
        }
        [HttpPost]
        public HttpResponseMessage GetGLAccountDetailsForVendor(JObject json)
        {
            try
            {
                var glAccountDetails = (from plnt in db.TEPOGLCodeMasters
                                        where plnt.IsDeleted == false && plnt.ReconAccntType.ToLower() == "k"
                                        select new
                                        {
                                            plnt.CommitmentItemCode,
                                            plnt.CommitmentItemDesc,
                                            plnt.GLAccountCode,
                                            plnt.GLAccountDesc,
                                            plnt.GLAccountShortName,
                                            plnt.Recon,
                                            plnt.UniqueID,
                                        }).ToList();
                int count = glAccountDetails.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = glAccountDetails, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = glAccountDetails, info = sinfo }) };
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
        public HttpResponseMessage SaveVendor(VendorDTO vendorDto)
        {
            int resultphsUniqueid = 0, loginId = 0;
            try
            {
                loginId = GetLogInUserId();
                resultphsUniqueid = new PurchaseOrderBAL().SaveVendor(vendorDto, loginId);
                if (resultphsUniqueid > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveVendorMaster(VendorMasterDto vendorDto)
        {
            int resultphsUniqueid = 0, loginId = 0;
            try
            {
                loginId = GetLogInUserId();
                SuccessInfo Valid = new SuccessInfo();
                Valid = SaveVendorValidationCheck(vendorDto);
                if (Valid.errorcode == 0) resultphsUniqueid = new PurchaseOrderBAL().SaveVendorMaster(vendorDto, loginId);
                else
                {
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };

                }
                if (resultphsUniqueid > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Unable to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateVendor(VendorDTO vendorDto)
        {
            int loginId = 0; bool result;
            try
            {
                loginId = GetLogInUserId();
                result = new PurchaseOrderBAL().UpdateVendor(vendorDto, loginId);
                if (result)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateVendorMaster(VendorMasterDto vendorDto)
        {
            int loginId = 0; bool result;
            try
            {
                loginId = GetLogInUserId();
                SuccessInfo Valid = new SuccessInfo();
                Valid = UpdateVendorValidationCheck(vendorDto);

                if (Valid.errorcode == 0) result = new PurchaseOrderBAL().UpdateVendorMaster(vendorDto, loginId);
                else
                {
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = Valid }) };

                }

                if (result)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
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
        public HttpResponseMessage DeletePurchaseVendor(TEPOVendorMaster vendor)
        {
            try
            {
                TEPOVendorMaster vendorExist = db.TEPOVendorMasters.Where(a => a.POVendorMasterId == vendor.POVendorMasterId && a.IsDeleted == false).FirstOrDefault();
                if (vendorExist != null)
                {
                    var vendorSapDetails = (from vendrDtl in db.TEPOVendorMasterDetails
                                            where vendrDtl.POVendorMasterId == vendor.POVendorMasterId && vendrDtl.VendorCode != null && vendrDtl.VendorCode != ""
                                            select new { vendrDtl.VendorCode }).FirstOrDefault();
                    if (vendorSapDetails != null)
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Vendor registered in SAP cannot delete!";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    string loginUsername = string.Empty;
                    vendorExist.LastModifiedOn = DateTime.Now;
                    vendorExist.IsDeleted = true;
                    db.Entry(vendorExist).CurrentValues.SetValues(vendorExist);
                    db.SaveChanges();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
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
        public HttpResponseMessage GetAllVendorMaster(JObject json)
        {
            int pagenumber = json["page_number"].ToObject<int>();
            int pagepercount = json["pagepercount"].ToObject<int>();
            int count = 0;
            try
            {
                var vendorList = (from vndr in db.TEPOVendorMasters
                                  join vndrDtl in db.TEPOVendorMasterDetails on vndr.POVendorMasterId equals vndrDtl.POVendorMasterId into tempvndrdtl
                                  from VdrdtlTbl in tempvndrdtl.DefaultIfEmpty()
                                  join region in db.TEGSTNStateMasters on VdrdtlTbl.RegionId equals region.StateID into regiontem
                                  from regDet in regiontem.DefaultIfEmpty()
                                  where vndr.IsDeleted == false && VdrdtlTbl.IsDeleted == false
                                  select new
                                  {
                                      vndr.POVendorMasterId,
                                      VdrdtlTbl.VendorCode,
                                      vndr.VendorContactId,
                                      vndr.VendorName,
                                      vndr.Currency,
                                      vndr.PAN,
                                      vndr.CIN,
                                      vndr.ServiceTax,
                                      vndr.IsActive,
                                      vndr.LastModifiedBy,
                                      vndr.LastModifiedOn,
                                      VdrdtlTbl.GSTIN,
                                      VdrdtlTbl.RepresentName,
                                      regDet.StateCode,
                                      regDet.StateName
                                  }).OrderBy(m => m.VendorName).ToList();
                //group new {vndr, VdrdtlTbl, regDet } by new { vndr.VendorName, vndr.PAN , regDet.StateCode, regDet.StateName,
                //    VdrdtlTbl.VendorCode, vndr.ServiceTax, VdrdtlTbl.GSTIN, vndr.Currency, VdrdtlTbl.IsActive, vndr.POVendorMasterId, VdrdtlTbl.RepresentName, vndr.VendorContactId, vndr.CIN,
                //} into temp

                count = vendorList.Count;
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
                    var finalvendorList = vendorList.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalvendorList.Count + start;
                    sinfo.totalrecords = count;
                    sinfo.listcount = finalvendorList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = finalvendorList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
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
        public HttpResponseMessage GetVendorMasterDetailByMasterId(JObject json)
        {
            try
            {
                int VendorMasterId = 0;
                VendorMasterId = json["VendorMasterId"].ToObject<int>();
                var masterdata = (from vndr in db.TEPOVendorMasters
                                  where vndr.IsDeleted == false && vndr.POVendorMasterId == VendorMasterId
                                  select new
                                  {
                                      vndr.CIN,
                                      vndr.POVendorMasterId,
                                      vndr.Currency,
                                      vndr.PAN,
                                      vndr.ServiceTax,
                                      vndr.VendorContactId,
                                      vndr.VendorName,
                                      vndr.IsActive,
                                      vndr.LastModifiedBy,
                                      vndr.LastModifiedOn,
                                  }).FirstOrDefault();
                var vendorDetailList = (from vendrDtl in db.TEPOVendorMasterDetails
                                        join region in db.TEGSTNStateMasters on vendrDtl.RegionId equals region.StateID into tempregion
                                        from state in tempregion.DefaultIfEmpty()
                                        join country in db.TEPOCountryMasters on vendrDtl.CountryId equals country.UniqueID into tempcntry
                                        from cntry in tempcntry.DefaultIfEmpty()
                                        join gstapl in db.TEPOGSTApplicabilityMasters on vendrDtl.GSTApplicabilityId equals gstapl.UniqueID into tempgstapl
                                        from finalgstapl in tempgstapl.DefaultIfEmpty()
                                        join vendAccntGrp in db.TEPOVendorAccountGroupMasters on vendrDtl.VendorAccountGroupId equals vendAccntGrp.UniqueID into tempvendAccntGrp
                                        from finalvendAccntGrp in tempvendAccntGrp.DefaultIfEmpty()
                                        join vendCatg in db.TEPOVendorCategoryMasters on vendrDtl.VendorCategoryId equals vendCatg.UniqueID into tempvendCatg
                                        from finalvendCatg in tempvendCatg.DefaultIfEmpty()
                                        join schema in db.TEPOVendorSchemaGroups on vendrDtl.ScehmaGroupId equals schema.SchemaGroupId into tempschema
                                        from finalschema in tempschema.DefaultIfEmpty()
                                        join glAccnt in db.TEPOGLCodeMasters on vendrDtl.GLAccountId equals glAccnt.UniqueID into tempglAccnt
                                        from finalglAccnt in tempglAccnt.DefaultIfEmpty()
                                        join withHldTaxType in db.TEPOWithholdingTaxMasters on vendrDtl.WithholdTaxTypeId equals withHldTaxType.UniqueID into tempWTHtax
                                        from finaltempWTHtax in tempWTHtax.DefaultIfEmpty()
                                        join withHldTaxCode in db.TEPOWithholdingTaxMasters on vendrDtl.WithholdTaxCodeId equals withHldTaxCode.UniqueID into tempWTHCode
                                        from finaltempWTHCode in tempcntry.DefaultIfEmpty()
                                        where vendrDtl.IsDeleted == false && vendrDtl.POVendorMasterId == VendorMasterId
                                        select new
                                        {
                                            POVendorDetailId = vendrDtl.POVendorDetailId,
                                            VendorCode = vendrDtl.VendorCode,
                                            BillingAddress = vendrDtl.BillingAddress,
                                            BillingCity = vendrDtl.BillingCity,
                                            BillingPostalCode = vendrDtl.BillingPostalCode,
                                            ShippingAddress = vendrDtl.ShippingAddress,
                                            ShippingCity = vendrDtl.ShippingCity,
                                            ShippingPostalCode = vendrDtl.ShippingPostalCode,
                                            RegionId = vendrDtl.RegionId,
                                            Region = state.StateName,
                                            CountryId = vendrDtl.CountryId,
                                            Country = cntry.Description,
                                            GSTApplicabilityId = vendrDtl.GSTApplicabilityId,
                                            GSTApplicability = finalgstapl.Description,
                                            GSTIN = vendrDtl.GSTIN,
                                            VendorAccountGroupId = vendrDtl.VendorAccountGroupId,
                                            VendorAccountGroup = finalvendAccntGrp.Description,
                                            VendorCategoryId = vendrDtl.VendorCategoryId,
                                            VendorCategory = finalvendCatg.Description,
                                            SchemaGroupId = vendrDtl.ScehmaGroupId,
                                            SchemaGroup = finalschema.SchemaDescription,
                                            GLAccountId = vendrDtl.GLAccountId,
                                            GLAccount = finalglAccnt.GLAccountShortName,
                                            WithholdTaxTypeId = vendrDtl.WithholdTaxTypeId,
                                            WithholdTaxType = finaltempWTHtax.Description,
                                            WithholdTaxCodeId = vendrDtl.WithholdTaxCodeId,
                                            WithholdTaxCode = finaltempWTHCode.Description,
                                            WithholdApplicability = vendrDtl.WithholdApplicability,
                                            BankAccountName = vendrDtl.BankAccountName,
                                            BankAccountNumber = vendrDtl.BankAccountNumber,
                                            BankName = vendrDtl.BankName,
                                            IFSCCode = vendrDtl.IFSCCode,
                                            RepresentName = vendrDtl.RepresentName,
                                            Designation = vendrDtl.Designation,
                                            RepresentContactNumber = vendrDtl.ContactNumber,
                                            RepresentEmailID = vendrDtl.EmailID,
                                            CancelledChequeRef = vendrDtl.CancelledChequeRef,
                                            RepresentContactId = vendrDtl.RepresentContactId,
                                            GSTNRegnCertificateRef = vendrDtl.GSTNRegnCertificateRef,
                                            IncorporationCertificateRef = vendrDtl.IncorporationCertificateRef
                                        }).ToList();

                if (vendorDetailList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetailList, POVendorMasterData = masterdata, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetailList, POVendorMasterData = masterdata, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorAccountingGroup()
        {
            try
            {
                var AccountGroupList = (from accnt in db.TEPOVendorAccountGroupMasters
                                        where accnt.IsDeleted == false
                                        select new
                                        {
                                            accnt.UniqueID,
                                            accnt.VendorAccountGroupCode,
                                            accnt.Description
                                        }).ToList();
                if (AccountGroupList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = AccountGroupList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = AccountGroupList, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorCategory()
        {
            try
            {
                var CategoryList = (from catg in db.TEPOVendorCategoryMasters
                                    where catg.IsDeleted == false
                                    select new
                                    {
                                        catg.UniqueID,
                                        catg.TEVendorCategoryCode,
                                        catg.Description
                                    }).OrderBy(x => x.Description).ToList();
                if (CategoryList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = CategoryList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = CategoryList, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorSchemaGroup()
        {
            try
            {
                var SchemaList = (from schm in db.TEPOVendorSchemaGroups
                                  where schm.IsDeleted == false
                                  select new
                                  {
                                      schm.SchemaGroupId,
                                      schm.SchemaGroupCode,
                                      schm.SchemaDescription
                                  }).ToList();
                if (SchemaList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = SchemaList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = SchemaList, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorGLAccountCode()
        {
            try
            {
                var GLAccountList = (from gl in db.TEPOGLCodeMasters
                                     where gl.IsDeleted == false && gl.ReconAccntType == "K"
                                     select new
                                     {
                                         gl.UniqueID,
                                         gl.GLAccountCode,
                                         gl.GLAccountShortName,
                                         gl.GLAccountDesc
                                     }).ToList();
                if (GLAccountList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GLAccountList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GLAccountList, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorWithholdingTaxType()
        {
            try
            {
                var WithHoldTaxList = (from tax in db.TEPOWithholdingTaxMasters
                                       where tax.IsDeleted == false
                                       select new
                                       {
                                           tax.UniqueID,
                                           tax.WithholdingTaxCode,
                                           tax.Description
                                       }).ToList();
                if (WithHoldTaxList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = WithHoldTaxList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = WithHoldTaxList, info = sinfo }) };
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
        public HttpResponseMessage GetAllGSTApplicability()
        {
            try
            {
                var GSTApplicabilityList = (from gst in db.TEPOGSTApplicabilityMasters
                                            join gstmap in db.TEPOGSTApplicabilityMappings on gst.UniqueID equals gstmap.GSTApplicabilityMasterId
                                            where gst.IsDeleted == false && gstmap.ApplicableTo == "Vendor"
                                            select new
                                            {
                                                gst.UniqueID,
                                                gst.GSTApplicabilityCode,
                                                gst.Description
                                            }).ToList();
                if (GSTApplicabilityList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GSTApplicabilityList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GSTApplicabilityList, info = sinfo }) };
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
        public HttpResponseMessage GetAllVendorsForSearch()
        {
            try
            {
                var vendorList = (from vndrdtl in db.TEPOVendorMasterDetails
                                  join vndr in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vndr.POVendorMasterId
                                  join region in db.TEGSTNStateMasters on vndrdtl.RegionId equals region.StateID
                                  join country in db.TEPOCountryMasters on vndrdtl.CountryId equals country.UniqueID
                                  join glaccnt in db.TEPOGLCodeMasters on vndrdtl.GLAccountId equals glaccnt.UniqueID
                                  where vndr.IsDeleted == false && vndrdtl.IsDeleted == false
                                  select new
                                  {
                                      vndrdtl.POVendorDetailId,
                                      vndr.POVendorMasterId,
                                      vndrdtl.VendorCode,
                                      vndr.VendorName,
                                      region.StateCode,
                                      region.StateName,
                                      vndrdtl.BillingAddress,
                                      vndrdtl.BillingCity,
                                      Country = country.Description,
                                      vndr.Currency,
                                      glaccnt.GLAccountCode,
                                      vndrdtl.GSTIN,
                                      vndr.PAN,
                                      vndr.ServiceTax
                                  }).OrderBy(a => a.VendorCode).ToList();
                if (vendorList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = vendorList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetVendorByDetailId(JObject json)
        {
            int id = 0;
            id = json["VendorDetailID"].ToObject<int>();
            try
            {
                var vendorDetail = (from vndrdtl in db.TEPOVendorMasterDetails
                                    join vndr in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vndr.POVendorMasterId
                                    join region in db.TEGSTNStateMasters on vndrdtl.RegionId equals region.StateID
                                    join country in db.TEPOCountryMasters on vndrdtl.CountryId equals country.UniqueID
                                    join glaccnt in db.TEPOGLCodeMasters on vndrdtl.GLAccountId equals glaccnt.UniqueID
                                    where vndr.IsDeleted == false && vndrdtl.IsDeleted == false && vndrdtl.POVendorDetailId == id
                                    select new
                                    {
                                        vndrdtl.POVendorDetailId,
                                        vndr.POVendorMasterId,
                                        vndrdtl.VendorCode,
                                        vndr.VendorName,
                                        region.StateCode,
                                        region.StateName,
                                        vndrdtl.BillingAddress,
                                        vndrdtl.BillingCity,
                                        vndrdtl.BillingPostalCode,
                                        vndrdtl.ShippingAddress,
                                        vndrdtl.ShippingCity,
                                        vndrdtl.ShippingPostalCode,
                                        Country = country.Description,
                                        vndr.Currency,
                                        glaccnt.GLAccountCode,
                                        vndrdtl.GSTIN,
                                        vndr.PAN,
                                        vndr.ServiceTax
                                    }).OrderBy(a => a.VendorCode).FirstOrDefault();
                if (vendorDetail != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetail, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorDetail, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
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

        #region  Representative
        [HttpPost]
        public HttpResponseMessage TEContactGetAllData_Representative(JObject TEData)
        {
            int authuser = 0;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("authUser")) { authuser = Convert.ToInt32(headers.GetValues("authUser").First()); }
            return ContactDetailsWithFilterForRepresentative(TEData, authuser);
        }
        public HttpResponseMessage ContactDetailsWithFilterForRepresentative(JObject TEData, int authuser)
        {
            HttpResponseMessage FinalResult = new HttpResponseMessage();
            List<ContactListDetails> finalData = new List<ContactListDetails>();
            finalData = ContactDetailsGetRepresentative(TEData, authuser);

            int PageNumber = 1;
            if (TEData["PageNumber"] != null && TEData["PageNumber"].ToString() != "") PageNumber = TEData["PageNumber"].ToObject<int>();
            int PageperCount = TEData["PageperCount"].ToObject<int>();
            int FromRecords = ((PageNumber - 1) * PageperCount) + 1;
            int ToRecords = (PageNumber * PageperCount);
            sinfo.fromrecords = FromRecords;
            sinfo.torecords = ToRecords;
            sinfo.totalrecords = ContactDetailsFullCountRE(TEData, authuser);
            sinfo.listcount = PageperCount;
            sinfo.pages = PageNumber.ToString();
            if (sinfo.torecords > sinfo.totalrecords) { sinfo.torecords = sinfo.totalrecords; sinfo.listcount = sinfo.totalrecords; }
            if (PageperCount == 0) { sinfo.torecords = sinfo.totalrecords; sinfo.listcount = sinfo.totalrecords; }
            if (finalData != null)
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successful";
                FinalResult.Content = new JsonContent(new { result = finalData, info = sinfo });
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "No Records to Show";
                FinalResult.Content = new JsonContent(new { result = "", info = sinfo });
            }
            return FinalResult;
        }
        public int ContactDetailsFullCountRE(JObject TEData, int authuser)
        {
            int ContactList = 0;

            var contlistRE = db.TEContacts.Where(x => x.ContactType.Equals("VendorRepresentative") && x.IsDeleted == false).ToList();
            ContactList = contlistRE.Count;
            return ContactList;
        }
        public List<ContactListDetails> ContactDetailsGetRepresentative(JObject TEData, int authuser)
        {

            List<ContactListDetails> ContactList = new List<ContactListDetails>();
            string ContactName = TEData["ContactName"].ToObject<string>();
            string ContactMobile = TEData["ContactMobile"].ToObject<string>();
            string ContactEmail = TEData["ContactEmail"].ToObject<string>();
            int? PageNumber = TEData["PageNumber"].ToObject<int>();
            int? PageperCount = TEData["PageperCount"].ToObject<int>();
            if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
            if (TEData["PageperCount"].ToObject<int>() == 0)
            {
                PageperCount = ContactDetailsFullCountVendor(TEData, authuser);
            }
            int? FromRecords = (PageNumber - 1) * PageperCount;
            int? ToRecords = (PageNumber * PageperCount) + 1;
            //context.Configuration.ProxyCreationEnabled = false;
            //List<>
            string cnString = "";
            string cnStringBegin = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY TC.uniqueid) as RowNum, ";
            cnStringBegin += " TC.Uniqueid, TC.CallName, TCM.Mobile, TCE.Emailid ";
            cnStringBegin += "FROM dbo.tecontact AS TC ";
            if (ContactEmail != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.Emailid like '%" + ContactEmail + "%') TCE ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCE ";
            }

            if (ContactMobile != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.mobile like '%" + ContactMobile + "%') TCM ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCM ";
            }
            cnStringBegin += "WHERE ";
            if (ContactEmail != "")
            {
                cnStringBegin += "TCE.emailid like '%" + ContactEmail + "%' and ";

            }
            if (ContactMobile != "")
            {
                cnStringBegin += "TCM.mobile like '%" + ContactMobile + "%' and ";

            }
            if (ContactName != "")
            {
                cnStringBegin += "TC.CallName like '%" + ContactName + "%' and ";

            }
            cnStringBegin += "TC.Status='active' and TC.ContactType = 'VendorRepresentative' and TC.IsDeleted = 0)";
            cnString = cnStringBegin + "AS T WHERE RowNum > " + FromRecords + " and RowNum < " + ToRecords + "";

            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ContactList.Add(new ContactListDetails
                        {
                            UniqueID = (dr["uniqueid"] != DBNull.Value) ? Convert.ToInt32(dr["uniqueid"].ToString()) : 0,
                            ContactName = dr["callname"].ToString(),
                            ContactMobile = dr["Mobile"].ToString(),
                            ContactEmail = dr["Emailid"].ToString(),
                        });
                    }
                }
                dr.Close();
                cn.Close();
            }
            return ContactList;
        }
        #endregion

        [HttpPost]
        public HttpResponseMessage TEContactGetAllData_Vendor(JObject TEData)
        {
            int authuser = 0;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("authUser")) { authuser = Convert.ToInt32(headers.GetValues("authUser").First()); }
            return ContactDetailsWithFilterForVendor(TEData, authuser);
        }
        public HttpResponseMessage ContactDetailsWithFilterForVendor(JObject TEData, int authuser)
        {
            HttpResponseMessage FinalResult = new HttpResponseMessage();
            List<ContactListDetails> finalData = new List<ContactListDetails>();
            finalData = ContactDetailsGetVendor(TEData, authuser);

            int PageNumber = 1;
            if (TEData["PageNumber"] != null && TEData["PageNumber"].ToString() != "") PageNumber = TEData["PageNumber"].ToObject<int>();
            int PageperCount = TEData["PageperCount"].ToObject<int>();
            int FromRecords = ((PageNumber - 1) * PageperCount) + 1;
            int ToRecords = (PageNumber * PageperCount);
            sinfo.fromrecords = FromRecords;
            sinfo.torecords = ToRecords;
            sinfo.totalrecords = ContactDetailsFullCount(TEData, authuser);
            sinfo.listcount = PageperCount;
            sinfo.pages = PageNumber.ToString();
            if (sinfo.torecords > sinfo.totalrecords) { sinfo.torecords = sinfo.totalrecords; sinfo.listcount = sinfo.totalrecords; }
            if (PageperCount == 0) { sinfo.torecords = sinfo.totalrecords; sinfo.listcount = sinfo.totalrecords; }
            if (finalData != null)
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successful";
                FinalResult.Content = new JsonContent(new { result = finalData, info = sinfo });
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "No Records to Show";
                FinalResult.Content = new JsonContent(new { result = "", info = sinfo });
            }
            return FinalResult;
        }
        public List<ContactListDetails> ContactDetailsGetVendor(JObject TEData, int authuser)
        {

            List<ContactListDetails> ContactList = new List<ContactListDetails>();
            string ContactName = TEData["ContactName"].ToObject<string>();
            string ContactMobile = TEData["ContactMobile"].ToObject<string>();
            string ContactEmail = TEData["ContactEmail"].ToObject<string>();
            int? PageNumber = TEData["PageNumber"].ToObject<int>();
            int? PageperCount = TEData["PageperCount"].ToObject<int>();
            if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
            if (TEData["PageperCount"].ToObject<int>() == 0)
            {
                PageperCount = ContactDetailsFullCountVendor(TEData, authuser);
            }
            int? FromRecords = (PageNumber - 1) * PageperCount;
            int? ToRecords = (PageNumber * PageperCount) + 1;
            //context.Configuration.ProxyCreationEnabled = false;
            string cnString = "";
            string cnStringBegin = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY TC.uniqueid) as RowNum, ";
            cnStringBegin += " TC.Uniqueid, TC.CallName, TCM.Mobile, TCE.Emailid ";
            cnStringBegin += "FROM dbo.tecontact AS TC ";
            if (ContactEmail != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.Emailid like '%" + ContactEmail + "%') TCE ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCE ";
            }

            if (ContactMobile != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.mobile like '%" + ContactMobile + "%') TCM ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCM ";
            }
            cnStringBegin += "WHERE ";
            if (ContactEmail != "")
            {
                cnStringBegin += "TCE.emailid like '%" + ContactEmail + "%' and ";

            }
            if (ContactMobile != "")
            {
                cnStringBegin += "TCM.mobile like '%" + ContactMobile + "%' and ";

            }
            if (ContactName != "")
            {
                cnStringBegin += "TC.CallName like '%" + ContactName + "%' and ";

            }
            cnStringBegin += "TC.Status='active' and TC.IsDeleted = 0)";
            cnString = cnStringBegin + "AS T WHERE RowNum > " + FromRecords + " and RowNum < " + ToRecords + "";

            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ContactList.Add(new ContactListDetails
                        {
                            UniqueID = (dr["uniqueid"] != DBNull.Value) ? Convert.ToInt32(dr["uniqueid"].ToString()) : 0,
                            ContactName = dr["callname"].ToString(),
                            ContactMobile = dr["Mobile"].ToString(),
                            ContactEmail = dr["Emailid"].ToString(),
                        });
                    }
                }
                dr.Close();
                cn.Close();
            }
            return ContactList;
        }

        public int ContactDetailsFullCountVendor(JObject TEData, int authuser)
        {
            int ContactList = 0;

            string ContactName = TEData["ContactName"].ToObject<string>();
            string ContactMobile = TEData["ContactMobile"].ToObject<string>();
            string ContactEmail = TEData["ContactEmail"].ToObject<string>();

            //context.Configuration.ProxyCreationEnabled = false;
            string cnString = "";
            string cnStringBegin = "SELECT COUNT(TC.Uniqueid) ";
            cnStringBegin += "FROM dbo.tecontact AS TC ";
            if (ContactEmail != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.Emailid like '%" + ContactEmail + "%') TCE ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCE ";
            }

            if (ContactMobile != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.mobile like '%" + ContactMobile + "%') TCM ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCM ";
            }
            cnStringBegin += "WHERE ";
            if (ContactEmail != "")
            {
                cnStringBegin += "TCE.emailid like '%" + ContactEmail + "%' and ";

            }
            if (ContactMobile != "")
            {
                cnStringBegin += "TCM.mobile like '%" + ContactMobile + "%' and ";

            }
            if (ContactName != "")
            {
                cnStringBegin += "TC.CallName like '%" + ContactName + "%' and ";

            }
            cnStringBegin += "TC.Status='active' and TC.ContactType = 'Vendor'";
            cnString = cnStringBegin;
            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                Int32 ContactList1 = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                ContactList = ContactList1;
            }
            return ContactList;
        }
        
        public int ContactDetailsFullCount(JObject TEData, int authuser)
        {
            int ContactList = 0;

            string ContactName = TEData["ContactName"].ToObject<string>();
            string ContactMobile = TEData["ContactMobile"].ToObject<string>();
            string ContactEmail = TEData["ContactEmail"].ToObject<string>();

            //context.Configuration.ProxyCreationEnabled = false;
            string cnString = "";
            string cnStringBegin = "SELECT COUNT(TC.Uniqueid) ";
            cnStringBegin += "FROM dbo.tecontact AS TC ";
            if (ContactEmail != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.Emailid like '%" + ContactEmail + "%') TCE ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Emailid FROM dbo.TEContactEmail AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCE ";
            }

            if (ContactMobile != "")
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid AND TCE1.mobile like '%" + ContactMobile + "%') TCM ";
            }
            else
            {
                cnStringBegin += "OUTER APPLY(SELECT TOP 1 TCE1.Mobile FROM dbo.TEContactMobile AS TCE1 WHERE TCE1.TEContact = TC.Uniqueid) TCM ";
            }
            cnStringBegin += "WHERE ";
            if (ContactEmail != "")
            {
                cnStringBegin += "TCE.emailid like '%" + ContactEmail + "%' and ";

            }
            if (ContactMobile != "")
            {
                cnStringBegin += "TCM.mobile like '%" + ContactMobile + "%' and ";

            }
            if (ContactName != "")
            {
                cnStringBegin += "TC.CallName like '%" + ContactName + "%' and ";

            }
            cnStringBegin += "TC.Status='active' ";
            cnString = cnStringBegin;
            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                Int32 ContactList1 = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                ContactList = ContactList1;
            }
            return ContactList;
        }

        public int GetCount()
        {
            var VendList = db.TEPOVendorMasters.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            return VendList.Count;
        }
        [HttpPost]
        public HttpResponseMessage GetSearchedVendors(JObject TEData)
        {
            try
            {
                List<SearchVendorDet> GetSearchedVendors = new List<SearchVendorDet>();

                int? PageNumber = TEData["PageNumber"].ToObject<int>();
                int? PageperCount = TEData["PageperCount"].ToObject<int>();
                if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
                int? FromRecords = (PageNumber - 1) * PageperCount;
                if (PageperCount == 0)
                {
                    PageperCount = GetCount();
                }
                int? ToRecords = (PageNumber * PageperCount) + 1;
                string cnString = "";
                string cnStringBegin = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t1.VendorName ASC) as RowNum, ";
                cnStringBegin += " t1.POVendorMasterId as UniqueID,t1.POVendorMasterId,t1.VendorName,t1.CIN,t1.ServiceTax,t1.Currency,t1.PAN as PanNumber,";
                cnStringBegin += " t2.VendorCode,t2.RegionId,t2.POVendorDetailId,t2.BillingAddress,t2.BillingPostalCode,t2.BillingCity,t2.ShippingAddress,t2.ShippingCity,t2.ShippingPostalCode,t2.GSTIN,";
                cnStringBegin += " t3.StateCode as RegionCode,t3.StateName as RegionCodeDesc,";
                cnStringBegin += " t4.Description as Country,t4.CountryCode ";
                cnStringBegin += " FROM dbo.TEPOVendorMaster AS t1 ";
                cnStringBegin += " JOIN dbo.TEPOVendorMasterDetail AS t2 ON t1.POVendorMasterId = t2.POVendorMasterId ";
                cnStringBegin += " left outer JOIN dbo.TEGSTNStateMaster AS t3 ON t2.RegionId = t3.StateID ";
                cnStringBegin += " JOIN dbo.TEPOCountryMaster AS t4 ON t2.CountryId = t4.UniqueID ";
                cnStringBegin += " WHERE  t1.IsActive = 1 and t1.IsDeleted=0 and ";
                cnStringBegin += SetWhereClause(TEData);
                //cnStringBegin += " t1.IsDeleted=0";
                cnStringBegin += ")";
                cnString = cnStringBegin + "AS T WHERE RowNum > " + FromRecords + " and RowNum < " + ToRecords + "";
                if (cnString != "")
                {
                    SqlConnection cn = new SqlConnection();
                    SqlCommand cmd = new SqlCommand();
                    cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = cnString;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GetSearchedVendors.Add(new SearchVendorDet
                            {
                                UniqueID = (dr["UniqueID"] != DBNull.Value) ? Convert.ToInt32(dr["UniqueID"]) : 0,
                                VendorCode = dr["VendorCode"].ToString(),
                                VendorName = dr["VendorName"].ToString(),
                                RegionId = (dr["RegionId"] != DBNull.Value) ? Convert.ToInt32(dr["RegionId"]) : 0,
                                RegionCode = dr["RegionCode"].ToString(),
                                RegionCodeDesc = dr["RegionCodeDesc"].ToString(),
                                POVendorMasterId = (dr["POVendorMasterId"] != DBNull.Value) ? Convert.ToInt32(dr["POVendorMasterId"]) : 0,
                                POVendorDetailId = (dr["POVendorDetailId"] != DBNull.Value) ? Convert.ToInt32(dr["POVendorDetailId"]) : 0,
                                BillingAddress = dr["BillingAddress"].ToString(),
                                BillingPostalCode = dr["BillingPostalCode"].ToString(),
                                BillingCity = dr["BillingCity"].ToString(),
                                ShippingAddress = dr["ShippingAddress"].ToString(),
                                ShippingCity = dr["ShippingCity"].ToString(),
                                ShippingPostalCode = dr["ShippingPostalCode"].ToString(),
                                CIN = dr["CIN"].ToString(),
                                Country = dr["Country"].ToString(),
                                CountryCode = dr["CountryCode"].ToString(),
                                Currency = dr["Currency"].ToString(),
                                GSTIN = dr["GSTIN"].ToString(),
                                PAN = dr["PanNumber"].ToString(),
                                ServiceTax = dr["ServiceTax"].ToString(),
                            });
                        }
                    }
                    dr.Close();
                    cn.Close();
                }
                if (GetSearchedVendors.Count > 0) { sinfo.errorcode = 0; sinfo.errormessage = "Success"; }
                else { sinfo.errorcode = 1; sinfo.errormessage = "Fail"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedVendors, info = sinfo }) };
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


        public string SetWhereClause(JObject TEData)
        {
            string FinalQuery = string.Empty;
            string Qtype = "and";
            if (TEData["Qtype"] != null) if (TEData["Qtype"].ToObject<string>() != "") Qtype = TEData["Qtype"].ToObject<string>();

            string VendorCodeQuery = "";
            if (TEData["VendorCode"] != null)
                if (TEData["VendorCode"].ToObject<string>() != "")
                    VendorCodeQuery = " t2.VendorCode like '%" + TEData["VendorCode"].ToObject<string>() + "%' " + Qtype + " ";
            string VendorNameQuery = "";
            if (TEData["VendorName"] != null)
                if (TEData["VendorName"].ToObject<string>() != "")
                    VendorNameQuery = " t1.VendorName like '%" + TEData["VendorName"].ToObject<string>() + "%' " + Qtype + " ";
            string BillingAddressQuery = "";
            if (TEData["BillingAddress"] != null)
                if (TEData["BillingAddress"].ToObject<string>() != "")
                    BillingAddressQuery = " t2.BillingAddress like '%" + TEData["BillingAddress"].ToObject<string>() + "%' " + Qtype + " ";
            string ShippingAddressQuery = "";
            if (TEData["ShippingAddress"] != null)
                if (TEData["ShippingAddress"].ToObject<string>() != "")
                    ShippingAddressQuery = " t2.ShippingAddress like '%" + TEData["ShippingAddress"].ToObject<string>() + "%' " + Qtype + " ";
            string RegionCodeQuery = "";
            if (TEData["RegionCode"] != null)
                if (TEData["RegionCode"].ToObject<string>() != "")
                    RegionCodeQuery = " t3.StateName like '%" + TEData["RegionCode"].ToObject<string>() + "%' " + Qtype + " ";
            string GSTINQuery = "";
            if (TEData["GSTIN"] != null)
                if (TEData["GSTIN"].ToObject<string>() != "")
                    GSTINQuery = " t2.GSTIN like '%" + TEData["GSTIN"].ToObject<string>() + "%' " + Qtype + " ";
            FinalQuery = VendorCodeQuery + VendorNameQuery + BillingAddressQuery + ShippingAddressQuery + RegionCodeQuery + GSTINQuery;
            if (Qtype.Equals("and")) FinalQuery = FinalQuery + " t1.IsDeleted=0 ";
            else
            {
                if (FinalQuery == null || FinalQuery == "") FinalQuery = FinalQuery + " t1.IsDeleted=0 ";
                else
                {
                    FinalQuery = FinalQuery.Remove(FinalQuery.Length - 4);
                    FinalQuery = " ( " + FinalQuery + ") and t1.IsDeleted=0 ";
                }
            }

            return FinalQuery;
        }
        [HttpPost]
        public HttpResponseMessage GetSearchedWBSCode(JObject TEData)
        {
            try
            {
                List<WbsCodeDtls> GetSearchedWbsCodeDet = new List<WbsCodeDtls>();

                int? PageNumber = TEData["PageNumber"].ToObject<int>();
                int? PageperCount = TEData["PageperCount"].ToObject<int>();
                if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
                int? FromRecords = (PageNumber - 1) * PageperCount;
                int? ToRecords = (PageNumber * PageperCount) + 1;
                string cnString = "";

                string cnStringBegin = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t1.WBSCode ASC) as RowNum, ";
                cnStringBegin += " t1.WBSID,t1.WBSCode,t1.ProjectDesc,t1.ProjectCode,t1.FundCentreID,";
                cnStringBegin += " t2.FundCenter_Code,t2.FundCenter_Description,t2.FundCenter_Owner,";
                cnStringBegin += " t3.WBSName";
                cnStringBegin += " FROM dbo.TEPOWBSFundCentreMapping AS t1 ";
                cnStringBegin += " INNER JOIN dbo.TEPOFundCenter AS t2 ON t1.FundCentreID = t2.Uniqueid ";
                cnStringBegin += " INNER JOIN dbo.TEPOWBSMaster AS t3 ON t1.WBSID = t3.WBSID ";
                //cnStringBegin += " WHERE t1.IsActive = 1 and ";
                cnStringBegin += " WHERE ";
                cnStringBegin += SetWhereClauseForWBSCode(TEData);
                cnStringBegin += ")";
                cnString = cnStringBegin + "AS T WHERE RowNum > " + FromRecords + " and RowNum < " + ToRecords + "";
                if (cnString != "")
                {
                    SqlConnection cn = new SqlConnection();
                    SqlCommand cmd = new SqlCommand();
                    cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = cnString;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GetSearchedWbsCodeDet.Add(new WbsCodeDtls
                            {
                                wbsID = (dr["WBSID"] != DBNull.Value) ? Convert.ToInt32(dr["WBSID"]) : 0,
                                wbsCode = dr["WBSCode"].ToString(),
                                wbsDescription = dr["WBSName"].ToString(),
                                ProjectDescription = dr["ProjectDesc"].ToString(),
                                fundcenterCode = dr["FundCenter_Code"].ToString(),
                                fundcenterDescription = dr["FundCenter_Description"].ToString(),
                                fundcenterOwner = dr["FundCenter_Owner"].ToString(),
                                ProjectCode = dr["ProjectCode"].ToString(),
                                FundCentreID = (dr["FundCentreID"] != DBNull.Value) ? Convert.ToInt32(dr["FundCentreID"]) : 0,
                            });
                        }
                    }
                    dr.Close();
                    cn.Close();
                }
                if (GetSearchedWbsCodeDet.Count > 0) { sinfo.errorcode = 0; sinfo.errormessage = "Success"; }
                else { sinfo.errorcode = 1; sinfo.errormessage = "Fail"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedWbsCodeDet, info = sinfo }) };
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
        public string SetWhereClauseForWBSCode(JObject TEData)
        {
            string FinalQuery = string.Empty;
            string Qtype = "and";
            if (TEData["Qtype"] != null) if (TEData["Qtype"].ToObject<string>() != "") Qtype = TEData["Qtype"].ToObject<string>();

            string ProjectCode = string.Empty;
            if (TEData["ProjectCode"] != null) if (TEData["ProjectCode"].ToObject<string>() != "") ProjectCode = TEData["ProjectCode"].ToObject<string>();
            int FundCentreID = 0;
            if (TEData["FundCentreID"] != null) if (TEData["FundCentreID"].ToObject<string>() != "") FundCentreID = TEData["FundCentreID"].ToObject<int>();

            string WBSCodeQuery = "";
            if (TEData["WBSCode"] != null)
                if (TEData["WBSCode"].ToObject<string>() != "")
                    WBSCodeQuery = " t1.WBSCode like '%" + TEData["WBSCode"].ToObject<string>() + "%' " + Qtype + " ";
            string WBSNameQuery = "";
            if (TEData["WBSName"] != null)
                if (TEData["WBSName"].ToObject<string>() != "")
                    WBSNameQuery = " t3.WBSName like '%" + TEData["WBSName"].ToObject<string>() + "%' " + Qtype + " ";
            string FundCenter_CodeQuery = "";
            if (TEData["FundCenter_Code"] != null)
                if (TEData["FundCenter_Code"].ToObject<string>() != "")
                    FundCenter_CodeQuery = " t2.FundCenter_Code like '%" + TEData["FundCenter_Code"].ToObject<string>() + "%' " + Qtype + " ";
            string FundCenter_OwnerQuery = "";
            if (TEData["FundCenter_Owner"] != null)
                if (TEData["FundCenter_Owner"].ToObject<string>() != "")
                    FundCenter_OwnerQuery = " t2.FundCenter_Owner like '%" + TEData["FundCenter_Owner"].ToObject<string>() + "%' " + Qtype + " ";

            FinalQuery = WBSCodeQuery + WBSNameQuery + FundCenter_CodeQuery + FundCenter_OwnerQuery;
            if (Qtype.Equals("and")) FinalQuery = FinalQuery + " t1.IsDeleted=0 and t1.FundCentreID=" + FundCentreID + " and t1.ProjectCode= '" + ProjectCode + "' ";
            else
            {
                if (FinalQuery == null || FinalQuery == "") FinalQuery = FinalQuery + " t1.IsDeleted=0 and t1.FundCentreID=" + FundCentreID + " and t1.ProjectCode= '" + ProjectCode + "' ";
                else
                {
                    FinalQuery = FinalQuery.Remove(FinalQuery.Length - 4);
                    FinalQuery = " ( " + FinalQuery + ") and t1.IsDeleted = 0 and t1.FundCentreID = " + FundCentreID + " and t1.ProjectCode = '" + ProjectCode + "' ";
                }
            }

            return FinalQuery;
        }

        [HttpPost]
        public HttpResponseMessage GetHSNDetailsForExpenseOrder(JObject TEData)
        {
            int authuser = 0;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("authUser")) { authuser = Convert.ToInt32(headers.GetValues("authUser").First()); }
            return GetHSNDetailsForExpense(TEData, authuser);
        }
        public HttpResponseMessage GetHSNDetailsForExpense(JObject TEData, int authuser)
        {
            HttpResponseMessage FinalResult = new HttpResponseMessage();
            List<HSNDetailsForExpense> finalData = new List<HSNDetailsForExpense>();
            finalData = GetHSNDetailsForExpenseWithFilter(TEData, authuser);

            int pagenumber = 0; int totCount = 0;
            if (TEData["PageNumber"] != null && TEData["PageNumber"].ToString() != "")
                pagenumber = TEData["PageNumber"].ToObject<int>();
            int pagepercount = TEData["PageperCount"].ToObject<int>();
            totCount = finalData.Count;
            var finalResult = finalData;
            if (totCount > 0)
            {
                if (pagenumber >= 0 && pagepercount > 0)
                {
                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    finalResult = finalData.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = totCount;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = pagenumber.ToString();
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";
                }
                FinalResult.Content = new JsonContent(new
                {
                    result = finalResult,
                    info = sinfo
                });
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "No Records to Show";
                FinalResult.Content = new JsonContent(new { result = "", info = sinfo });
            }
            return FinalResult;
        }
        public List<HSNDetailsForExpense> GetHSNDetailsForExpenseWithFilter(JObject TEData, int authuser)
        {
            List<HSNDetailsForExpense> HSNList = new List<HSNDetailsForExpense>();
            string HSNCode = TEData["HSNCode"].ToObject<string>();
            string VendorGSTApplicability = TEData["VendorGSTApplicability"].ToObject<string>();
            string DestinationCountry = TEData["DestinationCountry"].ToObject<string>();
            string VendorRegionCode = TEData["VendorRegionCode"].ToObject<string>();
            string DeliveryPlantRegionCode = TEData["DeliveryPlantRegionCode"].ToObject<string>();

            string cnStringBegin = @"select
                                    hsn.ApplicableTo, hsn.DestinationCountry, hsn.VendorRegionCode,hsn.VendorRegionDescription,
                                    hsn.DeliveryPlantRegionCode,hsn.DeliveryPlantRegionDescription,hsn.HSNCode,
                                    hsn.VendorGSTApplicability,gst.Description VendorGSTDescription,hsn.Taxcode,
                                    Sum(case when hsn.TaxType='CGST' then hsn.TaxRate else 0 end) CGSTTaxRate,
                                    Sum(case when hsn.TaxType='SGST' then hsn.TaxRate else 0 end) SGSTTaxRate,
                                    Sum(case when hsn.TaxType='IGST' then hsn.TaxRate else 0 end) IGSTTaxRate
                                    from TEPOHSNTaxCodeMapping hsn, TEPOGSTApplicabilityMaster gst
                                    where 
                                    hsn.VendorGSTApplicability=gst.UniqueID
                                    and hsn.ApplicableTo='Service'
                                    and hsn.isdeleted=0 and SYSDATETIME() >= hsn.ValidFrom and SYSDATETIME() <= hsn.ValidTo ";
            //if (VendorGSTApplicability != "")
            //{
            //    cnStringBegin += " and hsn.VendorGSTApplicability=" + VendorGSTApplicability;
            //}
            if (DestinationCountry != "")
            {
                cnStringBegin += " and hsn.DestinationCountry='" + DestinationCountry + "'";
            }
            if (VendorRegionCode != "")
            {
                cnStringBegin += " and hsn.VendorRegionCode='" + VendorRegionCode + "'";
            }
            if (DeliveryPlantRegionCode != "")
            {
                cnStringBegin += " and hsn.DeliveryPlantRegionCode = '" + DeliveryPlantRegionCode + "'";
            }
            if (HSNCode != "")
            {
                cnStringBegin += " and hsn.HSNCode like '%" + HSNCode + "%'";

            }
            cnStringBegin += @" group by hsn.ApplicableTo, hsn.DestinationCountry, hsn.VendorRegionCode,hsn.VendorRegionDescription,
                                hsn.DeliveryPlantRegionCode,hsn.DeliveryPlantRegionDescription,hsn.HSNCode,hsn.VendorGSTApplicability,
                                hsn.Taxcode,gst.Description";

            if (cnStringBegin != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnStringBegin;
                SqlDataReader dr = cmd.ExecuteReader();
                int seqNo = 1;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        HSNList.Add(new HSNDetailsForExpense
                        {
                            SeqID = seqNo,
                            ApplicableTo = dr["ApplicableTo"].ToString(),
                            DestinationCountry = dr["DestinationCountry"].ToString(),
                            VendorRegionCode = dr["VendorRegionCode"].ToString(),
                            VendorRegionDescription = dr["VendorRegionDescription"].ToString(),
                            DeliveryPlantRegionCode = dr["DeliveryPlantRegionCode"].ToString(),
                            DeliveryPlantRegionDescription = dr["DeliveryPlantRegionDescription"].ToString(),
                            HSNCode = dr["HSNCode"].ToString(),
                            VendorGSTApplicability = dr["VendorGSTApplicability"].ToString(),
                            VendorGSTDescription = dr["VendorGSTDescription"].ToString(),
                            Taxcode = dr["Taxcode"].ToString(),
                            CGSTTaxRate = (dr["CGSTTaxRate"] != DBNull.Value) ? Convert.ToDecimal(dr["CGSTTaxRate"].ToString()) : 0,
                            SGSTTaxRate = (dr["SGSTTaxRate"] != DBNull.Value) ? Convert.ToDecimal(dr["SGSTTaxRate"].ToString()) : 0,
                            IGSTTaxRate = (dr["IGSTTaxRate"] != DBNull.Value) ? Convert.ToDecimal(dr["IGSTTaxRate"].ToString()) : 0
                        });
                        seqNo += 1;
                    }
                }
                dr.Close();
                cn.Close();
            }
            return HSNList;
        }
        public int HSNDetailsFullCount(JObject TEData, int authuser)
        {
            int HSNCouunt = 0;

            string HSNCode = TEData["HSNCode"].ToObject<string>();
            string VendorGSTApplicability = TEData["VendorGSTApplicability"].ToObject<string>();
            string DestinationCountry = TEData["DestinationCountry"].ToObject<string>();
            string VendorRegionCode = TEData["VendorRegionCode"].ToObject<string>();
            string DeliveryPlantRegionCode = TEData["DeliveryPlantRegionCode"].ToObject<string>();

            string cnStringBegin = @"select
                                    count(hsn.HSNCode)
                                    from TEPOHSNTaxCodeMapping hsn, TEPOGSTApplicabilityMaster gst
                                    where 
                                    hsn.VendorGSTApplicability=gst.UniqueID
                                    and hsn.ApplicableTo='Service'
                                    and hsn.isdeleted=0 and SYSDATETIME() >= hsn.ValidFrom and SYSDATETIME() <= hsn.ValidTo ";
            if (VendorGSTApplicability != "")
            {
                cnStringBegin += "and hsn.VendorGSTApplicability=" + VendorGSTApplicability;
            }
            if (DestinationCountry != "")
            {
                cnStringBegin += @"and hsn.DestinationCountry='" + DestinationCountry + "'";
            }
            if (VendorRegionCode != "")
            {
                cnStringBegin += "and hsn.VendorRegionCode='" + VendorRegionCode + "'";
            }
            if (DeliveryPlantRegionCode != "")
            {
                cnStringBegin += "and hsn.DeliveryPlantRegionCode = '" + DeliveryPlantRegionCode + "'";
            }
            if (HSNCode != "")
            {
                cnStringBegin += "and hsn.HSNCode ='%" + HSNCode + "%'";

            }
            cnStringBegin += @"group by hsn.ApplicableTo, hsn.DestinationCountry, hsn.VendorRegionCode,hsn.VendorRegionDescription,
                                hsn.DeliveryPlantRegionCode,hsn.DeliveryPlantRegionDescription,hsn.HSNCode,hsn.VendorGSTApplicability,
                                hsn.Taxcode,gst.Description";

            if (cnStringBegin != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnStringBegin;
                Int32 HSNList = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                HSNCouunt = HSNList;
            }
            return HSNCouunt;
        }

        [HttpPost]
        public HttpResponseMessage CheckGSTINDuplicate(JObject TEData)
        {
            sinfo.errorcode = 1; sinfo.errormessage = "Error";
            List<string> gstinValue = TEData["GSTIN"].ToObject<List<string>>();
            List<string> vendorCode = TEData["VCode"].ToObject<List<string>>();
            string currencyType = TEData["Currency"].ToObject<string>();
            string PANno = TEData["PANno"].ToObject<string>();
            List<string> BPostalCode = TEData["BPCode"].ToObject<List<string>>();

            vendorCode.RemoveAll(x => x == null);

            if (currencyType == "INR")
            {
                // Check duplicate PAN on Vendor create and update event
                List<string> allPANs = db.TEPOVendorMasters.Select(x => x.PAN).ToList();

                if (allPANs.Contains(PANno))
                {
                    sinfo.errormessage = "Error, Duplicate value found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }

            }
            else
            {
                if (vendorCode.Count == 0)
                {
                    // Check duplicate Billing Postal Code when vendor create
                    List<string> allBPCodes = db.TEPOVendorMasterDetails.Select(x => x.BillingPostalCode).ToList();

                    foreach (string bpCode in BPostalCode)
                    {
                        if (allBPCodes.Contains(bpCode))
                        {
                            sinfo.errormessage = "Error, Duplicate value found";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                }
                else
                {
                    // Check duplicate Billing Postal Code when vendor Update
                    List<string> allBPCodes = db.TEPOVendorMasterDetails.Select(x => x.BillingPostalCode).Except(vendorCode).ToList();

                    foreach (string bpCode in BPostalCode)
                    {
                        if (allBPCodes.Contains(bpCode))
                        {
                            sinfo.errormessage = "Error, Duplicate value found";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                }
            }

            if (vendorCode.Count == 0)
            {
                // Check duplicate GSTIN when vendor create
                List<string> allGSTINS = db.TEPOVendorMasterDetails.Select(x => x.GSTIN).ToList();

                foreach (string gstin in gstinValue)
                {
                    if (allGSTINS.Contains(gstin))
                    {
                        sinfo.errormessage = "Error, Duplicate value found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
            }
            else
            {
                // Check duplicate GSTIN when vendor update
                List<string> allGSTINS = db.TEPOVendorMasterDetails.Select(x => x.GSTIN).Except(vendorCode).ToList();

                foreach (string gstin in gstinValue)
                {
                    if (allGSTINS.Contains(gstin))
                    {
                        sinfo.errormessage = "Error, Duplicate value found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
            }
            sinfo.errorcode = 0;
            sinfo.errormessage = "Success";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }

        [HttpPost]
        public HttpResponseMessage GetVendorDetailByID(JObject json)
        {
            try
            {
                int VendorMasterId = 0;
                VendorMasterId = json["VendorMasterId"].ToObject<int>();
                VendorMasterViewDto res = new VendorMasterViewDto();
                res = (from vndr in db.TEPOVendorMasters
                       where vndr.IsDeleted == false && vndr.POVendorMasterId == VendorMasterId
                       select new VendorMasterViewDto
                       {
                           CIN = vndr.CIN,
                           POVendorMasterId = vndr.POVendorMasterId,
                           Currency = vndr.Currency,
                           PAN = vndr.PAN,
                           ServiceTax = vndr.ServiceTax,
                           VendorContactId = vndr.VendorContactId,
                           VendorName = vndr.VendorName,
                           IsActive = vndr.IsActive,
                       }).FirstOrDefault();

                res.VendorMasterDetails = (from vendrDtl in db.TEPOVendorMasterDetails
                                           join region in db.TEGSTNStateMasters on vendrDtl.RegionId equals region.StateID into tempregion
                                           from state in tempregion.DefaultIfEmpty()
                                           join country in db.TEPOCountryMasters on vendrDtl.CountryId equals country.UniqueID into tempcntry
                                           from cntry in tempcntry.DefaultIfEmpty()
                                           join gstapl in db.TEPOGSTApplicabilityMasters on vendrDtl.GSTApplicabilityId equals gstapl.UniqueID into tempgstapl
                                           from finalgstapl in tempgstapl.DefaultIfEmpty()
                                           join vendAccntGrp in db.TEPOVendorAccountGroupMasters on vendrDtl.VendorAccountGroupId equals vendAccntGrp.UniqueID into tempvendAccntGrp
                                           from finalvendAccntGrp in tempvendAccntGrp.DefaultIfEmpty()
                                           join vendCatg in db.TEPOVendorCategoryMasters on vendrDtl.VendorCategoryId equals vendCatg.UniqueID into tempvendCatg
                                           from finalvendCatg in tempvendCatg.DefaultIfEmpty()
                                           join schema in db.TEPOVendorSchemaGroups on vendrDtl.ScehmaGroupId equals schema.SchemaGroupId into tempschema
                                           from finalschema in tempschema.DefaultIfEmpty()
                                           join glAccnt in db.TEPOGLCodeMasters on vendrDtl.GLAccountId equals glAccnt.UniqueID into tempglAccnt
                                           from finalglAccnt in tempglAccnt.DefaultIfEmpty()
                                           where vendrDtl.IsDeleted == false && vendrDtl.POVendorMasterId == VendorMasterId
                                           select new VendorMasterDetailViewDto
                                           {
                                               POVendorDetailId = vendrDtl.POVendorDetailId,
                                               VendorCode = vendrDtl.VendorCode,
                                               BillingAddress = vendrDtl.BillingAddress,
                                               BillingCity = vendrDtl.BillingCity,
                                               BillingPostalCode = vendrDtl.BillingPostalCode,
                                               ShippingAddress = vendrDtl.ShippingAddress,
                                               ShippingCity = vendrDtl.ShippingCity,
                                               ShippingPostalCode = vendrDtl.ShippingPostalCode,
                                               RegionId = vendrDtl.RegionId,
                                               Region = state.StateName,
                                               CountryId = vendrDtl.CountryId,
                                               Country = cntry.Description,
                                               GSTApplicabilityId = vendrDtl.GSTApplicabilityId,
                                               GSTApplicability = finalgstapl.Description,
                                               GSTIN = vendrDtl.GSTIN,
                                               VendorAccountGroupId = vendrDtl.VendorAccountGroupId,
                                               VendorAccountGroup = finalvendAccntGrp.Description,
                                               VendorCategoryId = vendrDtl.VendorCategoryId,
                                               VendorCategory = finalvendCatg.Description,
                                               SchemaGroupId = vendrDtl.ScehmaGroupId,
                                               SchemaGroup = finalschema.SchemaDescription,
                                               GLAccountId = vendrDtl.GLAccountId,
                                               GLAccount = finalglAccnt.GLAccountShortName,
                                               BankAccountName = vendrDtl.BankAccountName,
                                               BankAccountNumber = vendrDtl.BankAccountNumber,
                                               BankName = vendrDtl.BankName,
                                               IFSCCode = vendrDtl.IFSCCode,
                                               RepresentName = vendrDtl.RepresentName,
                                               Designation = vendrDtl.Designation,
                                               RepresentContactNumber = vendrDtl.ContactNumber,
                                               RepresentEmailID = vendrDtl.EmailID,
                                               CancelledChequeRef = vendrDtl.CancelledChequeRef,
                                               RepresentContactId = vendrDtl.RepresentContactId,
                                               GSTNRegnCertificateRef = vendrDtl.GSTNRegnCertificateRef,
                                               IncorporationCertificateRef = vendrDtl.IncorporationCertificateRef,
                                               IsActive = vendrDtl.IsActive
                                           }).ToList();

                if (res.VendorMasterDetails.Count > 0)
                {
                    for (int i = 0; i < res.VendorMasterDetails.Count; i++)
                    {
                        int VdmID = res.VendorMasterDetails[i].POVendorDetailId;
                        res.VendorMasterDetails[i].WithHoldApplicabilityDetails = (from t1 in db.TEPOVendorWithHoldApplicabilityDetails
                                                                                   join t2t in db.TEPOWithholdingTaxMasters on t1.WithHoldingTaxTypeId equals t2t.UniqueID into tempWTHtax
                                                                                   from t2 in tempWTHtax.DefaultIfEmpty()
                                                                                   join t3t in db.TEPOWithholdingTaxMasters on t1.WithHoldingCodeId equals t3t.UniqueID into tempWTHCode
                                                                                   from t3 in tempWTHCode.DefaultIfEmpty()
                                                                                   where t1.IsDeleted == false && t1.POVendorDetailId == VdmID
                                                                                   select new VendorWithHoldTaxDetailDto
                                                                                   {
                                                                                       POVendorWithHoldApplicabilityDetailId = t1.VendorWithHoldApplicabilityId,
                                                                                       WithHoldingTaxTypeId = t1.WithHoldingTaxTypeId,
                                                                                       WithHoldingTaxType = t2.Description,
                                                                                       WithHoldingCodeId = t1.WithHoldingCodeId,
                                                                                       WithHoldingCode = t3.WithholdingTaxCode,
                                                                                       WithHoldingApplicability = t1.WithHoldingApplicability,
                                                                                   }).ToList();
                    }
                }
                if (res != null) { sinfo.errorcode = 0; sinfo.errormessage = "Success"; }
                else { sinfo.errorcode = 1; sinfo.errormessage = "No Records Found"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = res, info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1; sinfo.errormessage = "Fail";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


        public SuccessInfo SaveVendorValidationCheck(VendorMasterDto vendorDto)
        {
            int dupVand = 0;
            sinfo.errorcode = 0; sinfo.errormessage = "Success";
            if (vendorDto.Currency.Equals("INR") && vendorDto.PAN == "") { sinfo.errorcode = 1; sinfo.errormessage = "PAN Number is mandatory"; return sinfo; }
            if (vendorDto.PAN != "")
            {
                dupVand = db.TEPOVendorMasters.Where(a => a.PAN.Equals(vendorDto.PAN) && a.IsDeleted == false).Count();
                if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already created with the same PAN Number"; return sinfo; }
            }
            dupVand = db.TEPOVendorMasters.Where(a => a.VendorContactId == vendorDto.VendorContactId && a.IsDeleted == false).Count();
            if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already created with the same Contact"; return sinfo; }

            for (int q = 0; q < vendorDto.VendorMasterDetails.Count; q++)
            {
                VendorMasterDetailDto CVmd = new VendorMasterDetailDto();
                CVmd = vendorDto.VendorMasterDetails[q];
                if (CVmd.GSTIN != "" && CVmd.GSTApplicabilityId != 10)
                {
                    dupVand = db.TEPOVendorMasterDetails.Where(a => a.GSTIN.Equals(CVmd.GSTIN) && a.IsDeleted == false).Count();
                    if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already with GSTIN: " + CVmd.GSTIN; return sinfo; }
                    List<string> RGcheck = new List<string>();
                    RGcheck = vendorDto.VendorMasterDetails.Where(a => a.GSTIN.Equals(CVmd.GSTIN)).Select(a => a.RegionId).ToList();
                    dupVand = RGcheck.Distinct().Count();
                    if (dupVand > 1) { sinfo.errorcode = 1; sinfo.errormessage = "Same GSTIN: " + CVmd.GSTIN + " with Different Regions are not allowed"; return sinfo; }
                }
            }
            return sinfo;
        }
        public SuccessInfo UpdateVendorValidationCheck(VendorMasterDto vendorDto)
        {
            int dupVand = 0;
            sinfo.errorcode = 0; sinfo.errormessage = "Success";
            if (vendorDto.Currency.Equals("INR") && vendorDto.PAN == "") { sinfo.errorcode = 1; sinfo.errormessage = "PAN Number is mandatory"; return sinfo; }
            if (vendorDto.PAN != "")
            {
                dupVand = db.TEPOVendorMasters.Where(a => a.PAN.Equals(vendorDto.PAN) && a.POVendorMasterId != vendorDto.POVendorMasterId && a.IsDeleted == false).Count();
                if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already created with the same PAN Number"; return sinfo; }
            }
            //dupVand = db.TEPOVendorMasters.Where(a => a.VendorContactId == vendorDto.VendorContactId && a.POVendorMasterId != vendorDto.POVendorMasterId && a.IsDeleted == false).Count();
            //if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already created with the same Contact"; return sinfo; }

            for (int q = 0; q < vendorDto.VendorMasterDetails.Count; q++)
            {
                VendorMasterDetailDto CVmd = new VendorMasterDetailDto();
                CVmd = vendorDto.VendorMasterDetails[q];
                if (CVmd.GSTIN != "" && CVmd.GSTApplicabilityId != 10)
                {
                    dupVand = db.TEPOVendorMasterDetails.Where(a => a.GSTIN.Equals(CVmd.GSTIN) && a.POVendorDetailId != CVmd.POVendorDetailId && a.IsDeleted == false).Count();
                    if (dupVand > 0) { sinfo.errorcode = 1; sinfo.errormessage = "Vendor already with GSTIN: " + CVmd.GSTIN; return sinfo; }
                    List<string> RGcheck = new List<string>();
                    RGcheck = vendorDto.VendorMasterDetails.Where(a => a.GSTIN.Equals(CVmd.GSTIN)).Select(a => a.RegionId).ToList();
                    dupVand = RGcheck.Distinct().Count();
                    if (dupVand > 1) { sinfo.errorcode = 1; sinfo.errormessage = "Same GSTIN: " + CVmd.GSTIN + " with Different Regions are not allowed"; return sinfo; }
                }
            }

            return sinfo;
        }

        ////[HttpPost]
        //public int CheckVendorDuplicate()
        //{
        //    //        Company
        //    //.GroupBy(c => c.Name)
        //    //.Where(grp => grp.Count() > 1)
        //    //.Select(grp => grp.Key);
        //    //List<string> list3 = new List<string>();
        //    //* var l*/ist = db.TEPOVendorMasterDetails.GroupBy(x => x.VendorCode).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

        //    var list2 = db.TEPOVendorMasterDetails.Where(x => x.IsDeleted == false && x.VendorCode != null).GroupBy(x => x.VendorCode).Where(x => x.Count() == 2).ToList();
        //   var list3 = db.TEPOVendorMasterDetails.Where(x => x.IsDeleted == false && x.VendorCode != null).GroupBy(x => new { x.VendorCode, x.BillingAddress,x.BillingCity,x.BillingPostalCode, x.ShippingAddress, x.ShippingCity, x.ShippingPostalCode, x.RegionId, x.CountryId, x.GSTApplicabilityId, x.GSTIN, x.VendorAccountGroupId, x.VendorCategoryId, x.ScehmaGroupId, x.GLAccountId } ).Where(x => x.Count() == 2).Select(x => new { counts = x.Count(), Keys = x.Key.VendorCode }).ToList();
        //    // int countlist = list.Count;
        //    //List<VendorDupliate> ventest = new List<VendorDupliate>();
        //    int count = 0;
        //    foreach (var vendor in list3)
        //    {
        //            var ventest = db.TEPOVendorMasterDetails
        //                        .Where(x => x.VendorCode == vendor.Keys)
        //                        .ToList();

        //            var distdata = ventest.DistinctBy(x => x.VendorCode).ToList();

        //            if (distdata.Count == 1)
        //            {
        //                count += 1;
        //                var panCount1 = 0;
        //                var panCount2 = 0;
        //                int singlemasterId1 = 0;
        //                int singlemasterId2 = 0;
        //                var pandataID = 0;
        //                foreach (var ve in ventest)
        //                {
        //                    var pandat = db.TEPOVendorMasters.Where(x => x.POVendorMasterId == ve.POVendorMasterId && x.PAN != null).FirstOrDefault();
        //                    if (pandat != null)
        //                    {
        //                            var singleDetail = db.TEPOVendorMasterDetails.Where(x => x.VendorCode == ve.VendorCode && x.POVendorMasterId != pandat.POVendorMasterId).FirstOrDefault();
        //                            if (singleDetail != null)
        //                            {
        //                                panCount1 += 1;
        //                                singlemasterId1 = singleDetail.POVendorDetailId;
        //                                pandataID = pandat.POVendorMasterId;
        //                            }
        //                    }
        //                    else
        //                    {
        //                        panCount2 += 1;
        //                        singlemasterId2 = ve.POVendorDetailId;
                                
        //                    }
        //                }
        //                if (panCount1 == 1)
        //                {
        //                    var detailData = db.TEPOVendorMasterDetails.Where(x => x.POVendorDetailId == singlemasterId1).FirstOrDefault();
        //                    if (detailData != null)
        //                    {
        //                        detailData.POVendorMasterId = pandataID;
        //                        detailData.IsDeleted = true;
        //                        db.Entry(detailData).CurrentValues.SetValues(detailData);
        //                        db.SaveChanges();
        //                    }
        //                }
        //                if (panCount2 == 1)
        //                {
        //                    var singlemaster = db.TEPOVendorMasters.Where(x => x.POVendorMasterId == singlemasterId2).FirstOrDefault();
        //                    singlemaster.IsDeleted = true;
        //                    db.Entry(singlemaster).CurrentValues.SetValues(singlemaster);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    return count;
        //}

        //from c in db.Company
        //group c by c.Name into grp
        //where grp.Count() > 1
        //select grp.Key

        //var list = from c in db.TEPOVendorMasterDetails
        //           group c by c.VendorCode into grp
        //           where grp.Count() > 1
        //           select grp.Key;

        //var list2 = 


        public class Purchase_Vendor_Mapping_List
        {
            public string LastModifiedOn { get; set; }
            public string LastModifiedBy { get; set; }
            public int Uniqueid { get; set; }
            public string Vendor_Code { get; set; }
            public string Vendor_Owner { get; set; }
            public string Status { get; set; }
            public string Address { get; set; }
            public string PanNumber { get; set; }
            public string ServiceTax { get; set; }
            public string Vat { get; set; }
            public string GSTIN { get; set; }
            public string GSTApplicability { get; set; }
            public string RegionCode { get; set; }
            public string RegionCodeDesc { get; set; }
            public string Country { get; set; }
            public int ProjectID { get; set; }
            public int? GLAccountID { get; set; }
            public string CIN { get; set; }
            public bool IsActive { get; set; }
            public string Currency { get; set; }
            public string GLAccountShortName { get; set; }
            public string GLAccountCode { get; set; }

            public List<int> VendorShippingID { get; set; }
            public List<string> Address_Ship { get; set; }
            public List<string> StateCode_Ship { get; set; }
            public List<string> StateCodeDescription_Ship { get; set; }
            public List<string> CountryCode_Ship { get; set; }
            public List<string> GSTIN_Ship { get; set; }
        }
        public class ContactListDetails
        {
            public int UniqueID { get; set; }
            public string ContactName { get; set; }
            public string ContactMobile { get; set; }
            public string ContactEmail { get; set; }
        }
        public class SearchVendorDet
        {
            public int UniqueID { get; set; }
            public string VendorCode { get; set; }
            public string VendorName { get; set; }
            public int? RegionId { get; set; }
            public string RegionCode { get; set; }
            public string RegionCodeDesc { get; set; }
            public int POVendorMasterId { get; set; }
            public int POVendorDetailId { get; set; }
            public string BillingAddress { get; set; }
            public string BillingPostalCode { get; set; }
            public string BillingCity { get; set; }
            public string ShippingAddress { get; set; }
            public string ShippingCity { get; set; }
            public string ShippingPostalCode { get; set; }
            public string CIN { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string Currency { get; set; }
            public string GSTIN { get; set; }
            public string PAN { get; set; }
            public string ServiceTax { get; set; }
        }
        public class HSNDetailsForExpense
        {
            public int SeqID { get; set; }
            public string ApplicableTo { get; set; }
            public string DestinationCountry { get; set; }
            public string VendorRegionCode { get; set; }
            public string VendorRegionDescription { get; set; }
            public string DeliveryPlantRegionCode { get; set; }
            public string DeliveryPlantRegionDescription { get; set; }
            public string HSNCode { get; set; }
            public string VendorGSTApplicability { get; set; }
            public string VendorGSTDescription { get; set; }
            public string Taxcode { get; set; }
            public decimal CGSTTaxRate { get; set; }
            public decimal SGSTTaxRate { get; set; }
            public decimal IGSTTaxRate { get; set; }
        }
    }
}
