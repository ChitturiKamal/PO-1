using CEMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.Models
{
    public class EmailSendingBL
    {
        TETechuvaDBContext db = new TETechuvaDBContext();
        RecordException exceptionObj = new RecordException();
        public void POEmail_SubmitForApproval(int? phsId,int? submitterId, int? lastModifiedBy)
        {
            try
            {
                ApproverEmailProfile submitterInfo = new ApproverEmailProfile();
                ApproverEmailProfile firstApproverInfo = new ApproverEmailProfile();

                var poApproversList = (from apr in db.TEPOApprovers
                                 where apr.IsDeleted == false && apr.POStructureId == phsId
                                 select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).OrderBy(a => a.SequenceNumber).ToList();
                var purchaseheaderstructInfo = (from phs in db.TEPOHeaderStructures
                               join vendor in db.TEPOVendorMasterDetails on phs.VendorID equals vendor.POVendorDetailId into tempvendorDtl
                               from vndrdtl in tempvendorDtl.DefaultIfEmpty()
                               join vendorms in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendorms.POVendorMasterId into tempvendorMstr
                               from vndrmstr in tempvendorMstr.DefaultIfEmpty()
                               where phs.Uniqueid == phsId && phs.IsDeleted == false
                               select new
                               {
                                   phs.Uniqueid,
                                   phs.PO_Title,
                                   phs.Fugue_Purchasing_Order_Number,
                                   phs.Purchasing_Order_Number,
                                   phs.Version,
                                   Vendor_Code= vndrdtl.VendorCode,
                                   Vendor_Owner=vndrmstr.VendorName
                               }).FirstOrDefault();
                if (poApproversList.Count > 0)
                {
                    if (submitterId > 0)
                    {
                        submitterInfo = getUserInfo(submitterId);
                    }
                    int? firstApproverId = 0;
                    firstApproverId = poApproversList.Where(a => a.SequenceNumber == 1).Select(a => a.ApproverId).FirstOrDefault();
                    if (firstApproverId > 0)
                    {
                        firstApproverInfo = getUserInfo(firstApproverId);
                    }
                }
                if (submitterInfo != null)
                {
                    string templateContent = string.Empty;
                    string subjectContent = string.Empty;
                    var templateinfo = db.TEEmailTemplates.Where(a => a.ModuleName == "POSubmit" && a.IsDeleted == false).FirstOrDefault();
                    if (templateinfo != null)
                    {
                        templateContent = templateinfo.EmailTemplate;
                        subjectContent = templateinfo.Subject;
                    }
                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        templateContent = templateContent.Replace("$Employee", firstApproverInfo.ApproverName);
                        if (purchaseheaderstructInfo.Purchasing_Order_Number != null) templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Purchasing_Order_Number);
                        else templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Uniqueid.ToString());
                        templateContent = templateContent.Replace("$POTitle", purchaseheaderstructInfo.PO_Title);
                        decimal? poAmount = 0;
                        poAmount = db.TEPOItemStructures.Where(a => a.IsDeleted == false && a.POStructureId == phsId).Sum(a => a.GrossAmount);
                        templateContent = templateContent.Replace("$POValue", poAmount.ToString());
                        templateContent = templateContent.Replace("$POVersion", purchaseheaderstructInfo.Version);
                        templateContent = templateContent.Replace("$VendorName", purchaseheaderstructInfo.Vendor_Code + '-' + purchaseheaderstructInfo.Vendor_Owner);
                        templateContent = templateContent.Replace("$SubmitterName", submitterInfo.ApproverName);
                    }
                    if (!string.IsNullOrEmpty(subjectContent))
                    {
                        subjectContent = subjectContent.Replace("$VendorName", purchaseheaderstructInfo.Vendor_Code + '-' + purchaseheaderstructInfo.Vendor_Owner);
                    }
                    string TO = firstApproverInfo.EmailId;
                    string CC = submitterInfo.EmailId;
                    string BCC = "";
                    string body = templateContent;
                    string subject = subjectContent;
                    string[] attachments = new string[0];
                    var mail = new Mailing();
                    var sendingmail = mail.SendEMail(TO, CC, BCC, body, subject, attachments, null);
                }
            }
            catch (Exception ex)
            {
                exceptionObj.RecordUnHandledException(ex);
            }
        }

        public void POEmail_Approved(int? phsId, int? submitterId, int? lastModifiedBy)
        {
            try
            {
                ApproverEmailProfile submitterInfo = new ApproverEmailProfile();
                ApproverEmailProfile nextApproverInfo = new ApproverEmailProfile();
                ApproverEmailProfile loginApproverInfo = getUserInfo(lastModifiedBy);
                var poApproversList = (from apr in db.TEPOApprovers
                                       where apr.IsDeleted == false && apr.POStructureId == phsId
                                       select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).OrderBy(a => a.SequenceNumber).ToList();
                var purchaseheaderstructInfo = (from phs in db.TEPOHeaderStructures
                                                join vendor in db.TEPOVendorMasterDetails on phs.VendorID equals vendor.POVendorDetailId into tempvendorDtl
                                                from vndrdtl in tempvendorDtl.DefaultIfEmpty()
                                                join vendorms in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendorms.POVendorMasterId into tempvendorMstr
                                                from vndrmstr in tempvendorMstr.DefaultIfEmpty()
                                                where phs.Uniqueid == phsId && phs.IsDeleted == false
                                                select new
                                                {
                                                    phs.Uniqueid,
                                                    phs.PO_Title,
                                                    phs.Fugue_Purchasing_Order_Number,
                                                    phs.Purchasing_Order_Number,
                                                    phs.Version,
                                                    Vendor_Code = vndrdtl.VendorCode,
                                                    Vendor_Owner = vndrmstr.VendorName
                                                }).FirstOrDefault();
                if (poApproversList.Count > 0)
                {
                   if (submitterId > 0)
                    {
                        submitterInfo = getUserInfo(submitterId);
                    }
                    int? loginApproverSeqId = 0;
                     loginApproverSeqId= poApproversList.Where(a => a.ApproverId == lastModifiedBy).OrderBy(a => a.SequenceNumber).Select(a => a.SequenceNumber).FirstOrDefault();
                    int? nextApproverId = 0;
                    nextApproverId = poApproversList.Where(a => a.SequenceNumber == loginApproverSeqId+1).Select(a => a.ApproverId).FirstOrDefault();
                    if (nextApproverId > 0)
                    {
                        nextApproverInfo = getUserInfo(nextApproverId);
                    }
                }
                if (submitterInfo != null)
                {
                    string templateContent = string.Empty;
                    string subjectContent = string.Empty;
                    var templateinfo = db.TEEmailTemplates.Where(a => a.ModuleName == "POApprove" && a.IsDeleted == false).FirstOrDefault();
                    if (templateinfo != null)
                    {
                        templateContent = templateinfo.EmailTemplate;
                        subjectContent = templateinfo.Subject;
                    }
                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        templateContent = templateContent.Replace("$Employee", submitterInfo.ApproverName);
                        if (purchaseheaderstructInfo.Purchasing_Order_Number != null) templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Purchasing_Order_Number);
                        else templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Uniqueid.ToString());
                        templateContent = templateContent.Replace("$VendorName", purchaseheaderstructInfo.Vendor_Code + '-' + purchaseheaderstructInfo.Vendor_Owner);
                        templateContent = templateContent.Replace("$ApproverName", loginApproverInfo.ApproverName);
                    }
                    if (!string.IsNullOrEmpty(subjectContent))
                    {
                        subjectContent = subjectContent.Replace("$ApproverName", loginApproverInfo.ApproverName);
                        if (purchaseheaderstructInfo.Purchasing_Order_Number != null)
                            subjectContent = subjectContent.Replace("$PO", purchaseheaderstructInfo.Purchasing_Order_Number);
                        else
                            subjectContent = subjectContent.Replace("$PO", purchaseheaderstructInfo.Uniqueid.ToString());
                    }
                    string TO = nextApproverInfo.EmailId;
                    string CC = submitterInfo.EmailId;
                    string BCC = "";
                    string body = templateContent;
                    string subject = subjectContent;
                    string[] attachments = new string[0];
                    var mail = new Mailing();
                    var sendingmail = mail.SendEMail(TO, CC, BCC, body, subject, attachments, null);
                }
            }
            catch (Exception ex)
            {
                exceptionObj.RecordUnHandledException(ex);
            }
        }

        public void POEmail_Rejected(int? phsId, int? submitterId, int? lastModifiedBy)
        {
            try
            {
                ApproverEmailProfile submitterInfo = new ApproverEmailProfile();

                ApproverEmailProfile loginApproverInfo = getUserInfo(lastModifiedBy);

                var poApproversList = (from apr in db.TEPOApprovers
                                       where apr.IsDeleted == false && apr.POStructureId == phsId
                                       select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).OrderBy(a => a.SequenceNumber).ToList();
                var purchaseheaderstructInfo = (from phs in db.TEPOHeaderStructures
                                                join vendor in db.TEPOVendorMasterDetails on phs.VendorID equals vendor.POVendorDetailId into tempvendorDtl
                                                from vndrdtl in tempvendorDtl.DefaultIfEmpty()
                                                join vendorms in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendorms.POVendorMasterId into tempvendorMstr
                                                from vndrmstr in tempvendorMstr.DefaultIfEmpty()
                                                where phs.Uniqueid == phsId && phs.IsDeleted == false
                                                select new
                                                {
                                                    phs.Uniqueid,
                                                    phs.PO_Title,
                                                    phs.Fugue_Purchasing_Order_Number,
                                                    phs.Purchasing_Order_Number,
                                                    phs.Version,
                                                    Vendor_Code = vndrdtl.VendorCode,
                                                    Vendor_Owner = vndrmstr.VendorName
                                                }).FirstOrDefault();
                if (poApproversList.Count > 0)
                {
                    if (submitterId > 0)
                    {
                        submitterInfo = getUserInfo(submitterId);
                    }
                }
                if (submitterInfo != null)
                {
                    string templateContent = string.Empty;
                    string subjectContent = string.Empty;
                    var templateinfo = db.TEEmailTemplates.Where(a => a.ModuleName == "POReject" && a.IsDeleted == false).FirstOrDefault();
                    if (templateinfo != null)
                    {
                        templateContent = templateinfo.EmailTemplate;
                        subjectContent = templateinfo.Subject;
                    }
                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        templateContent = templateContent.Replace("$Employee", submitterInfo.ApproverName);
                        if (purchaseheaderstructInfo.Purchasing_Order_Number != null) templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Purchasing_Order_Number);
                        else templateContent = templateContent.Replace("$PONumber", purchaseheaderstructInfo.Uniqueid.ToString());
                        templateContent = templateContent.Replace("$VendorName", purchaseheaderstructInfo.Vendor_Code + '-' + purchaseheaderstructInfo.Vendor_Owner);
                        templateContent = templateContent.Replace("$ApproverName", loginApproverInfo.ApproverName);
                    }
                    if (!string.IsNullOrEmpty(subjectContent))
                    {
                        subjectContent = subjectContent.Replace("$ApproverName", loginApproverInfo.ApproverName);
                        if (purchaseheaderstructInfo.Purchasing_Order_Number != null)
                            subjectContent = subjectContent.Replace("$PO", purchaseheaderstructInfo.Purchasing_Order_Number);
                        else
                            subjectContent = subjectContent.Replace("$PO", purchaseheaderstructInfo.Uniqueid.ToString());
                    }
                    string TO = submitterInfo.EmailId;
                    string CC = "";
                    string BCC = "";
                    string body = templateContent;
                    string subject = subjectContent;
                    string[] attachments = new string[0];
                    var mail = new Mailing();
                    var sendingmail = mail.SendEMail(TO, CC, BCC, body, subject, attachments, null);
                }
            }
            catch (Exception ex)
            {
                exceptionObj.RecordUnHandledException(ex);
            }
        }

        public ApproverEmailProfile getUserInfo(int? userId)
        {
            ApproverEmailProfile UserInfo = new ApproverEmailProfile();
            try
            {
                UserInfo = (from user in db.UserProfiles
                                                 where user.UserId == userId && user.IsDeleted == false
                                                 select new ApproverEmailProfile
                                                 {
                                                     ApproverName = user.CallName,
                                                     EmailId = user.email,
                                                     ApproverUserId = user.UserId
                                                 }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                exceptionObj.RecordUnHandledException(ex);
            }
            return UserInfo;
        }        
    }   
    public class ApproverEmailProfile
    {
        public int ApproverUserId { get; set; }
        public string ApproverName { get; set; }
        public string EmailId { get; set; }
    }

}