using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEDepartmentController : ApiController
    {
        //
        // GET: /TEDepartment/

       TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEDepartment> GetDepartmentList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEDepartments.Where(x => x.IsDeleted == false);
        }

        [HttpGet]
        public IEnumerable<TEDepartment> GetDepartmentbyCompanyid(int Companyid)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEDepartments.Where(
                x => (x.TECompany == Companyid)
                    &&
                    (x.IsDeleted == false)
                );
        }

        [HttpGet]
        public IEnumerable<TEDepartment> GetDepartmentbyLOBid(int LOBid)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEDepartments.Where(
                x => (x.TELineOfBusiness == LOBid)
                    &&
                    (x.IsDeleted == false)
                );
        }

        // GET api/<controller>/5
        [HttpGet]
        public TEDepartment GetDepartmentbyId(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEDepartments.Where(
                x => (x.Uniqueid == id)
                    &&
                    (x.IsDeleted == false)
                ).FirstOrDefault();
        }

        [HttpPost]
        public TEDepartment AddTEDepartment(TEDepartment value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEDepartment result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEDepartments.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TEDepartments.Attach(value);
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
            return db.TEDepartments.Find(value.Uniqueid);
        }

    }
}
