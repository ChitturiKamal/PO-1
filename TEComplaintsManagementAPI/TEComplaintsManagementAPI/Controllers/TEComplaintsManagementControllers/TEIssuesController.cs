using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Transactions;
using TEComplaintsManagementAPI.Models;
using TECommonEntityLayer;
using System.Web;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using TECommonLogicLayer.TEModelling;
using TECommonDTO;
using TEComplaintsManagementAPI.Constant;
using TEComplaintsManagementAPI.Services;
using TEComplaintsManagementAPI.Models.EmailModels;
using TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEIssuesController : ApiController
    {
        // GET api/<controller>
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        EmailService EmailMgr = new EmailService();
        TEIssuesDispositionMgrController dispo = new TEIssuesDispositionMgrController();

        [HttpGet]
        public object GetIssuescountbyuserid(int userid)
        {
            TEIssueCountModel result = new TEIssueCountModel();
            int cnt = db.TEIssues.Where(x => (x.RaisedBy == userid)
                   && (new[] { "ACCEPTED", "COMMENCED", "COST APPROVAL", "INQUEUE", "PAUSED", "QUEUED", "REOPEN", "RESUMED" }.Contains(x.Status))
                   )
                   .Count();
            //.GroupBy(x => x.Status).Select(g => new { g.Key, Count = g.Count() })

            return new { issuecount = cnt };

        }
        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesbyQueueId(int QueueID)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false) && (x.QueueID == QueueID))
                                .OrderByDescending(x => x.LastModifiedOn).ToList();
            foreach (var item in listOfEntity)
            {
                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {

                    //if (tempModel.RaisedBy + "".Length > 0)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (tempModel.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }
                    if (tempModel.QueueID != null)
                    {
                        int temQue = Convert.ToInt32(item.QueueID);
                        TEQueue temqueue = db.TEQueues.Find(temQue);
                        if (temqueue != null)
                            tempModel.Queuename = temqueue.QueueName;
                    }

                    //if (tempModel.AssignedTo != null)
                    //{
                    //int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    //TEEmpBasicInfo tempBasicInfo = db.TEEmpBasicInfoes.Find(tempAssignedTo);
                    //if (tempBasicInfo != null)
                    //    tempModel.AssignToName = tempBasicInfo.FirstName + " " + tempBasicInfo.LastName;
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (tempModel.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }

                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }

                result.Add(tempModel);
            }

            //Customization


            return result;
        }

        [HttpGet]
        public IEnumerable<TEqueueModel> GetqueueTechnicain(int AssignedTo)
        {
            List<TEqueueModel> result = new List<TEqueueModel>();
            try
            {
                var query = from i in db.TEIssues
                            join d in db.TEQueueDepartments on i.QueueID equals d.QueueID
                            join p in db.TEQueues on i.QueueID equals p.QueueID
                            where (i.AssignedTo == AssignedTo)
                            group new { i, p } by new { p.QueueName, p.Uniqueid } into g
                            orderby g.Key.QueueName descending
                            select new
                            {
                                g.Key.QueueName,

                                //g.Key.Status,
                                Count = g.Select(x => x.i.Uniqueid).Distinct().Count(),
                                INQUE = g.Count(x => x.i.Status == "INQUE"),
                                ASSIGNED = g.Count(x => x.i.Status == "ASSIGNED"),
                                ACCEPTED = g.Count(x => x.i.Status == "ACCEPTED"),
                                COMMENCED = g.Count(x => x.i.Status == "COMMENCED"),
                                PAUSED = g.Count(x => x.i.Status == "PAUSED"),
                                COSTAPPROVAL = g.Count(x => x.i.Status == "COSTAPPROVAL"),
                                RESUMED = g.Count(x => x.i.Status == "RESUMED"),
                                RESOLVED = g.Count(x => x.i.Status == "RESOLVED"),
                                CLOSED = g.Count(x => x.i.Status == "CLOSED"),
                                REOPEN = g.Count(x => x.i.Status == "REOPEN"),
                                Uniqueid = g.Key.Uniqueid
                            };

                foreach (var item in query)
                {
                    result.Add(new TEqueueModel()
                    {
                        Uniqueid = item.Uniqueid,
                        QueueName = item.QueueName,
                        //Status = item.Status,
                        QueueCount = item.Count,
                        INQUE = item.INQUE,
                        ASSIGNED = item.ASSIGNED,
                        ACCEPTED = item.ACCEPTED,
                        COMMENCED = item.COMMENCED,
                        PAUSED = item.PAUSED,
                        COSTAPPROVAL = item.COSTAPPROVAL,
                        RESUMED = item.RESUMED,
                        RESOLVED = item.RESOLVED,
                        CLOSED = item.CLOSED,
                        REOPEN = item.REOPEN
                    });
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEQUEUETechnician API | TEQUEUETechnician | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }
        [HttpGet]
        public IEnumerable<TEqueueModel> Getqueueemployee(int RaisedBy)
        {
            List<TEqueueModel> result = new List<TEqueueModel>();
            try
            {
                var query = from i in db.TEIssues
                            join d in db.TEQueueDepartments on i.QueueID equals d.Uniqueid
                            join p in db.TEQueues on i.QueueID equals p.Uniqueid
                            where (i.RaisedBy == RaisedBy)
                            group new { i, p } by new { p.QueueName, p.Uniqueid } into g
                            orderby g.Key.QueueName descending
                            select new
                            {
                                g.Key.QueueName,

                                //g.Key.Status,
                                Count = g.Select(x => x.i.Uniqueid).Distinct().Count(),
                                INQUE = g.Count(x => x.i.Status == "INQUE"),
                                ASSIGNED = g.Count(x => x.i.Status == "ASSIGNED"),
                                ACCEPTED = g.Count(x => x.i.Status == "ACCEPTED"),
                                COMMENCED = g.Count(x => x.i.Status == "COMMENCED"),
                                PAUSED = g.Count(x => x.i.Status == "PAUSED"),
                                COSTAPPROVAL = g.Count(x => x.i.Status == "COSTAPPROVAL"),
                                RESUMED = g.Count(x => x.i.Status == "RESUMED"),
                                RESOLVED = g.Count(x => x.i.Status == "RESOLVED"),
                                CLOSED = g.Count(x => x.i.Status == "CLOSED"),
                                REOPEN = g.Count(x => x.i.Status == "REOPEN"),
                                Uniqueid = g.Key.Uniqueid
                            };

                foreach (var item in query)
                {
                    result.Add(new TEqueueModel()
                    {
                        Uniqueid = item.Uniqueid,
                        QueueName = item.QueueName,
                        //Status = item.Status,
                        QueueCount = item.Count,
                        INQUE = item.INQUE,
                        ASSIGNED = item.ASSIGNED,
                        ACCEPTED = item.ACCEPTED,
                        COMMENCED = item.COMMENCED,
                        PAUSED = item.PAUSED,
                        COSTAPPROVAL = item.COSTAPPROVAL,
                        RESUMED = item.RESUMED,
                        RESOLVED = item.RESOLVED,
                        CLOSED = item.CLOSED,
                        REOPEN = item.REOPEN
                    });
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEQUEUETechnician API | TEQUEUETechnician | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }
        [HttpGet]
        public IEnumerable<TEqueueModel> Getqueue()
        {
            List<TEqueueModel> result = new List<TEqueueModel>();
            try
            {
                var query = from i in db.TEIssues
                            join d in db.TEQueueDepartments on i.QueueID equals d.Uniqueid
                            join p in db.TEQueues on i.QueueID equals p.Uniqueid
                            group new { i, p } by new { p.QueueName } into g
                            orderby g.Key.QueueName descending
                            select new
                            {
                                g.Key.QueueName,
                                //g.Key.Status,
                                Count = g.Select(x => x.i.Uniqueid).Distinct().Count(),
                                INQUE = g.Count(x => x.i.Status == "INQUE"),
                                ASSIGNED = g.Count(x => x.i.Status == "ASSIGNED"),
                                ACCEPTED = g.Count(x => x.i.Status == "ACCEPTED"),
                                COMMENCED = g.Count(x => x.i.Status == "COMMENCED"),
                                PAUSED = g.Count(x => x.i.Status == "PAUSED"),
                                COSTAPPROVAL = g.Count(x => x.i.Status == "COSTAPPROVAL"),
                                RESUMED = g.Count(x => x.i.Status == "RESUMED"),
                                RESOLVED = g.Count(x => x.i.Status == "RESOLVED"),
                                CLOSED = g.Count(x => x.i.Status == "CLOSED"),
                                REOPEN = g.Count(x => x.i.Status == "REOPEN")
                            };

                foreach (var item in query)
                {
                    result.Add(new TEqueueModel()
                    {
                        QueueName = item.QueueName,
                        //Status = item.Status,
                        QueueCount = item.Count,
                        INQUE = item.INQUE,
                        ASSIGNED = item.ASSIGNED,
                        ACCEPTED = item.ACCEPTED,
                        COMMENCED = item.COMMENCED,
                        PAUSED = item.PAUSED,
                        COSTAPPROVAL = item.COSTAPPROVAL,
                        RESUMED = item.RESUMED,
                        RESOLVED = item.RESOLVED,
                        CLOSED = item.CLOSED,
                        REOPEN = item.REOPEN
                    });
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEQUEUE API | TEQUEUE | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }
        [HttpGet]
        public IEnumerable<TEqueueModel> GetCategory()
        {
            List<TEqueueModel> result = new List<TEqueueModel>();
            try
            {
                var query = from i in db.TEIssues
                            join p in db.TECategories on i.CategoryID equals p.Uniqueid
                            group new { i, p } by new { p.Name } into g
                            orderby g.Key.Name descending
                            select new
                            {
                                //g.Key.QueueID,
                                g.Key.Name,
                                //g.Key.Status,
                                Count = g.Select(x => x.i.Uniqueid).Distinct().Count(),
                                INQUE = g.Count(x => x.i.Status == "INQUE"),
                                ASSIGNED = g.Count(x => x.i.Status == "ASSIGNED"),
                                ACCEPTED = g.Count(x => x.i.Status == "ACCEPTED"),
                                COMMENCED = g.Count(x => x.i.Status == "COMMENCED"),
                                PAUSED = g.Count(x => x.i.Status == "PAUSED"),
                                COSTAPPROVAL = g.Count(x => x.i.Status == "COSTAPPROVAL"),
                                RESUMED = g.Count(x => x.i.Status == "RESUMED"),
                                RESOLVED = g.Count(x => x.i.Status == "RESOLVED"),
                                CLOSED = g.Count(x => x.i.Status == "CLOSED"),
                                REOPEN = g.Count(x => x.i.Status == "REOPEN")
                            };

                foreach (var item in query)
                {
                    result.Add(new TEqueueModel()
                    {
                        //Queueid = item.QueueID,
                        CategoryName = item.Name,
                        //Status = item.Status,
                        CategoryCount = item.Count,
                        INQUE = item.INQUE,
                        ASSIGNED = item.ASSIGNED,
                        ACCEPTED = item.ACCEPTED,
                        COMMENCED = item.COMMENCED,
                        PAUSED = item.PAUSED,
                        COSTAPPROVAL = item.COSTAPPROVAL,
                        RESUMED = item.RESUMED,
                        RESOLVED = item.RESOLVED,
                        CLOSED = item.CLOSED,
                        REOPEN = item.REOPEN
                    });
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEImportantContacts API | TEImportantContacts | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }
        [HttpGet]
        public IEnumerable<TEqueueModel> GetCategoryTechnicain(int AssignedTo)
        {
            List<TEqueueModel> result = new List<TEqueueModel>();
            try
            {
                var dte = System.DateTime.Now;

                var query = from i in db.TEIssues
                            join p in db.TECategories on i.CategoryID equals p.Uniqueid
                            where (i.AssignedTo == AssignedTo)
                            group new { i, p } by new { p.Name } into g
                            orderby g.Key.Name descending
                            select new
                            {
                                g.Key.Name,
                                //g.Key.Status,
                                Count = g.Select(x => x.i.Uniqueid).Distinct().Count(),
                                INQUE = g.Count(x => x.i.Status == "INQUE"),
                                ASSIGNED = g.Count(x => x.i.Status == "ASSIGNED"),
                                ACCEPTED = g.Count(x => x.i.Status == "ACCEPTED"),
                                COMMENCED = g.Count(x => x.i.Status == "COMMENCED"),
                                PAUSED = g.Count(x => x.i.Status == "PAUSED"),
                                COSTAPPROVAL = g.Count(x => x.i.Status == "COSTAPPROVAL"),
                                RESUMED = g.Count(x => x.i.Status == "RESUMED"),
                                RESOLVED = g.Count(x => x.i.Status == "RESOLVED"),
                                CLOSED = g.Count(x => x.i.Status == "CLOSED"),
                                REOPEN = g.Count(x => x.i.Status == "REOPEN")
                            };

                foreach (var item in query)
                {
                    result.Add(new TEqueueModel()
                    {
                        //Queueid = item.QueueID,
                        CategoryName = item.Name,
                        //Status = item.Status,
                        CategoryCount = item.Count,
                        INQUE = item.INQUE,
                        ASSIGNED = item.ASSIGNED,
                        ACCEPTED = item.ACCEPTED,
                        COMMENCED = item.COMMENCED,
                        PAUSED = item.PAUSED,
                        COSTAPPROVAL = item.COSTAPPROVAL,
                        RESUMED = item.RESUMED,
                        RESOLVED = item.RESOLVED,
                        CLOSED = item.CLOSED,
                        REOPEN = item.REOPEN
                    });
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TECategory API | TECategory | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }

        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssues()
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            var listOfEntity = (from iss in db.TEIssues
                                join usr in db.UserProfiles on iss.RaisedBy equals usr.UserId
                                join cat in db.TECategories on iss.CategoryID equals cat.Uniqueid
                                join que in db.TEQueues on iss.QueueID equals que.Uniqueid
                                //join proj in db.TEProjects on iss.ProjectID equals proj.Uniqueid
                                where (iss.IsDeleted == false)
                                //orderby iss.Uniqueid descending
                                orderby iss.Priority, iss.Uniqueid descending
                                //orderby iss.CreatedOn descending
                                select new
                                {
                                    RaisedByName = usr.CallName == null ? usr.UserName : usr.CallName,
                                    Categoryname = cat.Name,
                                    Queuename = que.QueueName,
                                    // ProjectName = proj.ProjectName,
                                    // ProjectCode = proj.ProjectCode,
                                    iss.CreatedOn,
                                    iss.CreatedBy,
                                    iss.LastModifiedOn,
                                    iss.LastModifiedBy,
                                    iss.IsDeleted,
                                    iss.Uniqueid,
                                    iss.ISSUEID,
                                    iss.UNITONBOARDINGID,
                                    iss.Subject,
                                    iss.Descritpion,
                                    iss.Summary,
                                    iss.CategoryID,
                                    iss.ParentCatID,
                                    iss.QueueID,
                                    iss.Closed_Date,
                                    iss.Priority,
                                    iss.Status,
                                    iss.Stage,
                                    iss.AssignedTo,
                                    iss.ClosedBy,
                                    iss.RaisedBy,
                                    iss.Attachments,
                                    iss.Estimated_Close_date,
                                    iss.Actual_close_date,
                                    iss.Assigned,
                                    iss.Start,
                                    iss.Pasue,
                                    iss.Resolved_Date,
                                    iss.ProjectID,
                                    iss.UnitID,
                                    iss.AREATYPE,
                                    iss.Scheduled_Date,
                                    iss.RateUS,
                                    iss.COSTINVOLED,
                                    iss.EstimateCOST,
                                    iss.ActualCost,
                                    iss.LaborCost,
                                    iss.MaterialCost,
                                    iss.ApprovedBy,
                                    iss.ApprovedOn,
                                    iss.AvailableStart,
                                    iss.AvailableEnd,
                                    iss.Feedback,
                                    iss.FeedbackTechnician,
                                    iss.PrefferedTime,
                                    iss.TechnicianRate,
                                    iss.Reopen,
                                    iss.ReopenDate,
                                    iss.Worklocation,
                                    iss.DismissedReason,
                                    iss.ReopenReason,
                                    iss.PrimaryAssigne

                                }).ToList();
            foreach (var item in listOfEntity)
            {
                TEComplainceModel tempModel = new TEComplainceModel();
                TETransformEntityNModel translator = new TETransformEntityNModel();
                tempModel = translator.TransformAtoB(item, tempModel);
                try
                {
                    ////if (tempModel.RaisedBy + "".Length > 0)
                    ////{
                    //int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    //UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    //if (tempraised != null)
                    //    tempModel.RaisedByName = tempraised.CallName;
                    ////}

                    //if (tempModel.CategoryID != null)
                    //{
                    //    int temcat = Convert.ToInt32(item.CategoryID);
                    //    TECategory temcategry = db.TECategories.Find(temcat);
                    //    if (temcategry != null)
                    //        tempModel.Categoryname = temcategry.Name;
                    //}
                    //if (tempModel.QueueID != null)
                    //{
                    //    int temQue = Convert.ToInt32(item.QueueID);
                    //    TEQueue temqueue = db.TEQueues.Find(temQue);
                    //    if (temqueue != null)
                    //        tempModel.Queuename = temqueue.QueueName;
                    //}

                    //if (tempModel.AssignedTo != null)
                    //{
                    //int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    //TEEmpBasicInfo tempBasicInfo = db.TEEmpBasicInfoes.Find(tempAssignedTo);
                    //if (tempBasicInfo != null)
                    //    tempModel.AssignToName = tempBasicInfo.FirstName + " " + tempBasicInfo.LastName;

                    //int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    //UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    //if (tempuser != null)
                    //    tempModel.AssignToName = tempuser.CallName;
                    //}
                    tempModel.ParentCategoryName = (from cat in db.TECategories
                                                    where (cat.Uniqueid == item.ParentCatID)
                                                    select cat.Name).FirstOrDefault();

                    tempModel.AssignToName = (from usr in db.UserProfiles
                                              where (usr.UserId == item.AssignedTo)
                                              select usr.CallName == null ? usr.UserName : usr.CallName).FirstOrDefault();
                    tempModel.Disposition = (from usr in db.TEIssuesDispositions
                                             where (usr.IsDeleted == false && usr.TEIssueId == item.Uniqueid)
                                             select usr.Comment == null ? usr.Comment : usr.Comment).FirstOrDefault();
                    if (tempModel.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempproject = db.TEProjects.Find(Teproject);
                        if (tempproject != null)
                        {
                            tempModel.ProjectName = tempproject.ProjectName;
                            tempModel.Projectcode = tempproject.ProjectCode;
                        }
                        int TEUnit = Convert.ToInt32(item.UnitID);
                        TEUnit tempunit = db.TEUnits.Find(TEUnit);
                        if (tempunit != null)
                        {
                            tempModel.UnitNumber = tempunit.UnitNumber;
                        }
                    }
                    //tempModel.Priority = (from pick in db.TEPickListItems  
                    //                        where (pick.Uniqueid==Convert.ToInt32(item.Priority))
                    //                        select pick.Description ).FirstOrDefault();
                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                    if (item.PrimaryAssigne != null)
                    {
                        tempModel.primaryAssigne = item.PrimaryAssigne.Value;
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {                      
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }

                result.Add(tempModel);
            }

            //Customization


            return result;
        }


        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesList_old()
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => x.IsDeleted == false)
                                .OrderByDescending(x => x.LastModifiedOn).ToList();
            foreach (var item in listOfEntity)
            {
                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {

                    //if (tempModel.RaisedBy + "".Length > 0)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (tempModel.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }
                    if (tempModel.QueueID != null)
                    {
                        int temQue = Convert.ToInt32(item.QueueID);
                        TEQueue temqueue = db.TEQueues.Find(temQue);
                        if (temqueue != null)
                            tempModel.Queuename = temqueue.QueueName;
                    }

                    //if (tempModel.AssignedTo != null)
                    //{
                    //int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    //TEEmpBasicInfo tempBasicInfo = db.TEEmpBasicInfoes.Find(tempAssignedTo);
                    //if (tempBasicInfo != null)
                    //    tempModel.AssignToName = tempBasicInfo.FirstName + " " + tempBasicInfo.LastName;
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (tempModel.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }

                result.Add(tempModel);
            }

            //Customization


            return result;
        }

        [HttpGet]
        public TEIssuesModel GetIssuesOrderBy(Models.Enums.TEIssueOrderByEnums orderBy, int pagecount, string filterBy = "")
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEIssuesModel result = new TEIssuesModel();
            IEnumerable<TEComplainceModel> teissueresult = GetIssues().ToList();

            #region Orderby Category
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Category)
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                         .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    // int filter = Convert.ToInt32(filterBy);
                    string filter = filterBy;
                    //result.Issues = GetIssues().OrderBy(x => x.Categoryname).Where(x => x.Categoryname == filter).Skip((pagecount - 1) * 5).Take(5);
                    result.Issues = teissueresult.Where(x => x.Categoryname == filter)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    if (result.KeyNCount == null)
                    {
                        var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                         && (x.Name == filter))
                .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                        result.KeyNCount = teissueresult.Where(x => x.ParentCatID == listOfEntity.Uniqueid)
                             .GroupBy(x => x.Categoryname).Select(g => new { g.Key, Count = g.Count() });
                    }

                    return result;
                }

                //else 
                //{
                //    result.Issues = GetIssues().OrderBy(x => x.Categoryname);
                //    return result;
                //}

                result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Person
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Person)
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.AssignToName)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    // int filter = Convert.ToInt32(filterBy);
                    string filter = filterBy;
                    result.Issues = teissueresult.Where(x => x.AssignToName == filter)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    if (filterBy.IndexOf(".", StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        if (result.KeyNCount == null)
                        {
                            var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                             && (x.Name == filter))
                    .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                            result.KeyNCount = teissueresult.OrderBy(x => x.AssignToName).Where(x => x.ParentCatID == listOfEntity.Uniqueid)
                                 .GroupBy(x => x.AssignToName).Select(g => new { g.Key, Count = g.Count() });
                        }
                    }

                    return result;
                }
                //else
                //{
                //    result.Issues = GetIssues().OrderBy(x => x.AssignToName);
                //    return result;
                //}

                result.KeyNCount = teissueresult.GroupBy(n => n.AssignToName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion

            #region Orderby Assign To name i.e. Raised
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Raised)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    try
                    {
                        int raisedby = Convert.ToInt32(filterBy);
                        result.Issues = teissueresult.Where(x => x.RaisedBy == raisedby)
                            //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                            .OrderByDescending(x => x.CreatedOn)
                            .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    }
                    catch (Exception exra)
                    {
                        result.Issues = teissueresult.Where(x => x.RaisedByName == filterBy)
                            //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                            .OrderByDescending(x => x.CreatedOn)
                            .Skip((pagecount - 1) * 5).Take(5);
                        if (filterBy.IndexOf(".", StringComparison.OrdinalIgnoreCase) <= 0)
                        {
                            if (result.KeyNCount == null)
                            {
                                var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                                 && (x.Name == filterBy))
                        .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                                result.KeyNCount = teissueresult.OrderBy(x => x.RaisedByName).Where(x => x.ParentCatID == listOfEntity.Uniqueid)
                                     .GroupBy(x => x.RaisedByName).Select(g => new { g.Key, Count = g.Count() });
                            }
                        }
                    }
                    return result;
                }
                else
                {
                    //result.Issues = teissueresult.OrderBy(x => x.RaisedByName);
                    result.Issues = teissueresult.OrderByDescending(x => x.CreatedOn)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                           .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });


                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Assigned
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Assigned)
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                    .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    int AssignedTo = Convert.ToInt32(filterBy);
                    result.Issues = teissueresult.Where(x => x.AssignedTo == AssignedTo)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                            .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }
                else
                {
                    //result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    result.Issues = teissueresult.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                       .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion

            #region Orderby Assign To name i.e. primaryAssigne
            if (orderBy == Models.Enums.TEIssueOrderByEnums.primaryAssigne)
            {
                //teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                 if (filterBy.Length > 0)
                {
                    int primaryAssigne = Convert.ToInt32(filterBy);
                    result.Issues = teissueresult.Where(x => x.primaryAssigne == primaryAssigne)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                            .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }
                else
                {
                    //result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    result.Issues = teissueresult.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                       .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }
                return result;
            }
            #endregion

            #region Orderby Manager
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Manager)
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                if ((filterBy == null) || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    // int filter = Convert.ToInt32(filterBy);
                    //  result.Issues = GetIssues().OrderBy(x => x.Categoryname).Where(x => x.ParentCatID == filter);

                    // result.Issues = GetIssues().OrderBy(x => x.Categoryname).Where(x => x.ParentCategoryName == filterBy);
                    result.Issues = teissueresult.Where(x => x.ParentCategoryName == filterBy)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    //   var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                    //                                         && (x.Uniqueid == filterBy))
                    //.OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                    //result.Issues = GetIssues().OrderBy(x => x.Categoryname).Where(x => x.ParentCatID == listOfEntity.Uniqueid);
                    return result;
                }
                else
                {
                    // result.Issues =teissueresult.OrderBy(x => x.Categoryname);
                    result.Issues = teissueresult.OrderByDescending(x => x.CreatedOn)
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                         .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Queue
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Queue)
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                                    .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    result.Issues = teissueresult.OrderBy(x => x.Queuename).Where(x => x.Queuename == filterBy).OrderByDescending(x => x.CreatedOn).Skip((pagecount - 1) * 5).Take(5);
                    return result;
                }
                else
                {
                    // result.Issues = teissueresult.OrderBy(x => x.Queuename);
                    result.Issues = teissueresult
                        //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Order by Uniqueid
            else
            {
                teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                result.Issues = teissueresult.OrderByDescending(x => x.CreatedOn)
                    //.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                return result;
            }
            #endregion


        }

        [HttpGet]
        public TEIssuesModel GetIssuesFilterByPaging(Models.Enums.TEIssueOrderByEnums orderBy, string SelectedBy, int pagecount, string filterBy = "")
        {
            TEIssuesModel result = new TEIssuesModel();
            IEnumerable<TEComplainceModel> teissueresult = GetIssues().ToList();
            string assigned = "false";
            string[] selection = SelectedBy.Split('$');
            string[] selectionsub = SelectedBy.Split('$');
            for (int i = 0; i <= selection.Length - 1; i++)
            {
                selectionsub = selection[i].Split(':');
                if (selectionsub[0] == "Raised")
                {
                    try
                    {
                        int RaisedBy = Convert.ToInt32(selectionsub[1]);
                        teissueresult = teissueresult.Where(x => (x.RaisedBy == RaisedBy));
                    }
                    catch (Exception exr) { teissueresult = teissueresult.Where(x => (x.RaisedByName == selectionsub[1])); }
                }
                if (selectionsub[0] == "Assigned")
                {
                    try
                    {
                        assigned = "true";
                        int Assig = Convert.ToInt32(selectionsub[1]);
                        teissueresult = teissueresult.Where(x => (x.AssignedTo == Assig));
                    }
                    catch (Exception exr) { teissueresult = teissueresult.Where(x => (x.AssignToName == selectionsub[1])); }
                }
                if (selectionsub[0] == "Queue")
                {
                    int que = Convert.ToInt32(selectionsub[1]);
                    teissueresult = teissueresult.Where(x => x.QueueID == que);
                }
                if (selectionsub[0] == "Category")
                {
                    string Category = selectionsub[1];
                    teissueresult = teissueresult.Where(x => (x.ParentCategoryName == Category));
                }
                if (selectionsub[0] == "Status")
                {
                    string Status = selectionsub[1];
                    teissueresult = teissueresult.Where(x => x.Status == Status);
                }
                if (selectionsub[0] == "priority")
                {
                    string priority = selectionsub[1];
                    teissueresult = teissueresult.Where(x => x.PriorityName == priority);
                }
                if (selectionsub[0] == "prefferedtime")
                {
                    string prefferedtime = selectionsub[1];
                    teissueresult = teissueresult.Where(x => x.PrefferedTime == prefferedtime);
                }
            }
            // teissueresult = teissueresult.Where(x => x.AssignedTo == AssignedTo);
            #region Orderby Category
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Category)
            {
                //result.KeyNCount = GetIssues().Where(x => x.AssignedTo == AssignedTo)
                //       .GroupBy(x => x.Categoryname)
                //        .Select(g => new { g.Key, Count = g.Count() });
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.Categoryname)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                          .GroupBy(x => x.Categoryname)
                           .Select(g => new { g.Key, Count = g.Count() });
                }

                return result;
            }
            #endregion
            #region Orderby priority
            if (orderBy == Models.Enums.TEIssueOrderByEnums.priority)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.PriorityName)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                         .GroupBy(x => x.PriorityName)
                          .Select(g => new { g.Key, Count = g.Count() });
                }

                return result;
            }
            #endregion
            #region Orderby Status
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Status)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.Status)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                          .GroupBy(x => x.Status)
                           .Select(g => new { g.Key, Count = g.Count() });
                }
                return result;
            }
            #endregion
            #region Orderby prefferedtime
            if (orderBy == Models.Enums.TEIssueOrderByEnums.prefferedtime)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.PrefferedTime)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult
                           .GroupBy(x => x.PrefferedTime)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                return result;
            }
            #endregion

            #region Orderby Assign To name i.e. Person
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Person)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.AssignToName)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    result.Issues = teissueresult.OrderBy(x => x.AssignToName).Where(x => x.AssignToName == filterBy);
                    if (result.KeyNCount == null)
                    {

                        var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                          && (x.Name == filterBy))
                 .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                        result.KeyNCount = teissueresult.OrderBy(x => x.AssignToName).Where(x => x.ParentCatID == listOfEntity.Uniqueid)
                             .GroupBy(n => n.AssignToName).Select(g => new { g.Key, Count = g.Count() });
                    }
                    return result;
                }
                else
                {
                    result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.AssignToName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion


            #region Orderby Assign To name i.e. Raised
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Raised)
            {
                if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    int raisedby = Convert.ToInt32(filterBy);
                    result.Issues = teissueresult.OrderBy(x => x.RaisedByName).Where(x => x.RaisedBy == raisedby);
                    return result;
                }
                else
                {
                    result.Issues = teissueresult.OrderBy(x => x.RaisedByName);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Assigned
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Assigned)
            {
                if ((filterBy == null) || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.AssignToName)
                    .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    //int AssignedTo = Convert.ToInt32(filterBy);
                    //result.Issues = GetIssues().OrderBy(x => x.AssignToName).Where(x => x.AssignedTo == AssignedTo);
                    result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    return result;
                }
                else
                {
                    result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Manager
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Manager)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                          && (x.Name == filterBy))
                 .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                    result.Issues = teissueresult.OrderBy(x => x.Categoryname).Where(x => x.ParentCatID == listOfEntity.Uniqueid);
                    return result;
                }
                else
                {
                    result.Issues = teissueresult.OrderBy(x => x.Categoryname);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(x => x.Categoryname)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Queue
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Queue)
            {
                if ((filterBy == null) || (filterBy == ""))
                {
                    result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
       .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    result.Issues = teissueresult.OrderBy(x => x.Queuename).Where(x => x.Queuename == filterBy);
                    return result;
                }
                else
                {
                    result.Issues = teissueresult.OrderBy(x => x.Queuename);
                    return result;
                }

                result.KeyNCount = teissueresult.GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Order by Filter
            else
            {
                string[] filtersplit = filterBy.Split('$');
                string[] filtersub;
                string[] priority = new string[20];
                string[] Status = new string[20];
                string[] Raised = new string[20];
                string[] Assigned = new string[20];
                string[] Category = new string[100];
                string[] prefferedtime = new string[20];
                string[] Date = new string[2];
                DateTime Datefrom = System.DateTime.Now;
                DateTime Dateto = System.DateTime.Now;
                for (int i = 0; i <= filtersplit.Length - 1; i++)
                {
                    filtersub = filtersplit[i].Split(':');

                    if (filtersub[0] == "Priority")
                    {
                        // priority = filtersub[1];
                        priority = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Status")
                    {
                        //Status = filtersub[1];
                        Status = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Raised")
                    {
                        //Status = filtersub[1];
                        Raised = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Assigned")
                    {
                        //Status = filtersub[1];
                        Assigned = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Category")
                    {
                        // Category = filtersub[1];
                        Category = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "prefferedtime")
                    {
                        //prefferedtime = filtersub[1];
                        prefferedtime = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Date")
                    {
                        // Date = filtersub[1];
                        Date = filtersub[1].Split(',');
                        if (filtersub[1].ToString() == "Today")
                        {
                            Datefrom = System.DateTime.Today;
                            //Dateto = System.DateTime.Today.AddDays(-1);
                        }
                        else if (filtersub[1].ToString() == "Yesterday")
                        {
                            Datefrom = System.DateTime.Today.AddDays(-1);
                            //Dateto = System.DateTime.Today.AddDays(-1);
                        }
                        else if (filtersub[1].ToString() == "LastWeek")
                        {
                            Datefrom = System.DateTime.Today.AddDays(-7);
                            //Dateto = System.DateTime.Now;
                        }
                        else if (filtersub[1].ToString() == "Last2Week")
                        {
                            Datefrom = System.DateTime.Today.AddDays(-14);
                            //Dateto = System.DateTime.Now;
                        }
                        else if (filtersub[1].ToString() == "Last30Days")
                        {
                            Datefrom = System.DateTime.Today.AddMonths(-1);
                            //Dateto = System.DateTime.Now;
                        }
                    }
                }

                // result.Issues = teissueresult;

                if (priority[0] != null)
                {
                    if (priority[1] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x=>!IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => //(x.AssignedTo == AssignedTo) &&
                                      (priority.Contains(x.PriorityName))
                            // && (Category.Contains(x.Categoryname))
                            //  && (Status.Contains(x.Status))
                            //&& (prefferedtime.Contains(x.PrefferedTime))
                                    );
                        // .OrderBy(x => new { x.Priority, x.Uniqueid });
                        //teissueresult = result.Issues;
                    }
                }
                if (Status[0] != null)
                {
                    if (Status[0] != "ALL")
                    {
                        teissueresult = teissueresult.Where(x => Status.Contains(x.Status));
                    }
                }
                if (Assigned[0] != null)
                {
                    if (Assigned[0] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => Assigned.Contains(x.AssignToName));
                    }
                }
                if (Raised[0] != null)
                {
                    if (Raised[0] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => Raised.Contains(x.RaisedByName));
                    }
                }
                if(Category[0] != null)
                {
                    if (Category[0] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => Category.Contains(x.Categoryname));
                    }
                }
                if (prefferedtime[0] != null)
                {
                    if (prefferedtime[0] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => prefferedtime.Contains(x.PrefferedTime));
                    }
                }
                if (Date[0] != null)
                {
                    if (Date[0] == "ALL")
                    {
                        teissueresult = teissueresult.Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
                    }
                    else
                    {
                        teissueresult = teissueresult.Where(x => x.CreatedOn >= Datefrom);
                    }
                }
                result.Issues = teissueresult.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid)
                .Skip((pagecount - 1) * IssuesStatus.IssuesCount).Take(IssuesStatus.IssuesCount);
                return result;
            }
            #endregion
        }

        [HttpGet]
        public TEIssuesModel GetIssuesFilterBy(Models.Enums.TEIssueOrderByEnums orderBy, string AssignedTo, string filterBy = "")
        {
            TEIssuesModel result = new TEIssuesModel();
            IEnumerable<TEComplainceModel> teissueresult ;
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Status)
            {
                teissueresult = GetIssues().ToList();
            }
            else
            {
                teissueresult = GetIssues().Where(x => !IssuesStatus.IssuesStatusList.Contains(x.Status)).ToList();
            }
            string assigned = "false";
            string[] selection = AssignedTo.Split('$');
            string[] selectionsub = AssignedTo.Split('$');
            for (int i = 0; i <= selection.Length - 1; i++)
            {
                selectionsub = selection[i].Split(':');
                if (selectionsub[0] == "AssignedTo")
                {
                    assigned = "true";
                    int Assig = Convert.ToInt32(selectionsub[1]);
                    teissueresult = teissueresult.Where(x => (x.AssignedTo == Assig));
                }
                if (selectionsub[0] == "Queue")
                {
                    int que = Convert.ToInt32(selectionsub[1]);
                    teissueresult = teissueresult.Where(x => x.QueueID == que);
                }
            }
            // teissueresult = teissueresult.Where(x => x.AssignedTo == AssignedTo);
            #region Orderby Category
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Category)
            {
                //result.KeyNCount = GetIssues().Where(x => x.AssignedTo == AssignedTo)
                //       .GroupBy(x => x.Categoryname)
                //        .Select(g => new { g.Key, Count = g.Count() });
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.Categoryname)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                          .GroupBy(x => x.Categoryname)
                           .Select(g => new { g.Key, Count = g.Count() });
                }

                return result;
            }
            #endregion
            #region Orderby priority
            if (orderBy == Models.Enums.TEIssueOrderByEnums.priority)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.PriorityName)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                         .GroupBy(x => x.PriorityName)
                          .Select(g => new { g.Key, Count = g.Count() });
                }

                return result;
            }
            #endregion
            #region Orderby Status
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Status)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.Status)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                          .GroupBy(x => x.Status)
                           .Select(g => new { g.Key, Count = g.Count() });
                }
                return result;
            }
            #endregion
            #region Orderby prefferedtime
            if (orderBy == Models.Enums.TEIssueOrderByEnums.prefferedtime)
            {
                if (assigned == "true")
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                        //.Where(x => !new[] { "CLOSED", "DISMISSED", "CANCELLED" }.Contains(x.Status))
                           .GroupBy(x => x.PrefferedTime)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                else
                {
                    result.KeyNCount = teissueresult//.Where(x => x.AssignedTo == AssignedTo)
                           .GroupBy(x => x.PrefferedTime)
                            .Select(g => new { g.Key, Count = g.Count() });
                }
                return result;
            }
            #endregion

            #region Orderby Assign To name i.e. Person
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Person)
            {
                if ((filterBy == null) || (filterBy == ""))
                {
                    result.KeyNCount = GetIssues().GroupBy(n => n.AssignToName)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    result.Issues = GetIssues().OrderBy(x => x.AssignToName).Where(x => x.AssignToName == filterBy);
                    if (result.KeyNCount == null)
                    {

                        var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                          && (x.Name == filterBy))
                 .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                        result.KeyNCount = GetIssues().OrderBy(x => x.AssignToName).Where(x => x.ParentCatID == listOfEntity.Uniqueid)
                             .GroupBy(n => n.AssignToName).Select(g => new { g.Key, Count = g.Count() });
                    }
                    return result;
                }
                else
                {
                    result.Issues = GetIssues().OrderBy(x => x.AssignToName);
                    return result;
                }

                result.KeyNCount = GetIssues().GroupBy(n => n.AssignToName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion


            #region Orderby Assign To name i.e. Raised
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Raised)
            {
                if ((filterBy == null) || (filterBy == ""))
                {
                    result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    int raisedby = Convert.ToInt32(filterBy);
                    result.Issues = GetIssues().OrderBy(x => x.RaisedByName).Where(x => x.RaisedBy == raisedby);
                    return result;
                }
                else
                {
                    result.Issues = GetIssues().OrderBy(x => x.RaisedByName);
                    return result;
                }

                result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Assigned
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Assigned)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
                    .Select(g => new { g.Key, Count = g.Count() });
                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    //int AssignedTo = Convert.ToInt32(filterBy);
                    //result.Issues = GetIssues().OrderBy(x => x.AssignToName).Where(x => x.AssignedTo == AssignedTo);
                    result.Issues = teissueresult.OrderBy(x => x.AssignToName);
                    return result;
                }
                else
                {
                    result.Issues = GetIssues().OrderBy(x => x.AssignToName);
                    return result;
                }

                result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Manager
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Manager)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = GetIssues().GroupBy(x => x.Categoryname)
                     .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    var listOfEntity = db.TECategories.Where(x => (x.IsDeleted == false)
                                                          && (x.Name == filterBy))
                 .OrderByDescending(x => x.LastModifiedOn).ToList().FirstOrDefault();

                    result.Issues = GetIssues().OrderBy(x => x.Categoryname).Where(x => x.ParentCatID == listOfEntity.Uniqueid);
                    return result;
                }
                else
                {
                    result.Issues = GetIssues().OrderBy(x => x.Categoryname);
                    return result;
                }

                result.KeyNCount = GetIssues().GroupBy(x => x.Categoryname)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Orderby Assign To name i.e. Queue
            if (orderBy == Models.Enums.TEIssueOrderByEnums.Queue)
            {
                 if ((filterBy ==null) || (filterBy == "null") || (filterBy == ""))
                {
                    result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
       .Select(g => new { g.Key, Count = g.Count() });

                    return result;
                }
                else if (filterBy.Length > 0)
                {
                    result.Issues = GetIssues().OrderBy(x => x.Queuename).Where(x => x.Queuename == filterBy);
                    return result;
                }
                else
                {
                    result.Issues = GetIssues().OrderBy(x => x.Queuename);
                    return result;
                }

                result.KeyNCount = GetIssues().GroupBy(n => n.RaisedByName)
                      .Select(g => new { g.Key, Count = g.Count() });

                return result;
            }
            #endregion
            #region Order by Filter
            else
            {
                string[] filtersplit = filterBy.Split('$');
                string[] filtersub;
                string[] priority = new string[20];
                string[] Status = new string[20];
                string[] Category = new string[100];
                string[] prefferedtime = new string[20];
                string[] Date = new string[2];
                DateTime Datefrom = System.DateTime.Now;
                DateTime Dateto = System.DateTime.Now;
                for (int i = 0; i <= filtersplit.Length - 1; i++)
                {
                    filtersub = filtersplit[i].Split(':');

                    if (filtersub[0] == "Priority")
                    {
                        // priority = filtersub[1];
                        priority = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Status")
                    {
                        //Status = filtersub[1];
                        Status = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Category")
                    {
                        // Category = filtersub[1];
                        Category = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "PrefferedTime")
                    {
                        //prefferedtime = filtersub[1];
                        prefferedtime = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Date")
                    {
                        // Date = filtersub[1];
                        Date = filtersub[1].Split(',');

                        if (filtersub[1].ToString() == "Yesterday")
                        {
                            Datefrom = System.DateTime.Now.AddDays(-1);
                            Dateto = System.DateTime.Now.AddDays(-1);
                        }
                        else if (filtersub[1].ToString() == "Last 7 days")
                        {
                            Datefrom = System.DateTime.Now.AddDays(-7);
                            Dateto = System.DateTime.Now;
                        }
                        else if (filtersub[1].ToString() == "Last 30 days")
                        {
                            Datefrom = System.DateTime.Now.AddMonths(-1);
                            Dateto = System.DateTime.Now;
                        }
                    }
                }

                // result.Issues = teissueresult;

                if (priority[0] != null)
                {
                    teissueresult = teissueresult.Where(x => //(x.AssignedTo == AssignedTo) &&
                                  (priority.Contains(x.PriorityName))
                        // && (Category.Contains(x.Categoryname))
                        //  && (Status.Contains(x.Status))
                        //&& (prefferedtime.Contains(x.PrefferedTime))
             );
                    // .OrderBy(x => new { x.Priority, x.Uniqueid });
                    //teissueresult = result.Issues;
                }
                if (Status[0] != null)
                {
                    teissueresult = teissueresult.Where(x =>// (x.AssignedTo == AssignedTo)
                        //          && (priority.Contains(x.Priority))
                        // && (Category.Contains(x.Categoryname))
                        (Status.Contains(x.Status))
                        //&& (prefferedtime.Contains(x.PrefferedTime))
             );
                    // .OrderBy(x => new { x.Priority, x.Uniqueid });
                }
                if (Category[0] != null)
                {
                    teissueresult = teissueresult.Where(x =>// (x.AssignedTo == AssignedTo)
                        //          && (priority.Contains(x.Priority))
                          (Category.Contains(x.Categoryname))
                        //&& (Status.Contains(x.Status))
                        //&& (prefferedtime.Contains(x.PrefferedTime))
             );
                    //.OrderBy(x => new { x.Priority, x.Uniqueid });
                }
                if (prefferedtime[0] != null)
                {
                    teissueresult = teissueresult.Where(x => //(x.AssignedTo == AssignedTo)
                        //          && (priority.Contains(x.Priority))
                        // && (Category.Contains(x.Categoryname))
                        //&& (Status.Contains(x.Status))
             (prefferedtime.Contains(x.PrefferedTime))
             );
                    //.OrderBy(x => new { x.Priority, x.Uniqueid });
                }
                if (Date[0] != null)
                {
                    teissueresult = teissueresult.Where(x => //(x.AssignedTo == AssignedTo)
                        //         && (priority.Contains(x.Priority))
                        //&&(Category.Contains(x.Categoryname))
                        //&& (Status.Contains(x.Status))
                        // && (prefferedtime.Contains(x.PrefferedTime))
            (x.CreatedOn >= Datefrom && x.CreatedOn <= Dateto)
             );

                }

                // || (x.CreatedOn >= System.DateTime.Parse(Date[0]) && x.CreatedOn <= System.DateTime.Parse(Date[1]))

                //      || (new[] { "" + Status + "" }.Contains(x.Status))
                //// || x.Categoryname.Contains("" + Category + "")
                //        || new[] { "" + Category + "" }.Contains(x.Categoryname)
                //          || (new[] { "" + prefferedtime + "" }.Contains(x.PrefferedTime))
                //        // || x.PrefferedTime.Contains("" + prefferedtime + "")
                //         || (x.CreatedOn >= System.DateTime.Parse(Date[0]) && x.CreatedOn <= System.DateTime.Parse(Date[1])))
                //prefferedtime.Contains(x.PrefferedTime)


                // && (x.Priority.Contains("" + priority + "")
                //      || (new[] { "" + Status + "" }.Contains(x.Status))
                //// || x.Categoryname.Contains("" + Category + "")
                //        || new[] { "" + Category + "" }.Contains(x.Categoryname)
                //          || (new[] { "" + prefferedtime + "" }.Contains(x.PrefferedTime))
                //        // || x.PrefferedTime.Contains("" + prefferedtime + "")
                //         || (x.CreatedOn >= System.DateTime.Parse(Date[0]) && x.CreatedOn <= System.DateTime.Parse(Date[1])))
                result.Issues = teissueresult.OrderBy(x => x.Priority).ThenBy(n => n.Uniqueid);
                return result;
            }
            #endregion
        }

        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesRaisedBy(int RaisedBy, string UNITONBOARDINGID)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                &&
                                                                (x.RaisedBy == RaisedBy)
                                                                &&
                                                                (x.UNITONBOARDINGID == UNITONBOARDINGID))
                                                                .OrderByDescending(x => x.LastModifiedOn).ToList();
            foreach (var item in listOfEntity)
            {

                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {
                    //if (item.RaisedBy != null)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (item.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }

                    //if (item.AssignedTo != null)
                    //{
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (item.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }
                result.Add(tempModel);
                //TEcomplainceModel tempModel = CopyEntityToModel(item);

                //if (tempModel.AssignedTo != null)
                //{
                //    try
                //    {
                //        int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                //        TEEmpBasicInfo tempBasicInfo = db.TEEmpBasicInfoes.Find(tempAssignedTo);
                //        if (tempBasicInfo != null)
                //            tempModel.AssignToName = tempBasicInfo.FirstName + " " + tempBasicInfo.LastName;
                //    }
                //    catch (Exception ex)
                //    {
                //        db.ApplicationErrorLogs.Add(
                //       new ApplicationErrorLog
                //       {
                //           Error = ex.Message,
                //           ExceptionDateTime = System.DateTime.Now,
                //           InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                //           Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                //           Stacktrace = ex.StackTrace
                //       }
                //       );
                //        db.SaveChanges();
                //    }
                //}

                //result.Add(tempModel);


                //Customization

            }
            return result;
        }

        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesRaisedByemployee(int RaisedBy, int QueueID)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                &&
                                                                (x.RaisedBy == RaisedBy)
                                                                &&
                                                                (x.QueueID == QueueID))
                                                                .OrderByDescending(x => x.LastModifiedOn).ToList();
            foreach (var item in listOfEntity)
            {

                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {
                    //if (item.RaisedBy != null)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (item.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }

                    //if (item.AssignedTo != null)
                    //{
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (item.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }
                result.Add(tempModel);

            }
            return result;
        }

        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesAssignedToemployee(int AssignedTo, int QueueID)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                &&
                                                                (x.AssignedTo == AssignedTo)
                                                                &&
                                                                (x.QueueID == QueueID))
                                                                .OrderBy(x => new { x.Priority, x.Uniqueid }).ToList();
            foreach (var item in listOfEntity)
            {

                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {
                    //if (item.RaisedBy != null)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (item.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }

                    //if (item.AssignedTo != null)
                    //{
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (item.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }
                result.Add(tempModel);

            }
            return result;
        }
        [HttpGet]
        public IEnumerable<TEComplainceModel> GetIssuesClosed(int RaisedBy)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                &&
                                                                (x.RaisedBy == RaisedBy))
                                                                .OrderByDescending(x => x.LastModifiedOn).ToList();
            foreach (var item in listOfEntity)
            {
                TEComplainceModel tempModel = CopyEntityToModel(item);

                //if (tempModel.AssignedTo != null)
                //{
                try
                {
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }
                //}

                result.Add(tempModel);
            }

            //Customization


            return result;
        }

        [HttpGet]
        public object GetIssuesClosedrateus(int RaisedBy)
        {
            //TEcomplainceModel
            TEComplainceModel result = new TEComplainceModel();
            try
            {
                TEIssue listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                    &&
                                                                    (x.Status == "CLOSED")
                                                                      &&
                                                                    (x.RateUS.Equals(null))
                                                                    &&
                                                                    (x.RaisedBy == RaisedBy))
                                                                    .OrderByDescending(x => x.LastModifiedOn).ToList().First();

                result = CopyEntityToModel(listOfEntity);

                //if (tempModel.AssignedTo != null)
                //{

                int tempAssignedTo = Convert.ToInt32(listOfEntity.AssignedTo);
                UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                if (tempuser != null)
                    result.AssignToName = tempuser.CallName;
                return result;
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
               new ApplicationErrorLog
               {
                   Error = ex.Message,
                   ExceptionDateTime = System.DateTime.Now,
                   InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                   Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                   Stacktrace = ex.StackTrace
               }
               );

                //result=tempModel;
            }

            //Customization

            db.SaveChanges();


            //return result;
            return db.UserProfiles.Select(x => new { Message = "No user Info to rate" }).FirstOrDefault();
        }

        [HttpGet]
        public TEComplainceModel GetIssuesClosedrateusTechnician(int AssignedTo)
        {
            TEComplainceModel result = new TEComplainceModel();
            TEIssue listOfEntity = db.TEIssues.Where(x => (x.IsDeleted == false)
                                                                &&
                                                                (x.Status == "CLOSED")
                                                                  &&
                                                                (x.TechnicianRate.Equals(null))
                                                                &&
                                                                (x.AssignedTo == AssignedTo))
                                                                .OrderByDescending(x => x.LastModifiedOn).ToList().First();

            result = CopyEntityToModel(listOfEntity);

            //if (tempModel.AssignedTo != null)
            //{
            try
            {
                int tempAssignedTo = Convert.ToInt32(listOfEntity.AssignedTo);
                UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                if (tempuser != null)
                    result.AssignToName = tempuser.CallName;
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
               new ApplicationErrorLog
               {
                   Error = ex.Message,
                   ExceptionDateTime = System.DateTime.Now,
                   InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                   Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                   Stacktrace = ex.StackTrace
               }
               );
                db.SaveChanges();


                //result=tempModel;
            }

            //Customization


            return result;
        }
        public TEComplainceModel CopyEntityToModel(TEIssue a)
        {
            TEComplainceModel b = new TEComplainceModel();

            // copy fields
            var typeOfEntity = a.GetType();
            var typeOfModel = b.GetType();

            // copy properties
            foreach (var propertyOfA in typeOfEntity.GetProperties())
            {
                try
                {
                    var propertyOfB = typeOfModel.GetProperty(propertyOfA.Name);
                    propertyOfB.SetValue(b, propertyOfA.GetValue(a));
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                        new ApplicationErrorLog
                        {
                            Error = ex.Message,
                            ExceptionDateTime = System.DateTime.Now,
                            InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                            Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                            Stacktrace = ex.StackTrace
                        }
                        );
                }
            }

            return b;
        }

        //// GET api/<controller>/5
        //[HttpGet]
        //public TEIssue GetIssue(int id)
        //{
        //    return db.TEIssues.Find(id);
        //}
        public IEnumerable<TEComplainceModel> GetIssuesbyissueid(int id)
        {
            List<TEComplainceModel> result = new List<TEComplainceModel>();
            List<TEIssue> listOfEntity = db.TEIssues.Where(x => x.Uniqueid == id)
                                .OrderByDescending(x => x.Uniqueid).ToList();
            foreach (var item in listOfEntity)
            {
                TEComplainceModel tempModel = CopyEntityToModel(item);

                try
                {

                    //if (tempModel.RaisedBy + "".Length > 0)
                    //{
                    int tempraisedby = Convert.ToInt32(item.RaisedBy);
                    UserProfile tempraised = db.UserProfiles.Find(tempraisedby);
                    if (tempraised != null)
                        tempModel.RaisedByName = tempraised.CallName;
                    //}

                    if (tempModel.CategoryID != null)
                    {
                        int temcat = Convert.ToInt32(item.CategoryID);
                        TECategory temcategry = db.TECategories.Find(temcat);
                        if (temcategry != null)
                            tempModel.Categoryname = temcategry.Name;
                    }

                    //if (tempModel.AssignedTo != null)
                    //{
                    int tempAssignedTo = Convert.ToInt32(item.AssignedTo);
                    UserProfile tempuser = db.UserProfiles.Find(tempAssignedTo);
                    if (tempuser != null)
                        tempModel.AssignToName = tempuser.CallName;
                    //}

                    if (tempModel.ProjectID != null)
                    {
                        int Teproject = Convert.ToInt32(item.ProjectID);
                        TEProject tempprokect = db.TEProjects.Find(Teproject);
                        if (tempprokect != null)
                        {
                            tempModel.ProjectName = tempprokect.ProjectName;
                            tempModel.Projectcode = tempprokect.ProjectCode;
                        }
                    }

                    if (tempModel.Priority != null)
                    {
                        int tepriority = Convert.ToInt32(item.Priority);
                        TEPickListItem tempprokect = db.TEPickListItems.Find(tepriority);
                        if (tempprokect != null)
                        {
                            tempModel.PriorityName = tempprokect.Description;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                   new ApplicationErrorLog
                   {
                       Error = ex.Message,
                       ExceptionDateTime = System.DateTime.Now,
                       InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                       Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
                       Stacktrace = ex.StackTrace
                   }
                   );
                    db.SaveChanges();
                }

                result.Add(tempModel);
            }

            //Customization


            return result;
        }

        // POST api/<controller>
        [HttpPost]
        public TEIssue PostIssue(TEIssue value)
        {
            TEIssue result = value;
            using (var scope = new TransactionScope())
            {
                //Create
                if (!(value.Uniqueid + "".Length > 0))
                {
                    result.CreatedOn = System.DateTime.Now;
                    result.LastModifiedOn = System.DateTime.Now;
                    string priority = "";
                    if (result.Priority != null)
                    {
                        int priorityid=Convert.ToInt32(result.Priority);
                        TEQueueDepartment Escalation = db.TEQueueDepartments.Where(x => x.QueueID == value.QueueID && x.CategoryID == value.CategoryID).FirstOrDefault();
                        TEPickListItem picklist=db.TEPickListItems.Find(priorityid);
                        priority = picklist.Description;
                        if (Escalation != null)
                        {  
                            if (picklist.Description == "Normal")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAMedium));
                            else if (picklist.Description == "Critical")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAHigh));
                            else if (picklist.Description == "Emergency")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLACritical));
                        }
                    }
                    try
                    {
                        string worklocation = (from tew in db.TEWorkLocations
                                               join teass in db.TEEmpAssignmentDetails on tew.Uniqueid equals teass.TEWorkLocation
                                               where teass.TEEmpBasicInfo == (from bas in db.TEEmpBasicInfoes
                                                                              where bas.UserId ==
                                                                                  (from usr in db.UserProfiles where usr.UserId == value.RaisedBy select usr.UserName).FirstOrDefault()
                                                                              select bas.Uniqueid).FirstOrDefault()
                                               && teass.IsDeleted == false && tew.IsDeleted == false
                                               select tew.WorkLocationName).Distinct().FirstOrDefault();
                        #region Commented due to entity change
                        //if (worklocation == null)
                        //{
                        //    worklocation = (from tew in db.TEProjects
                        //                    where tew.Uniqueid == value.ProjectID && tew.IsDeleted == false
                        //                    select tew.ProjectName).Distinct().FirstOrDefault();
                        //}
                        #endregion
                        value.Worklocation = worklocation;

                        //if (result.QueueID + "".Length > 0)
                        if (result.QueueID.ToString() == "")
                        {
                            var que = db.TEQueues.Where(x => (x.PROJECTID == value.ProjectID)).ToList().First();
                            result.QueueID = que.QueueID;
                        }
                        if (result.AssignedTo == null)
                        {
                            TEQueueDepartment QueueDept = db.TEQueueDepartments.Where(x => x.IsDeleted == false && x.QueueID == result.QueueID && x.CategoryID == result.CategoryID).FirstOrDefault();
                            if (QueueDept != null)
                            {
                                result.AssignedTo = QueueDept.Default_assignee;
                                result.PrimaryAssigne = QueueDept.Default_assignee;
                            }
                        }
                        //string createdby = (from teiss in db.TEIssues
                        //                    where (teiss.Uniqueid == value.Uniqueid)
                        //                    select teiss.CreatedBy).ToList().First();
                        //value.CreatedBy = createdby;

                    }
                    catch (Exception ex1)
                    { ex1.Message.ToString(); }
                    //var assign = db.TEQueueDepartments.Where(x => (x.QueueID == que.QueueID) && (x.CategoryID == value.CategoryID)).ToList().First();
                    //result.AssignedTo = assign.Default_assignee;
                    result = db.TEIssues.Add(result);
                    db.SaveChanges();
                    if ((value.RateUS != "") || (value.TechnicianRate != ""))
                    {
                        int log = LogActivityTimeInfo(value);
                    }
                    db.SaveChanges();
                    List<UserProfile> userprofile = db.UserProfiles.Where(x => x.IsDeleted == false).ToList();
                    UserProfile requester = userprofile.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
                    UserProfile technician = userprofile.Where(x => x.UserId == value.AssignedTo).FirstOrDefault();
                    
                    string catname = (from x in db.TECategories where (x.CategoryID == value.CategoryID) select x.Name).Distinct().FirstOrDefault();
                    //string pcatname = (from x in db.TECategories where (x.CategoryID == value.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                    EmailSendModel SendDetails = new EmailSendModel();
                    var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "NewTicketCreation").FirstOrDefault();
                    SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", result.Uniqueid.ToString());
                    SendDetails.Html = potemp1.EmailTemplate.Replace("@Requestor Name@", (requester.CallName));
                    SendDetails.Html = SendDetails.Html.Replace("@ticket number@", result.Uniqueid.ToString());
                    SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", result.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", result.CreatedOn));
                    SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                    SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                    SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", result.Estimated_Close_date==null?"":String.Format("{0:dd-MM-yyyy}", result.Estimated_Close_date));
                    SendDetails.Html = SendDetails.Html.Replace("@Description@", value.Descritpion);
                    SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                    SendDetails.To = requester==null?"":requester.email;
                    EmailMgr.SendEmail(SendDetails);
                    TEQueue admin = new TEQueue();
                    UserProfile adminprofile = new UserProfile() ;
                    try
                    {
                        admin = db.TEQueues.Where(x=>x.Admin==value.QueueID).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                    }
                    if (admin != null)
                    {
                        adminprofile = userprofile.Where(x => x.UserId == admin.Admin).FirstOrDefault();
                        EmailSendModel AdminSendDetails = new EmailSendModel();
                        var potemp2 = db.TEEmailTemplates.Where(x => x.ModuleName == "AdminTicket").FirstOrDefault();
                        AdminSendDetails.Subject = potemp2.Subject;
                        AdminSendDetails.Html = potemp2.EmailTemplate.Replace("@Requestor Name@", (requester.CallName));
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@ticket number@", result.Uniqueid.ToString());
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Admin Name@", (adminprofile.CallName));
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Requestor Date@", result.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", result.CreatedOn));
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Category@", catname);
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Priority@", priority);
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Resolution Date@", result.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", result.Estimated_Close_date));
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Description@", value.Descritpion);
                        AdminSendDetails.Html = AdminSendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        AdminSendDetails.To = adminprofile == null ? "" : adminprofile.email;
                        EmailMgr.SendEmail(AdminSendDetails);
                    }
                    if (result.AssignedTo != null)
                    {
                        EmailSendModel AssignSendDetails = new EmailSendModel();
                        var potemp3 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketAssignedToTechnician").FirstOrDefault();
                        AssignSendDetails.Subject = potemp3.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        AssignSendDetails.Html = potemp3.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString()); ;
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Date@", result.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", result.CreatedOn));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Category@", catname);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Priority@", priority);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Resolution Date@", result.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", result.Estimated_Close_date));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Description@", value.Descritpion);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        AssignSendDetails.To = technician == null ? "" : technician.email;
                        EmailMgr.SendEmail(AssignSendDetails);
                    }
                    //SendNotification(value, "Ticket " + value.Uniqueid + " is in process. We will get back to you soon");

                    //var usr = db.TEQueueDepartments.Where(x => (x.CategoryID == value.CategoryID)).ToList();

                    //var employeeDept = from e in db.TEIssues 
                    //                   join d in db.TEQueueDepartments on e.CategoryID equals d.CategoryID 
                    //               select new
                    //               {
                    //                   TEIssues = e,
                    //                     TEQueueDepartments = d};

                    //   foreach(var ed in employeeDept)
                    //   {
                    //       ed.TEIssues.QueueID = ed.TEQueueDepartments.QueueID;

                    //       if (ed.TEQueueDepartments.AUTOASSIGNMENT != null)
                    //           ed.TEIssues.AssignedTo = ed.TEQueueDepartments.Default_assignee;
                    //       db.SaveChanges();
                    //      //int res = updateValue(ed.TEQueueDepartments, ed.TEIssues.Uniqueid);
                    //   }

                }
                //Edit | Delete
                else
                {
                   
                   // db = new TEHRIS_DevEntities();
                    TEIssue mailissues = db.TEIssues.Find(value.Uniqueid);
                    result.CreatedOn = mailissues.CreatedOn;
                    string priority = "";
                    if (value.Priority != null )
                    {
                        int priorityid = Convert.ToInt32(result.Priority);
                        TEQueueDepartment Escalation = db.TEQueueDepartments.Where(x => x.QueueID == value.QueueID && x.CategoryID == value.CategoryID).FirstOrDefault();
                        TEPickListItem picklist = db.TEPickListItems.Find(priorityid);
                        priority = picklist.Description;
                        if (Escalation != null)
                        {
                            if (picklist.Description == "Normal")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAMedium));
                            else if (picklist.Description == "Critical")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAHigh));
                            else if (picklist.Description == "Emergency")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLACritical));
                        }
                    }
                    else if(mailissues.Priority!=null){
                        int priorityid = Convert.ToInt32(mailissues.Priority);
                        TEQueueDepartment Escalation = db.TEQueueDepartments.Where(x => x.QueueID == value.QueueID && x.CategoryID == value.CategoryID).FirstOrDefault();
                        TEPickListItem picklist = db.TEPickListItems.Find(priorityid);
                        priority = picklist.Description;
                        if (Escalation != null)
                        {
                            if (picklist.Description == "Normal")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAMedium));
                            else if (picklist.Description == "Critical")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAHigh));
                            else if (picklist.Description == "Emergency")
                                result.Estimated_Close_date = value.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLACritical));
                        }
                    }
                    db = new TEHRIS_DevEntities();
                    db.TEIssues.Attach(value);
                    
                    
                    try
                    {
                        
                        string worklocation = (from tew in db.TEWorkLocations
                                               join teass in db.TEEmpAssignmentDetails on tew.Uniqueid equals teass.TEWorkLocation
                                               where teass.TEEmpBasicInfo == (from bas in db.TEEmpBasicInfoes
                                                                              where bas.UserId ==
                                                                                  (from usr in db.UserProfiles where usr.UserId == mailissues.RaisedBy select usr.UserName).FirstOrDefault()
                                                                              select bas.Uniqueid).FirstOrDefault()
                                               && teass.IsDeleted == false && tew.IsDeleted == false
                                               select tew.WorkLocationName).Distinct().FirstOrDefault();
                        value.Worklocation = worklocation;
                    }
                    catch (Exception ex1)
                    { }

                    foreach (PropertyInfo item in result.GetType().GetProperties())
                    {

                        string propname = item.Name;
                        if (propname.ToLower() == "createdon" || propname.ToLower() == "teissuesdispositions")
                            continue;
                        object propValue = item.GetValue(value);
                        if (propValue != null || Convert.ToString(propValue).Length != 0)
                            db.Entry(value).Property(propname).IsModified = true;

                    }
                    int? raisedby = (from teiss in db.TEIssues
                                     where (teiss.Uniqueid == value.Uniqueid)
                                     select teiss.RaisedBy).ToList().First();
                    value.RaisedBy = raisedby;

                    int? assignedtoperson = (from teiss in db.TEIssues
                                             where teiss.Uniqueid == value.Uniqueid
                                             select teiss.AssignedTo).Distinct().FirstOrDefault();
                    string catname = (from x in db.TECategories where (x.CategoryID == mailissues.CategoryID) select x.Name).Distinct().FirstOrDefault();
                    List<UserProfile> userprofile = db.UserProfiles.ToList();
                    UserProfile requester = userprofile.Where(x => x.UserId == mailissues.RaisedBy).FirstOrDefault();
                    UserProfile technician = new UserProfile();
                    if (value.AssignedTo == null && mailissues.AssignedTo == null)
                    {
                        technician = null;
                    }
                    else if (value.AssignedTo == null && mailissues.AssignedTo!=null)
                    {
                        technician = userprofile.Where(x => x.UserId == mailissues.AssignedTo).FirstOrDefault();
                    }
                    else 
                    {
                        technician = userprofile.Where(x => x.UserId == value.AssignedTo).FirstOrDefault();
                    }
                    if ((assignedtoperson == null) && (value.AssignedTo != null))
                    {
                        value.PrimaryAssigne = value.AssignedTo;
                        db.Entry(value).Property(x => x.PrimaryAssigne).IsModified = true;

                        technician = userprofile.Where(x => x.UserId == value.AssignedTo).FirstOrDefault();
                        //string pcatname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketAssignedForRequester").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Subject = potemp1.Subject.Replace("@Technician name@", (technician == null ? "" : technician.UserName));
                        SendDetails.Html = potemp1.EmailTemplate;
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Technician Date@", value.Assigned == null ? "" : String.Format("{0:dd-MM-yyyy}", value.Assigned));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);

                        EmailSendModel AssignSendDetails = new EmailSendModel();
                        var potemp3 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketAssignedToTechnician").FirstOrDefault();
                        AssignSendDetails.Subject = potemp3.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        AssignSendDetails.Html = potemp3.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Date@", result.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", result.CreatedOn));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Category@", catname);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Priority@", priority);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Resolution Date@", result.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", result.Estimated_Close_date));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        AssignSendDetails.To = technician == null ? "" : technician.email;
                        EmailMgr.SendEmail(AssignSendDetails);
                    }
                    if ((assignedtoperson != null) && (value.AssignedTo != null))
                    {
                        //string pcatname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        UserProfile oldassigne = userprofile.Where(x => x.UserId == mailissues.AssignedTo).FirstOrDefault();
                        technician = userprofile.Where(x => x.UserId == value.AssignedTo).FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketAssignedForRequester").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Subject = SendDetails.Subject.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Technician Date@", mailissues.Assigned == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Assigned));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@OldTechnician name@", (oldassigne == null ? "" : oldassigne.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);

                        EmailSendModel AssignSendDetails = new EmailSendModel();
                        var potemp2 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketReAssignedToTechnician").FirstOrDefault();
                        AssignSendDetails.Subject = potemp2.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        AssignSendDetails.Subject = AssignSendDetails.Subject.Replace("@OldTechnician name@", oldassigne.CallName);
                        AssignSendDetails.Subject = AssignSendDetails.Subject.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        AssignSendDetails.Html = potemp2.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Category@", catname);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Priority@", priority);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Technician Date@", mailissues.Assigned == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Assigned));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        AssignSendDetails.Html = AssignSendDetails.Html.Replace("@OldTechnician name@", (oldassigne == null ? "" : oldassigne.CallName));
                        AssignSendDetails.To = technician == null ? "" : technician.email;
                        EmailMgr.SendEmail(AssignSendDetails);
                    }
                    if (value.Status == "QUEUED")
                    {
                        //SendNotification(value, "Ticket " + value.Uniqueid + " is in process. We will get back to you soon");
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }

                    }
                    if (value.Status == "DISMISSED")
                    {
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }

                        //SendNotification(value, "Ticket " + value.Uniqueid + " has been Closed.");


                    }
                    if (value.Status == "ACCEPTED")
                    {
                        value.Assigned = System.DateTime.Now.ToString();
                        db.Entry(value).Property(x => x.Assigned).IsModified = true;

                        int? assignedto = (from teiss in db.TEIssues
                                           where (teiss.Uniqueid == value.Uniqueid)
                                           select teiss.AssignedTo).ToList().First();
                        value.AssignedTo = assignedto;
                        if (value.AssignedTo != null)
                        {
                            string assignedname = (from teuser in db.UserProfiles
                                                   where (teuser.UserId == assignedto)
                                                   select teuser.CallName).ToList().First();
                        }
                        //if (assignedname != null)
                            //SendNotification(value, "Ticket " + value.Uniqueid + " has been assigned to " + assignedname + ".");
                    }
                    if (value.Status == "COMMENCED")
                    {
                        value.Start = System.DateTime.Now.ToString();
                        db.Entry(value).Property(x => x.Start).IsModified = true;

                        int? assignedto = (from teiss in db.TEIssues
                                           where (teiss.Uniqueid == value.Uniqueid)
                                           select teiss.AssignedTo).ToList().First();
                        value.AssignedTo = assignedto;
                        if (value.AssignedTo != null)
                        {
                            string assignedname = (from teuser in db.UserProfiles
                                                   where (teuser.UserId == assignedto)
                                                   select teuser.CallName).ToList().First();
                        }
                       // if (assignedname != null)
                           // SendNotification(value, "The team has started work to resolve your ticket " + value.Uniqueid + ".");

                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }

                        //var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "Ticket Assigned to a technician").FirstOrDefault();
                        //var subject = potemp1.Subject;
                        //var body = potemp1.EmailTemplate.Replace("@Requestor Name@", (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.CallName).Distinct().FirstOrDefault());
                        //body = body.Replace("@Location@", value.Worklocation);
                        //body = body.Replace("@Category@", (from x in db.TECategories where (x.CategoryID == value.CategoryID) select x.Name).Distinct().FirstOrDefault());
                        //body = body.Replace("@Description@", value.Descritpion);
                        //body = body.Replace("@Technician name@", (from x in db.UserProfiles where (x.UserId == value.AssignedTo) select x.CallName).Distinct().FirstOrDefault());
                        //body = body.Replace("@Reason@", value.DismissedReason);
                        //var to = (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.email).Distinct().FirstOrDefault() == null ? (from x in db.TEContacts where (x.UserId == value.RaisedBy) select x.Emailid).Distinct().FirstOrDefault() : (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.email).Distinct().FirstOrDefault();
                        //sendmail(to, subject, body);
                    }
                    if (value.Status == "PAUSED")
                    {
                        value.Pasue = System.DateTime.Now.ToString();
                        db.Entry(value).Property(x => x.Pasue).IsModified = true;


                        //UserProfile tempuser = db.UserProfiles.Find(value.AssignedTo.ToString());
                        //if (tempuser != null)
                        int? assignedto = (from teiss in db.TEIssues
                                           where (teiss.Uniqueid == value.Uniqueid)
                                           select teiss.AssignedTo).ToList().First();
                        value.AssignedTo = assignedto;
                        if (value.AssignedTo != null)
                        {
                            string assignedname = (from teuser in db.UserProfiles
                                                   where (teuser.UserId == assignedto)
                                                   select teuser.CallName).ToList().First();
                        }
                        //if (assignedname != null)
                           // SendNotification(value, "Ticket " + value.Uniqueid + " has been paused by " + assignedname + ".");

                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }
                        //UserProfile.string catname = (from x in db.TECategories where (x.CategoryID == mailissues.CategoryID) select x.Name).Distinct().FirstOrDefault();
                        //UserProfile.string pcatname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketPaused").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.photo);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Technician Date@", mailissues.Assigned == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Assigned));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);
                    }
                    if (value.Status == "COST APPROVAL")
                    {
                        value.Pasue = System.DateTime.Now.ToString();
                        db.Entry(value).Property(x => x.Pasue).IsModified = true;

                        int? assignedto = (from teiss in db.TEIssues
                                           where (teiss.Uniqueid == value.Uniqueid)
                                           select teiss.AssignedTo).ToList().First();
                        value.AssignedTo = assignedto;
                        if (value.AssignedTo != null)
                        {
                            string assignedname = (from teuser in db.UserProfiles
                                                   where (teuser.UserId == assignedto)
                                                   select teuser.CallName).ToList().First();
                        }
                        //if (assignedname != null)
                            //SendNotification(value, "You are likely to incur an expense for Ticket " + value.Uniqueid + ". Kindly contact your Property Manager for details.");

                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }

                        //string catname = (from x in db.TECategories where (x.CategoryID == value.CategoryID) select x.Name).Distinct().FirstOrDefault();
                        //EmailSendModel SendDetails = new EmailSendModel();
                        //var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "Ticket Paused" && x.CampaignsName == catname).FirstOrDefault();
                        //SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        //SendDetails.Html = potemp1.EmailTemplate.Replace("@Requestor Name@", (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.CallName).Distinct().FirstOrDefault());
                        //SendDetails.Html = SendDetails.Html.Replace("@Location@", value.Worklocation);
                        //SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        //SendDetails.Html = SendDetails.Html.Replace("@Description@", value.Descritpion);
                        //SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (from x in db.UserProfiles where (x.UserId == value.AssignedTo) select x.CallName).Distinct().FirstOrDefault());
                        //SendDetails.Html = SendDetails.Html.Replace("@Reason@", value.DismissedReason);
                        //SendDetails.To = (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.email).Distinct().FirstOrDefault() == null ? (from x in db.TEContacts where (x.UserId == value.RaisedBy) select x.Emailid).Distinct().FirstOrDefault() : (from x in db.UserProfiles where (x.UserId == value.RaisedBy) select x.email).Distinct().FirstOrDefault();
                        //EmailMgr.SendEmail(SendDetails);
                    }
                    if (value.Status == "RESUMED")
                    {
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }
                    }
                    if (value.Status == "RESOLVED")
                    {
                        value.Resolved_Date = System.DateTime.Now.ToString();
                        db.Entry(value).Property(x => x.Resolved_Date).IsModified = true;

                        int? assignedto = (from teiss in db.TEIssues
                                           where (teiss.Uniqueid == value.Uniqueid)
                                           select teiss.AssignedTo).ToList().First();
                        value.AssignedTo = assignedto;
                        if(value.AssignedTo!=null){
                        string assignedname = (from teuser in db.UserProfiles
                                               where (teuser.UserId == assignedto)
                                               select teuser.CallName).ToList().First();
                        }
                        //if (assignedname != null)
                        //    SendNotification(value, "Ticket " + value.Uniqueid + " has been resolved by " + assignedname + ".");

                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }
                        //string catname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketResolved").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Technician Date@", mailissues.Assigned == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Assigned));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);
                    }
                    if (value.Status == "CLOSED")
                    {
                        value.Closed_Date = System.DateTime.Now;
                        db.Entry(value).Property(x => x.Closed_Date).IsModified = true;


                        //SendNotification(value, "Ticket " + value.Uniqueid + " has been Closed.Please provide feedback so that we can serve you better");

                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }
                        //string catname = (from x in db.TECategories where (x.CategoryID == mailissues.CategoryID) select x.Name).Distinct().FirstOrDefault();
                        //string pcatname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketClosed").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Closed Date@", value.Closed_Date == null ? "" : String.Format("{0:dd-MM-yyyy}", value.Closed_Date));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);
                    }
                    if (value.Status == "REOPEN")
                    {
                        value.ReopenDate = System.DateTime.Now;
                        db.Entry(value).Property(x => x.ReopenDate).IsModified = true;
                        db.Entry(value).Property(x => x.ReopenReason).IsModified = true;
                        int? assignedto = value.AssignedTo;
                        if (value.AssignedTo == null)
                        {
                            assignedto = (from teiss in db.TEIssues
                                          where (teiss.Uniqueid == value.Uniqueid)
                                          select teiss.AssignedTo).ToList().First();
                        }
                        value.AssignedTo = assignedto;
                        if (value.AssignedTo != null)
                        {
                            string assignedname = (from teuser in db.UserProfiles
                                                   where (teuser.UserId == assignedto)
                                                   select teuser.CallName).ToList().First();
                        }
                        //if (assignedname != null)
                        // SendNotification(value, "Ticket " + value.Uniqueid + " has been re-opened by " + assignedname + ".");



                        db.SaveChanges();
                        if ((value.RateUS != "") || (value.TechnicianRate != ""))
                        {
                            int log = LogActivityTimeInfo(value);
                        }


                        TEIssue issu = db.TEIssues.Where(x => x.Uniqueid == value.Uniqueid).FirstOrDefault();
                        if (issu != null)
                        {
                            //issu.Status = "QUEUED";
                            int Reopen = Convert.ToInt32(issu.Reopen);
                            issu.Reopen = (Reopen + 1).ToString();
                            db.SaveChanges();
                        }
                        //string catname = (from x in db.TECategories where (x.CategoryID == mailissues.CategoryID) select x.Name).Distinct().FirstOrDefault();
                        //string pcatname = (from x in db.TECategories where (x.CategoryID == mailissues.ParentCatID) select x.Name).Distinct().FirstOrDefault();
                        EmailSendModel SendDetails = new EmailSendModel();
                        var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketReOpenedForRequester").FirstOrDefault();
                        SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Closed Date@", mailissues.Closed_Date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Closed_Date));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = requester == null ? "" : requester.email;
                        EmailMgr.SendEmail(SendDetails);
                        SendDetails = new EmailSendModel();
                        var potemp2 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketReOpenedToTechnician").FirstOrDefault();
                        SendDetails.Subject = potemp2.Subject.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = potemp1.EmailTemplate.Replace("@ticket number@", value.Uniqueid.ToString());
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Name@", (requester.CallName));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Date@", mailissues.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.CreatedOn));
                        SendDetails.Html = SendDetails.Html.Replace("@Requestor Mobile@", requester.Phone);
                        SendDetails.Html = SendDetails.Html.Replace("@Category@", catname);
                        SendDetails.Html = SendDetails.Html.Replace("@Priority@", priority);
                        SendDetails.Html = SendDetails.Html.Replace("@Resolution Date@", mailissues.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Estimated_Close_date));
                        SendDetails.Html = SendDetails.Html.Replace("@Description@", mailissues.Descritpion);
                        SendDetails.Html = SendDetails.Html.Replace("@Closed Date@", mailissues.Closed_Date == null ? "" : String.Format("{0:dd-MM-yyyy}", mailissues.Closed_Date));
                        SendDetails.Html = SendDetails.Html.Replace("@Technician name@", (technician == null ? "" : technician.CallName));
                        SendDetails.To = technician == null ? "" : technician.email;
                        EmailMgr.SendEmail(SendDetails);
                        //value.Status = "QUEUED";
                        //value.Reopen = value.Reopen + 1;
                    }
                    db.SaveChanges();
                    //if ((value.RateUS != "") || (value.TechnicianRate != ""))
                    //{
                    //    int log = LogActivityTimeInfo(value);
                    //}
                    value.LastModifiedOn = System.DateTime.Now;
                    db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
                }

                //Save and Complete
                db.SaveChanges();
                scope.Complete();
            }
            return db.TEIssues.Find(result.Uniqueid);
        }

        private object sendmail(string to, string subject, string body)
        {
            TECommonLogicLayer.TEEmail temails = new TECommonLogicLayer.TEEmail();

            AegisImplicitMail.MimeMailMessage mailMessage = new AegisImplicitMail.MimeMailMessage();
            //mailMessage.From.DisplayName("bhargav.nula@total-environment.com");
            mailMessage.To.Add(to);
            // mailMessage.CC.Add("");
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            // mailMessage.Attachments="";
            temails.SendMail(mailMessage);
            return "success";
        }

        private TENotification SendNotification(TEIssue value, string description)
        {

            if ((value.Status == "PAUSED") || (value.Status == "COST APPROVAL"))
            {

                TENotification notify1 = new TENotification
                {
                    ApprovedBy = value.ApprovedBy,
                    ApprovedOn = System.DateTime.UtcNow,
                    CreatedBy = value.CreatedBy,
                    CreatedOn = System.DateTime.UtcNow,
                    //Have to be dynamic...

                    IsDeleted = false,
                    LastModifiedBy = value.ApprovedBy,
                    LastModifiedOn = System.DateTime.UtcNow,

                    Name = "Issue: " + value.Status,
                    description = description,
                    ReceivedBy = value.AssignedTo,
                    SendBy = value.AssignedTo,
                    ReadStatus = false,
                    Status = value.Status,
                    Module = "Issue",
                    ModuleUniqueid = value.Uniqueid
                    //Type

                };
                var y = db.TENotifications.Add(notify1);
            }

            TENotification notify = new TENotification
            {
                ApprovedBy = value.ApprovedBy,
                ApprovedOn = System.DateTime.UtcNow,
                CreatedBy = value.CreatedBy,
                CreatedOn = System.DateTime.UtcNow,
                //Have to be dynamic...

                IsDeleted = false,
                LastModifiedBy = value.ApprovedBy,
                LastModifiedOn = System.DateTime.UtcNow,

                Name = "Issue: " + value.Status,
                description = description,
                ReceivedBy = value.RaisedBy,
                SendBy = value.AssignedTo,
                ReadStatus = false,
                Status = value.Status,
                //Type

            };
            try
            {
                var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
                var usrtype = (from usrs in db.UserProfiles
                               join tebas in db.TEEmpBasicInfoes on usr.UserName equals tebas.UserId
                               where (usr.UserId == value.RaisedBy)
                               select usr.UserId).Distinct().FirstOrDefault();

                string apptype = "Fugue";
                if (usrtype == null)
                {
                    apptype = "Yellow";
                }
                if (usr.AndroidToken != null)
                {
                    try
                    {
                        TECommonLogicLayer.TEPushNotification tepush = new TECommonLogicLayer.TEPushNotification();
                        //tepush.SendNotification_Android("APA91bEWoz9U6m9VK-oBW1Jy-EIT6Fa9_xnWTOIyVfu-pdXpemMrahsiDW3A2MFMff9FuwKpV-EJ1-N4pfctx3EM2rnX1wAgGKIBfS1iLdCvus1VgYwpDbA", "Issue: " + value.Status.ToString(), "Fugue");


                        tepush.SendNotification_Android(usr.AndroidToken.ToString(), description.ToString(), apptype);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
                if (usr.IosToken != null)
                {
                    try
                    {
                        TECommonLogicLayer.TEPushNotification tepush = new TECommonLogicLayer.TEPushNotification();
                        //tepush.SendNotification_IOS("0c11cdc92d14b96d6f309cc661c4cd36abe9cb1f38bccf4019de31b6cf97992b", "Issue: " + value.Status.ToString(), "Yellow");
                        tepush.SendNotification_IOS(usr.IosToken.ToString(), "Issue: " + description.ToString(), apptype);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
            }
            catch (Exception extuyp) { }
            return new TENotificationsController().AddNotifications(notify);

        }

        //private int updateValue(TEQueueDepartment value, int IssueId)
        //{
        //    int result = 0;
        //    try
        //    {
        //        TEIssue issue = new TEIssue();
        //        issue.QueueID = value.QueueID;
        //        issue.AssignedTo = value.Default_assignee;
        //        issue.Uniqueid = IssueId
        //        using (var scope = new TransactionScope())
        //        {
        //            db.TEIssues.Attach(issue);

        //            foreach (PropertyInfo item in result.GetType().GetProperties())
        //            {
        //                string propname = item.Name;
        //                if (propname.ToLower() == "createdon")
        //                    continue;
        //                object propValue = item.GetValue(value);
        //                if (propValue != null || Convert.ToString(propValue).Length != 0)
        //                    db.Entry(value).Property(propname).IsModified = true;

        //            }

        //            value.LastModifiedOn = System.DateTime.Now;
        //            db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;

        //            db.SaveChanges();
        //            scope.Complete();
        //            result = 1;
        //        }
        //    }
        //    catch
        //    {
        //        result = 0;
        //    }
        //    return result;
        //}
        private int LogActivityTimeInfo(TEIssue value)
        {
            //var usr = db.UserProfiles.Where(x => (x.UserId == int.Parse(value.LastModifiedBy))).ToList().First();
            var status = "";
            if (value.Status == "QUEUED")
            {
                var repon = db.TEIssues.Where(x => (x.Uniqueid == value.Uniqueid)).ToList().First();
                if (repon != null)
                {
                    status = "Ticket Re-opened";
                }
                status = "Ticket Created";
            }
            //if (value.Status == "ACCEPTED")
            //{               
            //}
            if (value.Status == "COMMENCED")
            {
                status = "Started";
            }
            if (value.Status == "PAUSED")
            {
                status = "Ticket Paused";
            }
            if (value.Status == "COST APPROVAL")
            {
                status = "Cost involved. Approval required";
            }
            if (value.Status == "RESUMED")
            {
                status = "Resumed";
            }
            if (value.Status == "RESOLVED")
            {
                status = "Resolved";
            }
            if (value.Status == "CLOSED")
            {
                status = "Ticket Closed";
            }
            if (value.Status == "DISMISSED")
            {
                status = "Ticket Closed ";
            }
            if (value.Status == "REOPEN")
            {
                status = "Ticket Re-opened";
            }
            return new TECommonControllers.TEActivityTimelineInfoController()
                .Post(
                new TEActivityTimelineInfo
                {
                    CreatedBy = value.LastModifiedBy,
                    CreatedOn = System.DateTime.Now,
                    Date = System.DateTime.Now,
                    Description = status,
                    //Description =  value.Status,
                    IsDeleted = false,
                    // LastModifiedBy = usr.CallName,
                    LastModifiedBy = value.LastModifiedBy,
                    LastModifiedOn = System.DateTime.Now,
                    Type = value.GetType().ToString(),
                    ISSUEID = value.Uniqueid
                }
                ).Uniqueid;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        //public TEDocument DocumentPost(TEDocument doc)
        //{
        //    try
        //    {
        //        // Maintaining Folder structure 
        //        #region
        //        //Dummy data to test.
        //        //TEDocument doc = new TEDocument { DocumentType ="Education",DocumentSubType="MarkSheet"};
        //        string folder = "\\TEDocument\\";
        //        if (doc.DocumentType != null && (doc.DocumentType + "").Length > 0)
        //        {
        //            folder = folder + doc.DocumentType + "\\";
        //        }
        //        if (doc.DocumentSubType != null && (doc.DocumentSubType + "").Length > 0)
        //        {
        //            folder = folder + doc.DocumentSubType + "\\";
        //        }
        //        if (!(folder.Length > 1))
        //        {
        //            folder = "\\Images\\";
        //        }
        //        string Phyfolder = HttpContext.Current.Server.MapPath("~") + folder;
        //        #endregion

        //        // if it doesn't exist, create
        //        if (!Directory.Exists(Phyfolder))
        //            Directory.CreateDirectory(Phyfolder);

        //        //Get file from post method
        //        string result = null;

        //        db.ApplicationErrorLogs.Add(
        //         new ApplicationErrorLog
        //         {
        //             Error = doc.Path,
        //             ExceptionDateTime = System.DateTime.Now,
        //             //InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
        //             Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
        //             // Stacktrace = ex.StackTrace
        //         }
        //         );
        //        db.SaveChanges();

        //        if (Convert.ToString(doc.Path).Length > 0 && !(doc.Path.Contains("\\") && doc.Path.Contains("http:")))
        //        {
        //            string fileName = doc.DocumentType
        //                + doc.DocumentSubType
        //                + doc.KeyId
        //                + doc.DocumentName
        //                + ((System.DateTime.Now).ToString().Replace("/", "").Replace("\\", "").Replace(" ", "").Replace("-", "").Replace(":", ""));
        //            byte[] byteArray = Convert.FromBase64String(doc.Path);
        //            string filePath = SaveAnImage(byteArray, Phyfolder + fileName);
        //            //result = Request.CreateResponse(HttpStatusCode.Created, filePath);
        //            string prefix = ConfigurationManager.AppSettings["TEDocumentPrefix"];
        //            result = doc.Path = prefix + folder + fileName + ".png";
        //        }
        //        else if (doc.Path.Contains("\\") && doc.Path.Contains("http:"))
        //        {
        //            // Create a new WebClient instance.
        //            using (WebClient myWebClient = new WebClient())
        //            {
        //                folder = folder + "\\" + doc.DocumentName;
        //                // Download the Web resource and save it into the current filesystem folder.
        //                myWebClient.DownloadFile(doc.Path, folder);
        //            }
        //            //result = Request.CreateResponse(HttpStatusCode.Created, folder);
        //            result = doc.Path = result.ToString();
        //        }
        //        else
        //        {
        //            var httpRequest = HttpContext.Current.Request;
        //            if (httpRequest.Files.Count > 0)
        //            {
        //                var docfiles = new List<string>();
        //                foreach (string file in httpRequest.Files)
        //                {
        //                    var postedFile = httpRequest.Files[file];
        //                    var filePath = folder + postedFile.FileName;
        //                    postedFile.SaveAs(filePath);

        //                    docfiles.Add(filePath);

        //                    result = folder + postedFile.FileName;


        //                }
        //                //result = Request.CreateResponse(HttpStatusCode.Created, docfiles);

        //            }
        //            else
        //            {
        //                result = "";
        //            }
        //            result = doc.Path = result.ToString();
        //        }
        //        //Save path in DB.  
        //        //Update file path after saving file.



        //        new TEIssuesController().DocumentPost(doc);


        //    }
        //    catch (Exception ex)
        //    {
        //        db.ApplicationErrorLogs.Add(
        //            new ApplicationErrorLog
        //            {
        //                Error = ex.Message,
        //                ExceptionDateTime = System.DateTime.Now,
        //                InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
        //                Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
        //                Stacktrace = ex.StackTrace
        //            }
        //            );

        //    }
        //    db.SaveChanges();
        //    return doc;
        //}

        //public string SaveAnImage(byte[] byteArray, string folder)
        //{
        //    //  = Convert.FromBase64String(base64String); // Put the bytes of the image here....
        //    // System.Text.Encoding.Unicode.GetBytes(base64String);
        //    Image result = null;
        //    ImageFormat format = ImageFormat.Png;

        //    MemoryStream ms = new MemoryStream(byteArray);

        //    result = new Bitmap(ms);

        //    using (Image imageToExport = result)
        //    {
        //        // string filePath = string.Format(@"C:\test\Myfile.{0}", format.ToString());
        //        string filePath = string.Format(folder + ".{0}", format.ToString());

        //        imageToExport.Save(filePath, format);

        //        return filePath;
        //    }
        //}

        [HttpGet]
        public object GetTEissueAssigned(int Categoryid, int Queueid)
        {
            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teass in db.TEEmpAssignmentDetails on tebasic.Uniqueid equals teass.TEEmpBasicInfo
                     join tecomp in db.TECompanies on teass.TECompany equals tecomp.Uniqueid
                     join tedepart in db.TEDepartments on teass.TEDepartment equals tedepart.Uniqueid
                     join tesub in db.TESubFunctions on teass.TESubFunction equals tesub.Uniqueid
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where (teass.Status == "Active")
                     // && (uob.Project == cnt.proj)
                     //&&(uob.Uniqueid==cnt.Unitid)
                     orderby teass.TEEmpBasicInfo
                     select new
                     {
                         teuser.UserId,
                         teuser.CallName,
                         tebasic.Uniqueid
                     });

            List<TEEmployeeAssignee> list = new List<TEEmployeeAssignee>();
            foreach (var item in y)
            {
                list.Add(new TEEmployeeAssignee()
                {
                    Userid = item.UserId,
                    callname = item.CallName,
                    teempbasicinfo = item.Uniqueid
                });
            }
            return list;
        }
    }
}