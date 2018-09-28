using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.BAL
{
    public class FundcenterManagerMap
    {
        public int FundCenterPOMgrMappingId { get; set; }
        public int? UserId { get; set; }
        public int? FundCenterId { get; set; }
        public string UserName { get; set; }
        public string FundCenter_Description { get; set; }
        public string FundCenter_Code { get; set; }
        public int? LastModifiedBy { get; set; }
        public String LastModifiedBy_Name { get; set; }
        public Nullable<DateTime> LastModifiedOn { get; set; }
    }
}