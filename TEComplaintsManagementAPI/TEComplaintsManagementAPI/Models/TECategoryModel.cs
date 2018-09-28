using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
    public class TECategoryModel:TECategory
    {
        public TECategory ParentCategory { get; set; }
    }
}