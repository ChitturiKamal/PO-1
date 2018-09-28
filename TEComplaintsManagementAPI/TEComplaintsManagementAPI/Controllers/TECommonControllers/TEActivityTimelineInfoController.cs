using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer; 

namespace TEComplaintsManagementAPI.Controllers.TECommonControllers
{
    public class TEActivityTimelineInfoController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEActivityTimelineInfo> Get()
        {
            return db.TEActivityTimelineInfoes;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TEActivityTimelineInfo> GetByIssueId(int id)
        {
            return db.TEActivityTimelineInfoes.Where(x=>x.ISSUEID==id);
        }

        // POST api/<controller>
        [HttpPost]
        public TEActivityTimelineInfo Post(TEActivityTimelineInfo value)
        {
            TEActivityTimelineInfo result = value;

            using (var scope = new System.Transactions.TransactionScope())
            {
                //Create
                if (!(value.Uniqueid + "".Length > 0))
                {
                    result.CreatedOn = System.DateTime.Now;
                    result.LastModifiedOn = System.DateTime.Now;
                    result = db.TEActivityTimelineInfoes.Add(value);
                }
                //Edit | Delete
                else
                {
                    db = new TEHRIS_DevEntities();
                    db.TEActivityTimelineInfoes.Attach(value);

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
                //Save and Complete
                db.SaveChanges();
                scope.Complete();
            }
            return db.TEActivityTimelineInfoes.Find(result.Uniqueid);
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