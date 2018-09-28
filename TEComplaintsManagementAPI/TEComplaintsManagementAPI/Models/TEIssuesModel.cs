using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEComplaintsManagementAPI.Models
{
    public class TEIssuesModel
    {
        public IEnumerable<TEComplainceModel> Issues { get; set; }
        public object KeyNCount { get; set; }
    }

   
}