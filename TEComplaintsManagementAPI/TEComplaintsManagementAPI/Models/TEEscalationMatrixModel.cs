using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
    public class TEEscalationMatrixModel:TEESCALATIONMATRIX
    {
        public TEQueue Queue { get; set; }
        public TEEmpBasicInfo  Manager { get; set; }
        public string PriorityName { get; set; }
    }
}