using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEComplaintsManagementAPI.Constant
{
    public class IssuesStatus
    {
        public static readonly string[] IssuesStatusList = new string[] 
        {
            //o
            "CLOSED",
            //1
            "DISMISSED",
            //2
            "CANCELLED" 
        };

        public static readonly int IssuesCount = 10;

        public enum IssuesStatusRes
        {
            RESOLVED
        }

        public enum TEComplaintsRules
        {
            TicketAutoClose
        }

        public static readonly string[] EscalationStatus = new string[] 
        {
            "RESOLVED",
            "CLOSED",
            "DISMISSED",
            "CANCELLED",
            "PAUSED"
        };
    }
}