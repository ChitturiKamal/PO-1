using Newtonsoft.Json.Linq;
using PO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PO.BAL;

namespace PO.Controllers
{
    public class POSpecificTermandConditionsAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        public POSpecificTermandConditionsAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        [HttpPost]
        public HttpResponseMessage SavePOSpecificTandC(TEPOSpecificTCDetail specTC)
        {
            try
            {
                string temp = string.Empty;
                temp = specTC.Description.Trim();
                specTC.IsDeleted = false;
                specTC.LastModifiedOn = DateTime.Now;
                specTC.CreatedOn = DateTime.Now;
                specTC.Description = temp.Replace("<br/>", "").Replace("<br>", "");
                specTC.SpecificTCTitleMasterId = specTC.SpecificTCTitleMasterId;
                db.TEPOSpecificTCDetails.Add(specTC);
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
        public HttpResponseMessage UpdatePOSpecificTandC(TEPOSpecificTCDetail specTC)
        {
            try
            {
                String temp = specTC.Description;
                specTC.Description = temp.Replace("<br/>", "").Replace("<br>", "");
                TEPOSpecificTCDetail specTCExist = new TEPOSpecificTCDetail();
                specTCExist = db.TEPOSpecificTCDetails.Where(a => a.IsDeleted == false && a.SpecificTCId == specTC.SpecificTCId).FirstOrDefault();
                if (specTCExist != null)
                {
                    specTCExist.Description = specTC.Description;
                   
                    specTCExist.LastModifiedOn = DateTime.Now;
                    specTCExist.SpecificTCSubTitleMasterId = specTC.SpecificTCSubTitleMasterId;
                    specTCExist.SpecificTCTitleMasterId = specTC.SpecificTCTitleMasterId;
                    db.Entry(specTCExist).CurrentValues.SetValues(specTCExist);
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
        public HttpResponseMessage DeletePOSpecificTandC_SingleDelete(TEPOSpecificTCDetail specTC)
        {
            try
            {
                TEPOSpecificTCDetail specTCExist = new TEPOSpecificTCDetail();
                specTCExist = db.TEPOSpecificTCDetails.Where(a => a.IsDeleted == false && a.SpecificTCId == specTC.SpecificTCId).FirstOrDefault();
              //  specTCExist = db.TEPOSpecificTCDetails.Where(a => a.IsDeleted == false && a.SpecificTCId == specTC.SpecificTCSubTitleMasterId).FirstOrDefault();
                if (specTCExist != null)
                {
                    specTCExist.LastModifiedOn = DateTime.Now;
                    specTCExist.IsDeleted = true;
                    db.Entry(specTCExist).CurrentValues.SetValues(specTCExist);
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
        public HttpResponseMessage DeletePOSpecificTandC(TEPOSpecificTCTitleMaster specTitleTC)
        {
            try
            {
                var specTCListIds = (from tcDetail in db.TEPOSpecificTCDetails
                               join subtitle in db.TEPOSpecificTCSubTitleMasters on tcDetail.SpecificTCSubTitleMasterId equals subtitle.SpecificTCSubTitleMasterId
                               where subtitle.SpecificTCTitleMasterId == specTitleTC.SpecificTCTitleMasterId && subtitle.IsDeleted == false
                               select new { tcDetail.SpecificTCId }).ToList();

                foreach(var specData in specTCListIds)
                {
                    TEPOSpecificTCDetail specTCExist = new TEPOSpecificTCDetail();
                    specTCExist = db.TEPOSpecificTCDetails.Where(a => a.IsDeleted == false && a.SpecificTCId == specData.SpecificTCId).FirstOrDefault();
                    if (specTCExist != null)
                    {
                        specTCExist.LastModifiedOn = DateTime.Now;
                        specTCExist.IsDeleted = true;
                        db.Entry(specTCExist).CurrentValues.SetValues(specTCExist);
                        db.SaveChanges();
                    }
                    else
                    {
                        //sinfo.errorcode = 0;
                        //sinfo.errormessage = "Unable to Delete";
                        //return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail To Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

            sinfo.errorcode = 0;
            sinfo.errormessage = "Successfully Deleted";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }

        [HttpPost]
        public HttpResponseMessage GetPOSpecificTCTitles()
        {
           int count = 0;
            try
            {
                var  specTitleList = (from spcTitle in db.TEPOSpecificTCTitleMasters
                                 where spcTitle.IsDeleted == false
                                 select new
                                 {
                                     SpecificTCTitleMasterId = spcTitle.SpecificTCTitleMasterId,
                                     Title = spcTitle.Title
                                 }).OrderBy(x => x.Title).ToList();
                count = specTitleList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specTitleList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specTitleList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOSpecificTCSubTitleDetails(JObject json)
        {
            int count = 0;
            try
            {
                int masterId = json["SpecificTCTitleMasterId"].ToObject<int>();
                //var specSubTitleList = (from spcsubTitle in db.TEPOSpecificTCSubTitleMasters
                //                    where spcsubTitle.IsDeleted == false && spcsubTitle.SpecificTCTitleMasterId== masterId
                //                        select new
                //                    {
                //                        SpecificTCTitleMasterId = spcsubTitle.SpecificTCTitleMasterId,
                //                        SubTitleDesc = spcsubTitle.SubTitleDesc,
                //                        SpecificTCSubTitleMasterId = spcsubTitle.SpecificTCSubTitleMasterId

                //                    }).ToList();
                //var specSubTitleList =  (from tcd in db.TEPOSpecificTCDetails
                //                                        join spcsubTitle in db.TEPOSpecificTCSubTitleMasters on tcd.SpecificTCSubTitleMasterId equals spcsubTitle.SpecificTCSubTitleMasterId
                //                             where spcsubTitle.IsDeleted == false && spcsubTitle.SpecificTCTitleMasterId== masterId
                //                         select new
                //                        {
                //                            SpecificTCTitleMasterId = spcsubTitle.SpecificTCTitleMasterId,
                //                            SubTitleDesc = spcsubTitle.SubTitleDesc,
                //                            SpecificTCSubTitleMasterId = spcsubTitle.SpecificTCSubTitleMasterId,


                //                        }).Distinct().ToList();

                var specSubTitleList = (from spcsubTitle in db.TEPOSpecificTCSubTitleMasters
                                        where spcsubTitle.IsDeleted == false && spcsubTitle.SpecificTCTitleMasterId == masterId
                                        select new
                                        {
                                            SpecificTCTitleMasterId = spcsubTitle.SpecificTCTitleMasterId,
                                            SubTitleDesc = spcsubTitle.SubTitleDesc,
                                            SpecificTCSubTitleMasterId = spcsubTitle.SpecificTCSubTitleMasterId,

                                        }).Distinct().OrderBy(x =>x.SubTitleDesc).ToList();
                count = specSubTitleList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specSubTitleList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specSubTitleList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }
                
        [HttpPost]
        public HttpResponseMessage GetAllPOSpecificTCDetails(JObject json)
        {
            int headerId = json["POHeaderStructureId"].ToObject<int>();                      
            try
            {
               var specDtlList= new PurchaseOrderBAL().GetPOSpecificTCDetailsByPOHeaderStructereId(headerId);
                if (specDtlList.Count>0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specDtlList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specDtlList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Specific Terms and Conditions";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOSpecificTermsandConditions(JObject json)
        {
            List<SpecificTCDeatailDTO> specDtlList = new List<SpecificTCDeatailDTO>();
            int headerId = json["POHeaderStructureId"].ToObject<int>();
            try
            {
                List<SpecificDetail> spcTClist = new List<SpecificDetail>();
                    spcTClist = (from tcd in db.TEPOSpecificTCDetails
                                 join subTitle in db.TEPOSpecificTCSubTitleMasters on tcd.SpecificTCSubTitleMasterId equals subTitle.SpecificTCSubTitleMasterId into tempSubTitle
                                 from TitleSub in tempSubTitle.DefaultIfEmpty()
                                 join title in db.TEPOSpecificTCTitleMasters on tcd.SpecificTCTitleMasterId equals title.SpecificTCTitleMasterId into tempTitle
                                 from titleMas in tempTitle.DefaultIfEmpty()
                                 where tcd.POHeaderStructureId == headerId && tcd.IsDeleted == false
                                 select new SpecificDetail
                                 {
                                     Title = titleMas.Title,
                                     SpecificTCTitleMasterId = titleMas.SpecificTCTitleMasterId,
                                     SpecificTCSubTitleMasterId = TitleSub.SpecificTCSubTitleMasterId,
                                     SubTitleDesc = TitleSub.SubTitleDesc,
                                     SpecificTCId = tcd.SpecificTCId,
                                     Description = tcd.Description,
                                 }).Distinct().ToList();
                if (spcTClist.Count > 0)
                {
                    var titleList = spcTClist.Select(a => new { a.SpecificTCTitleMasterId, a.Title }).Distinct().ToList();
                        foreach (var titleSpec in titleList)
                        {
                            SpecificTCDeatailDTO specDtlDto = new SpecificTCDeatailDTO();
                            specDtlDto.SpecificTCTitleMasterId = Convert.ToInt32(titleSpec.SpecificTCTitleMasterId);
                            specDtlDto.Title = titleSpec.Title;
                            List<SpecificTCDTO> specSubTitleDtoList = new List<SpecificTCDTO>();
                            specSubTitleDtoList = (from tc in db.TEPOSpecificTCDetails
                                                   join subtc in db.TEPOSpecificTCSubTitleMasters on tc.SpecificTCSubTitleMasterId equals subtc.SpecificTCSubTitleMasterId into tempSub
                                                   from subMaster in tempSub.DefaultIfEmpty()
                                                   where tc.SpecificTCTitleMasterId == titleSpec.SpecificTCTitleMasterId && tc.IsDeleted == false &&  tc.POHeaderStructureId == headerId
                                                   select new SpecificTCDTO
                                                   {
                                                       SpecificTCSubTitleMasterId = subMaster.SpecificTCSubTitleMasterId,
                                                       SpecificTCTitleMasterId = tc.SpecificTCTitleMasterId,
                                                       SubTitleDesc = subMaster.SubTitleDesc,
                                                       SpecificTCId = tc.SpecificTCId,
                                                       Description = tc.Description,
                                                       POHeaderStructureId = tc.POHeaderStructureId
                                                   }).ToList();
                            specDtlDto.SpecSubTitlesList = specSubTitleDtoList;
                            if (specDtlDto != null)
                            {
                                specDtlList.Add(specDtlDto);
                            }
                        }
                    }
                    
                if (spcTClist.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specDtlList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = specDtlList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Specific Terms and Conditions";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        
        [HttpPost]
        public HttpResponseMessage TEPOSpecificTCDetailByID(JObject json)
        {
            int SpecificTCId = json["SpecificTCId"].ToObject<int>();
            try
            {
                var SpecificTCDetail = (from spcTC in db.TEPOSpecificTCDetails
                                        join subtitle in db.TEPOSpecificTCSubTitleMasters on spcTC.SpecificTCSubTitleMasterId equals subtitle.SpecificTCSubTitleMasterId
                                        join title in db.TEPOSpecificTCTitleMasters on subtitle.SpecificTCTitleMasterId equals title.SpecificTCTitleMasterId
                                        where spcTC.SpecificTCId== SpecificTCId
                                        select new
                                        {
                                            spcTC.SpecificTCSubTitleMasterId,
                                            spcTC.Description,
                                            spcTC.SpecificTCId,
                                            spcTC.TEPOHeaderStructure,
                                            subtitle.SubTitleDesc,
                                            subtitle.SpecificTCTitleMasterId,
                                            title.Title,
                                        }).First();
                if (SpecificTCDetail!=null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = SpecificTCDetail, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = SpecificTCDetail, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }


        public class SpecificDetail
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
