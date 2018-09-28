using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TECommonLogicLayer.TEModelling;
using TEComplaintsManagementAPI.Models;

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsMDMControllers
{
    public class TEEscalationMatrixController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        public IEnumerable<TEEscalationMatrixModel> Get()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TEESCALATIONMATRIX> list = db.TEESCALATIONMATRIces.Where(x => x.IsDeleted == false).ToList();

            List<TEEscalationMatrixModel> result = new List<TEEscalationMatrixModel>();
            foreach (var item in list)
            {
                TEEscalationMatrixModel model = new TEEscalationMatrixModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);

                if (item.QueueID !=null )
                {
                    TEQueue q = db.TEQueues.Find(item.QueueID.Value);
                    if (q != null)
                        model.Queue = new TEQueue
                        {
                            Uniqueid=q.Uniqueid,
                            QueueName=q.QueueName,
                            QueueID=q.QueueID,
                        };
                }
                if (model.Queue == null)
                {
                    model.Queue = new TEQueue();
                }
                if (item.ManagerID!=null)
                {
                    TEEmpBasicInfo mgr = db.TEEmpBasicInfoes.Find(item.ManagerID.Value);
                    if (mgr != null)
                        model.Manager = new TEEmpBasicInfo
                        {
                            Uniqueid=mgr.Uniqueid,
                            FirstName=mgr.FirstName,
                            LastName=mgr.LastName,
                            Mobile=mgr.Mobile,
                            OfficialEmail=mgr.OfficialEmail,
                        };
                }
                if (model.Manager == null)
                {
                    model.Manager = new TEEmpBasicInfo();
                }
                if (item.Priority != null)
                {
                    int picklistid = Convert.ToInt32(item.Priority);
                    TEPickListItem p = db.TEPickListItems.Find(picklistid);
                    if (p != null)
                        model.PriorityName = p.Description;
                }
                result.Add(model);
            }
            return result;
        }

        // GET api/<controller>/5
        public TEEscalationMatrixModel Get(int id)
        {
            TEESCALATIONMATRIX item = db.TEESCALATIONMATRIces.Find(id);

            TEEscalationMatrixModel model = new TEEscalationMatrixModel();

            TETransformEntityNModel translator = new TETransformEntityNModel();

            model = translator.TransformAtoB(item, model);

            if (item.QueueID != null)
            {
                TEQueue q = db.TEQueues.Find(item.QueueID.Value);
                if (q != null)
                    model.Queue = new TEQueue
                    {
                        Uniqueid = q.Uniqueid,
                        QueueName = q.QueueName,
                        QueueID = q.QueueID,
                    };
            }
            if (item.ManagerID != null)
            {
                TEEmpBasicInfo mgr = db.TEEmpBasicInfoes.Find(item.ManagerID.Value);
                if (mgr != null)
                    model.Manager = new TEEmpBasicInfo
                    {
                        Uniqueid = mgr.Uniqueid,
                        FirstName = mgr.FirstName,
                        LastName = mgr.LastName,
                        Mobile = mgr.Mobile,
                        OfficialEmail = mgr.OfficialEmail,
                    };
            }
            if (item.Priority != null)
            {
                int picklistid = Convert.ToInt32(item.Priority);
                TEPickListItem p = db.TEPickListItems.Find(picklistid);
                if (p != null)
                    model.PriorityName = p.Description;
            }

            return model;
        }

       
        // POST api/<controller>
        public TEESCALATIONMATRIX Post(TEESCALATIONMATRIX value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEESCALATIONMATRIX result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEESCALATIONMATRIces.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TEESCALATIONMATRIces.Attach(value);
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
            return db.TEESCALATIONMATRIces.Find(value.Uniqueid);
        }

        
    }
}