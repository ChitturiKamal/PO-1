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
    public class TEProjectUnitController : ApiController
    {
        //
        // GET: /TEUnit/ 
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEProjectUnitModel> Get()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TEProjects_UNIT> list = db.TEProjects_UNIT.Where(x => (x.IsDeleted == false)
                ).ToList();

            List<TEProjectUnitModel> result = new List<TEProjectUnitModel>();
            foreach (var item in list)
            {
                TEProjectUnitModel model = new TEProjectUnitModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);
                #region Commented due to entity change
                //if (item.PROJECT_ID!=null)
                //{
                //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
                //    if (pro != null)
                //        model.Project = new TEProject
                //        {
                //            Uniqueid = pro.Uniqueid,
                //            ProjectCode = pro.ProjectCode,
                //            ProjectName = pro.ProjectName,
                //            ProjectShortName = pro.ProjectShortName,
                //            ProjectStatus = pro.ProjectStatus,
                //            IsNewProject = pro.IsNewProject,
                //            City = pro.City,
                //            COLOURCODE = pro.COLOURCODE,
                //        };

                //}
                #endregion
                if (model.Project == null)
                    model.Project = new TEProject();

                if (item.TOWERID != null)
                {
                    TEProjects_TOWER tower = db.TEProjects_TOWER.Find(item.TOWERID);
                    if (tower != null)
                        model.Tower = new TEProjects_TOWER
                        {
                            Uniqueid = tower.Uniqueid,
                            TOWERNAME = tower.TOWERNAME,
                            TOWERCODE = tower.TOWERCODE,
                            DESCRIPTION = tower.DESCRIPTION
                        };
                }

                if (model.Tower == null)
                    model.Tower = new TEProjects_TOWER();
                result.Add(model);
            }
            return result;
        }
        #region Commented due to entity change
        //[HttpGet]
        //public object Getunitlist()
        //{
        //    var profile = (from proj in db.TEProjects
        //                  join tow in db.TEProjects_TOWER on proj.Uniqueid equals tow.PROJECT_ID
        //                  join unit in db.TEProjects_UNIT on tow.Uniqueid equals unit.TOWERID
        //                  where proj.IsDeleted == false && tow.IsDeleted == false && unit.IsDeleted == false
        //                  orderby proj.ProjectName, tow.TOWERNAME, unit.AREA,unit.Uniqueid
        //                   select new { proj.ProjectName, tow.TOWERNAME, unit.AREA, unit.Uniqueid }).Distinct().ToList();
        //    return profile;
        //}
        #endregion
        // GET api/<controller>/5
        [HttpGet]
        public TEProjectUnitModel Get(int id)
        {
            TEProjects_UNIT item = db.TEProjects_UNIT.Find(id);
            TEProjectUnitModel model = new TEProjectUnitModel();

            TETransformEntityNModel translator = new TETransformEntityNModel();

            model = translator.TransformAtoB(item, model);
            #region Commented due to entity change
            //if (item.PROJECT_ID.Value > 0)
            //{
            //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
            //    if (pro != null)
            //        model.Project = new TEProject
            //        {
            //            Uniqueid = pro.Uniqueid,
            //            ProjectCode = pro.ProjectCode,
            //            ProjectName = pro.ProjectName,
            //            ProjectShortName = pro.ProjectShortName,
            //            ProjectStatus = pro.ProjectStatus,
            //            IsNewProject = pro.IsNewProject,
            //            City = pro.City,
            //            COLOURCODE = pro.COLOURCODE,
            //        };

            //}
            #endregion
            if (item.TOWERID != null)
            {
                TEProjects_TOWER tower = db.TEProjects_TOWER.Find(item.TOWERID);
                if (tower != null)
                    model.Tower = new TEProjects_TOWER
                    {
                        Uniqueid = tower.Uniqueid,
                        TOWERNAME = tower.TOWERNAME,
                        TOWERCODE = tower.TOWERCODE,
                        DESCRIPTION = tower.DESCRIPTION
                    };
            }
            return model;
        }

        public IEnumerable<TEProjectUnitModel> GetByProjectNTowerId(int projectId, int towerId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TEProjects_UNIT> list = db.TEProjects_UNIT.Where(x => (x.IsDeleted == false)
                && (x.PROJECT_ID == projectId)
                && (x.TOWERID == towerId)
                ).ToList();

            List<TEProjectUnitModel> result = new List<TEProjectUnitModel>();
            foreach (var item in list)
            {
                TEProjectUnitModel model = new TEProjectUnitModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);
                #region Commented due to entity change
                //if (item.PROJECT_ID.Value > 0)
                //{
                //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
                //    if (pro != null)
                //        model.Project = new TEProject
                //        {
                //            Uniqueid = pro.Uniqueid,
                //            ProjectCode = pro.ProjectCode,
                //            ProjectName = pro.ProjectName,
                //            ProjectShortName = pro.ProjectShortName,
                //            ProjectStatus = pro.ProjectStatus,
                //            IsNewProject = pro.IsNewProject,
                //            City = pro.City,
                //            COLOURCODE = pro.COLOURCODE,
                //        };

                //}
                #endregion
                if (item.TOWERID!=null)
                {
                    TEProjects_TOWER tower = db.TEProjects_TOWER.Find(item.TOWERID);
                    if (tower != null)
                        model.Tower = new TEProjects_TOWER
                        {
                            Uniqueid = tower.Uniqueid,
                            TOWERNAME = tower.TOWERNAME,
                            TOWERCODE = tower.TOWERCODE,
                            DESCRIPTION = tower.DESCRIPTION
                        }; 
                }
                result.Add(model);
            }
            return result;
        }
       
        // POST api/<controller>
        [HttpPost]
        public TEProjects_UNIT Post(TEProjects_UNIT value)
        {
            TEProjects_UNIT result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEProjects_UNIT.Add(value);
            } 
            else
            {
                db = new TEHRIS_DevEntities();
                db.TEProjects_UNIT.Attach(value);

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

            return db.TEProjects_UNIT.Find(value.Uniqueid);
        }

        [HttpGet]
        public IEnumerable<TEProjects_TOWER> GetTowerByProjectId(int id)
        {
            return db.TEProjects_TOWER.Where(x => x.PROJECT_ID == id);
        }
        [HttpGet]
        public IEnumerable<TEProjects_UNIT> GetUnitByProjecttowerId(int pid,int tid)
        {
            return db.TEProjects_UNIT.Where(x => (x.PROJECT_ID == pid)&&(x.TOWERID==tid));
        }

        [HttpGet]
        public Object GetUnitByProjectId(int id)
        {
            var unit = from Unit in db.TEUnits
                       join Subp in db.TESubProductDetails on Unit.ProjectProductID equals Subp.ProjectProductID
                       join Proj in db.TEProjects on Subp.ProjectID equals Proj.ProjectID
                       where Proj.ProjectID == id
                       select new
                       {
                           Unit.UnitID,
                           Unit.UnitNumber
                       };
            return unit;
        }

        [HttpGet]
        public object GetUnitByQueueId(int Queueid)
        {
            TEQueue listQueue = db.TEQueues.Where(x => (x.Uniqueid == Queueid)).ToList().First();
            if (listQueue ==null)
            {
                return "No Units Found";
            }
                return db.TEProjects_UNIT.Where(x => x.PROJECT_ID == listQueue.PROJECTID);
        } 

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}
