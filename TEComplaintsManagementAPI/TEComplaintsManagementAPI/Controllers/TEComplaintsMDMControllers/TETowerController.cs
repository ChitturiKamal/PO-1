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
    public class TETowerController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        public IEnumerable<TEProjectTowerModel> Get()
        { 
            db.Configuration.ProxyCreationEnabled = false;
            List<TEProjects_TOWER> list = db.TEProjects_TOWER.Where(x => x.IsDeleted == false).ToList();

            List<TEProjectTowerModel> result = new List<TEProjectTowerModel>();
            foreach (var item in list)
            {
                TEProjectTowerModel model = new TEProjectTowerModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);
                #region Commented due to entity change
                //if (item.PROJECT_ID!=null)
                //{
                //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
                //    if(pro!=null)
                //    model.TowerInProject = new TEProject
                //    {
                //        Uniqueid = pro.Uniqueid,
                //        ProjectCode=pro.ProjectCode,
                //        ProjectName=pro.ProjectName,
                //        ProjectShortName=pro.ProjectShortName,
                //        ProjectStatus=pro.ProjectStatus,
                //        IsNewProject=pro.IsNewProject,
                //        City=pro.City,
                //        COLOURCODE=pro.COLOURCODE,
                //    };
                //}

                //if (model.TowerInProject == null)
                //{
                //    model.TowerInProject = new TEProject();
                //}

                #endregion
                result.Add(model);
            }
            return result;
        }

        // GET api/<controller>/5
        public TEProjectTowerModel Get(int id)
        {
            TEProjects_TOWER item = db.TEProjects_TOWER.Find(id);

            TEProjectTowerModel model = new TEProjectTowerModel();

            TETransformEntityNModel translator = new TETransformEntityNModel();

            model = translator.TransformAtoB(item, model);
            #region Commented due to entity change
            //if (item.PROJECT_ID.Value > 0)
            //{
            //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
            //    if (pro != null)
            //        model.TowerInProject = new TEProject
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
            return model;
        }

        public IEnumerable<TEProjectTowerModel> GetByProjectId(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<TEProjects_TOWER> list = db.TEProjects_TOWER.Where(x => (x.IsDeleted == false)
                &&(x.PROJECT_ID==id)).ToList();

            List<TEProjectTowerModel> result = new List<TEProjectTowerModel>();
            foreach (var item in list)
            {
                TEProjectTowerModel model = new TEProjectTowerModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);
                #region Commented due to entity change
                //if (item.PROJECT_ID.Value > 0)
                //{
                //    TEProject pro = db.TEProjects.Find(item.PROJECT_ID.Value);
                //    if (pro != null)
                //        model.TowerInProject = new TEProject
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
                result.Add(model);
            }
            return result;
        }

        // POST api/<controller>
        public TEProjects_TOWER Post(TEProjects_TOWER value)
        {
            db.Configuration.ProxyCreationEnabled = false;
            TEProjects_TOWER result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                //Create
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TEProjects_TOWER.Add(value);
            }
            else
            {
                //Edit
                db = new TEHRIS_DevEntities();
                db.TEProjects_TOWER.Attach(value);
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
            return db.TEProjects_TOWER.Find(value.Uniqueid);
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