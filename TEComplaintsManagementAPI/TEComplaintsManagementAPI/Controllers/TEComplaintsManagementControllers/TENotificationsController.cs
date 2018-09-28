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
using TECommonLogicLayer.TEModelling;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TENotificationsController : ApiController
    {
        //
        // GET: /TENotifications/
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/TEAllNotifications
        [HttpGet]
        public IEnumerable<TENotification> TEAllNotifications()
        {
            return db.TENotifications.Where(x => x.IsDeleted == false);
        }

        // GET api/TEAllNotifications
        [HttpGet]
        public IEnumerable<TENotification> TESelectNotifications(string  ReceivedBy)
        {
            int Received = Convert.ToInt32(ReceivedBy);
            return db.TENotifications.Where(x =>  (x.IsDeleted == false)                                       
                                                  &&
                                                (x.ReceivedBy == Received))
                                                .OrderByDescending(x => x.ReceivedBy);
        }

        // POST api/AddNotifications
        [HttpPost]
        public TENotification AddNotifications(TENotification value)
        {
            try
            {
                TENotification result = value;

                if (!(value.Uniqueid + "".Length > 0))
                {
                    //Create
                    result.CreatedOn = System.DateTime.Now;
                    result.LastModifiedOn = System.DateTime.Now;
                    result = db.TENotifications.Add(value);
                }
                else
                {
                    //IEnumerable<TENotification> updatenotif  = (db.TENotifications.Where(x=>(x.SendBy == value.SendBy)&&(x.ReadStatus == false)));

                    //foreach (System.Reflection.PropertyInfo value1 in updatenotif.GetType().GetProperties())
                    //{
                    //    //Edit
                    //    TENotification value1 = updatenotif(;
                    //    db = new TEHRIS_DevEntities();
                    //    db.TENotifications.Attach(value1);
                    //    foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                    //    {
                    //        string propname = item.Name;
                    //        if (propname.ToLower() == "createdon")
                    //            continue;
                    //        object propValue = item.GetValue(value);
                    //        if (propValue != null || Convert.ToString(propValue).Length != 0)
                    //            db.Entry(value).Property(propname).IsModified = true;
                    //    }

                    //    value.LastModifiedOn = System.DateTime.Now;
                    //    db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
                    //    db.SaveChanges();
                    //}
                    if (value.ReceivedBy != null)
                    {
                        foreach (var some in db.TENotifications.Where(x => (x.ReceivedBy == value.ReceivedBy) && (x.ReadStatus == false)).ToList())
                        {
                            some.ReadStatus = true;
                            some.LastModifiedOn = System.DateTime.Now;
                        }
                    }
                    else
                    {
                        //Edit
                        db = new TEHRIS_DevEntities();
                        db.TENotifications.Attach(value);
                        foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                        {
                            string propname = item.Name;
                            if (propname.ToLower() == "createdon")
                                continue;
                            object propValue = item.GetValue(value);
                            if (propValue != null || Convert.ToString(propValue).Length != 0)
                                db.Entry(value).Property(propname).IsModified = true;
                        }

                        value.LastModifiedOn = System.DateTime.Now;
                        db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
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
                        Source = "From TENotifications API | AddNotifications | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return db.TENotifications.Find(value.Uniqueid);
        }

        [HttpPost]
        public object SendNotifications(TENotificationModel value)
        {
            try
            {
                TEProjects_UNIT ProjectUnit = db.TEProjects_UNIT.Where(x => (x.PROJECT_ID == value.Project)
                                                                            && (x.TOWERID == Convert.ToInt32(value.Tower))
                                                                            && (x.AREA == value.unit)).ToList().First();
                if (ProjectUnit != null)
                {
                    List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)
                                                                            && (x.Towerid == value.Tower)
                                                                            && (x.Unitid == value.unit)).ToList();
                    if (contact != null)
                    {
                        foreach (var item in contact)
                        {
                             UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                             if (userprof != null)
                             {
                                 value.ReceivedBy = userprof.UserId;
                                 value.Status = "Active";
                                 SendNotification(value);
                             }
                        }
                    }
                }
                else
                {
                    List<TEProjects_TOWER> ProjectTower = db.TEProjects_TOWER.Where(x => (x.PROJECT_ID == value.Project)
                                                                            && (x.Uniqueid ==Convert.ToInt32(value.Tower))).ToList();
                    if (ProjectTower != null)
                    {
                        List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)
                                                                             && (x.Towerid == value.Tower)).ToList();
                        if (contact != null)
                        {
                            foreach (var item in contact)
                            {
                                UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                                if (userprof != null)
                                {
                                    value.ReceivedBy = userprof.UserId;
                                    value.Status = "Active";
                                    SendNotification(value);
                                }
                            }
                        }
                    }
                    else
                    { 
                        #region Commented due to entity change
                        //List<TEProject> Project = db.TEProjects.Where(x => (x.Uniqueid == value.Project)).ToList();
                        //if (ProjectTower != null)
                        //{
                        //    List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)).ToList();
                        //    if (contact != null)
                        //    {
                        //        foreach (var item in contact)
                        //        {
                        //            UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                        //            if (userprof != null)
                        //            {
                        //                value.ReceivedBy = userprof.UserId;
                        //                value.Status = "Active";
                        //                SendNotification(value);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                try
                {
                    List<TEProjects_TOWER> ProjectTower = db.TEProjects_TOWER.Where(x => (x.PROJECT_ID == value.Project)
                                                                           && (x.Uniqueid ==Convert.ToInt32( value.Tower))).ToList();
                    if (ProjectTower != null)
                    {
                        List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)
                                                                             && (x.Towerid == value.Tower)).ToList();
                        if (contact != null)
                        {
                            foreach (var item in contact)
                            {
                                UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                                if (userprof != null)
                                {
                                    value.ReceivedBy = userprof.UserId;
                                    value.Status = "Active";
                                    SendNotification(value);
                                }
                            }
                        }
                    }
                    else
                    {
                        #region Commented due to entity change
                        //List<TEProject> Project = db.TEProjects.Where(x => (x.Uniqueid == value.Project)).ToList();
                        //if (Project != null)
                        //{
                        //    List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)).ToList();
                        //    if (contact != null)
                        //    {
                        //        foreach (var item in contact)
                        //        {
                        //            UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                        //            if (userprof != null)
                        //            {
                        //                value.ReceivedBy = userprof.UserId;
                        //                value.Status = "Active";
                        //                SendNotification(value);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
                catch (Exception ex1)
                {
                    #region Commented due to entity change
                    //try
                    //{
                        
                        //List<TEProject> Project = db.TEProjects.Where(x => (x.Uniqueid == value.Project)).ToList();
                        //if (Project != null)
                        //{
                        //    List<TEContact> contact = db.TEContacts.Where(x => (x.Projectid == value.Project)).ToList();
                        //    if (contact != null)
                        //    {
                        //        foreach (var item in contact)
                        //        {
                        //            UserProfile userprof = db.UserProfiles.Where(x => (x.email == item.Emailid)).ToList().First();
                        //            if (userprof != null)
                        //            {
                        //                value.ReceivedBy = userprof.UserId;
                        //                value.Status = "Active";
                        //                SendNotification(value);
                        //            }
                        //        }
                        //    }
                             
                    //    }
                    //}
                    //catch(Exception ex2)
                    //{
                    //    db.ApplicationErrorLogs.Add(
                    //        new ApplicationErrorLog
                    //        {
                    //            Error = ex.Message,
                    //            ExceptionDateTime = System.DateTime.Now,
                    //            InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                    //            Source = "From TENotifications API | AddNotifications | " + this.GetType().ToString(),
                    //            Stacktrace = ex.StackTrace
                    //        }
                    //        );
                    //}
                #endregion
                }
               
            }

            db.SaveChanges();
            return "success";
        }
        private TENotification SendNotification(TENotificationModel value)
        {
            var usr = db.UserProfiles.Where(x => x.UserId == Convert.ToInt32(value.SendBy)).FirstOrDefault();
            string description1 = "Issue: " + value.description + " " + usr.CallName.ToString();

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
                description = description1,
                ReceivedBy = value.ReceivedBy,
                SendBy = value.SendBy,
                ReadStatus = false,
                Status = value.Status,
                //Type

            };

            var usrtype = (from usrs in db.UserProfiles
                           join tebas in db.TEEmpBasicInfoes on usrs.UserName equals tebas.UserId
                           where (usrs.UserId == Convert.ToInt32( value.SendBy))
                           select usrs.UserId).Distinct().FirstOrDefault();
            string apptype = "Fugue";
            if (usrtype == null)
            {
                apptype = "Yellow";
            }
            string res = "";
            if (usr.AndroidToken != null)
            {
                try
                {
                    TECommonLogicLayer.TEPushNotification tepush = new TECommonLogicLayer.TEPushNotification();
                    //tepush.SendNotification_Android("APA91bEWoz9U6m9VK-oBW1Jy-EIT6Fa9_xnWTOIyVfu-pdXpemMrahsiDW3A2MFMff9FuwKpV-EJ1-N4pfctx3EM2rnX1wAgGKIBfS1iLdCvus1VgYwpDbA", "Issue: " + value.Status.ToString(), "Fugue");
                    res = tepush.SendNotification_Android(usr.AndroidToken.ToString(), description1, apptype);

                    //AndroidNotification.Notification tepushandroid = new AndroidNotification.Notification();
                    //res=tepushandroid.AndroidNotification(usr.AndroidToken.ToString(), description1);
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
                    tepush.SendNotification_IOS(usr.IosToken.ToString(), description1, apptype);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return new TENotificationsController().AddNotifications(notify);


        }
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IEnumerable<TENotificationModel> teGetNotifications()
        {
            List<TENotificationModel> result = new List<TENotificationModel>();
            result = (from noti in db.TENotifications
                      join usr in db.UserProfiles on noti.ReceivedBy equals usr.UserId
                      //from tec in db.TEContacts   where noti.ReceivedBy == tec.UserId
                      from iss in db.TEIssues
                      where noti.ReceivedBy == iss.RaisedBy
                      from tep in db.TEProjects
                      //where iss.ProjectID == tep.Uniqueid
                      where noti.IsDeleted == false
                      && noti.ModuleUniqueid == iss.Uniqueid && noti.Module == "Issue"
                      //&& ( (iss.ProjectID==1) && (iss.UnitID =="1008"))
                      orderby noti.Uniqueid descending
                      select new TENotificationModel()
                                {
                                    CreatedBy = noti.CreatedBy,
                                    CreatedOn = noti.CreatedOn,
                                    LastModifiedOn = noti.LastModifiedOn,
                                    LastModifiedBy = noti.LastModifiedBy,
                                    IsDeleted = noti.IsDeleted,
                                    Uniqueid = noti.Uniqueid,
                                    //Name = noti.Name,
                                    description = noti.description,
                                    ReceivedBy = noti.ReceivedBy,
                                    SendBy = noti.SendBy,
                                    Status = noti.Status,
                                    ReadStatus = noti.ReadStatus,
                                    Type = noti.Type,
                                    Name = usr.CallName,
                                    Project = iss.ProjectID,
                                    ProjectName = tep.ProjectName,
                                    unit = iss.UnitID
                                }).Distinct().ToList();
            //foreach (var item in listOfEntity)
            //{
            //    TENotificationModel tempModel = new TENotificationModel();
            //    TETransformEntityNModel translator = new TETransformEntityNModel();
            //    tempModel = translator.TransformAtoB(item, tempModel);
            //    result.Add(tempModel);
            //}

            return result;
        }
        [HttpGet]
        public IEnumerable<TENotificationModel> GetNotificationFilterBy(int ReceivedBy, string filterBy = "")
        {
            IEnumerable<TENotificationModel> result ;
            IEnumerable<TENotificationModel> tenotoficationsresult = teGetNotifications().Where(x => (x.ReceivedBy == ReceivedBy)).ToList();
            try
            {
                string[] filtersplit = filterBy.Split('$');
                string[] filtersub;
                string[] Project = new string[20];
                string[] Unit = new string[20];
                string[] Date = new string[2];
                DateTime Datefrom = System.DateTime.Now;
                DateTime Dateto = System.DateTime.Now;
                for (int i = 0; i <= filtersplit.Length - 1; i++)
                {
                    filtersub = filtersplit[i].Split(':');

                    if (filtersub[0] == "Project")
                    {
                        Project = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Unit")
                    {
                        Unit = filtersub[1].Split(',');
                    }
                    if (filtersub[0] == "Date")
                    {
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

                if (Project[0] != null)
                {
                    tenotoficationsresult = tenotoficationsresult.Where(x => Project.Contains(x.ProjectName));
                }
                if (Unit[0] != null)
                {
                    tenotoficationsresult = tenotoficationsresult.Where(x => Unit.Contains(x.unit));
                }
                if (Date[0] != null)
                {
                    tenotoficationsresult = tenotoficationsresult.Where(x => x.CreatedOn >= Datefrom && x.CreatedOn <= Dateto);

                }
                tenotoficationsresult = tenotoficationsresult.OrderBy(x => x.Uniqueid).ToList();
              
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
            result = tenotoficationsresult;
            return result;
        }
    }
}
