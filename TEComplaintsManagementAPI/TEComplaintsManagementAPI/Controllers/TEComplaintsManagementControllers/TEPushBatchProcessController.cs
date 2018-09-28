using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models; 
namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEPushBatchProcessController : ApiController
    {
        //
        // GET: /TEPushBatchProcess/

        TEHRIS_DevEntities db = new TEHRIS_DevEntities();
        [HttpGet]
        public TENotification TEpushAttendenceRegularise(String NotificationModuleName,int Employee,DateTime Dte,string AttendenceType,string Leavetype,String Status)
        {
            TENotification result = new TENotification();
            var descr = from noti in db.TENotificationsTemplates
                        where (noti.IsDeleted == false) && (noti.ModuleName == NotificationModuleName)
                        select (noti.NotificationsTemplate.Replace("<Employee>", (from s in db.TEEmpBasicInfoes where (s.Uniqueid == Employee) select s.Salutation + '.' + s.FirstName + " " + s.LastName).Distinct().FirstOrDefault()))
                                .Replace("<Date>", Dte.ToString())
                                .Replace("<Mobile/Computer>", AttendenceType)
                                .Replace("<approved/Rejected>", Leavetype)
                                .Replace("<Leave Type>", Status);
            return result;
        }

        private TENotification SendNotification(TENotificationModel value)
        {
            var usr = db.UserProfiles.Where(x => x.UserId == Convert.ToInt32(value.SendBy)).FirstOrDefault();
            string description1 = "Issue: " + value.description + " " + usr.CallName.ToString();

            TENotification notify = new TENotification
            {
                ApprovedBy = value.ApprovedBy,
                ApprovedOn = System.DateTime.UtcNow,
                CreatedBy = value.CreatedBy,
                CreatedOn = System.DateTime.UtcNow,
                //Have to be dynamic...

                IsDeleted = false,
                LastModifiedBy = value.ApprovedBy,
                LastModifiedOn = System.DateTime.UtcNow,

                Name = "Issue: " + value.Status,
                description = description1,
                ReceivedBy = value.ReceivedBy,
                SendBy = value.SendBy,
                ReadStatus = false,
                Status = value.Status,
                //Type

            };

            var usrtype = (from usrs in db.UserProfiles
                           join tebas in db.TEEmpBasicInfoes on usrs.UserName equals tebas.UserId
                           where (usrs.UserId == Convert.ToInt32(value.SendBy))
                           select usrs.UserId).Distinct().FirstOrDefault();
            string apptype = "Fugue";
            if (usrtype == null)
            {
                apptype = "Yellow";
            }
            string res = "";
            if (usr.AndroidToken != null)
            {
                try
                {
                    TECommonLogicLayer.TEPushNotification tepush = new TECommonLogicLayer.TEPushNotification();
                    //tepush.SendNotification_Android("APA91bEWoz9U6m9VK-oBW1Jy-EIT6Fa9_xnWTOIyVfu-pdXpemMrahsiDW3A2MFMff9FuwKpV-EJ1-N4pfctx3EM2rnX1wAgGKIBfS1iLdCvus1VgYwpDbA", "Issue: " + value.Status.ToString(), "Fugue");
                    res = tepush.SendNotification_Android(usr.AndroidToken.ToString(), description1, apptype);

                    //AndroidNotification.Notification tepushandroid = new AndroidNotification.Notification();
                    //res=tepushandroid.AndroidNotification(usr.AndroidToken.ToString(), description1);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            if (usr.IosToken != null)
            {
                try
                {
                    TECommonLogicLayer.TEPushNotification tepush = new TECommonLogicLayer.TEPushNotification();
                    //tepush.SendNotification_IOS("0c11cdc92d14b96d6f309cc661c4cd36abe9cb1f38bccf4019de31b6cf97992b", "Issue: " + value.Status.ToString(), "Yellow");
                    tepush.SendNotification_IOS(usr.IosToken.ToString(), description1, apptype);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return new TENotificationsController().AddNotifications(notify);


        }

    }
}
