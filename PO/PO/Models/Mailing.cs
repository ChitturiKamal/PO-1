using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Net.Mime;
using System.Web.Configuration;
using PO.Models;

namespace CEMAPI.Models
{
    public class Mailing
    {
        TETechuvaDBContext context = new TETechuvaDBContext();

        public bool SendEMail(string TO, string CC, string BCC, string mailBody, string subject, string[] attachments, string attachmentFilename)
        {
            bool res = true;
            try
            {
                string FugueEmail = WebConfigurationManager.AppSettings["Fugue"];
                string SmtpServerObj = FugueEmail;
                string SmtpPassword = WebConfigurationManager.AppSettings["FuguePwd"];
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SmtpServerObj);
                mail.From = new MailAddress(FugueEmail);
                if (TO != null && TO != "")
                {
                    char getLastChar = TO[TO.Length - 1];
                    if (getLastChar == ',')
                    {
                        TO = TO.Remove(TO.Length - 1);
                    }
                    mail.To.Add(TO);
                }
                if (CC != null && CC != "")
                {
                    char getLastChar = CC[CC.Length - 1];
                    if (getLastChar == ',')
                    {
                        CC = CC.Remove(CC.Length - 1);
                    }
                    mail.CC.Add(CC);
                }
                if (BCC != null && BCC != "")
                {
                    char getLastChar = BCC[BCC.Length - 1];
                    if (getLastChar == ',')
                    {
                        BCC = BCC.Remove(BCC.Length - 1);
                    }
                    mail.Bcc.Add(BCC);
                }
                
                mail.Subject = subject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;

                System.Net.Mail.Attachment attachment;
                if (attachments.Count() > 0)
                {
                    foreach (string at in attachments)
                    {
                        attachment = new System.Net.Mail.Attachment(at);
                        mail.Attachments.Add(attachment);
                    }
                    if (mail.Attachments.Count > 0)
                    {
                        int cnt = 1;
                        foreach (var data in mail.Attachments)
                        {
                            string output = string.Empty;
                            if (cnt > 1)
                            {
                                output = attachmentFilename + cnt + "." + data.Name.Substring(data.Name.LastIndexOf('.') + 1);
                            }
                            else
                            {
                                output = attachmentFilename + "." + data.Name.Substring(data.Name.LastIndexOf('.') + 1);
                            }
                            data.Name = output;
                            cnt++;
                        }
                    }
                }
                SmtpServer.Host = ConfigurationManager.AppSettings["Host"];
                SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                SmtpServer.Credentials = new System.Net.NetworkCredential(FugueEmail, SmtpPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                SmtpServer.Send(mail);
                return res;
            }
            catch (Exception ex)
            {
                new RecordException().RecordUnHandledException(ex);
                res = false;
                return res;
            }
        }

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
}