using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TECompanyController : ApiController
    {
        //
        // GET: /TECompany/
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TECompany> GetCompanyList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            //return db.TECompanies.Where(x => x.IsDeleted == false);
            var y=(from  x in db.TECompanies.Where(x => x.IsDeleted == false)
                   orderby x.Uniqueid
                   select new
                   {
                       x.Uniqueid,
                       x.Name,
                       x.CompanyCode
                   });
            List<TECompany> list = new List<TECompany>();
            foreach (var item in y)
            {
                list.Add(new TECompany()
                {
                    Uniqueid = item.Uniqueid,
                    Name = item.Name,
                    CompanyCode = item.CompanyCode,
                   
                });
            }
            return list;
        }

        // GET api/<controller>/5
        [HttpGet]
        public TECompany GetCompanybyId(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.TECompanies.Where(
                x => (x.Uniqueid == id)
                    &&
                    (x.IsDeleted == false)
                ).FirstOrDefault();
        }

        [HttpPost]
        public TECompany AddTECompany(TECompany value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TECompany result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TECompanies.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TECompanies.Attach(value);
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
            return db.TECompanies.Find(value.Uniqueid);
        }

    }
}
