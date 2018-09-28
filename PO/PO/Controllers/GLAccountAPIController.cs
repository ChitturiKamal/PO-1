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
    public class GLAccountAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        public GLAccountAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        [HttpPost]
        public HttpResponseMessage SaveGLAccountDetails(TEPOGLCodeMaster glAccount)
        {
            try
            {
                glAccount.IsDeleted = false;
                glAccount.LastModifiedOn = DateTime.Now;
                db.TEPOGLCodeMasters.Add(glAccount);
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
        public HttpResponseMessage UpdateGLAccountDetails(TEPOGLCodeMaster glAcnt)
        {
            try
            {
                TEPOGLCodeMaster glAccountObj = db.TEPOGLCodeMasters.Where(a => a.UniqueID == glAcnt.UniqueID && a.IsDeleted == false).FirstOrDefault();
                if (glAccountObj != null)
                {
                    glAccountObj.CommitmentItemCode = glAcnt.CommitmentItemCode;
                    glAccountObj.GLAccountDesc = glAcnt.GLAccountDesc;
                    glAccountObj.GLAccountCode = glAcnt.GLAccountCode;
                    glAccountObj.CommitmentItemDesc = glAcnt.CommitmentItemDesc;
                    glAccountObj.Recon = glAcnt.Recon;
                    db.Entry(glAccountObj).CurrentValues.SetValues(glAccountObj);
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
        public HttpResponseMessage DeleteGLAccountDetails(TEPOGLCodeMaster glAccount)
        {
            try
            {
                TEPOGLCodeMaster glAccountObj = db.TEPOGLCodeMasters.Where(a => a.UniqueID == glAccount.UniqueID && a.IsDeleted == false).FirstOrDefault();
                if (glAccountObj != null)
                {
                    glAccountObj.LastModifiedOn = DateTime.Now;
                    glAccountObj.IsDeleted = true;
                    db.Entry(glAccountObj).CurrentValues.SetValues(glAccountObj);
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
        public HttpResponseMessage GetGLAccountDetailsByID(JObject json)
        {
            int glUniqueID = json["UniqueID"].ToObject<int>();
            try
            {
                var glAccountDetails = (from plnt in db.TEPOGLCodeMasters
                                          join user in db.UserProfiles on plnt.LastModifiedBy equals user.UserId into tempuser
                                          from prof in tempuser.DefaultIfEmpty()
                                          where plnt.IsDeleted == false && plnt.UniqueID == glUniqueID
                                          select new
                                          {
                                              plnt.CommitmentItemCode,
                                              plnt.CommitmentItemDesc,
                                              plnt.GLAccountCode,
                                              plnt.GLAccountDesc,
                                              plnt.LastModifiedBy,
                                              plnt.LastModifiedOn,
                                              plnt.GLAccountShortName,
                                              plnt.Recon,
                                              plnt.UniqueID,
                                              prof.UserName,
                                              prof.CallName
                                          }).FirstOrDefault();
                if (glAccountDetails != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
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
        public HttpResponseMessage GetGLAccountDetails_Pagination(JObject json)
        {
            int pagenumber = json["page_number"].ToObject<int>();
            int pagepercount = json["pagepercount"].ToObject<int>();
            int count = 0;
            try
            {
                var glAccountDetails = (from plnt in db.TEPOGLCodeMasters
                                        join user in db.UserProfiles on plnt.LastModifiedBy equals user.UserId into tempuser
                                        from prof in tempuser.DefaultIfEmpty()
                                        where plnt.IsDeleted == false
                                        select new
                                        {
                                            plnt.CommitmentItemCode,
                                            plnt.CommitmentItemDesc,
                                            plnt.GLAccountCode,
                                            plnt.GLAccountDesc,
                                            plnt.LastModifiedBy,
                                            plnt.LastModifiedOn,
                                            plnt.Recon,
                                            plnt.GLAccountShortName,
                                            plnt.UniqueID,
                                            prof.UserName,
                                            prof.CallName
                                        }).ToList();

                count = glAccountDetails.Count;
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
                    var finalResult = glAccountDetails.Skip(start).Take(iPageSize).ToList();
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
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
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
        public HttpResponseMessage GetGLAccountDetails(JObject json)
        {
            try
            {
                var glAccountDetails = (from plnt in db.TEPOGLCodeMasters
                                        where plnt.IsDeleted == false
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
        public HttpResponseMessage GetManagersList(JObject json)
        {
            try
            {
                var usersList = (from user in db.UserProfiles
                                     //  join emp in db.TEEmpBasicInfoes on usr.UserId equals emp.UserId
                                 where user.IsDeleted == false && user.UserName != null && user.UserName != ""
                                 && user.CallName != null && user.CallName != ""
                                 select new
                                 {
                                     user.CallName,
                                     PersonalEmail = user.email,
                                     user.UserId,
                                     //emp.EmployeeId,
                                     user.Phone
                                 }).OrderBy(a => a.CallName).ToList();
                int count = usersList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = usersList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = usersList, info = sinfo }) };
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
        public HttpResponseMessage GetManagersByFundCenterID(JObject json)
        {
            try
            {
                int fundcenterId = json["FundCenterID"].ToObject<int>();
                var usersList = (from fndMap in db.TEPOFundCenterPOMgrMappings
                                 join user in db.UserProfiles on fndMap.POManagerId equals user.UserId
                                 where fndMap.FundCenterId==fundcenterId && user.IsDeleted == false && fndMap.IsDeleted == false && user.UserName != null && user.UserName != "" 
                                 && user.CallName != null && user.CallName != ""
                                 select new
                                 {
                                     user.CallName,
                                     PersonalEmail = user.email,
                                     user.UserId,
                                     user.Phone
                                 }).Distinct().OrderBy(a => a.CallName).ToList();
                int count = usersList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = usersList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = usersList, info = sinfo }) };
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
        public HttpResponseMessage GetPurchaseItemswithPOIdForGLupdate(JObject json)
        {
            try
            {
                int phsId = json["ItemStructureID"].ToObject<int>();
                var list = (from items in db.TEPOItemStructures
                            where items.Uniqueid  == phsId && items.IsDeleted==false
                            select new
                            {
                                items.Uniqueid,
                                items.POStructureId,
                                items.Material_Number,
                                MaterialName = items.Short_Text,
                                MaterialDescription = items.Long_Text,
                                items.GLAccountNo,
                                items.ItemType
                            }).ToList();
                int count = list.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = list, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = list, info = sinfo }) };
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
        public HttpResponseMessage UpdateGLAccountsForPurchaseItems(GLAccountPuchaseItemsList glAccList)
        {
            try
            {
                for (int cnt = 0; cnt < glAccList.Uniqueid.Count; cnt++)
                {
                     if (glAccList.Uniqueid[cnt] != 0 && glAccList.GLAccountNo[cnt] != null && glAccList.GLAccountNo[cnt] != "")
                     {
                        int tempid = 0;
                        string user = string.Empty;
                        tempid = glAccList.Uniqueid[cnt];
                        user = db.UserProfiles.Where(x => x.UserId == glAccList.LastmodifiedByID && x.IsDeleted == false).Select(x => x.UserName).FirstOrDefault();
                        TEPOItemStructure item = new TEPOItemStructure();
                        item = db.TEPOItemStructures.Where(a => a.Uniqueid == tempid && a.IsDeleted == false).FirstOrDefault();
                        if (item != null)
                        {
                            item.GLAccountNo = glAccList.GLAccountNo[cnt];
                            item.LastModifiedBy = user;
                            item.LastModifiedOn = DateTime.Now.ToShortDateString();
                            db.Entry(item).CurrentValues.SetValues(item);
                            db.SaveChanges();
                        }
                    }
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Updated";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        
        public class GLAccountPuchaseItemsList
        {
            public List<int> Uniqueid { get; set; }
            public List<string> GLAccountNo { get; set; }
            public int LastmodifiedByID { get; set; }
        }
    }
}
