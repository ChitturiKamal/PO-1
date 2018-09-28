using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer; 

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEResidentController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEContact> Get()
        {
            return db.TEContacts.Where(x => x.IsDeleted == false);
        }

        // GET api/<controller>/5
        [HttpGet]
        public TEContact Get(int id)
        {
            return db.TEContacts.Find(id);
        }

        [HttpGet]
        public IEnumerable<TEContact> GetTEResidentByProjectNUnit(int projectId, string unitId)
        {
            return db.TEContacts.Where(x =>
                                        (x.Projectid==projectId)
                                        &&
                                        (x.Unitid == unitId)
                                        );
        }

        [HttpGet]
        public IEnumerable<TEImportantContact> GetTEImportantContacts(int projectId)
        {
            return db.TEImportantContacts.Where(x =>
                                        (x.TEProject == projectId)
                                        );
            //return db.TEImportantContacts.Where(x => x.IsDeleted == false);
        }

        // POST api/<controller>
        [HttpPost]
        public TEContact Post(TEContact value)
        {
            TEContact result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.UtcNow;
                result.LastModifiedOn = System.DateTime.UtcNow;
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

                value.LastModifiedOn = System.DateTime.UtcNow;
                db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
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
    }
}