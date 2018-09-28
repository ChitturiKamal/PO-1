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
    public class TECategoryController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TECategoryModel> Get()
        {
            List<TECategory> list = db.TECategories.Where(x => x.IsDeleted == false).ToList();

            List<TECategoryModel> result = new List<TECategoryModel>();
            foreach (var item in list)
            {
                TECategoryModel model = new TECategoryModel();

                TETransformEntityNModel translator = new TETransformEntityNModel();

                model = translator.TransformAtoB(item, model);

                if (item.Parent.Value > 0)
                {
                    TECategory cat = db.TECategories.Find(item.Parent.Value);
                    model.ParentCategory = cat;
                }
                if (model.ParentCategory == null)
                {
                    model.ParentCategory = new TECategory();
                }
                result.Add(model);
            }
            return result;
        }

        // GET api/<controller>/5
        [HttpGet]
        public TECategoryModel Get(int id)
        {
            TECategory item = db.TECategories.Find(id);

            TECategoryModel model = new TECategoryModel();
            TETransformEntityNModel translator = new TETransformEntityNModel();

            model = translator.TransformAtoB(item, model);

            if (item.Parent.Value > 0)
            {
                TECategory cat = db.TECategories.Find(item.Parent.Value);
                model.ParentCategory = cat;
            }

            return model;
        }

        // POST api/<controller>
        [HttpPost]
        public TECategory Post(TECategory value)
        {
            TECategory result = value;

            if (!(value.Uniqueid + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result = db.TECategories.Add(value);
            } 
            else
            {
                db = new TEHRIS_DevEntities();
                db.TECategories.Attach(value); 

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

            return db.TECategories.Find(value.Uniqueid);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TECategory> GetAllParent()
        {
            return db.TECategories.Where(x=>
                (x.Parent==null)
                ||
                (x.Parent+"".Trim().Length==0)
                );
        }
        [HttpGet]
        public IEnumerable<TECategory> GetCategorybyQueue(int queue)
        {
            return db.TECategories.Where(x =>
                (x.Name == (from que in db.TEQueues where que.Uniqueid==queue select que.QueueName).Distinct().FirstOrDefault())
                && (x.IsDeleted == false)
                );
        }
        [HttpGet]
        public IEnumerable<TECategory> GetAllParentpmd()
        {
            return db.TECategories.Where(x =>
                (x.Parent == null)
                ||
                (x.Parent + "".Trim().Length == 0)
                && (x.CategoryID == 1)
                );
        }
        [HttpGet]
        public IEnumerable<TECategory> GetAllParentinternal()
        {
            return db.TECategories.Where(x =>
                (x.Parent == null)
                ||
                (x.Parent + "".Trim().Length == 0)
                && (x.CategoryID != 1)
                && x.IsDeleted==false
                );
        }
        [HttpGet]
        public IEnumerable<TECategory> GetAllsubParent(int parentid)
        {
            return db.TECategories.Where(x =>x.Parent == parentid);
        }
        

        [HttpGet]
        public IEnumerable<TECategory> GetAllParentByCategoryId(int id)
        {
            TECategory cat = db.TECategories.Find(id);

            List<TECategory> lsitOfparent = new List<TECategory>(); 

            lsitOfparent.Add(cat);

            for (int i = 0; i < lsitOfparent.Count; i++)
            {
                TECategory immdiateParent = db.TECategories.Find(lsitOfparent[i].Parent);
                 
                if (lsitOfparent.Count > 100 || immdiateParent==null)
                    break;
                 
                lsitOfparent.Add(immdiateParent);
            } 

            return lsitOfparent;
        }

        [HttpGet]
        public string GetAllParentNameByCategoryId(int id)
        {
            IEnumerable<TECategory> catList=GetAllParentByCategoryId(id);
            string result = string.Empty;
            for (int i = catList.Count()-1; i >= 0; i--)
            { 
                result+=catList.ElementAt(i).Name+";";
            }

            return result;
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TECategory> GetLeafChildren()
        {
            var innerQuery = (from cat in db.TECategories
                              where cat.IsDeleted == false && cat.Parent != null
                              select cat.Parent).Distinct();
            
            return (from childCat in db.TECategories
                    where childCat.IsDeleted == false && innerQuery.Contains((int)(childCat.Uniqueid)) == false 
                    select childCat);
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<Proc_CategoryDepartment_Result> GetLeafChildrenByParentId(int id)
        {
            return db.Proc_CategoryDepartment(id); 
        }
         
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}