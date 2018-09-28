using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TECommonLogicLayer.TEModelling;
using TEComplaintsManagementAPI.Models;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEProjectController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEProject> GetTEProject()
        {
            db.Configuration.ProxyCreationEnabled = false;
            //var y = (from x in db.TEProjects
            //         where (x.IsDeleted == false)
            //         select new
            //           {
            //               x.ProjectName,
            //               x.Uniqueid,
            //               x.ProjectCode
            //           }
            //         );
            //db.Configuration.ProxyCreationEnabled = false;
            IEnumerable<TEProject> project=db.TEProjects.Where(x => x.IsDeleted == false).ToList();
            return project;
        }
        #region Commented due to entity change
        // GET api/<controller>/5
        //[HttpGet]
        //public object TEProjectbyId(int id)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    return db.TEProjects.Where(
        //        x => (x.Uniqueid == id)
        //            &&
        //            (x.IsDeleted == false)
        //        ).FirstOrDefault();
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public object PostProject(TEProject value)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    TEProject result = value;

        //    if (!(value.Uniqueid + "".Length > 0))
        //    {
        //        result.CreatedOn = System.DateTime.Now;
        //        result.LastModifiedOn = System.DateTime.Now;
        //        result = db.TEProjects.Add(value);
        //    } 
        //    else
        //    {
        //        db.TEProjects.Attach(value);

        //        foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
        //        {
        //            string propname = item.Name;
        //            if (propname.ToLower() == "createdon")
        //                continue;
        //            object propValue = item.GetValue(value);
        //            if (propValue != null || Convert.ToString(propValue).Length != 0)
        //                db.Entry(value).Property(propname).IsModified = true;
        //        }

        //        value.LastModifiedOn = System.DateTime.Now;
        //        db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
        //    }

        //    db.SaveChanges();

        //    return db.TEProjects.Find(value.Uniqueid);
        //}
        #endregion
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