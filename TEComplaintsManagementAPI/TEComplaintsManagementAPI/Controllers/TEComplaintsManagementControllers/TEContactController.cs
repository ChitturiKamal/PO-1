using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TECommonLogicLayer.TEModelling;
using TEComplaintsManagementAPI.Models;
using System.Data.Linq.SqlClient;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
  
    public class TEContactController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEContact> Get()
        {
            return db.TEContacts.Where(x => x.IsDeleted == false);
        }
        #region Commented due to entity change
        //[HttpGet]
        //public IEnumerable<TEContactModel> Getcontact(string contactid)
        //{
           
            //var y = (from cnt in db.TEContacts
            //         join proj in db.TEProjects
            //         on cnt.Projectid equals proj.Uniqueid
            //         where (cnt.Contactid == contactid)
            //         // && (uob.Project == cnt.proj)
            //         //&&(uob.Uniqueid==cnt.Unitid)
            //         orderby cnt.Projectid
            //         select new
            //         {
            //             cnt.Uniqueid,
            //             proj.ProjectName,
            //             cnt.Projectid,
            //             cnt.Unitid,
            //             cnt.Type,
            //             cnt.Salutation,
            //             cnt.FirstName,
            //             cnt.LastName,
            //             cnt.CallName,
            //             cnt.Mobile,
            //             cnt.Emailid,
            //             cnt.Photo
            //         });

            //List<TEContactModel> list = new List<TEContactModel>();
            //foreach (var item in y)
            //{
            //    list.Add(new TEContactModel()
            //    {
            //        Uniqueid = item.Uniqueid,
            //        Projectid = item.Projectid,
            //        ProjectName = item.ProjectName,
            //        Unitid = item.Unitid,
            //        Type = item.Type,
            //        Salutation = item.Salutation,
            //        FirstName = item.FirstName,
            //        LastName = item.LastName,
            //        CallName = item.CallName,
            //        Emailid = item.Emailid,
            //        Mobile = item.Mobile,
            //        Photo = item.Photo
            //    });
            //}
             
        //    return list;  
        //}
        //[HttpGet]
        //public IEnumerable<TEContactModel> GetcontactIndividual(int contactid)
        //{

        //    var y = (from cnt in db.TEContacts
        //             join proj in db.TEProjects
        //             on cnt.Projectid equals proj.Uniqueid
        //             where (cnt.Uniqueid == contactid)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby cnt.Projectid
        //             select new
        //             {
        //                 cnt.Uniqueid,
        //                 proj.ProjectName,
        //                 cnt.Projectid,
        //                 cnt.Unitid,
        //                 cnt.Type,
        //                 cnt.Salutation,
        //                 cnt.FirstName,
        //                 cnt.LastName,
        //                 cnt.CallName,
        //                 cnt.Mobile,
        //                 cnt.Emailid,
        //                 cnt.Photo
        //             });

        //    List<TEContactModel> list = new List<TEContactModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEContactModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            Projectid = item.Projectid,
        //            ProjectName = item.ProjectName,
        //            Unitid = item.Unitid,
        //            Type = item.Type,
        //            Salutation = item.Salutation,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            CallName = item.CallName,
        //            Emailid = item.Emailid,
        //            Mobile = item.Mobile,
        //            Photo = item.Photo
        //        });
        //    }
        //    return list;
        //}
        #endregion
        // GET api/<controller>/5
        [HttpGet]
        public TEContact Get(int id)
        {
            return db.TEContacts.Find(id);
        }
       
       

        [HttpGet]
        public object GetTEImportantContacts(int projectId,string Category)
        {
              try
            {
            return db.TEImportantContacts.Where(x =>
                                        (x.TEProject == projectId)
                                        &&
                                        (x.category==Category)
                                        );
            //return db.TEImportantContacts.Where(x => x.IsDeleted == false);
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
             return   db.UserProfiles.Select(x => new { Message="No contact info"});
        }

        [HttpGet]
        public object GetTEImportantContactsbycategory(int projectId, string Category)
        {
            try
            {
                var y = db.TEImportantContacts.Where(x =>
                                            (x.TEProject == projectId)
                                            &&
                                            (x.category == Category)
                                            )
                    //.sGroupBy(x => x.subcategory).Select(g => g.First())
                                            .Select(x => new { x.subcategory })
                                            .Distinct()
                                        .ToList();
                List<TEImportantContact> list = new List<TEImportantContact>();
                foreach (var item in y)
                {
                    list.Add(new TEImportantContact()
                    {
                        TEProject = projectId,
                        category = Category,
                        subcategory = item.subcategory,
                    });
                }
                return list;  
                //return db.TEImportantContacts.GroupBy(x => x.subcategory).Select(x => x.First());
                //return db.TEImportantContacts.Where(x => x.IsDeleted == false);
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
            return db.UserProfiles.Select(x => new { Message = "No contact info" });
        }

        [HttpGet]
        public object GetTEImportantContactsbysubcategory(int projectId, string SubCategory)
        {
            try
            {
                return db.TEImportantContacts.Where(x =>
                                            (x.TEProject == projectId)
                                            &&
                                            (x.subcategory == SubCategory)
                                            );
                //return db.TEImportantContacts.Where(x => x.IsDeleted == false);
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
            return db.UserProfiles.Select(x => new { Message = "No contact info" });
        }
        // POST api/<controller>
        [HttpPost]
        public TEContact PostContact(TEContact value)
        {

            TEContact result = value;
            try{
            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEContacts.Add(value);
            } 
            else
            {
                //Edit
                db.TEContacts.Attach(value);
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
                        Source = "From TEQUEUETechnician API | TEQUEUETechnician | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            
            //CRUD Mobile
            //CRUD Email
            //CRUD Address 

            return db.TEContacts.Find(value.Uniqueid);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        
        [HttpGet]
        public object userprofile(string Emailid, string Password)
        {
             try
            {
            var result= db.UserProfiles.Where(x =>
                                        (x.email == Emailid)
                                        &&
                                        (x.Password == Password)
                                        ).ToList();
            var usr = db.UserProfiles.Where(x => (x.email == Emailid)).ToList();
            var pswd = db.UserProfiles.Where(x => (x.Password == Password)).ToList();

            if (usr.Count== 0)               
                return db.UserProfiles.Select(x =>new { Message = "Invalid username." }).FirstOrDefault();
            else if (pswd.Count == 0)
                return db.UserProfiles.Select(x => new { Message = "Invalid password." }).FirstOrDefault();
            else if (result.Count >= 1)
                return result.First();
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
             return db.UserProfiles.Select(x => new { Message = "Invalid user credentials" }).FirstOrDefault();
        }

        [HttpGet]
        public object Userprofilebyemployeeid(int Employeeid)
        {
            string username = "";
            db.Configuration.ProxyCreationEnabled = true;
            var callnme = db.TEEmpBasicInfoes.Where(x => (x.Uniqueid == Employeeid)).Distinct().First();
            username = callnme.UserId;
            //UserProfile profile = db.UserProfiles.Where(x => x.UserName == username).First();
            ////UserProfile profile = db.UserProfiles.Find(UserId);
            var profile = (from usr in db.UserProfiles
                          join cnt in db.TEContacts on usr.UserId equals cnt.UserId
                          where usr.UserName == username
                          select new
                          {
                              usr.UserId,
                              usr.UserName,
                              usr.email,
                              usr.Phone,
                              usr.AndroidToken,
                              usr.IosToken,
                              usr.Password,
                              usr.facebookid,
                              usr.googleid,
                              usr.CallName,
                              usr.photo,
                              cnt.ISProfileSPublic
                          }).Distinct().First();
            return profile;
            
        }

        [HttpGet]
        public object userprofilebyuserid(int Userid)
        {
            try
            {
                //var result = db.UserProfiles.Where(x =>
                //                            (x.UserId == Userid)
                //                            ).ToList();
                var result = (from usr in db.UserProfiles
                              join cnt in db.TEContacts on usr.UserId equals cnt.UserId
                              where usr.UserId == Userid
                              select new
                              {
                                  usr.UserId,
                                  usr.UserName,
                                  usr.email,
                                  usr.Phone,
                                  usr.AndroidToken,
                                  usr.IosToken,
                                  usr.Password,
                                  usr.facebookid,
                                  usr.googleid,
                                  usr.CallName,
                                  usr.photo,
                                  cnt.ISProfileSPublic
                              }).Distinct().ToList();

                if (result.Count == 0)
                    return db.UserProfiles.Select(x => new { Message = "username do not match!" }).FirstOrDefault();
                else if (result.Count >= 1)
                    return result.First();
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
            return db.UserProfiles.Select(x => new { Message = "No user Info" }).FirstOrDefault();
        }
        [HttpGet]
        public object userprofilebyemail(string email)
        {
            try
            {
                //var result = db.UserProfiles.Where(x =>
                //                            (x.email == email)
                //                            ).ToList();
                var result = (from usr in db.UserProfiles
                              join cnt in db.TEContacts on usr.UserId equals cnt.UserId
                              where usr.email == email
                              select new
                              {
                                  usr.UserId,
                                  usr.UserName,
                                  usr.email,
                                  usr.Phone,
                                  usr.AndroidToken,
                                  usr.IosToken,
                                  usr.Password,
                                  usr.facebookid,
                                  usr.googleid,
                                  usr.CallName,
                                  usr.photo,
                                  cnt.ISProfileSPublic
                              }).Distinct().ToList();

                if (result.Count == 0)
                    return db.UserProfiles.Select(x => new { Message = "username do not match!" }).FirstOrDefault();
                else if (result.Count >= 1)
                    return result.First();
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
            return db.UserProfiles.Select(x => new { Message = "No user Info" }).FirstOrDefault();
        }
        [HttpPost]
        public UserProfile UpdateUserProfilePassword(UserProfile value)
        {
            try
            {
                 UserProfile usrprof = (from x in db.UserProfiles
                                              where x.email == value.email
                                              select x).Single();
                    usrprof.Password = value.Password;
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From AddUserProfile API | AddUserProfile | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return db.UserProfiles.Find(value.UserId);
        }

        [HttpPost]
        public TEContact updateIsProfilePublic(TEContact value)
        {
            try
            {
                TEContact result = value;
                  if (value.UserId != null)
                    {
                        foreach (var some in db.TEContacts.Where(x => (x.UserId == value.UserId) ).ToList())
                        {
                            some.ISProfileSPublic = value.ISProfileSPublic;
                            some.LastModifiedOn = System.DateTime.Now;
                        }
                    }
                  db.SaveChanges();
                  result.CallName = "Inserted Suuccessfully";
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
                        Source = "From AddUserProfile API | AddUserProfile | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return db.TEContacts.Find(value.UserId);
        }
        // POST api/AddNotifications
        [HttpPost]
        public object AddUserProfile(UserProfile value)
        {

            try
            {
                UserProfile result = value;
                if (value.email == null)
                {
                    result.UserName = result.email + " , cannot be null";
                    return result;
                }                    

                var usr = db.UserProfiles.Where(x => (x.email ==value.email)).ToList();
               
                //if (!(value.UserId + "".Length > 0))
                if (usr.Count == 0)
                {
                    //Create
                    //result.CreatedOn = System.DateTime.Now;
                    //result.LastModifiedOn = System.DateTime.Now;
                    result = db.UserProfiles.Add(value);
                    db.SaveChanges();

                    int urid = (from usrid in db.UserProfiles where usrid.email == value.email select usrid.UserId).FirstOrDefault();
                    value.UserId = urid;
                    var iscnt = (from cnt in db.TEContacts where cnt.Emailid == value.email select cnt.Emailid);
                    if (value.email!=iscnt.ToString())
                    {
                        AddUserContact(value);
                    }
                }
                else if (!(value.UserId + "".Length > 0)){
                    result.UserName = "Already exists";
                    return result;
                }
               
                else
                {
                    //Edit
                    ////UserProfile usrprof = (from x in db.UserProfiles
                                              //where x.email == value.email
                                              //select x).Single();
                    // string propname = value.UserName;
                    //object propValue = value.UserName;
                    ////    if (propValue != null || Convert.ToString(prop
                    //if (propValue != null || Convert.ToString(propValue).Length != 0)
                    //    db.Entry(value).Property(propname).IsModified = true;
                    ////usrprof.UserName = value.UserName;
                    ////usrprof.CallName = value.CallName;
                    ////usrprof.Phone = value.Phone;
                    ////usrprof.photo = value.photo;
                    ////usrprof.Password = value.Password;
                    ////usrprof.facebookid = value.facebookid;
                    ////usrprof.googleid = value.googleid;
                    db = new TEHRIS_DevEntities();
                    db.UserProfiles.Attach(value);
                    foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                    {
                        string propname = item.Name;
                        if ((propname.ToLower() == "email") || (propname.ToLower() == "UserId"))
                            continue;
                        object propValue = item.GetValue(value);
                        if (propValue != null || Convert.ToString(propValue).Length != 0)
                            db.Entry(value).Property(propname).IsModified = true;
                    }

                    //value.LastModifiedOn = System.DateTime.Now;
                    //db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
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
                        Source = "From AddUserProfile API | AddUserProfile | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            //return db.UserProfiles.Find(value.UserId);
            return (from usr in db.UserProfiles
                    join cnt in db.TEContacts on usr.UserId equals cnt.UserId
                    where usr.email == value.email
                    select new
                    {
                        usr.UserId,
                        usr.UserName,
                        usr.email,
                        usr.Phone,
                        usr.AndroidToken,
                        usr.IosToken,
                        usr.Password,
                        usr.facebookid,
                        usr.googleid,
                        usr.CallName,
                        usr.photo,
                        cnt.ISProfileSPublic
                    }).Distinct().FirstOrDefault();
        }

        [HttpPost]
        public TEContact AddUserContact(UserProfile value)
        {
            TEContact result = null;
            try
            {
                result = new TEContact
                {
                    ApprovedBy = value.CallName,
                    ApprovedOn = System.DateTime.UtcNow,
                    CreatedBy = value.CallName,
                    CreatedOn = System.DateTime.UtcNow,
                    LastModifiedBy = value.CallName,
                    LastModifiedOn = System.DateTime.UtcNow,
                    IsDeleted = false,
                    //Have to be dynamic...                     

                    Age = null,
                    CallName = value.CallName,
                    Category = null,
                    Contactid = null,
                    ContactType = null,
                    CountryCode = null,
                    CountryOfBirth = null,
                    DOB = null,
                    DOM = null,
                    Emailid = value.email,
                    FirstName = "",
                    Gender = "",
                    Importance = "",
                    IsAdvertise = true,
                    ISProfileSPublic = false,
                    LastName = null,
                    MaritalStatus = null,
                    MiddleName = null,
                    Mobile = value.Phone,
                    Nationality = null,
                    Objectid = "TEContact",
                    OldUniqueid = null,
                    Photo = value.photo,
                    PrefferedContact = null,
                    PrefferedMode = null,
                    PrefferedSaleConsultant = null,
                    Projectid = null,
                    Salutation = null,
                    Status = null,
                    UserId=value.UserId

                };
                    var y = db.TEContacts.Add(result);
                 db.SaveChanges();
                 result.CallName = "Success";
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From AddUserProfile API | AddUserProfile | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }
        //[HttpGet]
        //public IEnumerable<TELoginModel> userprofile_old(string Emailid, string Password)
        //{
            

        //    var y = (from usr in db.UserProfiles
        //             join pswd in db.webpages_Membership
        //             on usr.UserId equals pswd.UserId
        //             where (usr.email == Emailid)
        //              &&
        //               (pswd.Password == Password)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby usr.UserName
        //             select new
        //             {
        //                 usr.UserId,
        //                 usr.CallName,
        //                 usr.Phone,
        //                 usr.email,
        //                 usr.photo
        //             });

        //    List<TELoginModel> list = new List<TELoginModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TELoginModel()
        //        {
        //            UserId = item.UserId,
        //            CallName = item.CallName,
        //            Phone = item.Phone,
        //            email = item.email,
        //            Photo = item.photo
        //        });
        //    }
        //    return list;
        //}

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

            db.SaveChanges();

            //CRUD Mobile
            //CRUD Email
            //CRUD Address 

            return db.TEUnit_Onboarding.Find(value.Uniqueid);
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
        //                 teboard.PMDAPPROVEDREASON
        //             });

        //    List<TEUnit_Onboardingmodel> list = new List<TEUnit_Onboardingmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEUnit_Onboardingmodel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            UserID = item.UserID,
        //            CallName = item.CallName,
        //            Email = item.email,
        //            Phone = item.Phone,
        //            Project = item.Project,
        //            ProjectName = item.ProjectName,
        //            Unit = item.Unit,
        //            COLOURCODE = item.COLOURCODE,
        //            PMDAPPROVEDBY = item.PMDAPPROVEDBY,
        //            PMDAPPROVEDREASON = item.PMDAPPROVEDREASON,
        //            Status = item.Status
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
        //             //where (teboard.Status == "Inactive")
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
        //                 teboard.PMDAPPROVEDREASON
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
        //            Status = item.Status
        //        });
        //    }
        //    return list;
        //}
#endregion 
        [HttpGet]
        public object GetTEAssignedinfo(int QueueID, int CategoryID)
        {
            db.Configuration.ProxyCreationEnabled = false;

            //int? teDeptCompanyId=(from teqDept in db.TEQueueDepartments
            //                                 where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
            //                                 select teqDept.CompanyID
            //                                    ).ToList().First();
            int? teLOBId = (from teqDept in db.TEQueueDepartments
                                    where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                                    select teqDept.TELineOfBussiness
                                                ).ToList().First();
            int?  teDeptDepartmentId=(from teqDept in db.TEQueueDepartments where (teqDept.QueueID.Value == QueueID)&&(teqDept.CategoryID==CategoryID)
                                          select teqDept.DepartmentID).ToList().First();
            //int? teDeptSubfunctionId = (from teqDept in db.TEQueueDepartments
            //                            where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID == CategoryID)
            //                            select teqDept.SubfunctionID).ToList().First();
            if (teLOBId == null) 
            {
                teLOBId = 0;
            }
            if (teDeptDepartmentId == null)
            {
                teDeptDepartmentId = 0;
            }
            //if (teDeptSubfunctionId == null)
            //{
            //    teDeptSubfunctionId = 0;
            //}
            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teass in db.TEEmpAssignmentDetails on tebasic.Uniqueid equals teass.TEEmpBasicInfo
                     //join tecomp in db.TECompanies on teass.TECompany equals tecomp.Uniqueid
                     join tedepart in db.TEDepartments on teass.TEDepartment equals tedepart.Uniqueid
                     //join tesub in db.TESubFunctions on teass.TESubFunction equals tesub.Uniqueid
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where (teass.Status == "Active")
                     && (tedepart.TELineOfBusiness == teLOBId.Value)
                     && (tedepart.Uniqueid == teDeptDepartmentId.Value)
                      //&& (tesub.Uniqueid == teDeptSubfunctionId.Value)
                     //&& (tedepart.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.DepartmentID
                     //                           ).ToList().First())
                     //&& (tesub.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.SubfunctionID
                     //                         ).ToList().First())
                     orderby teass.TEEmpBasicInfo
                     select new
                     {
                         teuser.UserId,
                         teuser.CallName,
                         tebasic.Uniqueid,
                         tebasic.FirstName,
                         tebasic.LastName
                     }).ToList();

            List<TEEmployeeAssignee> list = new List<TEEmployeeAssignee>();
            foreach (var item in y)
            {
                list.Add(new TEEmployeeAssignee()
                {
                    Userid = item.UserId,
                    callname = item.CallName,
                    teempbasicinfo = item.Uniqueid,
                    UserName=item.FirstName+' '+item.LastName
                });
            }
            return list;
        }

        [HttpGet]
        public object GetTEAssignedinfobyqueue(int QueueID)
        {
            db.Configuration.ProxyCreationEnabled = false;

            //int? teDeptCompanyId = (from teqDept in db.TEQueueDepartments
            //                        where (teqDept.QueueID.Value == QueueID)
            //                        select teqDept.CompanyID
            //                                    ).ToList().First();
            int? teLOBId = (from teqDept in db.TEQueueDepartments
                            where (teqDept.QueueID.Value == QueueID)
                            select teqDept.TELineOfBussiness
                                                ).ToList().First();

            int? teDeptDepartmentId = (from teqDept in db.TEQueueDepartments
                                       where (teqDept.QueueID.Value == QueueID) 
                                       select teqDept.DepartmentID).ToList().First();
            int? teDeptSubfunctionId = (from teqDept in db.TEQueueDepartments
                                        where (teqDept.QueueID.Value == QueueID) 
                                        select teqDept.SubfunctionID).ToList().First();
            if (teLOBId == null)
            {
                teLOBId = 0;
            }
            if (teDeptDepartmentId == null)
            {
                teDeptDepartmentId = 0;
            }
            if (teDeptSubfunctionId == null)
            {
                teDeptSubfunctionId = 0;
            }
            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teass in db.TEEmpAssignmentDetails on tebasic.Uniqueid equals teass.TEEmpBasicInfo
                     //join tecomp in db.TECompanies on teass.TECompany equals tecomp.Uniqueid
                     join tedepart in db.TEDepartments on teass.TEDepartment equals tedepart.Uniqueid
                     //join tesub in db.TESubFunctions on teass.TESubFunction equals tesub.Uniqueid
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where (teass.Status == "Active") && ((tebasic.Status == "Approved" || tebasic.Status == "Resigned") && tebasic.IsDeleted==false)
                     && (tedepart.TELineOfBusiness == teLOBId.Value)
                     && (tedepart.Uniqueid == teDeptDepartmentId.Value)
                      //&& (tesub.Uniqueid == teDeptSubfunctionId.Value)
                     //&& (tedepart.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.DepartmentID
                     //                           ).ToList().First())
                     //&& (tesub.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.SubfunctionID
                     //                         ).ToList().First())
                     orderby teass.TEEmpBasicInfo
                     select new
                     {
                         teuser.UserId,
                         teuser.CallName,
                         tebasic.Uniqueid,
                         tebasic.FirstName,
                         tebasic.LastName,
                         tebasic.EmployeeId
                     }).ToList();

            List<TEEmployeeAssignee> list = new List<TEEmployeeAssignee>();
            foreach (var item in y)
            {
                list.Add(new TEEmployeeAssignee()
                {
                    Userid = item.UserId,
                    callname = item.CallName,
                    teempbasicinfo = item.Uniqueid,
                    UserName = item.FirstName + ' ' + item.LastName,
                    EmpNameCode = item.FirstName + ' ' + item.LastName + ' ' + item.EmployeeId
                });
            }
            return list;
        }

        [HttpGet]
        public object GetTEAssignedinfobyIssue(int IssueID,int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            int ? AssignedTo=0;
            if (ID == 1)
            {
                AssignedTo = (from teIssue in db.TEIssues where (teIssue.Uniqueid == IssueID) select teIssue.AssignedTo).ToList().First();
                if (AssignedTo == null)
                {
                    AssignedTo = 0;
                }
            }
            else if (ID == 2)
            {

                AssignedTo = (from teIssue in db.TEIssues where (teIssue.Uniqueid == IssueID) select teIssue.RaisedBy).ToList().First();
                if (AssignedTo == null)
                {
                    AssignedTo = 0;
                }
            }

            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teass in db.TEEmpAssignmentDetails on tebasic.Uniqueid equals teass.TEEmpBasicInfo
                     join com in db.TECompanies on teass.TECompany equals com.Uniqueid
                     join dept in db.TEDepartments on teass.TEDepartment equals dept.Uniqueid
                     join sf in db.TESubFunctions on teass.TESubFunction equals sf.Uniqueid
                     join lob in db.TELineOfBusinesses on dept.TELineOfBusiness equals lob.Uniqueid
                     join wl in db.TEWorkLocations on teass.TEWorkLocation equals wl.Uniqueid
                     join bg in db.TEBandsAndGrades on teass.TEBandAndGrade equals bg.Uniqueid
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where (teuser.UserId == AssignedTo)
                     select new {
                         tebasic.FirstName,
                         tebasic.OfficialEmail,
                         tebasic.Mobile,
                        //sf.Name,
                         com.Name,
                       bg.Designation,
                         wl.WorkLocationName
                       }).Distinct().FirstOrDefault();
            //UserProfile user=db.UserProfiles.Where(x=>x.UserId==AssignedTo).FirstOrDefault();
            //TEEmpBasicInfo basic=db.TEEmpBasicInfoes.Where(x=>x.IsDeleted==false && x.UserId==user.UserName).FirstOrDefault();
            //TEEmpAssignmentDetail ass=db.TEEmpAssignmentDetails.Where(x=>x.IsDeleted==false && x.TEEmpBasicInfo==basic.Uniqueid).FirstOrDefault();
            //TECompany comp =db.TECompanies.Where(x=>x.IsDeleted==false && x.Uniqueid==ass.TECompany).FirstOrDefault();
            //TEDepartment deptm =db.TEDepartments.Where(x=>x.IsDeleted==false && x.Uniqueid==ass.TEDepartment).FirstOrDefault();
            //TEWorkLocation work=db.TEWorkLocations.Where(x=>x.IsDeleted==false && x.Uniqueid==ass.TEWorkLocation).FirstOrDefault();
            //TEBandsAndGrade bng=db.TEBandsAndGrades.Where(x=>x.IsDeleted==false && x.Uniqueid==ass.TEBandAndGrade).FirstOrDefault();
      
            return y;
        }

        [HttpGet]
        public object GetAllTEAssignedinfo()
        {
            db.Configuration.ProxyCreationEnabled = false;

            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where ((tebasic.Status == "Approved" || tebasic.Status == "Resigned") && tebasic.IsDeleted==false)
                     orderby tebasic.Uniqueid
                     select new
                     {
                         teuser.UserId,
                         teuser.CallName,
                         tebasic.Uniqueid,
                         tebasic.FirstName,
                         tebasic.LastName,//,
                         tebasic.EmployeeId
                     }).ToList();

            List<TEEmployeeAssignee> list = new List<TEEmployeeAssignee>();
            foreach (var item in y)
            {
                list.Add(new TEEmployeeAssignee()
                {
                    Userid = item.UserId,
                    callname = item.CallName,
                    teempbasicinfo = item.Uniqueid,
                    UserName = item.FirstName + ' ' + item.LastName, //+' '+item.EmployeeId
                    EmpNameCode = item.FirstName + ' ' + item.LastName + ' ' + item.EmployeeId
                });
            }
            return list;
        }

        [HttpGet]
        public object GetTEAssignedinfobyLOB(int LOB, int department, int subfunction)
        {
            db.Configuration.ProxyCreationEnabled = false;

            //int? teDeptCompanyId = (from teqDept in db.TEQueueDepartments
            //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
            //                        select teqDept.CompanyID
            //                                    ).ToList().First();
            //int? teDeptDepartmentId = (from teqDept in db.TEQueueDepartments
            //                           where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID == CategoryID)
            //                           select teqDept.DepartmentID).ToList().First();
            //int? teDeptSubfunctionId = (from teqDept in db.TEQueueDepartments
            //                            where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID == CategoryID)
            //                            select teqDept.SubfunctionID).ToList().First();
            //if (teDeptCompanyId == null)
            //{
            //    teDeptCompanyId = 0;
            //}
            //if (teDeptDepartmentId == null)
            //{
            //    teDeptDepartmentId = 0;
            //}
            //if (teDeptSubfunctionId == null)
            //{
            //    teDeptSubfunctionId = 0;
            //}
            var y = (from tebasic in db.TEEmpBasicInfoes
                     join teass in db.TEEmpAssignmentDetails on tebasic.Uniqueid equals teass.TEEmpBasicInfo
                     //join tecomp in db.TECompanies on teass.TECompany equals tecomp.Uniqueid
                     join tedepart in db.TEDepartments on teass.TEDepartment equals tedepart.Uniqueid
                     //join tesub in db.TESubFunctions on teass.TESubFunction equals tesub.Uniqueid
                     join teuser in db.UserProfiles on tebasic.UserId equals teuser.UserName
                     where (teass.Status == "Active")
                     && (tedepart.TELineOfBusiness == LOB)
                     && (teass.TEDepartment == department)
                      && (teass.TESubFunction == subfunction)
                     //&& (tedepart.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.DepartmentID
                     //                           ).ToList().First())
                     //&& (tesub.Uniqueid == (from teqDept in db.TEQueueDepartments
                     //                        where (teqDept.QueueID.Value == QueueID) && (teqDept.CategoryID.Value == CategoryID)
                     //                        select teqDept.SubfunctionID
                     //                         ).ToList().First())
                     orderby teass.TEEmpBasicInfo
                     select new
                     {
                         teuser.UserId,
                         teuser.CallName,
                         tebasic.Uniqueid,
                         tebasic.FirstName,
                         tebasic.LastName
                     }).ToList();

            List<TEEmployeeAssignee> list = new List<TEEmployeeAssignee>();
            foreach (var item in y)
            {
                list.Add(new TEEmployeeAssignee()
                {
                    Userid = item.UserId,
                    callname = item.CallName,
                    teempbasicinfo = item.Uniqueid,
                    UserName = item.FirstName + ' ' + item.LastName
                });
            }
            return list;
        }
        #region Commented due to entity change
        //[HttpGet]
        //public IEnumerable<TEContactModel> GetTEContactByProjectid(int projectId)
        //{
        //    //var y = (from cnt in db.TEContacts
        //    //         join proj in db.TEProjects on cnt.Projectid equals proj.Uniqueid
        //    //         join tower in db.TEProjects_TOWER on cnt.Towerid equals tower.Uniqueid 
        //    //         where (cnt.Projectid == projectId)
        //    //          &&
        //    //           (cnt.Type == "Owner")
        //    //         orderby cnt.Projectid
        //    //         select new
        //    //         {
        //    //             cnt.Uniqueid,
        //    //             proj.ProjectName,
        //    //             proj.COLOURCODE,
        //    //             cnt.Projectid,
        //    //             tower.TOWERNAME,
        //    //             cnt.Unitid,
        //    //             cnt.Type,
        //    //             cnt.Salutation,
        //    //             cnt.FirstName,
        //    //             cnt.LastName,
        //    //             cnt.CallName,
        //    //             cnt.Mobile,
        //    //             cnt.Emailid,
        //    //             cnt.Photo,
        //    //         });
        //    var y = (from cnt in db.TEContacts
        //             join unit in db.TEUnit_Onboarding on cnt.UserId equals unit.UserID
        //             join proj in db.TEProjects on unit.Project equals proj.Uniqueid
        //             join punit in db.TEProjects_UNIT on unit.Unit equals punit.AREA
        //             join tower in db.TEProjects_TOWER on punit.TOWERID equals tower.Uniqueid
        //             where unit.Project == projectId
        //              && unit.Status == "Active"
        //              && cnt.Type == "Owner"
        //               && cnt.IsDeleted == false && unit.IsDeleted == false && proj.IsDeleted == false
        //              && punit.IsDeleted == false && tower.IsDeleted == false
        //             orderby cnt.CallName
        //             select new
        //             {
        //                 cnt.Uniqueid,
        //                 proj.ProjectName,
        //                 proj.COLOURCODE,
        //                 unit.Project,
        //                 tower.TOWERNAME,
        //                 unit.Unit,
        //                 cnt.Type,
        //                 cnt.Salutation,
        //                 cnt.FirstName,
        //                 cnt.LastName,
        //                 cnt.CallName,
        //                 cnt.Mobile,
        //                 cnt.Emailid,
        //                 cnt.Photo,
        //             }).Distinct();

        //    List<TEContactModel> list = new List<TEContactModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEContactModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            Projectid = item.Project,
        //            ProjectName = item.ProjectName,
        //            TOWERNAME=item.TOWERNAME,
        //            ColourCode = item.COLOURCODE,
        //            Unitid = item.Unit,
        //            Type = item.Type,
        //            Salutation = item.Salutation,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            CallName = item.CallName,
        //            Emailid = item.Emailid,
        //            Mobile = item.Mobile,
        //            Photo = item.Photo,
        //        });
        //    }
        //    return list;
        //    //return y.AsEnumerable()                .Cast<TEContact>().ToList();
        //}

        //[HttpGet]
        //public IEnumerable<TEProject> GetTEProjectByUser(int Userid)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var project = (from proj in db.TEProjects
        //                   join cont in db.TEContacts on proj.Uniqueid equals cont.Projectid
        //                   where cont.UserId == Userid
        //                   select proj).Distinct().ToList();
        //    return project;
        //}
       // [HttpGet]
       //  public IEnumerable<TEContactModel> GetTEContactusersByProject(int projectId, int Userid, int pagecount,string filter)
       //{
       //     db.Configuration.ProxyCreationEnabled = false;
       //     //return db.TEContacts.Where(x =>
       //     //                            (x.Projectid==projectId)
       //     //                            &&
       //     //                            (x.Unitid == unitId)
       //     List<TEContactModel> list = new List<TEContactModel>();
       //     if (filter == "null")
       //     {
       //         var y = (from cnt in db.TEContacts
       //                  join proj in db.TEProjects on cnt.Projectid equals proj.Uniqueid
       //                  join user in db.UserProfiles on cnt.Emailid equals user.email
       //                  where (cnt.Projectid == projectId) && (cnt.UserId != Userid)
       //                   && (cnt.Type == "Owner")
       //                   && (cnt.ISProfileSPublic == true)
       //                  orderby cnt.Projectid
       //                  select new
       //                  {
       //                      cnt.Uniqueid,
       //                      proj.ProjectName,
       //                      proj.COLOURCODE,
       //                      cnt.Projectid,
       //                      cnt.Unitid,
       //                      cnt.Type,
       //                      cnt.Salutation,
       //                      cnt.FirstName,
       //                      cnt.LastName,
       //                      cnt.CallName,
       //                      cnt.Mobile,
       //                      cnt.Emailid,
       //                      cnt.Photo,
       //                      user.UserId
       //                  }).ToList().Skip((pagecount - 1) * 20).Take(20);
               
       //         foreach (var item in y)
       //         {
       //             list.Add(new TEContactModel()
       //             {
       //                 Uniqueid = item.Uniqueid,
       //                 Projectid = item.Projectid,
       //                 ProjectName = item.ProjectName,
       //                 ColourCode = item.COLOURCODE,
       //                 Unitid = item.Unitid,
       //                 Type = item.Type,
       //                 Salutation = item.Salutation,
       //                 FirstName = item.FirstName,
       //                 LastName = item.LastName,
       //                 CallName = item.CallName,
       //                 Emailid = item.Emailid,
       //                 Mobile = item.Mobile,
       //                 Photo = item.Photo,
       //                 UserId = item.UserId
       //             });
       //         }
       //         return list;
       //     }
       //     else
       //     {
       //         var y = (from cnt in db.TEContacts
       //                  join proj in db.TEProjects on cnt.Projectid equals proj.Uniqueid
       //                  join user in db.UserProfiles on cnt.Emailid equals user.email
       //                  where (cnt.Projectid == projectId) && (cnt.UserId != Userid)
       //                   && (cnt.Type == "Owner")
       //                   && (cnt.ISProfileSPublic == true)
       //                    && (cnt.CallName.Contains("" + filter + "")
       //                   || cnt.Unitid.Contains("" + filter + "")
       //                      || cnt.Emailid.Contains("" + filter + "")
       //                      || cnt.Mobile.Contains("" + filter + ""))
       //                  // && (SqlMethods.Like(cnt.CallName, "%" + filter + "%")
       //                  // || SqlMethods.Like(cnt.Unitid, "%" + filter + "%")
       //                  // || SqlMethods.Like(cnt.Emailid, "%" + filter + "%"))
       //                  // || SqlMethods.Like(cnt.Mobile, "%" + filter + "%"))
       //                  orderby cnt.Projectid
       //                  select new
       //                  {
       //                      cnt.Uniqueid,
       //                      proj.ProjectName,
       //                      proj.COLOURCODE,
       //                      cnt.Projectid,
       //                      cnt.Unitid,
       //                      cnt.Type,
       //                      cnt.Salutation,
       //                      cnt.FirstName,
       //                      cnt.LastName,
       //                      cnt.CallName,
       //                      cnt.Mobile,
       //                      cnt.Emailid,
       //                      cnt.Photo,
       //                      user.UserId
       //                  }).ToList().Skip((pagecount - 1) * 20).Take(20);

               
       //     foreach (var item in y)
       //     {
       //         list.Add(new TEContactModel()
       //         {
       //             Uniqueid = item.Uniqueid,
       //             Projectid = item.Projectid,
       //             ProjectName = item.ProjectName,
       //             ColourCode = item.COLOURCODE,
       //             Unitid = item.Unitid,
       //             Type = item.Type,
       //             Salutation = item.Salutation,
       //             FirstName = item.FirstName,
       //             LastName = item.LastName,
       //             CallName = item.CallName,
       //             Emailid = item.Emailid,
       //             Mobile = item.Mobile,
       //             Photo = item.Photo,
       //             UserId = item.UserId
       //         });
       //     }
       //     return list;
       //     }
       //     return list;
       //     //return y.AsEnumerable()                .Cast<TEContact>().ToList();
       // }

        //[HttpGet]
        //public IEnumerable<TEContactModel> GetTEContacByProject(int projectId)
        //{
        //    //return db.TEContacts.Where(x =>
        //    //                            (x.Projectid==projectId)
        //    //                            &&
        //    //                            (x.Unitid == unitId)
        //    //                            );
        //    var y = (from cnt in db.TEContacts
        //             join proj in db.TEProjects on cnt.Projectid equals proj.Uniqueid
        //             join user in db.UserProfiles on cnt.Emailid equals user.email
        //             where (cnt.Projectid == projectId)
        //              &&
        //               (cnt.Type == "Owner")
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby cnt.Projectid
        //             select new
        //             {
        //                 cnt.Uniqueid,
        //                 proj.ProjectName,
        //                 proj.COLOURCODE,
        //                 cnt.Projectid,
        //                 cnt.Unitid,
        //                 cnt.Type,
        //                 cnt.Salutation,
        //                 cnt.FirstName,
        //                 cnt.LastName,
        //                 cnt.CallName,
        //                 cnt.Mobile,
        //                 cnt.Emailid,
        //                 cnt.Photo,
        //                 user.UserId
        //             });

        //    List<TEContactModel> list = new List<TEContactModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEContactModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            Projectid = item.Projectid,
        //            ProjectName = item.ProjectName,
        //            ColourCode = item.COLOURCODE,
        //            Unitid = item.Unitid,
        //            Type = item.Type,
        //            Salutation = item.Salutation,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            CallName = item.CallName,
        //            Emailid = item.Emailid,
        //            Mobile = item.Mobile,
        //            Photo = item.Photo,
        //            UserId = item.UserId
        //        });
        //    }
        //    return list;
        //    //return y.AsEnumerable()                .Cast<TEContact>().ToList();
        //}
        //[HttpGet]
        //public IEnumerable<TEContactModel> GetTEContacByProjectNUnit(int projectId, string unitId)
        //{
        //    //return db.TEContacts.Where(x =>
        //    //                            (x.Projectid==projectId)
        //    //                            &&
        //    //                            (x.Unitid == unitId)
        //    //                            );
        //    //var y = (from cnt in db.TEContacts
        //    //         join proj in db.TEProjects
        //    //         on cnt.Projectid equals proj.Uniqueid
        //    //         where (cnt.Projectid == projectId)
        //    //          &&
        //    //           (cnt.Unitid == unitId)
        //    //         // && (uob.Project == cnt.proj)
        //    //         //&&(uob.Uniqueid==cnt.Unitid)
        //    //         orderby cnt.Projectid
        //    //         select new
        //    //         {
        //    //             cnt.Uniqueid,
        //    //             proj.ProjectName,
        //    //             proj.COLOURCODE,
        //    //             cnt.Projectid,
        //    //             cnt.Unitid,
        //    //             cnt.Type,
        //    //             cnt.Salutation,
        //    //             cnt.FirstName,
        //    //             cnt.LastName,
        //    //             cnt.CallName,
        //    //             cnt.Mobile,
        //    //             cnt.Emailid,
        //    //             cnt.Photo
        //    //         });
        //    var y = (from cnt in db.TEContacts
        //             join unit in db.TEUnit_Onboarding on cnt.UserId equals unit.UserID
        //             join proj in db.TEProjects on unit.Project equals proj.Uniqueid
        //             join punit in db.TEProjects_UNIT on unit.Unit equals punit.AREA
        //             join tower in db.TEProjects_TOWER on punit.TOWERID equals tower.Uniqueid
        //             where unit.Project == projectId
        //              && unit.Status == "Active"
        //             && unit.Unit == unitId
        //              && cnt.Type == "Owner"
        //               && cnt.IsDeleted == false && unit.IsDeleted == false && proj.IsDeleted == false
        //              && punit.IsDeleted == false && tower.IsDeleted == false
        //             orderby cnt.CallName
        //             select new
        //             {
        //                 cnt.Uniqueid,
        //                 proj.ProjectName,
        //                 proj.COLOURCODE,
        //                 unit.Project,
        //                 tower.TOWERNAME,
        //                 unit.Unit,
        //                 cnt.Type,
        //                 cnt.Salutation,
        //                 cnt.FirstName,
        //                 cnt.LastName,
        //                 cnt.CallName,
        //                 cnt.Mobile,
        //                 cnt.Emailid,
        //                 cnt.Photo,
        //             }).Distinct();

        //    List<TEContactModel> list = new List<TEContactModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEContactModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            Projectid = item.Project,
        //            ProjectName = item.ProjectName,
        //            ColourCode = item.COLOURCODE,
        //            Unitid = item.Unit,
        //            Type = item.Type,
        //            Salutation = item.Salutation,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            CallName = item.CallName,
        //            Emailid = item.Emailid,
        //            Mobile = item.Mobile,
        //            Photo = item.Photo
        //        });
        //    }
        //    return list;
        //    //return y.AsEnumerable()                .Cast<TEContact>().ToList();
        //}

        //[HttpGet]
        //public object GetTEContacByQueueNUnit(int Queueid, string unitId)
        //{
        //    //return db.TEContacts.Where(x =>
        //    //                            (x.Projectid==projectId)
        //    //                            &&
        //    //                            (x.Unitid == unitId)
        //    //                            );
        //    TEQueue listQueue = db.TEQueues.Where(x => (x.Uniqueid == Queueid)).ToList().First();
        //    if (listQueue == null)
        //    {
        //        return "No Units Found";
        //    }
        //    var y = (from cnt in db.TEContacts
        //             join proj in db.TEProjects  on cnt.Projectid equals proj.Uniqueid
        //             join usr in db.UserProfiles on cnt.UserId equals usr.UserId
        //             where (cnt.Projectid == listQueue.PROJECTID)
        //              &&
        //               (cnt.Unitid == unitId)
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             orderby cnt.Projectid
        //             select new
        //             {
        //                 cnt.Uniqueid,
        //                 proj.ProjectName,
        //                 proj.COLOURCODE,
        //                 cnt.Projectid,
        //                 cnt.Unitid,
        //                 cnt.Type,
        //                 cnt.Salutation,
        //                 cnt.FirstName,
        //                 cnt.LastName,
        //                 cnt.CallName,
        //                 cnt.Mobile,
        //                 cnt.Emailid,
        //                 cnt.Photo,
        //                 cnt.UserId,
        //             });

        //    List<TEContactModel> list = new List<TEContactModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEContactModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            Projectid = item.Projectid,
        //            ProjectName = item.ProjectName,
        //            ColourCode = item.COLOURCODE,
        //            Unitid = item.Unitid,
        //            Type = item.Type,
        //            Salutation = item.Salutation,
        //            FirstName = item.FirstName,
        //            LastName = item.LastName,
        //            CallName = item.CallName,
        //            Emailid = item.Emailid,
        //            Mobile = item.Mobile,
        //            Photo = item.Photo,
        //            UserId=item.UserId
        //        });
        //    }
        //    return list;
        //    //return y.AsEnumerable()                .Cast<TEContact>().ToList();
        //}
        
        //[HttpGet]
        //public IEnumerable<TEProjectUnitmodel> TETowersByProjet(int projectid)
        //{
        //    var y = (from proj in db.TEProjects
        //             join tow in db.TEProjects_TOWER on proj.Uniqueid equals tow.PROJECT_ID into temptower
        //             from temptow in temptower.DefaultIfEmpty()
        //             where (temptow.PROJECT_ID == projectid)
        //             //(unit.TOWERID.ToString().Contains(towerid) ||
        //             orderby temptow.Uniqueid
        //             select new
        //             {
        //                 projectuniqueid = proj.Uniqueid,
        //                 proj.ProjectName,
        //                 proj.ProjectCode,
        //                 proj.COLOURCODE,
        //                 toweruniqueid = temptow.Uniqueid,
        //                 temptow.TOWERNAME,
        //                 temptow.TOWERCODE
        //             });

        //    List<TEProjectUnitmodel> list = new List<TEProjectUnitmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEProjectUnitmodel()
        //        {
        //            projectuniqueid = item.projectuniqueid,
        //            ProjectName = item.ProjectName,
        //            ProjectCode = item.ProjectCode,
        //            COLOURCODE = item.COLOURCODE,
        //            toweruniqueid = item.toweruniqueid.ToString(),
        //            towername = item.TOWERNAME,
        //            towercode = item.TOWERCODE
        //        });
        //    }
        //    return list;
        //}
        
        //[HttpGet]
        //public IEnumerable<TEProjectUnitmodel>  TEUnitByProjetNTower(int projectid, int towerid)
        //{
        //    var y = (from unit in db.TEProjects_UNIT
        //             join proj in db.TEProjects on unit.PROJECT_ID equals proj.Uniqueid into tempproj
        //             from temp in tempproj.DefaultIfEmpty()
        //            join tow in db.TEProjects_TOWER on unit.TOWERID equals tow.Uniqueid into temptower
        //             from temptow in temptower.DefaultIfEmpty()
        //                where (unit.PROJECT_ID==projectid)
        //                &&  ((unit.TOWERID != null) && (unit.TOWERID==towerid))
        //               //(unit.TOWERID.ToString().Contains(towerid) ||
        //             orderby unit.Uniqueid
        //             select new
        //             {
        //                 projectuniqueid = temp.Uniqueid,
        //                 temp.ProjectName,
        //                 temp.ProjectCode,
        //                 temp.COLOURCODE,
        //                 toweruniqueid = unit.TOWERID,
        //                 temptow.TOWERNAME,
        //                 temptow.TOWERCODE,
        //        unit.Uniqueid,unit.AREA 
        //             });

        //    List<TEProjectUnitmodel> list = new List<TEProjectUnitmodel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEProjectUnitmodel()
        //        {
        //            projectuniqueid = item.projectuniqueid,
        //            ProjectName = item.ProjectName,
        //            ProjectCode = item.ProjectCode,
        //            COLOURCODE = item.COLOURCODE,
        //            toweruniqueid = item.toweruniqueid.ToString(),
        //            towername = item.TOWERNAME,
        //            towercode = item.TOWERCODE,
        //            Uniqueid = item.Uniqueid,
        //            Unit = item.AREA
        //        });
        //    }
        //    return list;
        //}

        //[HttpGet]
        //public IEnumerable<TEResidentprojectModel> GetTEResidentsByProject()
        //{
        //    return db.TEContacts.Where(x =>
        //                                (x.Projectid == projectId)
        //                                &&
        //                                (x.Unitid == unitId)
        //                                );
        //    var y = (from cnt in db.TEContacts
        //             join proj in db.TEProjects on cnt.Projectid equals proj.Uniqueid
        //             join tower in db.TEProjects_TOWER on proj.Uniqueid equals tower.PROJECT_ID
        //             join unit in db.TEProjects_UNIT on proj.Uniqueid equals unit.PROJECT_ID
        //             where //(cnt.Projectid == projectId)                       &&
        //               (cnt.Type == "Owner")
        //             // && (uob.Project == cnt.proj)
        //             //&&(uob.Uniqueid==cnt.Unitid)
        //             group new { cnt, proj, tower, unit } by new { proj.Uniqueid, proj.ProjectName, proj.ProjectCode } into g
        //             orderby g.Key.Uniqueid descending
        //             select new
        //             {
        //                 g.Key.Uniqueid,
        //                 g.Key.ProjectName,
        //                 g.Key.ProjectCode,
        //                 CountTowers = g.Select(x => x.tower.Uniqueid).Distinct().Count(),
        //                 Countunit = g.Select(x => x.unit.Uniqueid).Distinct().Count(),
        //                 Countusers = g.Select(x => x.cnt.Uniqueid).Distinct().Count(),

        //             }).Distinct().ToList();
        //    var y = (from proj in db.TEProjects
        //             join tower in db.TEProjects_TOWER on proj.Uniqueid equals tower.PROJECT_ID
        //             join unit in db.TEProjects_UNIT on proj.Uniqueid equals unit.PROJECT_ID
        //             where proj.IsDeleted == false && unit.IsDeleted == false && tower.IsDeleted == false
        //             group new { proj, tower, unit } by new { proj.Uniqueid, proj.ProjectName, proj.ProjectCode } into g
        //             orderby g.Key.Uniqueid descending
        //             select new
        //             {
        //                 g.Key.Uniqueid,
        //                 g.Key.ProjectName,
        //                 g.Key.ProjectCode,
        //                 CountTowers = g.Select(x => x.tower.Uniqueid).Distinct().Count(),
        //                 Countunit = g.Select(x => x.unit.Uniqueid).Distinct().Count(),
        //                 Countusers = (from unit in db.TEUnit_Onboarding where (unit.Project == g.Key.Uniqueid && unit.IsDeleted == false && unit.Status == "Active") select unit).Count()
        //             }).Distinct().ToList();

        //    List<TEResidentprojectModel> list = new List<TEResidentprojectModel>();
        //    foreach (var item in y)
        //    {
        //        list.Add(new TEResidentprojectModel()
        //        {
        //            Uniqueid = item.Uniqueid,
        //            ProjectName = item.ProjectName,
        //            ProjectCode = item.ProjectCode,
        //            CountTowers = item.CountTowers,
        //            Countunit = item.Countunit,
        //            Countusers = item.Countusers
        //        });
        //    }
        #endregion
        //    return list;
        //    return y.AsEnumerable().Cast<TEContact>().ToList();
        //}

         [HttpGet]
         public object GetYelloAppVersion(string YelloAppVersion)
         {
             try
             {
                 return db.webpages_Roles.Where(x => (x.RoleName == YelloAppVersion)).Distinct().First();
             }
             catch (Exception ex)
             {
                 ex.Message.ToString();
                 return "Version do not exist!";
             }
             return "Version do not exist!";
         }
    }
}