using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models; 

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsMDMControllers
{
    public class TEUnitOnBoardingController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpPost]
        public TEUnit_Onboarding AddTEUnitOnboarding(TEUnit_Onboarding value)
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

                if (value.Status == "Active")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    TEUnit_Onboarding unit = db.TEUnit_Onboarding.Where(x => x.Uniqueid == value.Uniqueid).ToList().First();
                    if (unit != null)
                    {
                        string email = (from teuser in db.UserProfiles
                                        where (teuser.UserId == unit.UserID)
                                        select teuser.email).ToList().First();
                        //string tecontact = (from teoint in db.TEContacts
                        //                    where ((teoint.Emailid == email) && (teoint.Projectid == value.Project) && (teoint.Unitid == value.Unit))
                        //                    select teoint.CallName).ToList().First();
                        //if (tecontact == null)
                        try
                        {
                            TEContact contact = db.TEContacts.Where(x => (x.Emailid == email)
                                                                    && (x.Projectid == value.Project)
                                                                    && (x.Unitid == value.Unit)).ToList().First();
                            if (contact != null)
                            {
                                SendContact(value);
                            }
                        }
                        catch(Exception ex)
                        { SendContact(value);
                        ex.Message.ToString();
                        }
                    }
                }
            }

            db.SaveChanges();

            //CRUD Mobile
            //CRUD Email
            //CRUD Address 

            return db.TEUnit_Onboarding.Find(value.Uniqueid);
        }
        public object SendContact(TEUnit_Onboarding value)
        {

            TEUnit_Onboarding unit = db.TEUnit_Onboarding.Where(x => x.Uniqueid == value.Uniqueid).ToList().First();
            UserProfile userinfo = db.UserProfiles.Where(x => x.UserId == unit.UserID).ToList().First();

            if (userinfo != null)
            {
                int? tower=0;
                try
                {
                    TEProjects_UNIT projectunit = db.TEProjects_UNIT.Where(x => (x.PROJECT_ID == value.Project)
                                                                      && (x.AREA == value.Unit)).ToList().First();
                    if (projectunit != null)
                    {
                        tower = projectunit.TOWERID;
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                TEContact contact = new TEContact
{
    ApprovedBy = userinfo.CallName,
    ApprovedOn = System.DateTime.Now,
    CreatedBy = userinfo.CallName,
    CreatedOn = System.DateTime.Now,
    //Have to be dynamic...

    IsDeleted = false,
    LastModifiedBy = userinfo.CallName,
    LastModifiedOn = System.DateTime.UtcNow,
    Projectid = value.Project,
    Towerid=tower,
    Unitid = value.Unit,
    Mobile = userinfo.Phone,
    Emailid = userinfo.email,
    DOB = System.DateTime.Now,
    DOM = System.DateTime.Now,
    Type = "",
    PrefferedContact = "",
    PrefferedMode = "",
    ISProfileSPublic = false,
    IsAdvertise = false,
    Photo = userinfo.photo,
    Salutation = "",
    FirstName = "",
    MiddleName = "",
    LastName = "",
    CallName = userinfo.CallName,
    ContactType = "",
    Nationality = "",
    CountryOfBirth = "",
    Age = 0,
    Gender = "",
    MaritalStatus = "",
    Category = "",
    Importance = "",
    PrefferedSaleConsultant = "",
    TEOrganisation = 0,
    Status = value.Status,
    //Type

};
                return db.TEContacts.Add(contact);
            }
            return "Success";
        }
        #region Commented due to entity change
        //[HttpGet]
        //public IEnumerable<TEUnit_Onboardingmodel> GetTEUnitonboardingbyid(int Uniqueid)
        //{
        //    var y = (from teboard in db.TEUnit_Onboarding
        //             join teuser in db.UserProfiles on teboard.UserID equals teuser.UserId
        //             join teproj in db.TEProjects on teboard.Project equals teproj.Uniqueid
        //             where ((teboard.Uniqueid == Uniqueid) || (teboard.UserID == Uniqueid))
        //             && (teboard.Status != "Rejected")
        //             &&
        //            (teboard.IsDeleted == false)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby teboard.Project
        //             select new
        //             {
        //                 teboard.Uniqueid,
        //                 teboard.UserID,
        //                 teuser.CallName,
        //                 teuser.email,
        //                 teuser.Phone,
        //                 teboard.Project,
        //                 teproj.ProjectName,
        //                 teproj.COLOURCODE,
        //                 teboard.Unit,
        //                 teboard.Status,
        //                 teboard.PMDAPPROVEDBY,
        //                 teboard.PMDAPPROVEDREASON,
        //                 teproj.City
        //             });

        //    List<TEUnit_Onboardingmodel> list = new List<TEUnit_Onboardingmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEUnit_Onboardingmodel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            UserID = item.UserID,
        //            Project = item.Project,
        //            ProjectName = item.ProjectName,
        //            Unit = item.Unit,
        //            COLOURCODE = item.COLOURCODE,
        //            CallName = item.CallName,
        //            PMDAPPROVEDBY = item.PMDAPPROVEDBY,
        //            PMDAPPROVEDREASON = item.PMDAPPROVEDREASON,
        //            Status = item.Status,
        //            ProjectCity=item.City
        //        });
        //    }
        //    return list;
        //}
        
        //[HttpGet]
        //public IEnumerable<TEUnit_Onboardingmodel> GetTEUnitonboarding()
        //{
        //    var y = (from teboard in db.TEUnit_Onboarding
        //             join teuser in db.UserProfiles on teboard.UserID equals teuser.UserId
        //             join teproj in db.TEProjects on teboard.Project equals teproj.Uniqueid
        //             where (teboard.Status == "Inactive")
        //             &&
        //            (teboard.IsDeleted == false)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby teboard.Uniqueid descending
        //             select new
        //             {
        //                 teboard.Uniqueid,
        //                 teboard.UserID,
        //                 teuser.CallName,
        //                 teuser.email,
        //                 teuser.Phone,
        //                 teboard.Project,
        //                 teproj.ProjectName,
        //                 teproj.COLOURCODE,
        //                 teboard.Unit,
        //                 teboard.Status,
        //                 teboard.PMDAPPROVEDBY,
        //                 teboard.PMDAPPROVEDREASON,
        //                 teproj.City
        //             });

        //    List<TEUnit_Onboardingmodel> list = new List<TEUnit_Onboardingmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEUnit_Onboardingmodel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            UserID = item.UserID,
        //            Project = item.Project,
        //            ProjectName = item.ProjectName,
        //            Unit = item.Unit,
        //            COLOURCODE = item.COLOURCODE,
        //            CallName = item.CallName,
        //            PMDAPPROVEDBY = item.PMDAPPROVEDBY,
        //            PMDAPPROVEDREASON = item.PMDAPPROVEDREASON,
        //            Status = item.Status,
        //            ProjectCity=item.City,
        //            Email=item.email,
        //            Phone=item.Phone
        //        });
        //    }
        //    return list;
        //}

        //[HttpGet]
        //public IEnumerable<TEUnit_Onboardingmodel> GetActiveTEUnitonboardingByUserId(int UserID)
        //{
        //    var y = (from teboard in db.TEUnit_Onboarding
        //             join teuser in db.UserProfiles on teboard.UserID equals teuser.UserId
        //             join teproj in db.TEProjects on teboard.Project equals teproj.Uniqueid
        //             where ((teboard.Uniqueid == UserID) || (teboard.UserID == UserID))
        //             && (teboard.Status == "Active")
        //             &&
        //            (teboard.IsDeleted == false)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby teboard.Project
        //             select new
        //             {
        //                 teboard.Uniqueid,
        //                 teboard.UserID,
        //                 teuser.CallName,
        //                 teuser.email,
        //                 teuser.Phone,
        //                 teboard.Project,
        //                 teproj.ProjectName,
        //                 teproj.COLOURCODE,
        //                 teboard.Unit,
        //                 teboard.Status,
        //                 teboard.PMDAPPROVEDBY,
        //                 teboard.PMDAPPROVEDREASON,
        //                 teproj.City
        //             });

        //    List<TEUnit_Onboardingmodel> list = new List<TEUnit_Onboardingmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEUnit_Onboardingmodel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            UserID = item.UserID,
        //            Project = item.Project,
        //            ProjectName = item.ProjectName,
        //            Unit = item.Unit,
        //            COLOURCODE = item.COLOURCODE,
        //            CallName = item.CallName,
        //            PMDAPPROVEDBY = item.PMDAPPROVEDBY,
        //            PMDAPPROVEDREASON = item.PMDAPPROVEDREASON,
        //            Status = item.Status,
        //            ProjectCity = item.City
        //        });
        //    }
        //    return list;
        //}
        //[HttpGet]
        //public IEnumerable<TEUnit_Onboardingmodel> GetTEUnitonboardingByUserId(int UserID)
        //{
        //    var y = (from teboard in db.TEUnit_Onboarding
        //             join teuser in db.UserProfiles on teboard.UserID equals teuser.UserId
        //             join teproj in db.TEProjects on teboard.Project equals teproj.Uniqueid
        //             where ((teboard.Uniqueid == UserID) || (teboard.UserID == UserID))
        //            && (teboard.Status == "Active")
        //             &&
        //            (teboard.IsDeleted == false)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby teboard.Project
        //             select new
        //             {
        //                 teboard.UserID,
        //                 teuser.CallName,
        //                 teuser.email,
        //                 teuser.Phone,
        //                 teboard.Project,
        //                 teproj.ProjectName,
        //                 teproj.COLOURCODE,
        //                 teboard.Status,
        //                 teproj.City
        //             });

        //    List<TEUnit_Onboardingmodel> list = new List<TEUnit_Onboardingmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEUnit_Onboardingmodel()
        //        {
        //            UserID = item.UserID,
        //            Project = item.Project,
        //            ProjectName = item.ProjectName,
        //            COLOURCODE = item.COLOURCODE,
        //            CallName = item.CallName,
        //            Status = item.Status,
        //            ProjectCity = item.City
        //        });
        //    }
        //    return list;
        //}
        #endregion
        [HttpGet]
        public IEnumerable<TEUnit_Onboarding> GetUnitOnboardingByProjectId(int ProjectId)
        {
            return db.TEUnit_Onboarding.Where(x => (x.Project == ProjectId)
                                                &&
                                                    (x.IsDeleted == false));
        }
        [HttpGet]
        public IEnumerable<TEUnit_Onboarding> GetUnitOnboardingByunitId(int UserID, int ProjectId, string Unit)
        {
            return db.TEUnit_Onboarding.Where(x => (x.UserID == UserID)
                                                &&
                                                (x.Project == ProjectId)
                                                &&
                                                (x.Unit == Unit)
                                                &&
                                                    (x.IsDeleted == false));
        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}