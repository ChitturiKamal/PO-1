using PurchaseOrder.BAL;
using PurchaseOrder.Models;
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

namespace PurchaseOrder.Controllers
{
    public class PurchaseOrderController : ApiController
    {
        TETechuvaDBContext1 db = new TETechuvaDBContext1();
        PurchaseOrders DAl = new PurchaseOrders();
        RecordExceptions except = new RecordExceptions();

        public PurchaseOrderController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        [Route("api/PurchaseOrder/GetAll")]
        [HttpGet]
        public IEnumerable<TEPurchase_header> GetAll()
        {
            return db.TEPurchase_header.Where(d => d.IsDeleted == false).ToList();
        }

        //GET : api/poapicontroller/GetPurchase_header
        //parameters : Integer (id)
        [Route("api/PurchaseOrder/GetPurchaseheader/{id}")]
        [HttpGet]
        public TEPurchase_header GetPurchase_header(int id)
        {
            return db.TEPurchase_header.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
        }
        [Route("api/PurchaseOrder/CreatePO")]
        [HttpPost]
        public string CreatePO(TEPurchase_header_structure obj)
        {

            TEPurchase_header_structure PoStructure = new TEPurchase_header_structure();
            PoStructure = db.TEPurchase_header_structure.Add(obj);
            db.SaveChanges();
            return PoStructure.Uniqueid.ToString();
        }
        [Route("api/PurchaseOrder/GetAllTEPurchaseheaderstructure")]
        [HttpGet]
        public IEnumerable<TEPurchase_header_structure> GetAllTEPurchaseheaderstructure()
        {
            return db.TEPurchase_header_structure.Where(d => d.IsDeleted == false).ToList();
        }

        [Route("api/PurchaseOrder/GetTEPurchaseheaderstructure/{id}")]
        [HttpGet]
        public TEPurchase_header_structure GetTEPurchaseheaderstructure(int id)
        {
            return db.TEPurchase_header_structure.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
        }


        //**********//Customer//**************//
        [Route("api/PurchaseOrder/GetAllCustomers")]
        [HttpGet]
        public IEnumerable<TECompany> GetAllCustomers(TECompany customer)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TECompanies.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetCustomer/{id}")]
        [HttpGet]
        public TECompany GetCustomer(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TECompanies.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
        }

