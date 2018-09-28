using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TEComplaintsManagementAPI.Models.EmailModels
{
    public class EmailSendModel
    {
        public string Subject { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Html { get; set; }
        public string SenderType { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }
}