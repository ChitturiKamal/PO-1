using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEIssuesDispositionMgrController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/teissuesdispositionmgr
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/teissuesdispositionmgr/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/teissuesdispositionmgr
        public void Post([FromBody]string value)
        {
        }

        // PUT api/teissuesdispositionmgr/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/teissuesdispositionmgr/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        public List<TEIssuesDisposition> GetDisposition()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TEIssuesDisposition> Disposition = new List<TEIssuesDisposition>();
            Disposition = db.TEIssuesDispositions.Where(x => x.IsDeleted == false && x.Status == "Active").ToList();
            return Disposition;
        }

        [HttpGet]
        public TEIssuesDisposition GetDispositionById(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEIssuesDisposition Disposition = new TEIssuesDisposition();
            Disposition = db.TEIssuesDispositions.Find(Id);
            return Disposition;
        }

        [HttpPost]
        public TEIssuesDisposition PostDisposition(TEIssuesDisposition request)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEIssuesDisposition Disposition = new TEIssuesDisposition();
            Disposition = request;
            if (request.Uniqueid + "".Length == 0)
            {
                //Create
                request.CreatedOn = System.DateTime.Now;
                request.LastModifiedOn = System.DateTime.Now;
                request.ResolvedDate = System.DateTime.Now;
                Disposition = db.TEIssuesDispositions.Add(request);
            }
            else
            {
                db.TEIssuesDispositions.Attach(request);
                foreach (System.Reflection.PropertyInfo item in Disposition.GetType().GetProperties())
                {
                    string propname = item.Name;
                    if (propname.ToLower() == "createdon")
                        continue;
                    object propValue = item.GetValue(request);
                    if (propValue != null || Convert.ToString(propValue).Length != 0)
                        db.Entry(request).Property(propname).IsModified = true;
                }

                request.LastModifiedOn = System.DateTime.Now;
                db.Entry(request).Property(x => x.LastModifiedOn).IsModified = true;


            }
            db.SaveChanges();
            return db.TEIssuesDispositions.Find(request.Uniqueid);
        }
    }
}
