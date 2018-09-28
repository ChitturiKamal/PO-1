using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TESubfunctionController : ApiController
    {
        //
        // GET: /TESubfunction/

        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TESubFunction> GetSubFunctionList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TESubFunctions.Where(x => x.IsDeleted == false);
        }

        [HttpGet]
        public IEnumerable<TESubFunction> GetSubFunctionbyDepartmentId(int DepartmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TESubFunctions.Where(
                x => (x.TEDepartment == DepartmentId)
                    &&
                    (x.IsDeleted == false)
                );
        }
        // GET api/<controller>/5
        [HttpGet]
        public TESubFunction GetSubFunctionbyId(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TESubFunctions.Where(
                x => (x.Uniqueid == id)
                    &&
                    (x.IsDeleted == false)
                ).FirstOrDefault();
        }

        [HttpPost]
        public TESubFunction AddTESubFunction(TESubFunction value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TESubFunction result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TESubFunctions.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TESubFunctions.Attach(value);
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
            return db.TESubFunctions.Find(value.Uniqueid);
        }


    }
}
