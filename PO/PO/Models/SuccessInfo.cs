using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.Models
{
    public class SuccessInfo
    {
        public int listcount { get; set; }
        public int errorcode { get; set; }
        public string errormessage { get; set; }
        public int fromrecords { get; set; }
        public int torecords { get; set; }
        public int totalrecords { get; set; }
        public string pages { get; set; }
    }
}