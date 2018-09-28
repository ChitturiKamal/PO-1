using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer; 

namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TETechnicianController : ApiController
    {
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEEmpBasicInfo> GetByIssueId(int id)
        {
            //Step-1 Get category id from TEIssue by issue id 
            int catId = Convert.ToInt32(db.TEIssues.Find(id).CategoryID);

            //Step-2 Get Subfunctions from TEQueueDepartment by category Id 
            List<int?> subFunctionIdList=db.TEQueueDepartments
                                        .Where(x=>
                                            (x.CategoryID==catId)
                                            )
                                        .Select(x => x.SubfunctionID).Distinct().ToList();

            //Step-3 Get TEEmpBasicInfoId from TEEmpAssignmentDetails by subFunctionIdList 
            List<int?> teEmpBasicInfoId=db.TEEmpAssignmentDetails
                                        .Where(x=>(subFunctionIdList.Contains(x.TESubFunction)))
                                        .Select(x => x.TEEmpBasicInfo).Distinct().ToList();

            //Step-4 Get all employee by employee uniqueid.
            db.Configuration.ProxyCreationEnabled = false;
            return db.TEEmpBasicInfoes.Where(x => (teEmpBasicInfoId.Contains(x.Uniqueid))); 
        } 
    }
}