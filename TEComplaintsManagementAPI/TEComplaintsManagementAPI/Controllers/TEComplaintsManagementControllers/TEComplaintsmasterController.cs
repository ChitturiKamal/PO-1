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

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEComplaintsmasterController : ApiController
    {
        //
        // GET: /Complaintsmaster/
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // Get api/getTEUnit_Onboarding
        [HttpGet]
        public IEnumerable<TEUnit_Onboarding> SelectAllUnitOnboarding()
        {
            return db.TEUnit_Onboarding.Where(x => (x.IsDeleted == false)
                                        &&
                                        (x.Status == "Inactive")
                                        );
        }
        // Get api/getTEUnit_Onboarding
        [HttpGet]
        public IEnumerable<TEUnit_Onboarding> SelectUnit_Onboarding(int Uniqueid)
        {
            return db.TEUnit_Onboarding.Where(x => (x.IsDeleted == false)
                                        &&
                                        (x.Status == "Inactive")
                                        &&
                                        (x.Uniqueid == Uniqueid)
                                        ).OrderByDescending(x => x.Project);
        }
        [HttpGet]
        public IEnumerable<TEUnitModel> verifyUnit_Onboarding(int UserID)
        {
            var y = (from uob in db.TEUnit_Onboarding
                     join up in db.UserProfiles
                     on uob.UserID equals up.UserId
                     // join cnt in db.TEContacts
                     //on uob.UserID equals cnt.Contactid 
                     where (uob.UserID == UserID)
                      &&
                       (uob.Status == "Inactive")
                     // && (uob.Project == cnt.proj)
                     //&&(uob.Uniqueid==cnt.Unitid)
                     orderby uob.Project
                     select new { uob.Uniqueid,uob.Project,uob.Unit,uob.UserID,up.CallName, up.email, up.Phone, up.photo, up.Password }).ToList();


            List<TEUnitModel> list = new List<TEUnitModel>();
            foreach (var item in y)
            {
                list.Add(new TEUnitModel()
                {
                    UNITONBOARDINGID = item.Uniqueid,
                    ProjectID =item.Project,
                    UnitID=item.Unit,
                    Userid = item.UserID,
                    CallName = item.CallName,
                    Email = item.email,
                    Phone = item.Phone,
                    photo = item.photo
                });
            }
            return list;           
        }
        //[HttpGet]
        //public IEnumerable<TEContact> verifycontact(int Projectid, string Unitid)
        //{
        //    //return db.TEContacts.Where(x => (x.Projectid == Projectid)
        //    //                           &&
        //    //                           (x.Unitid == Unitid)
        //    //                           );
        //    var y = (from  cnt in db.TEContacts
        //             join proj in db.TEProjects
        //             on cnt.Projectid equals proj.Uniqueid
        //             where (cnt.Projectid == Projectid)
        //              &&
        //               (cnt.Unitid == Unitid)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby cnt.Projectid
        //             select new { }).ToList();

        //    return y.AsEnumerable().Cast<TEContact>().ToList();
        //}
        // POST api/AddTEUnit_Onboarding
        [HttpPost]
        public TEUnit_Onboarding AddUnit_Onboarding(TEUnit_Onboarding value)
        {
            try
            {
                TEUnit_Onboarding result = value;

                if (!(value.Uniqueid + "".Length > 0))
                {
                    //Create
                    result.CreatedOn = System.DateTime.Now;
                    result.LastModifiedOn = System.DateTime.Now;
                    result = db.TEUnit_Onboarding.Add(value);
                }
                else
                {
                    //Edit
                    db = new TEHRIS_DevEntities();
                    db.TEUnit_Onboarding.Attach(value);
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
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEUnitonboard| AddUnit_Onboarding | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return db.TEUnit_Onboarding.Find(value.Uniqueid);
        }

    }
}
