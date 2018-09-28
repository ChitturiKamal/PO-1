using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TEComplaintsManagementAPI.Models;
using TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers;
using TEComplaintsManagementAPI.Constant;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models.EmailModels;
using TEComplaintsManagementAPI.Services;
using System.Data;
namespace TEComplaintsManagementAPI.Controllers.MailTrigger
{
    public class TEComplaintsMailTriggersController : ApiController
    {
        TEIssuesController Mgr = new TEIssuesController();
        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        EmailService EmailMgr = new EmailService();
        // GET api/tecomplaintsmailtriggers
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/tecomplaintsmailtriggers/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tecomplaintsmailtriggers
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tecomplaintsmailtriggers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tecomplaintsmailtriggers/5
        public void Delete(int id)
        {
        }
        [HttpGet]
        public string AutoCloseIssues()
        {
            string status = IssuesStatus.IssuesStatusRes.RESOLVED.ToString();
            string Rule = IssuesStatus.TEComplaintsRules.TicketAutoClose.ToString();
            DateTime Today=System.DateTime.Today;
            List<TEComplainceModel> teissueresult = Mgr.GetIssues().Where(x => x.Status == status).ToList();
            TERule ResolvingDays = db.TERules.Where(x => x.RuleName == Rule).FirstOrDefault();
            int EstimantionDay=Convert.ToInt32(ResolvingDays.Value);

            foreach (TEComplainceModel item in teissueresult)
            {
                DateTime ResolvedDate=Convert.ToDateTime(item.Resolved_Date);
                if (Today > ResolvedDate.AddDays(EstimantionDay))
                {
                    try
                    {
                        TEIssue Issues = db.TEIssues.Find(item.Uniqueid);
                        Issues.Status = IssuesStatus.IssuesStatusList[0];
                        db.Entry(Issues).State = EntityState.Modified;
                        //db.Entry(Issues).Property(X=>X.Status).IsModified = true;
                        //db.TEIssues.Attach(Issues);
                        db.SaveChanges();
                        //EmailSendModel SendDetails = new EmailSendModel();
                        //var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "Ticket Closed").FirstOrDefault();
                        //SendDetails.Subject = potemp1.Subject.Replace("@ticket number@", Issues.Uniqueid.ToString());
                        //SendDetails.Html = potemp1.EmailTemplate.Replace("@Requestor Name@", (from x in db.UserProfiles where (x.UserId == Issues.RaisedBy) select x.CallName).Distinct().FirstOrDefault());
                        //SendDetails.Html = SendDetails.Html.Replace("@ticket number@", Issues.Uniqueid.ToString());
                        //SendDetails.To = (from x in db.UserProfiles where (x.UserId == Issues.RaisedBy) select x.email).Distinct().FirstOrDefault() == null ? (from x in db.TEContacts where (x.UserId == Issues.RaisedBy) select x.Emailid).Distinct().FirstOrDefault() : (from x in db.UserProfiles where (x.UserId == Issues.RaisedBy) select x.email).Distinct().FirstOrDefault();
                        //EmailMgr.SendEmail(SendDetails);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }

            return "Success";
        }

        [HttpGet]
        public string TEEscalationMails()
        {
            DateTime Today = System.DateTime.Today;
            int cnt = 0;
            List<TEComplainceModel> teissueresult = Mgr.GetIssues().Where(x => !IssuesStatus.EscalationStatus.Contains(x.Status)).ToList();
            List<TEComplainceModel> escalatedissueresult = new List<TEComplainceModel>();
            List<TEEmpBasicInfo> ManagerList = new List<TEEmpBasicInfo>();
            foreach (TEComplainceModel item in teissueresult)
            {
                DateTime EscalationDate=System.DateTime.Today;
                TEQueueDepartment Escalation = db.TEQueueDepartments.Where(x => x.QueueID == item.QueueID && x.CategoryID == item.CategoryID).FirstOrDefault();
                try
                {
                    if (Escalation != null)
                    {
                        if (item.PriorityName == "Normal")
                        {
                            EscalationDate = item.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAMedium));
                            item.issuePriority = 3;
                        }
                        else if (item.PriorityName == "Critical")
                        {
                            EscalationDate = item.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLAHigh));
                            item.issuePriority = 2;
                        }
                        else if (item.PriorityName == "Emergency")
                        {
                            EscalationDate = item.CreatedOn.AddDays(Convert.ToDouble(Escalation.SLACritical));
                            item.issuePriority = 1;
                        }
                    }
                    if (Today > EscalationDate)
                    {
                        TimeSpan difference = Today - item.CreatedOn.Date;
                        item.Age = Convert.ToInt32(difference.TotalDays);
                        UserProfile Assige = new UserProfile();
                        Assige = db.UserProfiles.Where(x => x.UserId == item.AssignedTo).FirstOrDefault();
                        string AssigneUserId = Convert.ToString(item.AssignedTo);
                        TEEmpBasicInfo AssigneBasic=null;
                        if (Assige != null)
                        {
                            AssigneBasic = new TEEmpBasicInfo();
                            AssigneBasic = db.TEEmpBasicInfoes.Where(x => x.UserId == Assige.UserName).FirstOrDefault();
                        }
                        TEEmpAssignmentDetail AssigneDetails = null;
                        if (AssigneBasic != null)
                        {
                            AssigneDetails = new TEEmpAssignmentDetail();
                            AssigneDetails = db.TEEmpAssignmentDetails.Where(x => x.TEEmpBasicInfo == AssigneBasic.Uniqueid && x.IsDeleted == false && x.Status == "Active").FirstOrDefault();
                        }
                        if (AssigneDetails != null)
                        {
                            TEEmpBasicInfo ManagerBasic = db.TEEmpBasicInfoes.Find(AssigneDetails.TEEmpBasicInfo_ReportingTo);
                            if (ManagerBasic != null)
                            {
                                item.AssigneManagerId = ManagerBasic.Uniqueid;
                                item.AssigneManagerName = ManagerBasic.FirstName + " " + ManagerBasic.LastName;
                                ManagerList.Add(ManagerBasic);
                            }
                        }
                        escalatedissueresult.Add(item);
                    }
                }
                catch (Exception ex)
                {                    
                    return ex.Message;
                }
            }
            ManagerList=ManagerList.Distinct().Where(a=>a.OfficialEmail!=null).ToList();
            foreach (TEEmpBasicInfo basicitem in ManagerList)
            {
                EmailSendModel SendDetails = new EmailSendModel();
                var potemp1 = db.TEEmailTemplates.Where(x => x.ModuleName == "TicketEscalation").FirstOrDefault();
                string bodytag = "";

                foreach (TEComplainceModel item in escalatedissueresult.OrderBy(x => x.issuePriority).ThenByDescending(x => x.Age).Where(x => x.AssigneManagerId == basicitem.Uniqueid).ToList())
                {
                try
                {
                    bodytag = bodytag + "<tr style='font-size: 9pt !important;'><td style='text-align:right;'>@ticket number@</td><td>@Category@</td><td>@Requestor Name@</td><td>@Requestor Date@</td><td>@Priority@</td><td>@Resolution Date@</td><td style='text-align:right;'>@Age@</td><td>@Technician name@</td><td>@Description@</td></tr>";
                    bodytag = bodytag.Replace("@ticket number@", item.Uniqueid.ToString());
                    bodytag = bodytag.Replace("@Requestor Name@", (item.RaisedByName));
                    bodytag = bodytag.Replace("@Requestor Date@", item.CreatedOn == null ? "" : String.Format("{0:dd-MM-yyyy}", item.CreatedOn));
                    bodytag = bodytag.Replace("@Manager Name@", item.AssigneManagerName);
                    bodytag = bodytag.Replace("@Category@", item.Categoryname);
                    bodytag = bodytag.Replace("@Priority@", item.PriorityName);
                    bodytag = bodytag.Replace("@Resolution Date@", item.Estimated_Close_date == null ? "" : String.Format("{0:dd-MM-yyyy}", item.Estimated_Close_date));
                    bodytag = bodytag.Replace("@Description@", item.Descritpion);
                    bodytag = bodytag.Replace("@Technician name@", item.AssignToName);
                    bodytag = bodytag.Replace("@Age@", item.Age.ToString());
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                }
                SendDetails.Subject = potemp1.Subject;
                SendDetails.Html = potemp1.EmailTemplate.Replace("@bodytag@", bodytag);
                SendDetails.Html = SendDetails.Html.Replace("@Manager Name@", basicitem.FirstName+" "+basicitem.LastName);
                SendDetails.To = basicitem.OfficialEmail;
                EmailMgr.SendEmail(SendDetails);
            }
            return "Success";
        }
    }
}
