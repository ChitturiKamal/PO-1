using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using PO.Models;
using PO.BAL;

namespace PO.Controllers
{
    public class FundCenterManagerMappingController : ApiController
    {
        RecordException ExceptionObj = new RecordException();
        TETechuvaDBContext db = new TETechuvaDBContext();
        TEPOFundCenterPOMgrMapping DAl = new TEPOFundCenterPOMgrMapping();
        SuccessInfo sinfo = new SuccessInfo();

        public FundCenterManagerMappingController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

       // [Route("api/FundCenterManagerMapping/Fundcenter_POManager_Mapping")]
        [HttpPost]
       // public HttpResponseMessage Fundcenter_POManager_Mapping(TEPOFundCenterPOMgrMapping Fund_Managermapping)
        public HttpResponseMessage Fundcenter_POManager_Mapping(JObject json)
        {
            try
            {
                int center = 0;int manager = 0;int LastModifiedBy = 0;

               // if (json["FundCenterId"] != null && json["FundCenterId"].ToString() != "")
                center = json["FundCenterId"].ToObject<int>();
                manager= json["POManagerId"].ToObject<int>();

                TEPOFundCenterPOMgrMapping Fund_Managermapping = new TEPOFundCenterPOMgrMapping();

                Fund_Managermapping.FundCenterId = center;
                Fund_Managermapping.CreatedBy = GetLogInUserId();
                //Fund_Managermapping.POManagerId = LastModifiedBy;
                Fund_Managermapping.POManagerId = json["POManagerId"].ToObject<int>();
                //TEPOFundCenterPOMgrMapping Fund_Managermapping = new TEPOFundCenterPOMgrMapping();
                //int center = json["FundCenterId"].ToObject<int>();
                string mesg = "";
                if (Fund_Managermapping == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
                //Fund_Managermapping.FundCenterId = 1;
                //Fund_Managermapping.POManagerId = 1;
                //Fund_Managermapping.LastModifiedOn = DateTime.Now;
                Fund_Managermapping.CreatedOn = DateTime.Now;
                Fund_Managermapping.IsDeleted = false;
                db.TEPOFundCenterPOMgrMappings.Add(Fund_Managermapping);
                db.SaveChanges();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
               

            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };

            }
        }
        [Route("api/FundCenterManagerMapping/updateFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage updateFundcenter_POManager_Mapping(JObject json)
        {
            try
            {
                int center = 0; int manager = 0; int LastModifiedBy = 0; int fundmanagmap = 0;
                center = json["FundCenterId"].ToObject<int>();
                manager = json["POManagerId"].ToObject<int>();
                fundmanagmap= json["FundCenterPOMgrMappingId"].ToObject<int>();
                TEPOFundCenterPOMgrMapping Fund_Managermapping = new TEPOFundCenterPOMgrMapping();

                Fund_Managermapping.FundCenterId = center;
                Fund_Managermapping.CreatedBy = GetLogInUserId();
                //Fund_Managermapping.POManagerId = LastModifiedBy;
                Fund_Managermapping.POManagerId = manager;
                Fund_Managermapping.FundCenterPOMgrMappingId = fundmanagmap;
                string mesg = "";
                if (Fund_Managermapping == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
                //Fund_Managermapping.LastModifiedOn = DateTime.Now;
                var map = db.TEPOFundCenterPOMgrMappings.Single(a => a.FundCenterPOMgrMappingId == Fund_Managermapping.FundCenterPOMgrMappingId);
                if (map != null)
                {
                    map.FundCenterId = Fund_Managermapping.FundCenterId;
                    map.POManagerId = Fund_Managermapping.POManagerId;
                    map.LastModifiedOn = DateTime.Now;
                    db.Entry(map).CurrentValues.SetValues(map);
                    db.SaveChanges();
                }

                //db.Entry(map).CurrentValues.SetValues(Fund_Managermapping);
                //db.Entry(Fund_Managermapping).CurrentValues.SetValues(Fund_Managermapping);
                //db.SaveChanges();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Updated";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };

            }
        }


        [Route("api/FundCenterManagerMapping/getFundcenter_POManager_Mapping_byid")]
        [HttpPost]
        public HttpResponseMessage getFundcenter_POManager_Mapping_byid(JObject json)
        {
            try
            {
                int FundCenterPOMgrMappingId = json["FundCenterPOMgrMappingId"].ToObject<int>();
                var obj = (from FP in db.TEPOFundCenterPOMgrMappings
                          where FP.FundCenterPOMgrMappingId == FundCenterPOMgrMappingId && FP.IsDeleted==false
                          join UP in db.UserProfiles on FP.POManagerId equals UP.UserId
                          join fund in db.TEPOFundCenters on
                             FP.FundCenterId equals fund.Uniqueid
                          select new FundcenterManagerMap
                          {
                              FundCenterPOMgrMappingId = FP.FundCenterPOMgrMappingId,
                              FundCenterId = fund.Uniqueid,
                              UserId = UP.UserId,

                              UserName = UP.UserName,
                              FundCenter_Description = fund.FundCenter_Description,

                              LastModifiedBy = FP.LastModifiedBy,
                              LastModifiedOn = FP.LastModifiedOn

                          }).ToList();
                //int count= obj.Count;
                if(obj.Count>0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = obj, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = obj, info = sinfo }) };
                }
            }
            catch(Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        //[Route("api/FundCenterManagerMapping/getAllFundcenter_POManager_Mapping")]
        //[HttpGet]
        //public List<FundcenterManagerMap> getAllFundcenter_POManager_Mapping()
        //{
        //    try
        //    {
        //        List<FundcenterManagerMap> FundcenterMangerLists = new List<FundcenterManagerMap>();
        //        FundcenterMangerLists = (from FP in db.TEPOFundCenterPOMgrMappings
        //                                 join UP in db.UserProfiles on FP.POManagerId equals UP.UserId
        //                                 //join fund in db.TEPOFundCenters on FP.FundCenterId equals fund.FundCenter_Code
        //                                 join fc in db.TEPOFundCenters on FP.FundCenterId equals fc.Uniqueid
        //                                 where FP.IsDeleted == false
        //                                 select new FundcenterManagerMap
        //                                 {
        //                                     FundCenterPOMgrMappingId = FP.FundCenterPOMgrMappingId,
        //                                     UserId = UP.UserId,
        //                                     FundCenterId = fc.Uniqueid,
        //                                     UserName = UP.UserName,
        //                                     FundCenter_Description = fc.FundCenter_Description,
        //                                     LastModifiedBy = FP.LastModifiedBy,
        //                                     LastModifiedOn = FP.LastModifiedOn

        //                                 }).ToList<FundcenterManagerMap>();
        //        //IQueryable<fundcenterDTO> query = FundcenterLists.AsQueryable();
        //        return FundcenterMangerLists;
        //    }
        //    catch(Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail";
        //        sinfo.listcount = 0;
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}


        [Route("api/FundCenterManagerMapping/getAllFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage getAllFundcenter_POManager_Mapping(JObject json)
        {
            try
            {
                int pagenumber = json["page_number"].ToObject<int>();
                int pagepercount = json["pagepercount"].ToObject<int>();
                int count = 0;
                List<FundcenterManagerMap> FundcenterMangerLists = new List<FundcenterManagerMap>();
                FundcenterMangerLists = (from FP in db.TEPOFundCenterPOMgrMappings
                                         join UP in db.UserProfiles on FP.POManagerId equals UP.UserId
                                         //join fund in db.TEPOFundCenters on FP.FundCenterId equals fund.FundCenter_Code
                                         join fc in db.TEPOFundCenters on FP.FundCenterId equals fc.Uniqueid
                                         join user in db.UserProfiles on FP.CreatedBy equals user.UserId
                                         where FP.IsDeleted == false
                                         select new FundcenterManagerMap
                                         {
                                             FundCenterPOMgrMappingId = FP.FundCenterPOMgrMappingId,
                                             UserId = UP.UserId,
                                             FundCenterId = fc.Uniqueid,
                                             UserName = UP.UserName,
                                             FundCenter_Description = fc.FundCenter_Description,
                                             FundCenter_Code = fc.FundCenter_Code,
                                             LastModifiedBy_Name = user.UserName,
                                             LastModifiedOn = FP.LastModifiedOn,
                                            

                                         }).ToList<FundcenterManagerMap>();
                //IQueryable<fundcenterDTO> query = FundcenterLists.AsQueryable();
                //return FundcenterMangerLists;



                 count = FundcenterMangerLists.Count;
                if (count>0)
                {
                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    var finalResult = FundcenterMangerLists.Skip(start).Take(iPageSize).ToList();
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
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = FundcenterMangerLists, info = sinfo }) };
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

        [Route("api/FundCenterManagerMapping/getAllUserFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage getAllUserFundcenter_POManager_Mapping(JObject json)
        {
            try
            {
                int pagenumber = json["page_number"].ToObject<int>();
                int pagepercount = json["pagepercount"].ToObject<int>();
                int count = 0;
                List<UserMapFundCenter> FundcenterMangerLists = new List<UserMapFundCenter>();

                var fundID = db.TEPOFundCenterUserMappings.Where(x => x.IsDeleted == false).Select(x => x.FundCenterId).ToList().Distinct();
                foreach (int Id in fundID)
                {
                    UserMapFundCenter UserFundCenter = new UserMapFundCenter();
                    var Fund = db.TEPOFundCenters.Where(x => x.Uniqueid == Id && x.IsDeleted == false).FirstOrDefault();
                    UserFundCenter.FundCenterCode = Fund.FundCenter_Code;
                    UserFundCenter.FundCenterID = Id;
                    UserFundCenter.FundCenterDescription = Fund.FundCenter_Description;
                    UserFundCenter.FundCentMap = (from FundCent in db.TEPOFundCenterUserMappings
                                                  join UserProf in db.UserProfiles
                                                  on FundCent.UserId equals UserProf.UserId into FundCentMap
                                                  from FundMap in FundCentMap.DefaultIfEmpty()

                                                  join UserProfModif in db.UserProfiles
                                                  on FundCent.LastModifiedBy equals UserProfModif.UserId into UserFundCentMap
                                                  from userFundMap in UserFundCentMap.DefaultIfEmpty()

                                                  where FundCent.IsDeleted == false && FundCent.FundCenterId == Id
                                                  select new TEPOFundCenterUserMappingModel
                                                  {
                                                    FundCenterUserMappingId = FundCent.FundCenterUserMappingId,
                                                      FundCenterId = FundCent.FundCenterId,
                                                      ManagerName = FundMap.CallName,
                                                      UserID = FundCent.UserId,
                                                      LastModifiedBy = userFundMap.CallName,
                                                      LastModifiedOn = FundCent.LastModifiedOn
                                                  }
                                                  ).ToList();
                    UserFundCenter.LastModify = UserFundCenter.FundCentMap[0].LastModifiedBy;
                    UserFundCenter.LastModifyOn = UserFundCenter.FundCentMap[0].LastModifiedOn;

                    //db.TEPOFundCenterUserMappings.Where(x => x.FundCenterId == Id).ToList();
                    FundcenterMangerLists.Add(UserFundCenter);

                }


                count = FundcenterMangerLists.Count;
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
                    var finalResult = FundcenterMangerLists.Skip(start).Take(iPageSize).ToList();
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
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = FundcenterMangerLists, info = sinfo }) };
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

        [Route("api/FundCenterManagerMapping/Add_UserFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage Add_UserFundcenter_POManager_Mapping(UserFundCenterMapping FundCentjson)
        {
            try
            {
                int FundCount = db.TEPOFundCenterUserMappings.Where(x => x.FundCenterId == FundCentjson.FundCenter && x.IsDeleted == false).Count();
                if (FundCount == 0)
                {
                    if (FundCentjson.MasterSubmitterlist.Count > 0)
                    {
                        foreach (MasterSubmitterlistModel Submitter in FundCentjson.MasterSubmitterlist.OrderBy(x => x.SequenceId).ToList())
                        {
                            //if(Submitter.)
                            TEPOFundCenterUserMapping TEPOFund = new TEPOFundCenterUserMapping();

                            TEPOFund.UserId = Submitter.SubmiterID;
                            TEPOFund.FundCenterId = FundCentjson.FundCenter;
                            TEPOFund.CreatedBy = FundCentjson.LastModifiedBy;
                            TEPOFund.LastModifiedBy = FundCentjson.LastModifiedBy;
                            TEPOFund.LastModifiedOn = DateTime.Now;
                            TEPOFund.CreatedOn = DateTime.Now;
                            TEPOFund.IsDeleted = false;
                            db.TEPOFundCenterUserMappings.Add(TEPOFund);
                            db.SaveChanges();

                        }
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Saved Sucessfully";
                        sinfo.listcount = 0;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Submitters Present";
                        sinfo.listcount = 0;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "mapping has been Done for this FundCenter";
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

        [Route("api/FundCenterManagerMapping/Delete_UserFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage Delete_UserFundcenter_POManager_Mapping(UserFundCenterMapping FundCentjson)
        {
            try
            {

                List<TEPOFundCenterUserMapping> FundVal = db.TEPOFundCenterUserMappings.Where(x => x.FundCenterId == FundCentjson.FundCenter && x.IsDeleted == false).ToList();

                foreach(TEPOFundCenterUserMapping TEPOFUnd in FundVal)
                {
                    TEPOFUnd.IsDeleted = true;
                    TEPOFUnd.LastModifiedBy = FundCentjson.LastModifiedBy;
                    TEPOFUnd.LastModifiedOn = DateTime.Now;
                    db.Entry(TEPOFUnd).CurrentValues.SetValues(TEPOFUnd);
                    db.SaveChanges();
                }

                sinfo.errorcode = 0;
                sinfo.errormessage = "Deleted Sucessfully";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch(Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

        }


        [Route("api/FundCenterManagerMapping/DUpdate_UserFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage DUpdate_UserFundcenter_POManager_Mapping(UserupdtFundCenterMapping FundCentjson)
        {
            try
            {

                List<TEPOFundCenterUserMapping> FundVal = db.TEPOFundCenterUserMappings.Where(x => x.FundCenterId == FundCentjson.FundCentreCode && x.IsDeleted == false).ToList();

                if (FundVal.Count > 0)
                {
                    foreach (TEPOFundCenterUserMapping TEPOFUnd in FundVal)
                    {
                        TEPOFUnd.IsDeleted = true;
                        TEPOFUnd.LastModifiedBy = FundCentjson.LastModifiedBy;
                        TEPOFUnd.LastModifiedOn = DateTime.Now;
                        db.Entry(TEPOFUnd).CurrentValues.SetValues(TEPOFUnd);
                        db.SaveChanges();
                    }

                    foreach (int Fund in FundCentjson.ManagerName)
                    {
                        if (Fund != 0 && !String.IsNullOrEmpty(Fund.ToString()))
                        {
                            TEPOFundCenterUserMapping TEPOFund = new TEPOFundCenterUserMapping();

                            TEPOFund.UserId = Fund;
                            TEPOFund.FundCenterId = FundCentjson.FundCentreCode;
                            TEPOFund.CreatedBy = FundCentjson.LastModifiedBy;
                            TEPOFund.LastModifiedBy = FundCentjson.LastModifiedBy;
                            TEPOFund.LastModifiedOn = DateTime.Now;
                            TEPOFund.CreatedOn = DateTime.Now;
                            TEPOFund.IsDeleted = false;
                            db.TEPOFundCenterUserMappings.Add(TEPOFund);
                            db.SaveChanges();
                        }
                    }
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Updated Sucessfully";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Mapping DOne";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
               
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Update";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

        }

        public class UserupdtFundCenterMapping
        {
            public int FundCentreCode { get; set; }
            public int LastModifiedBy { get; set; }
            public List<int> ManagerName { get; set; }
        }

        public class UserFundCenterMapping
        {
            public int FundCenter { get; set; }
            public int LastModifiedBy { get; set; }
            public List<int> ManagerName { get; set; }
            public List<MasterSubmitterlistModel> MasterSubmitterlist { get; set; }
        }

        public class MasterSubmitterlistModel
        {
            public int SubmiterID { get; set; }
            public String Type { get; set; }
            public int SequenceId { get; set; }
        }

        public class UserMapFundCenter
        {
            public int FundCenterID;
            public String FundCenterCode;
            public String FundCenterDescription;
            public String LastModify;
            public DateTime? LastModifyOn;
           public List<TEPOFundCenterUserMappingModel> FundCentMap;

        }

        public class TEPOFundCenterUserMappingModel
        {
            public int FundCenterUserMappingId { get; set; }
            public int FundCenterId { get; set; }
            public int UserID { get; set; }
            public String ManagerName { get; set; }
            public Nullable<System.DateTime> LastModifiedOn { get; set; }
            public String LastModifiedBy { get; set; }
            
        }

        [Route("api/FundCenterManagerMapping/GetAllUsers")]
        [HttpGet]
        public IEnumerable<UserProfile> GetAllUsers()
        {
            return db.UserProfiles.Where(d => d.IsDeleted == false).ToList();
        }

        [Route("api/FundCenterManagerMapping/GetAllgetFundcenter")]
        [HttpGet]
        public IEnumerable<TEPOFundCenter> GetAllgetFundcenter(TEPOFundCenter Fundcenter)
        {
            return db.TEPOFundCenters.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/FundCenterManagerMapping/DeleteFundcenter_POManager_Mapping")]
        [HttpPost]
        public HttpResponseMessage DeleteFundcenter_POManager_Mapping(JObject json)
        {
            try
            {
                int FundCenterPOMgrMappingId = json["FundCenterPOMgrMappingId"].ToObject<int>();
                TEPOFundCenterPOMgrMapping FundPOMap = db.TEPOFundCenterPOMgrMappings.Where(a => a.FundCenterPOMgrMappingId == FundCenterPOMgrMappingId && a.IsDeleted == false).FirstOrDefault();
                if (FundPOMap != null)
                {
                    FundPOMap.LastModifiedOn = DateTime.Now;
                    FundPOMap.IsDeleted = true;
                    db.Entry(FundPOMap).CurrentValues.SetValues(FundPOMap);
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