        //*************//Vendors//*****************//
        [Route("api/PurchaseOrder/GetAllVendors")]
        [HttpGet]
        public IEnumerable<TEPurchase_Vendor> GetAllVendors(TEPurchase_Vendor vendor)
        {
            return db.TEPurchase_Vendor.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetVendor/{id}")]
        [HttpGet]
        public TEPurchase_Vendor GetVendor(int id)
        {
            return db.TEPurchase_Vendor.Where(a => a.Uniqueid == id && a.IsDeleted == false).SingleOrDefault();
        }
        [Route("api/PurchaseOrder/createTEPurchase_Vendor")]
        [HttpPost]
        public HttpResponseMessage createTEPurchase_Vendor(TEPurchase_Vendor purchase_Vendor)
        {
            try
            {
                string mesg = "";
                if (purchase_Vendor == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
                purchase_Vendor.CreatedOn = DateTime.Now.Date.ToShortDateString();
                purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
                db.TEPurchase_Vendor.Add(purchase_Vendor);
                db.SaveChanges();
                mesg = "successfully Registered";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }
        [Route("api/PurchaseOrder/updateTEPurchase_Vendor")]
        [HttpPost]
        public HttpResponseMessage updateTEPurchase_Vendor(TEPurchase_Vendor purchase_Vendor)
        {
            try
            {
                string mesg = "";
                if (purchase_Vendor == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }

                purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
                var vendor = db.TEPurchase_Vendor.Single(a => a.Uniqueid == purchase_Vendor.Uniqueid);
                db.Entry(vendor).CurrentValues.SetValues(purchase_Vendor);
                db.SaveChanges();
                mesg = "successfully Updated";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }
        [Route("api/PurchaseOrder/createTEPOMasterApprovers")]
        [HttpPost]
        public HttpResponseMessage createTEPOMasterApprovers(TEPOMasterApprover masterApprover)
        {
            try
            {
                string mesg = "";
                //if (masterApprover == null)
                //{
                //    mesg = "check object";
                //    return new HttpResponseMessage()
                //    {
                //        StatusCode = HttpStatusCode.NoContent,
                //        Content = new StringContent(mesg)
                //    };
                //}
                if (masterApprover != null)
                {
                    masterApprover.CreatedOn = DateTime.Now;
                    masterApprover.LastModifiedOn = DateTime.Now;
                    masterApprover.IsDeleted = false;
                    db.TEPOMasterApprovers.Add(masterApprover);
                    db.SaveChanges();
                    mesg = "successfully registered";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(mesg)
                    };
                }
                else
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }
        [Route("api/PurchaseOrder/updateTEPOMasterApprovers")]
        [HttpPost]
        public HttpResponseMessage updateTEPOMasterApprovers(TEPOMasterApprover masterApprover)
        {
            try
            {
                string mesg = "";
                //if (masterApprover == null)
                //{
                //    mesg = "check object";
                //    return new HttpResponseMessage()
                //    {
                //        StatusCode = HttpStatusCode.NoContent,
                //        Content = new StringContent(mesg)
                //    };
                //}
                if (masterApprover != null)
                {

                    masterApprover.LastModifiedOn = DateTime.Now;
                    masterApprover.IsDeleted = false;
                    var approver = db.TEPOMasterApprovers.Single(a => a.UniqueId == masterApprover.UniqueId);
                    db.Entry(approver).CurrentValues.SetValues(masterApprover);
                    db.SaveChanges();
                    mesg = "successfully Updated";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(mesg)
                    };
                }
                else
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }
        [Route("api/PurchaseOrder/GetAllServiceConditions")]
        [HttpGet]
        public IEnumerable<TEPurchase_Service_Condition> GetAllServiceConditions()
        {
            return db.TEPurchase_Service_Condition.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetServiceCondition/{id}")]
        [HttpGet]
        public TEPurchase_Service_Condition GetServiceCondition(int id)
        {
            return db.TEPurchase_Service_Condition.Where(a => a.ServiceConditionId == id && a.IsDeleted == false).FirstOrDefault();
        }
        //TEPOMasterApprovers
        [Route("api/PurchaseOrder/GetAllMasterAprovers/{pagepercount}/{pageNumber}")]
        [HttpGet]
        public HttpResponseMessage GetAllMasterAprovers(int pagepercount, int pageNumber)
        {
            try
            {
                SuccessInfo sinfo = new SuccessInfo();
                FailInfo finfo = new FailInfo();
                HttpResponseMessage hrm = new HttpResponseMessage();
                List<object> objlist = new List<object>();
                var list = db.TEPOMasterApprovers.Where(d => d.IsDeleted == false).ToList();


                int count = list.Count;
                if (count > 0)
                {
                    if (pageNumber == 0)
                    {
                        pageNumber = 1;
                    }
                    int iPageNum = pageNumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    var finalResult = list.Skip(start).Take(iPageSize).ToList();
                    foreach (var li in finalResult)
                    {
                        var each = getTEPurchase_MasterApprovers(li.UniqueId);
                        objlist.Add(each);
                    }
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = start + iPageSize;
                    sinfo.totalrecords = count;
                    sinfo.listcount = count;
                    sinfo.pages = "1";

                    if (objlist.Count > 0)
                    {
                        hrm.Content = new JsonContent(new
                        {
                            result = objlist,
                            info = sinfo
                        });
                    }
                    else
                    {
                        finfo.errorcode = 0;
                        finfo.errormessage = "No Records";
                        finfo.listcount = 0;
                        hrm.Content = new JsonContent(new
                        {
                            info = finfo
                        });
                    }


                }
                return hrm;
            }
            catch (Exception e)
            {

                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }

        }
        [Route("api/PurchaseOrder/GetMasterAprover/{id}")]
        [HttpGet]
        public TEPOMasterApprover GetMasterAprover(int id)
        {
            return db.TEPOMasterApprovers.Where(a => a.UniqueId == id && a.IsDeleted == false).FirstOrDefault();
        }

        //*************//FundCenter//*************//
        [Route("api/PurchaseOrder/GetAllFundCenters")]
        [HttpGet]
        public IEnumerable<TEPurchase_FundCenter> GetAllFundCenters(TEPurchase_FundCenter fundCenter)
        {
            return db.TEPurchase_FundCenter.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetFundCenter/{id}")]
        [HttpGet]
        public TEPurchase_FundCenter GetFundCenter(int id)
        {
            return db.TEPurchase_FundCenter.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
        }
        [Route("api/PurchaseOrder/GetAllProjects/{id}")]
        [HttpGet]
        public IEnumerable<TEProject> GetAllProjects(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEProjects.Where(d => d.TECompany.Uniqueid == id && d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetProject/{id}")]
        [HttpGet]
        public IQueryable GetProject(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEProject project = db.TEProjects.Where(d => d.ProjectID == id && d.IsDeleted == false).FirstOrDefault();
            var obj = from TEP in db.TEProjects
                      where TEP.ProjectID == id
                      from
                      TEG in db.TEGSTNStateMasters
                      where TEP.StateID == TEG.StateID
                      select new
                      {
                          Project = TEP,
                          State = TEG
                      };
            return obj;
        }
        //[Route("api/poapi/GetProjectDetails/{id}")]
        //[HttpGet]
        //public TEProjectDetail GetProjectDetails(int id)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    return db.TEProjectDetails.Where(d => d.ProjectID == id && d.IsDeleted==false).FirstOrDefault();
        //}
        [Route("api/PurchaseOrder/GetGSTIN/{id}")]
        [HttpGet]
        public TEGSTNStateMaster GetGSTIN(int id)
        {
            return db.TEGSTNStateMasters.Where(d => d.StateID == id && d.IsDeleted == false).FirstOrDefault();

        }
        [Route("api/PurchaseOrder/GetGeneralTermsConditions")]
        [HttpGet]
        public IEnumerable<TEMasterTermsCondition> GetGeneralTermsConditions()
        {
            return db.TEMasterTermsConditions.Where(d => d.IsDeleted == false && d.IsActive == true).ToList();
        }
        [Route("api/PurchaseOrder/GetPOTypes")]
        [HttpGet]
        public IEnumerable<TEPurchase_OrderTypes> GetPOTypes()
        {
            return db.TEPurchase_OrderTypes.Where(d => d.IsDeleted == false).ToList();
        }
        [Route("api/PurchaseOrder/GetCategories")]
        [HttpGet]
        public IEnumerable<CATEGORYMASTER> GetCategories()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.CATEGORYMASTERs.ToList();
        }
        [Route("api/PurchaseOrder/GetSubCategories/{id}")]
        [HttpGet]
        public IEnumerable<SUBCATEGORYMASTER> GetSubCategories(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.SUBCATEGORYMASTERs.Where(d => d.CATEGORYID == id).ToList();
        }
        [Route("api/PurchaseOrder/GetWBSCodes/{category}/{subcatogery}")]
        [HttpGet]
        public IEnumerable<WBSMASTER> GetWBSCodes(int subcatogery, int category)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.WBSMASTERs.Where(d => d.SUBCATEGORYID == subcatogery && d.categoryid == category).ToList();

        }
        [Route("api/PurchaseOrder/billing/{projectCode}")]
        [HttpGet]
        public List<object> billing(int projectCode)
        {
            // db.Configuration.ProxyCreationEnabled = false;
            string pjCode = projectCode.ToString();
            // PlantStorageDetail billingData = new PlantStorageDetail();
            List<object> list = new List<object>();
            var billingData = db.PlantStorageDetails.Where(d => d.PlantStorageCode == pjCode && d.Type == "billing" && d.isdeleted == false).FirstOrDefault();
            list.Add(billingData);
            var shippingData = db.PlantStorageDetails.Where(d => d.StorageLocationCode == pjCode && d.Type == "shipping" && d.isdeleted == false).ToList();
            list.Add(shippingData);
            return list;
        }
        [Route("api/PurchaseOrder/shipping")]
        [HttpGet]
        public PlantStorageDetail shipping(string projectCode)
        {
            PlantStorageDetail p = new PlantStorageDetail();
            // db.Configuration.ProxyCreationEnabled = false;
            string pjCode = projectCode.ToString();
            p = db.PlantStorageDetails.Where(d => d.StorageLocationCode == pjCode && d.Type == "Shipping" && d.isdeleted == false).FirstOrDefault();
            return p;

        }
        [Route("api/PurchaseOrder/getShippingDetails/{id}")]
        [HttpGet]
        public PlantStorageDetail getShippingDetails(int id)
        {
            return db.PlantStorageDetails.Where(d => d.PlantStorageDetailsID == id && d.isdeleted == false).FirstOrDefault();
        }

        //**************************//WBS & Fundcenter mapping[Start]//******************//
        [Route("api/PurchaseOrder/Wbs_FundcenterMapping")]
        [HttpPost]
        public HttpResponseMessage Wbs_FundcenterMapping(TEPurchase_WBSFundCentreMapping mapping)
        {
            try
            {
                string mesg = "";
                if (mapping == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
                mapping.LastModifiedOn = DateTime.Now;
                db.TEPurchase_WBSFundCentreMapping.Add(mapping);
                db.SaveChanges();
                mesg = "successfully registered";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };

            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };

            }
        }
        [Route("api/PurchaseOrder/updateWbs_FundcenterMapping")]
        [HttpPost]
        public HttpResponseMessage updateWbs_FundcenterMapping(TEPurchase_WBSFundCentreMapping mapping)
        {
            try
            {
                string mesg = "";
                if (mapping == null)
                {
                    mesg = "check object";
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }
                mapping.LastModifiedOn = DateTime.Now;
                var map = db.TEPurchase_WBSFundCentreMapping.Single(a => a.UniqueID == mapping.UniqueID);
                db.Entry(map).CurrentValues.SetValues(mapping);
                db.SaveChanges();
                mesg = "successfully registered";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };

            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };

            }
        }
        [Route("api/PurchaseOrder/getWbs_FundcenterMapping/{uniqueid}")]
        [HttpGet]
        public IQueryable getWbs_FundcenterMapping(int uniqueid)
        {
            var obj = from ad in db.TEPurchase_WBSFundCentreMapping
                      where ad.UniqueID == uniqueid
                      join wbs in db.WBSMASTERs on ad.WBSID equals wbs.WBSID
                      join fund in db.TEPurchase_FundCenter on
                         ad.FundCentreID equals fund.Uniqueid
                      select new fundcenterDTO
                      {
                          uniqueID = ad.UniqueID,
                          categoryid = wbs.categoryid,
                          SUBCATEGORYID = wbs.SUBCATEGORYID,
                          WBSCode = ad.WBSCode,
                          FundcenterUniqueID = fund.Uniqueid,
                          name = wbs.name,
                          WbsUniqueID = wbs.WBSID,
                          FundCentreCode = ad.FundCentreCode,
                          FundCenter_Description = fund.FundCenter_Description,
                          LastModifiedBy = ad.LastModifiedBy,
                          LastModifiedOn = ad.LastModifiedOn

                      };
            return obj;
        }
        [Route("api/PurchaseOrder/getAllWbs_FundcenterMapping")]
        [HttpGet]
        public List<fundcenterDTO> getAllWbs_FundcenterMapping()
        {
            //  List<fundcenterDTO> FundcenterLists = new List<fundcenterDTO>();
            List<fundcenterDTO> FundcenterLists = new List<fundcenterDTO>();
            FundcenterLists = (from ad in db.TEPurchase_WBSFundCentreMapping
                               join wbs in db.WBSMASTERs on ad.WBSID equals wbs.WBSID
                               join fund in db.TEPurchase_FundCenter on
                                  ad.FundCentreID equals fund.Uniqueid
                               select new fundcenterDTO
                               {
                                   uniqueID = ad.UniqueID,
                                   categoryid = wbs.categoryid,
                                   SUBCATEGORYID = wbs.SUBCATEGORYID,
                                   WBSCode = ad.WBSCode,
                                   name = wbs.name,
                                   FundCentreCode = ad.FundCentreCode,
                                   FundCenter_Description = fund.FundCenter_Description,
                                   LastModifiedBy = ad.LastModifiedBy,
                                   LastModifiedOn = ad.LastModifiedOn

                               }).ToList<fundcenterDTO>();
            //IQueryable<fundcenterDTO> query = FundcenterLists.AsQueryable();
            return FundcenterLists;
        }
        //**************************//WBS & Fundcenter mapping[END]//******************//
        [Route("api/PurchaseOrder/GetAllUsers")]
        [HttpGet]
        public IEnumerable<UserProfile> GetAllUsers()
        {
            return db.UserProfiles.Where(d => d.IsDeleted == false).ToList();
        }

        //**************************//TEPurchase_MasterApprovers[START]//******************//
        [Route("api/PurchaseOrder/TEPurchase_MasterApprovers")]
        [HttpPost]
        public HttpResponseMessage TEPurchase_MasterApprovers(PurchaseApproversDTO pr)
        {
            try
            {
                string mesg = "";

                if (pr == null)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }

                TEPOMasterApprover approver = new TEPOMasterApprover();
                TEPOApprovalCondition condition = new TEPOApprovalCondition();
                condition = pr.ApprovalCondition;
                string req = DAl.addTEPOApprovalCondition(condition);
                req = DAl.addTEPOApprover(pr.MasterApproverlist, req);

                mesg = "successfully registered";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };

            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        [Route("api/PurchaseOrder/updateTEPurchase_MasterApprovers")]
        [HttpPost]
        public HttpResponseMessage updateTEPurchase_MasterApprovers(PurchaseApproversDTO pr)
        {
            try
            {
                string mesg = "";

                if (pr == null)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(mesg)
                    };
                }

                TEPOMasterApprover approver = new TEPOMasterApprover();

                TEPOApprovalCondition condition = new TEPOApprovalCondition();
                foreach (var appr in pr.MasterApproverlist)
                {
                    approver = db.TEPOMasterApprovers.Where(a => a.UniqueId == appr.UniqueId).SingleOrDefault();
                    approver.ApproverId = appr.ApproverId;
                    approver.ApproverName = appr.ApproverName;
                    approver.CreatedBy = appr.CreatedBy;
                    approver.CreatedOn = appr.CreatedOn;
                    approver.SequenceId = appr.SequenceId;
                    approver.ApprovalConditionId = appr.ApprovalConditionId;
                    approver.Type = appr.Type;
                    approver.LastModifiedOn = DateTime.Now;
                    approver.LastModifiedBy = appr.LastModifiedBy;

                }
                TEPOMasterApprover approverBack = new TEPOMasterApprover();
                approverBack = db.TEPOMasterApprovers.Where(a => a.UniqueId == approver.UniqueId).SingleOrDefault();
                db.Entry(approverBack).CurrentValues.SetValues(approver);


                TEPOApprovalCondition conditionBack = new TEPOApprovalCondition();
                conditionBack = db.TEPOApprovalConditions.Where(a => a.UniqueId == approver.ApprovalConditionId).SingleOrDefault();
                condition.OrderType = pr.ApprovalCondition.OrderType;
                condition.FundCenter = pr.ApprovalCondition.FundCenter;
                condition.MinAmount = pr.ApprovalCondition.MinAmount;
                condition.MaxAmount = pr.ApprovalCondition.MaxAmount;
                condition.PurchasingGroup = conditionBack.PurchasingGroup;
                condition.UniqueId = conditionBack.UniqueId;
                condition.OldUniqueId = conditionBack.OldUniqueId;
                condition.ObjectId = conditionBack.ObjectId;
                condition.IsDeleted = conditionBack.IsDeleted;
                condition.LastModifiedBy = approver.LastModifiedBy;
                condition.LastModifiedOn = DateTime.Now;
                condition.CreatedBy = approver.CreatedBy;
                condition.CreatedOn = approver.CreatedOn;
                db.Entry(conditionBack).CurrentValues.SetValues(condition);
                db.SaveChanges();

                mesg = "successfully updated";
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(mesg)
                };

            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        [Route("api/PurchaseOrder/getTEPurchase_MasterApprovers/{uniqueid}")]
        [HttpGet]
        public IQueryable getTEPurchase_MasterApprovers(int uniqueid)
        {
            var list = from ad in db.TEPOMasterApprovers
                       from aa in db.TEPOApprovalConditions
                       where ad.UniqueId == uniqueid && ad.ApprovalConditionId == aa.UniqueId
                       select new
                       {

                           OrderType = aa.OrderType,
                           FundCenter = aa.FundCenter,
                           MinAmount = aa.MinAmount,
                           MaxAmount = aa.MaxAmount,
                           Type = ad.Type,
                           SequenceId = ad.SequenceId,
                           CreatedBy = ad.CreatedBy,
                           CreatedOn = ad.CreatedOn,
                           LastModifiedBy = ad.LastModifiedBy,
                           LastModifiedOn = ad.LastModifiedOn,
                           ApprovalConditionId = ad.ApprovalConditionId,
                           ApproverId = ad.ApproverId,
                           ApproverName = ad.ApproverName,
                           uniqueid = ad.UniqueId

                       };
            return list;
        }

        [Route("api/PurchaseOrder/getAllTEPurchase_MasterApprovers")]
        [HttpGet]
        public IEnumerable<TEPOMasterApprover> getAllTEPurchase_MasterApprovers()
        {
            return db.TEPOMasterApprovers.Where(a => a.IsDeleted == false).ToList();
        }
        //**************************//TEPurchase_MasterApprovers[END]//******************//

        [Route("api/PurchaseOrder/TEPurchase_GetAllMaterials")]
        [HttpGet]
        public HttpResponseMessage TEPurchase_GetAllMaterials()
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                string mesg = "";
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //var url = ComponentLibraryHost + "/materials";
                var url = "https://clapi.total-environment.com/materials/all?details=true&batchSize=20";
                var form = new Dictionary<string, string>
                {
                    //{"name", projectname},
                    //{"projectCode",projectcode},
                    //{"location",projectlocation},
                };
                var tokenResponse = client.GetAsync(url).Result;
                if (tokenResponse.StatusCode.Equals(201))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                //return new HttpResponseMessage()
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Content = new StringContent(mesg)
                //};
                List<MaterialDTO> list = new List<MaterialDTO>();
                list = DAl.ParseJobject(tokenResponse);
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = list

                });
                return hrm;
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }

