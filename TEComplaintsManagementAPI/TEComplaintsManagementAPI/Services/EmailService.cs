using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TEComplaintsManagementAPI.Models.EmailModels;

namespace TEComplaintsManagementAPI.Services
{
    public class EmailService
    {
        public string SendEmail(EmailSendModel ReqEmail)
        {
            try
            {
                //EmailSendModel Email = new EmailSendModel();
                //Email.Html = "Body";
                ReqEmail.SenderType = System.Configuration.ConfigurationSettings.AppSettings["SenderType"];
                //Email.Subject = "testing";
                ReqEmail.From = System.Configuration.ConfigurationSettings.AppSettings["Fugue"];
                //ReqEmail.To = "deepak.hulluraiah@total-environment.com";
                //ReqEmail.Cc = "Santosh.Sachchidananda@total-environment.com";
                ReqEmail.Cc = "srikanth.t@techuva.com";
                //ReqEmail.Bcc = "deepak.hulluraiah@total-environment.com";
                ReqEmail.Bcc = "sudheer@techuva.com";
                string Url = System.Configuration.ConfigurationSettings.AppSettings["MailDomineUrl"];
                string EmailApi = System.Configuration.ConfigurationSettings.AppSettings["EmailApi"];
                RestClient client = new RestClient();
                client.BaseUrl = new Uri(Url);
                RestRequest request = new RestRequest(EmailApi, Method.POST);
                request.AddObject(ReqEmail);


                //request.AddBody(ReqEmail);
                var response = client.Execute(request);
                string datareturn = Convert.ToString(response.ResponseStatus);

                return datareturn;
                //return null;
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }
    }
}