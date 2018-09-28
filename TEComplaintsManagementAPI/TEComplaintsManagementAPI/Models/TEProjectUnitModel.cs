using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
    public class TEProjectUnitModel:TEProjects_UNIT
    {
        public TEProjects_TOWER Tower { get; set; }
        public TEProject Project { get; set; }
    }
}