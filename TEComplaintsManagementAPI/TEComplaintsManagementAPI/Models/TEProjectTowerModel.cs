﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
    public class TEProjectTowerModel:TEProjects_TOWER
    {
        public TEProject TowerInProject { get; set; }
    }
}