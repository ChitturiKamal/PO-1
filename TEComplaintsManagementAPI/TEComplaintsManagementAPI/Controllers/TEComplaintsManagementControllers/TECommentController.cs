using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer; 

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TECommentController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEComment> Get()
        {
            return db.TEComments.Where(x=>x.IsDeleted==false)
                 .OrderByDescending(x => x.Uniqueid);
        }

        // GET api/<controller>/5
        [HttpGet]
        public TEComment Get(int id)
        {
            return db.TEComments.Find(id);
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TEComment> GetCommentByIssueId(int id)
        {
            return db.TEComments.Where(x => x.IssueID == id);
        }
        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TEComment> GetCommentBycustomerIssueId(int id)
        {
            return db.TEComments.Where(x => (x.IssueID == id)
                                            && (x.IsPrivate==false)
                                            )
                 .OrderByDescending(x => x.Uniqueid); ;
        }
        // POST api/<controller>
        [HttpPost]
        public TEComment Post(TEComment value)
        {
            TEComment result = value;

            if (!(value.Uniqueid + "".Length > 0))
            { 
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEComments.Add(value);
            } 
            else
            {
                db = new TEHRIS_DevEntities();
                db.TEComments.Attach(value);


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

            return db.TEComments.Find(value.Uniqueid);
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