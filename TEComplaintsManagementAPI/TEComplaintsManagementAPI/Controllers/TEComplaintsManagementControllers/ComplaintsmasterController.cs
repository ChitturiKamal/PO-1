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
    public class ComplaintsmasterController : ApiController
    {
        //
        // GET: /Complaintsmaster/
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // Get api/getTEUnit_Onboarding
        [HttpGet]
        public IEnumerable<TEUnit_Onboarding>SelectAllUnitOnboarding()
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
                                        );
        }

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
                    result.CreatedOn = System.DateTime.UtcNow;
                    result.LastModifiedOn = System.DateTime.UtcNow;
                    result = db.TEUnit_Onboarding.Add(value);
                }
                else
                {
                    //Edit
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

                    value.LastModifiedOn = System.DateTime.UtcNow;
                    db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.UtcNow,
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
