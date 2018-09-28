using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models; 

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsMDMControllers
{
    public class TEQueueDepartmentController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
          [HttpGet]
        public IEnumerable<TEQueueDepartmentModel> GetTEQueueDepartment()
        {
            //return db.TEQueueDepartments.Where(x=>x.IsDeleted==false);

            var y = (from tqd in db.TEQueueDepartments
                     join tq in db.TEQueues on  tqd.QueueID equals tq.Uniqueid 
                     join td in db.TEDepartments on  tqd.DepartmentID equals td.Uniqueid
                     join tc in db.TELineOfBusinesses on tqd.TELineOfBussiness equals tc.Uniqueid 
                     join tca in db.TECategories on tqd.CategoryID equals tca.Uniqueid
                     join tsf in db.TESubFunctions on tqd.SubfunctionID equals tsf.Uniqueid  
                    
                     // && (uob.Project == cnt.proj)
                     //&&(uob.Uniqueid==cnt.Unitid)
                     orderby tqd.Uniqueid
                     select new
                     {
                         tqd.Uniqueid,
                         tqd.QueueID,
                         tq.QueueName,
                         tqd.DepartmentID,
                         td.Name,
                         tqd.TELineOfBussiness,
                         tcName=tc.Name ,
                         tqd.CategoryID,
                         tcaName=tca.Name,
                         tqd.SubfunctionID,
                         tsfname=tsf.Name,
                         tqd.SLACritical,
                         tqd.SLAHigh,
                         tqd.SLALow,
                         tqd.SLAMedium,
                         //
                         tqd.AUTOASSIGNMENT,
                         tqd.AUTOCOMMUNICATION,
                         tqd.Default_assignee
                     });

            List<TEQueueDepartmentModel> list = new List<TEQueueDepartmentModel>();
            foreach (var item in y)
            {
                TEEmpBasicInfo defaultAssignee=null;
                if (item.Default_assignee>0)
	            {
                    db.Configuration.ProxyCreationEnabled = false;
                    defaultAssignee = db.TEEmpBasicInfoes.Find(item.Default_assignee);
	            }
               
                list.Add(new TEQueueDepartmentModel()
                {
                    Default_assignee=item.Default_assignee,
                    Uniqueid = item.Uniqueid,
                    QueueID = item.QueueID,
                    QueueName = item.QueueName,
                    DepartmentID = item.DepartmentID,
                    DepartmentName = item.Name,
                    TELineOfBussiness = item.TELineOfBussiness,
                    CompanyName = item.tcName,
                    CategoryID = item.CategoryID,
                    CategoryName = item.tcaName,
                    SubfunctionID = item.SubfunctionID,
                    SubfunctionName = item.tsfname,
                    SLACritical = item.SLACritical,
                    SLAHigh = item.SLAHigh,
                    SLALow = item.SLALow,
                    SLAMedium=item.SLAMedium,
                    TEEmpDefaultAssignee = defaultAssignee,
                    AUTOASSIGNMENT=item.AUTOASSIGNMENT,
                    AUTOCOMMUNICATION=item.AUTOCOMMUNICATION,

                });
            }
            return list;  
        }

          [HttpGet]
          public IEnumerable<TEQueueDepartmentModel> GetTEQueueDepartmentBYiD(int Uniqueid)
          {
              //return db.TEQueueDepartments.Where(x=>x.IsDeleted==false);

              var y = (from tqd in db.TEQueueDepartments
                       join tq in db.TEQueues on tqd.QueueID equals tq.Uniqueid
                       join td in db.TEDepartments on tqd.DepartmentID equals td.Uniqueid
                       join tc in db.TELineOfBusinesses on tqd.TELineOfBussiness equals tc.Uniqueid 
                       join tca in db.TECategories on tqd.CategoryID equals tca.Uniqueid
                       join tsf in db.TESubFunctions on tqd.SubfunctionID equals tsf.Uniqueid
                       where (tqd.Uniqueid==Uniqueid)
                       // && (uob.Project == cnt.proj)
                       //&&(uob.Uniqueid==cnt.Unitid)
                       orderby tqd.Uniqueid
                       select new
                       {
                           tqd.Uniqueid,
                           tqd.QueueID,
                           tq.QueueName,
                           tqd.DepartmentID,
                           td.Name,
                           tqd.TELineOfBussiness,
                           tcName = tc.Name,
                           tqd.CategoryID,
                           tcaName = tca.Name,
                           tqd.SubfunctionID,
                           tsfname = tsf.Name,
                           tqd.SLACritical,
                           tqd.SLAHigh,
                           tqd.SLALow,
                           tqd.SLAMedium,
                            tqd.AUTOASSIGNMENT,
                         tqd.AUTOCOMMUNICATION,
                         tqd.Default_assignee
                       });

              List<TEQueueDepartmentModel> list = new List<TEQueueDepartmentModel>();
              foreach (var item in y)
              {
                  TEEmpBasicInfo defaultAssignee = null;
                  if (item.Default_assignee > 0)
                  {
                      db.Configuration.ProxyCreationEnabled = false;
                      defaultAssignee = db.TEEmpBasicInfoes.Find(item.Default_assignee);
                  }
                  list.Add(new TEQueueDepartmentModel()
                  {
                      Uniqueid = item.Uniqueid,
                      QueueID = item.QueueID,
                      QueueName = item.QueueName,
                      DepartmentID = item.DepartmentID,
                      DepartmentName = item.Name,
                      TELineOfBussiness = item.TELineOfBussiness,
                      CompanyName = item.tcName,
                      CategoryID = item.CategoryID,
                      CategoryName = item.tcaName,
                      SubfunctionID = item.SubfunctionID,
                      SubfunctionName = item.tsfname,
                      SLACritical = item.SLACritical,
                      SLAHigh = item.SLAHigh,
                      SLALow = item.SLALow,
                      SLAMedium = item.SLAMedium,
                      TEEmpDefaultAssignee = defaultAssignee,
                      AUTOASSIGNMENT = item.AUTOASSIGNMENT,
                      AUTOCOMMUNICATION = item.AUTOCOMMUNICATION,
                  });
              }
              return list;
          }
        // GET api/<controller>/5
          [HttpGet]
        public TEQueueDepartment GetTEQueueDepartmentbycompanyID(int CompanyID)
        {
            return db.TEQueueDepartments.Where(
                x => (x.CompanyID == CompanyID)
                    &&
                    (x.IsDeleted==false)
                ).FirstOrDefault();
        }
          [HttpGet]
          public TEQueueDepartment GetTEQueueDepartmentbyDepartmentID(int DepartmentID)
          {
              return db.TEQueueDepartments.Where(
                  x => (x.DepartmentID == DepartmentID)
                      &&
                      (x.IsDeleted == false)
                  ).FirstOrDefault();
          }

          [HttpPost]
          public TEQueueDepartment AddTEQueueDepartment(TEQueueDepartment value)
          {
              TEQueueDepartment result = value;

              if (!(value.Uniqueid + "".Length > 0))
              {
                  //Create
                  result.CreatedOn = System.DateTime.Now;
                  result.LastModifiedOn = System.DateTime.Now;
                  result = db.TEQueueDepartments.Add(value);
              }
              else
              {
                  //Edit
                  db = new TEHRIS_DevEntities();
                  db.TEQueueDepartments.Attach(value);
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
              return db.TEQueueDepartments.Find(value.Uniqueid);
          }
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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