        /// <summary>
        /// to generate Azure AD token to access the secured API's
        /// written on 17-October-2017by Sudheer Reddy Danda
        /// </summary>
        /// <returns>AccessToken</returns>
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
                //if (log.IsDebugEnabled)
                //    log.Debug("Entered into generateToken_EDesign method to Generate Token with the following Inputs \n" +
                //                "baseAddress :" + baseAddress + "/n" +
                //                "grant_type :" + grant_type + "/n" +
                //                "client_id :" + client_id + "/n" +
                //                "resource :" + resource +
                //                "client_secret :" + client_secret + "/n" +
                //                "tenant :" + tenant);
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
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Token Generated successfully and the  Token is " + tokenObj.AccessToken);
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Generate Token");
                }
            }
            catch (Exception ex)
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Failed to Generate Token against your request," + ex.Message);
            }
            //if (log.IsDebugEnabled)
            //    log.Debug("Leaving from generateToken_EDesign method");

            return tokenObj;
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
        [Route("api/PurchaseOrder/TEPurchase_GetMaterialByMaterialCode")]
        [HttpGet]
        public HttpResponseMessage TEPurchase_GetMaterialByMaterialCode(string materialCode)
        {
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                string mesg = "";
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // https://localhost:44380/materials/MFR023128?dataType=true
                var url = ComponentLibraryHost + "/materials/" + materialCode + "?dataType=true";

                var tokenResponse = client.GetAsync(url).Result;
                if (tokenResponse.StatusCode.Equals(201))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                //return new HttpResponseMessage()
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Content = new StringContent(mesg)
                //};
                return tokenResponse;
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }


        [Route("api/PurchaseOrder/TEPurchase_GetMaterialCheckListByID")]
        [HttpGet]
        public HttpResponseMessage TEPurchase_GetMaterialCheckListByID(string checkListID)
        {
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                string mesg = "";
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // http://localhost:8080/check-lists/MGPO01
                var url = ComponentLibraryHost + "/check-lists/" + checkListID;

                var tokenResponse = client.GetAsync(url).Result;
                if (tokenResponse.StatusCode.Equals(201))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                //return new HttpResponseMessage()
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Content = new StringContent(mesg)
                //};
                return tokenResponse;
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }

        }
        [Route("api/PurchaseOrder/TEPurchase_GetMaterialCheckListByIDs")]
        [HttpGet]
        public HttpResponseMessage TEPurchase_GetMaterialCheckListByIDs(List<string> TEPurchase_GetMaterialCheckListByIDs)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {

                List<HttpResponseMessage> lists = new List<HttpResponseMessage>();
                foreach (string j in TEPurchase_GetMaterialCheckListByIDs)
                {
                    var hrms = TEPurchase_GetMaterialCheckListByID(j);
                    if (hrms.StatusCode == HttpStatusCode.OK)
                    {
                        lists.Add(hrms);
                    }
                }

                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = lists

                });
                return hrm;
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }
        [Route("api/PurchaseOrder/TEPurchase_GetMaterialSpecifications")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage TEPurchase_GetMaterialSpecifications(List<string> TEPurchase_GetMaterialIDs)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                List<object> list = new List<object>();
                List<object> listm = new List<object>();
                if (TEPurchase_GetMaterialIDs != null)
                {
                    foreach (string id in TEPurchase_GetMaterialIDs)
                    {


                        HttpResponseMessage hrms = new HttpResponseMessage();

                        hrms = TEPurchase_GetMaterialByMaterialCode(id);
                        var onjs = hrms.Content.ReadAsStringAsync();
                        JObject jo = JObject.Parse(onjs.Result);
                        JToken x = jo.SelectToken("headers");


                        foreach (JToken v in x)
                        {
                            if (v["key"].ToString() == "specifications")
                            {


                                list.Add(v);



                            }

                        }
                        //foreach(string obj in list)
                        //{
                        //    HttpResponseMessage hrmss = new HttpResponseMessage();
                        //    hrmss = TEPurchase_GetMaterialCheckListByID(obj);
                        //    var ons = hrmss.Content.ReadAsStringAsync();
                        //    JObject joi = JObject.Parse(ons.Result);
                        //    listm.Add(joi);
                        //}
                    }
                }
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = list

                });
                return hrm;
            }
            catch (Exception e)
            {
                except.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        [Route("api/PurchaseOrder/TEPurchase_SavePO")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage TEPurchase_SavePO(JObject js)
        {
            string msg = "";
            if (js != null)
            {
                PurchaseHeaderStructure headerStructure = new PurchaseHeaderStructure();
                PurchaseItemStructure itemStructure = new PurchaseItemStructure();
                PurchaseService purServ = new PurchaseService();
                TEVendorPaymentMilestone value = new TEVendorPaymentMilestone();
                PurchaseItemwise purItemCond = new PurchaseItemwise();
                JToken obj = js.First;
                string FuguePurchaseOrderNumber = "Fugue_" + DateTime.Now.ToString("yyyymmdd_hhmmss");

                foreach (var jd in obj.First)
                {
                    headerStructure = jd.ToObject<PurchaseHeaderStructure>();
                    headerStructure.AgreementSignedDate = System.DateTime.Now.ToString();
                    headerStructure.CompanyCode = "";
                    headerStructure.CurrencyKey = "";
                    headerStructure.ExchangeRate = "";
                    headerStructure.PurchasingOrderNumber = "";
                    headerStructure.ManagedBy = "";
                    headerStructure.OurReference = "";
                    headerStructure.PartnerFunction2 = "";
                    headerStructure.PartnerFunction1 = "";
                    headerStructure.PartnerFunctionVendorCode1 = "";
                    headerStructure.PartnerFunctionVendorCode2 = "";
                    headerStructure.Path = "";
                    headerStructure.PaymentKey = "";
                    headerStructure.POTitle = (js["potype"].ToString() != null) ? js["potype"].ToString() : "";

                    headerStructure.PurchasingDocumentDate = System.DateTime.Now.ToString();
                    headerStructure.PurchasingDocumentType = "";
                    headerStructure.PurchasingGroup = "";
                    headerStructure.FuguePurchasingOrderNumber = FuguePurchaseOrderNumber;
                    headerStructure.PurchasingOrganization = "TE";
                    headerStructure.ReasonChange = "";
                    headerStructure.ReleaseGroup = "";
                    headerStructure.ReleaseStatus = "Draft";
                    headerStructure.ReleaseStrategy = "";
                    headerStructure.RequestedBy = "";
                    headerStructure.Statusversion = "";
                    headerStructure.SubmitterEmailID = "";
                    headerStructure.SubmitterName = "";
                    headerStructure.Telephone = "";
                    headerStructure.VendorAccountNumber = "";
                    headerStructure.Version = "";
                    headerStructure.VersionTextField = "";
                    headerStructure.YouReference = "";

                    itemStructure.Actual_Value = "";
                    itemStructure.Assignment_Category = "";
                    itemStructure.Delivery_Date = "";
                    itemStructure.Expected_Value = "7";
                    itemStructure.HeaderStructureID = 0;
                    itemStructure.HSN_Code = (jd["hsn_code"].ToString() != null) ? jd["hsn_code"].ToString() : ""; //
                    itemStructure.Item_Category = "";
                    itemStructure.Item_Number = "";
                    itemStructure.Item_Purchase_Requisition = "";
                    itemStructure.Line_item = "";
                    itemStructure.Long_Text = "";
                    itemStructure.Material_Group = "";
                    itemStructure.Material_Number = (jd["MaterialCode"].ToString() != null) ? jd["MaterialCode"].ToString() : "";//
                    itemStructure.Net_Price = "";
                    itemStructure.No_Limit = "";
                    itemStructure.Order_Qty = "";
                    itemStructure.Overall_limit = "";
                    itemStructure.Overdelivery_Tolerance = "";
                    itemStructure.Plant = "";
                    itemStructure.PurchasingOrderNumber = FuguePurchaseOrderNumber;
                    itemStructure.Requirement_Tracking_Number = "";
                    itemStructure.Requisition_Number = "";
                    itemStructure.Returns_Item = "";
                    itemStructure.Short_Text = "";//
                    itemStructure.Storage_Location = (js["Storage_Location"].ToString() != null) ? js["Storage_Location"].ToString() : "";//
                    itemStructure.Tax_salespurchases_code = "";
                    itemStructure.Underdelivery_Tolerance = "";
                    itemStructure.Unit_Measure = (jd["unit_of_measure"].ToString() != null) ? jd["unit_of_measure"].ToString() : "";//

                    purServ.ActivityNumber = "";
                    purServ.ActualQuantity = "";
                    purServ.Commitment_item = "";
                    purServ.Fund_Center = (js["Fund_Center"].ToString() != null) ? js["Fund_Center"].ToString() : "";//
                    purServ.GrossPrice = "";
                    purServ.HeaderStructureID = 0;
                    purServ.ItemNumber = "";
                    purServ.LineItem = "";
                    purServ.LineItemNumber = "";
                    purServ.Line_Number_INTROW = "";
                    purServ.LongText = "";
                    purServ.NetPrice = "";
                    purServ.OrderQuantity = "";
                    purServ.Package_number = "";
                    purServ.PurchasingOrderNumber = FuguePurchaseOrderNumber;
                    purServ.SAC_Code = "";
                    purServ.Seq_No_Acc_Ass_ESKN = "";
                    purServ.ShortText = (jd["short_description"].ToString() != null) ? jd["short_description"].ToString() : "";//
                    purServ.UnitMeasure = (jd["unit_of_measure"].ToString() != null) ? jd["unit_of_measure"].ToString() : "";//
                    purServ.WBS_Element = (jd["wbs_code"].ToString() != null) ? jd["wbs_code"].ToString() : "";//

                    value.Amount = 1;
                    value.ContextIdentifier = FuguePurchaseOrderNumber;
                    value.CreatedBy = "";
                    value.CreatedOn = System.DateTime.Now;
                    value.Date = System.DateTime.Now;
                    value.IsDeleted = false;
                    value.LastModifiedBy = "";
                    value.LastModifiedOn = System.DateTime.Now; ;
                    value.ModuleName = "";
                    // value.ObjectId = 0;
                    value.OldUniqueId = null;
                    value.PaymentTerm = js["PaymentTerm"].ToString();//
                    value.Percentage = Convert.ToInt32(js["Percentage"].ToString());//
                    value.Remarks = js["Remarks"].ToString();//
                                                             //value.UniqueId = "";

                    purItemCond.ConditionRate = 0;
                    purItemCond.ConditionType = "";
                    purItemCond.HeaderStructureID = 0;
                    purItemCond.ItemNumberPurchase = "";
                    purItemCond.PurchasingOrderNumber = FuguePurchaseOrderNumber;
                    purItemCond.VendorCode = js["VendorCode"].ToString();//



                    var vd = DAl.SavePO(headerStructure, itemStructure, purServ, value, purItemCond);
                    if (vd.StatusCode == HttpStatusCode.OK)
                    {
                        msg = "succcess";
                    }
                }
            }

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(msg)
            };
        }
        [Route("api/PurchaseOrder/GetPurchaseOrderByID/{purchaseOrderNumber}")]
        [HttpGet]
        public HttpResponseMessage GetPurchaseOrderByID(string purchaseOrderNumber)
        {
            var lists = "";
            List<object> list = new List<object>();
            //            TEPurchase_Itemwise
            //TEPurchase_Service
            //TEPurchase_Item_Structure
            //TEPurchase_header_structure
            List<TEPurchase_Itemwise> listTEPurchase_Itemwise = new List<TEPurchase_Itemwise>();
            List<TEPurchase_Service> listTEPurchase_Service = new List<Models.TEPurchase_Service>();
            List<TEPurchase_Item_Structure> listTEPurchase_Item_Structure = new List<Models.TEPurchase_Item_Structure>();
            List<TEPurchase_header_structure> listTEPurchase_header_structure = new List<Models.TEPurchase_header_structure>();
            List<TEVendorPaymentMilestone> listTEVendorPaymentMilestones = new List<TEVendorPaymentMilestone>();
            listTEPurchase_Itemwise = db.TEPurchase_Itemwise.Where(a => a.Purchasing_Order_Number == purchaseOrderNumber && a.IsDeleted == false).ToList();
            list.Add(listTEPurchase_Itemwise);
            listTEPurchase_Service = db.TEPurchase_Service.Where(a => a.Purchasing_Order_Number == purchaseOrderNumber && a.IsDeleted == false).ToList();
            list.Add(listTEPurchase_Service);
            listTEPurchase_Item_Structure = db.TEPurchase_Item_Structure.Where(a => a.Purchasing_Order_Number == purchaseOrderNumber && a.IsDeleted == false).ToList();
            list.Add(listTEPurchase_Item_Structure);
            listTEPurchase_header_structure = db.TEPurchase_header_structure.Where(a => a.Purchasing_Order_Number == purchaseOrderNumber && a.IsDeleted == false).ToList();
            list.Add(listTEPurchase_header_structure);
            listTEVendorPaymentMilestones = db.TEVendorPaymentMilestones.Where(a => a.ContextIdentifier == purchaseOrderNumber && a.IsDeleted == false).ToList();
            list.Add(listTEVendorPaymentMilestones);



            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }
        public HttpResponseMessage EditPurchaseOrder(JObject obj)
        {
            string msg = "";
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(msg)
            };
        }

        //Create Headerstructure
        //, List<TETermsAndCondition> generalList
        [Route("api/PurchaseOrder/SavePurchaseHeaderStructure")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage SavePurchaseHeaderStructure(PurchaseHeaderStructure headerFront)
        {
            int msg = 0;
            try
            {
                if (headerFront == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(msg.ToString())
                    };
                }
                string FuguePurchaseOrderNumber = "Fugue_" + DateTime.Now.ToString("yyyymmdd_hhmmss");
                headerFront.FuguePurchasingOrderNumber = FuguePurchaseOrderNumber;
                headerFront.PurchasingDocumentDate = System.DateTime.Now.ToString();
                msg = DAl.SavePurchaseHeaderStructure(headerFront);
                if (msg > 0)
                {
                    //DAl.SaveTermsAndConditions(generalList, msg);
                }
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(msg.ToString())
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
        [Route("api/PurchaseOrder/SaveTermsAndConditions/{id}")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage SaveTermsAndConditions(List<TETermsAndCondition> generalList, int id)
        {
            string msg = "";
            try
            {
                if (generalList == null || id == 0)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(msg.ToString())
                    };
                }
                if (id > 0)
                {
                    var magg = DAl.SaveTermsAndConditions(generalList, id);
                    if (magg.StatusCode == HttpStatusCode.OK)
                    {
                        msg = "success";
                    }
                }
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(msg.ToString())
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
        [Route("api/PurchaseOrder/SavePurchaseItemStructure/{id}/{companyCode}")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage SavePurchaseItemStructures(List<MaterialDTO> list, int id, int companyCode)
        {
            string Message = "";

            try
            {
                if (list == null || id == 0)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(Message)
                    };
                }
                foreach (var odj in list)
                {
                    PurchaseItemStructure itemStructure = new PurchaseItemStructure();
                    itemStructure.Actual_Value = "";
                    itemStructure.Assignment_Category = "";
                    itemStructure.Delivery_Date = "";
                    itemStructure.Expected_Value = "7";
                    itemStructure.HeaderStructureID = 0;
                    itemStructure.HSN_Code = odj.hsn_code; //
                    itemStructure.Item_Category = "";
                    itemStructure.Item_Number = "";
                    itemStructure.Item_Purchase_Requisition = "";
                    itemStructure.Line_item = "";
                    itemStructure.Long_Text = "";
                    itemStructure.Material_Group = "";
                    itemStructure.Material_Number = odj.MaterialCode;//
                    itemStructure.Net_Price = "";
                    itemStructure.No_Limit = "";
                    itemStructure.Order_Qty = "";
                    itemStructure.Overall_limit = "";
                    itemStructure.Overdelivery_Tolerance = "";
                    itemStructure.Plant = companyCode.ToString();//company Code
                    itemStructure.Status = odj.material_status;
                    itemStructure.Requirement_Tracking_Number = "";
                    itemStructure.Requisition_Number = "";
                    itemStructure.Returns_Item = "";
                    itemStructure.Short_Text = odj.short_description;//
                    itemStructure.Storage_Location = companyCode.ToString();//companyCode
                    itemStructure.Tax_salespurchases_code = "";
                    itemStructure.Underdelivery_Tolerance = "";
                    itemStructure.Unit_Measure = odj.unit_of_measure;//



                    Message = DAl.SavePurchaseItemStructure(itemStructure, id).ToString();
                }
                if (Convert.ToInt32(Message) > 0)
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
        [Route("api/PurchaseOrder/SavePurchaseMilestones")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage SavePurchaseMilestones(TEVendorPaymentMilestone List)
        {
            string Message = "";
            var md = 0;
            try
            {
                if (List == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(Message = "no content")
                    };
                }
                //  foreach(var obj in List) { 
                md = DAl.PostPOMilestones(List);
                // }
                if (md > 0)
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


        [Route("api/PurchaseOrder/SaveSpecialTermsAndConditions/{id}")]
        [HttpPost, HttpOptions]
        public HttpResponseMessage SaveSpecialTermsAndConditions(List<TETermsAndCondition> speciailList, int id)
        {
            string msg = "";
            try
            {
                if (speciailList == null || id == 0)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(msg.ToString())
                    };
                }
                if (id > 0)
                {
                    var magg = DAl.SaveSpecialTermsAndConditions(speciailList, id);
                    if (magg.StatusCode == HttpStatusCode.OK)
                    {
                        msg = "success";
                    }
                }
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(msg.ToString())
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

        //Update Header Structure
        [Route("api/PurchaseOrder/UpdatePurchaseHeaderStructure")]
        [HttpPost]
        public HttpResponseMessage UpdatePurchaseHeaderStructure(TEPurchase_header_structure headerFront)
        {
            int msg = 0;
            try
            {
                if (headerFront == null)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Content = new StringContent(msg.ToString())
                    };
                }
                var back = db.TEPurchase_header_structure.Where(a => a.Uniqueid == headerFront.Uniqueid).SingleOrDefault();
                headerFront.CreatedBy = back.CreatedBy;
                headerFront.CreatedOn = back.CreatedOn;
                headerFront.LastModifiedOn = DateTime.Now.ToString();
                db.Entry(back).CurrentValues.SetValues(headerFront);


                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(msg.ToString())
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





//if (c["key"].ToString() == "specification_sheet")
//                                   {
//                                       if (c["value"] != null)
//                                           foreach (JToken im in c["value"])
//                                           {
//                                               if (im.Name =="id")
//                                               {
//                                                   specid = im.Value.ToString();
//                                                   list.Add(specid);
//                                               }


//                                           }

//                                   }