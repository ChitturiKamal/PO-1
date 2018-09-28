using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models;
using TECommonLogicLayer.TEModelling;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEQueueController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEQueueModel> GetQueueList()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            List<TEQueue> listOfQueue = db.TEQueues.Where(x => (x.IsDeleted == false)).ToList();
                                                        //&& (x.PROJECTID != null)).ToList();

            List<TEQueueModel> result = new List<TEQueueModel>();
            foreach (var item in listOfQueue)
            {
                if (item.Admin!=null)
              {
                TETransformEntityNModel translator = new TETransformEntityNModel();     
              TEQueueModel qModel  = translator.TransformAtoB(item, new TEQueueModel());
              qModel.UserProfile = db.UserProfiles.Where(x => x.UserId == item.Admin).FirstOrDefault();
              result.Add(qModel);
              }
                    //UserProfile AdminName = db.UserProfiles.Where(x => x.- == item.admin).FirstOrDefault();
                //UserProfile   AdminName = db.u
                  
                        //qModel.

                #region Commented due to entity change
                //if (item.PROJECTID != null)
                //{
                //    TEProject proj = db.TEProjects.Find(item.PROJECTID.Value);
                //    if (proj != null)
                //        qModel.TEProject = new TEProject()
                //        {
                //            Uniqueid = proj.Uniqueid,
                //            ProjectCode = proj.ProjectCode,
                //            ProjectName = proj.ProjectName,
                //            ProjectShortName = proj.ProjectShortName
                //        };
                //}

                //if (qModel.TEProject == null)
                //{
                //    qModel.TEProject = new TEProject();
                //}
                #endregion
                   // result.Add(AdminName);
            }
            return result;

        }

        [HttpGet]
        public object GetQueueBycategory(int category)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var queue = (from tecat in db.TECategories
                         join teque in db.TEQueues on tecat.Name equals teque.QueueName
                         where tecat.Uniqueid == category && tecat.IsDeleted==false
                         select new { teque.Uniqueid }).Distinct().ToList();
            return queue;
        }
        [HttpGet]
        public List<TEQueueModel> GetQueueListinternal()
        {
            List<TEQueue> listOfQueue = db.TEQueues.Where(x => (x.IsDeleted == false && x.PROJECTID==null)).ToList();
                                                    //&&(x.PROJECTID == null)).ToList();

            List<TEQueueModel> result = new List<TEQueueModel>();
            foreach (var item in listOfQueue)
            {
                TEQueueModel qModel = new TEQueueModel();
                TETransformEntityNModel translator = new TETransformEntityNModel();

                qModel = translator.TransformAtoB(item, qModel);
                qModel.UserProfile = null;
                #region Commented due to entity change
                //if (item.PROJECTID != null)
                //{
                //    TEProject proj = db.TEProjects.Find(item.PROJECTID.Value);
                //    if (proj != null)
                //        qModel.TEProject = new TEProject()
                //        {
                //            Uniqueid = proj.Uniqueid,
                //            ProjectCode = proj.ProjectCode,
                //            ProjectName = proj.ProjectName,
                //            ProjectShortName = proj.ProjectShortName
                //        };
                //}

                //if (qModel.TEProject == null)
                //{
                //    qModel.TEProject = new TEProject();
                //}
                #endregion
                result.Add(qModel);
            }
            return result;

        }
        [HttpGet]
        public IEnumerable<TEQueueModel> GetQueueList(int Uniqueid)
        {
            //List<TEQueue> listOfQueue = db.TEQueues.Where(x => x.IsDeleted == false).ToList();
            int raisedby = (from iss in db.TEIssues
                            where (iss.Uniqueid == Uniqueid)
                            select  
                                         iss.RaisedBy).FirstOrDefault().Value;
            var listOfQueue = (from q in db.TEQueues
                                         join i in db.TEIssues on q.Uniqueid equals i.QueueID
                                         where //(i.RaisedBy == RaisedBy) && 
                                         (q.IsDeleted == false)
                                         && (i.RaisedBy==raisedby)
                                         orderby q.PROJECTID descending
                                         select new
                                         {
                                             q.Uniqueid,
                                             q.PROJECTID,
                                             q.QueueID,
                                             q.QueueName
                                         }).Distinct().ToList();

            List<TEQueueModel> result = new List<TEQueueModel>();
            foreach (var item in listOfQueue)
            {
                TEQueueModel qModel = new TEQueueModel();
                TETransformEntityNModel translator = new TETransformEntityNModel();

                qModel = translator.TransformAtoB(item, qModel);
                #region Commented due to entity change
                //if (item.PROJECTID != null)
                //{
                //    TEProject proj = db.TEProjects.Find(item.PROJECTID.Value);
                //    if (proj != null)
                //        qModel.TEProject = new TEProject()
                //        {
                //            Uniqueid = proj.Uniqueid,
                //            ProjectCode = proj.ProjectCode,
                //            ProjectName = proj.ProjectName,
                //            ProjectShortName = proj.ProjectShortName
                //        };
                //}

                //if (qModel.TEProject == null)
                //{
                //    qModel.TEProject = new TEProject();
                //}
                #endregion
                result.Add(qModel);
            }
            return result;

        }
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEQueue> GetQueueListEmployee()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return  db.TEQueues.Where(x => (x.IsDeleted == false)&&(x.PROJECTID==null)).ToList();

        }

        // GET api/<controller>/5
        [HttpGet]
        public TEQueue GetQueuebyId(int id)
        {
            TEQueue item=db.TEQueues.Find(id);
            TEQueueModel qModel = new TEQueueModel();
            TETransformEntityNModel translator = new TETransformEntityNModel();

            qModel = translator.TransformAtoB(item, qModel);
            #region Commented due to entity change
            //if (item.PROJECTID != null)
            //{
            //    TEProject proj = db.TEProjects.Find(item.PROJECTID.Value);
            //    qModel.TEProject = new TEProject()
            //    {
            //        Uniqueid = proj.Uniqueid,
            //        ProjectCode = proj.ProjectCode,
            //        ProjectName = proj.ProjectName,
            //        ProjectShortName = proj.ProjectShortName
            //    };
            //}
            #endregion
            return qModel;
        }

        [HttpGet]
        public TEQueue GetQueuebyProjectId(int PROJECTID)
        {
            return db.TEQueues.Where(
                x => (x.PROJECTID == PROJECTID)
                    &&
                    (x.IsDeleted == false)
                ).FirstOrDefault();
        }

        [HttpPost]
        public TEQueue AddTEQueue(TEQueue value)
        {
            TEQueue result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEQueues.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TEQueues.Attach(value);
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
            return db.TEQueues.Find(value.Uniqueid);
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

        [HttpGet]
        public IEnumerable<TEPickListItem> getpicklistItems(string picklist)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return  db.TEPickListItems.Where(x => (x.IsDeleted == false) &&
                                            (x.TEPickList == (from pri in db.TEPickLists
                                                                 where (pri.IsDeleted == false) && (pri.Name == "PMDPriority")
                                                                select pri.Uniqueid).FirstOrDefault())
                                            ).ToList();
             
        }
    }
}