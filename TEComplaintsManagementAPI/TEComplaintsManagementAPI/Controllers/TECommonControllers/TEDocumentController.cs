using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer; 

namespace TEComplaintsManagementAPI.Controllers.TECommonControllers
{
    public class TEDocumentController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEDocument> Get()
        {
            return db.TEDocuments.Where(x => x.IsDeleted == false);
        }

        // GET api/<controller>/5
        [HttpGet]
        public TEDocument Get(int id)
        {
            return db.TEDocuments.Find(id);
        }

        // POST api/<controller>
        [HttpPost]
        public TEDocument Post(TEDocument value)
        {
            TEDocument result = value;

            using (var scope = new System.Transactions.TransactionScope())
            {
                //Create
                if (!(value.Uniqueid + "".Length > 0))
                {
                    result.UploadedOn = System.DateTime.Now;
                    result.LastModifiedOn = System.DateTime.Now;
                    result = db.TEDocuments.Add(value);
                }
                //Edit | Delete
                else
                {
                    db = new TEHRIS_DevEntities();
                    db.TEDocuments.Attach(value);

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
            return db.TEDocuments.Find(result.Uniqueid);
        }

        [HttpPost]
        public void UploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    // Validate the uploaded image(optional)

                    // Get the complete file path
                    var fileSavePath = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);
                }
            }
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