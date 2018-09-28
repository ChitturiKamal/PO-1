using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PO.Models;
using PO.BAL;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace PO.Controllers
{
    public class PurchaseOrderController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();
        PurchaseOrders POBAL = new PurchaseOrders();
        public PurchaseOrderController()
        {

            db.Configuration.ProxyCreationEnabled = false;
        }
        #region commented 
        //[HttpPost]
        //public HttpResponseMessage TEPurchaseHomeByUser(JObject json)
        //{
        //    int UserId = 0; string Status = string.Empty;
        //    int PageCount = 0; string SortBy = string.Empty;
        //    UserId = json["UserId"].ToObject<int>();
        //    PageCount = json["PageCount"].ToObject<int>();
        //    Status = json["Status"].ToObject<string>();
        //    SortBy = json["SortBy"].ToObject<string>();
        //    List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
        //    try
        //    {
        //        var dte = System.DateTime.Now;

        //        var status = db.TEPOHeaderStructures;
        //        var vendor = db.TEPOVendors;
        //        int PoCount = 0;

        //        //var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();

        //        string user = "";
        //        string username = "";

        //        db.Configuration.ProxyCreationEnabled = true;

        //        UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();

        //        user = profile.CallName;
        //        username = profile.UserName;

        //        string user1 = "";
        //        user1 = "Approver";
        //        //foreach (var item in profile.webpages_Role)
        //        //{
        //        //    if (item.RoleName.Equals("PO  Approval Admin"))
        //        //    {
        //        //        user1 = "PO  Approval Admin";
        //        //        break;
        //        //    }
        //        //}
        //        //user1 = "PO  Approval Admin";

        //        if (user1 == "PO  Approval Admin")
        //        {

        //            var query = (from header in db.TEPOHeaderStructures
        //                         join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
        //                         from mnger in tempmgr.DefaultIfEmpty()
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status == Status && header.IsDeleted == false)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             Managed_by = mnger.CallName
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //            PoCount = query.Count();
        //            query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

        //            if (Status == "All")
        //            {
        //                query = (from header in db.TEPOHeaderStructures
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status ==
        //                         "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected" && header.IsDeleted == false)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //                PoCount = query.Count();
        //                query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //            }
        //            if (Status == "Draft")
        //            {
        //                query = (from header in db.TEPOHeaderStructures
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status == "Pending For Approval" && header.IsDeleted == false)

        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //                PoCount = query.Count();
        //                query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //            }
        //            foreach (var item in query)
        //            {
        //                string releasestatus = "";
        //                DateTime? releasedate = null;
        //                releasestatus = Status;
        //                //  string[] WbsList ={null};
        //                double? TotalPrice = 0.00;
        //                DateTime GSTDate = new DateTime(2017, 07, 03);
        //                DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
        //                List<TEPOItemwise> wiseList = db.TEPOItemwises.Where(i => i.POStructureId == item.Uniqueid
        //                                                                                    && (i.Condition_Type != "NAVS"
        //                                                                                    && i.Condition_Type != "JICG"
        //                                                                                && i.Condition_Type != "JISG"
        //                                                                                && i.Condition_Type != "JICR"
        //                                                                                && i.Condition_Type != "JISR"
        //                                                                                && i.Condition_Type != "JIIR"
        //                                                                                )
        //                                                                                && (i.VendorCode == null
        //                                                                                    || i.VendorCode == ""
        //                                                                                    || i.VendorCode == item.Vendor_Account_Number
        //                                                                                    || postingDate <= GSTDate
        //                                                                                    )
        //                                                                                    && i.IsDeleted == false
        //                                                                                    ).ToList();

        //                if (wiseList.Count > 0)
        //                {
        //                    TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //                }


        //                List<string> WbsList = new List<string>();
        //                List<string> WbsHeadsList = new List<string>();


        //                var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                        ).ToList();


        //                foreach (var IS in ItemStructure)
        //                {
        //                    var WBSElementsList = new List<string>();

        //                    if (IS.Item_Category != "D")
        //                    {
        //                        WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                   && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //                                   .Select(x => x.WBS_Element).ToList();

        //                    }
        //                    else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //                    {
        //                        WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                 && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //                                 .Select(x => x.WBS_Element).ToList();
        //                    }


        //                    foreach (var ele in WBSElementsList)
        //                    {
        //                        //getWbsElement(WbsList, ele, i);



        //                        int occ = ele.Count(x => x == '-');

        //                        if (occ >= 3)
        //                        {
        //                            int index = CustomIndexOf(ele, '-', 3);
        //                            // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //                            string part = ele.Substring(0, index);
        //                            WbsHeadsList.Add(part);
        //                        }
        //                        else
        //                        {
        //                            WbsHeadsList.Add(ele);
        //                        }

        //                        string Element = ele;
        //                        char firstChar = Element[0];
        //                        if (firstChar == '0')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 4));

        //                        }
        //                        else if (firstChar == 'A')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'C')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'M')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "MN")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }
        //                            else if (twoChar == "MC")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 4));

        //                            }
        //                        }
        //                        else if (firstChar == 'Y')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "YS")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }

        //                        }
        //                        else if (firstChar == 'O')
        //                        {
        //                            string threeChar = Element.Substring(0, 3);
        //                            if (threeChar == "OB2")
        //                            {

        //                                WbsList.Add(Element);

        //                            }

        //                        }
        //                    }

        //                }






        //                WbsList = WbsList.Distinct().ToList();
        //                WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //                string WbsHeads = "";
        //                foreach (var w in WbsHeadsList)
        //                {
        //                    if (WbsHeads == "")
        //                    {
        //                        WbsHeads = w;
        //                    }
        //                    else
        //                    {
        //                        WbsHeads = WbsHeads + "," + w;
        //                    }
        //                }

        //                string ProjectCodes = "";
        //                foreach (var w in WbsList)
        //                {
        //                    if (ProjectCodes == "")
        //                    {
        //                        ProjectCodes = w;
        //                    }
        //                    else
        //                    {
        //                        ProjectCodes = ProjectCodes + "," + w;
        //                    }
        //                }

        //                result.Add(new TEPurchaseHomeModel()
        //                {
        //                    Purchasing_Order_Number = item.Purchasing_Order_Number,
        //                    //Vendor_Account_Number = item.Vendor_Account_Number,
        //                    // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
        //                    Vendor_Account_Number = item.Vendor_Account_Number,
        //                    Purchasing_Document_Date = item.Purchasing_Document_Date,
        //                    //FundCenter_Description = item.FundCenter_Description,
        //                    Path = item.path,
        //                    ReleaseCodeStatus = releasestatus,
        //                    Purchasing_Release_Date = releasedate,
        //                    Amount
        //                    = TotalPrice,
        //                    Currency_Key = item.Currency_Key,
        //                    HeaderUniqueid = item.Uniqueid,
        //                    PoCount = PoCount,
        //                    PoTitle = item.PO_Title,
        //                    VendorName = (vendor
        //                    .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
        //                        .Select(x => x.Vendor_Owner).FirstOrDefault()),
        //                    Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //                    POStatus = item.ReleaseCode2Status,
        //                    ProjectCodes = ProjectCodes,
        //                    SubmitterName = item.SubmitterName,
        //                    WbsHeads = WbsHeads,
        //                    ManagerName = item.Managed_by


        //                });
        //            }
        //        }
        //        else
        //        {
        //            var query = (from header in db.TEPOHeaderStructures
        //                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
        //                         //join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
        //                         // join fund  in db.TEPOFundCenters on assign.Fund_Center equals fund.FundCenter_Code                             
        //                         //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

        //                         //join vend in db.TEPOVendors on header.Vendor_Account_Number equals vend.Vendor_Code
        //                         where (appr.ApproverId == UserId && appr.Status == Status && appr.IsDeleted == false && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             // vend.Vendor_Owner,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             // fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by

        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

        //            PoCount = query.Count();
        //            query = query.Skip((PageCount - 1) * 10).Take(10).ToList();


        //            if (Status == "All")
        //            {


        //                query = (from header in db.TEPOHeaderStructures
        //                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

        //                         where (appr.ApproverId == UserId && appr.IsDeleted == false && (appr.Status == "Pending For Approval" || appr.Status == "Draft" || appr.Status == "Approved" || appr.Status == "Rejected") && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             // vend.Vendor_Owner,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             // fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by

        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

        //                PoCount = query.Count();
        //                query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

        //            }


        //            foreach (var item in query)
        //            {
        //                string releasestatus = "";
        //                DateTime? releasedate = null;
        //                releasestatus = Status;

        //                double? TotalPrice = 0.00;
        //                DateTime GSTDate = new DateTime(2017, 07, 03);
        //                DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
        //                List<TEPOItemwise> wiseList = db.TEPOItemwises.Where(i => i.POStructureId == item.Uniqueid
        //                                                                                    && (i.Condition_Type != "NAVS"
        //                                                                                    && i.Condition_Type != "JICG"
        //                                                                                && i.Condition_Type != "JISG"
        //                                                                                && i.Condition_Type != "JICR"
        //                                                                                && i.Condition_Type != "JISR"
        //                                                                                && i.Condition_Type != "JIIR"
        //                                                                                )
        //                                                                                && (i.VendorCode == null
        //                                                                                    || i.VendorCode == ""
        //                                                                                    || i.VendorCode == item.Vendor_Account_Number
        //                                                                                    || postingDate <= GSTDate
        //                                                                                    )
        //                                                                                    && i.IsDeleted == false).ToList();

        //                if (wiseList.Count > 0)
        //                {
        //                    TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //                }




        //                List<string> WbsList = new List<string>();
        //                List<string> WbsHeadsList = new List<string>();

        //                var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                        ).ToList();


        //                foreach (var IS in ItemStructure)
        //                {
        //                    var WBSElementsList = new List<string>();

        //                    if (IS.Item_Category != "D")
        //                    {
        //                        WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                   && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //                                   .Select(x => x.WBS_Element).ToList();

        //                    }
        //                    else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //                    {
        //                        WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                 && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //                                 .Select(x => x.WBS_Element).ToList();
        //                    }


        //                    foreach (var ele in WBSElementsList)
        //                    {
        //                        //getWbsElement(WbsList, ele, i);



        //                        int occ = ele.Count(x => x == '-');

        //                        if (occ >= 3)
        //                        {
        //                            int index = CustomIndexOf(ele, '-', 3);
        //                            // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //                            string part = ele.Substring(0, index);
        //                            WbsHeadsList.Add(part);
        //                        }
        //                        else
        //                        {
        //                            WbsHeadsList.Add(ele);
        //                        }

        //                        string Element = ele;
        //                        char firstChar = Element[0];
        //                        if (firstChar == '0')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 4));

        //                        }
        //                        else if (firstChar == 'A')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'C')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'M')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "MN")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }
        //                            else if (twoChar == "MC")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 4));

        //                            }
        //                        }
        //                        else if (firstChar == 'Y')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "YS")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }

        //                        }
        //                        else if (firstChar == 'O')
        //                        {
        //                            string threeChar = Element.Substring(0, 3);
        //                            if (threeChar == "OB2")
        //                            {

        //                                WbsList.Add(Element);

        //                            }

        //                        }
        //                    }

        //                }


        //                WbsList = WbsList.Distinct().ToList();
        //                WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //                string WbsHeads = "";
        //                foreach (var w in WbsHeadsList)
        //                {
        //                    if (WbsHeads == "")
        //                    {
        //                        WbsHeads = w;
        //                    }
        //                    else
        //                    {
        //                        WbsHeads = WbsHeads + "," + w;
        //                    }
        //                }
        //                string ProjectCodes = "";
        //                foreach (var w in WbsList)
        //                {
        //                    if (ProjectCodes == "")
        //                    {
        //                        ProjectCodes = w;
        //                    }
        //                    else
        //                    {
        //                        ProjectCodes = ProjectCodes + "," + w;
        //                    }
        //                }



        //                result.Add(new TEPurchaseHomeModel()
        //                {
        //                    Purchasing_Order_Number = item.Purchasing_Order_Number,
        //                    Vendor_Account_Number = item.Vendor_Account_Number,
        //                    Purchasing_Document_Date = item.Purchasing_Document_Date,
        //                    // FundCenter_Description = item.FundCenter_Description,
        //                    Path = item.path,
        //                    ReleaseCodeStatus = releasestatus,
        //                    Purchasing_Release_Date = releasedate,
        //                    Amount
        //                    = TotalPrice,
        //                    Currency_Key = item.Currency_Key,
        //                    HeaderUniqueid = item.Uniqueid,
        //                    PoCount = PoCount,
        //                    PoTitle = item.PO_Title,
        //                    VendorName = (vendor
        //                    .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
        //                        .Select(x => x.Vendor_Owner).FirstOrDefault()),
        //                    Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //                    POStatus = item.ReleaseCode2Status,
        //                    ProjectCodes = ProjectCodes,
        //                    SubmitterName = item.SubmitterName,
        //                    WbsHeads = WbsHeads,
        //                    ManagerName = item.Managed_by

        //                });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        db.ApplicationErrorLogs.Add(
        //            new ApplicationErrorLog
        //            {
        //                Error = ex.Message,
        //                ExceptionDateTime = System.DateTime.Now,
        //                InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
        //                Source = "From TEPurchaseHome API | TEPurchaseHome | " + this.GetType().ToString(),
        //                Stacktrace = ex.StackTrace
        //            }
        //            );
        //    }

        //    //db.SaveChanges();

        //    if (SortBy == "PoDate")
        //        result = result.OrderBy(x => x.Purchasing_Document_Date).ToList();

        //    else if (SortBy == "VendorName")
        //        result = result.OrderBy(x => x.VendorName).ToList();
        //    //else if (SortBy == "PoNumber")
        //    //    return result.OrderBy(x => x.Purchasing_Order_Number).ToList();
        //    else
        //        result = result.OrderBy(x => x.HeaderUniqueid).ToList();

        //    return new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = new JsonContent(new
        //        {

        //            res = result

        //        })
        //    };
        //}
        //[HttpPost]
        //public HttpResponseMessage TEPurchaseHomeByUser_Pagination(JObject json)
        //{
        //    int UserId = 0; string Status = string.Empty;
        //    //int PageCount = 0;
        //    string SortBy = string.Empty;
        //    int pagenumber = 0, pagepercount = 0;
        //    UserId = json["UserId"].ToObject<int>();
        //    //PageCount = json["PageCount"].ToObject<int>();
        //    Status = json["Status"].ToObject<string>();
        //    SortBy = json["SortBy"].ToObject<string>();
        //    if (json["pageNumber"] != null)
        //        pagenumber = json["pageNumber"].ToObject<int>();
        //    if (json["pagePerCount"] != null)
        //        pagepercount = json["pagePerCount"].ToObject<int>();
        //    HttpResponseMessage hrm = new HttpResponseMessage();
        //    FailInfo finfo = new FailInfo();
        //    SuccessInfo sinfo = new SuccessInfo();
        //    List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
        //    try
        //    {
        //        var dte = System.DateTime.Now;

        //        var status = db.TEPOHeaderStructures;
        //        var vendor = db.TEPOVendors;
        //        int PoCount = 0;

        //        //var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();

        //        string user = "";
        //        string username = "";

        //        db.Configuration.ProxyCreationEnabled = true;

        //        UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();

        //        user = profile.CallName;
        //        username = profile.UserName;

        //        string user1 = "";
        //        user1 = "Approver";
        //        //foreach (var item in profile.webpages_Roles)
        //        //{
        //        //    if (item.RoleName.Equals("PO  Approval Admin"))
        //        //    {
        //        //        user1 = "PO  Approval Admin";
        //        //        break;
        //        //    }
        //        //}
        //        //user1 = "PO  Approval Admin";

        //        if (user1 == "PO  Approval Admin")
        //        {

        //            var query = (from header in db.TEPOHeaderStructures
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status == Status && header.IsDeleted == false)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //            //PoCount = query.Count();
        //            //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

        //            if (Status == "All")
        //            {
        //                query = (from header in db.TEPOHeaderStructures
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status ==
        //                         "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected" && header.IsDeleted == false)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //                //PoCount = query.Count();
        //                //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //            }
        //            if (Status == "Draft")
        //            {
        //                query = (from header in db.TEPOHeaderStructures
        //                             // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         where (header.ReleaseCode2Status == "Pending For Approval" && header.IsDeleted == false)

        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             //fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             // v.Vendor_Owner
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by
        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
        //                //PoCount = query.Count();
        //                //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //            }
        //            PoCount = query.Count;
        //            var finalResult = query;
        //            if (PoCount > 0)
        //            {
        //                if (pagenumber >= 0 && pagepercount > 0)
        //                {
        //                    if (pagenumber == 0)
        //                    {
        //                        pagenumber = 1;
        //                    }
        //                    int iPageNum = pagenumber;
        //                    int iPageSize = pagepercount;
        //                    int start = iPageNum - 1;
        //                    start = start * iPageSize;
        //                    finalResult = query.Skip(start).Take(iPageSize).ToList();
        //                    sinfo.errorcode = 0;
        //                    sinfo.errormessage = "Success";
        //                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
        //                    sinfo.torecords = finalResult.Count + start;
        //                    sinfo.totalrecords = PoCount;
        //                    sinfo.listcount = finalResult.Count;
        //                    sinfo.pages = pagenumber.ToString();
        //                }
        //                else
        //                {
        //                    sinfo.errorcode = 0;
        //                    sinfo.errormessage = "Success";
        //                    sinfo.fromrecords = 1;
        //                    sinfo.torecords = 10;
        //                    sinfo.totalrecords = 0;
        //                    sinfo.listcount = 0;
        //                    sinfo.pages = "0";

        //                    hrm.Content = new JsonContent(new
        //                    {
        //                        result = query, //error
        //                        info = sinfo //return exception
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                sinfo.errorcode = 0;
        //                sinfo.errormessage = "No Records Found";
        //                sinfo.fromrecords = 1;
        //                sinfo.torecords = 10;
        //                sinfo.totalrecords = 0;
        //                sinfo.listcount = 0;
        //                sinfo.pages = "0";

        //                hrm.Content = new JsonContent(new
        //                {
        //                    info = sinfo //return exception
        //                });
        //            }
        //            foreach (var item in finalResult)
        //            {
        //                string releasestatus = "";
        //                DateTime? releasedate = null;
        //                releasestatus = Status;
        //                //  string[] WbsList ={null};
        //                double? TotalPrice = 0.00;
        //                DateTime GSTDate = new DateTime(2017, 07, 03);
        //                DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
        //                List<TEPOItemwise> wiseList = db.TEPOItemwises.Where(i => i.POStructureId == item.Uniqueid
        //                                                                                    && (i.Condition_Type != "NAVS"
        //                                                                                    && i.Condition_Type != "JICG"
        //                                                                                && i.Condition_Type != "JISG"
        //                                                                                && i.Condition_Type != "JICR"
        //                                                                                && i.Condition_Type != "JISR"
        //                                                                                && i.Condition_Type != "JIIR"
        //                                                                                )
        //                                                                                && (i.VendorCode == null
        //                                                                                    || i.VendorCode == ""
        //                                                                                    || i.VendorCode == item.Vendor_Account_Number
        //                                                                                    || postingDate <= GSTDate
        //                                                                                    )
        //                                                                                    && i.IsDeleted == false
        //                                                                                    ).ToList();

        //                if (wiseList.Count > 0)
        //                {
        //                    TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //                }


        //                List<string> WbsList = new List<string>();
        //                List<string> WbsHeadsList = new List<string>();


        //                var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                        ).ToList();


        //                foreach (var IS in ItemStructure)
        //                {
        //                    var WBSElementsList = new List<string>();

        //                    if (IS.Item_Category != "D")
        //                    {
        //                        WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                   && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //                                   .Select(x => x.WBS_Element).ToList();

        //                    }
        //                    else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //                    {
        //                        WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                 && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //                                 .Select(x => x.WBS_Element).ToList();
        //                    }


        //                    foreach (var ele in WBSElementsList)
        //                    {
        //                        //getWbsElement(WbsList, ele, i);



        //                        int occ = ele.Count(x => x == '-');

        //                        if (occ >= 3)
        //                        {
        //                            int index = CustomIndexOf(ele, '-', 3);
        //                            // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //                            string part = ele.Substring(0, index);
        //                            WbsHeadsList.Add(part);
        //                        }
        //                        else
        //                        {
        //                            WbsHeadsList.Add(ele);
        //                        }

        //                        string Element = ele;
        //                        char firstChar = Element[0];
        //                        if (firstChar == '0')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 4));

        //                        }
        //                        else if (firstChar == 'A')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'C')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'M')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "MN")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }
        //                            else if (twoChar == "MC")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 4));

        //                            }
        //                        }
        //                        else if (firstChar == 'Y')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "YS")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }

        //                        }
        //                        else if (firstChar == 'O')
        //                        {
        //                            string threeChar = Element.Substring(0, 3);
        //                            if (threeChar == "OB2")
        //                            {

        //                                WbsList.Add(Element);

        //                            }

        //                        }
        //                    }

        //                }






        //                WbsList = WbsList.Distinct().ToList();
        //                WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //                string WbsHeads = "";
        //                foreach (var w in WbsHeadsList)
        //                {
        //                    if (WbsHeads == "")
        //                    {
        //                        WbsHeads = w;
        //                    }
        //                    else
        //                    {
        //                        WbsHeads = WbsHeads + "," + w;
        //                    }
        //                }

        //                string ProjectCodes = "";
        //                foreach (var w in WbsList)
        //                {
        //                    if (ProjectCodes == "")
        //                    {
        //                        ProjectCodes = w;
        //                    }
        //                    else
        //                    {
        //                        ProjectCodes = ProjectCodes + "," + w;
        //                    }
        //                }

        //                result.Add(new TEPurchaseHomeModel()
        //                {
        //                    Purchasing_Order_Number = item.Purchasing_Order_Number,
        //                    //Vendor_Account_Number = item.Vendor_Account_Number,
        //                    // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
        //                    Vendor_Account_Number = item.Vendor_Account_Number,
        //                    Purchasing_Document_Date = item.Purchasing_Document_Date,
        //                    //FundCenter_Description = item.FundCenter_Description,
        //                    Path = item.path,
        //                    ReleaseCodeStatus = releasestatus,
        //                    Purchasing_Release_Date = releasedate,
        //                    Amount
        //                    = TotalPrice,
        //                    Currency_Key = item.Currency_Key,
        //                    HeaderUniqueid = item.Uniqueid,
        //                    PoCount = PoCount,
        //                    PoTitle = item.PO_Title,
        //                    VendorName = (vendor
        //                    .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
        //                        .Select(x => x.Vendor_Owner).FirstOrDefault()),
        //                    Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //                    POStatus = item.ReleaseCode2Status,
        //                    ProjectCodes = ProjectCodes,
        //                    SubmitterName = item.SubmitterName,
        //                    WbsHeads = WbsHeads,
        //                    ManagerName = item.Managed_by


        //                });
        //            }
        //        }
        //        else
        //        {
        //            var query = (from header in db.TEPOHeaderStructures
        //                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
        //                         //join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //                         //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
        //                         // join fund  in db.TEPOFundCenters on assign.Fund_Center equals fund.FundCenter_Code                             
        //                         //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

        //                         //join vend in db.TEPOVendors on header.Vendor_Account_Number equals vend.Vendor_Code
        //                         where (appr.ApproverId == UserId && appr.Status == Status && appr.IsDeleted == false && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             // vend.Vendor_Owner,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             // fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by

        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

        //            //PoCount = query.Count();
        //            //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();


        //            if (Status == "All")
        //            {


        //                query = (from header in db.TEPOHeaderStructures
        //                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

        //                         where (appr.ApproverId == UserId && appr.IsDeleted == false && (appr.Status == "Pending For Approval" || appr.Status == "Draft" || appr.Status == "Approved" || appr.Status == "Rejected") && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
        //                         orderby header.Uniqueid descending
        //                         select new
        //                         {
        //                             header.Purchasing_Order_Number,
        //                             header.Vendor_Account_Number,
        //                             // vend.Vendor_Owner,
        //                             header.Purchasing_Document_Date,
        //                             header.path,
        //                             header.ReleaseCode2,
        //                             header.ReleaseCode2Status,
        //                             header.ReleaseCode2Date,
        //                             header.ReleaseCode3,
        //                             header.ReleaseCode3Status,
        //                             header.ReleaseCode3Date,
        //                             header.ReleaseCode4,
        //                             header.ReleaseCode4Status,
        //                             header.ReleaseCode4Date,
        //                             // fund.FundCenter_Description,
        //                             header.Currency_Key,
        //                             //appr.UniqueId,
        //                             //appr.SequenceNumber,
        //                             //appr.ReleaseCode,
        //                             header.Uniqueid,
        //                             header.PO_Title,
        //                             header.SubmitterName,
        //                             header.Managed_by

        //                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

        //                //PoCount = query.Count();
        //                //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

        //            }
        //            PoCount = query.Count;
        //            var finalResult = query;
        //            if (PoCount > 0)
        //            {
        //                if (pagenumber >= 0 && pagepercount > 0)
        //                {
        //                    if (pagenumber == 0)
        //                    {
        //                        pagenumber = 1;
        //                    }
        //                    int iPageNum = pagenumber;
        //                    int iPageSize = pagepercount;
        //                    int start = iPageNum - 1;
        //                    start = start * iPageSize;
        //                    finalResult = query.Skip(start).Take(iPageSize).ToList();
        //                    sinfo.errorcode = 0;
        //                    sinfo.errormessage = "Success";
        //                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
        //                    sinfo.torecords = finalResult.Count + start;
        //                    sinfo.totalrecords = PoCount;
        //                    sinfo.listcount = finalResult.Count;
        //                    sinfo.pages = pagenumber.ToString();
        //                }
        //                else
        //                {
        //                    sinfo.errorcode = 0;
        //                    sinfo.errormessage = "Success";
        //                    sinfo.fromrecords = 1;
        //                    sinfo.torecords = 10;
        //                    sinfo.totalrecords = 0;
        //                    sinfo.listcount = 0;
        //                    sinfo.pages = "0";

        //                    hrm.Content = new JsonContent(new
        //                    {
        //                        result = query, //error
        //                        info = sinfo //return exception
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                sinfo.errorcode = 0;
        //                sinfo.errormessage = "No Records Found";
        //                sinfo.fromrecords = 1;
        //                sinfo.torecords = 10;
        //                sinfo.totalrecords = 0;
        //                sinfo.listcount = 0;
        //                sinfo.pages = "0";

        //                hrm.Content = new JsonContent(new
        //                {
        //                    info = sinfo //return exception
        //                });
        //            }

        //            foreach (var item in finalResult)
        //            {
        //                string releasestatus = "";
        //                DateTime? releasedate = null;
        //                releasestatus = Status;

        //                double? TotalPrice = 0.00;
        //                DateTime GSTDate = new DateTime(2017, 07, 03);
        //                DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
        //                List<TEPOItemwise> wiseList = db.TEPOItemwises.Where(i => i.POStructureId == item.Uniqueid
        //                                                                                    && (i.Condition_Type != "NAVS"
        //                                                                                    && i.Condition_Type != "JICG"
        //                                                                                && i.Condition_Type != "JISG"
        //                                                                                && i.Condition_Type != "JICR"
        //                                                                                && i.Condition_Type != "JISR"
        //                                                                                && i.Condition_Type != "JIIR"
        //                                                                                )
        //                                                                                && (i.VendorCode == null
        //                                                                                    || i.VendorCode == ""
        //                                                                                    || i.VendorCode == item.Vendor_Account_Number
        //                                                                                    || postingDate <= GSTDate
        //                                                                                    )
        //                                                                                    && i.IsDeleted == false).ToList();

        //                if (wiseList.Count > 0)
        //                {
        //                    TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //                }




        //                List<string> WbsList = new List<string>();
        //                List<string> WbsHeadsList = new List<string>();

        //                var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                        ).ToList();


        //                foreach (var IS in ItemStructure)
        //                {
        //                    var WBSElementsList = new List<string>();

        //                    if (IS.Item_Category != "D")
        //                    {
        //                        WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                   && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //                                   .Select(x => x.WBS_Element).ToList();

        //                    }
        //                    else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //                    {
        //                        WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //                                                 && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //                                 .Select(x => x.WBS_Element).ToList();
        //                    }


        //                    foreach (var ele in WBSElementsList)
        //                    {
        //                        //getWbsElement(WbsList, ele, i);



        //                        int occ = ele.Count(x => x == '-');

        //                        if (occ >= 3)
        //                        {
        //                            int index = CustomIndexOf(ele, '-', 3);
        //                            // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //                            string part = ele.Substring(0, index);
        //                            WbsHeadsList.Add(part);
        //                        }
        //                        else
        //                        {
        //                            WbsHeadsList.Add(ele);
        //                        }

        //                        string Element = ele;
        //                        char firstChar = Element[0];
        //                        if (firstChar == '0')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 4));

        //                        }
        //                        else if (firstChar == 'A')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'C')
        //                        {

        //                            WbsList.Add(Element.Substring(0, 5));

        //                        }
        //                        else if (firstChar == 'M')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "MN")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }
        //                            else if (twoChar == "MC")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 4));

        //                            }
        //                        }
        //                        else if (firstChar == 'Y')
        //                        {
        //                            string twoChar = Element.Substring(0, 2);
        //                            if (twoChar == "YS")
        //                            {

        //                                WbsList.Add(Element.Substring(0, 7));

        //                            }

        //                        }
        //                        else if (firstChar == 'O')
        //                        {
        //                            string threeChar = Element.Substring(0, 3);
        //                            if (threeChar == "OB2")
        //                            {

        //                                WbsList.Add(Element);

        //                            }

        //                        }
        //                    }

        //                }


        //                WbsList = WbsList.Distinct().ToList();
        //                WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //                string WbsHeads = "";
        //                foreach (var w in WbsHeadsList)
        //                {
        //                    if (WbsHeads == "")
        //                    {
        //                        WbsHeads = w;
        //                    }
        //                    else
        //                    {
        //                        WbsHeads = WbsHeads + "," + w;
        //                    }
        //                }
        //                string ProjectCodes = "";
        //                foreach (var w in WbsList)
        //                {
        //                    if (ProjectCodes == "")
        //                    {
        //                        ProjectCodes = w;
        //                    }
        //                    else
        //                    {
        //                        ProjectCodes = ProjectCodes + "," + w;
        //                    }
        //                }

        //                result.Add(new TEPurchaseHomeModel()
        //                {
        //                    Purchasing_Order_Number = item.Purchasing_Order_Number,
        //                    Vendor_Account_Number = item.Vendor_Account_Number,
        //                    Purchasing_Document_Date = item.Purchasing_Document_Date,
        //                    // FundCenter_Description = item.FundCenter_Description,
        //                    Path = item.path,
        //                    ReleaseCodeStatus = releasestatus,
        //                    Purchasing_Release_Date = releasedate,
        //                    Amount
        //                    = TotalPrice,
        //                    Currency_Key = item.Currency_Key,
        //                    HeaderUniqueid = item.Uniqueid,
        //                    PoCount = PoCount,
        //                    PoTitle = item.PO_Title,
        //                    VendorName = (vendor
        //                    .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
        //                        .Select(x => x.Vendor_Owner).FirstOrDefault()),
        //                    Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //                    POStatus = item.ReleaseCode2Status,
        //                    ProjectCodes = ProjectCodes,
        //                    SubmitterName = item.SubmitterName,
        //                    WbsHeads = WbsHeads,
        //                    ManagerName = item.Managed_by

        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        db.ApplicationErrorLogs.Add(
        //            new ApplicationErrorLog
        //            {
        //                Error = ex.Message,
        //                ExceptionDateTime = System.DateTime.Now,
        //                InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
        //                Source = "From TEPurchaseHome API | TEPurchaseHome | " + this.GetType().ToString(),
        //                Stacktrace = ex.StackTrace
        //            }
        //            );
        //    }

        //    //db.SaveChanges();

        //    //if (SortBy == "PoDate")
        //    //    result = result.OrderBy(x => x.Purchasing_Document_Date).ToList();

        //    //else if (SortBy == "VendorName")
        //    //    result = result.OrderBy(x => x.VendorName).ToList();
        //    ////else if (SortBy == "PoNumber")
        //    ////    return result.OrderBy(x => x.Purchasing_Order_Number).ToList();
        //    //else
        //    //    result = result.OrderByDescending(x => x.Purchasing_Document_Date).ToList();

        //    if (result.Count > 0)
        //    {
        //        hrm.Content = new JsonContent(new
        //        {
        //            result = result,
        //            info = sinfo
        //        });
        //    }
        //    else
        //    {
        //        finfo.errorcode = 0;
        //        finfo.errormessage = "No Records";
        //        finfo.listcount = 0;
        //        hrm.Content = new JsonContent(new
        //        {
        //            result = result,
        //            info = finfo
        //        });
        //    }
        //    return hrm;
        //}
        //[HttpPost]
        //public HttpResponseMessage TEPurchaseSearch_Pagination_backup(JObject json)
        //{
        //    int UserId = 0; string Status = string.Empty;
        //    string SearchKey = string.Empty;
        //    int pagenumber = 0;
        //    //pagepercount = 0;
        //    if (json["UserId"] != null)
        //        UserId = json["UserId"].ToObject<int>();
        //    if (json["Search"] != null)
        //        SearchKey = json["Search"].ToObject<string>();
        //    //if (json["pageNumber"] != null)
        //    //    pagenumber = json["pageNumber"].ToObject<int>();
        //    //if (json["pagePerCount"] != null)
        //    //    pagepercount = json["pagePerCount"].ToObject<int>();
        //    int PoCount = 0;
        //    HttpResponseMessage hrm = new HttpResponseMessage();
        //    FailInfo finfo = new FailInfo();
        //    SuccessInfo sinfo = new SuccessInfo();
        //    List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
        //    try
        //    {
        //        #region old PO code
        //        //var dte = System.DateTime.Now;

        //        //var status = db.TEPurchase_header_structure;
        //        //var vendor = db.TEPurchase_Vendor;
        //        //int PoCount = 0;

        //        ////var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();

        //        //string user = "";
        //        //string username = "";

        //        //db.Configuration.ProxyCreationEnabled = true;

        //        ////UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();

        //        ////user = profile.CallName;
        //        ////username = profile.UserName;

        //        ////string user1 = "";
        //        ////user1 = "Approver";
        //        ////foreach (var item in profile.webpages_Roles)
        //        ////{
        //        ////    if (item.RoleName.Equals("PO  Approval Admin"))
        //        ////    {
        //        ////        user1 = "PO  Approval Admin";
        //        ////        break;
        //        ////    }
        //        ////}
        //        ////user1 = "PO  Approval Admin";
        //        //var query = (from header in db.TEPOHeaderStructures
        //        //             join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g                            
        //        //             from lftitem in g.DefaultIfEmpty()
        //        //             //join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
        //        //             //from lftitem in g.DefaultIfEmpty()
        //        //                 // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
        //        //             where (header.IsDeleted == false 
        //        //             &&
        //        //             (
        //        //             lftitem.Short_Text.Contains(SearchKey)||
        //        //             lftitem.Long_Text.Contains(SearchKey) ||
        //        //             header.Purchasing_Order_Number.Contains(SearchKey) ||
        //        //             header.Vendor_Account_Number.Contains(SearchKey) ||
        //        //             header.PO_Title.Contains(SearchKey) ||
        //        //             header.SubmitterName.Contains(SearchKey) ||
        //        //             header.Managed_by.Contains(SearchKey)
        //        //            )
        //        //             )
        //        //             orderby header.Uniqueid descending
        //        //             select new
        //        //             {
        //        //                 header.Purchasing_Order_Number,
        //        //                 header.Vendor_Account_Number,
        //        //                 header.Purchasing_Document_Date,
        //        //                 header.path,
        //        //                 header.ReleaseCode2,
        //        //                 header.ReleaseCode2Status,
        //        //                 header.ReleaseCode2Date,                                
        //        //                 //fund.FundCenter_Description,
        //        //                 header.Currency_Key,
        //        //                 //appr.UniqueId,
        //        //                 //appr.SequenceNumber,
        //        //                 //appr.ReleaseCode,
        //        //                 // v.Vendor_Owner
        //        //                 header.Uniqueid,
        //        //                 header.PO_Title,
        //        //                 header.SubmitterName,
        //        //                 header.Managed_by
        //        //             }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();       
        //        #endregion
        //        var query = (from header in db.TEPOHeaderStructures
        //                     join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
        //                     from lftitem in g.DefaultIfEmpty()
        //                     join fund  in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
        //                     join v in db.TEPOVendors on header.VendorID equals v.Uniqueid
        //                     join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
        //                     from proj in tempprj.DefaultIfEmpty()
        //                         //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
        //                     where (header.IsDeleted == false && header.IsNewPO == true
        //                     &&
        //                     (
        //                     lftitem.Material_Number.Contains(SearchKey) ||
        //                     lftitem.Short_Text.Contains(SearchKey) ||
        //                     lftitem.Long_Text.Contains(SearchKey) ||
        //                     header.Purchasing_Order_Number.Contains(SearchKey) ||
        //                     header.Vendor_Account_Number.Contains(SearchKey) ||
        //                     header.PO_Title.Contains(SearchKey) ||
        //                     header.SubmitterName.Contains(SearchKey) ||
        //                     header.Managed_by.Contains(SearchKey) ||
        //                     proj.ProjectCode.Contains(SearchKey) ||
        //                     fund.FundCenter_Description.Contains(SearchKey) ||
        //                     v.Vendor_Owner.Contains(SearchKey)
        //                    )
        //                     )
        //                     orderby header.Uniqueid descending
        //                     select new
        //                     {
        //                         header.Purchasing_Order_Number,
        //                         header.Vendor_Account_Number,
        //                         header.Purchasing_Document_Date,
        //                         header.path,
        //                         header.ReleaseCode2Status,
        //                         header.ReleaseCode2Date,
        //                         header.Version,
        //                         fund.FundCenter_Description,
        //                         header.Currency_Key,
        //                         v.Vendor_Owner,
        //                         header.Uniqueid,
        //                         header.PO_Title,
        //                         header.SubmitterName,
        //                         header.Managed_by,
        //                         proj.ProjectCode,
        //                         proj.ProjectName,
        //                         proj.ProjectShortName,
        //                         header.Fugue_Purchasing_Order_Number
        //                     }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList()
        //                     .GroupBy(a=>a.Fugue_Purchasing_Order_Number).Select(b=>b.OrderByDescending(y => y.Version).First()).ToList();
        //        PoCount = query.Count;
        //        var finalResult = query;
        //        if (PoCount > 0)
        //        {
        //            //if (pagenumber >= 0 && pagepercount > 0)
        //            //{
        //            //    if (pagenumber == 0)
        //            //    {
        //            //        pagenumber = 1;
        //            //    }
        //            //    int iPageNum = pagenumber;
        //            //    int iPageSize = pagepercount;
        //            //    int start = iPageNum - 1;
        //            //    start = start * iPageSize;
        //            //    finalResult = finalResult.Skip(start).Take(iPageSize).ToList();
        //            //    sinfo.errorcode = 0;
        //            //    sinfo.errormessage = "Success";
        //            //    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
        //            //    sinfo.torecords = finalResult.Count + start;
        //            //    sinfo.totalrecords = PoCount;
        //            //    sinfo.listcount = finalResult.Count;
        //            //    sinfo.pages = pagenumber.ToString();
        //            //}
        //            //else
        //            //{
        //            //    sinfo.errorcode = 0;
        //            //    sinfo.errormessage = "Success";
        //            //    sinfo.fromrecords = 1;
        //            //    sinfo.torecords = 10;
        //            //    sinfo.totalrecords = 0;
        //            //    sinfo.listcount = 0;
        //            //    sinfo.pages = "0";

        //            //    hrm.Content = new JsonContent(new
        //            //    {
        //            //        result = query, //error
        //            //        info = sinfo //return exception
        //            //    });
        //            //}
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Success";
        //            sinfo.fromrecords = 1;
        //            sinfo.torecords = finalResult.Count;
        //            sinfo.totalrecords = PoCount;
        //            sinfo.listcount = finalResult.Count;
        //            sinfo.pages = pagenumber.ToString();
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "No Records Found";
        //            sinfo.fromrecords = 1;
        //            sinfo.torecords = 10;
        //            sinfo.totalrecords = 0;
        //            sinfo.listcount = 0;
        //            sinfo.pages = "0";

        //            hrm.Content = new JsonContent(new
        //            {
        //                info = sinfo //return exception
        //            });
        //        }
        //        #region
        //        //foreach (var item in finalResult)
        //        //{
        //        //    string releasestatus = "";
        //        //    DateTime? releasedate = null;
        //        //    releasestatus = Status;
        //        //    //  string[] WbsList ={null};
        //        //    double? TotalPrice = 0.00;
        //        //    DateTime GSTDate = new DateTime(2017, 07, 03);
        //        //    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
        //        //    List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
        //        //                                                                        && (i.Condition_Type != "NAVS"
        //        //                                                                        && i.Condition_Type != "JICG"
        //        //                                                                    && i.Condition_Type != "JISG"
        //        //                                                                    && i.Condition_Type != "JICR"
        //        //                                                                    && i.Condition_Type != "JISR"
        //        //                                                                    && i.Condition_Type != "JIIR"
        //        //                                                                    )
        //        //                                                                    && (i.VendorCode == null
        //        //                                                                        || i.VendorCode == ""
        //        //                                                                        || i.VendorCode == item.Vendor_Account_Number
        //        //                                                                        || postingDate <= GSTDate
        //        //                                                                        )
        //        //                                                                        && i.IsDeleted == false
        //        //                                                                        ).ToList();

        //        //    if (wiseList.Count > 0)
        //        //    {
        //        //        TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //        //    }


        //        //    List<string> WbsList = new List<string>();
        //        //    List<string> WbsHeadsList = new List<string>();


        //        //    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //        //                                            ).ToList();


        //        //    foreach (var IS in ItemStructure)
        //        //    {
        //        //        var WBSElementsList = new List<string>();

        //        //        if (IS.Item_Category != "D")
        //        //        {
        //        //            WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //        //                                       && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //        //                       .Select(x => x.WBS_Element).ToList();

        //        //        }
        //        //        else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //        //        {
        //        //            WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //        //                                     && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //        //                     .Select(x => x.WBS_Element).ToList();
        //        //        }


        //        //        foreach (var ele in WBSElementsList)
        //        //        {
        //        //            //getWbsElement(WbsList, ele, i);



        //        //            int occ = ele.Count(x => x == '-');

        //        //            if (occ >= 3)
        //        //            {
        //        //                int index = CustomIndexOf(ele, '-', 3);
        //        //                // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //        //                string part = ele.Substring(0, index);
        //        //                WbsHeadsList.Add(part);
        //        //            }
        //        //            else
        //        //            {
        //        //                WbsHeadsList.Add(ele);
        //        //            }

        //        //            string Element = ele;
        //        //            char firstChar = Element[0];
        //        //            if (firstChar == '0')
        //        //            {

        //        //                WbsList.Add(Element.Substring(0, 4));

        //        //            }
        //        //            else if (firstChar == 'A')
        //        //            {

        //        //                WbsList.Add(Element.Substring(0, 5));

        //        //            }
        //        //            else if (firstChar == 'C')
        //        //            {

        //        //                WbsList.Add(Element.Substring(0, 5));

        //        //            }
        //        //            else if (firstChar == 'M')
        //        //            {
        //        //                string twoChar = Element.Substring(0, 2);
        //        //                if (twoChar == "MN")
        //        //                {

        //        //                    WbsList.Add(Element.Substring(0, 7));

        //        //                }
        //        //                else if (twoChar == "MC")
        //        //                {

        //        //                    WbsList.Add(Element.Substring(0, 4));

        //        //                }
        //        //            }
        //        //            else if (firstChar == 'Y')
        //        //            {
        //        //                string twoChar = Element.Substring(0, 2);
        //        //                if (twoChar == "YS")
        //        //                {

        //        //                    WbsList.Add(Element.Substring(0, 7));

        //        //                }

        //        //            }
        //        //            else if (firstChar == 'O')
        //        //            {
        //        //                string threeChar = Element.Substring(0, 3);
        //        //                if (threeChar == "OB2")
        //        //                {

        //        //                    WbsList.Add(Element);

        //        //                }

        //        //            }
        //        //        }

        //        //    }

        //        //    WbsList = WbsList.Distinct().ToList();
        //        //    WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //        //    string WbsHeads = "";
        //        //    foreach (var w in WbsHeadsList)
        //        //    {
        //        //        if (WbsHeads == "")
        //        //        {
        //        //            WbsHeads = w;
        //        //        }
        //        //        else
        //        //        {
        //        //            WbsHeads = WbsHeads + "," + w;
        //        //        }
        //        //    }

        //        //    string ProjectCodes = "";
        //        //    foreach (var w in WbsList)
        //        //    {
        //        //        if (ProjectCodes == "")
        //        //        {
        //        //            ProjectCodes = w;
        //        //        }
        //        //        else
        //        //        {
        //        //            ProjectCodes = ProjectCodes + "," + w;
        //        //        }
        //        //    }

        //        //    result.Add(new TEPurchaseHomeModel()
        //        //    {
        //        //        Purchasing_Order_Number = item.Purchasing_Order_Number,
        //        //        //Vendor_Account_Number = item.Vendor_Account_Number,
        //        //        // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
        //        //        Vendor_Account_Number = item.Vendor_Account_Number,
        //        //        Purchasing_Document_Date = item.Purchasing_Document_Date,
        //        //        //FundCenter_Description = item.FundCenter_Description,
        //        //        Path = item.path,
        //        //        ReleaseCodeStatus = releasestatus,
        //        //        Purchasing_Release_Date = releasedate,
        //        //        Amount = TotalPrice,
        //        //        Currency_Key = item.Currency_Key,
        //        //        HeaderUniqueid = item.Uniqueid,
        //        //        PoCount = PoCount,
        //        //        PoTitle = item.PO_Title,
        //        //        VendorName = (vendor
        //        //        .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
        //        //            .Select(x => x.Vendor_Owner).FirstOrDefault()),
        //        //        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //        //        POStatus = item.ReleaseCode2Status,
        //        //        ProjectCodes = ProjectCodes,
        //        //        SubmitterName = item.SubmitterName,
        //        //        WbsHeads = WbsHeads,
        //        //        ManagerName = item.Managed_by


        //        //    });
        //        //}
        //        #endregion
        //        foreach (var item in finalResult)
        //        {
        //            string releasestatus = "";
        //            DateTime? releasedate = null;
        //            releasestatus = Status;
        //            //  string[] WbsList ={null};
        //            double? TotalPrice = 0.00;
        //            DateTime GSTDate = new DateTime(2017, 07, 03);
        //            DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

        //            List<string> WbsList = new List<string>();
        //            List<string> WbsHeadsList = new List<string>();


        //            var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                    ).ToList();
        //            if (ItemStructure.Count > 0)
        //            {
        //                //TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.TotalAmount.Value));
        //                TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.GrossAmount));
        //            }

        //            result.Add(new TEPurchaseHomeModel()
        //            {
        //                Purchasing_Order_Number = string.IsNullOrEmpty(item.Purchasing_Order_Number) ? item.Uniqueid.ToString() : item.Purchasing_Order_Number,
        //                Vendor_Account_Number = item.Vendor_Account_Number,
        //                Purchasing_Document_Date = item.Purchasing_Document_Date,
        //                Path = item.path,
        //                ReleaseCodeStatus = item.ReleaseCode2Status,
        //                Purchasing_Release_Date = releasedate,
        //                Amount = TotalPrice,
        //                Currency_Key = item.Currency_Key,
        //                HeaderUniqueid = item.Uniqueid,
        //                PoCount = PoCount,
        //                PoTitle = item.PO_Title,
        //                VendorName = item.Vendor_Owner,
        //                Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
        //                POStatus = item.ReleaseCode2Status,
        //                ProjectCodes = item.ProjectCode,                        
        //                SubmitterName = item.SubmitterName,
        //                WbsHeads = item.FundCenter_Description,
        //                ManagerName = item.Managed_by,
        //                ProjectShortName = item.ProjectShortName,
        //                Version ="R"+item.Version
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        db.ApplicationErrorLogs.Add(
        //            new ApplicationErrorLog
        //            {
        //                Error = ex.Message,
        //                ExceptionDateTime = System.DateTime.Now,
        //                InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
        //                Source = "From TEPurchaseHome API | TEPurchaseHome | " + this.GetType().ToString(),
        //                Stacktrace = ex.StackTrace
        //            }
        //            );
        //    }

        //    //db.SaveChanges();

        //    //if (SortBy == "PoDate")
        //    //    result = result.OrderBy(x => x.Purchasing_Document_Date).ToList();

        //    //else if (SortBy == "VendorName")
        //    //    result = result.OrderBy(x => x.VendorName).ToList();
        //    ////else if (SortBy == "PoNumber")
        //    ////    return result.OrderBy(x => x.Purchasing_Order_Number).ToList();
        //    //else
        //    //    result = result.OrderByDescending(x => x.Purchasing_Document_Date).ToList();

        //    if (result.Count > 0)
        //    {
        //        hrm.Content = new JsonContent(new
        //        {
        //            result = result,
        //            info = sinfo
        //        });
        //    }
        //    else
        //    {
        //        finfo.errorcode = 0;
        //        finfo.errormessage = "No Records";
        //        finfo.listcount = 0;
        //        hrm.Content = new JsonContent(new
        //        {
        //            result = result,
        //            info = finfo
        //        });
        //    }
        //    return hrm;
        //}
        //[HttpPost]
        //public HttpResponseMessage TEPurchaseDraftList(JObject json)
        //{
        //    int UserId = 0; string FilterBy = string.Empty;
        //    int PageCount = 0;
        //    UserId = json["UserId"].ToObject<int>();
        //    PageCount = json["PageCount"].ToObject<int>();
        //    FilterBy = json["FilterBy"].ToObject<string>();

        //    List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();


        //    int PoCount = 0;
        //    UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
        //    string user1 = "";
        //    user1 = "Approver";
        //    //foreach (var item in profile.webpages_Roles)
        //    //{
        //    //    if (item.RoleName.Equals("PO  Approval Admin"))
        //    //    {
        //    //        user1 = "PO  Approval Admin";
        //    //        break;
        //    //    }
        //    //}


        //    var query = (from header in db.TEPOHeaderStructures
        //                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

        //                 where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))

        //                 select new
        //                 {
        //                     header.Purchasing_Order_Number,
        //                     header.Vendor_Account_Number,
        //                     // vend.Vendor_Owner,
        //                     header.Purchasing_Document_Date,
        //                     header.path,
        //                     header.ReleaseCode2Status,
        //                     header.Currency_Key,
        //                     header.Uniqueid,
        //                     header.PO_Title,
        //                     header.SubmitterName

        //                 }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();

        //    PoCount = query.Count();
        //    query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

        //    if (user1 == "PO  Approval Admin")
        //    {
        //        query = (from header in db.TEPOHeaderStructures
        //                 where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))

        //                 select new
        //                 {
        //                     header.Purchasing_Order_Number,
        //                     header.Vendor_Account_Number,
        //                     // vend.Vendor_Owner,
        //                     header.Purchasing_Document_Date,
        //                     header.path,
        //                     header.ReleaseCode2Status,
        //                     header.Currency_Key,
        //                     header.Uniqueid,
        //                     header.PO_Title,
        //                     header.SubmitterName

        //                 }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();


        //        if (FilterBy != null)
        //        {
        //            query = (from header in db.TEPOHeaderStructures
        //                     join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code into ps
        //                     from v in ps.DefaultIfEmpty()
        //                     where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))

        //                     select new
        //                     {
        //                         header.Purchasing_Order_Number,
        //                         header.Vendor_Account_Number,
        //                         // vend.Vendor_Owner,
        //                         header.Purchasing_Document_Date,
        //                         header.path,
        //                         header.ReleaseCode2Status,
        //                         header.Currency_Key,
        //                         header.Uniqueid,
        //                         header.PO_Title,
        //                         header.SubmitterName

        //                     }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();
        //        }


        //        PoCount = query.Count();
        //        query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //    }
        //    else
        //    {



        //        query = (from header in db.TEPOHeaderStructures
        //                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

        //                 where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))

        //                 select new
        //                 {
        //                     header.Purchasing_Order_Number,
        //                     header.Vendor_Account_Number,
        //                     // vend.Vendor_Owner,
        //                     header.Purchasing_Document_Date,
        //                     header.path,
        //                     header.ReleaseCode2Status,
        //                     header.Currency_Key,
        //                     header.Uniqueid,
        //                     header.PO_Title,
        //                     header.SubmitterName

        //                 }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();


        //        if (FilterBy != null)
        //        {
        //            query = (from header in db.TEPOHeaderStructures
        //                     join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
        //                     join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code into ps
        //                     from v in ps.DefaultIfEmpty()
        //                     where (appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))

        //                     select new
        //                     {
        //                         header.Purchasing_Order_Number,
        //                         header.Vendor_Account_Number,
        //                         // vend.Vendor_Owner,
        //                         header.Purchasing_Document_Date,
        //                         header.path,
        //                         header.ReleaseCode2Status,
        //                         header.Currency_Key,
        //                         header.Uniqueid,
        //                         header.PO_Title,
        //                         header.SubmitterName

        //                     }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();
        //        }


        //        PoCount = query.Count();
        //        query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
        //    }

        //    foreach (var item in query)
        //    {
        //        double TotalPrice = 0.00;
        //        DateTime GSTDate = new DateTime(2017, 07, 03);
        //        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

        //        List<TEPOItemwise> wiseList = db.TEPOItemwises.Where(i => i.POStructureId == item.Uniqueid
        //                                                                            && (i.Condition_Type != "NAVS"
        //                                                                            && i.Condition_Type != "JICG"
        //                                                                            && i.Condition_Type != "JISG"
        //                                                                            && i.Condition_Type != "JICR"
        //                                                                            && i.Condition_Type != "JISR"
        //                                                                            && i.Condition_Type != "JIIR"
        //                                                                            )
        //                                                                            && (i.VendorCode == null
        //                                                                                || i.VendorCode == ""
        //                                                                                || i.VendorCode == item.Vendor_Account_Number
        //                                                                                || postingDate <= GSTDate
        //                                                                                )
        //                                                                            && i.IsDeleted == false).ToList();

        //        if (wiseList.Count > 0)
        //        {
        //            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
        //            TotalPrice = Math.Round(TotalPrice);
        //            TotalPrice = Math.Truncate(TotalPrice);
        //        }

        //        TEPOVendor Vendor = db.TEPOVendors.Where(x => x.IsDeleted == false && x.Vendor_Code == item.Vendor_Account_Number).FirstOrDefault();
        //        string vendor_owner = "";
        //        if (Vendor != null)
        //        {
        //            vendor_owner = Vendor.Vendor_Owner;
        //        }

        //        List<string> WbsList = new List<string>();
        //        List<string> WbsHeadsList = new List<string>();

        //        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
        //                                                ).ToList();


        //        foreach (var IS in ItemStructure)
        //        {
        //            var WBSElementsList = new List<string>();

        //            if (IS.Item_Category != "D")
        //            {
        //                WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
        //                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
        //                           .Select(x => x.WBS_Element).ToList();


        //            }
        //            else if (IS.Item_Category == "D" && IS.Material_Number == "")
        //            {
        //                WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
        //                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
        //                         .Select(x => x.WBS_Element).ToList();

        //            }
        //            foreach (var ele in WBSElementsList)
        //            {
        //                //getWbsElement(WbsList, ele, i);



        //                int occ = ele.Count(x => x == '-');

        //                if (occ >= 3)
        //                {
        //                    int index = CustomIndexOf(ele, '-', 3);
        //                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
        //                    string part = ele.Substring(0, index);
        //                    WbsHeadsList.Add(part);
        //                }
        //                else
        //                {
        //                    WbsHeadsList.Add(ele);
        //                }

        //                string Element = ele;
        //                char firstChar = Element[0];
        //                if (firstChar == '0')
        //                {

        //                    WbsList.Add(Element.Substring(0, 4));

        //                }
        //                else if (firstChar == 'A')
        //                {

        //                    WbsList.Add(Element.Substring(0, 5));

        //                }
        //                else if (firstChar == 'C')
        //                {

        //                    WbsList.Add(Element.Substring(0, 5));

        //                }
        //                else if (firstChar == 'M')
        //                {
        //                    string twoChar = Element.Substring(0, 2);
        //                    if (twoChar == "MN")
        //                    {

        //                        WbsList.Add(Element.Substring(0, 7));

        //                    }
        //                    else if (twoChar == "MC")
        //                    {

        //                        WbsList.Add(Element.Substring(0, 4));

        //                    }
        //                }
        //                else if (firstChar == 'Y')
        //                {
        //                    string twoChar = Element.Substring(0, 2);
        //                    if (twoChar == "YS")
        //                    {

        //                        WbsList.Add(Element.Substring(0, 7));

        //                    }

        //                }
        //                else if (firstChar == 'O')
        //                {
        //                    string threeChar = Element.Substring(0, 3);
        //                    if (threeChar == "OB2")
        //                    {

        //                        WbsList.Add(Element);

        //                    }

        //                }
        //            }
        //        }

        //        WbsList = WbsList.Distinct().ToList();
        //        WbsHeadsList = WbsHeadsList.Distinct().ToList();

        //        string WbsHeads = "";
        //        foreach (var w in WbsHeadsList)
        //        {
        //            if (WbsHeads == "")
        //            {
        //                WbsHeads = w;
        //            }
        //            else
        //            {
        //                WbsHeads = WbsHeads + "," + w;
        //            }
        //        }

        //        string ProjectCodes = "";
        //        foreach (var w in WbsList)
        //        {
        //            if (ProjectCodes == "")
        //            {
        //                ProjectCodes = w;
        //            }
        //            else
        //            {
        //                ProjectCodes = ProjectCodes + "," + w;
        //            }
        //        }
        //        PoCount = PoCount + 1;
        //        result.Add(new TEPurchaseHomeModel()
        //        {
        //            Purchasing_Order_Number = item.Purchasing_Order_Number,
        //            Vendor_Account_Number = item.Vendor_Account_Number,
        //            Purchasing_Document_Date = item.Purchasing_Document_Date,
        //            Path = item.path,
        //            ReleaseCodeStatus = item.ReleaseCode2Status,
        //            Amount
        //            = TotalPrice,
        //            Currency_Key = item.Currency_Key,
        //            HeaderUniqueid = item.Uniqueid,
        //            PoCount = PoCount,
        //            PoTitle = item.PO_Title,
        //            VendorName = vendor_owner,
        //            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
        //            POStatus = item.ReleaseCode2Status,
        //            ProjectCodes = ProjectCodes,
        //            SubmitterName = item.SubmitterName,
        //            WbsHeads = WbsHeads
        //        });
        //    }
        //    return new HttpResponseMessage
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = new JsonContent(new
        //        {

        //            res = result

        //        })
        //    };

        //}
        #endregion
        [HttpPost]
        public HttpResponseMessage TEPurchasePendingForApproval_Pagination(JObject json)
        {
            int UserId = 0; string Status = string.Empty;
            string SortBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            //PageCount = json["PageCount"].ToObject<int>();
            if (json["Status"] != null)
                Status = json["Status"].ToObject<string>();
            if (json["SortBy"] != null)
                SortBy = json["SortBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> fResult = new List<TEPurchaseHomeModel>();
            try
            {
                #region OldPO
                //var dte = System.DateTime.Now;

                //var status = db.TEPurchase_header_structure;
                //var vendor = db.TEPurchase_Vendor;
                //int PoCount = 0;

                ////var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();

                //string user = "";
                //string username = "";

                //db.Configuration.ProxyCreationEnabled = true;

                //UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();

                //user = profile.CallName;
                //username = profile.UserName;

                //string user1 = "";
                //user1 = "Approver";
                //foreach (var item in profile.webpages_Roles)
                //{
                //    if (item.RoleName.Equals("PO  Approval Admin"))
                //    {
                //        user1 = "PO  Approval Admin";
                //        break;
                //    }
                //}
                ////user1 = "PO  Approval Admin";

                //if (user1 == "PO  Approval Admin")
                //{
                //    var query = (from header in db.TEPOHeaderStructures
                //                     // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
                //                 where (header.ReleaseCode2Status == "Pending For Approval" && header.IsDeleted == false)

                //                 orderby header.Uniqueid descending
                //                 select new
                //                 {
                //                     header.Purchasing_Order_Number,
                //                     header.Vendor_Account_Number,
                //                     header.Purchasing_Document_Date,
                //                     header.path,
                //                     header.ReleaseCode2,
                //                     header.ReleaseCode2Status,
                //                     header.ReleaseCode2Date,
                //                     header.ReleaseCode3,
                //                     header.ReleaseCode3Status,
                //                     header.ReleaseCode3Date,
                //                     header.ReleaseCode4,
                //                     header.ReleaseCode4Status,
                //                     header.ReleaseCode4Date,
                //                     //fund.FundCenter_Description,
                //                     header.Currency_Key,
                //                     //appr.UniqueId,
                //                     //appr.SequenceNumber,
                //                     //appr.ReleaseCode,
                //                     // v.Vendor_Owner
                //                     header.Uniqueid,
                //                     header.PO_Title,
                //                     header.SubmitterName,
                //                     header.Managed_by
                //                 }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
                //    //PoCount = query.Count();
                //    //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();



                //PoCount = query.Count;
                //    var finalResult = query;
                //    if (PoCount > 0)
                //    {
                //        if (pagenumber >= 0 && pagepercount > 0)
                //        {
                //            if (pagenumber == 0)
                //            {
                //                pagenumber = 1;
                //            }
                //            int iPageNum = pagenumber;
                //            int iPageSize = pagepercount;
                //            int start = iPageNum - 1;
                //            start = start * iPageSize;
                //            finalResult = query.Skip(start).Take(iPageSize).ToList();
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                //            sinfo.torecords = finalResult.Count + start;
                //            sinfo.totalrecords = PoCount;
                //            sinfo.listcount = finalResult.Count;
                //            sinfo.pages = pagenumber.ToString();
                //        }
                //        else
                //        {
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = 1;
                //            sinfo.torecords = 10;
                //            sinfo.totalrecords = 0;
                //            sinfo.listcount = 0;
                //            sinfo.pages = "0";

                //            hrm.Content = new JsonContent(new
                //            {
                //                result = query, //error
                //                info = sinfo //return exception
                //            });
                //        }
                //    }
                //    else
                //    {
                //        sinfo.errorcode = 0;
                //        sinfo.errormessage = "No Records Found";
                //        sinfo.fromrecords = 1;
                //        sinfo.torecords = 10;
                //        sinfo.totalrecords = 0;
                //        sinfo.listcount = 0;
                //        sinfo.pages = "0";

                //        hrm.Content = new JsonContent(new
                //        {
                //            info = sinfo //return exception
                //        });
                //    }
                //    foreach (var item in finalResult)
                //    {
                //        string releasestatus = "";
                //        DateTime? releasedate = null;
                //        releasestatus = Status;
                //        //  string[] WbsList ={null};
                //        double? TotalPrice = 0.00;
                //        DateTime GSTDate = new DateTime(2017, 07, 03);
                //        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                //        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                //                                                                            && (i.Condition_Type != "NAVS"
                //                                                                            && i.Condition_Type != "JICG"
                //                                                                        && i.Condition_Type != "JISG"
                //                                                                        && i.Condition_Type != "JICR"
                //                                                                        && i.Condition_Type != "JISR"
                //                                                                        && i.Condition_Type != "JIIR"
                //                                                                        )
                //                                                                        && (i.VendorCode == null
                //                                                                            || i.VendorCode == ""
                //                                                                            || i.VendorCode == item.Vendor_Account_Number
                //                                                                            || postingDate <= GSTDate
                //                                                                            )
                //                                                                            && i.IsDeleted == false
                //                                                                            ).ToList();

                //        if (wiseList.Count > 0)
                //        {
                //            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                //        }


                //        List<string> WbsList = new List<string>();
                //        List<string> WbsHeadsList = new List<string>();


                //        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                //                                                ).ToList();


                //        foreach (var IS in ItemStructure)
                //        {
                //            var WBSElementsList = new List<string>();

                //            if (IS.Item_Category != "D")
                //            {
                //                WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
                //                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                //                           .Select(x => x.WBS_Element).ToList();

                //            }
                //            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                //            {
                //                WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
                //                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                //                         .Select(x => x.WBS_Element).ToList();
                //            }


                //            foreach (var ele in WBSElementsList)
                //            {
                //                //getWbsElement(WbsList, ele, i);



                //                int occ = ele.Count(x => x == '-');

                //                if (occ >= 3)
                //                {
                //                    int index = CustomIndexOf(ele, '-', 3);
                //                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                //                    string part = ele.Substring(0, index);
                //                    WbsHeadsList.Add(part);
                //                }
                //                else
                //                {
                //                    WbsHeadsList.Add(ele);
                //                }

                //                string Element = ele;
                //                char firstChar = Element[0];
                //                if (firstChar == '0')
                //                {

                //                    WbsList.Add(Element.Substring(0, 4));

                //                }
                //                else if (firstChar == 'A')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'C')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'M')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "MN")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }
                //                    else if (twoChar == "MC")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 4));

                //                    }
                //                }
                //                else if (firstChar == 'Y')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "YS")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }

                //                }
                //                else if (firstChar == 'O')
                //                {
                //                    string threeChar = Element.Substring(0, 3);
                //                    if (threeChar == "OB2")
                //                    {

                //                        WbsList.Add(Element);

                //                    }

                //                }
                //            }

                //        }


                //        WbsList = WbsList.Distinct().ToList();
                //        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                //        string WbsHeads = "";
                //        foreach (var w in WbsHeadsList)
                //        {
                //            if (WbsHeads == "")
                //            {
                //                WbsHeads = w;
                //            }
                //            else
                //            {
                //                WbsHeads = WbsHeads + "," + w;
                //            }
                //        }

                //        string ProjectCodes = "";
                //        foreach (var w in WbsList)
                //        {
                //            if (ProjectCodes == "")
                //            {
                //                ProjectCodes = w;
                //            }
                //            else
                //            {
                //                ProjectCodes = ProjectCodes + "," + w;
                //            }
                //        }

                //        result.Add(new TEPurchaseHomeModel()
                //        {
                //            Purchasing_Order_Number = item.Purchasing_Order_Number,
                //            //Vendor_Account_Number = item.Vendor_Account_Number,
                //            // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
                //            Vendor_Account_Number = item.Vendor_Account_Number,
                //            Purchasing_Document_Date = item.Purchasing_Document_Date,
                //            //FundCenter_Description = item.FundCenter_Description,
                //            Path = item.path,
                //            ReleaseCodeStatus = releasestatus,
                //            Purchasing_Release_Date = releasedate,
                //            Amount
                //            = TotalPrice,
                //            Currency_Key = item.Currency_Key,
                //            HeaderUniqueid = item.Uniqueid,
                //            PoCount = PoCount,
                //            PoTitle = item.PO_Title,
                //            VendorName = (vendor
                //            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                //                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                //            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),

                //            POStatus = item.ReleaseCode2Status,
                //            ProjectCodes = ProjectCodes,
                //            SubmitterName = item.SubmitterName,
                //            WbsHeads = WbsHeads,
                //            ManagerName = item.Managed_by


                //        });
                //    }
                //}
                //else
                //{
                //    var query = (from header in db.TEPOHeaderStructures
                //                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

                //                 where (appr.ApproverId == UserId && appr.IsDeleted == false && (appr.Status == "Pending For Approval") && header.IsDeleted == false && header.ReleaseCode2Status != "Pending For Approval" && appr.SequenceNumber != 0)
                //                 orderby header.Uniqueid descending
                //                 select new
                //                 {
                //                     header.Purchasing_Order_Number,
                //                     header.Vendor_Account_Number,
                //                     // vend.Vendor_Owner,
                //                     header.Purchasing_Document_Date,
                //                     header.path,
                //                     header.ReleaseCode2,
                //                     header.ReleaseCode2Status,
                //                     header.ReleaseCode2Date,
                //                     header.ReleaseCode3,
                //                     header.ReleaseCode3Status,
                //                     header.ReleaseCode3Date,
                //                     header.ReleaseCode4,
                //                     header.ReleaseCode4Status,
                //                     header.ReleaseCode4Date,
                //                     // fund.FundCenter_Description,
                //                     header.Currency_Key,
                //                     //appr.UniqueId,
                //                     //appr.SequenceNumber,
                //                     //appr.ReleaseCode,
                //                     header.Uniqueid,
                //                     header.PO_Title,
                //                     header.SubmitterName,
                //                     header.Managed_by

                //                 }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

                //    //PoCount = query.Count();
                //    //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                //    PoCount = query.Count;
                //    var finalResult = query;
                //    if (PoCount > 0)
                //    {
                //        if (pagenumber >= 0 && pagepercount > 0)
                //        {
                //            if (pagenumber == 0)
                //            {
                //                pagenumber = 1;
                //            }
                //            int iPageNum = pagenumber;
                //            int iPageSize = pagepercount;
                //            int start = iPageNum - 1;
                //            start = start * iPageSize;
                //            finalResult = query.Skip(start).Take(iPageSize).ToList();
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                //            sinfo.torecords = finalResult.Count + start;
                //            sinfo.totalrecords = PoCount;
                //            sinfo.listcount = finalResult.Count;
                //            sinfo.pages = pagenumber.ToString();
                //        }
                //        else
                //        {
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = 1;
                //            sinfo.torecords = 10;
                //            sinfo.totalrecords = 0;
                //            sinfo.listcount = 0;
                //            sinfo.pages = "0";

                //            hrm.Content = new JsonContent(new
                //            {
                //                result = query, //error
                //                info = sinfo //return exception
                //            });
                //        }
                //    }
                //    else
                //    {
                //        sinfo.errorcode = 0;
                //        sinfo.errormessage = "No Records Found";
                //        sinfo.fromrecords = 1;
                //        sinfo.torecords = 10;
                //        sinfo.totalrecords = 0;
                //        sinfo.listcount = 0;
                //        sinfo.pages = "0";

                //        hrm.Content = new JsonContent(new
                //        {
                //            info = sinfo //return exception
                //        });
                //    }

                //    foreach (var item in finalResult)
                //    {
                //        string releasestatus = "";
                //        DateTime? releasedate = null;
                //        releasestatus = Status;

                //        double? TotalPrice = 0.00;
                //        DateTime GSTDate = new DateTime(2017, 07, 03);
                //        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                //        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                //                                                                            && (i.Condition_Type != "NAVS"
                //                                                                            && i.Condition_Type != "JICG"
                //                                                                        && i.Condition_Type != "JISG"
                //                                                                        && i.Condition_Type != "JICR"
                //                                                                        && i.Condition_Type != "JISR"
                //                                                                        && i.Condition_Type != "JIIR"
                //                                                                        )
                //                                                                        && (i.VendorCode == null
                //                                                                            || i.VendorCode == ""
                //                                                                            || i.VendorCode == item.Vendor_Account_Number
                //                                                                            || postingDate <= GSTDate
                //                                                                            )
                //                                                                            && i.IsDeleted == false).ToList();

                //        if (wiseList.Count > 0)
                //        {
                //            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                //        }




                //        List<string> WbsList = new List<string>();
                //        List<string> WbsHeadsList = new List<string>();

                //        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                //                                                ).ToList();


                //        foreach (var IS in ItemStructure)
                //        {
                //            var WBSElementsList = new List<string>();

                //            if (IS.Item_Category != "D")
                //            {
                //                WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
                //                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                //                           .Select(x => x.WBS_Element).ToList();

                //            }
                //            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                //            {
                //                WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
                //                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                //                         .Select(x => x.WBS_Element).ToList();
                //            }


                //            foreach (var ele in WBSElementsList)
                //            {
                //                //getWbsElement(WbsList, ele, i);



                //                int occ = ele.Count(x => x == '-');

                //                if (occ >= 3)
                //                {
                //                    int index = CustomIndexOf(ele, '-', 3);
                //                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                //                    string part = ele.Substring(0, index);
                //                    WbsHeadsList.Add(part);
                //                }
                //                else
                //                {
                //                    WbsHeadsList.Add(ele);
                //                }

                //                string Element = ele;
                //                char firstChar = Element[0];
                //                if (firstChar == '0')
                //                {

                //                    WbsList.Add(Element.Substring(0, 4));

                //                }
                //                else if (firstChar == 'A')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'C')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'M')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "MN")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }
                //                    else if (twoChar == "MC")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 4));

                //                    }
                //                }
                //                else if (firstChar == 'Y')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "YS")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }

                //                }
                //                else if (firstChar == 'O')
                //                {
                //                    string threeChar = Element.Substring(0, 3);
                //                    if (threeChar == "OB2")
                //                    {

                //                        WbsList.Add(Element);

                //                    }

                //                }
                //            }

                //        }


                //        WbsList = WbsList.Distinct().ToList();
                //        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                //        string WbsHeads = "";
                //        foreach (var w in WbsHeadsList)
                //        {
                //            if (WbsHeads == "")
                //            {
                //                WbsHeads = w;
                //            }
                //            else
                //            {
                //                WbsHeads = WbsHeads + "," + w;
                //            }
                //        }
                //        string ProjectCodes = "";
                //        foreach (var w in WbsList)
                //        {
                //            if (ProjectCodes == "")
                //            {
                //                ProjectCodes = w;
                //            }
                //            else
                //            {
                //                ProjectCodes = ProjectCodes + "," + w;
                //            }
                //        }

                //        result.Add(new TEPurchaseHomeModel()
                //        {
                //            Purchasing_Order_Number = item.Purchasing_Order_Number,
                //            Vendor_Account_Number = item.Vendor_Account_Number,
                //            Purchasing_Document_Date = item.Purchasing_Document_Date,
                //            // FundCenter_Description = item.FundCenter_Description,
                //            Path = item.path,
                //            ReleaseCodeStatus = releasestatus,
                //            Purchasing_Release_Date = releasedate,
                //            Amount = TotalPrice,
                //            Currency_Key = item.Currency_Key,
                //            HeaderUniqueid = item.Uniqueid,
                //            PoCount = PoCount,
                //            PoTitle = item.PO_Title,
                //            VendorName = (vendor
                //            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                //                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                //            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                //            POStatus = item.ReleaseCode2Status,
                //            ProjectCodes = ProjectCodes,
                //            SubmitterName = item.SubmitterName,
                //            WbsHeads = WbsHeads,
                //            ManagerName = item.Managed_by

                //        });
                //    }
                //}
                #endregion
                int PoCount = 0;
                var query = (from header in db.TEPOHeaderStructures
                             join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                             join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                             join v in db.TEPOVendorMasterDetails on header.VendorID equals v.POVendorDetailId
                             join vendor in this.db.TEPOVendorMasters on v.POVendorMasterId equals vendor.POVendorMasterId
                             join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                             from proj in tempprj.DefaultIfEmpty()
                                 //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                             where (header.IsDeleted == false && header.IsNewPO == true
                             && appr.ApproverId == UserId && appr.IsDeleted == false && appr.Status == "Pending For Approval"
                             && appr.SequenceNumber != 0 && header.ReleaseCode2Status == "Pending For Approval")
                             select new
                             {
                                 header.Purchasing_Order_Number,
                                 header.Vendor_Account_Number,
                                 header.Purchasing_Document_Date,
                                 header.path,
                                 header.ReleaseCode2Status,
                                 header.ReleaseCode2Date,
                                 fund.FundCenter_Description,
                                 header.Currency_Key,
                                 Vendor_Owner = vendor.VendorName,
                                 header.Uniqueid,
                                 header.PO_Title,
                                 header.SubmitterName,
                                 header.Managed_by,
                                 proj.ProjectCode,
                                 proj.ProjectName
                             }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

                PoCount = query.Count;
                var finalResult = query;
                if (PoCount > 0)
                {
                    if (pagenumber >= 0 && pagepercount > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        finalResult = query.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = PoCount;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                    }
                    else
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = 1;
                        sinfo.torecords = 10;
                        sinfo.totalrecords = 0;
                        sinfo.listcount = 0;
                        sinfo.pages = "0";

                        hrm.Content = new JsonContent(new
                        {
                            result = query, //error
                            info = sinfo //return exception
                        });
                    }
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";

                    hrm.Content = new JsonContent(new
                    {
                        info = sinfo //return exception
                    });
                }
                foreach (var item in finalResult)
                {
                    DateTime? releasedate = null;
                    double? TotalPrice = 0.00;
                    DateTime GSTDate = new DateTime(2017, 07, 03);
                    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();


                    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                            ).ToList();
                    if (ItemStructure.Count > 0)
                    {
                        TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.TotalAmount.Value));
                    }

                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = string.IsNullOrEmpty(item.Purchasing_Order_Number) ? item.Uniqueid.ToString() : item.Purchasing_Order_Number,
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = item.ReleaseCode2Status,
                        Purchasing_Release_Date = releasedate,
                        Amount = TotalPrice,
                        Currency_Key = item.Currency_Key,
                        HeaderUniqueid = item.Uniqueid,
                        PoCount = PoCount,
                        PoTitle = item.PO_Title,
                        VendorName = item.Vendor_Owner,
                        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                        POStatus = item.ReleaseCode2Status,
                        ProjectCodes = item.ProjectCode,
                        SubmitterName = item.SubmitterName,
                        WbsHeads = item.FundCenter_Description,
                        ManagerName = item.Managed_by
                    });
                }

            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEPurchaseHome API | TEPurchaseHome | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            if (result.Count > 0)
            {
                foreach (TEPurchaseHomeModel purHome in result)
                {
                    if (purHome.Approvers.Count > 0)
                    {
                        var NextApprover = (from u in db.TEPOApprovers
                                            where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval" && u.SequenceNumber != 0 && u.IsDeleted == false)
                                            orderby u.SequenceNumber
                                            select u).FirstOrDefault();
                        if (NextApprover != null)
                        {
                            if (UserId == NextApprover.ApproverId)
                                purHome.isCurrentApprover = true;
                            else
                                purHome.isCurrentApprover = false;
                        }
                        else
                            purHome.isCurrentApprover = false;
                    }
                    fResult.Add(purHome);
                }
            }
            if (fResult.Count > 0)
            {
                hrm.Content = new JsonContent(new
                {
                    result = fResult,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseApproved_Pagination_swamy(JObject json)
        {
            int UserId = 0; string Status = string.Empty;
            string SortBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            //PageCount = json["PageCount"].ToObject<int>();
            //Status = json["Status"].ToObject<string>();
            if (json["SortBy"] != null)
                SortBy = json["SortBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> fResult = new List<TEPurchaseHomeModel>();
            try
            {
                #region
                //var dte = System.DateTime.Now;

                //var status = db.TEPurchase_header_structure;
                //var vendor = db.TEPurchase_Vendor;
                //int PoCount = 0;

                ////var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();

                //string user = "";
                //string username = "";

                //db.Configuration.ProxyCreationEnabled = true;

                //UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();

                //user = profile.CallName;
                //username = profile.UserName;

                //string user1 = "";
                //user1 = "Approver";
                //foreach (var item in profile.webpages_Roles)
                //{
                //    if (item.RoleName.Equals("PO  Approval Admin"))
                //    {
                //        user1 = "PO  Approval Admin";
                //        break;
                //    }
                //}
                ////user1 = "PO  Approval Admin";

                //if (user1 == "PO  Approval Admin")
                //{
                //    var query = (from header in db.TEPOHeaderStructures
                //                     // join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code
                //                 where (header.ReleaseCode2Status == "Approved" && header.IsDeleted == false)

                //                 orderby header.Uniqueid descending
                //                 select new
                //                 {
                //                     header.Purchasing_Order_Number,
                //                     header.Vendor_Account_Number,
                //                     header.Purchasing_Document_Date,
                //                     header.path,
                //                     header.ReleaseCode2,
                //                     header.ReleaseCode2Status,
                //                     header.ReleaseCode2Date,
                //                     header.ReleaseCode3,
                //                     header.ReleaseCode3Status,
                //                     header.ReleaseCode3Date,
                //                     header.ReleaseCode4,
                //                     header.ReleaseCode4Status,
                //                     header.ReleaseCode4Date,
                //                     //fund.FundCenter_Description,
                //                     header.Currency_Key,
                //                     //appr.UniqueId,
                //                     //appr.SequenceNumber,
                //                     //appr.ReleaseCode,
                //                     // v.Vendor_Owner
                //                     header.Uniqueid,
                //                     header.PO_Title,
                //                     header.SubmitterName,
                //                     header.Managed_by
                //                 }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
                //    //PoCount = query.Count();
                //    //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                //    PoCount = query.Count;
                //    var finalResult = query;
                //    if (PoCount > 0)
                //    {
                //        if (pagenumber >= 0 && pagepercount > 0)
                //        {
                //            if (pagenumber == 0)
                //            {
                //                pagenumber = 1;
                //            }
                //            int iPageNum = pagenumber;
                //            int iPageSize = pagepercount;
                //            int start = iPageNum - 1;
                //            start = start * iPageSize;
                //            finalResult = query.Skip(start).Take(iPageSize).ToList();
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                //            sinfo.torecords = finalResult.Count + start;
                //            sinfo.totalrecords = PoCount;
                //            sinfo.listcount = finalResult.Count;
                //            sinfo.pages = pagenumber.ToString();
                //        }
                //        else
                //        {
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = 1;
                //            sinfo.torecords = 10;
                //            sinfo.totalrecords = 0;
                //            sinfo.listcount = 0;
                //            sinfo.pages = "0";

                //            hrm.Content = new JsonContent(new
                //            {
                //                result = query, //error
                //                info = sinfo //return exception
                //            });
                //        }
                //    }
                //    else
                //    {
                //        sinfo.errorcode = 0;
                //        sinfo.errormessage = "No Records Found";
                //        sinfo.fromrecords = 1;
                //        sinfo.torecords = 10;
                //        sinfo.totalrecords = 0;
                //        sinfo.listcount = 0;
                //        sinfo.pages = "0";

                //        hrm.Content = new JsonContent(new
                //        {
                //            info = sinfo //return exception
                //        });
                //    }
                //    foreach (var item in finalResult)
                //    {
                //        string releasestatus = "";
                //        DateTime? releasedate = null;
                //        releasestatus = Status;
                //        //  string[] WbsList ={null};
                //        double? TotalPrice = 0.00;
                //        DateTime GSTDate = new DateTime(2017, 07, 03);
                //        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                //        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                //                                                                            && (i.Condition_Type != "NAVS"
                //                                                                            && i.Condition_Type != "JICG"
                //                                                                        && i.Condition_Type != "JISG"
                //                                                                        && i.Condition_Type != "JICR"
                //                                                                        && i.Condition_Type != "JISR"
                //                                                                        && i.Condition_Type != "JIIR"
                //                                                                        )
                //                                                                        && (i.VendorCode == null
                //                                                                            || i.VendorCode == ""
                //                                                                            || i.VendorCode == item.Vendor_Account_Number
                //                                                                            || postingDate <= GSTDate
                //                                                                            )
                //                                                                            && i.IsDeleted == false
                //                                                                            ).ToList();

                //        if (wiseList.Count > 0)
                //        {
                //            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                //        }


                //        List<string> WbsList = new List<string>();
                //        List<string> WbsHeadsList = new List<string>();


                //        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                //                                                ).ToList();


                //        foreach (var IS in ItemStructure)
                //        {
                //            var WBSElementsList = new List<string>();

                //            if (IS.Item_Category != "D")
                //            {
                //                WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
                //                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                //                           .Select(x => x.WBS_Element).ToList();

                //            }
                //            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                //            {
                //                WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
                //                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                //                         .Select(x => x.WBS_Element).ToList();
                //            }


                //            foreach (var ele in WBSElementsList)
                //            {
                //                //getWbsElement(WbsList, ele, i);



                //                int occ = ele.Count(x => x == '-');

                //                if (occ >= 3)
                //                {
                //                    int index = CustomIndexOf(ele, '-', 3);
                //                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                //                    string part = ele.Substring(0, index);
                //                    WbsHeadsList.Add(part);
                //                }
                //                else
                //                {
                //                    WbsHeadsList.Add(ele);
                //                }

                //                string Element = ele;
                //                char firstChar = Element[0];
                //                if (firstChar == '0')
                //                {

                //                    WbsList.Add(Element.Substring(0, 4));

                //                }
                //                else if (firstChar == 'A')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'C')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'M')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "MN")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }
                //                    else if (twoChar == "MC")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 4));

                //                    }
                //                }
                //                else if (firstChar == 'Y')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "YS")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }

                //                }
                //                else if (firstChar == 'O')
                //                {
                //                    string threeChar = Element.Substring(0, 3);
                //                    if (threeChar == "OB2")
                //                    {

                //                        WbsList.Add(Element);

                //                    }

                //                }
                //            }

                //        }


                //        WbsList = WbsList.Distinct().ToList();
                //        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                //        string WbsHeads = "";
                //        foreach (var w in WbsHeadsList)
                //        {
                //            if (WbsHeads == "")
                //            {
                //                WbsHeads = w;
                //            }
                //            else
                //            {
                //                WbsHeads = WbsHeads + "," + w;
                //            }
                //        }

                //        string ProjectCodes = "";
                //        foreach (var w in WbsList)
                //        {
                //            if (ProjectCodes == "")
                //            {
                //                ProjectCodes = w;
                //            }
                //            else
                //            {
                //                ProjectCodes = ProjectCodes + "," + w;
                //            }
                //        }

                //        result.Add(new TEPurchaseHomeModel()
                //        {
                //            Purchasing_Order_Number = item.Purchasing_Order_Number,
                //            //Vendor_Account_Number = item.Vendor_Account_Number,
                //            // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
                //            Vendor_Account_Number = item.Vendor_Account_Number,
                //            Purchasing_Document_Date = item.Purchasing_Document_Date,
                //            //FundCenter_Description = item.FundCenter_Description,
                //            Path = item.path,
                //            ReleaseCodeStatus = releasestatus,
                //            Purchasing_Release_Date = releasedate,
                //            Amount
                //            = TotalPrice,
                //            Currency_Key = item.Currency_Key,
                //            HeaderUniqueid = item.Uniqueid,
                //            PoCount = PoCount,
                //            PoTitle = item.PO_Title,
                //            VendorName = (vendor
                //            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                //                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                //            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),

                //            POStatus = item.ReleaseCode2Status,
                //            ProjectCodes = ProjectCodes,
                //            SubmitterName = item.SubmitterName,
                //            WbsHeads = WbsHeads,
                //            ManagerName = item.Managed_by


                //        });
                //    }
                //}
                //else
                //{
                //    var query = (from header in db.TEPOHeaderStructures
                //                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                //                 where (appr.ApproverId == UserId && appr.IsDeleted == false && (appr.Status == "Approved") && (header.ReleaseCode2Status == "Approved" && header.IsDeleted == false) && appr.SequenceNumber != 0)
                //                 orderby header.Uniqueid descending
                //                 select new
                //                 {
                //                     header.Purchasing_Order_Number,
                //                     header.Vendor_Account_Number,
                //                     // vend.Vendor_Owner,
                //                     header.Purchasing_Document_Date,
                //                     header.path,
                //                     header.ReleaseCode2,
                //                     header.ReleaseCode2Status,
                //                     header.ReleaseCode2Date,
                //                     header.ReleaseCode3,
                //                     header.ReleaseCode3Status,
                //                     header.ReleaseCode3Date,
                //                     header.ReleaseCode4,
                //                     header.ReleaseCode4Status,
                //                     header.ReleaseCode4Date,
                //                     // fund.FundCenter_Description,
                //                     header.Currency_Key,
                //                     //appr.UniqueId,
                //                     //appr.SequenceNumber,
                //                     //appr.ReleaseCode,
                //                     header.Uniqueid,
                //                     header.PO_Title,
                //                     header.SubmitterName,
                //                     header.Managed_by

                //                 }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

                //    //PoCount = query.Count();
                //    //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                //    PoCount = query.Count;
                //    var finalResult = query;
                //    if (PoCount > 0)
                //    {
                //        if (pagenumber >= 0 && pagepercount > 0)
                //        {
                //            if (pagenumber == 0)
                //            {
                //                pagenumber = 1;
                //            }
                //            int iPageNum = pagenumber;
                //            int iPageSize = pagepercount;
                //            int start = iPageNum - 1;
                //            start = start * iPageSize;
                //            finalResult = query.Skip(start).Take(iPageSize).ToList();
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                //            sinfo.torecords = finalResult.Count + start;
                //            sinfo.totalrecords = PoCount;
                //            sinfo.listcount = finalResult.Count;
                //            sinfo.pages = pagenumber.ToString();
                //        }
                //        else
                //        {
                //            sinfo.errorcode = 0;
                //            sinfo.errormessage = "Success";
                //            sinfo.fromrecords = 1;
                //            sinfo.torecords = 10;
                //            sinfo.totalrecords = 0;
                //            sinfo.listcount = 0;
                //            sinfo.pages = "0";

                //            hrm.Content = new JsonContent(new
                //            {
                //                result = query, //error
                //                info = sinfo //return exception
                //            });
                //        }
                //    }
                //    else
                //    {
                //        sinfo.errorcode = 0;
                //        sinfo.errormessage = "No Records Found";
                //        sinfo.fromrecords = 1;
                //        sinfo.torecords = 10;
                //        sinfo.totalrecords = 0;
                //        sinfo.listcount = 0;
                //        sinfo.pages = "0";

                //        hrm.Content = new JsonContent(new
                //        {
                //            info = sinfo //return exception
                //        });
                //    }

                //    foreach (var item in finalResult)
                //    {
                //        string releasestatus = "";
                //        DateTime? releasedate = null;
                //        releasestatus = Status;

                //        double? TotalPrice = 0.00;
                //        DateTime GSTDate = new DateTime(2017, 07, 03);
                //        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                //        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                //                                                                            && (i.Condition_Type != "NAVS"
                //                                                                            && i.Condition_Type != "JICG"
                //                                                                        && i.Condition_Type != "JISG"
                //                                                                        && i.Condition_Type != "JICR"
                //                                                                        && i.Condition_Type != "JISR"
                //                                                                        && i.Condition_Type != "JIIR"
                //                                                                        )
                //                                                                        && (i.VendorCode == null
                //                                                                            || i.VendorCode == ""
                //                                                                            || i.VendorCode == item.Vendor_Account_Number
                //                                                                            || postingDate <= GSTDate
                //                                                                            )
                //                                                                            && i.IsDeleted == false).ToList();

                //        if (wiseList.Count > 0)
                //        {
                //            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                //        }




                //        List<string> WbsList = new List<string>();
                //        List<string> WbsHeadsList = new List<string>();

                //        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                //                                                ).ToList();


                //        foreach (var IS in ItemStructure)
                //        {
                //            var WBSElementsList = new List<string>();

                //            if (IS.Item_Category != "D")
                //            {
                //                WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
                //                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                //                           .Select(x => x.WBS_Element).ToList();

                //            }
                //            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                //            {
                //                WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
                //                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                //                         .Select(x => x.WBS_Element).ToList();
                //            }


                //            foreach (var ele in WBSElementsList)
                //            {
                //                //getWbsElement(WbsList, ele, i);



                //                int occ = ele.Count(x => x == '-');

                //                if (occ >= 3)
                //                {
                //                    int index = CustomIndexOf(ele, '-', 3);
                //                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                //                    string part = ele.Substring(0, index);
                //                    WbsHeadsList.Add(part);
                //                }
                //                else
                //                {
                //                    WbsHeadsList.Add(ele);
                //                }

                //                string Element = ele;
                //                char firstChar = Element[0];
                //                if (firstChar == '0')
                //                {

                //                    WbsList.Add(Element.Substring(0, 4));

                //                }
                //                else if (firstChar == 'A')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'C')
                //                {

                //                    WbsList.Add(Element.Substring(0, 5));

                //                }
                //                else if (firstChar == 'M')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "MN")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }
                //                    else if (twoChar == "MC")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 4));

                //                    }
                //                }
                //                else if (firstChar == 'Y')
                //                {
                //                    string twoChar = Element.Substring(0, 2);
                //                    if (twoChar == "YS")
                //                    {

                //                        WbsList.Add(Element.Substring(0, 7));

                //                    }

                //                }
                //                else if (firstChar == 'O')
                //                {
                //                    string threeChar = Element.Substring(0, 3);
                //                    if (threeChar == "OB2")
                //                    {

                //                        WbsList.Add(Element);

                //                    }

                //                }
                //            }

                //        }


                //        WbsList = WbsList.Distinct().ToList();
                //        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                //        string WbsHeads = "";
                //        foreach (var w in WbsHeadsList)
                //        {
                //            if (WbsHeads == "")
                //            {
                //                WbsHeads = w;
                //            }
                //            else
                //            {
                //                WbsHeads = WbsHeads + "," + w;
                //            }
                //        }
                //        string ProjectCodes = "";
                //        foreach (var w in WbsList)
                //        {
                //            if (ProjectCodes == "")
                //            {
                //                ProjectCodes = w;
                //            }
                //            else
                //            {
                //                ProjectCodes = ProjectCodes + "," + w;
                //            }
                //        }

                //        result.Add(new TEPurchaseHomeModel()
                //        {
                //            Purchasing_Order_Number = item.Purchasing_Order_Number,
                //            Vendor_Account_Number = item.Vendor_Account_Number,
                //            Purchasing_Document_Date = item.Purchasing_Document_Date,
                //            // FundCenter_Description = item.FundCenter_Description,
                //            Path = item.path,
                //            ReleaseCodeStatus = releasestatus,
                //            Purchasing_Release_Date = releasedate,
                //            Amount = TotalPrice,
                //            Currency_Key = item.Currency_Key,
                //            HeaderUniqueid = item.Uniqueid,
                //            PoCount = PoCount,
                //            PoTitle = item.PO_Title,
                //            VendorName = (vendor
                //            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                //                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                //            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                //            POStatus = item.ReleaseCode2Status,
                //            ProjectCodes = ProjectCodes,
                //            SubmitterName = item.SubmitterName,
                //            WbsHeads = WbsHeads,
                //            ManagerName = item.Managed_by

                //        });
                //    }
                //}
                #endregion

                int PoCount = 0;
                var query = (from header in db.TEPOHeaderStructures
                             join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                             join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                             join v in db.TEPOVendorMasterDetails on header.VendorID equals v.POVendorDetailId
                             join vendor in this.db.TEPOVendorMasters on v.POVendorMasterId equals vendor.POVendorMasterId
                             join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                             from proj in tempprj.DefaultIfEmpty()
                             join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                             from mnger in tempmgr.DefaultIfEmpty()
                             join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                             from finalorder in temporder.DefaultIfEmpty()
                                 //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                             where (header.IsDeleted == false && header.IsNewPO == true && header.Status == "Active"
                             && appr.ApproverId == UserId && appr.IsDeleted == false && appr.Status == "Approved"
                             //&& appr.SequenceNumber != 0
                             && header.ReleaseCode2Status == "Approved")
                             select new
                             {
                                 header.Purchasing_Order_Number,
                                 header.Vendor_Account_Number,
                                 header.Purchasing_Document_Date,
                                 header.path,
                                 header.CreatedBy,
                                 header.ReleaseCode2Status,
                                 header.ReleaseCode2Date,
                                 header.Version,
                                 fund.FundCenter_Description,
                                 header.Currency_Key,
                                 Vendor_Owner = vendor.VendorName,
                                 header.PurchaseRequestId,
                                 header.Uniqueid,
                                 header.PO_Title,
                                 OrderType = finalorder.Description,
                                 header.SubmitterName,
                                 Managed_by = mnger.CallName,
                                 header.POManagerID,
                                 proj.ProjectCode,
                                 proj.ProjectName,
                                 proj.ProjectShortName
                             }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

                PoCount = query.Count;
                var finalResult = query;
                if (PoCount > 0)
                {
                    if (pagenumber >= 0 && pagepercount > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        finalResult = query.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = PoCount;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                    }
                    else
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = 1;
                        sinfo.torecords = 10;
                        sinfo.totalrecords = 0;
                        sinfo.listcount = 0;
                        sinfo.pages = "0";

                        hrm.Content = new JsonContent(new
                        {
                            result = query, //error
                            info = sinfo //return exception
                        });
                    }
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";

                    hrm.Content = new JsonContent(new
                    {
                        info = sinfo //return exception
                    });
                }
                foreach (var item in finalResult)
                {
                    DateTime? releasedate = null;
                    double? TotalPrice = 0.00;
                    DateTime GSTDate = new DateTime(2017, 07, 03);
                    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();
                    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                            ).ToList();
                    if (ItemStructure.Count > 0)
                    {
                        TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount));
                    }

                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = item.ReleaseCode2Status,
                        Purchasing_Release_Date = releasedate,
                        Amount = TotalPrice,
                        PurchaseRequestId = item.PurchaseRequestId,
                        CreatedBy = item.CreatedBy,
                        Currency_Key = item.Currency_Key,
                        HeaderUniqueid = item.Uniqueid,
                        PoCount = PoCount,
                        PoTitle = item.PO_Title,
                        OrderType = item.OrderType,
                        VendorName = item.Vendor_Owner,
                        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                        POStatus = item.ReleaseCode2Status,
                        ProjectCodes = item.ProjectCode,
                        SubmitterName = item.SubmitterName,
                        WbsHeads = item.FundCenter_Description,
                        ManagerName = item.Managed_by,
                        ProjectShortName = item.ProjectShortName,
                        Version = "R" + item.Version
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
            if (result.Count > 0)
            {
                foreach (TEPurchaseHomeModel purHome in result)
                {
                    if (purHome.Approvers.Count > 0)
                    {
                        var NextApprover = (from u in db.TEPOApprovers
                                            where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Approved" && u.SequenceNumber != 0 && u.IsDeleted == false)
                                            orderby u.SequenceNumber
                                            select u).FirstOrDefault();
                        if (NextApprover != null)
                        {
                            if (UserId == NextApprover.ApproverId)
                                purHome.isCurrentApprover = true;
                            else
                                purHome.isCurrentApprover = false;
                        }
                        else
                            purHome.isCurrentApprover = false;
                    }
                    fResult.Add(purHome);
                }
            }
            if (fResult.Count > 0)
            {
                hrm.Content = new JsonContent(new
                {
                    result = fResult,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseDraftList_Pagination_Swamy(JObject json)
        {
            int UserId = 0; string FilterBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            if (json["FilterBy"] != null)
                FilterBy = json["FilterBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> fResult = new List<TEPurchaseHomeModel>();
            try
            {
                int PoCount = 0;
                var query = (from header in db.TEPOHeaderStructures
                             join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                             join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                             join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId into tempvendordtl
                             from vndrdtl in tempvendordtl.DefaultIfEmpty()
                             join vendor in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendor.POVendorMasterId into tempvendormstr
                             from vndrMstr in tempvendormstr.DefaultIfEmpty()
                             join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                             from proj in tempprj.DefaultIfEmpty()
                             join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                             from finalorder in temporder.DefaultIfEmpty()
                             join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                             from mnger in tempmgr.DefaultIfEmpty()
                                 // join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                             where (header.IsDeleted == false && header.IsNewPO == true
                             && appr.ApproverId == UserId && appr.IsDeleted == false
                             && (appr.Status == "Draft" || appr.Status == "Pending For Approval" || appr.Status == "Approved")
                             && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval"))
                             select new
                             {
                                 header.Purchasing_Order_Number,
                                 header.Vendor_Account_Number,
                                 header.Purchasing_Document_Date,
                                 header.path,
                                 header.ReleaseCode2Status,
                                 header.ReleaseCode2Date,
                                 header.Version,
                                 fund.FundCenter_Description,
                                 header.Currency_Key,
                                 Vendor_Owner = vndrMstr.VendorName,
                                 header.CreatedBy,
                                 header.CreatedOn,
                                 header.Uniqueid,
                                 header.PO_Title,
                                 OrderType = finalorder.Description,
                                 header.SubmitterName,
                                 Managed_by = mnger.CallName,
                                 header.POManagerID,
                                 proj.ProjectCode,
                                 proj.ProjectName,
                                 header.PurchaseRequestId,
                                 proj.ProjectShortName
                             }).Distinct().OrderByDescending(a => a.CreatedOn).ToList();

                PoCount = query.Count;
                var finalResult = query;
                if (PoCount > 0)
                {
                    if (pagenumber >= 0 && pagepercount > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        finalResult = query.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = finalResult.Count + start;
                        sinfo.totalrecords = PoCount;
                        sinfo.listcount = finalResult.Count;
                        sinfo.pages = pagenumber.ToString();
                    }
                    else
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = 1;
                        sinfo.torecords = 10;
                        sinfo.totalrecords = 0;
                        sinfo.listcount = 0;
                        sinfo.pages = "0";

                        hrm.Content = new JsonContent(new
                        {
                            result = query, //error
                            info = sinfo //return exception
                        });
                    }
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";

                    hrm.Content = new JsonContent(new
                    {
                        info = sinfo
                    });
                }
                foreach (var item in finalResult)
                {
                    DateTime? releasedate = null;
                    double? TotalPrice = 0.00;
                    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();


                    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                            ).ToList();
                    if (ItemStructure.Count > 0)
                    {
                        TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount.Value));
                    }

                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = item.ReleaseCode2Status,
                        Purchasing_Release_Date = releasedate,
                        Amount = TotalPrice,
                        PurchaseRequestId = item.PurchaseRequestId,
                        Currency_Key = item.Currency_Key,
                        HeaderUniqueid = item.Uniqueid,
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy,
                        OrderType = item.OrderType,
                        PoCount = PoCount,
                        PoTitle = item.PO_Title,
                        VendorName = item.Vendor_Owner,
                        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                        POStatus = item.ReleaseCode2Status,
                        ProjectCodes = item.ProjectCode,
                        SubmitterName = item.SubmitterName,
                        WbsHeads = item.FundCenter_Description,
                        ManagerName = item.Managed_by,
                        ProjectShortName = item.ProjectShortName,
                        Version = "R" + item.Version
                    });
                }

            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(
                    new ApplicationErrorLog
                    {
                        Error = ex.Message,
                        ExceptionDateTime = System.DateTime.Now,
                        InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                        Source = "From TEPurchaseHome API | TEPurchaseHome | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            if (result.Count > 0)
            {
                foreach (TEPurchaseHomeModel purHome in result)
                {
                    if (purHome.Approvers.Count > 0)
                    {
                        var NextApprover = (from u in db.TEPOApprovers
                                            where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval"
                                            //&& u.SequenceNumber != 0 
                                            && u.IsDeleted == false)
                                            orderby u.SequenceNumber
                                            select u).FirstOrDefault();
                        if (NextApprover != null)
                        {
                            if (UserId == NextApprover.ApproverId)
                                purHome.isCurrentApprover = true;
                            else
                                purHome.isCurrentApprover = false;
                        }
                        else
                            purHome.isCurrentApprover = false;
                    }
                    fResult.Add(purHome);
                }
            }
            if (fResult.Count > 0)
            {
                hrm.Content = new JsonContent(new
                {
                    result = fResult,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseSearch_Pagination(JObject json)
        {
            int UserId = 0, PoCount = 0; string Status = string.Empty;
            string SearchKey = string.Empty;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            if (json["Search"] != null)
                SearchKey = json["Search"].ToObject<string>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            try
            {
                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                 join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                 where userrole.UserId == UserId && webrole.RoleName.ToLower() == "po access"
                                 select new { webrole.RoleName }).FirstOrDefault();

                if(isPoAccess == null)
                {
                    finfo.errorcode = 0;
                    finfo.errormessage = "Don't have PO Access";
                    finfo.listcount = 0;
                    hrm.Content = new JsonContent(new
                    {
                        result = result,
                        info = finfo
                    });
                    return hrm;
                }

                List<TEPurchaseHomeModel> posearchList = new List<TEPurchaseHomeModel>();
                var PoadminroleCheck = (from userrole in db.webpages_UsersInRoles
                                        join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                        where userrole.UserId == UserId && webrole.RoleName.ToLower() == "po admin"
                                        select new { webrole.RoleName }).FirstOrDefault();

                //Returning all the PO informations because he is Having PO Admin Role
                if (PoadminroleCheck != null)
                {

                    posearchList = (from header in db.TEPOHeaderStructures
                                    join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
                                    from lftitem in g.DefaultIfEmpty()
                                    join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                    join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                    join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                    join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                                    from proj in tempprj.DefaultIfEmpty()
                                    join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                    from finalorder in temporder.DefaultIfEmpty()
                                    join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                    from mnger in tempmgr.DefaultIfEmpty()
                                    //join role in db.webpages_UsersInRoles on 
                                        //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                                    where (header.IsDeleted == false && header.Status == "Active"
                                    &&
                                    (
                                    lftitem.Material_Number.Contains(SearchKey) ||
                                    lftitem.Short_Text.Contains(SearchKey) ||
                                    lftitem.Long_Text.Contains(SearchKey) ||
                                    lftitem.Level1.Contains(SearchKey) ||
                                    lftitem.Level2.Contains(SearchKey) ||
                                    lftitem.Level3.Contains(SearchKey) ||
                                    lftitem.Level4.Contains(SearchKey) ||
                                    header.Purchasing_Order_Number.Contains(SearchKey) ||
                                    header.Vendor_Account_Number.Contains(SearchKey) ||
                                    header.PO_Title.Contains(SearchKey) ||
                                    header.Status.Contains(SearchKey) ||
                                         header.Uniqueid.ToString().Contains(SearchKey) ||
                                         header.PurchaseRequestId.ToString().Contains(SearchKey) ||
                                    header.PO_Title.Contains(SearchKey) ||
                                    finalorder.Description.Contains(SearchKey) ||
                                    header.SubmitterName.Contains(SearchKey) ||
                                    mnger.CallName.Contains(SearchKey) ||
                                    proj.ProjectCode.Contains(SearchKey) ||
                                    fund.FundCenter_Description.Contains(SearchKey) ||
                                    vendor.VendorName.Contains(SearchKey)
                                   )
                                    )
                                    orderby header.Uniqueid descending
                                    select new TEPurchaseHomeModel
                                    {
                                        Purchasing_Order_Number = header.Purchasing_Order_Number,
                                        Vendor_Account_Number = header.Vendor_Account_Number,
                                        Purchasing_Document_Date = header.Purchasing_Document_Date,
                                        Path = header.path,
                                        ReleaseCodeStatus = header.ReleaseCode2Status,
                                        Purchasing_Release_Date = header.ReleaseCode2Date,
                                        Version = header.Version,
                                        IsNewPO = header.IsNewPO,
                                        FundCenter_Description = fund.FundCenter_Description,
                                        Currency_Key = header.Currency_Key,
                                        VendorName = vendor.VendorName,
                                        PurchaseRequestId = header.PurchaseRequestId,
                                        HeaderUniqueid = header.Uniqueid,
                                        PoTitle = header.PO_Title,
                                        SubmitterName = header.SubmitterName,
                                        ManagerName = mnger.CallName,
                                        OrderType = finalorder.Description,
                                        ProjectCodes = proj.ProjectCode,
                                        ProjectName = proj.ProjectName,
                                        ProjectShortName = proj.ProjectShortName,
                                        Fugue_Purchasing_Order_Number = header.Fugue_Purchasing_Order_Number,
                                        CreatedOn = header.CreatedOn
                                    }).Distinct().OrderByDescending(a => a.CreatedOn).ToList()
                                  .GroupBy(a => a.Fugue_Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();

                    foreach (var item in posearchList)
                    {
                        string releasestatus = "";
                        DateTime? releasedate = null;
                        releasestatus = Status;
                        double? TotalPrice = 0.00;
                        DateTime GSTDate = new DateTime(2017, 07, 03);
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();


                        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false)
                                                                ).ToList();
                        if (ItemStructure.Count > 0)
                        {
                            TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount));
                        }

                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                            Path = item.Path,
                            ReleaseCodeStatus = item.ReleaseCodeStatus,
                            Purchasing_Release_Date = releasedate,
                            Amount = TotalPrice,
                            PurchaseRequestId = item.PurchaseRequestId,
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid = item.HeaderUniqueid,
                            PoCount = PoCount,
                            OrderType = item.OrderType,
                            PoTitle = item.PoTitle,
                            IsNewPO = item.IsNewPO,
                            VendorName = item.VendorName,
                            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false).ToList(),
                            POStatus = item.ReleaseCodeStatus,
                            ProjectCodes = item.ProjectCodes,
                            SubmitterName = item.SubmitterName,
                            WbsHeads = item.FundCenter_Description,
                            ManagerName = item.ManagerName,
                            ProjectShortName = item.ProjectShortName,
                            CreatedOn = item.CreatedOn,
                            Version = "R" + item.Version
                        });
                    }
                }
                //Returning PO informations based on login user associated to the POs as a approver
                else
                {
                    posearchList = (from header in db.TEPOHeaderStructures
                                    join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
                                    from lftitem in g.DefaultIfEmpty()
                                    join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                    join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                    join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                    join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                                    from proj in tempprj.DefaultIfEmpty()
                                    join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                    from finalorder in temporder.DefaultIfEmpty()
                                    join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                    from mnger in tempmgr.DefaultIfEmpty()
                                        //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                                    where (header.IsDeleted == false && header.Status == "Active"
                                    &&
                                    (
                                    lftitem.Material_Number.Contains(SearchKey) ||
                                    lftitem.Short_Text.Contains(SearchKey) ||
                                    lftitem.Long_Text.Contains(SearchKey) ||
                                    lftitem.WBSElementCode.Contains(SearchKey) ||
                                    lftitem.Level1.Contains(SearchKey) ||
                                    lftitem.Level2.Contains(SearchKey) ||
                                    lftitem.Level3.Contains(SearchKey) ||
                                    lftitem.Level4.Contains(SearchKey) ||
                                    header.Purchasing_Order_Number.Contains(SearchKey) ||
                                    header.Vendor_Account_Number.Contains(SearchKey) ||
                                    finalorder.Description.Contains(SearchKey) ||
                                    header.PO_Title.Contains(SearchKey) ||
                                    header.SubmitterName.Contains(SearchKey) ||
                                    header.Managed_by.Contains(SearchKey) ||
                                    proj.ProjectCode.Contains(SearchKey) ||
                                    fund.FundCenter_Description.Contains(SearchKey) ||
                                    header.Uniqueid.ToString().Contains(SearchKey) ||
                                    vendor.VendorName.Contains(SearchKey)
                                   )
                                    )
                                    orderby header.Uniqueid descending
                                    select new TEPurchaseHomeModel
                                    {
                                        Purchasing_Order_Number = header.Purchasing_Order_Number,
                                        Vendor_Account_Number = header.Vendor_Account_Number,
                                        Purchasing_Document_Date = header.Purchasing_Document_Date,
                                        Path = header.path,
                                        ReleaseCodeStatus = header.ReleaseCode2Status,
                                        Purchasing_Release_Date = header.ReleaseCode2Date,
                                        Version = header.Version,
                                        IsNewPO = header.IsNewPO,
                                        FundCenter_Description = fund.FundCenter_Description,
                                        Currency_Key = header.Currency_Key,
                                        VendorName = vendor.VendorName,
                                        HeaderUniqueid = header.Uniqueid,
                                        PoTitle = header.PO_Title,
                                        OrderType = finalorder.Description,
                                        SubmitterName = header.SubmitterName,
                                        ManagerName = mnger.CallName,
                                        PurchaseRequestId = header.PurchaseRequestId,
                                        ProjectCodes = proj.ProjectCode,
                                        ProjectName = proj.ProjectName,
                                        ProjectShortName = proj.ProjectShortName,
                                        Fugue_Purchasing_Order_Number = header.Fugue_Purchasing_Order_Number,
                                        CreatedOn = header.CreatedOn
                                    }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList()
                                  .GroupBy(a => a.Fugue_Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();

                    foreach (var item in posearchList)
                    {
                        //var approveCheckingUser = (from apr in db.TEPOApprovers
                        //                           where apr.IsDeleted == false && apr.POStructureId == item.HeaderUniqueid && apr.ApproverId == UserId
                        //                           select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).FirstOrDefault();
                        //if (approveCheckingUser != null)
                        //{
                            string releasestatus = "";
                            DateTime? releasedate = null;
                            releasestatus = Status;
                            double? TotalPrice = 0.00;
                            DateTime GSTDate = new DateTime(2017, 07, 03);
                            DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                            List<string> WbsList = new List<string>();
                            List<string> WbsHeadsList = new List<string>();

                            var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false)
                                                                    ).ToList();
                            if (ItemStructure.Count > 0)
                            {
                                TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.GrossAmount));
                            }

                            result.Add(new TEPurchaseHomeModel()
                            {
                                Purchasing_Order_Number = item.Purchasing_Order_Number,
                                Vendor_Account_Number = item.Vendor_Account_Number,
                                Purchasing_Document_Date = item.Purchasing_Document_Date,
                                Path = item.Path,
                                ReleaseCodeStatus = item.ReleaseCodeStatus,
                                Purchasing_Release_Date = releasedate,
                                Amount = TotalPrice,
                                OrderType = item.OrderType,
                                Currency_Key = item.Currency_Key,
                                HeaderUniqueid = item.HeaderUniqueid,
                                PoCount = PoCount,
                                PurchaseRequestId = item.PurchaseRequestId,
                                PoTitle = item.PoTitle,
                                IsNewPO = item.IsNewPO,
                                VendorName = item.VendorName,
                                Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false).ToList(),
                                POStatus = item.ReleaseCodeStatus,
                                ProjectCodes = item.ProjectCodes,
                                SubmitterName = item.SubmitterName,
                                WbsHeads = item.FundCenter_Description,
                                ManagerName = item.ManagerName,
                                ProjectShortName = item.ProjectShortName,
                                CreatedOn = item.CreatedOn,
                                Version = "R" + item.Version
                            });
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
            if (result.Count > 0)
            {
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseApproved_Pagination(JObject json)
        {
            int UserId = 0; string FilterBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            if (json["FilterBy"] != null)
                FilterBy = json["FilterBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            System.Web.HttpContext.Current.Server.ScriptTimeout = 90000;
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> poApprovedPaginationSearchList = new List<TEPurchaseHomeModel>();
            try
            {
                int PoCount = 0;
                if (!string.IsNullOrEmpty(FilterBy))
                {
                    List<TEPurchaseHomeModel> poDraftSearchList = new List<TEPurchaseHomeModel>();
                    poDraftSearchList = (from header in db.TEPOHeaderStructures
                                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                                         join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
                                         from lftitem in g.DefaultIfEmpty()
                                         join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                         join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                         join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                         join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                         from finalorder in temporder.DefaultIfEmpty()
                                         join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                         from mnger in tempmgr.DefaultIfEmpty()
                                         where (header.IsDeleted == false && header.Status == "Active"
                                       && appr.ApproverId == UserId
                                                       && appr.IsDeleted == false && header.ReleaseCode2Status == "Approved"
                                                       &&
                                                       (
                                                       lftitem.Material_Number.Contains(FilterBy) ||
                                                       lftitem.Short_Text.Contains(FilterBy) ||
                                                       lftitem.Long_Text.Contains(FilterBy) ||
                                                       lftitem.Level1.Contains(FilterBy) ||
                                                       lftitem.Level2.Contains(FilterBy) ||
                                                       lftitem.Level3.Contains(FilterBy) ||
                                                       lftitem.Level4.Contains(FilterBy) ||
                                                       header.Purchasing_Order_Number.Contains(FilterBy) ||
                                                       header.Vendor_Account_Number.Contains(FilterBy) ||
                                                       header.PO_Title.Contains(FilterBy) ||
                                                       header.Status.Contains(FilterBy) ||
                                                            header.Uniqueid.ToString().Contains(FilterBy) ||
                                                            header.PurchaseRequestId.ToString().Contains(FilterBy) ||
                                                       header.PO_Title.Contains(FilterBy) ||
                                                       finalorder.Description.Contains(FilterBy) ||
                                                       header.SubmitterName.Contains(FilterBy) ||
                                                       mnger.CallName.Contains(FilterBy) ||
                                                       fund.ProjectCode.Contains(FilterBy) ||
                                                       fund.FundCenter_Description.Contains(FilterBy) ||
                                                       vendor.VendorName.Contains(FilterBy)
                                                      )
                                                       )
                                         orderby header.Uniqueid descending
                                         select new TEPurchaseHomeModel
                                         {
                                             Purchasing_Order_Number = header.Purchasing_Order_Number,
                                             Vendor_Account_Number = header.Vendor_Account_Number,
                                             Purchasing_Document_Date = header.Purchasing_Document_Date,
                                             Path = header.path,
                                             CreatedBy = header.CreatedBy,
                                             ReleaseCodeStatus = header.ReleaseCode2Status,
                                             POStatus = header.ReleaseCode2Status,
                                             Purchasing_Release_Date = header.ReleaseCode2Date,
                                             Version = header.Version,
                                             FundCenter_Description = fund.FundCenter_Description,
                                             Currency_Key = header.Currency_Key,
                                             VendorName = vendor.VendorName,
                                             HeaderUniqueid = header.Uniqueid,
                                             PoTitle = header.PO_Title,
                                             OrderType = finalorder.Description,
                                             SubmitterName = header.SubmitterName,
                                             ManagerName = mnger.CallName,
                                             PurchaseRequestId = header.PurchaseRequestId,
                                             ProjectCodes = fund.ProjectCode,
                                             ProjectName = fund.ProjectName,
                                             Fugue_Purchasing_Order_Number = header.Fugue_Purchasing_Order_Number,
                                             CreatedOn = header.CreatedOn,
                                             IsNewPO = header.IsNewPO,
                                         }).Distinct().OrderByDescending(a => a.CreatedOn).ToList()
                                  .GroupBy(a => a.Fugue_Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();
                    #region
                    //(from header in db.TEPOHeaderStructures
                    //                     join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                    //                     //FundCentre Mapping of PO Managers
                    //                     join FundMap in db.TEPOFundCenterUserMappings on header.FundCenterID equals FundMap.FundCenterId
                    //                     join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
                    //                     from lftitem in g.DefaultIfEmpty()
                    //                     join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                    //                     join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                    //                     join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                    //                     join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                    //                     from finalorder in temporder.DefaultIfEmpty()
                    //                     join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                    //                     from mnger in tempmgr.DefaultIfEmpty()
                    //                     where (header.IsDeleted == false && header.Status == "Active"
                    //                     && (appr.ApproverId == UserId || (FundMap.UserId == UserId && FundMap.IsDeleted == false)) 

                    //                    && appr.IsDeleted == false
                    //                    //&& appr.Status == "Approved"
                    //                    && header.ReleaseCode2Status == "Approved"
                    //                    &&
                    //                     (
                    //                     lftitem.Material_Number.Contains(FilterBy) ||
                    //                     lftitem.Short_Text.Contains(FilterBy) ||
                    //                     lftitem.Long_Text.Contains(FilterBy) ||
                    //                     lftitem.WBSElementCode.Contains(FilterBy) ||
                    //                     lftitem.Level1.Contains(FilterBy) ||
                    //                     lftitem.Level2.Contains(FilterBy) ||
                    //                     lftitem.Level3.Contains(FilterBy) ||
                    //                     lftitem.Level4.Contains(FilterBy) ||
                    //                     header.Purchasing_Order_Number.Contains(FilterBy) ||
                    //                     header.Vendor_Account_Number.Contains(FilterBy) ||
                    //                     header.Status.Contains(FilterBy) ||
                    //                     header.Uniqueid.ToString().Contains(FilterBy) ||
                    //                     header.PurchaseRequestId.ToString().Contains(FilterBy) ||
                    //                     finalorder.Description.Contains(FilterBy) ||
                    //                     header.PO_Title.Contains(FilterBy) ||
                    //                     header.SubmitterName.Contains(FilterBy) ||
                    //                     header.Managed_by.Contains(FilterBy) ||
                    //                     fund.ProjectCode.Contains(FilterBy) ||
                    //                     fund.FundCenter_Description.Contains(FilterBy) ||
                    //                     vendor.VendorName.Contains(FilterBy)
                    //                    )
                    //                     )
                    //                     orderby header.Uniqueid descending
                    //                     select new TEPurchaseHomeModel
                    //                     {
                    //                         Purchasing_Order_Number = header.Purchasing_Order_Number,
                    //                         Vendor_Account_Number = header.Vendor_Account_Number,
                    //                         Purchasing_Document_Date = header.Purchasing_Document_Date,
                    //                         Path = header.path,
                    //                         CreatedBy = header.CreatedBy,
                    //                         ReleaseCodeStatus = header.ReleaseCode2Status,
                    //                         POStatus = header.ReleaseCode2Status,
                    //                         Purchasing_Release_Date = header.ReleaseCode2Date,
                    //                         Version = header.Version,
                    //                         FundCenter_Description = fund.FundCenter_Description,
                    //                         Currency_Key = header.Currency_Key,
                    //                         VendorName = vendor.VendorName,
                    //                         HeaderUniqueid = header.Uniqueid,
                    //                         PoTitle = header.PO_Title,
                    //                         OrderType = finalorder.Description,
                    //                         SubmitterName = header.SubmitterName,
                    //                         ManagerName = mnger.CallName,
                    //                         PurchaseRequestId = header.PurchaseRequestId,
                    //                         ProjectCodes = fund.ProjectCode,
                    //                         ProjectName = fund.ProjectName,
                    //                         Fugue_Purchasing_Order_Number = header.Fugue_Purchasing_Order_Number,
                    //                         CreatedOn = header.CreatedOn,
                    //                         IsNewPO = header.IsNewPO,
                    //                     }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList()
                    //                 .GroupBy(a => a.Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();
                    #endregion
                    PoCount = poDraftSearchList.Count;
                    if (PoCount > 0)
                    {
                        if (pagenumber >= 0 && pagepercount > 0)
                        {
                            if (pagenumber == 0)
                            {
                                pagenumber = 1;
                            }
                            int iPageNum = pagenumber;
                            int iPageSize = pagepercount;
                            int start = iPageNum - 1;
                            start = start * iPageSize;
                            poApprovedPaginationSearchList = poDraftSearchList.Skip(start).Take(iPageSize).ToList();
                            sinfo.errorcode = 0;
                            sinfo.errormessage = "Success";
                            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                            sinfo.torecords = poApprovedPaginationSearchList.Count + start;
                            sinfo.totalrecords = PoCount;
                            sinfo.listcount = poApprovedPaginationSearchList.Count;
                            sinfo.pages = pagenumber.ToString();
                        }
                    }
                    foreach (var item in poApprovedPaginationSearchList)
                    {
                        var approveCheckingUser = (from apr in db.TEPOApprovers
                                                   where apr.IsDeleted == false && apr.POStructureId == item.HeaderUniqueid && apr.ApproverId == UserId
                                                   select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).FirstOrDefault();
                        var fundUserMap = db.TEPOFundCenterUserMappings.Where(x => x.IsDeleted == false && x.UserId == UserId).FirstOrDefault();
                        if (approveCheckingUser != null /*|| fundUserMap != null*/)
                        {
                            DateTime? releasedate = null;
                            double? TotalPrice = 0.00;
                            DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                            List<string> WbsList = new List<string>();
                            List<string> WbsHeadsList = new List<string>();
                            bool isRevisionAllowed = true;

                            var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false)
                                                                    ).ToList();
                            //if (item.IsNewPO == false)
                            //{
                            //var serviceOrd = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false && x.ItemType== "ServiceOrder")
                            //                                        ).ToList();
                            //var serviceOrd = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false && x.ItemType == "ServiceOrder")
                            //                                        ).ToList();
                            //if (serviceOrd.Count > 0)
                            //{
                            //    isRevisionAllowed = false;
                            //}

                            //}
                            if (ItemStructure.Count > 0)
                            {
                                TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.GrossAmount));
                            }

                            result.Add(new TEPurchaseHomeModel()
                            {
                                Purchasing_Order_Number = item.Purchasing_Order_Number,
                                Vendor_Account_Number = item.Vendor_Account_Number,
                                Purchasing_Document_Date = item.Purchasing_Document_Date,
                                Path = item.Path,
                                ReleaseCodeStatus = item.ReleaseCodeStatus,
                                Purchasing_Release_Date = releasedate,
                                Amount = TotalPrice,
                                OrderType = item.OrderType,
                                Currency_Key = item.Currency_Key,
                                HeaderUniqueid = item.HeaderUniqueid,
                                PoCount = PoCount,
                                PurchaseRequestId = item.PurchaseRequestId,
                                PoTitle = item.PoTitle,
                                IsNewPO = item.IsNewPO,
                                CreatedBy = item.CreatedBy,
                                VendorName = item.VendorName,
                                Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false).ToList(),
                                POStatus = item.ReleaseCodeStatus,
                                ProjectCodes = item.ProjectCodes,
                                SubmitterName = item.SubmitterName,
                                WbsHeads = item.FundCenter_Description,
                                ManagerName = item.ManagerName,
                                ProjectShortName = item.ProjectShortName,
                                CreatedOn = item.CreatedOn,
                                Version = "R" + item.Version,
                                isRevisionAllowed = isRevisionAllowed
                            });
                        }
                    }
                    if (result.Count > 0)
                    {
                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = sinfo
                        });
                    }
                    else
                    {
                        finfo.errorcode = 0;
                        finfo.errormessage = "No Records";
                        finfo.listcount = 0;
                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = finfo
                        });
                    }
                    return hrm;
                }
                else
                    {
                        var query = (from header in db.TEPOHeaderStructures
                                     join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                                     join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                     join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId into tempvendordtl
                                     from vndrdtl in tempvendordtl.DefaultIfEmpty()
                                     join vendor in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendor.POVendorMasterId into tempvendormstr
                                     from vndrMstr in tempvendormstr.DefaultIfEmpty()
                                     join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                     from finalorder in temporder.DefaultIfEmpty()
                                     join fundmap in db.TEPOFundCenterUserMappings on header.FundCenterID equals fundmap.FundCenterId into fundm
                                     from usermap in fundm.DefaultIfEmpty()
                                     join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                     from mnger in tempmgr.DefaultIfEmpty()
                                     where (header.IsDeleted == false && header.Status == "Active"
                                            && (appr.ApproverId == UserId || (usermap.UserId == UserId && usermap.IsDeleted == false)) && appr.IsDeleted == false
                                            //&& appr.Status == "Approved"
                                            && header.ReleaseCode2Status == "Approved")
                                     select new
                                     {
                                         header.Purchasing_Order_Number,
                                         header.Vendor_Account_Number,
                                         header.Purchasing_Document_Date,
                                         header.path,
                                         header.ReleaseCode2Status,
                                         header.ReleaseCode2Date,
                                         header.Version,
                                         fund.FundCenter_Description,
                                         header.Currency_Key,
                                         Vendor_Owner = vndrMstr.VendorName,
                                         header.CreatedBy,
                                         header.CreatedOn,
                                         header.Uniqueid,
                                         header.PO_Title,
                                         IsNewPO = header.IsNewPO,
                                         OrderType = finalorder.Description,
                                         header.SubmitterName,
                                         Managed_by = mnger.CallName,
                                         header.POManagerID,
                                         fund.ProjectCode,
                                         fund.ProjectName,
                                         header.PurchaseRequestId
                                     }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList()
                                     .GroupBy(a => a.Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();

                        PoCount = query.Count;
                        var finalResult = query;
                        if (PoCount > 0)
                        {
                            if (pagenumber >= 0 && pagepercount > 0)
                            {
                                if (pagenumber == 0)
                                {
                                    pagenumber = 1;
                                }
                                int iPageNum = pagenumber;
                                int iPageSize = pagepercount;
                                int start = iPageNum - 1;
                                start = start * iPageSize;
                                finalResult = query.Skip(start).Take(iPageSize).ToList();
                                sinfo.errorcode = 0;
                                sinfo.errormessage = "Success";
                                sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                                sinfo.torecords = finalResult.Count + start;
                                sinfo.totalrecords = PoCount;
                                sinfo.listcount = finalResult.Count;
                                sinfo.pages = pagenumber.ToString();
                            }
                        }
                        foreach (var item in finalResult)
                        {
                            DateTime? releasedate = null;
                            double? TotalPrice = 0.00;
                            bool isRevisionAllowed = true;
                            DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                            var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                    ).ToList();
                            if (ItemStructure.Count > 0)
                            {
                                TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount.Value));
                            }
                            if (item.IsNewPO == false)
                            {
                                var serviceOrd = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false
                                                                            && x.ItemType == "ServiceOrder")).ToList();
                                if (serviceOrd.Count > 0)
                                {
                                    isRevisionAllowed = false;
                                }
                            }
                            result.Add(new TEPurchaseHomeModel()
                            {
                                Purchasing_Order_Number = item.Purchasing_Order_Number,
                                Vendor_Account_Number = item.Vendor_Account_Number,
                                Purchasing_Document_Date = item.Purchasing_Document_Date,
                                Path = item.path,
                                ReleaseCodeStatus = item.ReleaseCode2Status,
                                Purchasing_Release_Date = releasedate,
                                Amount = TotalPrice,
                                PurchaseRequestId = item.PurchaseRequestId,
                                Currency_Key = item.Currency_Key,
                                HeaderUniqueid = item.Uniqueid,
                                CreatedOn = item.CreatedOn,
                                CreatedBy = item.CreatedBy,
                                OrderType = item.OrderType,
                                PoCount = PoCount,
                                IsNewPO = item.IsNewPO,
                                PoTitle = item.PO_Title,
                                VendorName = item.Vendor_Owner,
                                Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                                POStatus = item.ReleaseCode2Status,
                                ProjectCodes = item.ProjectCode,
                                SubmitterName = item.SubmitterName,
                                WbsHeads = item.FundCenter_Description,
                                ManagerName = item.Managed_by,
                                Version = "R" + item.Version,
                                isRevisionAllowed = isRevisionAllowed
                            });
                        }
                    }
                    if (result.Count > 0)
                    {
                        foreach (TEPurchaseHomeModel purHome in result)
                        {
                            if (purHome.Approvers.Count > 0)
                            {
                                var NextApprover = (from u in db.TEPOApprovers
                                                    where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval"
                                                    && u.IsDeleted == false)
                                                    orderby u.SequenceNumber
                                                    select u).FirstOrDefault();
                                if (NextApprover != null)
                                {
                                    if (UserId == NextApprover.ApproverId)
                                        purHome.isCurrentApprover = true;
                                    else
                                        purHome.isCurrentApprover = false;
                                }
                                else
                                    purHome.isCurrentApprover = false;
                            }
                            // result.Add(purHome);
                        }
                    }
                    if (result.Count > 0)
                    {
                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = sinfo
                        });
                    }
                    else
                    {
                        finfo.errorcode = 0;
                        finfo.errormessage = "No Records";
                        finfo.listcount = 0;
                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = finfo
                        });
                    }
                    return hrm;
                }
                catch (Exception ex)
                {
                    ExceptionObj.RecordUnHandledException(ex);
                    finfo.errorcode = 0;
                    finfo.errormessage = "Fail to Get Data";
                    finfo.listcount = 0;
                    hrm.Content = new JsonContent(new
                    {
                        result = result,
                        info = finfo
                    });
                    return hrm;
                }
            }

        [HttpPost]
        public HttpResponseMessage TEPurchaseDraftList_Pagination(JObject json)
        {
            int UserId = 0; string FilterBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            if (json["FilterBy"] != null)
                FilterBy = json["FilterBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> poDraftPaginationSearchList = new List<TEPurchaseHomeModel>();
            try
            {
                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == UserId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    finfo.errorcode = 0;
                    finfo.errormessage = "Don't have PO Access";
                    finfo.listcount = 0;
                    hrm.Content = new JsonContent(new
                    {
                        result = result,
                        info = finfo
                    });
                    return hrm;
                }

                //join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                //                         from proj in tempprj.DefaultIfEmpty()
                int PoCount = 0;
                if (!string.IsNullOrEmpty(FilterBy))
                {
                    List<TEPurchaseHomeModel> poDraftSearchList = new List<TEPurchaseHomeModel>();
                    poDraftSearchList = (from header in db.TEPOHeaderStructures
                                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                                         join item in db.TEPOItemStructures on header.Uniqueid equals item.POStructureId into g
                                         from lftitem in g.DefaultIfEmpty()
                                         join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                         join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                         join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                         join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                         from finalorder in temporder.DefaultIfEmpty()
                                         join fundmap in db.TEPOFundCenterUserMappings on header.FundCenterID equals fundmap.FundCenterId into fundm
                                         from usermap in fundm.DefaultIfEmpty()
                                         join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                         from mnger in tempmgr.DefaultIfEmpty()
                                         where (header.IsDeleted == false && (appr.ApproverId == UserId || (usermap.UserId == UserId && usermap.IsDeleted == false))
                                         && appr.IsDeleted == false && (appr.Status == "Draft" || appr.Status == "Pending For Approval" || appr.Status == "Approved")
                                         && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval")
                                         && header.Status == "Active"
                                         &&
                                         (
                                         lftitem.Material_Number.Contains(FilterBy) ||
                                         lftitem.Short_Text.Contains(FilterBy) ||
                                         lftitem.Long_Text.Contains(FilterBy) ||
                                         lftitem.WBSElementCode.Contains(FilterBy) ||
                                         lftitem.Level1.Contains(FilterBy) ||
                                         lftitem.Level2.Contains(FilterBy) ||
                                         lftitem.Level3.Contains(FilterBy) ||
                                         lftitem.Level4.Contains(FilterBy) ||
                                         header.Purchasing_Order_Number.Contains(FilterBy) ||
                                         header.Vendor_Account_Number.Contains(FilterBy) ||
                                         header.Status.Contains(FilterBy) ||
                                         header.Uniqueid.ToString().Contains(FilterBy) ||
                                         header.PurchaseRequestId.ToString().Contains(FilterBy) ||
                                         finalorder.Description.Contains(FilterBy) ||
                                         header.PO_Title.Contains(FilterBy) ||
                                         header.SubmitterName.Contains(FilterBy) ||
                                         header.Managed_by.Contains(FilterBy) ||
                                         fund.ProjectCode.Contains(FilterBy) ||
                                         fund.FundCenter_Description.Contains(FilterBy) ||
                                         vendor.VendorName.Contains(FilterBy)
                                        )
                                         )
                                         orderby header.Uniqueid descending
                                         select new TEPurchaseHomeModel
                                         {
                                             Purchasing_Order_Number = header.Purchasing_Order_Number,
                                             Vendor_Account_Number = header.Vendor_Account_Number,
                                             Purchasing_Document_Date = header.Purchasing_Document_Date,
                                             Path = header.path,
                                             CreatedBy = header.CreatedBy,
                                             ReleaseCodeStatus = header.ReleaseCode2Status,
                                             POStatus = header.ReleaseCode2Status,
                                             Purchasing_Release_Date = header.ReleaseCode2Date,
                                             Version = header.Version,
                                             FundCenter_Description = fund.FundCenter_Description,
                                             WbsHeads = fund.FundCenter_Description,
                                             Currency_Key = header.Currency_Key,
                                             VendorName = vendor.VendorName,
                                             HeaderUniqueid = header.Uniqueid,
                                             IsNewPO = header.IsNewPO,
                                             PoTitle = header.PO_Title,
                                             OrderType = finalorder.Description,
                                             SubmitterName = header.SubmitterName,
                                             ManagerName = mnger.CallName,
                                             PurchaseRequestId = header.PurchaseRequestId,
                                             ProjectCodes = fund.ProjectCode,
                                             ProjectName = fund.ProjectName,
                                             //ProjectShortName = proj.ProjectShortName,
                                             Fugue_Purchasing_Order_Number = header.Fugue_Purchasing_Order_Number,
                                             CreatedOn = header.CreatedOn,
                                             LastModifiedOn = header.LastModifiedOn
                                         }).Distinct().OrderByDescending(a => a.CreatedOn).ToList();
                    //.GroupBy(a => a.Fugue_Purchasing_Order_Number).Select(b => b.OrderByDescending(y => y.Version).First()).ToList();
                    PoCount = poDraftSearchList.Count;
                    if (PoCount > 0)
                    {
                        if (pagenumber >= 0 && pagepercount > 0)
                        {
                            if (pagenumber == 0)
                            {
                                pagenumber = 1;
                            }
                            int iPageNum = pagenumber;
                            int iPageSize = pagepercount;
                            int start = iPageNum - 1;
                            start = start * iPageSize;
                            poDraftPaginationSearchList = poDraftSearchList.Skip(start).Take(iPageSize).ToList();
                            sinfo.errorcode = 0;
                            sinfo.errormessage = "Success";
                            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                            sinfo.torecords = poDraftPaginationSearchList.Count + start;
                            sinfo.totalrecords = PoCount;
                            sinfo.listcount = poDraftPaginationSearchList.Count;
                            sinfo.pages = pagenumber.ToString();
                        }
                    }
                    foreach (var item in poDraftPaginationSearchList)
                    {
                        var approveCheckingUser = (from apr in db.TEPOApprovers
                                                   where apr.IsDeleted == false && apr.POStructureId == item.HeaderUniqueid && apr.ApproverId == UserId
                                                   select new { apr.ApproverId, apr.ApproverName, apr.Status, apr.SequenceNumber }).FirstOrDefault();
                        var fundUserMap = db.TEPOFundCenterUserMappings.Where(x => x.IsDeleted == false && x.UserId == UserId).FirstOrDefault();
                        if (approveCheckingUser != null || fundUserMap != null)
                        {
                            DateTime? releasedate = null;
                            double? TotalPrice = 0.00;
                            DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                            var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false)
                                                                    ).ToList();
                            if (ItemStructure.Count > 0)
                            {
                                TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.GrossAmount));
                            }

                            result.Add(new TEPurchaseHomeModel()
                            {
                                Purchasing_Order_Number = item.Purchasing_Order_Number,
                                Vendor_Account_Number = item.Vendor_Account_Number,
                                Purchasing_Document_Date = item.Purchasing_Document_Date,
                                Path = item.Path,
                                ReleaseCodeStatus = item.ReleaseCodeStatus,
                                Purchasing_Release_Date = releasedate,
                                Amount = TotalPrice,
                                OrderType = item.OrderType,
                                IsNewPO = item.IsNewPO,
                                Currency_Key = item.Currency_Key,
                                HeaderUniqueid = item.HeaderUniqueid,
                                PoCount = PoCount,
                                PurchaseRequestId = item.PurchaseRequestId,
                                PoTitle = item.PoTitle,
                                VendorName = item.VendorName,
                                Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.HeaderUniqueid && x.IsDeleted == false).ToList(),
                                POStatus = item.ReleaseCodeStatus,
                                ProjectCodes = item.ProjectCodes,
                                SubmitterName = item.SubmitterName,
                                CreatedBy = item.CreatedBy,
                                WbsHeads = item.FundCenter_Description,
                                ManagerName = item.ManagerName,
                                ProjectShortName = item.ProjectShortName,
                                CreatedOn = item.CreatedOn,
                                Version = "R" + item.Version
                            });
                        }
                    }
                    if (result.Count > 0)
                    {
                        foreach (TEPurchaseHomeModel purHome in result)
                        {
                            if (purHome.Approvers.Count > 0)
                            {
                                var NextApprover = (from u in db.TEPOApprovers
                                                    where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval"
                                                    //&& u.SequenceNumber != 0 
                                                    && u.IsDeleted == false)
                                                    orderby u.SequenceNumber
                                                    select u).FirstOrDefault();
                                if (NextApprover != null)
                                {
                                    if (UserId == NextApprover.ApproverId)
                                        purHome.isCurrentApprover = true;
                                    else
                                        purHome.isCurrentApprover = false;
                                }
                                else
                                    purHome.isCurrentApprover = false;
                            }
                        }

                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = sinfo
                        });
                    }
                    else
                    {
                        finfo.errorcode = 0;
                        finfo.errormessage = "No Records";
                        finfo.listcount = 0;
                        hrm.Content = new JsonContent(new
                        {
                            result = result,
                            info = finfo
                        });
                    }
                    return hrm;
                }
                else
                {
                    var query = (from header in db.TEPOHeaderStructures
                                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                                 join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                 join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId into tempvendordtl
                                 from vndrdtl in tempvendordtl.DefaultIfEmpty()
                                 join vendor in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendor.POVendorMasterId into tempvendormstr
                                 from vndrMstr in tempvendormstr.DefaultIfEmpty()
                                 join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                 from finalorder in temporder.DefaultIfEmpty()
                                 join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                 from mnger in tempmgr.DefaultIfEmpty()
                                 join fundmap in db.TEPOFundCenterUserMappings on header.FundCenterID equals fundmap.FundCenterId into fundm
                                 from usermap in fundm.DefaultIfEmpty()
                                 where (header.IsDeleted == false && (appr.ApproverId == UserId || (usermap.UserId == UserId && usermap.IsDeleted == false)) && appr.IsDeleted == false
                                 && (appr.Status == "Draft" || appr.Status == "Pending For Approval" || appr.Status == "Approved")
                                 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending For Approval"))
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.Version,
                                     fund.FundCenter_Description,
                                     header.Currency_Key,
                                     IsNewPO = header.IsNewPO,
                                     Vendor_Owner = vndrMstr.VendorName,
                                     header.CreatedBy,
                                     header.CreatedOn,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     OrderType = finalorder.Description,
                                     header.SubmitterName,
                                     Managed_by = mnger.CallName,
                                     header.POManagerID,
                                     fund.ProjectCode,
                                     fund.ProjectName,
                                     header.PurchaseRequestId,
                                 }).Distinct().OrderByDescending(a => a.CreatedOn).ToList();

                    PoCount = query.Count;
                    var finalResult = query;
                    if (PoCount > 0)
                    {
                        if (pagenumber >= 0 && pagepercount > 0)
                        {
                            if (pagenumber == 0)
                            {
                                pagenumber = 1;
                            }
                            int iPageNum = pagenumber;
                            int iPageSize = pagepercount;
                            int start = iPageNum - 1;
                            start = start * iPageSize;
                            finalResult = query.Skip(start).Take(iPageSize).ToList();
                            sinfo.errorcode = 0;
                            sinfo.errormessage = "Success";
                            sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                            sinfo.torecords = finalResult.Count + start;
                            sinfo.totalrecords = PoCount;
                            sinfo.listcount = finalResult.Count;
                            sinfo.pages = pagenumber.ToString();
                        }
                    }
                    foreach (var item in finalResult)
                    {
                        DateTime? releasedate = null;
                        double? TotalPrice = 0.00;
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();


                        var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                ).ToList();
                        if (ItemStructure.Count > 0)
                        {
                            TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount.Value));
                        }
                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                            Path = item.path,
                            ReleaseCodeStatus = item.ReleaseCode2Status,
                            Purchasing_Release_Date = releasedate,
                            Amount = TotalPrice,
                            PurchaseRequestId = item.PurchaseRequestId,
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid = item.Uniqueid,
                            CreatedOn = item.CreatedOn,
                            CreatedBy = item.CreatedBy,
                            OrderType = item.OrderType,
                            PoCount = PoCount,
                            IsNewPO = item.IsNewPO,
                            PoTitle = item.PO_Title,
                            VendorName = item.Vendor_Owner,
                            Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                            POStatus = item.ReleaseCode2Status,
                            ProjectCodes = item.ProjectCode,
                            SubmitterName = item.SubmitterName,
                            WbsHeads = item.FundCenter_Description,
                            ManagerName = item.Managed_by,
                            Version = "R" + item.Version
                        });
                    }
                }

                if (result.Count > 0)
                {
                    foreach (TEPurchaseHomeModel purHome in result)
                    {
                        if (purHome.Approvers.Count > 0)
                        {
                            var NextApprover = (from u in db.TEPOApprovers
                                                where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval"
                                                //&& u.SequenceNumber != 0 
                                                && u.IsDeleted == false)
                                                orderby u.SequenceNumber
                                                select u).FirstOrDefault();
                            if (NextApprover != null)
                            {
                                if (UserId == NextApprover.ApproverId)
                                    purHome.isCurrentApprover = true;
                                else
                                    purHome.isCurrentApprover = false;
                            }
                            else
                                purHome.isCurrentApprover = false;
                        }
                    }
                    hrm.Content = new JsonContent(new
                    {
                        result = result,
                        info = sinfo
                    });
                }
                else
                {
                    finfo.errorcode = 0;
                    finfo.errormessage = "No Records";
                    finfo.listcount = 0;
                    hrm.Content = new JsonContent(new
                    {
                        result = result,
                        info = finfo
                    });
                }
                return hrm;
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                finfo.errorcode = 0;
                finfo.errormessage = "Fail to Get Data";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
                return hrm;
            }
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseDraftList_Pagination_Old(JObject json)
        {
            int UserId = 0; string FilterBy = string.Empty;
            int pagenumber = 0, pagepercount = 0;
            if (json["UserId"] != null)
                UserId = json["UserId"].ToObject<int>();
            if (json["FilterBy"] != null)
                FilterBy = json["FilterBy"].ToObject<string>();
            if (json["pageNumber"] != null)
                pagenumber = json["pageNumber"].ToObject<int>();
            if (json["pagePerCount"] != null)
                pagepercount = json["pagePerCount"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();

            int PoCount = 0;

            #region OldPO
            //UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
            //string user1 = "";
            //user1 = "Approver";
            //foreach (var item in profile.webpages_Roles)
            //{
            //    if (item.RoleName.Equals("PO  Approval Admin"))
            //    {
            //        user1 = "PO  Approval Admin";
            //        break;
            //    }
            //}


            //var query = (from header in db.TEPOHeaderStructures
            //             join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

            //             where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft"))

            //             select new
            //             {
            //                 header.Purchasing_Order_Number,
            //                 header.Vendor_Account_Number,
            //                 // vend.Vendor_Owner,
            //                 header.Purchasing_Document_Date,
            //                 header.path,
            //                 header.ReleaseCode2Status,
            //                 header.Currency_Key,
            //                 header.Uniqueid,
            //                 header.PO_Title,
            //                 header.SubmitterName

            //             }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();

            ////PoCount = query.Count();
            ////query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

            //if (user1 == "PO  Approval Admin")
            //{
            //    query = (from header in db.TEPOHeaderStructures
            //             where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft"))

            //             select new
            //             {
            //                 header.Purchasing_Order_Number,
            //                 header.Vendor_Account_Number,
            //                 // vend.Vendor_Owner,
            //                 header.Purchasing_Document_Date,
            //                 header.path,
            //                 header.ReleaseCode2Status,
            //                 header.Currency_Key,
            //                 header.Uniqueid,
            //                 header.PO_Title,
            //                 header.SubmitterName

            //             }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();


            //    if (FilterBy != null)
            //    {
            //        query = (from header in db.TEPOHeaderStructures
            //                 join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code into ps
            //                 from v in ps.DefaultIfEmpty()
            //                 where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))

            //                 select new
            //                 {
            //                     header.Purchasing_Order_Number,
            //                     header.Vendor_Account_Number,
            //                     // vend.Vendor_Owner,
            //                     header.Purchasing_Document_Date,
            //                     header.path,
            //                     header.ReleaseCode2Status,
            //                     header.Currency_Key,
            //                     header.Uniqueid,
            //                     header.PO_Title,
            //                     header.SubmitterName

            //                 }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();
            //    }


            //    //    PoCount = query.Count();
            //    //    query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
            //}
            //else
            //{



            //    query = (from header in db.TEPOHeaderStructures
            //             join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId

            //             where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft"))

            //             select new
            //             {
            //                 header.Purchasing_Order_Number,
            //                 header.Vendor_Account_Number,
            //                 // vend.Vendor_Owner,
            //                 header.Purchasing_Document_Date,
            //                 header.path,
            //                 header.ReleaseCode2Status,
            //                 header.Currency_Key,
            //                 header.Uniqueid,
            //                 header.PO_Title,
            //                 header.SubmitterName

            //             }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();


            //    if (FilterBy != null)
            //    {
            //        query = (from header in db.TEPOHeaderStructures
            //                 join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
            //                 join v in db.TEPOVendors on header.Vendor_Account_Number equals v.Vendor_Code into ps
            //                 from v in ps.DefaultIfEmpty()
            //                 where (appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))

            //                 select new
            //                 {
            //                     header.Purchasing_Order_Number,
            //                     header.Vendor_Account_Number,
            //                     // vend.Vendor_Owner,
            //                     header.Purchasing_Document_Date,
            //                     header.path,
            //                     header.ReleaseCode2Status,
            //                     header.Currency_Key,
            //                     header.Uniqueid,
            //                     header.PO_Title,
            //                     header.SubmitterName

            //                 }).Distinct().OrderByDescending(x => x.Purchasing_Document_Date).ToList();
            //    }


            //    //PoCount = query.Count();
            //    //query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
            //}
            #endregion

            var query = (from header in db.TEPOHeaderStructures
                         join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                         join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                         join v in db.TEPOVendorMasterDetails on header.VendorID equals v.POVendorDetailId
                         join vendor in db.TEPOVendorMasters on v.POVendorMasterId equals vendor.POVendorMasterId
                         join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                         from proj in tempprj.DefaultIfEmpty()
                             //join proj in db.TEProjects on header.ProjectID equals proj.ProjectID
                         where (header.IsDeleted == false && header.IsNewPO == true
                         && appr.ApproverId == UserId && appr.IsDeleted == false &&
                         appr.SequenceNumber == 0 && header.ReleaseCode2Status == "Draft"
                         )
                         select new
                         {
                             header.Purchasing_Order_Number,
                             header.Vendor_Account_Number,
                             header.Purchasing_Document_Date,
                             header.path,
                             header.ReleaseCode2Status,
                             header.ReleaseCode2Date,
                             fund.FundCenter_Description,
                             header.Currency_Key,
                             Vendor_Owner = vendor.VendorName,
                             header.Uniqueid,
                             header.PO_Title,
                             header.SubmitterName,
                             header.Managed_by,
                             proj.ProjectCode,
                             proj.ProjectName
                         }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();

            PoCount = query.Count;
            var finalResult = query;
            if (PoCount > 0)
            {
                if (pagenumber >= 0 && pagepercount > 0)
                {
                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    finalResult = query.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalResult.Count + start;
                    sinfo.totalrecords = PoCount;
                    sinfo.listcount = finalResult.Count;
                    sinfo.pages = pagenumber.ToString();
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";

                    hrm.Content = new JsonContent(new
                    {
                        result = query, //error
                        info = sinfo //return exception
                    });
                }
            }
            else
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "No Records Found";
                sinfo.fromrecords = 1;
                sinfo.torecords = 10;
                sinfo.totalrecords = 0;
                sinfo.listcount = 0;
                sinfo.pages = "0";

                hrm.Content = new JsonContent(new
                {
                    info = sinfo //return exception
                });
            }

            #region
            //foreach (var item in finalResult)
            //{
            //    double TotalPrice = 0.00;
            //    DateTime GSTDate = new DateTime(2017, 07, 03);
            //    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

            //    List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
            //                                                                        && (i.Condition_Type != "NAVS"
            //                                                                        && i.Condition_Type != "JICG"
            //                                                                        && i.Condition_Type != "JISG"
            //                                                                        && i.Condition_Type != "JICR"
            //                                                                        && i.Condition_Type != "JISR"
            //                                                                        && i.Condition_Type != "JIIR"
            //                                                                        )
            //                                                                        && (i.VendorCode == null
            //                                                                            || i.VendorCode == ""
            //                                                                            || i.VendorCode == item.Vendor_Account_Number
            //                                                                            || postingDate <= GSTDate
            //                                                                            )
            //                                                                        && i.IsDeleted == false).ToList();

            //    if (wiseList.Count > 0)
            //    {
            //        TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
            //        TotalPrice = Math.Round(TotalPrice);
            //        TotalPrice = Math.Truncate(TotalPrice);
            //    }

            //    TEPurchase_Vendor Vendor = db.TEPOVendors.Where(x => x.IsDeleted == false && x.Vendor_Code == item.Vendor_Account_Number).FirstOrDefault();
            //    string vendor_owner = "";
            //    if (Vendor != null)
            //    {
            //        vendor_owner = Vendor.Vendor_Owner;
            //    }

            //    List<string> WbsList = new List<string>();
            //    List<string> WbsHeadsList = new List<string>();

            //    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
            //                                            ).ToList();


            //    foreach (var IS in ItemStructure)
            //    {
            //        var WBSElementsList = new List<string>();

            //        if (IS.Item_Category != "D")
            //        {
            //            WBSElementsList = db.TEPOAssignments.Where(x => (x.POStructureId == item.Uniqueid)
            //                                       && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
            //                       .Select(x => x.WBS_Element).ToList();


            //        }
            //        else if (IS.Item_Category == "D" && IS.Material_Number == "")
            //        {
            //            WBSElementsList = db.TEPOServices.Where(x => (x.POStructureId == item.Uniqueid)
            //                                     && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
            //                     .Select(x => x.WBS_Element).ToList();

            //        }
            //        foreach (var ele in WBSElementsList)
            //        {
            //            //getWbsElement(WbsList, ele, i);



            //            int occ = ele.Count(x => x == '-');

            //            if (occ >= 3)
            //            {
            //                int index = CustomIndexOf(ele, '-', 3);
            //                // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
            //                string part = ele.Substring(0, index);
            //                WbsHeadsList.Add(part);
            //            }
            //            else
            //            {
            //                WbsHeadsList.Add(ele);
            //            }

            //            string Element = ele;
            //            char firstChar = Element[0];
            //            if (firstChar == '0')
            //            {

            //                WbsList.Add(Element.Substring(0, 4));

            //            }
            //            else if (firstChar == 'A')
            //            {

            //                WbsList.Add(Element.Substring(0, 5));

            //            }
            //            else if (firstChar == 'C')
            //            {

            //                WbsList.Add(Element.Substring(0, 5));

            //            }
            //            else if (firstChar == 'M')
            //            {
            //                string twoChar = Element.Substring(0, 2);
            //                if (twoChar == "MN")
            //                {

            //                    WbsList.Add(Element.Substring(0, 7));

            //                }
            //                else if (twoChar == "MC")
            //                {

            //                    WbsList.Add(Element.Substring(0, 4));

            //                }
            //            }
            //            else if (firstChar == 'Y')
            //            {
            //                string twoChar = Element.Substring(0, 2);
            //                if (twoChar == "YS")
            //                {

            //                    WbsList.Add(Element.Substring(0, 7));

            //                }

            //            }
            //            else if (firstChar == 'O')
            //            {
            //                string threeChar = Element.Substring(0, 3);
            //                if (threeChar == "OB2")
            //                {

            //                    WbsList.Add(Element);

            //                }

            //            }
            //        }
            //    }

            //    WbsList = WbsList.Distinct().ToList();
            //    WbsHeadsList = WbsHeadsList.Distinct().ToList();

            //    string WbsHeads = "";
            //    foreach (var w in WbsHeadsList)
            //    {
            //        if (WbsHeads == "")
            //        {
            //            WbsHeads = w;
            //        }
            //        else
            //        {
            //            WbsHeads = WbsHeads + "," + w;
            //        }
            //    }

            //    string ProjectCodes = "";
            //    foreach (var w in WbsList)
            //    {
            //        if (ProjectCodes == "")
            //        {
            //            ProjectCodes = w;
            //        }
            //        else
            //        {
            //            ProjectCodes = ProjectCodes + "," + w;
            //        }
            //    }



            //    PoCount = PoCount + 1;
            //    result.Add(new TEPurchaseHomeModel()
            //    {
            //        Purchasing_Order_Number = item.Purchasing_Order_Number,
            //        Vendor_Account_Number = item.Vendor_Account_Number,
            //        Purchasing_Document_Date = item.Purchasing_Document_Date,
            //        Path = item.path,
            //        ReleaseCodeStatus = item.ReleaseCode2Status,
            //        Amount
            //        = TotalPrice,
            //        Currency_Key = item.Currency_Key,
            //        HeaderUniqueid = item.Uniqueid,
            //        PoCount = PoCount,
            //        PoTitle = item.PO_Title,
            //        VendorName = vendor_owner,
            //        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
            //        POStatus = item.ReleaseCode2Status,
            //        ProjectCodes = ProjectCodes,
            //        SubmitterName = item.SubmitterName,
            //        WbsHeads = WbsHeads

            //    });

            //}

            //return new HttpResponseMessage
            //{
            //    StatusCode = HttpStatusCode.OK,
            //    Content = new JsonContent(new
            //    {

            //        res = result

            //    })
            //};
            #endregion

            foreach (var item in finalResult)
            {
                DateTime? releasedate = null;
                double? TotalPrice = 0.00;
                DateTime GSTDate = new DateTime(2017, 07, 03);
                DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                List<string> WbsList = new List<string>();
                List<string> WbsHeadsList = new List<string>();


                var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                        ).ToList();
                if (ItemStructure.Count > 0)
                {
                    TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.TotalAmount.Value));
                }

                result.Add(new TEPurchaseHomeModel()
                {
                    Purchasing_Order_Number = string.IsNullOrEmpty(item.Purchasing_Order_Number) ? item.Uniqueid.ToString() : item.Purchasing_Order_Number,
                    Vendor_Account_Number = item.Vendor_Account_Number,
                    Purchasing_Document_Date = item.Purchasing_Document_Date,
                    Path = item.path,
                    ReleaseCodeStatus = item.ReleaseCode2Status,
                    Purchasing_Release_Date = releasedate,
                    Amount = TotalPrice,
                    Currency_Key = item.Currency_Key,
                    HeaderUniqueid = item.Uniqueid,
                    PoCount = PoCount,
                    PoTitle = item.PO_Title,
                    VendorName = item.Vendor_Owner,
                    Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                    POStatus = item.ReleaseCode2Status,
                    ProjectCodes = item.ProjectCode,
                    SubmitterName = item.SubmitterName,
                    WbsHeads = item.FundCenter_Description,
                    ManagerName = item.Managed_by
                });
            }

            if (result.Count > 0)
            {
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;

        }

        [HttpPost]
        public HttpResponseMessage MyPOApprovalsList(JObject json)
        {
            int UserId = 0;
            if (json["UserId"] != null) UserId = json["UserId"].ToObject<int>();
            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            List<TEPurchaseHomeModel> fResult = new List<TEPurchaseHomeModel>();
            try
            {
                int PoCount = 0;
                var myPOList = (from header in db.TEPOHeaderStructures
                                join appr in db.TEPOApprovers on header.Uniqueid equals appr.POStructureId
                                join fund in db.TEPOFundCenters on header.FundCenterID equals fund.Uniqueid
                                join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId into tempvendordtl
                                from vndrdtl in tempvendordtl.DefaultIfEmpty()
                                join vendor in db.TEPOVendorMasters on vndrdtl.POVendorMasterId equals vendor.POVendorMasterId into tempvendormstr
                                from vndrMstr in tempvendormstr.DefaultIfEmpty()
                                join prj in db.TEProjects on fund.ProjectCode equals prj.ProjectCode into tempprj
                                from proj in tempprj.DefaultIfEmpty()
                                join order in db.TEPurchase_OrderTypes on header.PO_OrderTypeID equals order.UniqueId into temporder
                                from finalorder in temporder.DefaultIfEmpty()
                                join manager in db.UserProfiles on header.POManagerID equals manager.UserId into tempmgr
                                from mnger in tempmgr.DefaultIfEmpty()
                                where (header.IsDeleted == false
                                && appr.ApproverId == UserId && appr.IsDeleted == false
                                && (appr.Status == "Pending For Approval")
                                && (header.ReleaseCode2Status == "Pending For Approval"))
                                select new
                                {
                                    header.Purchasing_Order_Number,
                                    header.Vendor_Account_Number,
                                    header.Purchasing_Document_Date,
                                    header.path,
                                    header.ReleaseCode2Status,
                                    header.ReleaseCode2Date,
                                    header.Version,
                                    fund.FundCenter_Description,
                                    header.Currency_Key,
                                    Vendor_Owner = vndrMstr.VendorName,
                                    header.CreatedBy,
                                    OrderType = finalorder.Description,
                                    header.CreatedOn,
                                    header.Uniqueid,
                                    header.PO_Title,
                                    header.SubmitterName,
                                    Managed_by = mnger.CallName,
                                    header.POManagerID,
                                    proj.ProjectCode,
                                    proj.ProjectName,
                                    header.PurchaseRequestId,
                                    proj.ProjectShortName
                                }).Distinct().OrderByDescending(a => a.CreatedOn).ToList();
                PoCount = myPOList.Count;
                foreach (var item in myPOList)
                {
                    DateTime? releasedate = null;
                    double? TotalPrice = 0.00;
                    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();
                    var ItemStructure = db.TEPOItemStructures.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                            ).ToList();
                    if (ItemStructure.Count > 0)
                    {
                        TotalPrice = Convert.ToDouble(ItemStructure.Where(a => a.GrossAmount != null).Sum(x => x.GrossAmount.Value));
                    }
                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = item.ReleaseCode2Status,
                        Purchasing_Release_Date = releasedate,
                        Amount = TotalPrice,
                        PurchaseRequestId = item.PurchaseRequestId,
                        Currency_Key = item.Currency_Key,
                        HeaderUniqueid = item.Uniqueid,
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy,
                        PoCount = PoCount,
                        OrderType = item.OrderType,
                        PoTitle = item.PO_Title,
                        VendorName = item.Vendor_Owner,
                        Approvers = db.TEPOApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                        POStatus = item.ReleaseCode2Status,
                        ProjectCodes = item.ProjectCode,
                        SubmitterName = item.SubmitterName,
                        WbsHeads = item.FundCenter_Description,
                        ManagerName = item.Managed_by,
                        ProjectShortName = item.ProjectShortName,
                        Version = "R" + item.Version
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
            if (result.Count > 0)
            {
                foreach (TEPurchaseHomeModel purHome in result)
                {
                    if (purHome.Approvers.Count > 0)
                    {
                        var NextApprover = (from u in db.TEPOApprovers
                                            where (u.POStructureId == purHome.HeaderUniqueid && u.Status == "Pending For Approval"
                                            && u.IsDeleted == false)
                                            orderby u.SequenceNumber
                                            select u).FirstOrDefault();
                        if (NextApprover != null)
                        {
                            if (UserId == NextApprover.ApproverId)
                                purHome.isCurrentApprover = true;
                            else
                            {
                                fResult.Remove(purHome);
                                //purHome.isCurrentApprover = false;
                            }
                        }
                        else
                            fResult.Remove(purHome);
                    }
                    fResult.Add(purHome);
                }
            }
            if (fResult.Count > 0)
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Success";
                sinfo.fromrecords = fResult.Count;
                sinfo.torecords = fResult.Count;
                sinfo.totalrecords = fResult.Count;
                sinfo.listcount = fResult.Count;
                hrm.Content = new JsonContent(new
                {
                    result = fResult,
                    info = sinfo
                });
            }
            else
            {
                finfo.errorcode = 0;
                finfo.errormessage = "No Records";
                finfo.listcount = 0;
                hrm.Content = new JsonContent(new
                {
                    result = result,
                    info = finfo
                });
            }
            return hrm;
        }

        [HttpPost]
        public HttpResponseMessage TEPurchaseOrdersCount(JObject json)
        {
            try
            {
                int UserId = 0;
                UserId = json["UserId"].ToObject<int>();
                TEPurchaseCountModel result = new TEPurchaseCountModel();
                if (UserId > 0)
                {
                    result.PendingCount = db.TEPOApprovers.Where(x => x.ApproverId == UserId && x.Status == "Pending For Approval" && x.IsDeleted == false).Count();
                    result.ApprovedCount = db.TEPOApprovers.Where(x => x.ApproverId == UserId && x.Status == "Approved" && x.IsDeleted == false).Count();
                    result.UpcomingCount = db.TEPOApprovers.Where(x => x.ApproverId == UserId && x.Status == "Draft" && x.IsDeleted == false).Count();
                    result.RejectedCount = db.TEPOApprovers.Where(x => x.ApproverId == UserId && x.Status == "Rejected" && x.IsDeleted == false).Count();
                    result.TotalCount = db.TEPOApprovers.Where(x => x.ApproverId == UserId && x.Status != "NULL" && x.IsDeleted == false).Count();
                }
                if (result != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = result }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = result }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GETAllStorageDetailsByStorageCode(JObject json)
        {
            string Code = string.Empty;
            Code = json["Code"].ToObject<string>();
            List<TEPOPlantStorageDetail> result = new List<TEPOPlantStorageDetail>();
            result = db.TEPOPlantStorageDetails.Where(x => x.isdeleted == false && x.StorageLocationCode != null && x.StateCode == Code).ToList();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    res = result

                })
            };
        }
        [HttpPost]
        public HttpResponseMessage GetAllCustomers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.TECompanies.Where(d => d.IsDeleted == false).ToList();

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetAllVendors()
        {
            try
            {
                var vendorList = (from vndr in db.TEPOVendorMasters
                                  join vndrdtl in db.TEPOVendorMasterDetails on vndr.POVendorMasterId equals vndrdtl.POVendorMasterId
                                  join state in db.TEGSTNStateMasters on vndrdtl.RegionId equals state.StateID into tempregion
                                  from region in tempregion.DefaultIfEmpty()
                                  join cntry in db.TEPOCountryMasters on vndrdtl.CountryId equals cntry.UniqueID into tempctry
                                  from country in tempctry.DefaultIfEmpty()
                                  where vndr.IsDeleted == false && vndrdtl.IsDeleted == false
                                  select new
                                  {
                                      vndrdtl.VendorCode,
                                      vndr.VendorName,
                                      vndrdtl.RegionId,
                                      RegionCode = region.StateCode,
                                      RegionCodeDesc = region.StateName,
                                      vndr.POVendorMasterId,
                                      vndrdtl.POVendorDetailId,
                                      vndrdtl.BillingAddress,
                                      vndrdtl.BillingPostalCode,
                                      vndrdtl.BillingCity,
                                      vndrdtl.ShippingAddress,
                                      vndrdtl.ShippingCity,
                                      vndrdtl.ShippingPostalCode,
                                      vndr.CIN,
                                      Country = country.Description,
                                      country.CountryCode,
                                      vndr.Currency,
                                      vndrdtl.GSTIN,
                                      PanNumber = vndr.PAN,
                                      vndr.ServiceTax,
                                  }).OrderBy(a => a.VendorName).ToList();
                if (vendorList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = vendorList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetVendor(JObject json)
        {
            try
            {
                int vendordetailid = 0;
                vendordetailid = json["ID"].ToObject<int>();

                var vendorData = (from vndr in db.TEPOVendorMasters
                                  join vndrdtl in db.TEPOVendorMasterDetails on vndr.POVendorMasterId equals vndrdtl.POVendorMasterId
                                  join state in db.TEGSTNStateMasters on vndrdtl.RegionId equals state.StateID into tempregion
                                  from region in tempregion.DefaultIfEmpty()
                                  join cntry in db.TEPOCountryMasters on vndrdtl.CountryId equals cntry.UniqueID into tempctry
                                  from country in tempctry.DefaultIfEmpty()
                                  where vndr.IsDeleted == false && vndrdtl.IsDeleted == false && vndrdtl.POVendorDetailId == vendordetailid
                                  select new
                                  {
                                      vndrdtl.VendorCode,
                                      vndr.VendorName,
                                      vndrdtl.RegionId,
                                      RegionCode = region.StateCode,
                                      RegionCodeDesc = region.StateName,
                                      vndr.POVendorMasterId,
                                      vndrdtl.POVendorDetailId,
                                      vndrdtl.BillingAddress,
                                      vndrdtl.BillingPostalCode,
                                      vndrdtl.BillingCity,
                                      vndrdtl.ShippingAddress,
                                      vndrdtl.ShippingCity,
                                      vndrdtl.ShippingPostalCode,
                                      vndr.CIN,
                                      Country = country.Description,
                                      country.CountryCode,
                                      vndr.Currency,
                                      vndrdtl.GSTIN,
                                      PanNumber = vndr.PAN,
                                      vndr.ServiceTax,
                                  }).OrderBy(a => a.VendorName).FirstOrDefault();
                if (vendorData != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorData, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = vendorData, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }

        //[HttpPost]
        //public HttpResponseMessage CreateVendor(TEPOVendor purchase_Vendor)
        //{
        //    try
        //    {
        //        string mesg = "";
        //        if (purchase_Vendor == null)
        //        {
        //            mesg = "check object";
        //            return new HttpResponseMessage()
        //            {
        //                StatusCode = HttpStatusCode.NoContent,
        //                Content = new StringContent(mesg)
        //            };
        //        }
        //        purchase_Vendor.CreatedOn = DateTime.Now.Date.ToShortDateString();
        //        purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
        //        db.TEPOVendors.Add(purchase_Vendor);
        //        db.SaveChanges();
        //        mesg = "successfully Registered";
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent(mesg)
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.BadRequest,
        //            Content = new StringContent(e.Message)
        //        };
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage UpdateVendor(TEPOVendor purchase_Vendor)
        //{
        //    try
        //    {
        //        string mesg = "";
        //        if (purchase_Vendor == null)
        //        {
        //            mesg = "check object";
        //            return new HttpResponseMessage()
        //            {
        //                StatusCode = HttpStatusCode.NoContent,
        //                Content = new StringContent(mesg)
        //            };
        //        }

        //        purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
        //        var vendor = db.TEPOVendors.Single(a => a.Uniqueid == purchase_Vendor.Uniqueid);
        //        db.Entry(vendor).CurrentValues.SetValues(purchase_Vendor);
        //        db.SaveChanges();
        //        mesg = "successfully Updated";
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent(mesg)
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        return new HttpResponseMessage()
        //        {
        //            StatusCode = HttpStatusCode.BadRequest,
        //            Content = new StringContent(e.Message)
        //        };
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage GetAllFundCenters()
        {
            try
            {
                var fndList = (from fnd in db.TEPOFundCenters
                               where fnd.IsDeleted == false && fnd.FundCenter_Code != null
                               select new
                               {
                                   fnd.FundCenter_Code,
                                   fnd.FundCenter_Description,
                                   fnd.FundCenter_Owner,
                                   fnd.Uniqueid,
                                   fnd.ProjectCode,
                                   fnd.ProjectName
                               }).OrderBy(a => a.Uniqueid).ToList();
                if (fndList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = fndList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fndList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fndList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetAllFundCentersBySearch(JObject json)
        {
            try
            {
                string serchTest = string.Empty;
                serchTest = json["SearchText"].ToObject<string>();
                var fndList = (from fnd in db.TEPOFundCenters
                               where fnd.IsDeleted == false && fnd.FundCenter_Code.Contains(serchTest)
                               select new
                               {
                                   fnd.FundCenter_Code,
                                   fnd.FundCenter_Description,
                                   fnd.FundCenter_Owner,
                                   fnd.Uniqueid,
                                   fnd.ProjectCode,
                                   fnd.ProjectName
                               }).OrderBy(a => a.FundCenter_Owner).ToList();
                if (fndList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = fndList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fndList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fndList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetFundCenterByCode(JObject json)
        {
            try
            {
                string serchTest = string.Empty;
                serchTest = json["SearchText"].ToObject<string>();
                var list = db.TEPOFundCenters.Where(a => a.FundCenter_Code == serchTest).FirstOrDefault();
                if (list != null) { sinfo.errorcode = 0; sinfo.errormessage = "Got Record"; } else { sinfo.errorcode = 1; sinfo.errormessage = "No Record Found"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = list, info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetFundCenter(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            var list = db.TEPOFundCenters.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }
        [HttpPost]
        public HttpResponseMessage GetFundCenterByUserId(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            var list = (from usr in db.UserProfiles
                        join fnd in db.TEPOFundCenters on usr.CallName equals fnd.FundCenter_Owner
                        where (usr.UserId == id && fnd.IsDeleted == false)
                        select fnd).ToList();

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        //[HttpPost]
        //public HttpResponseMessage createTEPurchase_Vendor(TEPOVendor purchase_Vendor)
        //{
        //    try
        //    {
        //        int id = 0;
        //        if (purchase_Vendor != null)
        //        {
        //            purchase_Vendor.CreatedOn = DateTime.Now.Date.ToShortDateString();
        //            purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
        //            db.TEPOVendors.Add(purchase_Vendor);
        //            db.SaveChanges();
        //            id = purchase_Vendor.Uniqueid;
        //        }

        //        if (id > 0)
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Saved";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 1;
        //            sinfo.errormessage = "Failed to Save/Update";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Failed to Save/Update";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage updateTEPurchase_Vendor(TEPOVendor purchase_Vendor)
        //{
        //    try
        //    {
        //        int id = 0;
        //        if (purchase_Vendor != null)
        //        {
        //            purchase_Vendor.LastModifiedOn = DateTime.Now.Date.ToShortDateString();
        //            var vendor = db.TEPOVendors.Single(a => a.Uniqueid == purchase_Vendor.Uniqueid);
        //            db.Entry(purchase_Vendor).CurrentValues.SetValues(purchase_Vendor);
        //            db.SaveChanges();
        //            id = purchase_Vendor.Uniqueid;
        //        }

        //        if (id > 0)
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Updated";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 1;
        //            sinfo.errormessage = "Failed to Save/Update";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Failed to Save/Update";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage getTEPurchase_MasterApprovers(JObject json)
        {
            try
            {
                int uniqueid = 0;
                uniqueid = json["uniqueid"].ToObject<int>();
                var list = (from ad in db.POMasterApprovers
                            from aa in db.POApprovalConditions
                            where ad.UniqueId == uniqueid && ad.ApprovalConditionId == aa.UniqueId
                            select new
                            {
                                OrderType = aa.OrderType,
                                FundCenter = aa.FundCenter,
                                MinAmount = aa.MinAmount,
                                MaxAmount = aa.MaxAmount,
                                Type = ad.Type,
                                SequenceId = ad.SequenceId,
                                CreatedBy = ad.CreatedBy,
                                CreatedOn = ad.CreatedOn,
                                LastModifiedBy = ad.LastModifiedBy,
                                LastModifiedOn = ad.LastModifiedOn,
                                ApprovalConditionId = ad.ApprovalConditionId,
                                ApproverId = ad.ApproverId,
                                ApproverName = ad.ApproverName,
                                uniqueid = ad.UniqueId
                            }).ToList();

                if (list.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = list, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = list, info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetAllUsers()
        {
            try
            {
                List<UserProfile> userProfileList = new List<UserProfile>();
                userProfileList = db.UserProfiles.Where(d => d.IsDeleted == false).ToList();
                if (userProfileList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = userProfileList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = userProfileList, info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        #region  WBS Fundcenter
        //Insert Method
        [HttpPost]
        public HttpResponseMessage Wbs_FundcenterMapping(TEPOWBSMasteModelr mappingObj)
        {
            try
            {
                int id = 0;
                if (mappingObj != null)
                {

                    var FundProj = (from fund in db.TEPOFundCenters
                                    where fund.Uniqueid == mappingObj.FundCentreID
                                    select new
                                    {
                                        Uniqueid = fund.Uniqueid,
                                        ProjectCode = fund.ProjectCode,
                                        FundCenter_Code = fund.FundCenter_Code,
                                        ProjectName = fund.ProjectName
                                    }).FirstOrDefault();

                    TEPOWBSMaster WBSMaster = new TEPOWBSMaster();

                    WBSMaster.WBSCode = mappingObj.WBSCode;
                    WBSMaster.WBSName = mappingObj.WBSName;
                    WBSMaster.ProjectCode = FundProj.ProjectCode;
                    WBSMaster.WBSCode = mappingObj.WBSCode;
                    WBSMaster.CreatedDate = DateTime.Now;
                    WBSMaster.CreatedBy = "";
                    WBSMaster.IsDeleted = false;
                    //WBSMaster.WBSID = 0;
                    db.TEPOWBSMasters.Add(WBSMaster);
                    db.SaveChanges();
                    id = WBSMaster.WBSID;
                    if (id > 0)
                    {
                        TEPOWBSFundCentreMapping WBSFundMap = new TEPOWBSFundCentreMapping();
                        WBSFundMap.WBSID = id;
                        WBSFundMap.WBSCode = WBSMaster.WBSCode;
                        WBSFundMap.FundCentreID = FundProj.Uniqueid;
                        WBSFundMap.FundCentreCode = FundProj.FundCenter_Code;
                        WBSFundMap.ProjectCode = FundProj.ProjectCode;
                        WBSFundMap.ProjectDesc = FundProj.ProjectName;
                        WBSFundMap.LastModifiedBy = this.GetLogInUserId();
                        WBSFundMap.LastModifiedOn = DateTime.Now;
                        WBSFundMap.IsDeleted = false;
                        db.TEPOWBSFundCentreMappings.Add(WBSFundMap);
                        db.SaveChanges();

                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Save/Update";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }

                }

                if (id > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        //Update Method
        [HttpPost]
        public HttpResponseMessage updateWbs_FundcenterMapping(TEPOWBSFundCentreMapping mappingObj)
        {
            try
            {
                int id = 0;
                if (mappingObj != null)
                {
                    TEPOWBSFundCentreMapping WBSNObj = db.TEPOWBSFundCentreMappings.Where(a => a.UniqueID == mappingObj.UniqueID && a.IsDeleted == false).FirstOrDefault();
                    if (WBSNObj != null)
                    {
                        var FundProj = (from fund in db.TEPOFundCenters
                                        where fund.Uniqueid == mappingObj.FundCentreID
                                        select new
                                        { ProjectCode = fund.ProjectCode, FundCenter_Code = fund.FundCenter_Code, ProjectName = fund.ProjectName }).FirstOrDefault();


                        WBSNObj.FundCentreID = mappingObj.FundCentreID;
                        WBSNObj.FundCentreCode = FundProj.FundCenter_Code;
                        WBSNObj.ProjectCode = FundProj.ProjectCode;
                        WBSNObj.ProjectDesc = FundProj.ProjectName;
                        WBSNObj.LastModifiedOn = DateTime.Now;
                        WBSNObj.LastModifiedBy = GetLogInUserId();
                        WBSNObj.IsDeleted = false;
                        db.Entry(WBSNObj).CurrentValues.SetValues(WBSNObj);
                        db.SaveChanges();

                        //mappingObj.UniqueID = 0;
                        //mappingObj.FundCentreCode = db.TEPOWBSFundCentreMappings.Select(x => x.FundCentreCode)
                        //mappingObj.IsDeleted = false;
                        //db.TEPOWBSFundCentreMappings.Add(mappingObj);

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Save/Update";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                    }
                }

                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //Update Method
        [HttpPost]
        public HttpResponseMessage DeleteWbs_FundcenterMapping(TEPOWBSFundCentreMapping mappingObj)
        {
            try
            {

                if (mappingObj != null)
                {
                    TEPOWBSFundCentreMapping WBSNObj = db.TEPOWBSFundCentreMappings.Where(a => a.UniqueID == mappingObj.UniqueID && a.IsDeleted == false).FirstOrDefault();
                    if (WBSNObj != null)
                    {

                        WBSNObj.LastModifiedOn = DateTime.Now;
                        WBSNObj.LastModifiedBy = GetLogInUserId();
                        WBSNObj.IsDeleted = true;
                        db.Entry(WBSNObj).CurrentValues.SetValues(WBSNObj);
                        db.SaveChanges();

                        // mappingObj.UniqueID = 0;
                        //mappingObj.FundCentreCode = db.TEPOWBSFundCentreMappings.Select(x => x.FundCentreCode)
                        //mappingObj.IsDeleted = false;
                        //db.TEPOWBSFundCentreMappings.Add(mappingObj);

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Deleted";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Delete";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                    }
                }

                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Delete";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        //Get Record by ID
        [HttpPost]
        public HttpResponseMessage getWbs_FundcenterMapping(JObject json)
        {
            int uniqueid = json["UniqueID"].ToObject<int>();
            try
            {
                fundcenterDTO fundObj = new fundcenterDTO();
                if (uniqueid > 0)
                {
                    fundObj = (from ad in db.TEPOWBSFundCentreMappings
                               where ad.UniqueID == uniqueid
                               join wbs in db.TEPOWBSMasters on ad.WBSID equals wbs.WBSID
                               into tempwbs
                               from WBSTbl in tempwbs.DefaultIfEmpty()
                               join fund in db.TEPOFundCenters on ad.FundCentreID equals fund.Uniqueid
                               into tempfund
                               from fundtbl in tempfund.DefaultIfEmpty()
                               where ad.IsDeleted == false
                               select new fundcenterDTO
                               {
                                   uniqueID = ad.UniqueID,
                                   //categoryid = wbs.categoryid,
                                   //SUBCATEGORYID = wbs.subcategoryid,
                                   WBSCode = ad.WBSCode,
                                   FundcenterUniqueID = fundtbl.Uniqueid,
                                   name = WBSTbl.WBSName,
                                   WbsUniqueID = WBSTbl.WBSID,
                                   FundCentreCode = ad.FundCentreCode,
                                   FundCenter_Description = fundtbl.FundCenter_Description,
                                   LastModifiedBy = ad.LastModifiedBy,
                                   LastModifiedOn = ad.LastModifiedOn

                               }).FirstOrDefault();
                }

                if (fundObj != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Record";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundObj, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Get Record";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundObj, info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        //Get All Records
        [HttpPost]
        public HttpResponseMessage getWBSFundcenterMappingPagination(JObject json)
        {
            int pagenumber = json["page_number"].ToObject<int>();
            int pagepercount = json["pagepercount"].ToObject<int>();
            List<fundcenterDTO> fundcenterList = new List<fundcenterDTO>();
            List<fundcenterDTO> finalfundcenterList = new List<fundcenterDTO>();
            int count = 0;
            try
            {
                fundcenterList = (from fndmap in db.TEPOWBSFundCentreMappings
                                  join prof in db.UserProfiles on fndmap.LastModifiedBy equals prof.UserId into tempprof
                                  from user in tempprof.DefaultIfEmpty()
                                      //join wbs in db.WBSMASTERs on ad.WBSID equals wbs.WBSID
                                  join fund in db.TEPOFundCenters on fndmap.FundCentreID equals fund.Uniqueid
                                  where fndmap.IsDeleted == false
                                  select new fundcenterDTO
                                  {
                                      uniqueID = fndmap.UniqueID,
                                      //categoryid = wbs.categoryid,
                                      //SUBCATEGORYID = wbs.subcategoryid,
                                      WBSCode = fndmap.WBSCode,
                                      // name = wbs.NAME,
                                      ProjectCode = fndmap.ProjectCode,
                                      ProjectName = fndmap.ProjectDesc,
                                      FundCentreCode = fndmap.FundCentreCode,
                                      FundCenter_Description = fund.FundCenter_Description,
                                      LastModifiedBy = fndmap.LastModifiedBy,
                                      LastModifiedOn = fndmap.LastModifiedOn
                                  }).OrderByDescending(x => x.uniqueID).ToList();

                count = fundcenterList.Count;
                if (count > 0)
                {
                    if (pagenumber == 0)
                    {
                        pagenumber = 1;
                    }
                    int iPageNum = pagenumber;
                    int iPageSize = pagepercount;
                    int start = iPageNum - 1;
                    start = start * iPageSize;
                    finalfundcenterList = fundcenterList.Skip(start).Take(iPageSize).ToList();
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                    sinfo.torecords = finalfundcenterList.Count + start;
                    sinfo.totalrecords = count;
                    sinfo.listcount = finalfundcenterList.Count;
                    sinfo.pages = "1";
                    if (finalfundcenterList.Count > 0)
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully";
                        return new HttpResponseMessage() { Content = new JsonContent(new { result = finalfundcenterList, info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "No Records";
                        sinfo.listcount = 0;
                        return new HttpResponseMessage() { Content = new JsonContent(new { result = finalfundcenterList, info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = finalfundcenterList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { result = fundcenterList, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage getAllWbs_FundcenterMapping()
        {
            try
            {
                List<fundcenterDTO> fundcenterList = new List<fundcenterDTO>();
                fundcenterList = (from ad in db.TEPOWBSFundCentreMappings
                                  join wbs in db.WBSMASTERs on ad.WBSID equals wbs.WBSID
                                  join fund in db.TEPOFundCenters on
                                     ad.FundCentreID equals fund.Uniqueid
                                  select new fundcenterDTO
                                  {
                                      uniqueID = ad.UniqueID,
                                      //categoryid = wbs.categoryid,
                                      //SUBCATEGORYID = wbs.subcategoryid,
                                      WBSCode = ad.WBSCode,
                                      name = wbs.NAME,
                                      FundCentreCode = ad.FundCentreCode,
                                      FundCenter_Description = fund.FundCenter_Description,
                                      LastModifiedBy = ad.LastModifiedBy,
                                      LastModifiedOn = ad.LastModifiedOn
                                  }).ToList();

                if (fundcenterList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundcenterList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundcenterList, info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


        [HttpPost]
        public HttpResponseMessage GetWBSFundCenterDataForSearch(JObject json)
        {
            try
            {
                int fundCenterID = 0; string projectCode = string.Empty;
                if (json["FundCenterID"] != null)
                    fundCenterID = json["FundCenterID"].ToObject<int>();
                if (json["ProjectCode"] != null)
                    projectCode = json["ProjectCode"].ToObject<string>();
                List<fundcenterDTO> fundcenterList = new List<fundcenterDTO>();
                fundcenterList = (from ad in db.TEPOWBSFundCentreMappings
                                  join fund in db.TEPOFundCenters on
                                     ad.FundCentreID equals fund.Uniqueid
                                  where ad.FundCentreID == fundCenterID && ad.ProjectCode == projectCode
                                  select new fundcenterDTO
                                  {
                                      uniqueID = ad.UniqueID,
                                      WBSCode = ad.WBSCode,
                                      FundCentreCode = ad.FundCentreCode,
                                      FundCenter_Description = fund.FundCenter_Description,
                                      LastModifiedBy = ad.LastModifiedBy,
                                      LastModifiedOn = ad.LastModifiedOn
                                  }).ToList();

                if (fundcenterList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundcenterList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = fundcenterList, info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        public class TEPOWBSMasteModelr
        {
            public int WBSID { get; set; }
            public string WBSCode { get; set; }
            public string WBSName { get; set; }
            public string ProjectCode { get; set; }

            public int FundCentreID { get; set; }
            public Nullable<bool> IsDeleted { get; set; }
            public string LastModifiedBy { get; set; }
            public Nullable<System.DateTime> LastModifiedDate { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
        }

        #endregion

        [HttpPost]
        public HttpResponseMessage GetAllMasterAprovers(JObject json)
        {
            try
            {
                int pagenumber = 0, pagepercount = 0;
                if (json["pageNumber"] != null)
                    pagenumber = json["pageNumber"].ToObject<int>();
                if (json["pagePerCount"] != null)
                    pagepercount = json["pagePerCount"].ToObject<int>();
                SuccessInfo sinfo = new SuccessInfo();
                FailInfo finfo = new FailInfo();
                HttpResponseMessage hrm = new HttpResponseMessage();
                List<PurchaseApproversDTO> poApprDtoList = new List<PurchaseApproversDTO>();

                List<POApprovalConditionDTO> newList =
                   (from cond in db.POApprovalConditions
                    join apprl in db.POMasterApprovers on cond.UniqueId equals apprl.ApprovalConditionId

                    join fund in db.TEPOFundCenters on cond.FundCenter equals fund.Uniqueid
                    where cond.IsDeleted == false && apprl.IsDeleted == false
                    select new POApprovalConditionDTO
                    {
                        FundCenterCode = fund.FundCenter_Code,
                        FundCenter = fund.Uniqueid,
                        FundCenterDescription = fund.FundCenter_Description,
                        LastModifiedBy = cond.LastModifiedBy,
                        POManagerID = cond.POManagerID,
                        MinAmount = cond.MinAmount,
                        MaxAmount = cond.MaxAmount,
                        UniqueId = cond.UniqueId
                    }
                    ).Distinct().ToList();

                if (newList.Count > 0)
                {
                    foreach (POApprovalConditionDTO apprCond in newList)
                    {
                        PurchaseApproversDTO poApprDto = new PurchaseApproversDTO();
                        //if(apprCond.UniqueId==3617)
                        // {

                        // }
                        var PurApprList = (from cond in db.POApprovalConditions
                                           join apprl in db.POMasterApprovers on cond.UniqueId equals apprl.ApprovalConditionId
                                           where cond.IsDeleted == false && apprl.IsDeleted == false && cond.UniqueId == apprCond.UniqueId
                                           select apprl).Distinct().ToList();

                        //var Fundcentername = (from cond in db.POApprovalConditions
                        //                      join fund in db.TEPOFundCenters on cond.FundCenter equals fund.Uniqueid
                        //                      where cond.IsDeleted == false && cond.UniqueId == apprCond.UniqueId
                        //                      select fund
                        //                   ).Distinct().ToList();
                        poApprDto.POApprovalConditionDTO = apprCond;
                        poApprDto.MasterApproverlist = PurApprList;
                        //poApprDto.POApprovalConditionDTO.FundCenter = Fundcentername.
                        poApprDtoList.Add(poApprDto);
                    }
                }

                int count = poApprDtoList.Count;
                if (count > 0)
                {
                    if (pagenumber >= 0 && pagepercount > 0)
                    {
                        if (pagenumber == 0)
                        {
                            pagenumber = 1;
                        }
                        int iPageNum = pagenumber;
                        int iPageSize = pagepercount;
                        int start = iPageNum - 1;
                        start = start * iPageSize;
                        var flist = poApprDtoList.Skip(start).Take(iPageSize).ToList();
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = (start == 0) ? 1 : start + 1;
                        sinfo.torecords = flist.Count + start;
                        sinfo.totalrecords = count;
                        sinfo.listcount = flist.Count;
                        sinfo.pages = pagenumber.ToString();
                        return new HttpResponseMessage() { Content = new JsonContent(new { result = flist, info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Success";
                        sinfo.fromrecords = 1;
                        sinfo.torecords = 10;
                        sinfo.totalrecords = 0;
                        sinfo.listcount = 0;
                        sinfo.pages = "0";

                        return new HttpResponseMessage() { Content = new JsonContent(new { result = poApprDtoList, info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records Found";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.totalrecords = 0;
                    sinfo.listcount = 0;
                    sinfo.pages = "0";

                    return new HttpResponseMessage() { Content = new JsonContent(new { result = poApprDtoList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

        }
        //[HttpPost]
        //public HttpResponseMessage SaveTEPurchaseMasterApprovers(PurchaseApproversDTO pr)
        //{
        //    try
        //    {
        //        string req = string.Empty;
        //        if (pr != null)
        //        {
        //            POMasterApprover approver = new POMasterApprover();
        //            POApprovalCondition condition = new POApprovalCondition();
        //            condition = pr.ApprovalCondition;
        //            req = addTEPOApprovalCondition(condition);
        //            req = addTEPOApprover(pr.MasterApproverlist, req);
        //        }

        //        if (req == "success")
        //        {
        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Saved";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 1;
        //            sinfo.errormessage = "Failed to Save/Update";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Failed to Save/Update";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        //    }

        //}
        [HttpPost]
        public HttpResponseMessage SaveTEPurchaseMasterApprovers(PurchaseApproversDTO pr)
        {
            try
            {
                string req = string.Empty;
                if (pr != null)
                {

                    var apprCond = (from app in db.POApprovalConditions
                                    where app.FundCenter == pr.ApprovalCondition.FundCenter
                                    && app.MinAmount == pr.ApprovalCondition.MinAmount &&
                                    app.MaxAmount == pr.ApprovalCondition.MaxAmount && app.IsDeleted == false
                                    select new
                                    {
                                        app.UniqueId,

                                    }).Distinct().ToList();

                    if (apprCond.Count() > 0)
                    {
                        foreach (var x in apprCond)
                        {
                            var apprC = (from app in db.POMasterApprovers
                                         where app.ApprovalConditionId == x.UniqueId && app.IsDeleted == false
                                         select new { app.UniqueId }).Distinct().ToList();
                            if (apprC.Count() > 0)
                            {
                                sinfo.errorcode = 0;
                                sinfo.errormessage = "Approvers already exists with these Fund Center and Approval Condition";
                                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                            }

                        }
                    }

                    POMasterApprover approver = new POMasterApprover();
                    POApprovalCondition condition = new POApprovalCondition();
                    condition = pr.ApprovalCondition;
                    req = addTEPOApprovalCondition(condition);
                    req = addTEPOApprover(pr.MasterApproverlist, req);

                }

                if (req == "success")
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateTEPurchaseMasterApprovers(PurchaseApproversDTO pr)
        {
            try
            {
                if (pr != null)
                {
                    int lastmod = Convert.ToInt32(pr.ApprovalCondition.LastModifiedBy);
                    var user = db.UserProfiles.Where(u => u.UserId == lastmod).Select(u => u.UserName).FirstOrDefault();
                    POMasterApprover approver = new POMasterApprover();
                    POApprovalCondition condition = new POApprovalCondition();
                    condition = pr.ApprovalCondition;
                    POApprovalCondition AppCond = db.POApprovalConditions.Where(a => a.UniqueId == pr.ApprovalCondition.UniqueId && a.IsDeleted == false).FirstOrDefault();
                    if (AppCond != null)
                    {
                        AppCond.LastModifiedBy = pr.ApprovalCondition.LastModifiedBy;
                        AppCond.MinAmount = pr.ApprovalCondition.MinAmount;
                        AppCond.MaxAmount = pr.ApprovalCondition.MaxAmount;
                        AppCond.FundCenter = pr.ApprovalCondition.FundCenter;
                        db.Entry(AppCond).CurrentValues.SetValues(AppCond);
                        db.SaveChanges();
                    }
                    var masterApproverIds = db.POMasterApprovers.Where(a => a.ApprovalConditionId == pr.ApprovalCondition.UniqueId && a.IsDeleted == false).Select(a => a.UniqueId).ToList();
                    var newPOmasterApproverList = pr.MasterApproverlist.Select(a => a.UniqueId).ToList();
                    var nonintersectApproverIds = newPOmasterApproverList.Except(masterApproverIds).Union(masterApproverIds.Except(newPOmasterApproverList));
                    //soft deleting non-selected Master Approvers details
                    foreach (var masterApproverId in nonintersectApproverIds)
                    {
                        POMasterApprover deleteApprover = db.POMasterApprovers.Where(a => a.UniqueId == masterApproverId && a.IsDeleted == false).FirstOrDefault();
                        if (deleteApprover != null)
                        {
                            deleteApprover.IsDeleted = true;
                            deleteApprover.LastModifiedBy = pr.ApprovalCondition.LastModifiedBy;
                            deleteApprover.LastModifiedOn = DateTime.Now;
                            db.Entry(deleteApprover).CurrentValues.SetValues(deleteApprover);
                            db.SaveChanges();
                        }
                    }
                    foreach (var data in pr.MasterApproverlist)
                    {
                        if (data.ApproverId != null && data.ApproverId != 0)
                        {
                            POMasterApprover masterApproverCheck = db.POMasterApprovers.Where(a => a.UniqueId == data.UniqueId && a.IsDeleted == false).FirstOrDefault();
                            if (masterApproverCheck != null)
                            {
                                //  masterApproverCheck.LastModifiedBy = pr.ApprovalCondition.LastModifiedBy;
                                masterApproverCheck.LastModifiedBy = user;
                                masterApproverCheck.LastModifiedOn = DateTime.Now;
                                masterApproverCheck.SequenceId = data.SequenceId;
                                masterApproverCheck.ApproverId = data.ApproverId;
                                masterApproverCheck.IsDeleted = false;
                                masterApproverCheck.ApproverName = data.ApproverName;
                                db.Entry(masterApproverCheck).CurrentValues.SetValues(masterApproverCheck);
                                db.SaveChanges();
                            }
                            else
                            {
                                POMasterApprover AddmasterApprover = new POMasterApprover();
                                // AddmasterApprover.CreatedBy = pr.ApprovalCondition.LastModifiedBy;
                                AddmasterApprover.CreatedBy = user;

                                AddmasterApprover.CreatedOn = DateTime.Now;
                                AddmasterApprover.SequenceId = data.SequenceId;
                                AddmasterApprover.ApproverId = data.ApproverId;
                                AddmasterApprover.ApproverName = data.ApproverName;
                                AddmasterApprover.Type = data.Type;
                                AddmasterApprover.ApprovalConditionId = pr.ApprovalCondition.UniqueId;
                                db.POMasterApprovers.Add(AddmasterApprover);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Updated";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateTEPurchaseMasterApprovers_new(JObject json)
        {
            try
            {

                //int lastmod = Convert.ToInt32(pr.ApprovalCondition.LastModifiedBy);
                int lastmod = json["LastModifiedBy"].ToObject<int>();
                var user = db.UserProfiles.Where(u => u.UserId == lastmod).Select(u => u.UserName).FirstOrDefault();
                POMasterApprover approver = new POMasterApprover();
                POApprovalCondition condition = new POApprovalCondition();
                int unique = json["uniqueid"].ToObject<int>();

                var masterApproverIds = db.POMasterApprovers.Where(a => a.ApprovalConditionId == unique && a.IsDeleted == false).Select(a => a.UniqueId).ToList();


                foreach (var masterApproverId in masterApproverIds)
                {
                    POMasterApprover deleteApprover = db.POMasterApprovers.Where(a => a.UniqueId == masterApproverId && a.IsDeleted == false).FirstOrDefault();
                    if (deleteApprover != null)
                    {
                        deleteApprover.IsDeleted = true;
                        deleteApprover.LastModifiedBy = user;
                        deleteApprover.LastModifiedOn = DateTime.Now;
                        db.Entry(deleteApprover).CurrentValues.SetValues(deleteApprover);
                        db.SaveChanges();
                    }
                }



                List<int?> ApproverIDs = new List<int?>();
                if (json != null)
                    if (json["ApproverName"] != null)
                        if (json["ApproverName"].HasValues)
                        {
                            JToken json1 = json["ApproverName"];
                            if (json1 is JArray)
                            {

                                ApproverIDs = json["ApproverName"].ToObject<List<int?>>();
                            }

                        }


                int sequence = 2;

                foreach (int data in ApproverIDs)
                {
                    if (data != 0)
                    {
                        POMasterApprover AddmasterApprover = new POMasterApprover();
                        // AddmasterApprover.CreatedBy = pr.ApprovalCondition.LastModifiedBy;
                        var approvername = db.UserProfiles.Where(u => u.UserId == data).Select(u => u.UserName).FirstOrDefault();
                        AddmasterApprover.CreatedBy = user;

                        AddmasterApprover.CreatedOn = DateTime.Now;
                        AddmasterApprover.SequenceId = sequence;
                        AddmasterApprover.ApproverId = data;
                        AddmasterApprover.ApproverName = approvername;
                        AddmasterApprover.Type = Convert.ToString(json["Type"]);
                        AddmasterApprover.ApprovalConditionId = unique;
                        db.POMasterApprovers.Add(AddmasterApprover);
                        db.SaveChanges();
                        sequence++;
                    }

                }

                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Updated";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


        public string addTEPOApprovalCondition(POApprovalCondition obj)
        {
            try
            {
                obj.CreatedOn = System.DateTime.Now;
                obj.LastModifiedOn = System.DateTime.Now;
                obj = db.POApprovalConditions.Add(obj);
                db.SaveChanges();
                return obj.UniqueId.ToString();
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                return ex.Message;
            }
        }

        public string addTEPOApprover(List<POMasterApprover> li, string approverUniqueID)
        {
            try
            {
                if (approverUniqueID != null && li != null)
                {
                    foreach (var obj in li)
                    {
                        obj.CreatedOn = System.DateTime.Now;
                        obj.LastModifiedOn = System.DateTime.Now;
                        obj.ApprovalConditionId = Convert.ToInt32(approverUniqueID);
                        db.POMasterApprovers.Add(obj);
                        db.SaveChanges();
                    }
                    return "success";
                }
                else
                    return "null";
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                return ex.Message;
            }
        }

        [HttpPost]
        public HttpResponseMessage updateTEPurchase_MasterApprovers(PurchaseApproversDTO pr)
        {
            try
            {
                string req = string.Empty;
                if (pr != null)
                {
                    var apprCond = db.POApprovalConditions.Where(a => a.UniqueId == pr.ApprovalCondition.UniqueId && a.IsDeleted == false).FirstOrDefault();
                    apprCond.IsDeleted = true;
                    apprCond.LastModifiedOn = System.DateTime.Now;
                    db.Entry(apprCond).CurrentValues.SetValues(apprCond);
                    db.SaveChanges();
                    var PurApprList = (from cond in db.POApprovalConditions
                                       join apprl in db.POMasterApprovers on cond.UniqueId equals apprl.ApprovalConditionId
                                       where cond.IsDeleted == false && apprl.IsDeleted == false
                                       select apprl).Distinct().ToList();
                    foreach (POMasterApprover appr in PurApprList)
                    {
                        var apprObj = db.POMasterApprovers.Where(a => a.UniqueId == appr.UniqueId && a.IsDeleted == false).FirstOrDefault();
                        apprObj.IsDeleted = true;
                        apprObj.LastModifiedOn = System.DateTime.Now;
                        db.Entry(apprObj).CurrentValues.SetValues(apprObj);
                        db.SaveChanges();
                    }

                    POMasterApprover approver = new POMasterApprover();
                    POApprovalCondition condition = new POApprovalCondition();
                    condition = pr.ApprovalCondition;
                    req = addTEPOApprovalCondition(condition);
                    req = addTEPOApprover(pr.MasterApproverlist, req);
                }

                if (req == "success")
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

        }
        [HttpPost]
        public HttpResponseMessage GetCustomer(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            var list = db.TECompanies.Where(a => a.Uniqueid == id && a.IsDeleted == false).FirstOrDefault();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetAllProjects(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            var list = db.TEProjects.Where(d => d.CompanyID == id && d.IsDeleted == false).ToList();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetProject(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            TEProject project = db.TEProjects.Where(d => d.ProjectID == id && d.IsDeleted == false).FirstOrDefault();
            var obj = from TEP in db.TEProjects
                      where TEP.ProjectID == id
                      from
                      TEG in db.TEGSTNStateMasters
                          // where TEP.StateID == TEG.StateID
                      select new
                      {
                          Project = TEP,
                          State = TEG
                      };
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = obj

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetProjectDetails(JObject json)
        {
            int id = 0;
            id = json["ID"].ToObject<int>();
            var list = db.TEProjectDetails.Where(d => d.ProjectID == id && d.IsDeleted == false).FirstOrDefault();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetGeneralTermsConditions()
        {
            var list = db.TETermsAndConditions.Where(d => d.IsDeleted == false && d.IsActive == true).ToList();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage GetPOTypes_Nir()
        {
            var list = db.TEPurchase_OrderTypes.Where(d => d.IsDeleted == false).ToList();
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonContent(new
                {

                    result = list

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage POSubmitforApproval(Submitforapprovalreq requestObj)
        {
            int POUniqueId = requestObj.POUniqueId;
            string PurchaseOrderNumber = requestObj.PurchaseOrderNumber;
            string SubmitterComments = requestObj.SubmitterComments;
            int UserId = requestObj.UserId;
            //string shipTo = requestObj.shipTo;
            string response = "";
            try
            {
                TEPOHeaderStructure POStructure = db.TEPOHeaderStructures.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                #region validation for submission
                if (POStructure == null)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "PO details not found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                if (!(POStructure.POManagerID > 0))
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill Manager details of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                if (!(POStructure.VendorID > 0))
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill Vendor details of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                var ItemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).ToList();
                double? TotalPrice = 0;
                if (ItemStructure.Count == 0)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill all the details of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }

                else
                {
                    TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.TotalAmount));
                }
                var paymntTerms = db.TEPOVendorPaymentMilestones.Where(x => x.IsDeleted == false && x.POHeaderStructureId == POUniqueId).ToList();
                if (paymntTerms.Count == 0)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill PaymentTerms of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    double? paymntSum = 0;
                    paymntSum = paymntTerms.Where(a => a.Amount > 0).Sum(b => b.Amount);
                    double chkPymntTermSum = 0;
                    chkPymntTermSum = Math.Round(paymntSum.Value);
                    double chkTotalPrice = 0;
                    chkTotalPrice = Math.Round(TotalPrice.Value);

                    if (chkTotalPrice != chkPymntTermSum)
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "InComplete PO. Fill PaymentTerms of PO for 100% before Submit for approval";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }

                    //double? paymntPercentage = 0;
                    //paymntPercentage = paymntTerms.Where(a => a.Percentage > 0).Sum(b => b.Percentage);
                    //if (paymntPercentage < 100)
                    //{
                    //    sinfo.errorcode = 1;
                    //    sinfo.errormessage = "InComplete PO. Fill PaymentTerms of PO for 100% before Submit for approval";
                    //    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    //}
                    //else if (paymntPercentage > 100)
                    //{
                    //    sinfo.errorcode = 1;
                    //    sinfo.errormessage = "PaymentTerms of PO Crossed 100%. Fill Payment Terms for 100%  before Submit for approval";
                    //    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    //}
                }
                #endregion
                UserProfile user = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == UserId).FirstOrDefault();

                // approvers code
                var NextApprovers = (from u in db.TEPOApprovers
                                     where (u.POStructureId == POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0 && u.SequenceNumber != 1)
                                     orderby u.UniqueId
                                     select u).ToList();
                if (NextApprovers.Count == 0)
                {
                    //string FundCenter = "";
                    #region OLD PO
                    //TEPurchase_Item_Structure itemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && x.Item_Category == "D").FirstOrDefault();

                    //if (itemStructure != null)
                    //{
                    //    FundCenter = db.TEPOServices.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                    //         && x.Item_Number == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();

                    //}
                    //else
                    //{
                    //    itemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).FirstOrDefault();

                    //    FundCenter = db.TEPOAssignments.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                    //        && x.ItemNumber == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();
                    //}

                    //double TotalPrice = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == POUniqueId && (q.Condition_Type != "NAVS")
                    //              ).Sum(q => q.Condition_rate).Value;

                    //TotalPrice = TotalPrice * Convert.ToDouble(POStructure.Exchange_Rate);
                    //TotalPrice = Math.Round(TotalPrice);
                    //TotalPrice = Math.Truncate(TotalPrice);

                    //TEPurchase_FundCenter Fund = db.TEPOFundCenters.Where(x => x.IsDeleted == false && x.FundCenter_Code == FundCenter).FirstOrDefault();
                    ////TEPurchasingGroup PGroup = db.TEPurchasingGroups.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Group).FirstOrDefault();
                    //TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Document_Type).FirstOrDefault();
                    #endregion
                    String Valid_Val = SubmitValidation(POUniqueId, 0);
                    if (!String.IsNullOrEmpty(Valid_Val))
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = Valid_Val;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }

                    TEPOFundCenter Fund = db.TEPOFundCenters.Where(x => x.IsDeleted == false && x.Uniqueid == POStructure.FundCenterID).FirstOrDefault();

                    POApprovalCondition AppCon = null;
                    if (Fund != null)
                    {
                        AppCon = db.POApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == Fund.Uniqueid
                            && TotalPrice >= x.MinAmount && TotalPrice <= x.MaxAmount).FirstOrDefault();
                    }
                    if (AppCon != null)
                    {
                        List<POMasterApprover> MasterApprovers = db.POMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == AppCon.UniqueId && x.Type == "Approver").OrderBy(x => x.SequenceId).ToList();

                        if (MasterApprovers.Count > 0)
                        {
                            int count = 2;
                            foreach (var Appr in MasterApprovers)
                            {
                                TEPOApprover result = new TEPOApprover();
                                //count = count + 1;
                                string AprName = "Not Available";
                                result.CreatedOn = System.DateTime.Now;
                                result.LastModifiedOn = System.DateTime.Now;
                                result.CreatedBy = user.UserName;
                                result.LastModifiedBy = user.UserName;
                                //result.SequenceNumber = Convert.ToInt32(Appr.SequenceId);
                                result.SequenceNumber = Convert.ToInt32(count);
                                result.PurchaseOrderNumber = POStructure.Uniqueid.ToString();

                                AprName = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == Appr.ApproverId).Select(x => x.CallName).FirstOrDefault();

                                if (AprName == null)
                                {
                                    result.ApproverName = "Not Available";
                                }
                                else
                                {
                                    result.ApproverName = AprName;
                                }
                                result.Status = "Draft";
                                result.ApproverId = Appr.ApproverId;
                                result.POStructureId = POStructure.Uniqueid;
                                db.TEPOApprovers.Add(result);
                                db.SaveChanges();
                                count += 1;
                            }

                            db.TEPOHeaderStructures.Attach(POStructure);

                            POStructure.ReleaseCode2Status = "Pending For Approval";
                            db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                            POStructure.LastModifiedOn = DateTime.Now;
                            db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;
                            POStructure.CreatedOn = DateTime.Now;
                            db.Entry(POStructure).Property(x => x.CreatedOn).IsModified = true;

                            POStructure.LastModifiedBy = requestObj.UserId;
                            db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                            POStructure.SubmittedBy = UserId;
                            db.Entry(POStructure).Property(x => x.SubmittedBy).IsModified = true;

                            POStructure.SubmitterComments = SubmitterComments;
                            db.Entry(POStructure).Property(x => x.SubmitterComments).IsModified = true;

                            //POStructure.ShipTpCode = shipTo;
                            //db.Entry(POStructure).Property(x => x.ShipTpCode).IsModified = true;
                            db.SaveChanges();

                            //Current Approver
                            var CurrentApprover = (from u in db.TEPOApprovers
                                                   where (u.POStructureId == POUniqueId && u.IsDeleted == false && (u.SequenceNumber == 0))
                                                   orderby u.UniqueId
                                                   select u).FirstOrDefault();
                            db.TEPOApprovers.Attach(CurrentApprover);
                            CurrentApprover.LastModifiedOn = System.DateTime.Now;
                            db.Entry(CurrentApprover).Property(x => x.LastModifiedOn).IsModified = true;
                            CurrentApprover.LastModifiedBy = user.UserName;
                            db.Entry(CurrentApprover).Property(x => x.LastModifiedBy).IsModified = true;
                            CurrentApprover.ApprovedOn = System.DateTime.Now;
                            db.Entry(CurrentApprover).Property(x => x.ApprovedOn).IsModified = true;
                            CurrentApprover.Comments = SubmitterComments;
                            db.Entry(CurrentApprover).Property(x => x.Comments).IsModified = true;
                            CurrentApprover.Status = "Approved";
                            db.Entry(CurrentApprover).Property(x => x.Status).IsModified = true;
                            db.SaveChanges();

                            var POManager = (from u in db.TEPOApprovers
                                             where (u.POStructureId == POUniqueId && u.IsDeleted == false && u.SequenceNumber == 1)
                                             orderby u.UniqueId
                                             select u).FirstOrDefault();

                            db.TEPOApprovers.Attach(POManager);

                            POManager.Status = "Pending For Approval";
                            db.Entry(POManager).Property(x => x.Status).IsModified = true;

                            db.SaveChanges();

                            new EmailSendingBL().POEmail_SubmitForApproval(requestObj.POUniqueId, POStructure.CreatedBy, requestObj.UserId);

                            #region temporarily commented email notification
                            //try
                            //{






                            //    string VendorName = (db.TEPurchase_Vendor
                            //                              .Where(x => x.Vendor_Code == POStructure.Vendor_Account_Number)
                            //                              .Select(x => x.Vendor_Owner).FirstOrDefault());

                            //    var NextApproversList = (from u in db.TEPOApprovers
                            //                             join usr in db.UserProfiles on u.ApproverId equals usr.UserId
                            //                             where (u.POStructureId == POStructure.Uniqueid && u.IsDeleted == false && u.Status == "Pending For Approval" && u.SequenceNumber != 0)
                            //                             orderby u.UniqueId

                            //                             select new
                            //                             {
                            //                                 u.UniqueId,
                            //                                 u.SequenceNumber,
                            //                                 u.ApproverName,
                            //                                 usr.UserId,
                            //                                 u.Status
                            //                             }).ToList();

                            //    if (NextApproversList.Count > 0)
                            //    {
                            //        foreach (var item in NextApproversList)
                            //        {
                            //            UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();
                            //            UserProfile Submitter = db.UserProfiles.Where(x => x.UserId == POStructure.SubmittedBy).FirstOrDefault();
                            //            TEEmpBasicInfo emp = db.TEEmpBasicInfoes.Where(x => x.UserId == Submitter.UserName && x.IsDeleted == false).FirstOrDefault();
                            //            var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmit").ToList();
                            //            if (Emails.Count > 0)
                            //            {


                            //                TEEmailTemplate Email = new TEEmailTemplate();

                            //                Email.Subject = Emails[0].Subject.Replace("$VendorName", VendorName);
                            //                Email.EmailTemplate = Emails[0].EmailTemplate.Replace("$Employee", ToUser.CallName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POValue", TotalPrice.ToString());
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber ", POStructure.Purchasing_Order_Number);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$SubmitterName", Submitter.CallName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$EmpCode", emp.EmployeeId);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POTitle", POStructure.PO_Title);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POVersion", "R" + POStructure.Version);
                            //                SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);


                            //            }


                            //            var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                            //            SendNotification(item.UserId, "Purchase Order " + POStructure.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(), POStructure.Uniqueid);
                            //        }
                            //    }

                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            #endregion

                            response = "Success";
                        }
                        else
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Approvers are not available to this PO";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                    else
                    {

                        db.ApplicationErrorLogs.Add(

                         new ApplicationErrorLog
                         {
                             Error = "Approval Condition doesn't exist for po ",
                             ExceptionDateTime = System.DateTime.Now,
                             InnerException = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber,
                             Source = "From TEPODetailsController | POSubmitforApproval Mehod | " + this.GetType().ToString(),
                             Stacktrace = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber


                         }
                     );
                        db.SaveChanges();
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Approval Condition not exist for this PO";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    String Valid_Val = SubmitValidation(POUniqueId, 1);
                    if (!String.IsNullOrEmpty(Valid_Val))
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = Valid_Val;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    var currentAppr = (from usr in db.TEPOApprovers
                                       where usr.POStructureId == POUniqueId && usr.ApproverId == user.UserId && usr.IsDeleted == false && usr.SequenceNumber == 0
                                       select usr).FirstOrDefault();

                    int ApproverSquence = Convert.ToInt32(currentAppr.SequenceNumber);

                    var NextApproverList = (from u in db.TEPOApprovers
                                            where (u.SequenceNumber > ApproverSquence && u.POStructureId == POUniqueId && u.IsDeleted == false)
                                            orderby u.UniqueId
                                            select new
                                            {
                                                u.UniqueId,
                                                u.SequenceNumber,
                                                u.ApproverName,
                                                u.ReleaseCode,
                                                u.ApproverId
                                            }).ToList();

                    var NxtSeqApprover = NextApproverList.Where(a => a.SequenceNumber == ApproverSquence + 1).FirstOrDefault();

                    if (NxtSeqApprover.SequenceNumber == ApproverSquence + 1)
                    {
                        var curApprover = (from u in db.TEPOApprovers where (u.UniqueId == currentAppr.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(curApprover);

                        curApprover.Status = "Approved";
                        db.Entry(curApprover).Property(x => x.Status).IsModified = true;

                        curApprover.ApprovedOn = System.DateTime.Now;
                        db.Entry(curApprover).Property(x => x.ApprovedOn).IsModified = true;

                        curApprover.Comments = SubmitterComments;
                        db.Entry(curApprover).Property(x => x.Comments).IsModified = true;
                        db.SaveChanges();

                        var NextApprover = (from u in db.TEPOApprovers where (u.UniqueId == NxtSeqApprover.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(NextApprover);

                        NextApprover.Status = "Pending For Approval";
                        db.Entry(NextApprover).Property(x => x.Status).IsModified = true;
                        db.SaveChanges();
                    }
                    db.TEPOHeaderStructures.Attach(POStructure);

                    POStructure.ReleaseCode2Status = "Pending For Approval";
                    db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                    POStructure.LastModifiedOn = DateTime.Now;
                    db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;

                    POStructure.LastModifiedBy = requestObj.UserId;
                    db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                    POStructure.SubmittedBy = UserId;
                    db.Entry(POStructure).Property(x => x.SubmittedBy).IsModified = true;

                    POStructure.SubmitterComments = SubmitterComments;
                    db.Entry(POStructure).Property(x => x.SubmitterComments).IsModified = true;
                    db.SaveChanges();

                    new EmailSendingBL().POEmail_SubmitForApproval(requestObj.POUniqueId, POStructure.CreatedBy, requestObj.UserId);

                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Submitted for Approval";
            }
            catch (Exception ex)
            {
                {
                    ExceptionObj.RecordUnHandledException(ex);
                    response = ex.Message;
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed To Submit for Approval";
                }
            }

            return new HttpResponseMessage() { Content = new JsonContent(new { res = response, info = sinfo }) };
        }

        public String SubmitValidation(int headID, int Status)
        {
            TEPOHeaderStructure POStructure = db.TEPOHeaderStructures.Where(x => x.IsDeleted == false && x.Uniqueid == headID).FirstOrDefault();

            if (POStructure != null)
            {

                string Purchasing_Order_Number = POStructure.Purchasing_Order_Number;

                //SAPResponse sapRespnse = new SAPResponse();

                //if (String.IsNullOrEmpty(Purchasing_Order_Number))
                //{
                //    SetItemDataReadyForSAPPosting(headID, true);
                //    sapRespnse = new PurchaseOrderBAL().SendPODetailsToSAP(headID, true);
                //    if (sapRespnse.ReturnCode == "1")
                //        return sapRespnse.Message;
                //}
                //else
                //{
                //    SetItemDataReadyForSAPPosting(headID, false);
                //    sapRespnse = new PurchaseOrderBAL().UpdatePODetailsToSAP(headID, true);
                //    if (sapRespnse.ReturnCode == "1")
                //        return sapRespnse.Message;
                //}

                string PONumber = string.Empty;
                string CompanyCode = string.Empty;
                string VendorAccountNo = string.Empty;
                string DocumentTypeCode = string.Empty;
                string Currency = string.Empty;
                string ProjectCode = string.Empty;
                var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == POStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                var shippingLocation = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == POStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
                var CmpnyVendCode = (from purHead in db.TEPOHeaderStructures
                                     join proj in db.TEProjects on purHead.ProjectID equals proj.ProjectID
                                     join cmpny in db.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                                     join vendordtl in this.db.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                                     join vendor in this.db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                     join orderType in db.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                                     where
                                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                                      //&& vendor.IsDeleted == false && vendordtl.IsDeleted == false && orderType.IsDeleted == false &&
                                      purHead.Uniqueid == POStructure.Uniqueid
                                     select new
                                     {
                                         cmpny.CompanyCode,
                                         vendordtl.VendorCode,
                                         vendor.Currency,
                                         orderType.Code,
                                         proj.ProjectCode
                                     }).FirstOrDefault();
                if (CmpnyVendCode != null)
                {
                    CompanyCode = CmpnyVendCode.CompanyCode;
                    VendorAccountNo = CmpnyVendCode.VendorCode;
                    DocumentTypeCode = CmpnyVendCode.Code;
                    Currency = CmpnyVendCode.Currency;
                    ProjectCode = CmpnyVendCode.ProjectCode;
                }


                if (!String.IsNullOrEmpty(POStructure.Purchasing_Order_Number) && Convert.ToInt32(POStructure.Version) == 0)
                    return "For new PO creation: PO number should be blank (PO_NUMBER)";

                if (String.IsNullOrEmpty(POStructure.Purchasing_Order_Number) && Convert.ToInt32(POStructure.Version) > 0)
                    return "PO Number is not Present";

                if (String.IsNullOrEmpty(CompanyCode))
                    return "Company code should be mandatory (COMP_CODE)";

                if (String.IsNullOrEmpty(DocumentTypeCode))
                    return "Document Type is mandatory (DOC_TYPE)";

                if (String.IsNullOrEmpty(VendorAccountNo))
                    return "Vendor Code is Not present in the DB";

                if (String.IsNullOrEmpty(Currency))
                    return "Currency is Not Present";

                var pomanagerObj = db.UserProfiles.Where(a => a.UserId == POStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
                if (pomanagerObj == null)
                    return "Project Manager is not Present";

                if (String.IsNullOrEmpty(POStructure.PO_Title))
                    return "PO Title Not Present";

                if (String.IsNullOrEmpty(ProjectCode))
                    return "Project Code is Not Present";


                var Items = db.TEPOItemStructures.Where(x => x.POStructureId == POStructure.Uniqueid && x.IsDeleted == false).ToList();
                foreach (var SubItems in Items)
                {
                    //if (string.IsNullOrEmpty(SubItems.Tax_salespurchases_code))
                    //    return "Tax Code Not Present";

                    if (SubItems.ItemType == "MaterialOrder" || SubItems.ItemType == "ExpenseOrder")
                    {
                        if (string.IsNullOrEmpty(SubItems.GLAccountNo))
                            return "GL Account is Not Present";
                    }
                    if (String.IsNullOrEmpty(plantStorage.PlantStorageCode))
                        return "Plant Code is Not Present";

                    if (String.IsNullOrEmpty(shippingLocation.StorageLocationCode))
                        return "Storage Location is Not Present";

                    if (String.IsNullOrEmpty(SubItems.Order_Qty))
                        return "Quantity is Not Present";

                    if (String.IsNullOrEmpty(SubItems.Unit_Measure))
                        return "UOM is Not Present";

                    if (SubItems.ItemType == "MaterialOrder")
                    {
                        if (String.IsNullOrEmpty(SubItems.Material_Number))
                        {
                            if (String.IsNullOrEmpty(SubItems.Material_Group))
                                return "Material Number and Material Group Not Present";
                        }
                    }
                    if (SubItems.ItemType == "ExpenseOrder")
                    {
                        if (SubItems.TotalAmount == 0)
                            return "Limit and Exp Value is not Present";

                    }
                    if (SubItems.ItemType == "MaterialOrder" || SubItems.ItemType == "ExpenseOrder")
                    {
                        if (String.IsNullOrEmpty(SubItems.WBSElementCode))
                            return "WBS Element is Not Present";
                    }

                    if (SubItems.ItemType == "ServiceOrder")
                    {
                        //if (String.IsNullOrEmpty(SubItems.Material_Number))
                        //    return "Service Activity Code is Not Present";

                        if (SubItems.Rate == 0 || SubItems.Rate == null)
                            return "Rate is Not Present for the Service";

                        if (String.IsNullOrEmpty(SubItems.WBSElementCode))
                            return "WBS Element is Not Present for this service";

                        if (string.IsNullOrEmpty(SubItems.GLAccountNo))
                            return "GL Account is Not Present for this Service";

                    }
                }            //        string PONumber = string.Empty;
                             //string CompanyCode = string.Empty;
                             //string VendorAccountNo = string.Empty;
                             //string DocumentTypeCode = string.Empty;
                             //string Currency = string.Empty;
                             //string ProjectCode = string.Empty;
                             //var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == POStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                             //var shippingLocation = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == POStructure.ShippedToID && a.isdeleted == false).FirstOrDefault();
                             //var CmpnyVendCode = (from purHead in db.TEPOHeaderStructures
                             //                     join proj in db.TEProjects on purHead.ProjectID equals proj.ProjectID
                             //                     join cmpny in db.TECompanies on proj.CompanyID equals cmpny.Uniqueid
                             //                     join vendordtl in this.db.TEPOVendorMasterDetails on purHead.VendorID equals vendordtl.POVendorDetailId
                             //                     join vendor in this.db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                             //                     join orderType in db.TEPurchase_OrderTypes on purHead.PO_OrderTypeID equals orderType.UniqueId
                             //                     where
                             //                      //proj.IsDeleted == false && cmpny.IsDeleted == false
                             //                      //&& vendor.IsDeleted == false && vendordtl.IsDeleted == false && orderType.IsDeleted == false &&
                             //                      purHead.Uniqueid == POStructure.Uniqueid
                             //                     select new
                             //                     {
                             //                         cmpny.CompanyCode,
                             //                         vendordtl.VendorCode,
                             //                         vendor.Currency,
                             //                         orderType.Code,
                             //                         proj.ProjectCode
                             //                     }).FirstOrDefault();
                             //if (CmpnyVendCode != null)
                             //{
                             //    CompanyCode = CmpnyVendCode.CompanyCode;
                             //    VendorAccountNo = CmpnyVendCode.VendorCode;
                             //    DocumentTypeCode = CmpnyVendCode.Code;
                             //    Currency = CmpnyVendCode.Currency;
                             //    ProjectCode = CmpnyVendCode.ProjectCode;
                             //}


                //if (!String.IsNullOrEmpty(POStructure.Purchasing_Order_Number) && Convert.ToInt32(POStructure.Version) == 0)
                //    return "For new PO creation: PO number should be blank (PO_NUMBER)";

                //if (String.IsNullOrEmpty(POStructure.Purchasing_Order_Number) && Convert.ToInt32(POStructure.Version) > 0)
                //    return "PO Number is not Present";

                //if (String.IsNullOrEmpty(CompanyCode))
                //    return "Company code should be mandatory (COMP_CODE)";

                //if (String.IsNullOrEmpty(DocumentTypeCode))
                //    return "Document Type is mandatory (DOC_TYPE)";

                //if (String.IsNullOrEmpty(VendorAccountNo))
                //    return "Vendor Code is Not present in the DB";

                //if (String.IsNullOrEmpty(Currency))
                //    return "Currency is Not Present";

                //var pomanagerObj = db.UserProfiles.Where(a => a.UserId == POStructure.POManagerID && a.IsDeleted == false).FirstOrDefault();
                //if (pomanagerObj == null)
                //    return "Project Manager is not Present";

                //if (String.IsNullOrEmpty(POStructure.PO_Title))
                //    return "PO Title Not Present";

                //if (String.IsNullOrEmpty(ProjectCode))
                //    return "Project Code is Not Present";


                //var Items = db.TEPOItemStructures.Where(x => x.POStructureId == POStructure.Uniqueid && x.IsDeleted == false).ToList();
                //foreach (var SubItems in Items)
                //{
                //    //if (string.IsNullOrEmpty(SubItems.Tax_salespurchases_code))
                //    //    return "Tax Code Not Present";

                //    if (SubItems.ItemType == "MaterialOrder" || SubItems.ItemType == "ExpenseOrder")
                //    {
                //        if (string.IsNullOrEmpty(SubItems.GLAccountNo))
                //            return "GL Account is Not Present";
                //    }
                //    if (String.IsNullOrEmpty(plantStorage.PlantStorageCode))
                //        return "Plant Code is Not Present";

                //    if (String.IsNullOrEmpty(shippingLocation.StorageLocationCode))
                //        return "Storage Location is Not Present";

                //    if (String.IsNullOrEmpty(SubItems.Order_Qty))
                //        return "Quantity is Not Present";

                //    if (String.IsNullOrEmpty(SubItems.Unit_Measure))
                //        return "UOM is Not Present";

                //    if (SubItems.ItemType == "MaterialOrder")
                //    {
                //        if (String.IsNullOrEmpty(SubItems.Material_Number))
                //        {
                //            if (String.IsNullOrEmpty(SubItems.Material_Group))
                //                return "Material Number and Material Group Not Present";
                //        }
                //    }
                //    if (SubItems.ItemType == "ExpenseOrder")
                //    {
                //        if (SubItems.TotalAmount == 0)
                //            return "Limit and Exp Value is not Present";

                //    }
                //    if (SubItems.ItemType == "MaterialOrder" || SubItems.ItemType == "ExpenseOrder")
                //    {
                //        if (String.IsNullOrEmpty(SubItems.WBSElementCode))
                //            return "WBS Element is Not Present";
                //    }

                //    if (SubItems.ItemType == "ServiceOrder")
                //    {
                //        //if (String.IsNullOrEmpty(SubItems.Material_Number))
                //        //    return "Service Activity Code is Not Present";

                //        if (SubItems.Rate == 0 || SubItems.Rate == null)
                //            return "Rate is Not Present for the Service";

                //        if (String.IsNullOrEmpty(SubItems.WBSElementCode))
                //            return "WBS Element is Not Present for this service";

                //        if (string.IsNullOrEmpty(SubItems.GLAccountNo))
                //            return "GL Account is Not Present for this Service";

                //    }
            }
            return String.Empty;


        }

        [HttpPost]
        public HttpResponseMessage POApproval(ApprovalReq value)
        {
            ApprovalReq result = value;

            //Approve
            //Edit           
            var result1 = (from u in db.TEPOHeaderStructures
                           where (u.Purchasing_Order_Number == value.PurchaseOrderNumber && u.Uniqueid == result.POUniqueId)
                           select u).FirstOrDefault();
            var uniqid = result1.Uniqueid;

            //var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
            var usrval = (from usr in db.UserProfiles
                          join terel in db.TEPOReleaseCodes on usr.CallName equals terel.ReleaseCode2By
                          //where (terel.Release_Code2 == result.ReleaseCode)
                          select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

            var apprsList = (from usr in db.TEPOApprovers
                             where usr.POStructureId == uniqid && usr.Status != "Approved" && usr.IsDeleted == false
                             select usr).ToList();

            var currentAppr = (from usr in db.TEPOApprovers
                               where usr.POStructureId == uniqid && usr.ApproverId == result.UserId && usr.IsDeleted == false
                               select usr).FirstOrDefault();

            if (apprsList.Count == 1)
            {
                result1.ReleaseCode2Status = "Approved";
                db.Entry(result1).Property(x => x.ReleaseCode2Status).IsModified = true;
                db.SaveChanges();

                //var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();

                //dbuser.ReleaseCode2Status = "Approved";
                //db.Entry(dbuser).Property(x => x.ReleaseCode2Status).IsModified = true;

                //dbuser.ReleaseCode2Date = System.DateTime.Now;
                //db.Entry(dbuser).Property(x => x.ReleaseCode2Date).IsModified = true;

                //db.SaveChanges();
            }
            currentAppr.Status = "Approved";
            db.Entry(currentAppr).Property(x => x.LastModifiedBy).IsModified = true;
            db.SaveChanges();
            return new HttpResponseMessage
            {
                Content = new JsonContent(new
                {

                    res = result1.Uniqueid

                })
            };
        }

        [HttpPost]
        public HttpResponseMessage POApprove(ApprovalReq value)
        {
            ApprovalReq result = value;
            try
            {
                //Approve
                //Edit           
                var result1 = (from u in db.TEPOHeaderStructures
                               where (u.Uniqueid == result.POUniqueId)
                               select u).FirstOrDefault();
                var uniqid = result1.Uniqueid;
                int currentVersion = 0;

                var usrval = (from usr in db.UserProfiles
                              where (usr.UserId == result.UserId)
                              select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

                var currentAppr = (from usr in db.TEPOApprovers
                                   where usr.POStructureId == uniqid && usr.ApproverId == result.UserId
                                   && usr.IsDeleted == false && usr.SequenceNumber != 0
                                   && usr.Status == "Pending For Approval"
                                   select usr).FirstOrDefault();

                int ApproverSquence = Convert.ToInt32(currentAppr.SequenceNumber);

                var NextApprovers = (from u in db.TEPOApprovers
                                     where (u.SequenceNumber > ApproverSquence && u.POStructureId == value.POUniqueId && u.IsDeleted == false)
                                     orderby u.UniqueId
                                     select new
                                     {
                                         u.UniqueId,
                                         u.SequenceNumber,
                                         u.ApproverName,
                                         u.ReleaseCode,
                                         u.ApproverId
                                     }).ToList();
                if (NextApprovers.Count() > 0)
                {
                    var NxtSeqApprover = NextApprovers.Where(a => a.SequenceNumber == ApproverSquence + 1).FirstOrDefault();

                    if (NxtSeqApprover.SequenceNumber == ApproverSquence + 1)
                    {
                        var curApprover = (from u in db.TEPOApprovers where (u.UniqueId == currentAppr.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(curApprover);

                        curApprover.Status = "Approved";
                        db.Entry(curApprover).Property(x => x.Status).IsModified = true;

                        curApprover.ApprovedOn = System.DateTime.Now;
                        db.Entry(curApprover).Property(x => x.ApprovedOn).IsModified = true;

                        curApprover.Comments = value.SubmitterComments;
                        db.Entry(curApprover).Property(x => x.Comments).IsModified = true;
                        db.SaveChanges();

                        var NextApprover = (from u in db.TEPOApprovers where (u.UniqueId == NxtSeqApprover.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(NextApprover);

                        NextApprover.Status = "Pending For Approval";
                        db.Entry(NextApprover).Property(x => x.Status).IsModified = true;
                        db.SaveChanges();
                        new EmailSendingBL().POEmail_Approved(result.POUniqueId, result1.CreatedBy, result.UserId);
                    }

                }
                else
                {
                    currentVersion = Convert.ToInt32(result1.Version);


                    string prevversion = (currentVersion - 1).ToString();

                    if ((!string.IsNullOrEmpty(result1.Purchasing_Order_Number) && currentVersion > 0))
                    {
                        string PONumber = string.Empty;
                        var prevHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Purchasing_Order_Number == result1.Purchasing_Order_Number
                                                                                         && a.Version == prevversion && a.IsDeleted == false).FirstOrDefault();
                        SetItemDataReadyForSAPPosting(value.POUniqueId, false);
                        var sapRespnse = new PurchaseOrderBAL().UpdatePODetailsToSAP(value.POUniqueId);
                        if (sapRespnse != null)
                        {
                            PONumber = sapRespnse.PONumber;
                        }
                        if (!string.IsNullOrEmpty(PONumber))
                        {
                            var NextApprover = (from u in db.TEPOApprovers where (u.UniqueId == currentAppr.UniqueId) select u);

                            var dbuser1 = NextApprover.First();
                            db.TEPOApprovers.Attach(dbuser1);

                            dbuser1.Status = "Approved";
                            db.Entry(dbuser1).Property(x => x.Status).IsModified = true;

                            dbuser1.ApprovedOn = System.DateTime.Now;
                            db.Entry(dbuser1).Property(x => x.ApprovedOn).IsModified = true;

                            dbuser1.Comments = value.SubmitterComments;
                            db.Entry(dbuser1).Property(x => x.Comments).IsModified = true;
                            db.SaveChanges();


                            var PO = (from u in db.TEPOHeaderStructures where (u.Uniqueid == value.POUniqueId) select u);

                            var dbuser2 = PO.First();
                            db.TEPOHeaderStructures.Attach(dbuser2);

                            dbuser2.ReleaseCode2Status = "Approved";
                            //dbuser2.Purchasing_Document_Date = DateTime.Now;
                            db.Entry(dbuser2).Property(x => x.ReleaseCode2Status).IsModified = true;

                            db.SaveChanges();

                            if (prevHeadrStructure != null)
                            {
                                prevHeadrStructure.Status = "Superseded";
                                db.Entry(prevHeadrStructure).CurrentValues.SetValues(prevHeadrStructure);
                                db.SaveChanges();
                            }
                            new EmailSendingBL().POEmail_Approved(result.POUniqueId, result1.CreatedBy, result.UserId);
                        }
                        else
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Update PO Number in SAP.Exception:" + sapRespnse.Message;
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                    else
                    {
                        string PONumber = string.Empty;
                        SetItemDataReadyForSAPPosting(value.POUniqueId, true);

                        var sapRespnse = new PurchaseOrderBAL().SendPODetailsToSAP(value.POUniqueId);
                        if (sapRespnse != null)
                        {
                            PONumber = sapRespnse.PONumber;
                        }
                        if (!string.IsNullOrEmpty(PONumber))
                        {
                            var NextApprover = (from u in db.TEPOApprovers where (u.UniqueId == currentAppr.UniqueId) select u);

                            var dbuser1 = NextApprover.First();
                            db.TEPOApprovers.Attach(dbuser1);

                            dbuser1.Status = "Approved";
                            db.Entry(dbuser1).Property(x => x.Status).IsModified = true;

                            dbuser1.ApprovedOn = System.DateTime.Now;
                            db.Entry(dbuser1).Property(x => x.ApprovedOn).IsModified = true;

                            dbuser1.Comments = value.SubmitterComments;
                            db.Entry(dbuser1).Property(x => x.Comments).IsModified = true;
                            db.SaveChanges();


                            var PO = (from u in db.TEPOHeaderStructures where (u.Uniqueid == value.POUniqueId) select u);

                            var dbuser2 = PO.First();
                            db.TEPOHeaderStructures.Attach(dbuser2);
                            dbuser2.Purchasing_Document_Date = DateTime.Now;
                            dbuser2.ReleaseCode2Status = "Approved";
                            dbuser2.ApprovedOn = DateTime.Today.ToShortDateString();
                            dbuser2.ApprovedBy = dbuser1.ApproverName;
                            db.Entry(dbuser2).Property(x => x.ReleaseCode2Status).IsModified = true;

                            db.SaveChanges();
                            new EmailSendingBL().POEmail_Approved(result.POUniqueId, result1.CreatedBy, result.UserId);

                            //var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == result.POUniqueId && a.IsDeleted==false).ToList();
                            //if (itemList.Count > 0)
                            //{
                            //    foreach (TEPOItemStructure item in itemList)
                            //    {
                            //        if (item.ItemType == "MaterialOrder")
                            //        {
                            //            UpdateMaterialRateRequest materialRateReq = new UpdateMaterialRateRequest();
                            //            materialRateReq.MaterialCode = item.Material_Number;
                            //            MaterialPurchaseDetail purchdtl = new MaterialPurchaseDetail();
                            //            purchdtl.Currency = "INR";
                            //            purchdtl.Amount = item.Rate.ToString();
                            //            materialRateReq.LastPurchaseRate = purchdtl;
                            //            decimal? wghtdAvg= GetWeightedAverageForItem(item.Material_Number);
                            //            MaterialPurchaseDetail weighdtl = new MaterialPurchaseDetail();
                            //            weighdtl.Currency = "INR";
                            //            weighdtl.Amount = wghtdAvg;
                            //            materialRateReq.WeightedAveragePurchaseRate = weighdtl;
                            //            UpdateMaterialLastPurchaseRateToCL(materialRateReq);
                            //        }
                            //        else if (item.ItemType == "ServiceOrder")
                            //        {
                            //            UpdateServiceRateRequest serviceRateReq = new UpdateServiceRateRequest();                                       
                            //            ServicePurchaseDetail purchdtl = new ServicePurchaseDetail();
                            //            purchdtl.currency = "INR";
                            //            purchdtl.amount = item.Rate.ToString();
                            //            serviceRateReq.lastPurchaseRate = purchdtl;
                            //            decimal? wghtdAvg= GetWeightedAverageForItem(item.Material_Number);
                            //            ServicePurchaseDetail weighdtl = new ServicePurchaseDetail();
                            //            weighdtl.currency = "INR";
                            //            weighdtl.amount = wghtdAvg;
                            //            serviceRateReq.weightedAveragePurchaseRate = weighdtl;
                            //            UpdateServiceLastPurchaseRateToCL(serviceRateReq, item.Material_Number);                                                                            
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Failed to Generate PO Number in SAP.Exception:" + sapRespnse.Message;
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }

                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Approved";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Approve";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }

        #region Author:[Kamal] Changed for the Service Order Posting.
        public void SetItemDataReadyForSAPPosting(int headerUniqueId, bool IsFirstTime)
        {
            POPDFModel PODetails = new POPDFModel();
            var HeadOrderType = (from Head in db.TEPOHeaderStructures
                                 join OrdType in db.TEPurchase_OrderTypes on Head.PO_OrderTypeID equals OrdType.UniqueId
                                 where Head.Uniqueid == headerUniqueId
                                 select OrdType.Code).FirstOrDefault();

            var HeaderList = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == headerUniqueId).OrderBy(x => x.UniqueID).ToList();
            #region For Service
            //if (HeaderList.Count > 0 && (HeadOrderType == "NB"))
            //{
            //    if (IsFirstTime)
            //    {
            //        #region For Service

            //        foreach (TEPOServiceHeader Head in HeaderList)
            //        {
            //            int fugueSeqCount = 10;
            //            var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.ServiceHeaderId == Head.UniqueID && a.IsDeleted == false).ToList();

            //            if (itemList.Count > 0)
            //            {
            //                foreach (TEPOItemStructure item in itemList)
            //                {
            //                    var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
            //                    itemStrc.FugueItemSeqNo = fugueSeqCount;
            //                    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
            //                    db.SaveChanges();
            //                    fugueSeqCount += 10;
            //                }
            //            }

            //        }

            //        foreach (TEPOServiceHeader Head in HeaderList)
            //        {
            //            int sapSeqCount = 10;
            //            var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.ServiceHeaderId == Head.UniqueID && a.IsDeleted == false)
            //                           .OrderBy(b => b.FugueItemSeqNo).ToList();
            //            if (itemListForSAP.Count > 0)
            //            {
            //                foreach (TEPOItemStructure itemSAP in itemListForSAP)
            //                {

            //                    var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid).FirstOrDefault();
            //                    itemStrcSAP.SAPItemSeqNo = sapSeqCount;
            //                    db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
            //                    db.SaveChanges();
            //                    sapSeqCount += 10;

            //                }
            //            }
            //        }
            //    }
            //    #endregion
            //    else
            //    {
            //        #region Service

            //        foreach (TEPOServiceHeader Head in HeaderList)
            //        {
            //            var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.ServiceHeaderId == Head.UniqueID).OrderBy(a => a.Uniqueid).ToList();
            //            if (itemList.Count > 0)
            //            {
            //                int maxFugueSeqNo = 10; int? maxFugItemNo = 10;
            //                //maxFugItemNo = itemList.Max(a => a.FugueItemSeqNo);
            //                //maxFugueSeqNo = Convert.ToInt32(maxFugItemNo) + 10;
            //                foreach (TEPOItemStructure item in itemList)
            //                {

            //                    var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
            //                    itemStrc.FugueItemSeqNo = maxFugueSeqNo;
            //                    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
            //                    db.SaveChanges();
            //                    maxFugueSeqNo += 10;

            //                }
            //            }
            //        }

            //        foreach (TEPOServiceHeader Head in HeaderList)
            //        {
            //            var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.ServiceHeaderId == Head.UniqueID)
            //                                                                .OrderBy(b => b.FugueItemSeqNo).ToList();
            //            if (itemListForSAP.Count > 0)
            //            {
            //                int? maxSApSeqNo = 10;
            //                var CurrentHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerUniqueId && a.IsDeleted == false).FirstOrDefault();
            //                if (CurrentHeadrStructure != null)
            //                {
            //                    int prevVersion = Convert.ToInt32(CurrentHeadrStructure.Version) - 1;
            //                    string prVer = prevVersion.ToString();
            //                    var prevHStructure = db.TEPOHeaderStructures.Where(a => a.Purchasing_Order_Number == CurrentHeadrStructure.Purchasing_Order_Number
            //                                                                                         && a.Version == prVer && a.IsDeleted == false).FirstOrDefault();
            //                    var prevItemList = db.TEPOItemStructures.Where(a => a.POStructureId == prevHStructure.Uniqueid && a.ServiceHeaderId == Head.UniqueID).OrderBy(a => a.Uniqueid).ToList();

            //                    maxSApSeqNo = prevItemList.Max(a => a.SAPItemSeqNo);
            //                    maxSApSeqNo += 10;
            //                }
            //                //maxSApSeqNo = itemListForSAP.Max(a => a.SAPItemSeqNo);
            //                //maxSApSeqNo += 1;
            //                maxSApSeqNo = 10;
            //                foreach (TEPOItemStructure itemSAP in itemListForSAP)
            //                {
            //                    var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid && a.ServiceHeaderId == Head.UniqueID).FirstOrDefault();
            //                    if (itemStrcSAP.SAPItemSeqNo > 0)
            //                    {
            //                        if (itemStrcSAP.IsDeleted == true && itemStrcSAP.IsRecordInSAP == true)
            //                            itemStrcSAP.SAPTransactionCode = "X";
            //                        itemStrcSAP.SAPItemSeqNo = maxSApSeqNo;
            //                        maxSApSeqNo += 10;
            //                    }
            //                    else
            //                    {
            //                        if (itemStrcSAP.IsDeleted == false && itemStrcSAP.IsRecordInSAP != true)
            //                        {
            //                            itemStrcSAP.SAPItemSeqNo = maxSApSeqNo;
            //                            itemStrcSAP.SAPTransactionCode = "C";
            //                            maxSApSeqNo += 10;
            //                        }
            //                        else
            //                        {
            //                            itemStrcSAP.SAPItemSeqNo = maxSApSeqNo;

            //                            maxSApSeqNo += 10;
            //                        }
            //                    }


            //                    db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
            //                    db.SaveChanges();
            //                }
            //            }
            //        }

            //        #endregion
            //    }
            //}
            #endregion
            //else
            //{
            PODetails = new PurchaseOrderBAL().GetDataForPOPDF(headerUniqueId);
            List<Mat_Serv_Seq> SeqList = this.Seq_Mat_Service(PODetails, headerUniqueId);
            if (IsFirstTime == true)
            {
                int fugueSeqCount = 1;
                var prevHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerUniqueId && a.IsDeleted == false).FirstOrDefault();


                #region First Time

                if (SeqList.Count > 0 && SeqList != null)
                {
                    //var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId).OrderBy(x => x.Uniqueid).ToList();
                    //if (itemList.Count > 0)
                    //{
                    Dictionary<int?, String> ServItemNumber = new Dictionary<int?, string>();
                    foreach (Mat_Serv_Seq item in SeqList)
                    {
                        //Material and Expense Order Sequence Creation
                        if (item.ServHead == null)
                        {
                            foreach (PurchaseItemStructureDetail POStructure in item.ItemS)
                            {
                                //if (POStructure.SAPItemSeqNo < 1 || POStructure.SAPItemSeqNo == null)
                                //{
                                    var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == POStructure.ItemUniqueId).FirstOrDefault();
                                //if (itemStrc.Item_Number == "0" || String.IsNullOrEmpty(itemStrc.Item_Number))
                                //{
                                        itemStrc.Item_Number = fugueSeqCount.ToString();
                                        itemStrc.FugueItemSeqNo = fugueSeqCount;
                                        itemStrc.SAPItemSeqNo = fugueSeqCount;
                                    //}
                                    //else
                                    //{
                                    //    itemStrc.FugueItemSeqNo = Convert.ToInt32(itemStrc.Item_Number);
                                    //    itemStrc.SAPItemSeqNo = Convert.ToInt32(itemStrc.Item_Number);
                                    //}
                                    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                    db.SaveChanges();
                                    fugueSeqCount++;
                               // }
                            }
                        }
                        //Service Order Sequence Creation
                        else if (item.ServHead != null)
                        {
                            //int MaxLine = Convert.ToInt32(item.ItemS.Max(x => Convert.ToInt32(x.SAPItemSeqNo)));
                            int SAPServCount =  10;
                            foreach (PurchaseItemStructureDetail POServItems in item.ItemS)
                            {

                                TEPOItemStructure itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == POServItems.ItemUniqueId).FirstOrDefault();
                                    //if (String.IsNullOrEmpty(item.ServHead.ItemNumber) || item.ServHead.ItemNumber == "0")
                                    //{
                                        //itemStrc.FugueItemSeqNo = fugueSeqCount;
                                        //itemStrc.SAPItemSeqNo = fugueSeqCount;
                                        if (!ServItemNumber.ContainsKey(itemStrc.ServiceHeaderId))
                                        {
                                            TEPOServiceHeader ServHead = db.TEPOServiceHeaders.Where(x => x.UniqueID == item.ServHead.UniqueID).FirstOrDefault();
                                            ServHead.ItemNumber = fugueSeqCount.ToString();
                                            db.Entry(ServHead).CurrentValues.SetValues(ServHead);
                                            db.SaveChanges();

                                        itemStrc.Item_Number = fugueSeqCount.ToString();
                                            
                                            ServItemNumber.Add(itemStrc.ServiceHeaderId, itemStrc.Item_Number);
                                        fugueSeqCount++;
                                    }
                                        else
                                        {
                                            itemStrc.Item_Number = ServItemNumber[itemStrc.ServiceHeaderId];
                                        }

                                    itemStrc.FugueItemSeqNo = SAPServCount;
                                    itemStrc.SAPItemSeqNo = SAPServCount;
                                    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                    db.SaveChanges();
                                    SAPServCount += 10;
                                #region Comment Section

                                //}
                                //else
                                //{
                                //    //itemStrc.FugueItemSeqNo = Convert.ToInt32(item.ServHead.ItemNumber);
                                //    //itemStrc.SAPItemSeqNo = Convert.ToInt32(item.ServHead.ItemNumber);
                                //    itemStrc.Item_Number = item.ServHead.ItemNumber;

                                //}
                                //if (Convert.ToInt32(POServItems.SAPItemSeqNo) < 1 || POServItems.SAPItemSeqNo == null)
                                //{ 
                                //    itemStrc.FugueItemSeqNo = SAPServCount;
                                //    itemStrc.SAPItemSeqNo = SAPServCount;
                                //}
                                //itemStrc.LINEITEMNUMBER = SAPServCount.ToString();


                                //}
                                //else if (Convert.ToInt32(POServItems.SAPItemSeqNo) < 1 || String.IsNullOrEmpty(POServItems.SAPItemSeqNo.ToString()))
                                //{
                                //    var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == POServItems.ItemUniqueId).FirstOrDefault();

                                //    itemStrc.FugueItemSeqNo = SAPServCount;
                                //    itemStrc.SAPItemSeqNo = SAPServCount;
                                //    //itemStrc.LINEITEMNUMBER = SAPServCount.ToString();
                                //    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                //    db.SaveChanges();
                                //    SAPServCount += 10;
                                //}
                                
                                    #endregion Comment Section
                            }

                        }
                        //}
                    }

                    //var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.IsDeleted == false)
                    //                    .OrderBy(b => b.FugueItemSeqNo).ToList();
                    //if (itemListForSAP.Count > 0)
                    //{
                    //    foreach (TEPOItemStructure itemSAP in itemListForSAP)
                    //    {
                    //        if (itemSAP.SAPItemSeqNo > 0)
                    //        {
                    //        }
                    //        else
                    //        {
                    //            var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid).FirstOrDefault();
                    //            itemStrcSAP.SAPItemSeqNo = sapSeqCount;
                    //            db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                    //            db.SaveChanges();
                    //            sapSeqCount += 1;
                    //        }
                    //    }
                    //}

                    #endregion
                }
            }
            else
            {
                #region Update
                //For Material Max Value

                int maxFugueSeqNo = this.SetMat_Serv_Seq(headerUniqueId);
                foreach (Mat_Serv_Seq Servitem in SeqList)
                {
                    //For Material and Expense Order
                    if (Servitem.ServHead == null)
                    {
                        foreach (PurchaseItemStructureDetail item in Servitem.ItemS)
                        {
                            if (item.SAPItemSeqNo < 1 || item.SAPItemSeqNo == null)
                            {
                                TEPOItemStructure itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.ItemUniqueId).FirstOrDefault();
                                itemStrc.FugueItemSeqNo = maxFugueSeqNo;
                                itemStrc.SAPItemSeqNo = maxFugueSeqNo;
                                itemStrc.Item_Number = maxFugueSeqNo.ToString();
                                if (itemStrc.IsRecordInSAP != true && itemStrc.IsDeleted == false)
                                    itemStrc.SAPTransactionCode = "C";

                                db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                db.SaveChanges();
                                maxFugueSeqNo += 1;
                            }
                            else if(Convert.ToInt32(item.ItemNo) < 1 || String.IsNullOrEmpty(item.ItemNo))
                            {
                                TEPOItemStructure itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.ItemUniqueId).FirstOrDefault();
                                itemStrc.Item_Number = itemStrc.SAPItemSeqNo.ToString();
                                db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                db.SaveChanges();
                            }
                        }
                    }
                    // For Service Order
                    else if (Servitem.ServHead != null )
                    {
                        Dictionary<int, String> ServValues = new Dictionary<int, String>();
                        foreach (PurchaseItemStructureDetail item in Servitem.ItemS)
                        {
                            if (Convert.ToInt32(item.ItemNo) < 1 || String.IsNullOrEmpty(item.ItemNo))
                            {
                                var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.ItemUniqueId).FirstOrDefault();
                                var ServHead = db.TEPOServiceHeaders.Where(a => a.UniqueID == itemStrc.ServiceHeaderId).FirstOrDefault();

                                int? MaxitemStrcLine = db.TEPOItemStructures.Where(a => a.ServiceHeaderId == itemStrc.ServiceHeaderId && a.Uniqueid != item.ItemUniqueId).Max(x => x.SAPItemSeqNo);
                                if (!ServValues.ContainsKey(ServHead.UniqueID))
                                {
                                    ServHead.ItemNumber = (Convert.ToInt32(ServHead.ItemNumber) + maxFugueSeqNo).ToString();
                                    itemStrc.Item_Number = ServHead.ItemNumber;
                                    ServValues.Add(ServHead.UniqueID, ServHead.ItemNumber);
                                    db.Entry(ServHead).CurrentValues.SetValues(ServHead);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    ServHead.ItemNumber = ServValues[ServHead.UniqueID].ToString();
                                    itemStrc.Item_Number = ServValues[ServHead.UniqueID].ToString();
                                    db.Entry(ServHead).CurrentValues.SetValues(ServHead);
                                    db.SaveChanges();
                                }
                                //itemStrc.Item_Number = ServHead.ItemNumber;
                                if (Convert.ToInt32(item.SAPItemSeqNo) < 1 || String.IsNullOrEmpty(item.SAPItemSeqNo.ToString()))
                                {
                                    itemStrc.SAPItemSeqNo = (Convert.ToInt32(MaxitemStrcLine) + 10);
                                    itemStrc.FugueItemSeqNo = (Convert.ToInt32(MaxitemStrcLine) + 10);
                                }
                                //itemStrc.LINEITEMNUMBER = (Convert.ToInt32(MaxitemStrcLine) + 10).ToString();

                                db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                db.SaveChanges();
                            }
                            else if (Convert.ToInt32(item.SAPItemSeqNo) < 1 || String.IsNullOrEmpty(item.SAPItemSeqNo.ToString()))
                            {
                                var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.ItemUniqueId).FirstOrDefault();
                                var ServHead = db.TEPOServiceHeaders.Where(a => a.UniqueID == itemStrc.ServiceHeaderId).FirstOrDefault();
                                int? MaxitemStrcLine = db.TEPOItemStructures.Where(a => a.ServiceHeaderId == itemStrc.ServiceHeaderId && a.Uniqueid != item.ItemUniqueId).Max(x => x.SAPItemSeqNo);

                                itemStrc.SAPItemSeqNo = (Convert.ToInt32(MaxitemStrcLine) + 10);
                                itemStrc.FugueItemSeqNo = (Convert.ToInt32(MaxitemStrcLine) + 10);
                                //itemStrc.LINEITEMNUMBER = (Convert.ToInt32(MaxitemStrcLine) + 10).ToString();

                                db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                #region Duplicate Bool
                //bool BoolDuplicate = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && (a.IsRecordInSAP == true || (a.IsDeleted == false && a.IsRecordInSAP == false))).GroupBy(x => x.FugueItemSeqNo).Any(x => x.Count() > 1);

                //if (BoolDuplicate)
                //{
                //    var itemDupliList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId).OrderBy(a => a.FugueItemSeqNo).ThenBy(a => a.SAPItemSeqNo).ToList();
                //    int? DupmaxFugItemNo = itemDupliList.Max(a => a.FugueItemSeqNo) + 1;

                //    List<int?> SeqVal = new List<int?>();
                //    Dictionary<int?, int?> HeadSeqdict = new Dictionary<int?, int?>();
                //    foreach (TEPOItemStructure itemFugue in itemDupliList)
                //    {
                //        if (SeqVal.Where(x => x.Value == itemFugue.FugueItemSeqNo).ToList().Count != 0)
                //        {
                //            if (itemFugue.ItemType == "ServiceOrder")
                //            {
                //                if (HeadSeqdict.ContainsKey(itemFugue.ServiceHeaderId))
                //                {
                //                    if (HeadSeqdict[itemFugue.ServiceHeaderId] != itemFugue.FugueItemSeqNo)
                //                    {
                //                        itemFugue.SAPItemSeqNo = HeadSeqdict[itemFugue.ServiceHeaderId];
                //                        itemFugue.FugueItemSeqNo = HeadSeqdict[itemFugue.ServiceHeaderId];
                //                    }
                //                }
                //                else
                //                {
                //                    itemFugue.SAPItemSeqNo = DupmaxFugItemNo;
                //                    itemFugue.FugueItemSeqNo = DupmaxFugItemNo;
                //                    SeqVal.Add(DupmaxFugItemNo);
                //                    HeadSeqdict.Add(itemFugue.ServiceHeaderId, DupmaxFugItemNo);
                //                    DupmaxFugItemNo++;
                //                }
                //            }
                //            else if (itemFugue.ItemType == "ExpenseOrder" || itemFugue.ItemType == "MaterialOrder")
                //            {
                //                if (SeqVal.Where(x => x.Value == itemFugue.FugueItemSeqNo).ToList().Count != 0)
                //                {
                //                    itemFugue.SAPItemSeqNo = DupmaxFugItemNo;
                //                    itemFugue.FugueItemSeqNo = DupmaxFugItemNo;
                //                    SeqVal.Add(DupmaxFugItemNo);
                //                    DupmaxFugItemNo++;
                //                }
                //                else
                //                    SeqVal.Add(itemFugue.FugueItemSeqNo);
                //            }
                //        }
                //        else
                //            SeqVal.Add(itemFugue.FugueItemSeqNo);

                //        db.Entry(itemFugue).CurrentValues.SetValues(itemFugue);
                //        db.SaveChanges();
                //    }

                //}

                //var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId)
                //                                                            .OrderBy(b => b.FugueItemSeqNo).ToList();
                //    if (itemListForSAP.Count > 0)
                //    {
                //        int? maxSApSeqNo = 0;
                //        var CurrentHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerUniqueId && a.IsDeleted == false).FirstOrDefault();
                //        if (CurrentHeadrStructure != null)
                //        {
                //            int prevVersion = Convert.ToInt32(CurrentHeadrStructure.Version) - 1;
                //            string prVer = prevVersion.ToString();
                //            var prevHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Purchasing_Order_Number == CurrentHeadrStructure.Purchasing_Order_Number
                //                                                                                 && a.Version == prVer && a.IsDeleted == false).FirstOrDefault();
                //            var prevItemList = db.TEPOItemStructures.Where(a => a.POStructureId == prevHeadrStructure.Uniqueid).OrderBy(a => a.Uniqueid).ToList();

                //            maxSApSeqNo = prevItemList.Max(a => a.SAPItemSeqNo);
                //            maxSApSeqNo += 1;
                //        }
                //        //maxSApSeqNo = itemListForSAP.Max(a => a.SAPItemSeqNo);
                //        //maxSApSeqNo += 1;
                //        foreach (TEPOItemStructure itemSAP in itemListForSAP)
                //        {
                //            var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid).FirstOrDefault();
                //            if (itemStrcSAP.SAPItemSeqNo > 0)
                //            {
                //                if (itemStrcSAP.IsDeleted == true && itemStrcSAP.IsRecordInSAP == true)
                //                    itemStrcSAP.SAPTransactionCode = "X";
                //            }
                //            else
                //            {
                //                if (itemStrcSAP.IsDeleted == false && itemStrcSAP.IsRecordInSAP != true)
                //                {
                //                    itemStrcSAP.SAPItemSeqNo = maxSApSeqNo;
                //                    itemStrcSAP.SAPTransactionCode = "C";
                //                    maxSApSeqNo += 1;
                //                }
                //            }

                //            db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
                //            db.SaveChanges();
                //        }
                //    }
                #endregion
                #endregion
            }
            //}
        }

        public List<Mat_Serv_Seq> Seq_Mat_Service(POPDFModel PODetails, int POID)
        {
            List<Mat_Serv_Seq> SeqList = new List<Mat_Serv_Seq>();

            int Mat_Seq = 0; int Service_Seq = 0;

            if (PODetails.PurchaseItemStructureDetails.Where(x => x.ItemNo != null && (x.POHeaderStructureid == null || x.POHeaderStructureid == 0)).Count() > 0)
                Mat_Seq = PODetails.PurchaseItemStructureDetails.Where(x => x.ItemNo != null && (x.POHeaderStructureid == null || x.POHeaderStructureid == 0)).Select(v => Convert.ToInt32(v.ItemNo)).Max();

            if (PODetails.POServiceHeader.Where(x => x.ItemNumber != null && (x.POHeaderStructureid != null && x.POHeaderStructureid != 0)).Count() > 0)
                Service_Seq = PODetails.POServiceHeader.Where(x => x.ItemNumber != null && (x.POHeaderStructureid != null && x.POHeaderStructureid != 0)).Max(x => Convert.ToInt32(x.ItemNumber));

            if (Mat_Seq != 0 || Service_Seq != 0)
            {
                int MaxSeq = 0;

                if (Convert.ToInt32(Mat_Seq) > Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Mat_Seq);
                else if (Convert.ToInt32(Mat_Seq) < Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Service_Seq);
                else if (Convert.ToInt32(Mat_Seq) == Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Service_Seq);

                for (int i = 1; i <= MaxSeq; i++)
                {
                    String CountVal = i.ToString();

                    List<TEPOServiceHeader> ServHead = new List<TEPOServiceHeader>();
                    ServHead = PODetails.POServiceHeader.Where(x => x.ItemNumber == CountVal).ToList();

                    List<PurchaseItemStructureDetail> POItems = new List<PurchaseItemStructureDetail>();
                    POItems = PODetails.PurchaseItemStructureDetails.Where(x => x.ItemNo == CountVal && (x.POHeaderStructureid == 0|| x.POHeaderStructureid == null)).ToList();

                    // For Service Items whcich has sequence
                    foreach (TEPOServiceHeader ItemHead in ServHead)
                    {
                        Mat_Serv_Seq ListNonSeq = new Mat_Serv_Seq();
                        TEPOServiceHeader TempServ = new TEPOServiceHeader();
                        ListNonSeq.ServHead = ItemHead;
                        ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ItemHead.UniqueID && (x.Assignment_Category != "P" && x.Item_Category !="D")).ToList();
                        SeqList.Add(ListNonSeq);
                    }

                    // For Material Items whcich has sequence
                    if (POItems.Count > 0)
                    {
                        Mat_Serv_Seq ListMatSeq = new Mat_Serv_Seq();
                        ListMatSeq.ItemS = POItems;
                        SeqList.Add(ListMatSeq);
                    }
                }
                // For Null Values whcich has no sequence
                List<TEPOServiceHeader> NullServHead = new List<TEPOServiceHeader>();
                NullServHead = PODetails.POServiceHeader.Where(x => x.ItemNumber == null).ToList();

                List<PurchaseItemStructureDetail> NullPOItems = new List<PurchaseItemStructureDetail>();
                NullPOItems = PODetails.PurchaseItemStructureDetails.Where(x => x.ItemNo == null && (x.POHeaderStructureid == 0 || x.POHeaderStructureid == null)).ToList();

                if (NullServHead != null && NullServHead.Count > 0)
                {
                    foreach (TEPOServiceHeader ServHead in NullServHead)
                    {
                        Mat_Serv_Seq NullListNonSeq = new Mat_Serv_Seq();
                        TEPOServiceHeader TempServ = new TEPOServiceHeader();
                        NullListNonSeq.ServHead = ServHead;
                        NullListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ServHead.UniqueID && (x.Assignment_Category != "P" && x.Item_Category != "D")).ToList();
                        SeqList.Add(NullListNonSeq);
                    }
                }

                if (NullPOItems.Count > 0)
                {
                    Mat_Serv_Seq NullListMatSeq = new Mat_Serv_Seq();
                    NullListMatSeq.ItemS = NullPOItems;
                    SeqList.Add(NullListMatSeq
                        );
                }

            }
            // Which are all the items Null Values
            else
            {
                foreach (TEPOServiceHeader ServHead in PODetails.POServiceHeader)
                {
                    Mat_Serv_Seq ListNonSeq = new Mat_Serv_Seq();
                    TEPOServiceHeader TempServ = new TEPOServiceHeader();
                    ListNonSeq.ServHead = ServHead;
                    ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ServHead.UniqueID && (x.Assignment_Category != "P" && x.Item_Category != "D")).ToList();
                    SeqList.Add(ListNonSeq);
                }

                Mat_Serv_Seq ListNonSeqMat = new Mat_Serv_Seq();
                ListNonSeqMat.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == null || x.POHeaderStructureid == 0).ToList();
                SeqList.Add(ListNonSeqMat);
            }
            return SeqList;
        }

        public int SetMat_Serv_Seq(int HeaderStructureid)
        {
            int Max_Seq = 1;
            List<string> materialNumber = new List<string>();
            List<int> materialNumber_int = new List<int>();
            List<string> serviceNumber = new List<string>();
            List<int> serviceNumber_int = new List<int>();
            int Mat_Item_Number = 0;
            int ServHead_Item_Number = 0;

            var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == HeaderStructureid && (a.ServiceHeaderId == 0 || a.ServiceHeaderId == null)
               && (a.IsRecordInSAP == true || (a.IsDeleted == false && a.IsRecordInSAP == false))).Select(x => x.SAPItemSeqNo).ToList();
            //For Service Max Value
            var ServitemList = db.TEPOItemStructures.Where(a => a.POStructureId == HeaderStructureid && (a.ServiceHeaderId != 0 || a.ServiceHeaderId != null)
            && (a.IsRecordInSAP == true || (a.IsDeleted == false && a.IsRecordInSAP == false))).Select(x => x.Item_Number).ToList();

                       

            materialNumber = db.TEPOItemStructures.Where(x => x.POStructureId == HeaderStructureid).Select(x => x.Item_Number).ToList();
            foreach (int? val in itemList)
            {
                int temp = Convert.ToInt32(val);
                materialNumber_int.Add(temp);
            }
            if (materialNumber_int.Count() > 0)
                Mat_Item_Number = materialNumber_int.Max();

            serviceNumber = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == HeaderStructureid).Select(x => x.ItemNumber).ToList();
            foreach (string val in ServitemList)
            {
                int temp = Convert.ToInt32(val);
                serviceNumber_int.Add(temp);
            }
            if (serviceNumber_int.Count() > 0)
                ServHead_Item_Number = serviceNumber_int.Max();

            //String Mat_Item_Number = db.TEPRItemStructures.Where(x => x.PurchaseRequestId == HeaderStructureid).Max(x => x.Item_Number);
            //string ServHead_Item_Number = db.TEPRServiceHeaders.Where(x => x.PRHeaderStructureid == HeaderStructureid).Max(x => x.ItemNumber); ;

            if (Convert.ToInt32(Mat_Item_Number) > Convert.ToInt32(ServHead_Item_Number))
                return Convert.ToInt32(Mat_Item_Number) + 1;
            else if (Convert.ToInt32(ServHead_Item_Number) > Convert.ToInt32(Mat_Item_Number))
                return Convert.ToInt32(ServHead_Item_Number) + 1;
            else if (Convert.ToInt32(ServHead_Item_Number) == Convert.ToInt32(Mat_Item_Number))
                return Convert.ToInt32(ServHead_Item_Number) + 1;

            return Max_Seq;
        }

        //public void SetItemDataReadyForSAPPosting(int headerUniqueId, bool IsFirstTime)
        //{
        //    if (IsFirstTime == true)
        //    {
        //        int fugueSeqCount = 1, sapSeqCount = 1;
        //        var prevHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerUniqueId && a.IsDeleted == false).FirstOrDefault();
        //        var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId).ToList();
        //        if (itemList.Count > 0)
        //        {
        //            foreach (TEPOItemStructure item in itemList)
        //            {
        //                if (item.FugueItemSeqNo > 0)
        //                { }
        //                else
        //                {
        //                    var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
        //                    itemStrc.FugueItemSeqNo = fugueSeqCount;
        //                    db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
        //                    db.SaveChanges();
        //                    fugueSeqCount += 1;
        //                }
        //            }
        //        }
        //        var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId && a.IsDeleted == false)
        //                            .OrderBy(b => b.FugueItemSeqNo).ToList();
        //        if (itemListForSAP.Count > 0)
        //        {
        //            foreach (TEPOItemStructure itemSAP in itemListForSAP)
        //            {
        //                if (itemSAP.SAPItemSeqNo > 0)
        //                {
        //                }
        //                else
        //                {
        //                    var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid).FirstOrDefault();
        //                    itemStrcSAP.SAPItemSeqNo = sapSeqCount;
        //                    db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
        //                    db.SaveChanges();
        //                    sapSeqCount += 1;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var itemList = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId).OrderBy(a => a.Uniqueid).ToList();

        //        int maxFugueSeqNo = 0; int? maxFugItemNo = 0;
        //        maxFugItemNo = itemList.Max(a => a.FugueItemSeqNo);
        //        maxFugueSeqNo = Convert.ToInt32(maxFugItemNo) + 1;
        //        foreach (TEPOItemStructure item in itemList)
        //        {
        //            if (item.FugueItemSeqNo > 0)
        //            { }
        //            else
        //            {
        //                var itemStrc = db.TEPOItemStructures.Where(a => a.Uniqueid == item.Uniqueid).FirstOrDefault();
        //                itemStrc.FugueItemSeqNo = maxFugueSeqNo;
        //                db.Entry(itemStrc).CurrentValues.SetValues(itemStrc);
        //                db.SaveChanges();
        //                maxFugueSeqNo += 1;
        //            }
        //        }

        //        var itemListForSAP = db.TEPOItemStructures.Where(a => a.POStructureId == headerUniqueId)
        //                                                                .OrderBy(b => b.FugueItemSeqNo).ToList();
        //        if (itemListForSAP.Count > 0)
        //        {
        //            int? maxSApSeqNo = 0;
        //            var CurrentHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerUniqueId && a.IsDeleted == false).FirstOrDefault();
        //            if (CurrentHeadrStructure != null)
        //            {
        //                int prevVersion = Convert.ToInt32(CurrentHeadrStructure.Version) - 1;
        //                string prVer = prevVersion.ToString();
        //                var prevHeadrStructure = db.TEPOHeaderStructures.Where(a => a.Purchasing_Order_Number == CurrentHeadrStructure.Purchasing_Order_Number
        //                                                                                     && a.Version == prVer && a.IsDeleted == false).FirstOrDefault();
        //                var prevItemList = db.TEPOItemStructures.Where(a => a.POStructureId == prevHeadrStructure.Uniqueid).OrderBy(a => a.Uniqueid).ToList();

        //                maxSApSeqNo = prevItemList.Max(a => a.SAPItemSeqNo);
        //                maxSApSeqNo += 1;
        //            }
        //            //maxSApSeqNo = itemListForSAP.Max(a => a.SAPItemSeqNo);
        //            //maxSApSeqNo += 1;
        //            foreach (TEPOItemStructure itemSAP in itemListForSAP)
        //            {
        //                var itemStrcSAP = db.TEPOItemStructures.Where(a => a.Uniqueid == itemSAP.Uniqueid).FirstOrDefault();
        //                if (itemStrcSAP.SAPItemSeqNo > 0)
        //                {
        //                    if (itemStrcSAP.IsDeleted == true && itemStrcSAP.IsRecordInSAP == true)
        //                        itemStrcSAP.SAPTransactionCode = "X";
        //                }
        //                else
        //                {
        //                    if (itemStrcSAP.IsDeleted == false && itemStrcSAP.IsRecordInSAP != true)
        //                    {
        //                        itemStrcSAP.SAPItemSeqNo = maxSApSeqNo;
        //                        itemStrcSAP.SAPTransactionCode = "C";
        //                        maxSApSeqNo += 1;
        //                    }
        //                }

        //                db.Entry(itemStrcSAP).CurrentValues.SetValues(itemStrcSAP);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //}
        #endregion
        //DSR//SavePurchaseOrderApproverForDraft
        [HttpPost]
        public HttpResponseMessage POReject_Old(ApprovalReq value)
        {
            try
            {
                ApprovalReq result = value;

                //Reject
                //Edit           
                var result1 = (from u in db.TEPOHeaderStructures
                               where (u.Uniqueid == result.POUniqueId && u.IsDeleted == false)
                               select u).FirstOrDefault();
                var uniqid = result1.Uniqueid;

                var usrval = (from usr in db.UserProfiles
                              where (usr.UserId == result.UserId)
                              select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

                var apprsList = (from usr in db.TEPOApprovers
                                 where usr.POStructureId == uniqid && usr.IsDeleted == false
                                 select usr).ToList();

                var currentAppr = (from usr in db.TEPOApprovers
                                   where usr.POStructureId == uniqid && usr.ApproverId == result.UserId
                                   select usr).FirstOrDefault();

                if (apprsList.Count > 0)
                {
                    foreach (var appr in apprsList)
                    {
                        if (appr.SequenceNumber == 0)
                        {
                            db.TEPOApprovers.Attach(appr);
                            appr.Status = "Pending For Approval";
                            appr.LastModifiedBy = result.UserId.ToString();
                            db.Entry(appr).Property(x => x.LastModifiedBy).IsModified = true;
                            appr.LastModifiedOn = System.DateTime.Now;
                            db.Entry(appr).Property(x => x.LastModifiedOn).IsModified = true;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.TEPOApprovers.Attach(appr);
                            appr.Status = "Draft";
                            appr.LastModifiedBy = result.UserId.ToString();
                            db.Entry(appr).Property(x => x.LastModifiedBy).IsModified = true;
                            appr.LastModifiedOn = System.DateTime.Now;
                            db.Entry(appr).Property(x => x.LastModifiedOn).IsModified = true;
                            db.SaveChanges();
                        }
                    }
                    result1.ReleaseCode2Status = "Draft";
                    db.Entry(result1).Property(x => x.ReleaseCode2Status).IsModified = true;
                    result1.ReleaseCode2Date = System.DateTime.Now;
                    db.Entry(result1).Property(x => x.ReleaseCode2Date).IsModified = true;
                    db.SaveChanges();

                    //var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();                                
                }

                //return new HttpResponseMessage
                //{
                //    Content = new JsonContent(new
                //    {

                //        res = result1.Uniqueid

                //    })
                //};
                new EmailSendingBL().POEmail_Rejected(result.POUniqueId, result1.CreatedBy, result.UserId);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Rejected Successfully";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(

                            new ApplicationErrorLog
                            {
                                Error = "Approval Condition doesn't exist for po ",
                                ExceptionDateTime = System.DateTime.Now,
                                InnerException = "ex.Message",
                                Source = "From TEPODetailsController | POReject Mehod | " + this.GetType().ToString(),
                                Stacktrace = "PO UniqueId: " + value.POUniqueId + " , PO Number: " + value.PurchaseOrderNumber
                            }
                            );
                db.SaveChanges();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Failed To Reject";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage POReject(ApprovalReq ApproveObj)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(ApproveObj.UserId);
                TEPOHeaderStructure purchaseheaderObj = db.TEPOHeaderStructures.Where(a => a.Uniqueid == ApproveObj.POUniqueId && a.IsDeleted == false).FirstOrDefault();
                List<TEPOApprover> apprsList = db.TEPOApprovers.Where(a => a.POStructureId == ApproveObj.POUniqueId && a.IsDeleted == false).ToList();

                if (purchaseheaderObj != null)
                {
                    purchaseheaderObj.ReleaseCode2Status = "Draft";
                    purchaseheaderObj.ReleaseCode2Date = System.DateTime.Now;
                    purchaseheaderObj.LastModifiedBy = ApproveObj.UserId;
                    purchaseheaderObj.LastModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    //To send an Email when purchase order rejected
                    new EmailSendingBL().POEmail_Rejected(ApproveObj.POUniqueId, purchaseheaderObj.CreatedBy, ApproveObj.UserId);

                    //To delete (soft) the Submitter ,Manager and Po Approver Records 
                    apprsList.ForEach(a => { a.IsDeleted = true; a.LastModifiedBy = loginUsername; a.LastModifiedOn = DateTime.Now; });
                    db.SaveChanges();

                    //Re-Inserting the submitter and manager info to TEPOApprover table
                    new PurchaseOrderBAL().SavePurchaseOrderApproverForDraft(ApproveObj.POUniqueId, (int)purchaseheaderObj.CreatedBy, (int)purchaseheaderObj.POManagerID);
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Rejected";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Reject";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage POWithDrawl_Old(ApprovalReq value)
        {
            ApprovalReq result = value;

            //Approve
            //Edit           
            var result1 = (from u in db.TEPOHeaderStructures
                           where (u.Uniqueid == result.POUniqueId && u.IsDeleted == false)
                           select u).FirstOrDefault();
            var uniqid = result1.Uniqueid;

            //var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
            var usrval = (from usr in db.UserProfiles
                          join terel in db.TEPOReleaseCodes on usr.CallName equals terel.ReleaseCode2By
                          //where (terel.Release_Code2 == result.ReleaseCode)
                          select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

            var apprsList = (from usr in db.TEPOApprovers
                             where usr.POStructureId == uniqid && usr.IsDeleted == false
                             select usr).ToList();

            var currentAppr = (from usr in db.TEPOApprovers
                               where usr.POStructureId == uniqid && usr.ApproverId == result.UserId && usr.IsDeleted == false
                               select usr).FirstOrDefault();

            if (apprsList.Count > 0)
            {
                foreach (var appr in apprsList)
                {
                    if (appr.SequenceNumber == 0)
                    {
                        db.TEPOApprovers.Attach(appr);
                        appr.Status = "Pending For Approval";
                        appr.LastModifiedBy = result.UserId.ToString();
                        db.Entry(appr).Property(x => x.LastModifiedBy).IsModified = true;
                        appr.LastModifiedOn = System.DateTime.Now;
                        db.Entry(appr).Property(x => x.LastModifiedOn).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.TEPOApprovers.Attach(appr);
                        appr.Status = "Draft";
                        db.Entry(appr).Property(x => x.Status).IsModified = true;
                        appr.LastModifiedBy = result.UserId.ToString();
                        db.Entry(appr).Property(x => x.LastModifiedBy).IsModified = true;
                        appr.LastModifiedOn = System.DateTime.Now;
                        db.Entry(appr).Property(x => x.LastModifiedOn).IsModified = true;
                        db.SaveChanges();
                    }
                }
                result1.ReleaseCode2Status = "Draft";
                db.Entry(result1).Property(x => x.ReleaseCode2Status).IsModified = true;
                db.SaveChanges();
            }

            //return new HttpResponseMessage
            //{
            //    Content = new JsonContent(new
            //    {

            //        res = result1.Uniqueid

            //    })
            //};
            sinfo.errorcode = 0;
            sinfo.errormessage = "Withdraw done Successfully";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }

        [HttpPost]
        public HttpResponseMessage POWithDrawl(ApprovalReq ApproveObj)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(ApproveObj.UserId);
                TEPOHeaderStructure purchaseheaderObj = db.TEPOHeaderStructures.Where(a => a.Uniqueid == ApproveObj.POUniqueId && a.IsDeleted == false).FirstOrDefault();
                List<TEPOApprover> apprsList = db.TEPOApprovers.Where(a => a.POStructureId == ApproveObj.POUniqueId && a.IsDeleted == false).ToList();

                if (purchaseheaderObj != null)
                {
                    purchaseheaderObj.ReleaseCode2Status = "Draft";
                    purchaseheaderObj.ReleaseCode2Date = System.DateTime.Now;
                    purchaseheaderObj.LastModifiedBy = ApproveObj.UserId;
                    purchaseheaderObj.LastModifiedOn = DateTime.Now;
                    db.SaveChanges();

                    //To delete (soft) the Submitter ,Manager and Po Approver Records 
                    apprsList.ForEach(a => { a.IsDeleted = true; a.LastModifiedBy = loginUsername; a.LastModifiedOn = DateTime.Now; });
                    db.SaveChanges();

                    //Re-Inserting the submitter and manager info to TEPOApprover table
                    new PurchaseOrderBAL().SavePurchaseOrderApproverForDraft(ApproveObj.POUniqueId, ApproveObj.UserId, (int)purchaseheaderObj.POManagerID);
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Withdraw PO";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Failed To Withdraw PO";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage POSubmitforApproval_Old(Submitforapprovalreq requestObj)
        {
            int POUniqueId = requestObj.POUniqueId;
            string PurchaseOrderNumber = requestObj.PurchaseOrderNumber;
            string SubmitterComments = requestObj.SubmitterComments;
            int UserId = requestObj.UserId;
            //string shipTo = requestObj.shipTo;
            string response = "";
            try
            {
                TEPOHeaderStructure POStructure = db.TEPOHeaderStructures.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();

                TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.UniqueId == POStructure.PO_OrderTypeID).FirstOrDefault();

                var ItemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).ToList();

                if (POrderType == null || ItemStructure.Count == 0)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill all the details of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }

                UserProfile user = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == UserId).FirstOrDefault();

                // approvers code
                var NextApprovers = (from u in db.TEPOApprovers
                                     where (u.POStructureId == POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0)
                                     orderby u.UniqueId
                                     select u).ToList();
                if (NextApprovers.Count == 0)
                {
                    //string FundCenter = "";
                    #region OLD PO
                    //TEPurchase_Item_Structure itemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && x.Item_Category == "D").FirstOrDefault();

                    //if (itemStructure != null)
                    //{
                    //    FundCenter = db.TEPOServices.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                    //         && x.Item_Number == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();

                    //}
                    //else
                    //{
                    //    itemStructure = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).FirstOrDefault();

                    //    FundCenter = db.TEPOAssignments.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                    //        && x.ItemNumber == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();
                    //}

                    //double TotalPrice = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == POUniqueId && (q.Condition_Type != "NAVS")
                    //              ).Sum(q => q.Condition_rate).Value;

                    //TotalPrice = TotalPrice * Convert.ToDouble(POStructure.Exchange_Rate);
                    //TotalPrice = Math.Round(TotalPrice);
                    //TotalPrice = Math.Truncate(TotalPrice);

                    //TEPurchase_FundCenter Fund = db.TEPOFundCenters.Where(x => x.IsDeleted == false && x.FundCenter_Code == FundCenter).FirstOrDefault();
                    ////TEPurchasingGroup PGroup = db.TEPurchasingGroups.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Group).FirstOrDefault();
                    //TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Document_Type).FirstOrDefault();
                    #endregion

                    TEPOFundCenter Fund = db.TEPOFundCenters.Where(x => x.IsDeleted == false && x.Uniqueid == POStructure.FundCenterID).FirstOrDefault();

                    double TotalPrice = 0.00;
                    if (ItemStructure.Count > 0)
                    {
                        TotalPrice = Convert.ToDouble(ItemStructure.Sum(x => x.TotalAmount.Value));
                    }
                    POApprovalCondition AppCon = null;
                    if (Fund != null && POrderType != null)
                    {
                        AppCon = db.POApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == Fund.Uniqueid
                            && x.OrderType == POrderType.UniqueId && TotalPrice >= x.MinAmount && TotalPrice <= x.MaxAmount).FirstOrDefault();
                    }
                    if (AppCon != null)
                    {
                        List<POMasterApprover> MasterApprovers = db.POMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == AppCon.UniqueId && x.Type == "Approver").OrderBy(x => x.SequenceId).ToList();

                        if (MasterApprovers.Count > 0)
                        {
                            int count = 1;
                            foreach (var Appr in MasterApprovers)
                            {
                                TEPOApprover result = new TEPOApprover();

                                count = count + 1;

                                string AprName = "Not Available";

                                result.CreatedOn = System.DateTime.Now;
                                result.LastModifiedOn = System.DateTime.Now;
                                result.CreatedBy = user.UserName;
                                result.LastModifiedBy = user.UserName;
                                result.SequenceNumber = Convert.ToInt32(Appr.SequenceId);
                                result.PurchaseOrderNumber = POStructure.Uniqueid.ToString();

                                AprName = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == Appr.ApproverId).Select(x => x.CallName).FirstOrDefault();

                                if (AprName == null)
                                {
                                    result.ApproverName = "Not Available";
                                }
                                else
                                {
                                    result.ApproverName = AprName;
                                }
                                if (Appr.SequenceId == 2)
                                {
                                    result.Status = "Pending For Approval";
                                }
                                else
                                {
                                    result.Status = "Draft";
                                }
                                if (count == MasterApprovers.Count)
                                {
                                    result.ReleaseCode = "02";
                                }
                                result.ApproverId = Appr.ApproverId;
                                result.POStructureId = POStructure.Uniqueid;
                                db.TEPOApprovers.Add(result);
                                db.SaveChanges();
                            }

                            db.TEPOHeaderStructures.Attach(POStructure);

                            POStructure.ReleaseCode2Status = "Pending For Approval";
                            db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                            POStructure.LastModifiedOn = DateTime.Now;
                            db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;
                            POStructure.CreatedOn = DateTime.Now;
                            db.Entry(POStructure).Property(x => x.CreatedOn).IsModified = true;

                            POStructure.LastModifiedBy = requestObj.UserId;
                            db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                            POStructure.SubmittedBy = UserId;
                            db.Entry(POStructure).Property(x => x.SubmittedBy).IsModified = true;

                            POStructure.SubmitterComments = SubmitterComments;
                            db.Entry(POStructure).Property(x => x.SubmitterComments).IsModified = true;

                            //POStructure.ShipTpCode = shipTo;
                            //db.Entry(POStructure).Property(x => x.ShipTpCode).IsModified = true;
                            db.SaveChanges();

                            //Current Approver
                            var CurrentApprover = (from u in db.TEPOApprovers
                                                   where (u.POStructureId == POUniqueId && u.IsDeleted == false && u.SequenceNumber == 0)
                                                   orderby u.UniqueId
                                                   select u).FirstOrDefault();
                            db.TEPOApprovers.Attach(CurrentApprover);
                            CurrentApprover.LastModifiedOn = System.DateTime.Now;
                            db.Entry(CurrentApprover).Property(x => x.LastModifiedOn).IsModified = true;
                            CurrentApprover.LastModifiedBy = user.UserName;
                            db.Entry(CurrentApprover).Property(x => x.LastModifiedBy).IsModified = true;
                            CurrentApprover.Status = "Approved";
                            db.Entry(CurrentApprover).Property(x => x.Status).IsModified = true;
                            db.SaveChanges();
                            new EmailSendingBL().POEmail_SubmitForApproval(requestObj.POUniqueId, POStructure.CreatedBy, requestObj.UserId);

                            #region temporarily commented email notification
                            //try
                            //{






                            //    string VendorName = (db.TEPurchase_Vendor
                            //                              .Where(x => x.Vendor_Code == POStructure.Vendor_Account_Number)
                            //                              .Select(x => x.Vendor_Owner).FirstOrDefault());

                            //    var NextApproversList = (from u in db.TEPOApprovers
                            //                             join usr in db.UserProfiles on u.ApproverId equals usr.UserId
                            //                             where (u.POStructureId == POStructure.Uniqueid && u.IsDeleted == false && u.Status == "Pending For Approval" && u.SequenceNumber != 0)
                            //                             orderby u.UniqueId

                            //                             select new
                            //                             {
                            //                                 u.UniqueId,
                            //                                 u.SequenceNumber,
                            //                                 u.ApproverName,
                            //                                 usr.UserId,
                            //                                 u.Status
                            //                             }).ToList();

                            //    if (NextApproversList.Count > 0)
                            //    {
                            //        foreach (var item in NextApproversList)
                            //        {
                            //            UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();
                            //            UserProfile Submitter = db.UserProfiles.Where(x => x.UserId == POStructure.SubmittedBy).FirstOrDefault();
                            //            TEEmpBasicInfo emp = db.TEEmpBasicInfoes.Where(x => x.UserId == Submitter.UserName && x.IsDeleted == false).FirstOrDefault();
                            //            var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmit").ToList();
                            //            if (Emails.Count > 0)
                            //            {


                            //                TEEmailTemplate Email = new TEEmailTemplate();

                            //                Email.Subject = Emails[0].Subject.Replace("$VendorName", VendorName);
                            //                Email.EmailTemplate = Emails[0].EmailTemplate.Replace("$Employee", ToUser.CallName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POValue", TotalPrice.ToString());
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber ", POStructure.Purchasing_Order_Number);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$SubmitterName", Submitter.CallName);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$EmpCode", emp.EmployeeId);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POTitle", POStructure.PO_Title);
                            //                Email.EmailTemplate = Email.EmailTemplate.Replace("$POVersion", "R" + POStructure.Version);
                            //                SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);


                            //            }


                            //            var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                            //            SendNotification(item.UserId, "Purchase Order " + POStructure.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(), POStructure.Uniqueid);
                            //        }
                            //    }

                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            #endregion

                            response = "Success";
                        }
                        else
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Approvers are not available to this PO";
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }
                    }
                    else
                    {

                        db.ApplicationErrorLogs.Add(

                         new ApplicationErrorLog
                         {
                             Error = "Approval Condition doesn't exist for po ",
                             ExceptionDateTime = System.DateTime.Now,
                             InnerException = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber,
                             Source = "From TEPODetailsController | POSubmitforApproval Mehod | " + this.GetType().ToString(),
                             Stacktrace = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber


                         }
                     );
                        db.SaveChanges();
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Approval Condition not exist for this PO";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    var currentAppr = (from usr in db.TEPOApprovers
                                       where usr.POStructureId == POUniqueId && usr.ApproverId == user.UserId && usr.IsDeleted == false && usr.SequenceNumber == 0
                                       select usr).FirstOrDefault();

                    int ApproverSquence = Convert.ToInt32(currentAppr.SequenceNumber);

                    var NextApproverList = (from u in db.TEPOApprovers
                                            where (u.SequenceNumber > ApproverSquence && u.POStructureId == POUniqueId && u.IsDeleted == false)
                                            orderby u.UniqueId
                                            select new
                                            {
                                                u.UniqueId,
                                                u.SequenceNumber,
                                                u.ApproverName,
                                                u.ReleaseCode,
                                                u.ApproverId
                                            }).ToList();

                    var NxtSeqApprover = NextApproverList.Where(a => a.SequenceNumber == ApproverSquence + 1).FirstOrDefault();

                    if (NxtSeqApprover.SequenceNumber == ApproverSquence + 1)
                    {
                        var curApprover = (from u in db.TEPOApprovers where (u.UniqueId == currentAppr.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(curApprover);

                        curApprover.Status = "Approved";
                        db.Entry(curApprover).Property(x => x.Status).IsModified = true;

                        curApprover.ApprovedOn = System.DateTime.Now;
                        db.Entry(curApprover).Property(x => x.ApprovedOn).IsModified = true;

                        curApprover.Comments = SubmitterComments;
                        db.Entry(curApprover).Property(x => x.Comments).IsModified = true;
                        db.SaveChanges();

                        var NextApprover = (from u in db.TEPOApprovers where (u.UniqueId == NxtSeqApprover.UniqueId) select u).FirstOrDefault();
                        db.TEPOApprovers.Attach(NextApprover);

                        NextApprover.Status = "Pending For Approval";
                        db.Entry(NextApprover).Property(x => x.Status).IsModified = true;

                        NextApprover.ApprovedOn = System.DateTime.Now;
                        db.Entry(NextApprover).Property(x => x.ApprovedOn).IsModified = true;

                        NextApprover.Comments = SubmitterComments;
                        db.Entry(NextApprover).Property(x => x.Comments).IsModified = true;
                        db.SaveChanges();
                    }
                    db.TEPOHeaderStructures.Attach(POStructure);

                    POStructure.ReleaseCode2Status = "Pending For Approval";
                    db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                    POStructure.LastModifiedOn = DateTime.Now;
                    db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;

                    POStructure.LastModifiedBy = requestObj.UserId;
                    db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                    POStructure.SubmittedBy = UserId;
                    db.Entry(POStructure).Property(x => x.SubmittedBy).IsModified = true;

                    POStructure.SubmitterComments = SubmitterComments;
                    db.Entry(POStructure).Property(x => x.SubmitterComments).IsModified = true;

                    //POStructure.ShipTpCode = shipTo;
                    //db.Entry(POStructure).Property(x => x.ShipTpCode).IsModified = true;
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                db.ApplicationErrorLogs.Add(

               new ApplicationErrorLog
               {
                   Error = "Exception " + ex.Message,
                   ExceptionDateTime = System.DateTime.Now,
                   InnerException = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber,
                   Source = "From TEPODetailsController | POSubmitforApproval Method | " + this.GetType().ToString(),
                   Stacktrace = ex.StackTrace

               }
               );

                db.SaveChanges();

                response = ex.Message;
            }
            //return new HttpResponseMessage
            //{
            //    Content = new JsonContent(new
            //    {

            //        res = response

            //    })
            //};
            sinfo.errorcode = 0;
            sinfo.errormessage = "Saved Successfully";
            return new HttpResponseMessage() { Content = new JsonContent(new { res = response, info = sinfo }) };
        }

        [HttpPost]
        public HttpResponseMessage test()
        {
            return new HttpResponseMessage
            {
                Content = new JsonContent(new
                {

                    res = "success"

                })
            };
        }
        [HttpPost]
        public HttpResponseMessage GetTaxDtlsForItem(PurchaseTaxInput json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                var taxDtls = GetTaxDetailsForOrderItem(json);

                if (taxDtls != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetWBSCodeDetails(WbsFundCenterInput json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                var wbsDtls = GetWBSCodeDetailsByFundCenter(json);

                if (wbsDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = wbsDtls.Count;
                    sinfo.torecords = wbsDtls.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = wbsDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = wbsDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetGLCodeDetails(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                var glDtls = GetAllGLCodes();

                if (glDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = glDtls.Count;
                    sinfo.torecords = glDtls.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = glDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = glDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage SavePurchaseItem(JObject json)
        {
            try
            {
                List<PurchaseItemStructure> listItems = new List<PurchaseItemStructure>();
                int id = 0;
                if (listItems != null && listItems.Count > 0)
                {
                    foreach (PurchaseItemStructure itm in listItems)
                    {
                        if (itm.HeaderStructureID != 0)
                            id = SavePurchaseItemStructure(itm);
                    }
                }

                if (id > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveMultiplePurchaseItems(PurchaseItemStructureList itemStructure)
        {
            try
            {
                int saveupdatecount = 1; string PlantStorageCode = string.Empty;
                string location = string.Empty; string vendorCurrency = string.Empty;
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(itemStructure.LastModifiedBy));

                var purchaseHeaderStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == itemStructure.HeaderStructureID && a.IsDeleted == false).FirstOrDefault();

                if (purchaseHeaderStructure != null)
                {
                    var locationName = (from proj in db.TEProjects
                                        where proj.ProjectID == purchaseHeaderStructure.ProjectID
                                        select new
                                        {
                                            proj.Location
                                        }
                                   ).FirstOrDefault();
                    if (!string.IsNullOrEmpty(locationName.Location))
                    {
                        location = locationName.Location;
                    }
                    var curerncy = (from vend in db.TEPOVendorMasters
                                    join dtl in db.TEPOVendorMasterDetails on vend.POVendorMasterId equals dtl.POVendorMasterId
                                    where dtl.POVendorDetailId == purchaseHeaderStructure.VendorID
                                    select new
                                    {
                                        vend.Currency
                                    }
                                   ).FirstOrDefault();
                    if (curerncy != null)
                    {
                        if (!string.IsNullOrEmpty(curerncy.Currency))
                        {
                            vendorCurrency = curerncy.Currency;
                        }
                    }
                }
                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0" && itemStructure.Material_Number[cnt] != "0")
                    {
                        if (itemStructure.ItemType[cnt] == "MaterialOrder" || itemStructure.ItemType[cnt] == "ServiceOrder")
                        {
                            decimal? itemRate = 0, rateValidationValue = 1000000;
                            decimal controlBaserate = 0; decimal thresholdRate = 0;
                            var rateValidationLimit = db.TEMasterRules.Where(a => a.RuleName.Contains("CLRateValidationLimit") && a.IsDeleted == false).FirstOrDefault();
                            if (rateValidationLimit != null)
                            {
                                rateValidationValue = Convert.ToDecimal(rateValidationLimit.RuleValue);
                            }
                            itemRate = itemStructure.Rate[cnt];

                            decimal TotAmt = Convert.ToDecimal(Convert.ToDecimal(itemStructure.Order_Qty[cnt]) * itemStructure.Rate[cnt]);

                            //BY Jagan to Get Domestic/Import and Intra/inter-state
                            var poDetailsList = (from phs in db.TEPOHeaderStructures
                                                 join tpv in db.TEPOVendorMasterDetails on phs.VendorID equals tpv.POVendorDetailId into temppv
                                                 from vndrmasterdatail in temppv.DefaultIfEmpty()
                                                 join vndMaster in db.TEPOVendorMasters on vndrmasterdatail.POVendorMasterId equals vndMaster.POVendorMasterId into tempvndMaster
                                                 from vndrmstr in tempvndMaster.DefaultIfEmpty()
                                                 join state in db.TEGSTNStateMasters on vndrmasterdatail.RegionId equals state.StateID into tempstate
                                                 from region in tempstate.DefaultIfEmpty()
                                                 join te_shippedto in db.TEPOPlantStorageDetails on phs.ShippedToID equals te_shippedto.PlantStorageDetailsID into tempship
                                                 from shippedto in tempship.DefaultIfEmpty()
                                                 where phs.Uniqueid == itemStructure.HeaderStructureID && phs.IsDeleted == false
                                                 select new
                                                 {
                                                     ShipToData = shippedto,
                                                     vndrmstr.Currency,
                                                     RegionCode = region.StateCode,
                                                 }).FirstOrDefault();
                            string NatureofTransaction = string.Empty;
                            string TransactionType = string.Empty;
                            if (poDetailsList.Currency != "INR") { TransactionType = "Import"; NatureofTransaction = "Import"; }
                            else
                            {
                                TransactionType = "Domestic";
                                if (poDetailsList.ShipToData != null)
                                {
                                    if (poDetailsList.RegionCode == poDetailsList.ShipToData.StateCode) { NatureofTransaction = "Intra-state"; }
                                    else { NatureofTransaction = "Inter-state"; }
                                }
                                else { NatureofTransaction = "Inter-state"; }
                            }
                            //BY Jagan to Get Domestic/Import and Intra/Inter-state

                            FinalItemBaseRate baseRate = GetFinalBaseRateInfo(itemStructure.Material_Number[cnt], location, itemStructure.ItemType[cnt], vendorCurrency, TransactionType, NatureofTransaction);
                            controlBaserate = baseRate.ControlBaserate;
                            thresholdRate = baseRate.ThresholdValue;
                            if (controlBaserate != 0)
                            {
                                decimal thresValue = 0;
                                thresValue = (controlBaserate + ((controlBaserate * thresholdRate) / 100));

                                if (itemRate > thresValue)
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Unable to Process.Rate has Crossed Threshold value for item:" + itemStructure.Material_Number[cnt];
                                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                            else if (TotAmt > rateValidationValue)
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Unable to Process.The Total Rate has Crossed CL Rate Validation Limit value for item:" + itemStructure.Material_Number[cnt];
                                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                            }

                           

                            if (purchaseHeaderStructure.PurchaseRequestId > 0)
                            {
                                bool IsPOItemQntyCrossed = false;
                                IsPOItemQntyCrossed = IsPOItemQntyCrossedPRItemQnty(purchaseHeaderStructure.PurchaseRequestId, itemStructure.Material_Number[cnt], Convert.ToDecimal(itemStructure.Order_Qty[cnt]));
                                if (IsPOItemQntyCrossed == true)
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Unable to Process.Quantity Of PO is Crossed to PR for item:" + itemStructure.Material_Number[cnt];
                                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                        }
                    }
                }

                if (purchaseHeaderStructure != null)
                {
                    var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == purchaseHeaderStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                    if (plantStorage != null) PlantStorageCode = plantStorage.PlantStorageCode;
                }
                //Sequence During Saving
                int LineItem_Seq = GetMat_Serv_Seq(purchaseHeaderStructure.Uniqueid);
                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0" && itemStructure.Material_Number[cnt] != "0")
                    {
                        int tempItemStructureID = 0;
                        tempItemStructureID = itemStructure.ItemStructureID[cnt];
                        TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == tempItemStructureID && a.IsDeleted == false).FirstOrDefault();
                        if (existItem != null)
                        {
                            existItem.POStructureId = itemStructure.HeaderStructureID;
                            existItem.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                            existItem.Item_Number = saveupdatecount.ToString();
                            existItem.Item_Category = itemStructure.Item_Category;
                            existItem.Material_Number = itemStructure.Material_Number[cnt];//materialcode
                            existItem.Short_Text = itemStructure.Short_Text[cnt];//shortDescription
                            existItem.Long_Text = itemStructure.Long_Text[cnt];
                            if (string.IsNullOrEmpty(existItem.Short_Text))
                            { existItem.Short_Text = existItem.Long_Text; }
                            existItem.Objectid = itemStructure.Objectid[cnt];
                            //existItem.Line_item = itemStructure.Line_item;
                            existItem.HSNCode = itemStructure.HSN_Code[cnt];
                            existItem.WBSElementCode = itemStructure.WBSElementCode[cnt];
                            existItem.WBSElementCode2 = itemStructure.WBSElementCode2[cnt];
                            //existItem.InternalOrderNumber = itemStructure.InternalOrderNumber[cnt];
                            //existItem.GLAccountNo = itemStructure.GLAccountNo[cnt];
                            existItem.Brand = itemStructure.Brand[cnt];
                            existItem.Unit_Measure = itemStructure.Unit_Measure[cnt];//Unit

                            //begining of calculations
                            existItem.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                            existItem.Rate = itemStructure.Rate[cnt] == null ? 0 : itemStructure.Rate[cnt];
                            existItem.TotalAmount = Convert.ToDecimal(Math.Round(Convert.ToDouble(Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate), 2));

                            existItem.IGSTRate = itemStructure.IGSTRate[cnt] == null ? 0 : itemStructure.IGSTRate[cnt];
                            existItem.IGSTAmount = (existItem.TotalAmount * existItem.IGSTRate) / 100;

                            existItem.CGSTRate = itemStructure.CGSTRate[cnt] == null ? 0 : itemStructure.CGSTRate[cnt];
                            existItem.CGSTAmount = (existItem.TotalAmount * existItem.CGSTRate) / 100;

                            existItem.SGSTRate = itemStructure.SGSTRate[cnt] == null ? 0 : itemStructure.SGSTRate[cnt];
                            existItem.SGSTAmount = (existItem.TotalAmount * existItem.SGSTRate) / 100;

                            existItem.TotalTaxAmount = existItem.IGSTAmount + existItem.CGSTAmount + existItem.SGSTAmount;
                            existItem.GrossAmount = existItem.TotalAmount + existItem.TotalTaxAmount;
                            //ending of calculations

                            existItem.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                            existItem.Material_Group = itemStructure.Material_Group;
                            existItem.Plant = itemStructure.Plant;//Plant Code
                            existItem.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code     
                            existItem.LastModifiedOn = System.DateTime.Now.ToString();
                            // existItem.LastModifiedBy = itemStructure.LastModifiedBy;
                            existItem.LastModifiedBy = loginuser;
                            existItem.Status = itemStructure.Status;
                            existItem.ServiceHeaderId = itemStructure.ServiceHeaderId[cnt];
                            existItem.IsDeleted = false;
                            if (existItem.ItemType == "MaterialOrder")
                            {
                                existItem.Item_Number = LineItem_Seq.ToString();
                                LineItem_Seq++;

                                if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                {
                                    existItem.GLAccountNo = "110600";
                                }
                                else
                                {
                                    existItem.GLAccountNo = "110950";
                                }
                            }
                            if (existItem.ItemType == "ServiceOrder")
                            {
                                int ServID = Convert.ToInt32(itemStructure.ServiceHeaderId[cnt]);
                                TEPOServiceHeader HeadServ = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == purchaseHeaderStructure.Uniqueid && x.UniqueID == ServID).FirstOrDefault();
                                existItem.Item_Number = HeadServ.ItemNumber.ToString();
                                if (String.IsNullOrEmpty(HeadServ.ItemNumber))
                                {
                                    HeadServ.ItemNumber = LineItem_Seq.ToString();
                                    existItem.Item_Number = LineItem_Seq.ToString();
                                    LineItem_Seq++;
                                    db.Entry(HeadServ).CurrentValues.SetValues(HeadServ);
                                    db.SaveChanges();
                                }
                                else
                                    existItem.Item_Number = HeadServ.ItemNumber.ToString();

                                existItem.GLAccountNo = "515200";
                            }
                            if (existItem.ItemType == "ExpenseOrder")
                            {
                                existItem.GLAccountNo = "529900";
                                existItem.Short_Text = itemStructure.Long_Text[cnt];
                                if (string.IsNullOrEmpty(existItem.Tax_salespurchases_code) || existItem.Tax_salespurchases_code == "0")
                                    existItem.Tax_salespurchases_code = "G0";
                            }
                            db.Entry(existItem).CurrentValues.SetValues(existItem);
                            db.SaveChanges();

                            if (existItem.PRRef > 0)
                            {
                                UpdatePRitemBalanceQnty(existItem.PRRef, existItem.Material_Number);
                            }
                            saveupdatecount++;

                        }
                        else
                        {
                            TEPOItemStructure itms = new TEPOItemStructure();
                            itms.POStructureId = itemStructure.HeaderStructureID;
                            itms.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                            itms.Item_Number = itemStructure.Item_Number;
                            itms.Item_Category = itemStructure.Item_Category;
                            itms.Material_Number = itemStructure.Material_Number[cnt];//materialcode
                            itms.Short_Text = itemStructure.Short_Text[cnt];//shortDescription
                            itms.Long_Text = itemStructure.Long_Text[cnt];
                            //if (string.IsNullOrEmpty(itms.Short_Text))
                            // itms.Short_Text = itms.Long_Text; 
                            itms.ItemType = itemStructure.ItemType[cnt];

                            itms.HSNCode = itemStructure.HSN_Code[cnt];
                            itms.WBSElementCode = itemStructure.WBSElementCode[cnt];
                            itms.ManufactureCode = itemStructure.ManufactureCode[cnt];
                            itms.Level1 = itemStructure.Level1[cnt];
                            itms.Level2 = itemStructure.Level2[cnt];
                            itms.Level3 = itemStructure.Level3[cnt];
                            itms.Level4 = itemStructure.Level4[cnt];
                            itms.Level5 = itemStructure.Level5[cnt];
                            itms.Level6 = itemStructure.Level6[cnt];
                            itms.Level7 = itemStructure.Level7[cnt];
                            itms.Brand = itemStructure.Brand[cnt];
                            itms.Unit_Measure = itemStructure.Unit_Measure[cnt];//Unit                           

                            if (String.IsNullOrEmpty(itemStructure.HSN_Code[cnt]))
                            {
                                itms.Tax_salespurchases_code = "G0";
                                itms.IGSTRate = 0;
                                itms.SGSTRate = 0;
                                itms.CGSTRate = 0;
                            }
                            else
                            {
                                itms.IGSTRate = itemStructure.IGSTRate[cnt];
                                itms.SGSTRate = itemStructure.SGSTRate[cnt];
                                itms.CGSTRate = itemStructure.CGSTRate[cnt];
                                itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                            }
                            //begining of calculations
                            itms.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                            itms.Rate = itemStructure.Rate[cnt];
                            itms.TotalAmount = Convert.ToDecimal(itms.Order_Qty) * itms.Rate;
                            itms.TotalAmount = Convert.ToDecimal(Math.Round(Convert.ToDouble(Convert.ToDecimal(itms.Order_Qty) * itms.Rate), 2));

                            itms.IGSTAmount = (itms.TotalAmount * itms.IGSTRate) / 100;

                            
                            itms.CGSTAmount = (itms.TotalAmount * itms.CGSTRate) / 100;

                            
                            itms.SGSTAmount = (itms.TotalAmount * itms.SGSTRate) / 100;
                            itms.TotalTaxAmount = itms.IGSTAmount + itms.CGSTAmount + itms.SGSTAmount;
                            itms.GrossAmount = itms.TotalAmount + itms.TotalTaxAmount;
                            //ending of calculations
                          
                            itms.Material_Group = itemStructure.Material_Group;
                            itms.Plant = itemStructure.Plant;//Plant Code
                            itms.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code            
                            itms.CreatedOn = System.DateTime.Now.ToString();
                            itms.CreatedBy = loginuser;
                            itms.LastModifiedBy = loginuser;
                            itms.LastModifiedOn = System.DateTime.Now.ToString();
                            itms.Status = itemStructure.Status;
                            itms.ServiceHeaderId = itemStructure.ServiceHeaderId[cnt];
                            itms.IsDeleted = false;
                            if (itms.ItemType == "MaterialOrder")
                            {
                                itms.Item_Number = LineItem_Seq.ToString();
                                LineItem_Seq++;

                                if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                {
                                    itms.GLAccountNo = "110600";
                                }
                                else
                                {
                                    itms.GLAccountNo = "110950";
                                }
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                int ServID = Convert.ToInt32(itemStructure.ServiceHeaderId[cnt]);
                                TEPOServiceHeader HeadServ = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == purchaseHeaderStructure.Uniqueid && x.UniqueID == ServID).FirstOrDefault();
                                if (String.IsNullOrEmpty(HeadServ.ItemNumber))
                                {
                                    HeadServ.ItemNumber = LineItem_Seq.ToString();
                                    itms.Item_Number = LineItem_Seq.ToString();
                                    db.Entry(HeadServ).CurrentValues.SetValues(HeadServ);
                                    db.SaveChanges();
                                    LineItem_Seq++;
                                }
                                else
                                    itms.Item_Number = HeadServ.ItemNumber.ToString();

                                itms.GLAccountNo = "515200";
                            }
                            if (itms.ItemType == "ExpenseOrder")
                            {
                                itms.GLAccountNo = "529900";
                                if (string.IsNullOrEmpty(itms.Tax_salespurchases_code) || itms.Tax_salespurchases_code == "0")
                                    itms.Tax_salespurchases_code = "G0";
                                itms.Short_Text = itemStructure.Long_Text[cnt];
                            }
                            db.TEPOItemStructures.Add(itms);
                            db.SaveChanges();
                            tempItemStructureID = itms.Uniqueid;
                            if (itms.ItemType == "MaterialOrder")
                            {
                                if (!string.IsNullOrEmpty(itemStructure.AnnexureChecklistId[cnt].ToString()))
                                {
                                    SaveAnnexureSpecificationSheet(itemStructure.AnnexureChecklistId[cnt].ToString(), (int)itms.POStructureId, tempItemStructureID, Convert.ToInt32(itemStructure.LastModifiedBy));
                                }
                                TEPurchase_SaveMaterialSpecifications(itms.Material_Number, itemStructure.HeaderStructureID, itms.Uniqueid, Convert.ToInt32(itemStructure.LastModifiedBy));
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                if (!string.IsNullOrEmpty(itemStructure.Material_Number[cnt].ToString()))
                                {
                                    SaveServiceAnnexureSpecificationSheet(itemStructure.Material_Number[cnt].ToString(), (int)itms.POStructureId, tempItemStructureID);
                                }
                            }
                            if (itms.PRRef > 0)
                            {
                                UpdatePRitemBalanceQnty(itms.PRRef, itms.Material_Number);
                            }
                            saveupdatecount++;
                        }
                    }

                }
                if (purchaseHeaderStructure != null)
                {
                    var pymntTerms = db.TEPOVendorPaymentMilestones.Where(a => a.POHeaderStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).OrderBy(b => b.UniqueId).ToList();
                    if (pymntTerms.Count > 0)
                    {
                        var itmsList = db.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).ToList();
                        if (itmsList.Count > 0)
                        {
                            double? TotalPrice = 0;
                            TotalPrice = Convert.ToDouble(itmsList.Sum(x => x.TotalAmount));
                            foreach (var pmnt in pymntTerms)
                            {
                                var pymntTrm = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == pmnt.UniqueId && a.IsDeleted == false).FirstOrDefault();
                                double? percentage = 0, pymntTermAmnt = 0;
                                pymntTermAmnt = pymntTrm.Amount;
                                if (pymntTermAmnt > 0)
                                {
                                    percentage = ((pymntTermAmnt * 100) / TotalPrice);
                                    percentage = Math.Round((Double)percentage, 2);
                                }
                                pymntTrm.Percentage = percentage;
                                db.Entry(pymntTrm).CurrentValues.SetValues(pymntTrm);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                if (saveupdatecount > 1)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Unable to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }


        }


        public int GetMat_Serv_Seq(int HeaderStructureid)
        {
            int Max_Seq = 1;
            List<String> Mat_Item_Number = db.TEPOItemStructures.Where(x => x.POStructureId == HeaderStructureid && (x.ServiceHeaderId == 0 || x.ServiceHeaderId == null)).Select(x => x.Item_Number).ToList();
            List<String> ServHead_Item_Number = db.TEPOServiceHeaders.Where(x => x.POHeaderStructureid == HeaderStructureid).Select(x => x.ItemNumber).ToList();
            int Mat_Seq = 1; int Serv_Seq = 1;
            foreach (String itemNum in Mat_Item_Number)
            {
                int IntNum = Convert.ToInt32(itemNum);
                if (Mat_Seq < IntNum)
                    Mat_Seq = IntNum;
            }
            foreach (String itemNum in ServHead_Item_Number)
            {
                int IntNum = Convert.ToInt32(itemNum);
                if (Serv_Seq < IntNum)
                    Serv_Seq = IntNum;
            }

            if (Mat_Seq > Serv_Seq)
                return Mat_Seq + 1;
            else if (Serv_Seq > Mat_Seq)
                return Serv_Seq + 1;
            else if (Serv_Seq == Mat_Seq)
                return Serv_Seq + 1;
            else
            return Max_Seq;
        }

        [HttpPost]
        public HttpResponseMessage SaveMultiplePurchaseItemsOrd(PurchaseItemStructureList itemStructure)
        {
            try
            {
                int saveupdatecount = 1; string PlantStorageCode = string.Empty;
                string location = string.Empty; string vendorCurrency = string.Empty;
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(itemStructure.LastModifiedBy));

                var purchaseHeaderStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == itemStructure.HeaderStructureID && a.IsDeleted == false).FirstOrDefault();

                if (purchaseHeaderStructure != null)
                {
                    var locationName = (from proj in db.TEProjects
                                        where proj.ProjectID == purchaseHeaderStructure.ProjectID
                                        select new
                                        {
                                            proj.Location
                                        }
                                   ).FirstOrDefault();
                    if (!string.IsNullOrEmpty(locationName.Location))
                    {
                        location = locationName.Location;
                    }
                    var curerncy = (from vend in db.TEPOVendorMasters
                                    join dtl in db.TEPOVendorMasterDetails on vend.POVendorMasterId equals dtl.POVendorMasterId
                                    where dtl.POVendorDetailId == purchaseHeaderStructure.VendorID
                                    select new
                                    {
                                        vend.Currency
                                    }
                                   ).FirstOrDefault();
                    if (curerncy != null)
                    {
                        if (!string.IsNullOrEmpty(curerncy.Currency))
                        {
                            vendorCurrency = curerncy.Currency;
                        }
                    }
                }
                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0" && itemStructure.Material_Number[cnt] != "0")
                    {
                        if (itemStructure.ItemType[cnt] == "MaterialOrder" || itemStructure.ItemType[cnt] == "ServiceOrder")
                        {
                            decimal? itemRate = 0, rateValidationValue = 1000000;
                            decimal controlBaserate = 0; decimal thresholdRate = 0;
                            var rateValidationLimit = db.TEMasterRules.Where(a => a.RuleName.Contains("CLRateValidationLimit") && a.IsDeleted == false).FirstOrDefault();
                            if (rateValidationLimit != null)
                            {
                                rateValidationValue = Convert.ToDecimal(rateValidationLimit.RuleValue);
                            }
                            itemRate = itemStructure.Rate[cnt];

                            decimal TotAmt = Convert.ToDecimal(Convert.ToDecimal(itemStructure.Order_Qty[cnt]) * itemStructure.Rate[cnt]);

                            //BY Jagan to Get Domestic/Import and Intra/inter-state
                            var poDetailsList = (from phs in db.TEPOHeaderStructures
                                                 join tpv in db.TEPOVendorMasterDetails on phs.VendorID equals tpv.POVendorDetailId into temppv
                                                 from vndrmasterdatail in temppv.DefaultIfEmpty()
                                                 join vndMaster in db.TEPOVendorMasters on vndrmasterdatail.POVendorMasterId equals vndMaster.POVendorMasterId into tempvndMaster
                                                 from vndrmstr in tempvndMaster.DefaultIfEmpty()
                                                 join state in db.TEGSTNStateMasters on vndrmasterdatail.RegionId equals state.StateID into tempstate
                                                 from region in tempstate.DefaultIfEmpty()
                                                 join te_shippedto in db.TEPOPlantStorageDetails on phs.ShippedToID equals te_shippedto.PlantStorageDetailsID into tempship
                                                 from shippedto in tempship.DefaultIfEmpty()
                                                 where phs.Uniqueid == itemStructure.HeaderStructureID && phs.IsDeleted == false
                                                 select new
                                                 {
                                                     ShipToData = shippedto,
                                                     vndrmstr.Currency,
                                                     RegionCode = region.StateCode,
                                                 }).FirstOrDefault();
                            string NatureofTransaction = string.Empty;
                            string TransactionType = string.Empty;
                            if (poDetailsList.Currency != "INR") { TransactionType = "Import"; NatureofTransaction = "Import"; }
                            else
                            {
                                TransactionType = "Domestic";
                                if (poDetailsList.ShipToData != null)
                                {
                                    if (poDetailsList.RegionCode == poDetailsList.ShipToData.StateCode) { NatureofTransaction = "Intra-state"; }
                                    else { NatureofTransaction = "Inter-state"; }
                                }
                                else { NatureofTransaction = "Inter-state"; }
                            }
                            //BY Jagan to Get Domestic/Import and Intra/Inter-state

                            FinalItemBaseRate baseRate = GetFinalBaseRateInfo(itemStructure.Material_Number[cnt], location, itemStructure.ItemType[cnt], vendorCurrency, TransactionType, NatureofTransaction);
                            controlBaserate = baseRate.ControlBaserate;
                            thresholdRate = baseRate.ThresholdValue;
                            if (controlBaserate != 0)
                            {
                                decimal thresValue = 0;
                                thresValue = (controlBaserate + ((controlBaserate * thresholdRate) / 100));

                                if (itemRate > thresValue)
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Unable to Process.Rate has Crossed Threshold value for item:" + itemStructure.Material_Number[cnt];
                                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                            else if (TotAmt > rateValidationValue)
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Unable to Process.The Total Rate has Crossed CL Rate Validation Limit value for item:" + itemStructure.Material_Number[cnt];
                                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                            }


                            if (purchaseHeaderStructure.PurchaseRequestId > 0)
                            {
                                bool IsPOItemQntyCrossed = false;
                                IsPOItemQntyCrossed = IsPOItemQntyCrossedPRItemQnty(purchaseHeaderStructure.PurchaseRequestId, itemStructure.Material_Number[cnt], Convert.ToDecimal(itemStructure.Order_Qty[cnt]));
                                if (IsPOItemQntyCrossed == true)
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Unable to Process.Quantity Of PO is Crossed to PR for item:" + itemStructure.Material_Number[cnt];
                                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                        }
                    }
                }

                if (purchaseHeaderStructure != null)
                {
                    var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == purchaseHeaderStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                    if (plantStorage != null) PlantStorageCode = plantStorage.PlantStorageCode;
                }

                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0" && itemStructure.Material_Number[cnt] != "0")
                    {
                        int tempItemStructureID = 0;
                        tempItemStructureID = itemStructure.ItemStructureID[cnt];
                        TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == tempItemStructureID && a.IsDeleted == false).FirstOrDefault();

                        if (itemStructure.ItemType[cnt] == "ServiceOrder")
                        {
                            int ServiceID = Convert.ToInt32(itemStructure.ServiceHeaderId[cnt]);
                            TEPOItemStructure HeadItem = db.TEPOItemStructures.Where(a => a.Uniqueid == ServiceID && a.POStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).FirstOrDefault();

                            String Item_Number = HeadItem.Item_Number;
                            Decimal? TotalAmount = Convert.ToDecimal(itemStructure.Order_Qty[cnt]) * itemStructure.Rate[cnt];
                            if (HeadItem.TotalAmount == null)
                                HeadItem.TotalAmount = TotalAmount;
                            else
                                HeadItem.TotalAmount += TotalAmount;
                            HeadItem.HSNCode = itemStructure.HSN_Code[cnt];
                            HeadItem.IGSTRate = itemStructure.IGSTRate[cnt];
                            if (HeadItem.IGSTAmount == null)
                                HeadItem.IGSTAmount = (itemStructure.TotalAmount[cnt] * itemStructure.IGSTRate[cnt]) / 100;
                            else
                                HeadItem.IGSTAmount += (itemStructure.TotalAmount[cnt] * itemStructure.IGSTRate[cnt]) / 100;

                            HeadItem.CGSTRate = itemStructure.CGSTRate[cnt];

                            if (HeadItem.CGSTAmount == null)
                                HeadItem.CGSTAmount = (itemStructure.TotalAmount[cnt] * itemStructure.CGSTRate[cnt]) / 100;
                            else
                                HeadItem.CGSTAmount += (itemStructure.TotalAmount[cnt] * itemStructure.CGSTRate[cnt]) / 100;

                            HeadItem.GLAccountNo = "515200";
                            HeadItem.SGSTRate = itemStructure.SGSTRate[cnt];

                            if (HeadItem.SGSTAmount == null)
                                HeadItem.SGSTAmount = (HeadItem.TotalAmount * HeadItem.SGSTRate) / 100;
                            else
                                HeadItem.SGSTAmount += (HeadItem.TotalAmount * HeadItem.SGSTRate) / 100;

                            HeadItem.TotalTaxAmount = HeadItem.IGSTAmount + HeadItem.CGSTAmount + HeadItem.SGSTAmount;

                            HeadItem.GrossAmount = HeadItem.TotalAmount + HeadItem.TotalTaxAmount;

                            HeadItem.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                            HeadItem.Material_Group = itemStructure.Material_Group;
                            HeadItem.Plant = itemStructure.Plant;//Plant Code
                            HeadItem.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code     
                            HeadItem.LastModifiedOn = System.DateTime.Now.ToString();

                            db.Entry(HeadItem).CurrentValues.SetValues(HeadItem);
                            db.SaveChanges();

                            String ServiceLineItem = db.TEPOServiceBreakUps.Where(a => a.POStructureId == itemStructure.HeaderStructureID && a.Item_Number == Item_Number && a.IsDeleted == false).Max(x => x.Line_item_number);
                            int LinItemNumber = Convert.ToInt32(ServiceLineItem) + 1;
                            TEPOServiceBreakUp ServiceLine = new TEPOServiceBreakUp();
                            ServiceLine.POStructureId = itemStructure.HeaderStructureID;
                            ServiceLine.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                            ServiceLine.Item_Number = HeadItem.Item_Number;
                            ServiceLine.Line_item_number = LinItemNumber.ToString();
                            ServiceLine.Activity_Number = itemStructure.Material_Number[cnt];
                            ServiceLine.Short_Text = itemStructure.Short_Text[cnt];
                            ServiceLine.Order_Qty = itemStructure.Order_Qty[cnt];
                            ServiceLine.Unit_Measure = itemStructure.Unit_Measure[cnt];
                            ServiceLine.Actual_Qty = itemStructure.Order_Qty[cnt];
                            ServiceLine.line_item = itemStructure.Long_Text[cnt];
                            ServiceLine.Version = purchaseHeaderStructure.Version;
                            ServiceLine.Status = itemStructure.Status;
                            ServiceLine.IsDeleted = false;
                            ServiceLine.Gross_Price = itemStructure.Rate[cnt].ToString();
                            ServiceLine.Net_Price = itemStructure.TotalAmount[cnt].ToString();
                            ServiceLine.SACCode = itemStructure.HSN_Code[cnt];
                            ServiceLine.WBS_Element = itemStructure.WBSElementCode[cnt];
                            ServiceLine.LastModifiedOn = System.DateTime.Now.ToString();
                            int LastMod = Convert.ToInt32(itemStructure.LastModifiedBy);
                            ServiceLine.LastModifiedBy = db.UserProfiles.Where(x => x.UserId == LastMod).Select(x => x.UserName).FirstOrDefault();
                            db.TEPOServiceBreakUps.Add(ServiceLine);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (existItem != null)
                            {
                                existItem.POStructureId = itemStructure.HeaderStructureID;
                                existItem.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                                existItem.Item_Number = saveupdatecount.ToString();
                                existItem.Item_Category = itemStructure.Item_Category;
                                existItem.Material_Number = itemStructure.Material_Number[cnt];//materialcode
                                existItem.Short_Text = itemStructure.Short_Text[cnt];//shortDescription
                                existItem.Long_Text = itemStructure.Long_Text[cnt];
                                if (string.IsNullOrEmpty(existItem.Short_Text))
                                { existItem.Short_Text = existItem.Long_Text; }
                                existItem.Objectid = itemStructure.Objectid[cnt];
                                //existItem.Line_item = itemStructure.Line_item;
                                existItem.HSNCode = itemStructure.HSN_Code[cnt];
                                existItem.WBSElementCode = itemStructure.WBSElementCode[cnt];
                                existItem.WBSElementCode2 = itemStructure.WBSElementCode2[cnt];
                                //existItem.InternalOrderNumber = itemStructure.InternalOrderNumber[cnt];
                                //existItem.GLAccountNo = itemStructure.GLAccountNo[cnt];
                                existItem.Brand = itemStructure.Brand[cnt];
                                existItem.Unit_Measure = itemStructure.Unit_Measure[cnt];//Unit

                                //begining of calculations
                                existItem.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                                existItem.Rate = itemStructure.Rate[cnt];
                                existItem.TotalAmount = Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate;

                                existItem.IGSTRate = itemStructure.IGSTRate[cnt];
                                existItem.IGSTAmount = (existItem.TotalAmount * existItem.IGSTRate) / 100;

                                existItem.CGSTRate = itemStructure.CGSTRate[cnt];
                                existItem.CGSTAmount = (existItem.TotalAmount * existItem.CGSTRate) / 100;

                                existItem.SGSTRate = itemStructure.SGSTRate[cnt];
                                existItem.SGSTAmount = (existItem.TotalAmount * existItem.SGSTRate) / 100;
                                existItem.TotalTaxAmount = existItem.IGSTAmount + existItem.CGSTAmount + existItem.SGSTAmount;
                                existItem.GrossAmount = existItem.TotalAmount + existItem.TotalTaxAmount;
                                //ending of calculations

                                existItem.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                                existItem.Material_Group = itemStructure.Material_Group;
                                existItem.Plant = itemStructure.Plant;//Plant Code
                                existItem.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code     
                                existItem.LastModifiedOn = System.DateTime.Now.ToString();
                                // existItem.LastModifiedBy = itemStructure.LastModifiedBy;
                                existItem.LastModifiedBy = loginuser;
                                existItem.Status = itemStructure.Status;
                                existItem.ServiceHeaderId = itemStructure.ServiceHeaderId[cnt];
                                existItem.IsDeleted = false;
                                if (existItem.ItemType == "MaterialOrder")
                                {
                                    if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                    {
                                        existItem.GLAccountNo = "110600";
                                    }
                                    else
                                    {
                                        existItem.GLAccountNo = "110950";
                                    }
                                }
                                if (existItem.ItemType == "ServiceOrder")
                                {
                                    existItem.GLAccountNo = "515200";
                                }
                                if (existItem.ItemType == "ExpenseOrder")
                                {
                                    existItem.GLAccountNo = "529900";
                                    existItem.Short_Text = itemStructure.Long_Text[cnt];
                                    if (string.IsNullOrEmpty(existItem.Tax_salespurchases_code) || existItem.Tax_salespurchases_code == "0")
                                        existItem.Tax_salespurchases_code = "G0";
                                }
                                db.Entry(existItem).CurrentValues.SetValues(existItem);
                                db.SaveChanges();

                                if (existItem.PRRef > 0)
                                {
                                    UpdatePRitemBalanceQnty(existItem.PRRef, existItem.Material_Number);
                                }
                                saveupdatecount++;

                            }
                            else
                            {
                                TEPOItemStructure itms = new TEPOItemStructure();
                                itms.POStructureId = itemStructure.HeaderStructureID;
                                itms.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                                itms.Item_Number = itemStructure.Item_Number;
                                itms.Item_Category = itemStructure.Item_Category;
                                itms.Material_Number = itemStructure.Material_Number[cnt];//materialcode
                                itms.Short_Text = itemStructure.Short_Text[cnt];//shortDescription
                                itms.Long_Text = itemStructure.Long_Text[cnt];
                                //if (string.IsNullOrEmpty(itms.Short_Text))
                                // itms.Short_Text = itms.Long_Text; 
                                itms.ItemType = itemStructure.ItemType[cnt];
                                itms.HSNCode = itemStructure.HSN_Code[cnt];
                                itms.WBSElementCode = itemStructure.WBSElementCode[cnt];
                                itms.ManufactureCode = itemStructure.ManufactureCode[cnt];
                                itms.Level1 = itemStructure.Level1[cnt];
                                itms.Level2 = itemStructure.Level2[cnt];
                                itms.Level3 = itemStructure.Level3[cnt];
                                itms.Level4 = itemStructure.Level4[cnt];
                                itms.Level5 = itemStructure.Level5[cnt];
                                itms.Level6 = itemStructure.Level6[cnt];
                                itms.Level7 = itemStructure.Level7[cnt];
                                itms.Brand = itemStructure.Brand[cnt];
                                itms.Unit_Measure = itemStructure.Unit_Measure[cnt];//Unit                           

                                //begining of calculations
                                itms.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                                itms.Rate = itemStructure.Rate[cnt];
                                itms.TotalAmount = Convert.ToDecimal(itms.Order_Qty) * itms.Rate;

                                itms.IGSTRate = itemStructure.IGSTRate[cnt];
                                itms.IGSTAmount = (itms.TotalAmount * itms.IGSTRate) / 100;

                                itms.CGSTRate = itemStructure.CGSTRate[cnt];
                                itms.CGSTAmount = (itms.TotalAmount * itms.CGSTRate) / 100;

                                itms.SGSTRate = itemStructure.SGSTRate[cnt];
                                itms.SGSTAmount = (itms.TotalAmount * itms.SGSTRate) / 100;
                                itms.TotalTaxAmount = itms.IGSTAmount + itms.CGSTAmount + itms.SGSTAmount;
                                itms.GrossAmount = itms.TotalAmount + itms.TotalTaxAmount;
                                //ending of calculations
                                itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                                itms.Material_Group = itemStructure.Material_Group;
                                itms.Plant = itemStructure.Plant;//Plant Code
                                itms.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code            
                                itms.CreatedOn = System.DateTime.Now.ToString();
                                itms.CreatedBy = loginuser;
                                itms.LastModifiedBy = loginuser;
                                itms.LastModifiedOn = System.DateTime.Now.ToString();
                                itms.Status = itemStructure.Status;
                                itms.ServiceHeaderId = itemStructure.ServiceHeaderId[cnt];
                                itms.IsDeleted = false;
                                if (itms.ItemType == "MaterialOrder")
                                {
                                    if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                    {
                                        itms.GLAccountNo = "110600";
                                    }
                                    else
                                    {
                                        itms.GLAccountNo = "110950";
                                    }
                                }
                                if (itms.ItemType == "ServiceOrder")
                                {
                                    itms.GLAccountNo = "515200";
                                }
                                if (itms.ItemType == "ExpenseOrder")
                                {
                                    itms.GLAccountNo = "529900";
                                    if (string.IsNullOrEmpty(itms.Tax_salespurchases_code) || itms.Tax_salespurchases_code == "0")
                                        itms.Tax_salespurchases_code = "G0";
                                    itms.Short_Text = itemStructure.Long_Text[cnt];
                                }
                                db.TEPOItemStructures.Add(itms);
                                db.SaveChanges();
                                tempItemStructureID = itms.Uniqueid;
                                if (itms.ItemType == "MaterialOrder")
                                {
                                    if (!string.IsNullOrEmpty(itemStructure.AnnexureChecklistId[cnt].ToString()))
                                    {
                                        SaveAnnexureSpecificationSheet(itemStructure.AnnexureChecklistId[cnt].ToString(), (int)itms.POStructureId, tempItemStructureID, Convert.ToInt32(itemStructure.LastModifiedBy));
                                    }
                                    TEPurchase_SaveMaterialSpecifications(itms.Material_Number, itemStructure.HeaderStructureID, itms.Uniqueid, Convert.ToInt32(itemStructure.LastModifiedBy));
                                }
                                if (itms.ItemType == "ServiceOrder")
                                {
                                    if (!string.IsNullOrEmpty(itemStructure.Material_Number[cnt].ToString()))
                                    {
                                        SaveServiceAnnexureSpecificationSheet(itemStructure.Material_Number[cnt].ToString(), (int)itms.POStructureId, tempItemStructureID);
                                    }
                                }
                                if (itms.PRRef > 0)
                                {
                                    UpdatePRitemBalanceQnty(itms.PRRef, itms.Material_Number);
                                }
                                saveupdatecount++;
                            }
                        }
                    }
                }
                if (purchaseHeaderStructure != null)
                {
                    var pymntTerms = db.TEPOVendorPaymentMilestones.Where(a => a.POHeaderStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).OrderBy(b => b.UniqueId).ToList();
                    if (pymntTerms.Count > 0)
                    {
                        var itmsList = db.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).ToList();
                        if (itmsList.Count > 0)
                        {
                            double? TotalPrice = 0;
                            TotalPrice = Convert.ToDouble(itmsList.Sum(x => x.TotalAmount));
                            foreach (var pmnt in pymntTerms)
                            {
                                var pymntTrm = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == pmnt.UniqueId && a.IsDeleted == false).FirstOrDefault();
                                double? percentage = 0, pymntTermAmnt = 0;
                                pymntTermAmnt = pymntTrm.Amount;
                                if (pymntTermAmnt > 0)
                                {
                                    percentage = ((pymntTermAmnt * 100) / TotalPrice);
                                    percentage = Math.Round((Double)percentage, 2);
                                }
                                pymntTrm.Percentage = percentage;
                                db.Entry(pymntTrm).CurrentValues.SetValues(pymntTrm);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                if (saveupdatecount > 1)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Unable to Save/Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }


        }


        [HttpPost]
        public HttpResponseMessage CopyCurrentPurchaseItem(CopyPurchaseItemStructure copyItem)
        {
            UserProfile loginuser = db.UserProfiles.Where(u => u.UserId == copyItem.LastModifiedById && u.IsDeleted == false).FirstOrDefault();
            TEPOItemStructure exitItem = new TEPOItemStructure();
            int itemStructureID = 0; int headerStructureID = 0;
            int serviceheaderId = 0; int maxItemNumber = 0;
            itemStructureID = copyItem.ItemStructureID;
            exitItem = db.TEPOItemStructures.Where(x => x.Uniqueid == itemStructureID && x.IsDeleted == false).FirstOrDefault();
            serviceheaderId = Convert.ToInt32(exitItem.ServiceHeaderId);
            headerStructureID = Convert.ToInt32(exitItem.POStructureId);
            maxItemNumber = GetMat_Serv_Seq(headerStructureID);
            try
            {
                new PurchaseOrderBAL().POItemsClone(serviceheaderId, headerStructureID, headerStructureID, exitItem, loginuser, true, maxItemNumber);
            }
            catch(Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Copy Current Item";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }

            sinfo.errorcode = 0;
            sinfo.errormessage = "Successfully Copy Line Item";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }


        [HttpPost]
        public HttpResponseMessage UpdatePurchaseItems(PurchaseItemStructure upateitem)
        {
            try
            {
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(upateitem.LastModifiedBy));
                int tempItemStructureID = 0; string PlantStorageCode = string.Empty;
                string location = string.Empty; string vendorCurrency = string.Empty;
                string PlantShipToRegionCode = string.Empty;
                tempItemStructureID = upateitem.ItemStructureID;
                var purchaseHeaderStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == upateitem.HeaderStructureID && a.IsDeleted == false).FirstOrDefault();
                if (purchaseHeaderStructure != null)
                {
                    var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == purchaseHeaderStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                    PlantStorageCode = plantStorage.PlantStorageCode;

                    var plantStrg = db.TEPOPlantStorageDetails.Where(a => a.isdeleted == false && a.PlantStorageDetailsID == purchaseHeaderStructure.ShippedToID).FirstOrDefault();
                    PlantShipToRegionCode = plantStrg.StateCode;
                }

                var vendr = (from vend in db.TEPOVendorMasters
                             join vendrDtl in db.TEPOVendorMasterDetails on vend.POVendorMasterId equals vendrDtl.POVendorMasterId
                             join country in db.TEPOCountryMasters on vendrDtl.CountryId equals country.UniqueID into cntry
                             from cntr in cntry.DefaultIfEmpty()
                             join region in db.TEGSTNStateMasters on vendrDtl.RegionId equals region.StateID into rgn
                             from vendRgn in rgn.DefaultIfEmpty()
                             where vendrDtl.POVendorDetailId == purchaseHeaderStructure.VendorID
                             select new
                             {
                                 CountryCode = cntr.Description != null ? cntr.Description : string.Empty,
                                 StateCode = vendRgn.StateCode != null ? vendRgn.StateCode : string.Empty,
                                 vend.Currency,
                                 GSTApplicabilityId = vendrDtl.GSTApplicabilityId != null ? vendrDtl.GSTApplicabilityId : 0
                             }
                            ).FirstOrDefault();

                TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == tempItemStructureID && a.IsDeleted == false).FirstOrDefault();
                //if (Convert.ToDecimal(existItem.Order_Qty) <= Convert.ToDecimal(upateitem.Order_Qty))
                //{
                if (existItem != null)
                {
                    PurchaseTaxInput taxInput = new PurchaseTaxInput();
                    if (vendr != null)
                    {
                        taxInput.CountryCode = vendr.CountryCode;
                        taxInput.HSNCode = existItem.HSNCode;
                        taxInput.PlantRegionCode = PlantShipToRegionCode;
                        taxInput.VendorRegionCode = vendr.StateCode;
                        taxInput.VendorGSTApplicabilityId = vendr.GSTApplicabilityId;
                        taxInput.OrderType = existItem.ItemType;
                    }
                    //PurchaseTaxDetails taxDtls = GetTaxDetailsForItem(taxInput);
                    PurchaseTaxDetails taxDtls = GetTaxDetailsForOrderItem(taxInput);
                    if (existItem.ItemType == "MaterialOrder" || existItem.ItemType == "ServiceOrder")
                    {
                        if (purchaseHeaderStructure != null)
                        {
                            var locationName = (from proj in db.TEProjects
                                                where proj.ProjectID == purchaseHeaderStructure.ProjectID
                                                select new
                                                {
                                                    proj.Location
                                                }
                                           ).FirstOrDefault();
                            if (!string.IsNullOrEmpty(locationName.Location))
                            {
                                location = locationName.Location;
                            }
                            if (vendr != null)
                            {
                                if (!string.IsNullOrEmpty(vendr.Currency))
                                {
                                    vendorCurrency = vendr.Currency;
                                }
                            }
                        }

                        decimal? itemRate = 0, rateValidationValue = 1000000;
                        decimal controlBaserate = 0; decimal thresholdRate = 0;
                        itemRate = upateitem.Rate;
                        var rateValidationLimit = db.TEMasterRules.Where(a => a.RuleName.Contains("CLRateValidationLimit") && a.IsDeleted == false).FirstOrDefault();
                        if (rateValidationLimit != null)
                        {
                            rateValidationValue = Convert.ToDecimal(rateValidationLimit.RuleValue);
                        }

                        decimal TotAmt = Convert.ToDecimal(Convert.ToDecimal(upateitem.Order_Qty) * upateitem.Rate);

                        //BY Jagan to Get Domestic/Import and Intra/Inter-state
                        var poDetailsList = (from phs in db.TEPOHeaderStructures
                                             join tpv in db.TEPOVendorMasterDetails on phs.VendorID equals tpv.POVendorDetailId into temppv
                                             from vndrmasterdatail in temppv.DefaultIfEmpty()
                                             join vndMaster in db.TEPOVendorMasters on vndrmasterdatail.POVendorMasterId equals vndMaster.POVendorMasterId into tempvndMaster
                                             from vndrmstr in tempvndMaster.DefaultIfEmpty()
                                             join state in db.TEGSTNStateMasters on vndrmasterdatail.RegionId equals state.StateID into tempstate
                                             from region in tempstate.DefaultIfEmpty()
                                             join te_shippedto in db.TEPOPlantStorageDetails on phs.ShippedToID equals te_shippedto.PlantStorageDetailsID into tempship
                                             from shippedto in tempship.DefaultIfEmpty()
                                             where phs.Uniqueid == upateitem.HeaderStructureID && phs.IsDeleted == false
                                             select new
                                             {
                                                 ShipToData = shippedto,
                                                 vndrmstr.Currency,
                                                 RegionCode = region.StateCode,
                                             }).FirstOrDefault();
                        string NatureofTransaction = string.Empty;
                        string TransactionType = string.Empty;
                        if (poDetailsList.Currency != "INR") { TransactionType = "Import"; NatureofTransaction = "Import"; }
                        else
                        {
                            TransactionType = "Domestic";
                            if (poDetailsList.ShipToData != null)
                            {
                                if (poDetailsList.RegionCode == poDetailsList.ShipToData.StateCode) { NatureofTransaction = "Intra-state"; }
                                else { NatureofTransaction = "Inter-state"; }
                            }
                            else { NatureofTransaction = "Inter-state"; }
                        }
                        //BY Jagan to Get Domestic/Import and Intra/inter-state

                        FinalItemBaseRate baseRate = GetFinalBaseRateInfo(existItem.Material_Number, location, existItem.ItemType, vendorCurrency, TransactionType, NatureofTransaction);



                        controlBaserate = baseRate.ControlBaserate;
                        thresholdRate = baseRate.ThresholdValue;
                        if (controlBaserate != 0)
                        {
                            decimal thresValue = 0;
                            thresValue = (controlBaserate + ((controlBaserate * thresholdRate) / 100));

                            if (itemRate > thresValue)
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Unable to Process.Rate has Crossed Threshold value for item:" + existItem.Material_Number;
                                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                            }
                        }
                        else if (TotAmt > rateValidationValue)
                        {
                            sinfo.errorcode = 1;
                            sinfo.errormessage = "Unable to Process.The Total Rate has Crossed CL Rate Validation Limit value for item:" + existItem.Material_Number;
                            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        }

                        #region Code Change of Rate Validation 
                        //if (existItem.TotalAmount > rateValidationValue)
                        //{
                        //    if (!string.IsNullOrEmpty(location))
                        //    {
                        //        FinalItemBaseRate baseRate = GetFinalBaseRateInfo(existItem.Material_Number, location, existItem.ItemType, vendorCurrency);
                        //        if (baseRate != null)
                        //        {
                        //            controlBaserate = baseRate.ControlBaserate;
                        //            thresholdRate = baseRate.ThresholdValue;
                        //        }
                        //    }
                        //    if (controlBaserate > 0)
                        //    {
                        //        decimal thresValue = 0;
                        //        thresValue = (controlBaserate * thresholdRate) / 100;
                        //        if (itemRate > (controlBaserate +((controlBaserate * thresValue)/100)))
                        //        {
                        //            sinfo.errorcode = 1;
                        //            sinfo.errormessage = "Unable to Process.Rate has Crossed Threshold value for item:" + existItem.Material_Number;
                        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(existItem.Level1))
                        //        {
                        //            sinfo.errorcode = 1;
                        //            sinfo.errormessage = "Rate for this material does not exist in CL, Please contact CPMO to set up the rate for this item:" + existItem.Material_Number;
                        //            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        //            //if (itemRate > rateValidationValue)
                        //            //{
                        //            //    sinfo.errorcode = 1;
                        //            //    sinfo.errormessage = "Rate for this material does not exist in CL, Please contact CPMO to set up the rate for this item:" + existItem.Material_Number;
                        //            //    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                        //            //}
                        //        }

                        //    }
                        //}
                        #endregion

                        if (purchaseHeaderStructure.PurchaseRequestId > 0)
                        {
                            bool IsPOItemQntyCrossed = false;
                            IsPOItemQntyCrossed = IsPOItemQntyCrossedPRItemQnty(purchaseHeaderStructure.PurchaseRequestId, existItem.Material_Number, tempItemStructureID, Convert.ToDecimal(upateitem.Order_Qty));
                            if (IsPOItemQntyCrossed == true)
                            {
                                sinfo.errorcode = 1;
                                sinfo.errormessage = "Unable to Process.Quantity Of PO is Crossed to PR for item:" + existItem.Material_Number;
                                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                            }
                        }
                    }

                    if (existItem.ItemType == "ExpenseOrder")
                    {
                        //existItem.Material_Number = upateitem.Material_Number;
                        existItem.Long_Text = upateitem.Long_Text;
                        //existItem.Short_Text = upateitem.Short_Text;
                        existItem.Short_Text = upateitem.Long_Text;
                        existItem.HSNCode = upateitem.HSN_Code;
                        //existItem.Unit_Measure = upateitem.Unit_Measure;
                        //if (string.IsNullOrEmpty(existItem.Unit_Measure) || existItem.Unit_Measure == "0")
                        //    existItem.WBSElementCode = "Lumpsum";
                        //existItem.WBSElementCode2 = upateitem.WBSElementCode2;
                        existItem.Order_Qty = upateitem.Order_Qty;
                        existItem.Rate = upateitem.Rate;
                        //decimal? tempTotal = 0;
                        //tempTotal = Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate;
                        //if (tempTotal > 0)
                        //{
                        //    tempTotal = Math.Round((decimal)tempTotal, 2);
                        //}
                        existItem.TotalAmount = Convert.ToDecimal(Math.Round(Convert.ToDouble(Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate), 2));
                        if (taxDtls != null)
                        {
                            existItem.Tax_salespurchases_code = taxDtls.TaxCode;
                            existItem.IGSTRate = taxDtls.IGSTTaxRate;
                            existItem.CGSTRate = taxDtls.CGSTTaxRate;
                            existItem.SGSTRate = taxDtls.SGSTTaxRate;
                        }

                        existItem.WBSElementCode = upateitem.WBSElementCode;
                        existItem.IGSTAmount = (existItem.TotalAmount * existItem.IGSTRate) / 100;
                        existItem.CGSTAmount = (existItem.TotalAmount * existItem.CGSTRate) / 100;
                        existItem.SGSTAmount = (existItem.TotalAmount * existItem.SGSTRate) / 100;

                        existItem.TotalTaxAmount = existItem.IGSTAmount + existItem.CGSTAmount + existItem.SGSTAmount;
                        existItem.GrossAmount = existItem.TotalAmount + existItem.TotalTaxAmount;

                        existItem.LastModifiedOn = System.DateTime.Now.ToString();
                        existItem.LastModifiedBy = loginuser;
                        existItem.GLAccountNo = "529900";
                        if (string.IsNullOrEmpty(existItem.Tax_salespurchases_code) || existItem.Tax_salespurchases_code == "0")
                            existItem.Tax_salespurchases_code = "G0";

                        db.Entry(existItem).CurrentValues.SetValues(existItem);
                        db.SaveChanges();
                    }
                    else
                    {
                        existItem.WBSElementCode = upateitem.WBSElementCode;
                        //existItem.Unit_Measure = upateitem.Unit_Measure;
                        existItem.Brand = upateitem.Brand;
                        //begining of calculations                       
                        existItem.Order_Qty = upateitem.Order_Qty;//Quantity
                        existItem.Rate = upateitem.Rate;
                        decimal? tempTotal = 0;
                        //tempTotal = Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate;
                        //if (tempTotal > 0)
                        //{
                        //    tempTotal = Math.Round((decimal)tempTotal, 2);
                        //}
                        existItem.TotalAmount = Convert.ToDecimal(Math.Round(Convert.ToDouble(Convert.ToDecimal(existItem.Order_Qty) * existItem.Rate), 2)); 

                        if (taxDtls != null)
                        {
                            existItem.Tax_salespurchases_code = taxDtls.TaxCode;
                            existItem.IGSTRate = taxDtls.IGSTTaxRate;
                            existItem.CGSTRate = taxDtls.CGSTTaxRate;
                            existItem.SGSTRate = taxDtls.SGSTTaxRate;
                        }

                        existItem.IGSTAmount = (existItem.TotalAmount * existItem.IGSTRate) / 100;
                        existItem.CGSTAmount = (existItem.TotalAmount * existItem.CGSTRate) / 100;
                        existItem.SGSTAmount = (existItem.TotalAmount * existItem.SGSTRate) / 100;

                        existItem.TotalTaxAmount = existItem.IGSTAmount + existItem.CGSTAmount + existItem.SGSTAmount;
                        existItem.GrossAmount = existItem.TotalAmount + existItem.TotalTaxAmount;
                        //ending of calculations    
                        existItem.LastModifiedOn = System.DateTime.Now.ToString();
                        existItem.LastModifiedBy = loginuser;
                        existItem.IsDeleted = false;

                        if (existItem.ItemType == "MaterialOrder")
                        {
                            if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                            {
                                existItem.GLAccountNo = "110600";
                            }
                            else
                            {
                                existItem.GLAccountNo = "110950";
                            }
                        }
                        if (existItem.ItemType == "ServiceOrder")
                        {
                            existItem.GLAccountNo = "515200";
                        }
                        if (string.IsNullOrEmpty(existItem.Tax_salespurchases_code) || existItem.Tax_salespurchases_code == "0")
                            existItem.Tax_salespurchases_code = "G0";
                        db.Entry(existItem).CurrentValues.SetValues(existItem);
                        db.SaveChanges();
                        if (existItem.PRRef > 0)
                        {
                            UpdatePRitemBalanceQnty(existItem.PRRef, existItem.Material_Number);
                        }
                    }
                    if (purchaseHeaderStructure != null)
                    {
                        var pymntTerms = db.TEPOVendorPaymentMilestones.Where(a => a.POHeaderStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).OrderBy(b => b.UniqueId).ToList();
                        if (pymntTerms.Count > 0)
                        {
                            var itmsList = db.TEPOItemStructures.Where(a => a.POStructureId == purchaseHeaderStructure.Uniqueid && a.IsDeleted == false).ToList();
                            if (itmsList.Count > 0)
                            {
                                double? TotalPrice = 0;
                                TotalPrice = Convert.ToDouble(itmsList.Sum(x => x.TotalAmount));
                                foreach (var pmnt in pymntTerms)
                                {
                                    var pymntTrm = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == pmnt.UniqueId && a.IsDeleted == false).FirstOrDefault();
                                    double? percentage = 0, pymntTermAmnt = 0;
                                    pymntTermAmnt = pymntTrm.Amount;
                                    if (pymntTermAmnt > 0)
                                    {
                                        percentage = ((pymntTermAmnt * 100) / TotalPrice);
                                        percentage = Math.Round((Double)percentage, 2);
                                    }
                                    pymntTrm.Percentage = percentage;
                                    db.Entry(pymntTrm).CurrentValues.SetValues(pymntTrm);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Updated";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Unable to Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                //}
                //else
                //{
                //    sinfo.errorcode = 1;
                //    sinfo.errormessage = "Current Quantity is Greater than the Ordered Quantity";
                //    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                //}
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage AddServiceHeader(TEPOServiceHeader ItemsHead)
        {
            if (ItemsHead != null)
            {
                ItemsHead.IsDeleted = false;

                ItemsHead.LastModifiedBy = ItemsHead.LastModifiedBy;
                ItemsHead.LastModifiedOn = DateTime.Now;
                db.TEPOServiceHeaders.Add(ItemsHead);
                db.SaveChanges();
                List<TEPOServiceHeader> ItemHeadList = new List<Models.TEPOServiceHeader>();
                ItemHeadList.Add(ItemsHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Saved";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = ItemHeadList }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage AddServiceHeaderOrd(TEPOItemStructure ItemsHead)
        {
            if (ItemsHead != null)
            {
                var POHead = db.TEPOHeaderStructures.Where(x => x.Uniqueid == ItemsHead.POStructureId && x.IsDeleted == false).FirstOrDefault();
                int Items = db.TEPOItemStructures.Where(x => x.POStructureId == ItemsHead.POStructureId && x.IsDeleted == false).Count();
                Items++;

                ItemsHead.Item_Number = Items.ToString();
                ItemsHead.IsDeleted = false;
                ItemsHead.ItemType = "ServiceOrder";
                ItemsHead.Purchasing_Order_Number = POHead.Purchasing_Order_Number;
                int UserID = Convert.ToInt32(ItemsHead.LastModifiedBy);
                String UserName = db.UserProfiles.Where(x => x.UserId == UserID).Select(x => x.UserName).FirstOrDefault();
                ItemsHead.LastModifiedBy = UserName;
                ItemsHead.CreatedBy = UserName;
                ItemsHead.LastModifiedOn = DateTime.Now.ToString();
                ItemsHead.CreatedOn = DateTime.Now.ToString();
                db.TEPOItemStructures.Add(ItemsHead);
                db.SaveChanges();
                List<TEPOItemStructure> ItemHeadList = new List<Models.TEPOItemStructure>();
                ItemHeadList.Add(ItemsHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Saved";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = ItemHeadList }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateServiceHeader(TEPOServiceHeader UpdtIemHead)
        {
            if (UpdtIemHead != null)
            {
                TEPOServiceHeader ItemHead = db.TEPOServiceHeaders.Where(x => x.UniqueID == UpdtIemHead.UniqueID && x.IsDeleted == false).FirstOrDefault();
                ItemHead.Title = UpdtIemHead.Title;
                ItemHead.Description = UpdtIemHead.Description;
                ItemHead.IsDeleted = false;
                ItemHead.LastModifiedOn = DateTime.Now;
                db.Entry(ItemHead).CurrentValues.SetValues(ItemHead);
                db.SaveChanges();
                List<TEPOServiceHeader> ItemHeadList = new List<Models.TEPOServiceHeader>();
                ItemHeadList.Add(UpdtIemHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Updated";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = ItemHeadList }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateServiceHeaderOrd(TEPOItemStructure UpdtIemHead)
        {
            if (UpdtIemHead != null)
            {
                TEPOItemStructure ItemHead = db.TEPOItemStructures.Where(x => x.Uniqueid == UpdtIemHead.Uniqueid && x.IsDeleted == false).FirstOrDefault();
                ItemHead.Short_Text = UpdtIemHead.Short_Text;
                ItemHead.Long_Text = UpdtIemHead.Long_Text;
                ItemHead.IsDeleted = false;
                ItemHead.LastModifiedOn = DateTime.Now.ToString();
                db.Entry(ItemHead).CurrentValues.SetValues(ItemHead);
                db.SaveChanges();
                List<TEPOItemStructure> ItemHeadList = new List<Models.TEPOItemStructure>();
                ItemHeadList.Add(UpdtIemHead);
                sinfo.errorcode = 0;
                sinfo.errormessage = "Sucessfully Header Updated";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = ItemHeadList }) };
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteServiceHeader(TEPOServiceHeader UpdtIemHead)
        {
            if (UpdtIemHead != null)
            {
                TEPOServiceHeader ItemHead = db.TEPOServiceHeaders.Where(x => x.UniqueID == UpdtIemHead.UniqueID && x.IsDeleted == false).FirstOrDefault();

                if (ItemHead != null)
                {
                    TEPOHeaderStructure HeadStruct = db.TEPOHeaderStructures.Where(x => x.Uniqueid == ItemHead.POHeaderStructureid && x.IsDeleted == false).FirstOrDefault();
                    SAPResponse SAPR = new SAPResponse();
                    List<TEPOItemStructure> POItemStructList = db.TEPOItemStructures.Where(x => x.ServiceHeaderId == UpdtIemHead.UniqueID && x.POStructureId == ItemHead.POHeaderStructureid).ToList();
                    int ItemsCount = POItemStructList.Where(x => x.IsRecordInSAP == true).Count();
                    if (!(String.IsNullOrEmpty(HeadStruct.Purchasing_Order_Number)) && ItemsCount > 0)
                    {
                        SetItemDataReadyForSAPPosting(Convert.ToInt32(ItemHead.POHeaderStructureid), false);
                        SAPR = new PurchaseOrderBAL().DeleteHeadPODetailsToSAP(ItemHead);
                    }
                    if (SAPR.ReturnCode == "0" || (String.IsNullOrEmpty(HeadStruct.Purchasing_Order_Number) || ItemsCount == 0))
                    {
                        
                        foreach (TEPOItemStructure POItemStruct in POItemStructList)
                        {
                            POItemStruct.IsDeleted = true;
                            db.Entry(POItemStruct).CurrentValues.SetValues(POItemStruct);
                            db.SaveChanges();
                        }

                        ItemHead.IsDeleted = true;
                        ItemHead.LastModifiedOn = DateTime.Now;
                        db.Entry(ItemHead).CurrentValues.SetValues(ItemHead);
                        db.SaveChanges();
                        //new PurchaseOrderBAL().DeleteHeadPODetailsToSAP(ItemHead);
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Sucessfully Header Deleted";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = SAPR.Message;
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Save";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetServiceHeadItemsbyHeadStructID(TEPOServiceHeader PO_StructID)
        {

            try
            {
                if (PO_StructID != null)
                {

                    var HeadData = POBAL.GetPOHeadItemStructure(PO_StructID.POHeaderStructureid);
                    sinfo.errorcode = 0;
                    // sinfo.totalrecords = HeadData;
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = HeadData }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetWorkServiceHeadItemsbyHeadStructID(TEPOServiceHeader PO_StructID)
        {

            try
            {
                if (PO_StructID != null)
                {

                    var HeadData = POBAL.GetPOHeadItemStructureWorkOrd(PO_StructID.POHeaderStructureid);
                    sinfo.errorcode = 0;
                    // sinfo.totalrecords = HeadData;
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = HeadData }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPurchaseItemsByPOHeaderId(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int phsId = json["POHeaderStructureID"].ToObject<int>();
                var taxDtls = new PurchaseOrderBAL().GetPurchaseItemStructureByPOStructureId(phsId);

                if (taxDtls != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = taxDtls.Count;
                    sinfo.torecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        public int SavePurchaseItemStructure(PurchaseItemStructure itemStructure)
        {
            int uniqueID = 0;
            string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(itemStructure.CreatedBy));
            TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == itemStructure.ItemStructureID && a.IsDeleted == false).FirstOrDefault();
            if (existItem != null)
            {
                existItem.POStructureId = itemStructure.HeaderStructureID;
                existItem.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                existItem.Item_Number = itemStructure.Item_Number;
                existItem.Item_Category = itemStructure.Item_Category;
                existItem.Material_Number = itemStructure.Material_Number;//materialcode
                existItem.Short_Text = itemStructure.Short_Text;//shortDescription
                existItem.Long_Text = itemStructure.Long_Text;
                existItem.Line_item = itemStructure.Line_item;
                existItem.HSNCode = itemStructure.HSN_Code;
                existItem.WBSElementCode = itemStructure.WBSElementCode;
                existItem.InternalOrderNumber = itemStructure.InternalOrderNumber;
                existItem.GLAccountNo = itemStructure.GLAccountNo;
                existItem.Brand = itemStructure.Brand;
                existItem.Order_Qty = itemStructure.Order_Qty;//Quantity
                existItem.Unit_Measure = itemStructure.Unit_Measure;//Unit
                existItem.Rate = itemStructure.Rate;
                existItem.TotalAmount = itemStructure.TotalAmount;
                existItem.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code;
                existItem.IGSTRate = itemStructure.IGSTRate;
                existItem.IGSTAmount = itemStructure.IGSTAmount;
                existItem.CGSTRate = itemStructure.CGSTRate;
                existItem.CGSTAmount = itemStructure.CGSTAmount;
                existItem.SGSTRate = itemStructure.SGSTRate;
                existItem.SGSTAmount = itemStructure.SGSTAmount;
                existItem.TotalTaxAmount = itemStructure.TotalTaxAmount;
                existItem.GrossAmount = itemStructure.GrossAmount;
                existItem.Material_Group = itemStructure.Material_Group;
                existItem.Plant = itemStructure.Plant;//Plant Code
                existItem.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code     
                existItem.LastModifiedOn = System.DateTime.Now.ToString();
                // existItem.LastModifiedBy = itemStructure.LastModifiedBy;
                existItem.LastModifiedBy = loginuser;
                existItem.Status = itemStructure.Status;
                existItem.IsDeleted = false;
                db.Entry(existItem).CurrentValues.SetValues(existItem);
                db.SaveChanges();
            }
            else
            {
                TEPOItemStructure itms = new TEPOItemStructure();
                itms.POStructureId = itemStructure.HeaderStructureID;
                itms.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                itms.Item_Number = itemStructure.Item_Number;
                itms.Item_Category = itemStructure.Item_Category;
                itms.Material_Number = itemStructure.Material_Number;//materialcode
                itms.Short_Text = itemStructure.Short_Text;//shortDescription
                itms.Long_Text = itemStructure.Long_Text;
                itms.Line_item = itemStructure.Line_item;
                itms.HSNCode = itemStructure.HSN_Code;
                itms.WBSElementCode = itemStructure.WBSElementCode;
                itms.InternalOrderNumber = itemStructure.InternalOrderNumber;
                itms.GLAccountNo = itemStructure.GLAccountNo;
                itms.Brand = itemStructure.Brand;
                itms.Order_Qty = itemStructure.Order_Qty;//Quantity
                itms.Unit_Measure = itemStructure.Unit_Measure;//Unit
                itms.Rate = itemStructure.Rate;
                itms.TotalAmount = itemStructure.TotalAmount;
                itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code;
                itms.IGSTRate = itemStructure.IGSTRate;
                itms.IGSTAmount = itemStructure.IGSTAmount;
                itms.CGSTRate = itemStructure.CGSTRate;
                itms.CGSTAmount = itemStructure.CGSTAmount;
                itms.SGSTRate = itemStructure.SGSTRate;
                itms.SGSTAmount = itemStructure.SGSTAmount;
                itms.TotalTaxAmount = itemStructure.TotalTaxAmount;
                itms.GrossAmount = itemStructure.GrossAmount;
                itms.Material_Group = itemStructure.Material_Group;
                itms.Plant = itemStructure.Plant;//Plant Code
                itms.Storage_Location = itemStructure.Storage_Location;//StorageLocation Code            
                itms.CreatedOn = System.DateTime.Now.ToString();
                //  itms.CreatedBy = itemStructure.CreatedBy;
                existItem.CreatedBy = loginuser;
                existItem.LastModifiedBy = loginuser;
                itms.LastModifiedOn = System.DateTime.Now.ToString();
                //  itms.LastModifiedBy = itemStructure.LastModifiedBy;
                itms.Status = itemStructure.Status;
                itms.IsDeleted = false;
                //itms.Net_Price = itemStructure.Net_Price;
                //itms.Delivery_Date = itemStructure.Delivery_Date;
                //itms.Assignment_Category = itemStructure.Assignment_Category;
                //itms.Requirement_Tracking_Number = itemStructure.Requirement_Tracking_Number;
                //itms.Requisition_Number = itemStructure.Requisition_Number;
                //itms.Item_Purchase_Requisition = itemStructure.Item_Purchase_Requisition;
                //itms.Returns_Item = itemStructure.Returns_Item;

                //itms.Overall_limit = itemStructure.Overall_limit;
                //itms.Expected_Value = itemStructure.Expected_Value;
                //itms.Actual_Value = itemStructure.Actual_Value;
                //itms.No_Limit = itemStructure.No_Limit;
                //itms.Overdelivery_Tolerance = itemStructure.Overdelivery_Tolerance;
                //itms.Underdelivery_Tolerance = itemStructure.Underdelivery_Tolerance;

                db.TEPOItemStructures.Add(itms);
                db.SaveChanges();
                if (itms.Uniqueid != 0)
                    uniqueID = itms.Uniqueid;
            }

            return uniqueID;
        }

        [HttpPost]
        public HttpResponseMessage DeletePurchaseItemStructure(PurchaseItemStructure item)
        {
            try
            {
                TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == item.ItemStructureID && a.IsDeleted == false).FirstOrDefault();
                if (existItem != null)
                {
                    SAPResponse SAPR = new SAPResponse();

                    TEPOHeaderStructure HeadStruct = db.TEPOHeaderStructures.Where(x => x.Uniqueid == item.HeaderStructureID && x.IsDeleted == false).FirstOrDefault();
                    if (!String.IsNullOrEmpty(HeadStruct.Purchasing_Order_Number) && existItem.IsRecordInSAP == true)
                    {
                        // To Delete on SAP
                        SetItemDataReadyForSAPPosting(Convert.ToInt32(item.HeaderStructureID), false);
                        SAPR = new PurchaseOrderBAL().DeletePODetailsToSAP(existItem);
                    }
                    if (SAPR.ReturnCode == "0" || existItem.IsRecordInSAP != true )
                    {
                        string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(item.LastModifiedBy));
                        existItem.CreatedOn = System.DateTime.Now.ToString();
                        existItem.LastModifiedOn = System.DateTime.Now.ToString();
                        //existItem.LastModifiedBy = itemStructure.LastModifiedBy;
                        existItem.LastModifiedBy = loginuser;
                        existItem.IsDeleted = true;
                        db.Entry(existItem).CurrentValues.SetValues(existItem);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Deleted";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = SAPR.Message;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Delete";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage DeletePurchaseItemStructureAndSubItems(int ItemStructureID, string LastModifiedBy)
        {
            try
            {
                TEPOItemStructure existItem = db.TEPOItemStructures.Where(a => a.Uniqueid == ItemStructureID && a.IsDeleted == false).FirstOrDefault();
                if (existItem != null)
                {


                    string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(LastModifiedBy));
                    existItem.CreatedOn = System.DateTime.Now.ToString();
                    existItem.LastModifiedOn = System.DateTime.Now.ToString();
                    existItem.LastModifiedBy = loginuser;
                    existItem.IsDeleted = true;
                    db.Entry(existItem).CurrentValues.SetValues(existItem);
                    db.SaveChanges();

                    var specificTCList = db.TEPOSpecificTandCMasters.Where(a => a.PO_ItemStructureID == ItemStructureID && a.IsDeleted == false).ToList();
                    if (specificTCList.Count > 0)
                    {
                        foreach (TEPOSpecificTandCMaster item in specificTCList)
                        {
                            var specificTC = db.TEPOSpecificTandCMasters.Where(a => a.POSpecificTCUniqueId == item.POSpecificTCUniqueId && a.IsDeleted == false).FirstOrDefault();
                            specificTC.IsDeleted = true;
                            specificTC.LastModifiedOn = System.DateTime.Now;
                            specificTC.LastModifiedBy = Convert.ToInt32(LastModifiedBy);
                            db.Entry(specificTC).CurrentValues.SetValues(specificTC);
                            db.SaveChanges();
                        }

                    }


                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Delete";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPurchaseRateHistoryByItemCode(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                string itemCode = json["itemCode"].ToObject<string>();
                decimal totItmQnty = 0; decimal? weightedAvg = 0;
                decimal? price = 0; decimal? totPrice = 0; decimal finalWeightedAvg = 0;

                var purchaseList = (from item in db.TEPOItemStructures
                                    join header in db.TEPOHeaderStructures on item.POStructureId equals header.Uniqueid
                                    join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                    where item.Material_Number == itemCode && item.IsDeleted == false
                                    select new
                                    {
                                        PONumber = header.Purchasing_Order_Number,
                                        MaterialCode = item.Material_Number,
                                        MaterailName = item.Short_Text,
                                        Rate = item.Rate,
                                        //qnty = string.IsNullOrEmpty(item.Order_Qty) ? 0 : Convert.ToDecimal(item.Order_Qty),
                                        qnty = item.Order_Qty,
                                        Purchasedate = item.LastModifiedOn,
                                        Vendor = vendordtl.VendorCode,
                                        WeightedAvgRate = 0,
                                        Cost = 0
                                    }).OrderByDescending(x => x.Purchasedate).ToList();

                if (purchaseList.Count > 0)
                {
                    if (purchaseList.Count > 5)
                        purchaseList = purchaseList.Take(5).ToList();
                    if (purchaseList.Count > 0)
                    {
                        foreach (var pur in purchaseList)
                        {
                            decimal purQnty = 0;
                            if (!string.IsNullOrEmpty(pur.qnty))
                            { purQnty = Convert.ToDecimal(pur.qnty); }
                            price = pur.Rate * purQnty;
                            totPrice += price;
                            totItmQnty += purQnty;
                            weightedAvg = totPrice / totItmQnty;
                        }
                    }
                    if (weightedAvg > 0)
                    {
                        finalWeightedAvg = (decimal)weightedAvg;
                        finalWeightedAvg = Math.Round(finalWeightedAvg, 2);
                    }
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = purchaseList.Count;
                    sinfo.torecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = purchaseList, weightedAverage = finalWeightedAvg }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = purchaseList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        public decimal? GetWeightedAverageForItem(string itemCode)
        {
            decimal totItmQnty = 0; decimal? weightedAvg = 0;
            decimal? price = 0; decimal? totPrice = 0;
            try
            {
                var purchaseList = (from item in db.TEPOItemStructures
                                    join header in db.TEPOHeaderStructures on item.POStructureId equals header.Uniqueid
                                    join vendordtl in db.TEPOVendorMasterDetails on header.VendorID equals vendordtl.POVendorDetailId
                                    where item.Material_Number == itemCode && item.IsDeleted == false
                                    select new
                                    {
                                        PONumber = header.Purchasing_Order_Number,
                                        MaterialCode = item.Material_Number,
                                        MaterailName = item.Short_Text,
                                        Rate = item.Rate,
                                        qnty = item.Order_Qty,
                                        Purchasedate = item.LastModifiedOn,
                                        Vendor = vendordtl.VendorCode,
                                        WeightedAvgRate = 0,
                                        Cost = 0
                                    }).OrderByDescending(x => x.Purchasedate).ToList();

                if (purchaseList.Count > 0)
                {
                    if (purchaseList.Count > 5)
                        purchaseList = purchaseList.Take(5).ToList();
                    if (purchaseList.Count > 0)
                    {
                        foreach (var pur in purchaseList)
                        {
                            decimal purQnty = 0;
                            if (!string.IsNullOrEmpty(pur.qnty))
                            { purQnty = Convert.ToDecimal(pur.qnty); }
                            price = pur.Rate * purQnty;
                            totPrice += price;
                            totItmQnty += purQnty;
                            weightedAvg = totPrice / totItmQnty;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
            if (weightedAvg > 0)
                weightedAvg = Math.Round((decimal)weightedAvg, 2);
            return weightedAvg;
        }

        [HttpPost]
        public HttpResponseMessage GetVersionHistoryByPONumber(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                string PONumber = json["PONumber"].ToObject<string>();
                if (string.IsNullOrEmpty(PONumber))
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "PONumber should not be Empty";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
                }

                var purchaseList = (from header in db.TEPOHeaderStructures
                                    where header.Purchasing_Order_Number == PONumber && header.IsDeleted == false
                                    select new
                                    {
                                        POHeaderId = header.Uniqueid,
                                        PONumber = header.Purchasing_Order_Number,
                                        PODate = header.Purchasing_Document_Date,
                                        Status = header.ReleaseCode2Status,
                                        Version = "R" + header.Version
                                    }).OrderByDescending(x => x.Version).ToList();

                if (purchaseList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = purchaseList.Count;
                    sinfo.torecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = purchaseList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = purchaseList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage ClonePO(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            int uniqueId = 0, headerId = 0;
            try
            {
                headerId = json["HeaderID"].ToObject<int>();
                int LastMod = json["LastModifiedBy"].ToObject<int>();
                int loginId = 0;
                loginId = json["LastModifiedBy"].ToObject<int>();
                var CurrentPOHeaderStructure = (from header in db.TEPOHeaderStructures
                                                where header.Uniqueid == headerId && header.IsDeleted == false
                                                select header).FirstOrDefault();
                var POHeaderStructureList = (from header in db.TEPOHeaderStructures
                                             where header.Purchasing_Order_Number == CurrentPOHeaderStructure.Purchasing_Order_Number
                                             && header.IsDeleted == false
                                             && header.Status == "Active"
                                             select header).ToList();
                if (POHeaderStructureList.Count > 1)
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "Already PO is Revised and is not Approved";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = uniqueId }) };
                }
                else
                {
                    uniqueId = new PurchaseOrderBAL().ClonePO(headerId, loginId);
                    if (uniqueId > 0)
                    {
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Saved";
                        sinfo.fromrecords = 1;
                        sinfo.totalrecords = 1;
                        sinfo.torecords = 1;
                        sinfo.pages = "1";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = uniqueId }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.totalrecords = 1;
                        sinfo.errormessage = "Failed To Save";
                        return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = uniqueId }) };
                    }
                }
            }
            catch (Exception ex)
            {
                if(uniqueId > 0)
                {
                    TEPOHeaderStructure faultyHeader = new TEPOHeaderStructure();
                    TEPOHeaderStructure priHeader = new TEPOHeaderStructure();
                    faultyHeader = db.TEPOHeaderStructures.Where(x => x.Uniqueid == uniqueId && x.IsDeleted == false).FirstOrDefault();
                    faultyHeader.IsDeleted = true;
                    db.Entry(faultyHeader).CurrentValues.SetValues(faultyHeader);
                    db.SaveChanges();
                    priHeader = db.TEPOHeaderStructures.Where(x => x.Uniqueid == headerId && x.IsDeleted == false).FirstOrDefault();
                    priHeader.Status = "Active";
                    db.Entry(priHeader).CurrentValues.SetValues(priHeader);
                    db.SaveChanges();
                }
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage SendPODetailsToSAP(JObject json)
        {
            int PurchaseHeaderID = json["PurchaseHeaderID"].ToObject<int>();
            var sapRespnse = new PurchaseOrderBAL().SendPODetailsToSAP(PurchaseHeaderID);
            sinfo.errorcode = 0;
            sinfo.totalrecords = 1;
            sinfo.errormessage = "Posted Successfully";
            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
        }
        [HttpPost]
        public HttpResponseMessage UpdatePODetailsToSAP(JObject json)
        {
            int PurchaseHeaderID = json["PurchaseHeaderID"].ToObject<int>();
            var sapRespnse = new PurchaseOrderBAL().UpdatePODetailsToSAP(PurchaseHeaderID);
            sinfo.errorcode = 0;
            sinfo.totalrecords = 1;
            sinfo.errormessage = "Posted Successfully";
            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
        }
        [HttpPost]
        public HttpResponseMessage SendVendorDetailsToSAP(JObject json)
        {
            int VendorMasterDetailID = json["VendorMasterDetailID"].ToObject<int>();
            string PONumber = new PurchaseOrderBAL().SaveVendorDetailsToSAP(VendorMasterDetailID);
            sinfo.errorcode = 0;
            sinfo.totalrecords = 1;
            sinfo.errormessage = "Posted Successfully";
            return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
        }

        public int SavePurchaseItemWiseCondition(PurchaseItemwise purItemCond)
        {
            int uniqueID = 0;

            TEPOItemwise itm = new TEPOItemwise();
            itm.Purchasing_Order_Number = purItemCond.PurchasingOrderNumber;
            itm.POStructureId = purItemCond.HeaderStructureID;
            itm.Item_Number_of_Purchasing_Document = purItemCond.ItemNumberPurchase;
            itm.Condition_Type = purItemCond.ConditionType;
            itm.Condition_rate = Convert.ToDouble(purItemCond.ConditionRate);
            itm.VendorCode = purItemCond.VendorCode;//vendorCode
            db.TEPOItemwises.Add(itm);
            db.SaveChanges();
            if (itm.Uniqueid != 0)
                uniqueID = itm.Uniqueid;

            return uniqueID;
        }

        public PurchaseTaxDetails GetTaxDetailsForItem(PurchaseTaxInput inputObj)
        {
            PurchaseTaxDetails taxDtls = new PurchaseTaxDetails();
            taxDtls.TaxCode = "G0";
            taxDtls.CGSTTaxRate = 0;
            taxDtls.SGSTTaxRate = 0;
            taxDtls.IGSTTaxRate = 0;
            DateTime toDate = DateTime.Today.AddDays(1);

            PurchaseTaxDetails cgsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                          where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                          && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                          && hsn.DestinationCountry == inputObj.CountryCode
                                          && hsn.IsDeleted == false && hsn.TaxType == "CGST" && hsn.HSNCode == inputObj.HSNCode.Trim()
                                          //&& hsn.ValidFrom >= DateTime.Today && hsn.ValidTo <= toDate
                                          )
                                          select new PurchaseTaxDetails
                                          {
                                              TaxCode = hsn.TaxCode,
                                              CGSTTaxRate = hsn.TaxRate
                                          }).FirstOrDefault();
            PurchaseTaxDetails sgsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                          where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                          && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                          && hsn.DestinationCountry == inputObj.CountryCode
                                          && hsn.IsDeleted == false && hsn.TaxType == "SGST" && hsn.HSNCode == inputObj.HSNCode.Trim()
                                          //&& hsn.ValidFrom >= DateTime.Today && hsn.ValidTo <= toDate
                                          )
                                          select new PurchaseTaxDetails
                                          {
                                              TaxCode = hsn.TaxCode,
                                              SGSTTaxRate = hsn.TaxRate
                                          }).FirstOrDefault();
            PurchaseTaxDetails igsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                          where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                          && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                          && hsn.DestinationCountry == inputObj.CountryCode
                                          && hsn.IsDeleted == false && hsn.TaxType == "IGST" && hsn.HSNCode == inputObj.HSNCode.Trim()
                                          //&& hsn.ValidFrom >= DateTime.Today && hsn.ValidTo <= toDate
                                          )
                                          select new PurchaseTaxDetails
                                          {
                                              TaxCode = hsn.TaxCode,
                                              IGSTTaxRate = hsn.TaxRate
                                          }).FirstOrDefault();
            if (cgsttax != null)
            {
                taxDtls.TaxCode = cgsttax.TaxCode;
                taxDtls.CGSTTaxRate = cgsttax.CGSTTaxRate;
            }
            if (sgsttax != null)
                taxDtls.SGSTTaxRate = sgsttax.SGSTTaxRate;
            if (igsttax != null)
                taxDtls.IGSTTaxRate = igsttax.IGSTTaxRate;
            return taxDtls;
        }

        public PurchaseTaxDetails GetTaxDetailsForOrderItem(PurchaseTaxInput inputObj)
        {
            PurchaseTaxDetails taxDtls = new PurchaseTaxDetails();
            taxDtls.TaxCode = "G0";
            taxDtls.CGSTTaxRate = 0;
            taxDtls.SGSTTaxRate = 0;
            taxDtls.IGSTTaxRate = 0;
            if (!string.IsNullOrEmpty(inputObj.HSNCode))
            {
                String HSNCodeTrim = ((inputObj.HSNCode).TrimEnd()).TrimStart();
                string OrderType = string.Empty;
                if (inputObj.OrderType == "MaterialOrder")
                    OrderType = "Material";
                else
                    OrderType = "Service";
                PurchaseTaxDetails cgsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                              where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                              && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                              && hsn.DestinationCountry == inputObj.CountryCode
                                              && hsn.VendorGSTApplicability == inputObj.VendorGSTApplicabilityId
                                              && hsn.IsDeleted == false && hsn.TaxType == "CGST"
                                              && hsn.ApplicableTo == OrderType
                                              && hsn.HSNCode == HSNCodeTrim
                                              && DateTime.Today >= hsn.ValidFrom && DateTime.Today <= hsn.ValidTo
                                              )
                                              select new PurchaseTaxDetails
                                              {
                                                  TaxCode = hsn.TaxCode,
                                                  CGSTTaxRate = hsn.TaxRate
                                              }).FirstOrDefault();
                PurchaseTaxDetails sgsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                              where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                              && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                              && hsn.DestinationCountry == inputObj.CountryCode
                                              && hsn.VendorGSTApplicability == inputObj.VendorGSTApplicabilityId
                                              && hsn.IsDeleted == false && hsn.TaxType == "SGST"
                                               && hsn.ApplicableTo == OrderType
                                              && hsn.HSNCode == HSNCodeTrim
                                              && DateTime.Today >= hsn.ValidFrom && DateTime.Today <= hsn.ValidTo
                                              )
                                              select new PurchaseTaxDetails
                                              {
                                                  TaxCode = hsn.TaxCode,
                                                  SGSTTaxRate = hsn.TaxRate
                                              }).FirstOrDefault();
                PurchaseTaxDetails igsttax = (from hsn in db.TEPOHSNTaxCodeMappings
                                              where (hsn.VendorRegionCode == inputObj.VendorRegionCode
                                              && hsn.DeliveryPlantRegionCode == inputObj.PlantRegionCode
                                              && hsn.DestinationCountry == inputObj.CountryCode
                                              && hsn.VendorGSTApplicability == inputObj.VendorGSTApplicabilityId
                                              && hsn.IsDeleted == false && hsn.TaxType == "IGST"
                                               && hsn.ApplicableTo == OrderType
                                              && hsn.HSNCode == HSNCodeTrim
                                              && DateTime.Today >= hsn.ValidFrom && DateTime.Today <= hsn.ValidTo
                                              )
                                              select new PurchaseTaxDetails
                                              {
                                                  TaxCode = hsn.TaxCode,
                                                  IGSTTaxRate = hsn.TaxRate
                                              }).FirstOrDefault();
                if (cgsttax != null)
                {
                    taxDtls.TaxCode = cgsttax.TaxCode;
                    taxDtls.CGSTTaxRate = cgsttax.CGSTTaxRate;
                }
                if (sgsttax != null)
                {
                    taxDtls.TaxCode = sgsttax.TaxCode;
                    taxDtls.SGSTTaxRate = sgsttax.SGSTTaxRate;
                }
                if (igsttax != null)
                {
                    taxDtls.TaxCode = igsttax.TaxCode;
                    taxDtls.IGSTTaxRate = igsttax.IGSTTaxRate;
                }
            }
            return taxDtls;
        }

        [HttpPost]
        public HttpResponseMessage GetHSNDetailsForExpenseOrder(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int phsId = json["POHeaderStructureID"].ToObject<int>();
                var taxDtls = GetHSNTaxDetailsForExpenseOrder(phsId);

                if (taxDtls.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = taxDtls.Count;
                    sinfo.torecords = taxDtls.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = taxDtls }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }
        public List<HSNTaxDetails> GetHSNTaxDetailsForExpenseOrder(int poUniqueID)
        {
            string VendorRegionCode = string.Empty; string PlantShipToRegionCode = string.Empty;
            string Country = string.Empty; int? VendorGSTApplicabilityId = 0;
            int? VendorClassificationId = 0;
            var purchaseHeaderStructure = db.TEPOHeaderStructures.Where(a => a.Uniqueid == poUniqueID && a.IsDeleted == false).FirstOrDefault();
            if (purchaseHeaderStructure != null)
            {
                var plantStrg = db.TEPOPlantStorageDetails.Where(a => a.isdeleted == false && a.PlantStorageDetailsID == purchaseHeaderStructure.ShippedToID).FirstOrDefault();
                PlantShipToRegionCode = plantStrg.StateCode;
                Country = plantStrg.CountryCode;
            }

            var vendr = (from vend in db.TEPOVendorMasters
                         join vendrDtl in db.TEPOVendorMasterDetails on vend.POVendorMasterId equals vendrDtl.POVendorMasterId
                         join country in db.TEPOCountryMasters on vendrDtl.CountryId equals country.UniqueID
                         join region in db.TEGSTNStateMasters on vendrDtl.RegionId equals region.StateID
                         where vendrDtl.POVendorDetailId == purchaseHeaderStructure.VendorID
                         select new
                         {
                             country.Description,
                             region.StateCode,
                             vend.Currency,
                             vendrDtl.GSTApplicabilityId,
                             vendrDtl.VendorCategoryId
                         }
                        ).FirstOrDefault();
            if (vendr != null)
            {
                VendorRegionCode = vendr.StateCode;
                VendorGSTApplicabilityId = vendr.GSTApplicabilityId;
                VendorClassificationId = vendr.VendorCategoryId;
            }

            List<HSNTaxDetails> taxDtlsList = new List<HSNTaxDetails>();
            var hsnTaxDtls = (from hsn in db.TEPOHSNTaxCodeMappings
                              join vendGstAppl in db.TEPOGSTApplicabilityMasters on hsn.VendorGSTApplicability equals vendGstAppl.UniqueID
                              where (hsn.IsDeleted == false
                              && hsn.VendorRegionCode == VendorRegionCode
                              && hsn.DeliveryPlantRegionCode == PlantShipToRegionCode
                              && hsn.DestinationCountry == Country
                              && vendGstAppl.UniqueID == VendorGSTApplicabilityId
                              && hsn.ApplicableTo == "Service"
                             && hsn.ValidFrom <= DateTime.Today && hsn.ValidTo >= DateTime.Today
                              )
                              select new
                              {
                                  hsn.ApplicableTo,
                                  hsn.DeliveryPlantRegionCode,
                                  hsn.DeliveryPlantRegionDescription,
                                  hsn.DestinationCountry,
                                  GSTVendorClassification = "",
                                  hsn.HSNCode,
                                  hsn.LastModifiedBy,
                                  hsn.LastModifiedOn,
                                  hsn.MaterialGSTApplicability,
                                  hsn.TaxCode,
                                  hsn.TaxRate,
                                  hsn.TaxType,
                                  hsn.ValidFrom,
                                  hsn.ValidTo,
                                  hsn.VendorGSTApplicability,
                                  hsn.VendorRegionCode,
                                  hsn.VendorRegionDescription,
                                  vendGstAppl.GSTApplicabilityCode,
                                  vendGSTDesc = vendGstAppl.Description

                              }).ToList();

            if (hsnTaxDtls.Count > 0)
            {
                foreach (var hsnTax in hsnTaxDtls)
                {
                    HSNTaxDetails taxDtls = new HSNTaxDetails();
                    taxDtls.HSNCode = hsnTax.HSNCode;
                    taxDtls.Country = hsnTax.DestinationCountry;
                    taxDtls.DeliveryPlantRegionCode = hsnTax.DeliveryPlantRegionCode;
                    taxDtls.DeliveryPlantRegionDesc = hsnTax.DeliveryPlantRegionDescription;
                    taxDtls.VendorRegionCode = hsnTax.VendorRegionCode;
                    taxDtls.VendorRegionDesc = hsnTax.VendorRegionDescription;
                    taxDtls.GSTVendorClassificationCode = "";
                    taxDtls.GSTVendorClassificationDesc = "";
                    taxDtls.VendorGSTApplicabilityCode = hsnTax.GSTApplicabilityCode;
                    taxDtls.VendorGSTApplicabilityDesc = hsnTax.vendGSTDesc;
                    taxDtls.MaterialGSTApplicabilityCode = "";
                    taxDtls.MaterialGSTApplicabilityDesc = "";
                    taxDtls.ValidFrom = hsnTax.ValidFrom;
                    taxDtls.ValidTo = hsnTax.ValidTo;
                    if (hsnTax.TaxType == "CGST")
                    {
                        taxDtls.TaxCode = hsnTax.TaxCode;
                        taxDtls.CGSTTaxRate = hsnTax.TaxRate;
                    }
                    if (hsnTax.TaxType == "SGST")
                    {
                        taxDtls.TaxCode = hsnTax.TaxCode;
                        taxDtls.SGSTTaxRate = hsnTax.TaxRate;
                    }
                    if (hsnTax.TaxType == "IGST")
                    {
                        taxDtls.TaxCode = hsnTax.TaxCode;
                        taxDtls.IGSTTaxRate = hsnTax.TaxRate;
                    }
                    taxDtlsList.Add(taxDtls);
                }
            }
            return taxDtlsList;
        }

        public List<WbsCodeDtls> GetWBSCodeDetailsByFundCenter(WbsFundCenterInput fundCntrObj)
        {
            List<WbsCodeDtls> wbsDtls = new List<WbsCodeDtls>();

            wbsDtls = (from wbs in db.TEPOWBSFundCentreMappings
                       join fnd in db.TEPOFundCenters on wbs.FundCentreID equals fnd.Uniqueid
                       join wbsMaster in db.TEPOWBSMasters on wbs.WBSID equals wbsMaster.WBSID
                       where (wbs.FundCentreID == fundCntrObj.FundCentreID
                             && wbs.ProjectCode == fundCntrObj.ProjectCode
                             && wbs.IsDeleted == false && fnd.IsDeleted == false && wbsMaster.IsDeleted == false)
                       select new WbsCodeDtls
                       {
                           wbsID = wbs.WBSID,
                           wbsCode = wbs.WBSCode,
                           wbsDescription = wbsMaster.WBSName,
                           ProjectDescription = wbs.ProjectDesc,
                           fundcenterCode = fnd.FundCenter_Code,
                           fundcenterDescription = fnd.FundCenter_Description,
                           fundcenterOwner = fnd.FundCenter_Owner,
                           ProjectCode = wbs.ProjectCode,
                           FundCentreID = wbs.FundCentreID
                       }).ToList();
            return wbsDtls;
        }

        public List<TEPOGLCodeMaster> GetAllGLCodes()
        {
            var glList = (from gl in db.TEPOGLCodeMasters
                          where (gl.IsDeleted == false)
                          select gl).ToList();
            return glList;
        }

        private int GetLogInUserId()
        {
            var re = Request;
            var header = re.Headers;
            int authuser = 0;
            if (header.Contains("authUser"))
            {
                authuser = Convert.ToInt32(header.GetValues("authUser").First());
            }
            return authuser;
        }

        private bool IsPOViewRoleUser(int logInUserId)
        {
            bool IsPOViewRoleUser = true;
            //var rolename = (from userrole in db.webpages_UsersInRoles
            //                join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
            //                where userrole.UserId == logInUserId
            //                select new { webrole.RoleName }).ToList();
            //var role = rolename.Find(b => b.RoleName.Replace(" ", string.Empty).ToLower().Equals("poview"));

            //if (role != null)
            //{
            //    IsCEMViewRoleUser = true;
            //}
            return IsPOViewRoleUser;
        }

        public Tuple<string, string> GiveWBSCodes(string wbscode)
        {
            string tempWBSPart1 = string.Empty;
            string tempWBSPart2 = string.Empty;
            string[] splitwbsCode = wbscode.Split('-');
            tempWBSPart1 = splitwbsCode[0] + '-' + splitwbsCode[1] + '-' + splitwbsCode[2] + '-' + splitwbsCode[3];
            for (int i = 4; i < splitwbsCode.Count(); i++)
            {
                if (i < splitwbsCode.Count() - 1)
                    tempWBSPart2 += splitwbsCode[i] + '-';
                else
                    tempWBSPart2 += splitwbsCode[i];
            }
            return new Tuple<string, string>(tempWBSPart1, tempWBSPart2);
        }


        #region Sudheer WebAPIS

        [HttpPost]
        public HttpResponseMessage SaveMultipleExpensePurchaseItems(PurchaseItemStructureList itemStructure)
        {
            try
            {
                int saveupdatecount = 1;
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(itemStructure.LastModifiedBy));
                for (int cnt = 0; cnt < itemStructure.HSN_Code.Count(); cnt++)
                {
                    if (itemStructure.HSN_Code[cnt] != "0")
                    {
                        TEPOItemStructure itms = new TEPOItemStructure();
                        itms.POStructureId = itemStructure.HeaderStructureID;
                        itms.Purchasing_Order_Number = itemStructure.PurchasingOrderNumber;
                        itms.Short_Text = itemStructure.Long_Text[cnt];//shortDescription
                        itms.Long_Text = itemStructure.Long_Text[cnt];
                        //itms.Material_Number = itemStructure.Material_Number[cnt];
                        itms.HSNCode = itemStructure.HSN_Code[cnt];
                        itms.ItemType = itemStructure.ItemType[cnt];
                        itms.Unit_Measure = itemStructure.Unit_Measure[cnt];
                        itms.WBSElementCode = itemStructure.WBSElementCode[cnt];
                        // itms.WBSElementCode2 = itemStructure.WBSElementCode2[cnt];
                        itms.Order_Qty = itemStructure.Order_Qty[cnt];//Quantity
                        itms.Rate = itemStructure.Rate[cnt];//Rate
                        decimal? tempTotal = 0;
                        tempTotal = Convert.ToDecimal(itms.Order_Qty) * itms.Rate;
                        if(tempTotal > 0)
                        {
                            tempTotal = Math.Round((decimal)tempTotal, 2);
                        }
                        itms.TotalAmount = Convert.ToDecimal(itms.Order_Qty) * itms.Rate;//Total Amt

                        itms.IGSTRate = itemStructure.IGSTRate[cnt];
                        itms.IGSTAmount = (itms.TotalAmount * itms.IGSTRate) / 100;

                        itms.CGSTRate = itemStructure.CGSTRate[cnt];
                        itms.CGSTAmount = (itms.TotalAmount * itms.CGSTRate) / 100;

                        itms.SGSTRate = itemStructure.SGSTRate[cnt];
                        itms.SGSTAmount = (itms.TotalAmount * itms.SGSTRate) / 100;

                        itms.TotalTaxAmount = itms.IGSTAmount + itms.CGSTAmount + itms.SGSTAmount;
                        itms.GrossAmount = itms.TotalAmount + itms.TotalTaxAmount;

                        itms.GLAccountNo = "529900";
                        itms.Tax_salespurchases_code = itemStructure.Tax_salespurchases_code[cnt];
                        if (string.IsNullOrEmpty(itms.Tax_salespurchases_code))
                            itms.Tax_salespurchases_code = "G0";

                        itms.CreatedOn = System.DateTime.Now.ToString();
                        itms.CreatedBy = loginuser;
                        itms.LastModifiedBy = loginuser;
                        itms.LastModifiedOn = System.DateTime.Now.ToString();
                        itms.Status = "Draft";
                        itms.IsDeleted = false;
                        db.TEPOItemStructures.Add(itms);
                        db.SaveChanges();
                        saveupdatecount++;
                    }
                }
                if (saveupdatecount > 1)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Unable to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetLinkedDetails(JObject json)
        {
            int headerstructId = 0;
            if (json["POHeaderStructureID"] != null)
                headerstructId = json["POHeaderStructureID"].ToObject<int>();

            HttpResponseMessage hrm = new HttpResponseMessage();
            FailInfo finfo = new FailInfo();
            SuccessInfo sinfo = new SuccessInfo();
            try
            {
                var phsData = db.TEPOHeaderStructures.Where(a => a.Uniqueid == headerstructId && a.IsDeleted == false).FirstOrDefault();
                var LinkedPOList = (from linkpo in db.TEPOLinkedPOes
                                    join phs in db.TEPOHeaderStructures on linkpo.LinkedPOID equals phs.Uniqueid
                                    join appr in db.TEPOApprovers on phs.Uniqueid equals appr.POStructureId
                                    join fund in db.TEPOFundCenters on phs.FundCenterID equals fund.Uniqueid
                                    join vendordtl in db.TEPOVendorMasterDetails on phs.VendorID equals vendordtl.POVendorDetailId
                                    join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                    join proj in db.TEProjects on phs.ProjectID equals proj.ProjectID
                                    where (phs.IsDeleted == false && phs.IsNewPO == true
                                    && fund.Uniqueid == phsData.FundCenterID && appr.IsDeleted == false
                                    //&& appr.Status == "Approved" && phs.ReleaseCode2Status == "Approved"
                                    && linkpo.MainPOID == headerstructId && linkpo.IsDeleted == false
                                    && appr.SequenceNumber != 0)
                                    select new
                                    {
                                        linkpo.MainPOID,
                                        LinkPOUniqueID = linkpo.UniqueID,
                                        phs.PODescription,
                                        linkpo.LinkedPOID,
                                        phs.Purchasing_Order_Number,
                                        phs.Vendor_Account_Number,
                                        phs.Purchasing_Document_Date,
                                        phs.path,
                                        phs.ReleaseCode2Status,
                                        phs.ReleaseCode2Date,
                                        fund.FundCenter_Description,
                                        phs.Currency_Key,
                                        Vendor_Owner = vendor.VendorName,
                                        POHeaderstructureID = phs.Uniqueid,
                                        phs.PO_Title,
                                        phs.SubmitterName,
                                        phs.Managed_by,
                                        proj.ProjectCode,
                                        proj.ProjectName
                                    }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
                var LinkedPOIdsList = LinkedPOList.Select(a => a.LinkedPOID).ToList();
                var myPhsList = (from phs in db.TEPOHeaderStructures
                                 join appr in db.TEPOApprovers on phs.Uniqueid equals appr.POStructureId
                                 join fund in db.TEPOFundCenters on phs.FundCenterID equals fund.Uniqueid
                                 join vendordtl in db.TEPOVendorMasterDetails on phs.VendorID equals vendordtl.POVendorDetailId
                                 join vendor in db.TEPOVendorMasters on vendordtl.POVendorMasterId equals vendor.POVendorMasterId
                                 join proj in db.TEProjects on phs.ProjectID equals proj.ProjectID
                                 where (phs.IsDeleted == false && phs.IsNewPO == true
                                 && fund.Uniqueid == phsData.FundCenterID && appr.IsDeleted == false
                                 && phs.Uniqueid != headerstructId
                                 //&& appr.Status == "Approved" && phs.ReleaseCode2Status == "Approved"
                                 && appr.SequenceNumber != 0) && !LinkedPOIdsList.Contains(phs.Uniqueid)
                                 select new
                                 {
                                     phs.Purchasing_Order_Number,
                                     phs.PODescription,
                                     phs.Vendor_Account_Number,
                                     phs.Purchasing_Document_Date,
                                     phs.path,
                                     phs.ReleaseCode2Status,
                                     phs.ReleaseCode2Date,
                                     fund.FundCenter_Description,
                                     phs.Currency_Key,
                                     Vendor_Owner = vendor.VendorName,
                                     POHeaderstructureID = phs.Uniqueid,
                                     phs.PO_Title,
                                     phs.SubmitterName,
                                     phs.Managed_by,
                                     proj.ProjectCode,
                                     proj.ProjectName
                                 }).Distinct().OrderByDescending(a => a.Purchasing_Document_Date).ToList();
                int count = LinkedPOList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { LinkedPOList, POList = myPhsList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { LinkedPOList, POList = myPhsList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        //[HttpPost]
        //public HttpResponseMessage GetVendorShippingInfo(JObject json)
        //{
        //    try
        //    {
        //        int vendorid = json["VendorID"].ToObject<int>();
        //        var shippingData = (from shipdat in db.TEPOVendorShippingDetails
        //                            join vendor in db.TEPOVendors on shipdat.VendorID equals vendor.Uniqueid
        //                            where shipdat.VendorID == vendorid && shipdat.IsDeleted == false && vendor.IsDeleted == false
        //                            select new
        //                            {
        //                                shipdat.Address,
        //                                shipdat.CountryCode,
        //                                shipdat.GSTIN,
        //                                shipdat.StateCode,
        //                                shipdat.StateCodeDescription,
        //                                shipdat.VendorID,
        //                                shipdat.VendorShippingID,
        //                                VendorData = vendor
        //                            }).ToList();

        //        if (shippingData.Count > 0)
        //        {

        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Got Records";
        //            sinfo.fromrecords = 1;
        //            sinfo.torecords = 10;
        //            sinfo.pages = "1";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { ShippingData = shippingData, info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 1;
        //            sinfo.errormessage = "No Records Found";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { ShippingData = shippingData, info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail to get Records";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { BillingData = "", ShippingData = "", info = sinfo }) };
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage GetVendorShippingInfoById(JObject json)
        //{
        //    try
        //    {
        //        int vendorshipid = json["VendorShippingID"].ToObject<int>();
        //        var shippingData = (from shipdat in db.TEPOVendorShippingDetails
        //                            join vendor in db.TEPOVendors on shipdat.VendorID equals vendor.Uniqueid
        //                            where shipdat.VendorShippingID == vendorshipid && shipdat.IsDeleted == false && vendor.IsDeleted == false
        //                            select new
        //                            {
        //                                shipdat.Address,
        //                                shipdat.CountryCode,
        //                                shipdat.GSTIN,
        //                                shipdat.StateCode,
        //                                shipdat.StateCodeDescription,
        //                                shipdat.VendorID,
        //                                shipdat.VendorShippingID,
        //                                VendorData = vendor
        //                            }).FirstOrDefault();

        //        if (shippingData != null)
        //        {

        //            sinfo.errorcode = 0;
        //            sinfo.errormessage = "Successfully Got Records";
        //            sinfo.fromrecords = 1;
        //            sinfo.torecords = 10;
        //            sinfo.pages = "1";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { ShippingData = shippingData, info = sinfo }) };
        //        }
        //        else
        //        {
        //            sinfo.errorcode = 1;
        //            sinfo.errormessage = "No Records Found";
        //            return new HttpResponseMessage() { Content = new JsonContent(new { ShippingData = shippingData, info = sinfo }) };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionObj.RecordUnHandledException(ex);
        //        sinfo.errorcode = 1;
        //        sinfo.errormessage = "Fail to get Records";
        //        return new HttpResponseMessage() { Content = new JsonContent(new { ShippingData = "", info = sinfo }) };
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage BillingandShippingInfoByCompCode(JObject json)
        {
            TEPOPlantStorageDetail billingData = new TEPOPlantStorageDetail();
            List<TEPOPlantStorageDetail> shippingData = new List<TEPOPlantStorageDetail>();
            try
            {
                string ProjectCode = json["ProjectCode"].ToObject<string>();
                if (!string.IsNullOrEmpty(ProjectCode))
                {
                    billingData = db.TEPOPlantStorageDetails.Where(d => d.ProjectCode == ProjectCode && d.Type == "billing" && d.isdeleted == false).FirstOrDefault();
                    shippingData = db.TEPOPlantStorageDetails.Where(d => d.ProjectCode == ProjectCode && d.Type == "shipping" && d.isdeleted == false).ToList();

                    if (billingData != null || shippingData.Count > 0)
                    {

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Got Records";
                        sinfo.fromrecords = 1;
                        sinfo.torecords = 10;
                        sinfo.pages = "1";
                        return new HttpResponseMessage() { Content = new JsonContent(new { BillingData = billingData, ShippingData = shippingData, info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "No Records Found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { BillingData = billingData, ShippingData = shippingData, info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Plant Storage details Found with selected Project";
                    return new HttpResponseMessage() { Content = new JsonContent(new { BillingData = billingData, ShippingData = shippingData, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { BillingData = billingData, ShippingData = shippingData, info = sinfo }) };
            }


        }

        [HttpPost]
        public HttpResponseMessage SavePOGeneralTermsandConditions(GeneralTandCList genTandC)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(genTandC.LastModifiedBy));
                foreach (int id in genTandC.MasterTandCId)
                {
                    int sequenceid = 0;
                    sequenceid = db.TETermsAndConditions.Where(a => a.IsActive == true && a.PickListItemId == genTandC.PickListItemId && a.POHeaderStructureId == genTandC.POHeaderstructureID && a.IsDeleted == false).OrderByDescending(a => a.UniqueId).Select(a => a.SequenceId).FirstOrDefault();
                    TETermsAndCondition tandObj = new TETermsAndCondition();
                    tandObj.LastModifiedBy = loginUsername;
                    tandObj.CreatedBy = loginUsername;
                    tandObj.CreatedOn = DateTime.Now;
                    tandObj.LastModifiedOn = DateTime.Now;
                    tandObj.ModuleName = "PO";
                    tandObj.POHeaderStructureId = genTandC.POHeaderstructureID;
                    tandObj.IsActive = true;
                    tandObj.SequenceId = sequenceid;
                    tandObj.IsDeleted = false;
                    tandObj.MasterTandCId = id;
                    tandObj.MasterId = id.ToString();
                    tandObj.PickListItemId = genTandC.PickListItemId;
                    tandObj.Type = genTandC.PickListItemId.ToString();
                    db.TETermsAndConditions.Add(tandObj);
                    db.SaveChanges();
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveorUpdatePOSpecialTermsandConditions(TETermsAndCondition specialTandC)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(specialTandC.LastModifiedBy));

                int picklistId = 0;
                picklistId = new PurchaseOrderBAL().getPickListItemId("Special");
                //Updating existing PO TE TermsAndCondition
                if (specialTandC.UniqueId > 0)
                {
                    TETermsAndCondition duplicatecheck = db.TETermsAndConditions.Where(a => a.IsActive == true && a.UniqueId != specialTandC.UniqueId && a.IsDeleted == false
                    && a.SequenceId == specialTandC.SequenceId && a.ModuleName == "PO" && a.PickListItemId == picklistId
                    && a.POHeaderStructureId == specialTandC.POHeaderStructureId).FirstOrDefault();
                    if (duplicatecheck != null)
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Sequence No. already exists";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    TETermsAndCondition tc = db.TETermsAndConditions.Where(a => a.UniqueId == specialTandC.UniqueId && a.PickListItemId == picklistId && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                    if (tc != null)
                    {
                        tc.LastModifiedBy = loginUsername;
                        tc.LastModifiedOn = DateTime.Now;
                        tc.Title = specialTandC.Title;
                        tc.SequenceId = specialTandC.SequenceId;
                        tc.Condition = specialTandC.Condition;
                        db.Entry(tc).CurrentValues.SetValues(tc);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Special  Terms and Conditions not Found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    TETermsAndCondition duplicatecheck = db.TETermsAndConditions.Where(a => a.IsActive == true && a.IsDeleted == false
                    && a.SequenceId == specialTandC.SequenceId && a.ModuleName == "PO" && a.PickListItemId == picklistId
                    && a.POHeaderStructureId == specialTandC.POHeaderStructureId).FirstOrDefault();
                    if (duplicatecheck != null)
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Sequence No. already exists";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    specialTandC.LastModifiedBy = loginUsername;
                    specialTandC.CreatedBy = loginUsername;
                    specialTandC.CreatedOn = DateTime.Now;
                    specialTandC.LastModifiedOn = DateTime.Now;
                    specialTandC.ModuleName = "PO";
                    specialTandC.IsActive = true;
                    specialTandC.SequenceId = specialTandC.SequenceId;
                    specialTandC.IsDeleted = false;
                    specialTandC.PickListItemId = picklistId;
                    specialTandC.Type = picklistId.ToString();
                    db.TETermsAndConditions.Add(specialTandC);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePOGeneralTermsandConditions(TETermsAndCondition genTandC)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(genTandC.LastModifiedBy));
                TETermsAndCondition tc = db.TETermsAndConditions.Where(a => a.UniqueId == genTandC.UniqueId && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (tc != null)
                {
                    tc.LastModifiedBy = loginUsername;
                    tc.LastModifiedOn = DateTime.Now;
                    tc.Title = genTandC.Title;
                    tc.Condition = genTandC.Condition;
                    db.Entry(tc).CurrentValues.SetValues(tc);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Updated";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "General Terms and Conditions not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }

            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePOGeneralTermsandConditions(TETermsAndCondition genTandC)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(genTandC.LastModifiedBy));
                TETermsAndCondition tc = db.TETermsAndConditions.Where(a => a.UniqueId == genTandC.UniqueId && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (tc != null)
                {
                    tc.LastModifiedBy = loginUsername;
                    tc.LastModifiedOn = DateTime.Now;
                    tc.IsDeleted = true;
                    db.Entry(tc).CurrentValues.SetValues(tc);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "General Terms and Conditions not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }

            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetPOGeneralTermsandConditions(JObject json)
        {
            try
            {
                int poHeaderId = json["POHeaderstructureID"].ToObject<int>();
                int picklistitemitmId = 0;
                int count = 0; string picklistitemType = string.Empty;
                picklistitemitmId = new PurchaseOrderBAL().getPickListItemId("General");
                if (picklistitemitmId > 0)
                    picklistitemType = picklistitemitmId.ToString();
                var TandClist = (from tc in db.TETermsAndConditions
                                 join mtc in db.TEMasterTermsConditions on tc.MasterTandCId equals mtc.UniqueId
                                 where tc.ModuleName == "PO" && tc.POHeaderStructureId == poHeaderId && tc.PickListItemId == picklistitemitmId
                                           && tc.IsDeleted == false && mtc.IsDeleted == false
                                 select new { TandCId = tc.UniqueId, tc.MasterTandCId, mtc.ModuleName, mtc.Title, mtc.Condition, tc.PickListItemId, tc.SequenceId }).OrderBy(a => a.SequenceId).ToList();

                //var masterTandClist = (from mtc in db.TEMasterTermsConditions
                //                       where !(from tc in db.TEPOTermsAndConditions
                //                               where tc.ModuleName == "PO" && tc.POHeaderStructureId == poHeaderId && tc.PickListItemId == picklistitemitmId && tc.IsDeleted==false
                //                               select tc.MasterTandCId)
                //                               .Contains(mtc.UniqueId) && mtc.ModuleName == "PO" && mtc.Type== picklistitemType && mtc.IsDeleted == false
                //                       select new { mtc.Condition, mtc.UniqueId, mtc.Title }).Distinct().ToList();

                count = TandClist.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { TandClist, picklistitemitmId, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { TandClist, picklistitemitmId, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }


        }

        [HttpPost]
        public HttpResponseMessage GetPOGeneralTermsandConditionsToCreate(JObject json)
        {
            try
            {
                int poHeaderId = json["POHeaderstructureID"].ToObject<int>();
                int picklistitemitmId = 0;
                int count = 0;
                var masterIdsList = (from tc in db.TETermsAndConditions
                                     where tc.ModuleName == "PO" && tc.Type == picklistitemitmId.ToString() && tc.IsDeleted == false && tc.ContextIdentifier == poHeaderId.ToString()
                                     select tc.MasterId).Distinct().ToList();
                var list = (from mtc in db.TEMasterTermsConditions
                            join tc in db.TETermsAndConditions on mtc.UniqueId equals tc.UniqueId
                            where tc.ModuleName == "PO" && mtc.Type == "sz" && masterIdsList.Contains("1") && mtc.IsDeleted == false
                            select new
                            {
                                mtc.ModuleName,
                                mtc.Title,
                                mtc.Type,
                                mtc.SequenceId,
                                mtc.Condition,
                                tc.ContextIdentifier,
                            }).ToList();
                count = masterIdsList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = masterIdsList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = masterIdsList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }


        }

        public string CreategeneralTermsandConditions(string PoNumber)
        {
            string lsResponse = "";
            try
            {
                int parent = db.TEPickLists.Where(x => x.IsDeleted == false && x.Description == "TermsAndConditionType").Select(x => x.Uniqueid).FirstOrDefault();
                int Type = 0;
                Type = db.TEPickListItems.Where(x => x.IsDeleted == false && x.Description == "General" && x.TEPickList == parent).Select(x => x.Uniqueid).FirstOrDefault();
                string ConditionType = "";
                ConditionType = Convert.ToString(Type);

                List<TETermsAndCondition> PoTerms = db.TETermsAndConditions.Where(x => x.ContextIdentifier == PoNumber && x.IsDeleted == false && x.Type == ConditionType).ToList();
                if (PoTerms.Count == 0)
                {
                    List<TEMasterTermsCondition> MasterTerms = db.TEMasterTermsConditions.Where(x => x.IsDeleted == false && x.IsActive == true && x.Type == ConditionType).ToList();

                    int count = 0;

                    foreach (var term in MasterTerms)
                    {
                        TETermsAndCondition result = new TETermsAndCondition();
                        count = count + 1;
                        result.CreatedOn = System.DateTime.Now;
                        result.LastModifiedOn = System.DateTime.Now;
                        result.CreatedBy = "SapAdmin";
                        result.LastModifiedBy = "SapAdmin";
                        result.IsActive = true;
                        result.SequenceId = count;
                        result.Title = term.Title;
                        result.MasterId = Convert.ToString(term.UniqueId);
                        result.ContextIdentifier = PoNumber;
                        result.Condition = term.Condition;
                        result.Type = term.Type;
                        result.ModuleName = "PO";
                        db.TETermsAndConditions.Add(result);
                        db.SaveChanges();
                    }
                    lsResponse = "General Terms and Conditions  created successfully";
                }
            }
            catch (Exception Exc)
            {
                return "Error: " + Exc.Message;
            }
            return lsResponse;
        }
        [HttpPost]
        public HttpResponseMessage GetProjectsByCompanyCode(JObject json)
        {
            try
            {
                string cmpcode = string.Empty;
                cmpcode = json["CompanyCode"].ToObject<string>();
                var projectsList = (from proj in db.TEProjects
                                    join cmp in db.TECompanies on proj.CompanyID equals cmp.Uniqueid
                                    where cmp.CompanyCode == cmpcode && cmp.IsDeleted == false && proj.IsDeleted == false
                                    select new { proj.ProjectName, proj.ProjectShortName, proj.CompanyID, proj.ProjectID, cmp.Name, cmp.CompanyCode, cmp.StateCode, cmp.StateName }).ToList();
                if (projectsList.Count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = projectsList.Count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = projectsList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = projectsList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetClientProjectInfoByProjectCode(JObject json)
        {
            try
            {
                string projCode = string.Empty;
                projCode = json["ProjectCode"].ToObject<string>();
                var projectsList = (from proj in db.TEProjects
                                    join cmp in db.TECompanies on proj.CompanyID equals cmp.Uniqueid
                                    where proj.ProjectCode == projCode && cmp.IsDeleted == false && proj.IsDeleted == false
                                    select new { proj.ProjectName, proj.ProjectShortName, proj.CompanyID, proj.ProjectID, cmp.Name, cmp.CompanyCode, cmp.StateCode, cmp.StateName }).FirstOrDefault();
                if (projectsList != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = projectsList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = projectsList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOPaymentTermsById(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            List<TEPOVendorPaymentMilestone> paymentTermsList = new List<TEPOVendorPaymentMilestone>();
            try
            {
                int phsuniqueId = json["UniqueId"].ToObject<int>();
                paymentTermsList = db.TEPOVendorPaymentMilestones.Where(a => a.IsDeleted == false && a.ContextIdentifier == phsuniqueId.ToString()).ToList();
                int count = 0;
                count = paymentTermsList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = paymentTermsList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.totalrecords = count;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = paymentTermsList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = paymentTermsList }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage getShippingDetails(JObject json)
        {
            TEPOPlantStorageDetail shippingData = new TEPOPlantStorageDetail();
            try
            {
                int plantstorageDetId = json["ID"].ToObject<int>();
                shippingData = db.TEPOPlantStorageDetails.Where(d => d.PlantStorageDetailsID == plantstorageDetId && d.isdeleted == false).FirstOrDefault();
                if (shippingData != null)
                {

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = shippingData, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = shippingData, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = shippingData, info = sinfo }) };
            }


        }

        [HttpPost]
        public HttpResponseMessage SavePO(TEPOHeaderStructure pur_head_struct_obj)
        {
            int resultphsUniqueid = 0, loginId = 0;
            try
            {
                loginId = GetLogInUserId();
                resultphsUniqueid = new PurchaseOrderBAL().SavePO(pur_head_struct_obj, loginId);
                if (resultphsUniqueid > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to Save";
                    return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { UniqueId = resultphsUniqueid, info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetPODetailsById(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int phsuniqueId = json["UniqueId"].ToObject<int>();
                var poDetailsList = (from phs in db.TEPOHeaderStructures
                                     join tpfc in db.TEPOFundCenters on phs.FundCenterID equals tpfc.Uniqueid into temppfc
                                     from pfc in temppfc.DefaultIfEmpty()
                                     join tpv in db.TEPOVendorMasterDetails on phs.VendorID equals tpv.POVendorDetailId into temppv
                                     from vndrmasterdatail in temppv.DefaultIfEmpty()
                                     join vndMaster in db.TEPOVendorMasters on vndrmasterdatail.POVendorMasterId equals vndMaster.POVendorMasterId into tempvndMaster
                                     from vndrmstr in tempvndMaster.DefaultIfEmpty()
                                     join cntry in db.TEPOCountryMasters on vndrmasterdatail.CountryId equals cntry.UniqueID into tempcntry
                                     from country in tempcntry.DefaultIfEmpty()
                                     join state in db.TEGSTNStateMasters on vndrmasterdatail.RegionId equals state.StateID into tempstate
                                     from region in tempstate.DefaultIfEmpty()
                                     join te_billedto in db.TEPOPlantStorageDetails on phs.BilledToID equals te_billedto.PlantStorageDetailsID into tempBill
                                     from billedto in tempBill.DefaultIfEmpty()
                                     join te_shippedto in db.TEPOPlantStorageDetails on phs.ShippedToID equals te_shippedto.PlantStorageDetailsID into tempship
                                     from shippedto in tempship.DefaultIfEmpty()
                                     join order in db.TEPurchase_OrderTypes on phs.PO_OrderTypeID equals order.UniqueId into temporder
                                     from finalorder in temporder.DefaultIfEmpty()
                                     join proj in db.TEProjects on pfc.ProjectCode equals proj.ProjectCode into tempproj
                                     from prjct in tempproj.DefaultIfEmpty()
                                     join cmp in db.TECompanies on prjct.CompanyID equals cmp.Uniqueid into tempcmp
                                     from cmpny in tempcmp.DefaultIfEmpty()
                                     join manager in db.UserProfiles on phs.POManagerID equals manager.UserId into tempmgr
                                     from mnger in tempmgr.DefaultIfEmpty()
                                     where phs.Uniqueid == phsuniqueId && phs.IsDeleted == false
                                     select new
                                     {
                                         phs.ApprovedBy,
                                         phs.PODescription,
                                         phs.PO_Title,
                                         phs.PO_OrderTypeID,
                                         PO_OrdertypeDescription = finalorder.Description,
                                         PO_OrderTypeCode = finalorder.Code,
                                         phs.ApprovedOn,
                                         phs.Company_Code,
                                         phs.CreatedBy,
                                         phs.CreatedOn,
                                         phs.Currency_Key,
                                         phs.Exchange_Rate,
                                         phs.Fugue_Purchasing_Order_Number,
                                         phs.FundCenterID,
                                         phs.LastModifiedBy,
                                         phs.LastModifiedOn,
                                         Managed_by = mnger.CallName,
                                         phs.POManagerID,
                                         POManagerName = mnger.CallName,
                                         phs.Purchasing_Document_Date,
                                         phs.Purchasing_Document_Type,
                                         phs.Purchasing_Group,
                                         phs.Purchasing_Order_Number,
                                         phs.Purchasing_Organization,
                                         phs.ReleaseCode2Status,
                                         phs.ReleaseCodes,
                                         phs.ShippedFromID,
                                         phs.ShippedToID,
                                         phs.Status,
                                         phs.Statusversion,
                                         phs.SubmittedBy,
                                         phs.SubmitterComments,
                                         phs.SubmitterEmailID,
                                         phs.SubmitterName,
                                         phs.Uniqueid,
                                         phs.VendorID,
                                         phs.Vendor_Account_Number,
                                         phs.Version,
                                         cmpny.CompanyCode,
                                         cmpny.Name,
                                         cmpny.StateCode,
                                         cmpny.StateName,
                                         phs.ProjectID,
                                         prjct.ProjectCode,
                                         prjct.ProjectName,
                                         prjct.ProjectShortName,
                                         phs.IsPRPO,
                                         phs.PurchaseRequestId,
                                         FundcenterData = pfc,
                                         ShipToData = shippedto,
                                         phs.BilledToID,
                                         BilledToData = billedto,
                                         phs.BilledByID,
                                         TransactionType = "",
                                         NatureOfTransaction = "",
                                         NatureOfSupply = "",
                                         //Vendor Details 
                                         vndrmasterdatail.ShippingAddress,
                                         vndrmasterdatail.ShippingCity,
                                         vndrmasterdatail.ShippingPostalCode,
                                         vndrmasterdatail.GSTIN,
                                         vndrmasterdatail.VendorCode,
                                         vndrmasterdatail.BillingAddress,
                                         vndrmasterdatail.BillingCity,
                                         vndrmasterdatail.BillingPostalCode,
                                         vndrmasterdatail.CountryId,
                                         VendorGSTApplicability = vndrmasterdatail.GSTApplicabilityId,
                                         Country = country.Description,
                                         vndrmasterdatail.Designation,
                                         vndrmstr.VendorName,
                                         vndrmstr.CIN,
                                         vndrmstr.PAN,
                                         vndrmstr.Currency,
                                         vndrmstr.ServiceTax,
                                         RegionId = vndrmasterdatail.RegionId,
                                         RegionCodeDesc = region.StateName,
                                         RegionCode = region.StateCode,
                                     }).FirstOrDefault();
                var ApproversList = (from u in db.TEPOApprovers
                                     where (u.POStructureId == phsuniqueId
                                     && u.IsDeleted == false)
                                     orderby u.SequenceNumber
                                     select u).OrderBy(u => u.SequenceNumber).ToList();

                #region NatureOfSupplyDetails
                var purItemstructDtls = (from itm in db.TEPOItemStructures
                                         where itm.POStructureId == phsuniqueId && itm.IsDeleted == false
                                         && itm.IsDeleted == false
                                         select itm.ItemType).Distinct().ToList();
                string NatureofSupply = string.Empty;
                string NatureofTransaction = string.Empty;
                string TransactionType = string.Empty;

                if (purItemstructDtls.Count == 1 && purItemstructDtls.Contains("ExpenseOrder"))
                {
                    NatureofSupply = "Expense";
                }
                else if (purItemstructDtls.Count == 1 && purItemstructDtls.Contains("ServiceOrder"))
                {
                    NatureofSupply = "Services";
                }
                else if (purItemstructDtls.Count > 1 && purItemstructDtls.Contains("MaterialOrder") && purItemstructDtls.Contains("ServiceOrder"))
                {
                    NatureofSupply = "Goods & Services";
                }
                else if (purItemstructDtls.Count == 1 && purItemstructDtls.Contains("MaterialOrder"))
                {
                    NatureofSupply = "Goods";
                }

                if (poDetailsList.Currency != "INR")
                {
                    TransactionType = "Import";
                    NatureofTransaction = "Import";
                }
                else
                {
                    if (poDetailsList.ShipToData != null)
                    {
                        if (poDetailsList.RegionCode == poDetailsList.ShipToData.StateCode)
                        {
                            NatureofTransaction = "Intra-state";
                        }
                        else
                        {
                            NatureofTransaction = "inter-state";
                        }
                    }
                    else
                    {
                        NatureofTransaction = "inter-state";
                    }
                    TransactionType = "Domestic";
                }
                //poDetailsList.NatureOfSupply = "sdzcx";
                //if (material == 1 && service == 1)
                //{
                //    vm.NatureofSupply = 'Goods & Services';
                //}
                //else if (material == 0 && service == 1)
                //{
                //    vm.NatureofSupply = 'Services';
                //}
                //else if (material == 1 && service == 0)
                //{
                //    vm.NatureofSupply = 'Goods';
                //}
                //if (vm.poDetails.Currency_Key != "INR")
                //{
                //    vm.NatureofTransaction = "Import";
                //}
                //else if (vm.poDetails.VenderRegionCode == vm.poDetails.PlantRegionCode)
                //{
                //    vm.NatureofTransaction = "Intra-state";
                //}
                //else
                //{
                //    vm.NatureofTransaction = "inter-state";
                //}
                #endregion

                if (poDetailsList != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = poDetailsList, NatureofSupply, NatureofTransaction, TransactionType, ApproversList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Record Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = poDetailsList, NatureofSupply, NatureofTransaction, TransactionType, ApproversList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Record";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePO(TEPOHeaderStructure pur_head_struct_obj)
        {
            int loginId = 0;
            try
            {
                TEPOHeaderStructure phsObj = db.TEPOHeaderStructures.Where(a => a.IsDeleted == false && a.Uniqueid == pur_head_struct_obj.Uniqueid).FirstOrDefault();
                if (phsObj != null)
                {
                    loginId = GetLogInUserId();
                    phsObj.LastModifiedOn = DateTime.Now;
                    phsObj.LastModifiedBy = pur_head_struct_obj.LastModifiedBy;
                    phsObj.VendorID = pur_head_struct_obj.VendorID;
                    phsObj.FundCenterID = pur_head_struct_obj.FundCenterID;
                    phsObj.ProjectID = pur_head_struct_obj.ProjectID;
                    phsObj.BilledByID = pur_head_struct_obj.BilledByID;
                    phsObj.BilledToID = pur_head_struct_obj.BilledToID;
                    phsObj.POManagerID = pur_head_struct_obj.POManagerID;
                    phsObj.ShippedFromID = pur_head_struct_obj.ShippedFromID;
                    phsObj.ShippedToID = pur_head_struct_obj.ShippedToID;
                    if (!string.IsNullOrEmpty(pur_head_struct_obj.PO_Title))
                        phsObj.PO_Title = pur_head_struct_obj.PO_Title;
                    if (!string.IsNullOrEmpty(pur_head_struct_obj.PODescription))
                        phsObj.PODescription = pur_head_struct_obj.PODescription;
                    if (pur_head_struct_obj.PO_OrderTypeID > 0)
                        phsObj.PO_OrderTypeID = pur_head_struct_obj.PO_OrderTypeID;

                    db.Entry(phsObj).CurrentValues.SetValues(phsObj);
                    db.SaveChanges();
                    //if (phsObj.IsPRPO == true)
                    //{
                    //    UpdateTaxDetailsForPRItem(phsObj.Uniqueid);
                    //}
                    UpdateTaxDetailsForPRItem(phsObj.Uniqueid);
                    var ManagerAppr = (from usr in db.TEPOApprovers
                                       where usr.POStructureId == phsObj.Uniqueid && usr.IsDeleted == false && usr.SequenceNumber == 1
                                       select usr).FirstOrDefault();
                    if (ManagerAppr == null)
                    {
                        int managerId = Convert.ToInt32(pur_head_struct_obj.POManagerID);
                        new PurchaseOrderBAL().SavePOManagerForDraft(phsObj.Uniqueid, loginId, managerId);
                    }
                    else
                    {
                        if (ManagerAppr.ApproverId != phsObj.POManagerID)
                        {
                            ManagerAppr.IsDeleted = true;
                            db.Entry(ManagerAppr).CurrentValues.SetValues(ManagerAppr);
                            db.SaveChanges();
                            int managerId = Convert.ToInt32(pur_head_struct_obj.POManagerID);
                            new PurchaseOrderBAL().SavePOManagerForDraft(phsObj.Uniqueid, loginId, managerId);
                        }
                    }
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Unable to Update";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        public void UpdateTaxDetailsForPRItem(int headerStructureId)
        {
            try
            {
                TEPOHeaderStructure phsObj = db.TEPOHeaderStructures.Where(a => a.IsDeleted == false && a.Uniqueid == headerStructureId).FirstOrDefault();
                var itemStructureList = db.TEPOItemStructures.Where(a => a.IsDeleted == false && a.POStructureId == phsObj.Uniqueid).ToList();
                if (itemStructureList.Count > 0)
                {
                    var plantStrg = db.TEPOPlantStorageDetails.Where(a => a.isdeleted == false && a.PlantStorageDetailsID == phsObj.ShippedToID).FirstOrDefault();
                    var vendr = (from vendrDtl in db.TEPOVendorMasterDetails
                                 join country in db.TEPOCountryMasters on vendrDtl.CountryId equals country.UniqueID into cntry
                                 from cntr in cntry.DefaultIfEmpty()
                                 join region in db.TEGSTNStateMasters on vendrDtl.RegionId equals region.StateID into rgn
                                 from vendRgn in rgn.DefaultIfEmpty()
                                 where vendrDtl.POVendorDetailId == phsObj.VendorID
                                 select new
                                 {
                                     CountryCode = cntr.Description != null ? cntr.Description : string.Empty,
                                     StateCode = vendRgn.StateCode != null ? vendRgn.StateCode : string.Empty,
                                     GSTApplicabilityId = vendrDtl.GSTApplicabilityId != null ? vendrDtl.GSTApplicabilityId : 0
                                 }
                                ).FirstOrDefault();
                    if (plantStrg != null && vendr != null)
                    {
                        foreach (TEPOItemStructure itemStuct in itemStructureList)
                        {
                            // if (string.IsNullOrEmpty(itemStuct.Tax_salespurchases_code))
                            // {
                            PurchaseTaxInput taxInput = new PurchaseTaxInput();
                            if (vendr != null)
                            {
                                taxInput.CountryCode = vendr.CountryCode;
                                taxInput.HSNCode = itemStuct.HSNCode;
                                taxInput.PlantRegionCode = plantStrg.StateCode;
                                taxInput.VendorRegionCode = vendr.StateCode;
                                taxInput.VendorGSTApplicabilityId = vendr.GSTApplicabilityId;
                                taxInput.OrderType = itemStuct.ItemType;
                            }
                            //PurchaseTaxDetails taxDtls = GetTaxDetailsForItem(taxInput);
                            PurchaseTaxDetails taxDtls = GetTaxDetailsForOrderItem(taxInput);
                            if (taxDtls != null)
                            {
                                itemStuct.Tax_salespurchases_code = taxDtls.TaxCode;
                                itemStuct.CGSTRate = taxDtls.CGSTTaxRate;
                                itemStuct.SGSTRate = taxDtls.SGSTTaxRate;
                                itemStuct.IGSTRate = taxDtls.IGSTTaxRate;
                                //begining of calculations  
                                if (itemStuct.TotalAmount > 0)
                                {
                                    itemStuct.IGSTAmount = (itemStuct.TotalAmount * taxDtls.IGSTTaxRate) / 100;
                                    itemStuct.CGSTAmount = (itemStuct.TotalAmount * taxDtls.CGSTTaxRate) / 100;
                                    itemStuct.SGSTAmount = (itemStuct.TotalAmount * taxDtls.SGSTTaxRate) / 100;
                                    itemStuct.TotalTaxAmount = itemStuct.IGSTAmount + itemStuct.CGSTAmount + itemStuct.SGSTAmount;
                                    itemStuct.GrossAmount = itemStuct.TotalAmount + itemStuct.TotalTaxAmount;
                                }
                                //ending of calculations
                                db.Entry(itemStuct).CurrentValues.SetValues(itemStuct);
                                db.SaveChanges();
                            }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveorUpdatePaymentTerm(TEPOVendorPaymentMilestone vendorpaymilestone)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(vendorpaymilestone.LastModifiedBy));

                //Updating existing PO payment Term
                if (vendorpaymilestone.UniqueId > 0)
                {
                    TEPOVendorPaymentMilestone vpmObj = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == vendorpaymilestone.UniqueId && a.IsDeleted == false).FirstOrDefault();
                    if (vpmObj != null)
                    {
                        vpmObj.LastModifiedBy = loginUsername;
                        vpmObj.LastModifiedOn = DateTime.Now;
                        vpmObj.ModuleName = vendorpaymilestone.ModuleName;
                        vpmObj.PaymentTerm = vendorpaymilestone.PaymentTerm;
                        vpmObj.Percentage = vendorpaymilestone.Percentage;
                        vpmObj.Date = vendorpaymilestone.Date;
                        vpmObj.Amount = vendorpaymilestone.Amount;
                        vpmObj.Remarks = vendorpaymilestone.Remarks;
                        db.Entry(vpmObj).CurrentValues.SetValues(vpmObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Payment Terms Details not Found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    vendorpaymilestone.CreatedOn = DateTime.Now;
                    vendorpaymilestone.CreatedBy = loginUsername;
                    vendorpaymilestone.LastModifiedBy = loginUsername;
                    vendorpaymilestone.LastModifiedOn = DateTime.Now;
                    vendorpaymilestone.IsDeleted = false;
                    if (!string.IsNullOrEmpty(vendorpaymilestone.ContextIdentifier))
                        vendorpaymilestone.POHeaderStructureId = Convert.ToInt32(vendorpaymilestone.ContextIdentifier);
                    db.TEPOVendorPaymentMilestones.Add(vendorpaymilestone);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveorUpdatePaymentTermList(List<TEPOVendorPaymentMilestone> vendorpaymilestoneList)
        {
            try
            {
                int loginId = 0;
                loginId = GetLogInUserId();
                if (vendorpaymilestoneList.Count > 0)
                {
                    var vendorpaymilestoneLst = vendorpaymilestoneList.Where(a => a.PaymentTerm != "0").ToList();
                    if (vendorpaymilestoneLst.Count > 0)
                    {

                        foreach (TEPOVendorPaymentMilestone vendorpaymilestone in vendorpaymilestoneLst)
                        {
                            string loginUsername = string.Empty;
                            loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(loginId);

                            //Updating existing PO payment Term
                            if (vendorpaymilestone.UniqueId > 0)
                            {
                                TEPOVendorPaymentMilestone vpmObj = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == vendorpaymilestone.UniqueId && a.IsDeleted == false).FirstOrDefault();
                                if (vpmObj != null)
                                {
                                    vpmObj.LastModifiedBy = loginUsername;
                                    vpmObj.LastModifiedOn = DateTime.Now;
                                    vpmObj.ModuleName = vendorpaymilestone.ModuleName;
                                    vpmObj.PaymentTerm = vendorpaymilestone.PaymentTerm;
                                    vpmObj.Percentage = vendorpaymilestone.Percentage;
                                    vpmObj.Date = vendorpaymilestone.Date;
                                    vpmObj.Amount = vendorpaymilestone.Amount;
                                    vpmObj.Remarks = vendorpaymilestone.Remarks;
                                    db.Entry(vpmObj).CurrentValues.SetValues(vpmObj);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    sinfo.errorcode = 1;
                                    sinfo.errormessage = "Payment Terms Details not Found";
                                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                                }
                            }
                            else
                            {
                                vendorpaymilestone.CreatedOn = DateTime.Now;
                                vendorpaymilestone.CreatedBy = loginUsername;
                                vendorpaymilestone.LastModifiedBy = loginUsername;
                                vendorpaymilestone.LastModifiedOn = DateTime.Now;
                                vendorpaymilestone.IsDeleted = false;
                                if (!string.IsNullOrEmpty(vendorpaymilestone.ContextIdentifier))
                                    vendorpaymilestone.POHeaderStructureId = Convert.ToInt32(vendorpaymilestone.ContextIdentifier);
                                db.TEPOVendorPaymentMilestones.Add(vendorpaymilestone);
                                db.SaveChanges();
                            }
                        }
                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Saved";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Payment Terms Details not Found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Payment Terms Details not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePaymentTerm(TEPOVendorPaymentMilestone vendorpaymilestone)
        {
            try
            {
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(vendorpaymilestone.LastModifiedBy));

                TEPOVendorPaymentMilestone vpmObj = db.TEPOVendorPaymentMilestones.Where(a => a.UniqueId == vendorpaymilestone.UniqueId && a.IsDeleted == false).FirstOrDefault();
                if (vpmObj != null)
                {
                    vpmObj.LastModifiedBy = loginUsername;
                    vpmObj.LastModifiedOn = DateTime.Now;
                    vpmObj.IsDeleted = true;
                    db.Entry(vpmObj).CurrentValues.SetValues(vpmObj);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Payment Terms Details not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOSpecialTandC(JObject json)
        {
            try
            {
                int poid = 0;
                poid = json["POUniqueID"].ToObject<int>();
                int picklistitemitmId = 0;
                int count = 0; string picklistitemType = string.Empty;
                picklistitemitmId = new PurchaseOrderBAL().getPickListItemId("Special");
                //if (picklistitemitmId > 0)
                //    picklistitemType = picklistitemitmId.ToString();

                var tcList = db.TETermsAndConditions.Where(a => a.IsDeleted == false && a.IsActive == true && a.PickListItemId == picklistitemitmId && a.ModuleName == "PO" && a.POHeaderStructureId == poid).OrderBy(a => a.SequenceId).ToList();
                count = tcList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = tcList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = tcList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOPaymentTerms(JObject json)
        {
            try
            {
                int poid = 0;
                poid = json["POUniqueID"].ToObject<int>();

                var vpmList = db.TEPOVendorPaymentMilestones.Where(a => a.IsDeleted == false && a.ModuleName == "PO" && a.ContextIdentifier == poid.ToString()).ToList();
                int count = vpmList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = vpmList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = vpmList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePOSpecialTermsandConditions(TETermsAndCondition specialTandC)
        {
            try
            {
                //Retrieving login user details
                string loginUsername = string.Empty;
                loginUsername = new PurchaseOrderBAL().getloginUsernamebyId(Convert.ToInt32(specialTandC.LastModifiedBy));
                TETermsAndCondition tc = db.TETermsAndConditions.Where(a => a.UniqueId == specialTandC.UniqueId && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();
                if (tc != null)
                {
                    tc.LastModifiedBy = loginUsername;
                    tc.LastModifiedOn = DateTime.Now;
                    tc.IsDeleted = true;
                    tc.Condition = specialTandC.Condition;
                    db.Entry(tc).CurrentValues.SetValues(tc);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Updated";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Special Terms Details not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOSpecialConditionsbyPOID(JObject json)
        {
            try
            {
                int poid = 0;
                poid = json["POUniqueID"].ToObject<int>();

                var tcList = db.TETermsAndConditions.Where(a => a.IsDeleted == false && a.IsActive == true && a.ModuleName == "PO" && a.ContextIdentifier == poid.ToString()).ToList();
                int count = tcList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = tcList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = tcList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveorUpdateLinkedPO(TEPOLinkedPO linkedPO)
        {
            try
            {
                //Updating existing PO payment Term
                if (linkedPO.UniqueID > 0)
                {
                    TEPOLinkedPO linkedPOObj = db.TEPOLinkedPOes.Where(a => a.UniqueID == linkedPO.UniqueID && a.IsDeleted == false).FirstOrDefault();
                    if (linkedPOObj != null)
                    {
                        linkedPOObj.LastModifiedBy = linkedPO.LastModifiedBy;
                        linkedPOObj.LastModifiedOn = DateTime.Now;
                        linkedPOObj.LinkedPOID = linkedPO.LinkedPOID;
                        linkedPOObj.MainPOID = linkedPO.MainPOID;
                        db.Entry(linkedPOObj).CurrentValues.SetValues(linkedPOObj);
                        db.SaveChanges();

                        sinfo.errorcode = 0;
                        sinfo.errormessage = "Successfully Updated";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    else
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Linked PO Details not Found";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    linkedPO.LastModifiedBy = linkedPO.LastModifiedBy;
                    linkedPO.LastModifiedOn = DateTime.Now;
                    linkedPO.IsDeleted = false;
                    db.TEPOLinkedPOes.Add(linkedPO);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save/Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveMultipleLinkPurchaseOrders(Purchase_LinkedPO_List linkedPOList)
        {
            int count = 0;
            try
            {
                foreach (var poId in linkedPOList.LinkedPOID)
                {
                    TEPOLinkedPO linkedPOObj = db.TEPOLinkedPOes.Where(a => a.UniqueID == poId && a.IsDeleted == false).FirstOrDefault();
                    if (linkedPOObj == null)
                    {
                        TEPOLinkedPO linkPO = new TEPOLinkedPO();
                        linkPO.LastModifiedBy = linkedPOList.LastModifiedBy;
                        linkPO.LastModifiedOn = DateTime.Now;
                        linkPO.LinkedPOID = poId;
                        linkPO.IsDeleted = false;
                        linkPO.MainPOID = linkedPOList.MainPOID;
                        db.TEPOLinkedPOes.Add(linkPO);
                        db.SaveChanges();
                        count++;
                    }
                }
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Saved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "PO already Exists";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Save";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteLinkedPO(TEPOLinkedPO linkedPO)
        {
            try
            {
                TEPOLinkedPO linkedPOObj = db.TEPOLinkedPOes.Where(a => a.UniqueID == linkedPO.UniqueID && a.IsDeleted == false).FirstOrDefault();
                if (linkedPOObj != null)
                {
                    linkedPOObj.LastModifiedBy = linkedPO.LastModifiedBy;
                    linkedPOObj.LastModifiedOn = DateTime.Now;
                    linkedPOObj.IsDeleted = true;
                    db.Entry(linkedPOObj).CurrentValues.SetValues(linkedPOObj);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Linked PO Details not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public string CreatePO(TEPOHeaderStructure obj)
        {

            TEPOHeaderStructure PoStructure = new TEPOHeaderStructure();
            PoStructure = db.TEPOHeaderStructures.Add(obj);
            db.SaveChanges();
            return PoStructure.Uniqueid.ToString();
        }

        [HttpPost]
        public HttpResponseMessage TEPurchase_GetAllMaterials()
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var url = ComponentLibraryHost + "/materials/all?details=true&batchSize=20";
                // var url = "https://clapi.total-environment.com/materials/all?details=true&batchSize=20";

                var tokenResponse = client.GetAsync(url).Result;
                if (tokenResponse.StatusCode.Equals(200))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                List<MaterialDTO> list = new List<MaterialDTO>();
                list = POBAL.ParseJobject(tokenResponse);
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = list

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }

        [HttpPost]
        public HttpResponseMessage SearchGroups_Materials(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            try
            {
                searchOj = json["Search"].ToObject<string>();
                if (!string.IsNullOrEmpty(searchOj))
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var url = ComponentLibraryHost + "/materials/group/" + searchOj;
                    // var url = "https://clapi.total-environment.com/materials/group/" + searchOj;
                    var tokenResponse = client.GetAsync(url).Result;
                    if (tokenResponse.StatusCode.Equals(200))
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Successful");
                    }
                    else
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Unable to Get");
                    }
                    var onjs = tokenResponse.Content.ReadAsStringAsync();

                    string responseString = string.Empty;
                    responseString = onjs.Result.Replace("\r\n", "").Replace("\"", "'");
                    responseString = responseString.Replace("',  '", "##").Replace("'", "").Replace("[", "").Replace("]", "");
                    string[] Temps = responseString.Split(new string[] { "##" }, StringSplitOptions.None);
                    List<SearchMaterialGrpResult> materialList = new List<SearchMaterialGrpResult>();
                    foreach (string s in Temps) materialList.Add(new SearchMaterialGrpResult { GroupName = s.TrimEnd().TrimStart() });
                    hrm.Content = new JsonContent(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        result = materialList,
                    });
                    return hrm;
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }


        [HttpPost]
        public HttpResponseMessage SearchBy_Materials(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            try
            {
                searchOj = json["Search"].ToObject<string>();
                if (!string.IsNullOrEmpty(searchOj))
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var url = ComponentLibraryHost + "/materials/group/" + searchOj;
                    // var url = "https://clapi.total-environment.com/materials/group/" + searchOj;
                    var tokenResponse = client.GetAsync(url).Result;
                    var onjs = tokenResponse.Content.ReadAsStringAsync();

                    List<SearchMaterialGrpResult> materialList = new List<SearchMaterialGrpResult>();
                    string[] stringSeparators = new string[] { "\r\n" };
                    string[] lines = onjs.Result.Split(stringSeparators, StringSplitOptions.None);
                    #region for the Direct Material Search without Dropdown
                    if (lines.Count() == 3)
                    {
                        SearchMaterialGroup searchNewObj = new Models.SearchMaterialGroup();
                        searchNewObj.searchQuery = searchOj;
                        //searchNewObj.groupname = lines[1].Replace("\\", "").Replace("\"", "").Replace(",", "").Trim();
                        searchNewObj.groupname = lines[1].Replace("\\", "").Replace("\"", "").Trim();
                        return SearchWithinGroups_Materials(searchNewObj);
                    }
                    #endregion
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(e.Message) };
            }
            sinfo.errorcode = 1;
            sinfo.errormessage = "Fail to get Data";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }


        [HttpPost]
        public HttpResponseMessage SearchWithinGroups_Materials(SearchMaterialGroup searchObj)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            searchOj = searchObj.searchQuery;
            try
            {
                searchObj.pageNumber = 1;
                searchObj.batchSize = 999999;
                searchObj.details = true;
                searchObj.filterDatas = new List<string>();
                if (searchOj != null)
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    // string token = "";
                    // token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJLVmN1enFBaWRPTHFXU2FvbDd3Z0ZSR0NZbyIsImtpZCI6IjJLVmN1enFBaWRPTHFXU2FvbDd3Z0ZSR0NZbyJ9.eyJhdWQiOiJodHRwczovL2FiZHVsc2F0dGhvdWdodHdvcmtzLm9ubWljcm9zb2Z0LmNvbS9xYWVkZXNpZ25hcGkiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC8yNTlkZTZjMi05ZDRhLTRiYzUtOGQ2Zi1kNTllNGUxYzhmZDkvIiwiaWF0IjoxNTEwODM0NDkxLCJuYmYiOjE1MTA4MzQ0OTEsImV4cCI6MTUxMDgzODM5MSwiYWlvIjoiWTJOZ1lMRGg1LzY3NHZUVWdJL0tQN095dDFZYUFnQT0iLCJhcHBpZCI6ImIxNjBlNzkyLWJkYzMtNGY3Ny05NDBlLTQ1NjA0N2EwNTU2NiIsImFwcGlkYWNyIjoiMSIsImlkcCI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0LzI1OWRlNmMyLTlkNGEtNGJjNS04ZDZmLWQ1OWU0ZTFjOGZkOS8iLCJvaWQiOiIzMmU4YzllYy04YjhjLTRkNTAtYWQ5MS02MDk0NjZmYzFiYmEiLCJzdWIiOiIzMmU4YzllYy04YjhjLTRkNTAtYWQ5MS02MDk0NjZmYzFiYmEiLCJ0aWQiOiIyNTlkZTZjMi05ZDRhLTRiYzUtOGQ2Zi1kNTllNGUxYzhmZDkiLCJ2ZXIiOiIxLjAifQ.DEaHnpbFPB11KEsqmM83jgc8unONAVrQ-pxRLh3qIjeCc6pEcWAqbu2vqSomwM_ZHfC-Rbc2ZO67BlXxEcLH_QilL9W5P0ctsAE7wGefRthfCUQ9sHBYlCvrA7jVmTmFqJxUoiXxszLDIqYJEz_JcIFGPDKXauKmp2RG8b6bS1Y0E9cca9QTdD8dEo-359uPU2RaF_LjcphVWwSfZ8ZRE4b13muqz25EYzRVLzM316sVOdPJf75KJ-9uIZgLLncvWQMPcyHtI4vBqdoTVLxRzBV4nH8VS9xW10VHaZbw2zNdTsh3ScGaOFG7b69H3FCQi1vpGpOYCRxrCqYI706gLw";
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var url = ComponentLibraryHost + "/materials/searchwithingroup";
                    //var url = "https://clapi.total-environment.com/materials/searchwithingroup";

                    var tokenResponse = client.PostAsJsonAsync(url, searchObj).Result;
                    if (tokenResponse.StatusCode.Equals(200))
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Successful");
                    }
                    else
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Unable to Get");
                    }
                    List<MaterialDTO> list = new List<MaterialDTO>();
                    List<MaterialDTO> ApprovedList = new List<MaterialDTO>();
                    list = POBAL.ParseJobject(tokenResponse);
                    if (list.Count > 0)
                    {
                        ApprovedList = list.Where(a => a.material_status == "Approved").ToList();
                    }
                    hrm.Content = new JsonContent(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        result = ApprovedList

                    });
                    return hrm;
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage SearchBy_Services(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            try
            {
                searchOj = json["Search"].ToObject<string>();
                if (!string.IsNullOrEmpty(searchOj))
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var url = ComponentLibraryHost + "/services/group-search?searchQuery=" + searchOj;
                    // var url = "https://clapi.total-environment.com/services/group-search?searchQuery=" + searchOj;
                    var tokenResponse = client.GetAsync(url).Result;
                    var onjs = tokenResponse.Content.ReadAsStringAsync();

                    string responseString = string.Empty;
                    responseString = onjs.Result.Replace("\r\n", "").Replace("\"", "'");
                    responseString = responseString.Replace("',  '", "##").Replace("'", "").Replace("[", "").Replace("]", "");
                    string[] Temps = responseString.Split(new string[] { "##" }, StringSplitOptions.None);
                    foreach (string s in Temps)
                    {
                        SearchMaterialGroup searchNewObj = new Models.SearchMaterialGroup();
                        searchNewObj.searchQuery = searchOj;
                        searchNewObj.groupname = s.TrimEnd().TrimStart();
                        return SearchWithinGroups_Services(searchNewObj);
                    }
                    #endregion
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent(e.Message) };
            }
            sinfo.errorcode = 1;
            sinfo.errormessage = "Fail to get Data";
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
        }


        [HttpPost]
        public HttpResponseMessage SearchGroups_Services(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            try
            {
                searchOj = json["Search"].ToObject<string>();
                if (!string.IsNullOrEmpty(searchOj))
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var url = ComponentLibraryHost + "/services/group-search?searchQuery=" + searchOj;
                    // var url = "https://clapi.total-environment.com/services/group-search?searchQuery=" + searchOj;
                    var tokenResponse = client.GetAsync(url).Result;
                    if (tokenResponse.StatusCode.Equals(200))
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Successful");
                    }
                    else
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Unable to Get");
                    }
                    var onjs = tokenResponse.Content.ReadAsStringAsync();
                    string responseString = string.Empty;
                    responseString = onjs.Result.Replace("\r\n", "").Replace("\"", "'");
                    responseString = responseString.Replace("',  '", "##").Replace("'", "").Replace("[", "").Replace("]", "");
                    string[] Temps = responseString.Split(new string[] { "##" }, StringSplitOptions.None);
                    List<SearchMaterialGrpResult> serrviceList = new List<SearchMaterialGrpResult>();
                    foreach (string s in Temps) serrviceList.Add(new SearchMaterialGrpResult { GroupName = s.TrimEnd().TrimStart() });
                    hrm.Content = new JsonContent(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        result = serrviceList,
                    });
                    return hrm;
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage SearchWithinGroups_Services(SearchMaterialGroup searchObj)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            string searchOj = string.Empty;
            searchOj = searchObj.searchQuery;
            try
            {
                //searchObj.groupname=
                //searchObj.searchQuery=
                searchObj.pageNumber = 0;
                searchObj.batchSize = 999999;
                searchObj.details = true;
                searchObj.filterDatas = new List<string>();
                if (searchOj != null)
                {
                    Token tokenObj = generateToken_ComponentLibrary();
                    string token = tokenObj.AccessToken;
                    HttpClient client = new HttpClient();
                    string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                    string tenant = WebConfigurationManager.AppSettings["tenant"];

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var url = ComponentLibraryHost + "/services?searchKeyword=" + searchObj.searchQuery + "&pageNumber=0&serviceLevel1=" + searchObj.groupname;
                    //var url = ComponentLibraryHost + "/services/searchwithingroup";
                    //var url = "https://clapi.total-environment.com/services/searchwithingroup";

                    // var tokenResponse_Old = client.PostAsJsonAsync(url, searchObj).Result;

                    var tokenResponse = client.GetAsync(url).Result;

                    if (tokenResponse.StatusCode.Equals(200))
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Successful");
                    }
                    else
                    {
                        //if (log.IsDebugEnabled)
                        //    log.Debug("Unable to Get");
                    }
                    List<MaterialDTO> servicelist = new List<MaterialDTO>();
                    List<MaterialDTO> ApprovedList = new List<MaterialDTO>();
                    servicelist = POBAL.ParseServiceJobject(tokenResponse);
                    if (servicelist.Count > 0)
                    {
                        ApprovedList = servicelist.Where(a => a.material_status == "Approved").ToList();
                    }
                    hrm.Content = new JsonContent(new
                    {
                        StatusCode = HttpStatusCode.OK,
                        result = ApprovedList

                    });
                    return hrm;
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Fail to get Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        public Token generateToken_ComponentLibrary()
        {
            Token tokenObj = new Token();
            try
            {
                HttpClient client = new HttpClient();
                string baseAddress = WebConfigurationManager.AppSettings["baseAddress"];
                string grant_type = WebConfigurationManager.AppSettings["grant_type"];
                string client_id = WebConfigurationManager.AppSettings["client_id"];
                string resource = WebConfigurationManager.AppSettings["resource"];
                string client_secret = WebConfigurationManager.AppSettings["client_secret"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];
                var form = new Dictionary<string, string>
                {
                   { "grant_type",grant_type},
                   {"client_id",client_id},
                   {"resource",resource},
                   { "client_secret",client_secret},
                };
                var tokenResponse = client.PostAsync(baseAddress + "/" + tenant + "/oauth2/token", new FormUrlEncodedContent(form)).Result;
                if (tokenResponse.StatusCode.Equals(200) || tokenResponse.ReasonPhrase.Equals("OK"))
                {
                    tokenObj = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }

            return tokenObj;
        }

        [HttpPost]
        public HttpResponseMessage GetPOTypes()
        {
            try
            {
                var orderList = (from order in db.TEPurchase_OrderTypes
                                 where order.IsDeleted == false
                                 select new
                                 {
                                     order.Code,
                                     order.Description,
                                     order.UniqueId
                                 }).ToList();
                if (orderList.Count > 0)
                {

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = orderList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = orderList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = orderList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = "", info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage TEPurchase_GetMaterialSpecifications_Old(MaterialSpecification mtlspecs)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                List<object> list = new List<object>();
                if (mtlspecs != null)
                {
                    foreach (string code in mtlspecs.MaterialCode)
                    {
                        HttpResponseMessage hrms = new HttpResponseMessage();
                        hrms = TEPurchase_GetMaterialByMaterialCode(code);
                        var onjs = hrms.Content.ReadAsStringAsync();
                        JObject jo = JObject.Parse(onjs.Result);
                        JToken x = jo.SelectToken("headers");
                        foreach (JToken v in x)
                        {
                            if (v["key"].ToString() == "specifications")
                            {
                                list.Add(v);
                            }
                        }
                        //foreach(string obj in list)
                        //{
                        //    HttpResponseMessage hrmss = new HttpResponseMessage();
                        //    hrmss = TEPurchase_GetMaterialCheckListByID(obj);
                        //    var ons = hrmss.Content.ReadAsStringAsync();
                        //    JObject joi = JObject.Parse(ons.Result);
                        //    listm.Add(joi);
                        //}
                    }
                }
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = list

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage TEPurchase_GetMaterialSpecifications(MaterialSpecification mtlspecs)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                List<columnsStruct> specColumnsList = new List<columnsStruct>();
                List<columnsStruct> classificationList = new List<columnsStruct>();
                List<object> list = new List<object>();
                ObjFromComponentLib myList = new ObjFromComponentLib();
                if (mtlspecs != null)
                {
                    foreach (string code in mtlspecs.MaterialCode)
                    {
                        HttpResponseMessage hrms = new HttpResponseMessage();
                        hrms = TEPurchase_GetMaterialByMaterialCode(code);
                        var onjs = hrms.Content.ReadAsStringAsync();
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        myList = JsonConvert.DeserializeObject<ObjFromComponentLib>(onjs.Result, settings);
                        JObject jo = JObject.Parse(onjs.Result);
                        JToken x = jo.SelectToken("headers");

                        if (myList != null)
                        {
                            try
                            {
                                specColumnsList = myList.headers.Find(a => a.key.Equals("specifications")).columns.ToList();
                                classificationList = myList.headers.Find(a => a.key.Equals("classification")).columns.ToList();
                            }
                            catch (Exception ex)
                            {
                                ExceptionObj.RecordUnHandledException(ex);
                            }
                        }
                        //foreach (JToken v in x)
                        //{
                        //    if (v["key"].ToString() == "specifications")
                        //    {
                        //        list.Add(v);
                        //    }
                        //}
                        //foreach(string obj in list)
                        //{
                        //    HttpResponseMessage hrmss = new HttpResponseMessage();
                        //    hrmss = TEPurchase_GetMaterialCheckListByID(obj);
                        //    var ons = hrmss.Content.ReadAsStringAsync();
                        //    JObject joi = JObject.Parse(ons.Result);
                        //    listm.Add(joi);
                        //}
                    }
                }
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = specColumnsList,
                    classificatoinList = classificationList

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage TEPurchase_GetMaterialByMaterialCode(string materialCode)
        {
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // https://localhost:44380/materials/MFR023128?dataType=true
                var url = ComponentLibraryHost + "/materials/" + materialCode + "?dataType=true";

                var tokenResponse = client.GetAsync(url).Result;
                if (tokenResponse.StatusCode.Equals(201))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                //return new HttpResponseMessage()
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Content = new StringContent(mesg)
                //};
                return tokenResponse;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }

        public string GetMaterialSpecificationChecklistId(string materialCode)
        {
            string CheckListId = string.Empty;
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // https://localhost:44380/materials/MFR023128?dataType=true
                var url = ComponentLibraryHost + "/materials/" + materialCode + "?dataType=true";

                var tokenResponse = client.GetAsync(url).Result;

                var onjs = tokenResponse.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(onjs.Result);
                List<POServiceAnnexureDTO> list = new List<POServiceAnnexureDTO>();
                JToken type = jo.SelectToken("headers");
                int v = type.Count();
                POServiceAnnexureDTO service = new POServiceAnnexureDTO();
                foreach (JToken headertype in type)
                {
                    if (headertype["name"].ToString() == "Specifications")
                    {
                        foreach (JToken jack in headertype["columns"])
                        {
                            if (jack["key"].ToString() == "specification_sheet")
                            {
                                if (jack["value"].ToString() != null)
                                {
                                    foreach (JProperty im in jack["value"])
                                    {
                                        if (im.Name == "id")
                                        {
                                            CheckListId = im.Value.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //if (tokenResponse.StatusCode.Equals(201))
                //{
                //    //if (log.IsDebugEnabled)
                //    //    log.Debug("Successful");
                //}
                //else
                //{
                //    //if (log.IsDebugEnabled)
                //    //    log.Debug("Unable to Get");
                //}
                //return new HttpResponseMessage()
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Content = new StringContent(mesg)
                //};                
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);

            }
            return CheckListId;
        }

        [HttpPost]
        public HttpResponseMessage GetAllPurchaseOrdersInformation(JObject json)
        {
            try
            {
                var poDetailsList = (from phs in db.TEPOHeaderStructures
                                     join manager in db.UserProfiles on phs.POManagerID equals manager.UserId into tempmgr
                                     from mnger in tempmgr.DefaultIfEmpty()
                                         //join pfc  in db.TEPOFundCenters on phs.FundCenterID equals pfc.Uniqueid
                                         //join pv in db.TEPOVendors on phs.VendorID equals pv.Uniqueid
                                         //join vendorshipFrom in db.TEPOVendorShippingDetails on phs.ShippedFromID equals vendorshipFrom.VendorShippingID
                                         //join vendorbilledBy in db.TEPOVendors on phs.BilledByID equals vendorbilledBy.Uniqueid
                                         //join billedto in db.TEPOPlantStorageDetails on phs.BilledToID equals billedto.PlantStorageDetailsID
                                         //join shippedto in db.TEPOPlantStorageDetails on phs.ShippedToID equals shippedto.PlantStorageDetailsID
                                         //join cmp in db.TECompanies on phs.Company_Code equals cmp.CompanyCode
                                         //join order in db.TEPurchase_OrderTypes on phs.PO_OrderTypeID equals order.UniqueId into temporder
                                         //from finalorder in temporder.DefaultIfEmpty()
                                         //join proj in db.TEProjects on phs.ProjectID equals proj.ProjectID
                                     where phs.IsDeleted == false
                                     select new
                                     {
                                         phs.Agreement_signed_date,
                                         phs.ApprovedBy,
                                         phs.PODescription,
                                         phs.PO_Title,
                                         phs.PO_OrderTypeID,
                                         //PO_OrdertypeDescription = finalorder.Description,
                                         //PO_OrderTypeCode = finalorder.Code,
                                         phs.ApprovedOn,
                                         phs.Company_Code,
                                         phs.Exchange_Rate,
                                         phs.Fugue_Purchasing_Order_Number,
                                         phs.FundCenterID,
                                         Managed_by = mnger.CallName,
                                         phs.POManagerID,
                                         phs.Objectid,
                                         phs.path,
                                         phs.Purchasing_Order_Number,
                                         phs.Purchasing_Organization,
                                         phs.Uniqueid,
                                         phs.VendorID,
                                         phs.Vendor_Account_Number,
                                         phs.Version,
                                         phs.VersionTextField,
                                         //cmp.CompanyCode,
                                         //cmp.Name,
                                         //cmp.StateCode,
                                         //cmp.StateName,
                                         //proj.ProjectID,
                                         //proj.ProjectCode,
                                         //proj.ProjectName,
                                         //proj.ProjectShortName,
                                         //FundcenterData = pfc,
                                         //VendorData = pv,
                                         //ShipFromData = vendorshipFrom,
                                         //ShipToData = shippedto,
                                         phs.BilledToID,
                                         //BilledToData = billedto,
                                         //VendorBilledByData = vendorbilledBy,
                                         phs.BilledByID
                                     }).ToList();
                int count = poDetailsList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.totalrecords = count;
                    sinfo.torecords = 10;
                    sinfo.pages = "1";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = poDetailsList }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo, result = poDetailsList }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetPOAllSPecificTermsandCoditions(JObject json)
        {
            int count = 0;
            List<SPecificMainTermsandCoditions> spcMainTCList = new List<SPecificMainTermsandCoditions>();
            List<SPecificTermsandCoditionsDTO> spcTCList = new List<SPecificTermsandCoditionsDTO>();
            try
            {
                int pheadstructId = json["POHeaderStructureID"].ToObject<int>();

                spcMainTCList = (from itm in db.TEPOItemStructures
                                 where itm.POStructureId == pheadstructId && itm.IsDeleted == false
                                 select new SPecificMainTermsandCoditions
                                 {
                                     POItemStructureID = itm.Uniqueid,
                                     POHeaderStructureID = itm.POStructureId,
                                     MaterialCode = itm.Material_Number,
                                     MaterialName = itm.Short_Text,
                                     MaterialDescription = itm.Long_Text,
                                 }).Distinct().ToList();

                foreach (var mainTC in spcMainTCList)
                {
                    spcTCList = (from mstc in db.TEPOSpecificTandCMasters
                                 where mstc.PO_ItemStructureID == mainTC.POItemStructureID && mstc.PO_HeaderStructureID == mainTC.POHeaderStructureID && mstc.IsDeleted == false
                                 select new SPecificTermsandCoditionsDTO
                                 {
                                     SpecTCDescription = mstc.Description,
                                     CheckListId = mstc.CheckListId,
                                     SpecTCCmpLibraryID = mstc.ID,
                                     SpecificTCMasterId = mstc.POSpecificTCUniqueId,
                                     Template = mstc.Template,
                                     SpecTCTitle = mstc.Title,
                                 }).ToList();
                    mainTC.SPecificTCMaster = spcTCList;
                    foreach (var tcmasterdata in spcTCList)
                    {
                        tcmasterdata.PO_SpecificTCDetails = db.TEPOSpecificTCDetails.Where(a => a.IsDeleted == false && a.SpecificTCSubTitleMasterId == tcmasterdata.SpecificTCMasterId).ToList();
                    }
                }
                count = spcMainTCList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = spcMainTCList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = spcMainTCList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = spcMainTCList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = spcMainTCList, info = sinfo }) };
            }
        }

        [HttpPut]
        public HttpResponseMessage TEPurchase_UpdateMaterialLastPurchaseRate()
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                UpdateMaterialRateRequest materialRateReq = new UpdateMaterialRateRequest();
                materialRateReq.MaterialCode = "MSY000008";
                MaterialPurchaseDetail purchdtl = new MaterialPurchaseDetail();
                purchdtl.Currency = "INR";
                purchdtl.Amount = "1234";
                materialRateReq.LastPurchaseRate = purchdtl;
                MaterialPurchaseDetail weighdtl = new MaterialPurchaseDetail();
                weighdtl.Currency = "INR";
                weighdtl.Amount = "1234";
                materialRateReq.WeightedAveragePurchaseRate = weighdtl;
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var url = ComponentLibraryHost + "/updatematerialrate";

                var response = client.PutAsJsonAsync(url, materialRateReq).Result;

                if (response.StatusCode.Equals(200))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = response

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        [HttpPut]
        public HttpResponseMessage TEPurchase_UpdateServiceLastPurchaseRate(UpdateServiceRateRequest serviceRateReq, string serviceCode)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var url = ComponentLibraryHost + "/services/" + serviceCode + "/rates";

                var response = client.PutAsJsonAsync(url, serviceRateReq).Result;

                if (response.StatusCode.Equals(200))
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Successful");
                }
                else
                {
                    //if (log.IsDebugEnabled)
                    //    log.Debug("Unable to Get");
                }
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = response

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        public void UpdateMaterialLastPurchaseRateToCL(UpdateMaterialRateRequest materialRateReq)
        {
            Token tokenObj = generateToken_ComponentLibrary();
            string token = tokenObj.AccessToken;
            HttpClient client = new HttpClient();
            string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
            string tenant = WebConfigurationManager.AppSettings["tenant"];

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var url = ComponentLibraryHost + "/updatematerialrate";

            var response = client.PutAsJsonAsync(url, materialRateReq).Result;

            if (response.StatusCode.Equals(200))
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Successful");
            }
            else
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Unable to Get");
            }
        }
        public void UpdateServiceLastPurchaseRateToCL(UpdateServiceRateRequest serviceRateReq, string serviceCode)
        {
            Token tokenObj = generateToken_ComponentLibrary();
            string token = tokenObj.AccessToken;
            HttpClient client = new HttpClient();
            string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
            string tenant = WebConfigurationManager.AppSettings["tenant"];

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var url = ComponentLibraryHost + "/services/" + serviceCode + "/rates";

            var response = client.PutAsJsonAsync(url, serviceRateReq).Result;

            if (response.StatusCode.Equals(200))
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Successful");
            }
            else
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Unable to Get");
            }
        }
        [HttpPost]
        public HttpResponseMessage TEPurchase_GetMaterialRate(SearchMaterialRate materialRateReq)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                //materialRateReq.materialId = "MFR015140";
                //materialRateReq.location = "Hyderabad";
                //string currentDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                MaterailBaseRateInfo myList = new MaterailBaseRateInfo();
                myList = GetMaterialRateFromCL(materialRateReq);

                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = myList

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        public MaterailBaseRateInfo GetMaterialRateFromCL(SearchMaterialRate materialRateReq)
        {

            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            Token tokenObj = generateToken_ComponentLibrary();
            string token = tokenObj.AccessToken;
            HttpClient client = new HttpClient();
            string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
            string tenant = WebConfigurationManager.AppSettings["tenant"];
            string typeOfPurchase = "Domestic"; string NatureofTransaction = "Intra-state";
            typeOfPurchase = materialRateReq.TransactionType;
            NatureofTransaction = materialRateReq.NatureofTransaction;


            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var url = ComponentLibraryHost + "/material-landed-rates?materialId=" + materialRateReq.materialId
                + "&location=" + materialRateReq.location
                + "&currency=" + materialRateReq.currency
                + "&onDate=" + currentDate + "T00%3A00%3A00%2B5%3A30"
                + "&typeOfPurchase=" + typeOfPurchase + " " + NatureofTransaction;


            var response = client.GetAsync(url).Result;

            if (response.StatusCode.Equals(200))
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Successful");
            }
            else
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Unable to Get");
            }
            var onjs = response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            MaterailBaseRateInfo myList = new MaterailBaseRateInfo();
            myList = JsonConvert.DeserializeObject<MaterailBaseRateInfo>(onjs.Result, settings);
            return myList;
        }
        public FinalItemBaseRate GetFinalBaseRateInfo(string materialCode, string location, string orderType, string currency, string TransactionType, string NatureofTransaction)
        {
            decimal controlBaserate = 0; decimal thresholdRate = 0;
            SearchMaterialRate matrlRate = new SearchMaterialRate();
            MaterailBaseRateInfo baserateInfo = null;
            matrlRate.materialId = materialCode;
            matrlRate.location = location;
            matrlRate.currency = currency;
            matrlRate.TransactionType = TransactionType;
            matrlRate.NatureofTransaction = NatureofTransaction;
            if (orderType == "MaterialOrder")
                baserateInfo = GetMaterialRateFromCL(matrlRate);
            else if (orderType == "ServiceOrder")
                baserateInfo = GetServiceRateFromCL(matrlRate);

            if (baserateInfo != null)
            {
                string cntrlBaserate = string.Empty;
                if (baserateInfo.controlBaseRate != null)
                {
                    if (!string.IsNullOrEmpty(baserateInfo.controlBaseRate.value))
                    {
                        controlBaserate = string.IsNullOrEmpty(baserateInfo.controlBaseRate.value) ? 0 : Convert.ToDecimal(baserateInfo.controlBaseRate.value);
                        thresholdRate = string.IsNullOrEmpty(baserateInfo.procurement_rate_threshold) ? 0 : Convert.ToDecimal(baserateInfo.procurement_rate_threshold);
                    }
                }
            }
            FinalItemBaseRate baseRate = new FinalItemBaseRate();
            baseRate.ControlBaserate = controlBaserate;
            baseRate.ThresholdValue = thresholdRate;
            return baseRate;
        }
        [HttpPost]
        public HttpResponseMessage TEPurchase_GetServiceRate(SearchMaterialRate serviceRateReq)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                //serviceRateReq.materialId = "MFR015140";
                //serviceRateReq.location = "Hyderabad";
                //string currentDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
                MaterailBaseRateInfo myList = new MaterailBaseRateInfo();
                myList = GetServiceRateFromCL(serviceRateReq);
                hrm.Content = new JsonContent(new
                {
                    StatusCode = HttpStatusCode.OK,
                    result = myList

                });
                return hrm;
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(e.Message)
                };
            }


        }
        public MaterailBaseRateInfo GetServiceRateFromCL(SearchMaterialRate serviceRateReq)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            Token tokenObj = generateToken_ComponentLibrary();
            string token = tokenObj.AccessToken;
            HttpClient client = new HttpClient();
            string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
            string tenant = WebConfigurationManager.AppSettings["tenant"];
            string typeOfPurchase = "Domestic"; string NatureofTransaction = "Intra-state";
            typeOfPurchase = serviceRateReq.TransactionType;
            NatureofTransaction = serviceRateReq.NatureofTransaction;

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var url = ComponentLibraryHost + "/service-landed-rates?serviceId=" + serviceRateReq.materialId
                + "&location=" + serviceRateReq.location
                + "&currency=" + serviceRateReq.currency
                + "&onDate=" + currentDate + "T00%3A00%3A00%2B5%3A30"
                + "&typeOfPurchase=" + typeOfPurchase + " " + NatureofTransaction;
            var response = client.GetAsync(url).Result;

            if (response.StatusCode.Equals(200))
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Successful");
            }
            else
            {
                //if (log.IsDebugEnabled)
                //    log.Debug("Unable to Get");
            }
            var onjs = response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            MaterailBaseRateInfo myList = new MaterailBaseRateInfo();
            myList = JsonConvert.DeserializeObject<MaterailBaseRateInfo>(onjs.Result, settings);
            return myList;
        }
        [HttpPost]
        public HttpResponseMessage GetPOAnnexureSPecifications(JObject json)
        {
            int count = 0;
            List<TEPOSpecificatonAnnexureDTO> annexSpcList = new List<TEPOSpecificatonAnnexureDTO>();
            try
            {
                int pheadstructId = json["POHeaderStructureID"].ToObject<int>();

                annexSpcList = (from itm in db.TEPOItemStructures
                                join anex in db.TEPOSpecificationAnnexures on itm.Uniqueid equals anex.POItemStructureId
                                where itm.POStructureId == pheadstructId && itm.IsDeleted == false
                                select new TEPOSpecificatonAnnexureDTO
                                {
                                    POHeaderStructureId = pheadstructId,
                                    POItemStructureId = itm.Uniqueid,
                                    MaterialCode = itm.Material_Number,
                                    MaterialName = itm.Short_Text,
                                }).Distinct().ToList();
                foreach (var data in annexSpcList)
                {
                    data.SPecsData = db.TEPOSpecificationAnnexures.Where(anex => anex.POHeaderStructureId == pheadstructId && anex.POItemStructureId == data.POItemStructureId && anex.IsDeleted == false).ToList();
                    if (data.SPecsData.Count > 0) { data.MaterialGroup = data.SPecsData[0].MaterialGroup; }
                }
                count = annexSpcList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.listcount = annexSpcList.Count;
                    sinfo.totalrecords = annexSpcList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetPOAnnexureSpecificationsByHeaderStructureId(JObject json)
        {
            int count = 0;
            List<POAnnexureSpecificatonDTO> annexSpcList = new List<POAnnexureSpecificatonDTO>();
            try
            {
                int pheadstructId = json["POHeaderStructureID"].ToObject<int>();
                annexSpcList = new PurchaseOrderBAL().GetPOAnnexureSpecificationsByHeaderStructureId(pheadstructId);
                count = annexSpcList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.listcount = annexSpcList.Count;
                    sinfo.totalrecords = annexSpcList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage GetPOServiceAnnexureByHeaderStructureId(JObject json)
        {
            int count = 0;
            List<POServiceAnnexureSpecificatonDTO> annexSpcList = new List<POServiceAnnexureSpecificatonDTO>();
            try
            {
                int pheadstructId = json["POHeaderStructureID"].ToObject<int>();
                annexSpcList = new PurchaseOrderBAL().GetPOServiceAnnexureByHeaderStructureId(pheadstructId);
                count = annexSpcList.Count;
                if (count > 0)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Got Records";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.listcount = annexSpcList.Count;
                    sinfo.totalrecords = annexSpcList.Count;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.fromrecords = 0;
                    sinfo.torecords = 0;
                    sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to get Records";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = annexSpcList, info = sinfo }) };
            }
        }

        public void TEPurchase_SaveMaterialSpecifications(string materialcode, int headStructId, int itemStructId, int lastmodifiedby)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                List<columnsStruct> specColumnsList = new List<columnsStruct>();
                ObjFromComponentLib myList = new ObjFromComponentLib();
                //ObjFromComponentLib PO_SpecialTC = new ObjFromComponentLib();

                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                string tenant = WebConfigurationManager.AppSettings["tenant"];

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // https://localhost:44380/materials/MFR023128?dataType=true
                var url = ComponentLibraryHost + "/materials/" + materialcode + "?dataType=true";

                var tokenResponse = client.GetAsync(url).Result;

                var onjs = tokenResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                myList = JsonConvert.DeserializeObject<ObjFromComponentLib>(onjs.Result, settings);
                // PO_SpecialTC = JsonConvert.DeserializeObject<ObjFromComponentLib>(onjs.Result, settings);

                if (myList != null)
                {
                    try
                    {
                        specColumnsList = myList.headers.Find(a => a.key.Equals("specifications")).columns.ToList();
                        //var specific_termsdata = myList.headers.Find(a => a.key.Equals("purchase")).columns.Where(a => a.key == "special_po_terms" && a.dataType.name == "CheckList").FirstOrDefault();
                        //if (specific_termsdata != null)
                        //{
                        //    if (specific_termsdata.value != null)
                        //    {
                        //        SpecifiTCValueClass valObj = new SpecifiTCValueClass();
                        //        valObj =(SpecifiTCValueClass) specific_termsdata.value;
                        //        if (valObj.id != null)
                        //        {
                        //            savePOSpecificTermCoditions(token, ComponentLibraryHost, valObj.id, headStructId, itemStructId, lastmodifiedby);
                        //        }
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        ExceptionObj.RecordUnHandledException(ex);
                    }

                    foreach (var annex in specColumnsList)
                    {
                        TEPOSpecificationAnnexure Spc_annex = new TEPOSpecificationAnnexure();
                        Spc_annex.Name = annex.name;
                        Spc_annex.MaterialGroup = myList.group;
                        Spc_annex.CmpLibId = myList.id;
                        Spc_annex.MaterialCode = materialcode;
                        Spc_annex.POHeaderStructureId = headStructId;
                        Spc_annex.POItemStructureId = itemStructId;
                        Spc_annex.IsDeleted = false;
                        if (annex.dataType.subType != null)
                        {
                            SpecAnnexValue valObj = new SpecAnnexValue();
                            if (annex.value != null)
                            {
                                try
                                {
                                    valObj = JsonConvert.DeserializeObject<SpecAnnexValue>(annex.value.ToString());
                                    Spc_annex.Value = valObj.value;
                                    Spc_annex.ValueType = valObj.type;
                                }
                                catch (Exception ex)
                                {
                                    ExceptionObj.RecordUnHandledException(ex);

                                }
                            }
                        }
                        else
                        {
                            if (annex.value != null)
                                Spc_annex.Value = annex.value.ToString();
                        }
                        Spc_annex.LastModifiedBy = lastmodifiedby;
                        Spc_annex.LastModifiedOn = DateTime.Now;
                        db.TEPOSpecificationAnnexures.Add(Spc_annex);
                        db.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);

            }
        }

        public void savePOSpecificTermCoditions(string token, string ComponentLibraryHost, string checklistId, int headStructId, int itemStructId, int lastmodifiedby)
        {
            try
            {
                List<columnsStruct> specColumnsList = new List<columnsStruct>();
                List<columnsStruct> classificationList = new List<columnsStruct>();
                MtlSpecTandC myList = new MtlSpecTandC();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // var url = "https://clapi.total-environment.com/check-lists/MCCP0008";
                var url = ComponentLibraryHost + "/check-lists/" + checklistId;

                var tokenResponse = client.GetAsync(url).Result;
                var onjs = tokenResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                myList = JsonConvert.DeserializeObject<MtlSpecTandC>(onjs.Result, settings);
                if (myList != null)
                {
                    int tempspcmasterUniqueId = 0;
                    TEPOSpecificTandCMaster Po_spcmaster = new TEPOSpecificTandCMaster();
                    Po_spcmaster.CheckListId = myList.checkListId;
                    Po_spcmaster.Title = myList.title;
                    Po_spcmaster.Template = myList.template;
                    Po_spcmaster.PO_HeaderStructureID = headStructId;
                    Po_spcmaster.PO_ItemStructureID = itemStructId;
                    Po_spcmaster.ID = myList.id;
                    Po_spcmaster.LastModifiedBy = lastmodifiedby;
                    Po_spcmaster.IsDeleted = false;
                    Po_spcmaster.LastModifiedOn = DateTime.Now;
                    int cnt = 0;
                    foreach (var data in myList.content.entries)
                    {
                        if (cnt > 0)
                        {
                            if (!string.IsNullOrEmpty(data.cells[0].value))
                            {
                                Po_spcmaster.Description = data.cells[1].value;
                                db.TEPOSpecificTandCMasters.Add(Po_spcmaster);

                                db.SaveChanges();
                                tempspcmasterUniqueId = Po_spcmaster.POSpecificTCUniqueId;
                            }
                            else if (tempspcmasterUniqueId != 0)
                            {
                                TEPOSpecificTCDetail spcTCDetails = new TEPOSpecificTCDetail();
                                //spcTCDetails.LastModifiedBy = lastmodifiedby;
                                //spcTCDetails.SpecificTermCondition = data.cells[2].value;
                                //spcTCDetails.SpecificTCMasterId = tempspcmasterUniqueId;
                                //spcTCDetails.IsDeleted = false;
                                //spcTCDetails.LastModifiedOn = DateTime.Now;
                                //db.TEPOSpecificTCDetails.Add(spcTCDetails);

                                db.SaveChanges();
                            }
                            else { }
                        }
                        cnt++;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
            }
        }

        [HttpPost]
        public HttpResponseMessage MaterialSpecsWithCheckListId(JObject json)
        {
            try
            {
                string checkListId = json["CheckListId"].ToObject<string>();
                MtlSpecTandC myList = new MtlSpecTandC();
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                HttpClient client = new HttpClient();
                List<object> list = new List<object>();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // var url = "https://clapi.total-environment.com/check-lists/MCCP0008";
                var url = ComponentLibraryHost + "/check-lists/" + checkListId;

                var tokenResponse = client.GetAsync(url).Result;
                var onjs = tokenResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                myList = JsonConvert.DeserializeObject<MtlSpecTandC>(onjs.Result, settings);

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new
                    {
                        result = myList
                    })
                };
            }
            catch (Exception e)
            {
                ExceptionObj.RecordUnHandledException(e);
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new JsonContent(new
                    {
                        res = ""

                    })
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage ItemViewInfoByUniqueId(JObject json)
        {
            CmpLibraryItemDTO ItemData = new CmpLibraryItemDTO();
            try
            {
                string ItemType = json["ItemType"].ToObject<string>();
                string ItemCode = json["ItemCode"].ToObject<string>();
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;
                HttpClient client = new HttpClient();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                string url = string.Empty;
                if (ItemType == "MaterialOrder") { url = ComponentLibraryHost + "/materials/" + ItemCode; }
                else if (ItemType == "ServiceOrder") { url = ComponentLibraryHost + "/services/" + ItemCode; }
                var tokenResponse = client.GetAsync(url).Result;
                var onjs = tokenResponse.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(onjs.Result);
                JToken type = jo.SelectToken("headers");
                foreach (JToken jad in type)
                {
                    if (jad["key"].ToString() == "classification")
                    {
                        ItemData.Mtl_Classific_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "general")
                    {
                        ItemData.Mtl_General_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "purchase")
                    {
                        ItemData.Mtl_Purchase_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "planning")
                    {
                        ItemData.Mtl_Planning_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "quality")
                    {
                        ItemData.Mtl_Quality_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "specifications")
                    {
                        ItemData.Mtl_Specs_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "system_logs")
                    {
                        ItemData.Mtl_Log_Info = jad["columns"];
                    }
                    else if (jad["key"].ToString() == "classification_definition")
                    {
                        ItemData.Mtl_Definition_Info = jad["columns"];
                    }
                }
                sinfo.errorcode = 1;
                sinfo.fromrecords = 0;
                sinfo.torecords = 0;
                sinfo.totalrecords = 1;
                sinfo.errormessage = "Success";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ItemData, info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.fromrecords = 0;
                sinfo.torecords = 0;
                sinfo.errormessage = "Failed To get Data";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ItemData, info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePO_SpecificTandCMaster(TEPOSpecificTandCMaster specTC)
        {
            try
            {
                TEPOSpecificTandCMaster po_spcTC_Obj = db.TEPOSpecificTandCMasters.Where(a => a.POSpecificTCUniqueId == specTC.POSpecificTCUniqueId && a.IsDeleted == false).FirstOrDefault();
                if (po_spcTC_Obj != null)
                {
                    po_spcTC_Obj.LastModifiedBy = specTC.LastModifiedBy;
                    po_spcTC_Obj.LastModifiedOn = DateTime.Now;
                    po_spcTC_Obj.IsDeleted = true;
                    db.Entry(po_spcTC_Obj).CurrentValues.SetValues(po_spcTC_Obj);
                    db.SaveChanges();

                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Successfully Deleted";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Terms and Coditions Details not Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage GetFundManagerInfoByUniqueId(JObject json)
        {
            try
            {
                int fundcenterId = json["Uniqueid"].ToObject<int>();
                var managerdata = (from apr in db.POApprovalConditions
                                   join user in db.UserProfiles on apr.POManagerID equals user.UserId
                                   where apr.FundCenter == fundcenterId && apr.IsDeleted == false
                                   select new
                                   {
                                       ManagerName = user.CallName,
                                       POManagerID = user.UserId
                                   }).FirstOrDefault();
                if (managerdata != null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Success";
                    sinfo.fromrecords = 1;
                    sinfo.torecords = 1;
                    sinfo.totalrecords = 1;
                    sinfo.listcount = 1;
                    sinfo.pages = "1";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = managerdata, info = sinfo }) };
                }
                else
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "No Records";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = managerdata, info = sinfo }) };
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail";
                sinfo.listcount = 0;
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


        public void SaveAnnexureSpecificationSheet(string checkListId, int headStructId, int itemStructId, int lastmodifiedby)
        {
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                HttpClient client = new HttpClient();
                List<object> list = new List<object>();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // var url = "https://clapi.total-environment.com/check-lists/MCCC0026";
                var url = ComponentLibraryHost + "/check-lists/" + checkListId;

                var tokenResponse = client.GetAsync(url).Result;
                var onjs = tokenResponse.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                SpecificationSheetMain myList = new SpecificationSheetMain();
                myList = JsonConvert.DeserializeObject<SpecificationSheetMain>(onjs.Result, settings);
                if (myList != null)
                {
                    TEPOAnnexure annexure = new TEPOAnnexure();
                    annexure.CheckListId = myList.checkListId;
                    annexure.Title = myList.title;
                    annexure.Template = myList.template;
                    annexure.PO_HeaderStructureID = headStructId;
                    annexure.PO_ItemStructureID = itemStructId;
                    annexure.LastModifiedBy = lastmodifiedby;
                    //annexure.PO_HeaderStructureID = 130;
                    //annexure.PO_ItemStructureID = 211;
                    //annexure.LastModifiedBy = 251;
                    annexure.IsDeleted = false;
                    annexure.LastModifiedOn = DateTime.Now;
                    db.TEPOAnnexures.Add(annexure);
                    db.SaveChanges();

                    List<TEPOAnnexureSpecification> specSheetList = new List<TEPOAnnexureSpecification>();
                    int cnt = 0;
                    foreach (var data in myList.content.entries)
                    {
                        if (cnt > 0)
                        {
                            TEPOAnnexureSpecification specSheet = new TEPOAnnexureSpecification();
                            specSheet.TestObject = data.cells[0].value;
                            specSheet.TestMethod = data.cells[1].value;
                            specSheet.Value1 = data.cells[2].value;
                            specSheet.Value2 = data.cells[3].value;
                            specSheet.UnitOfMeasurement = data.cells[4].value;
                            specSheet.Mandatory = data.cells[5].value;
                            specSheet.AcceptanceCriteria = data.cells[6].value;
                            specSheet.MTCValue = data.cells[7].value;
                            specSheet.TestValue = data.cells[8].value;
                            specSheet.SourceOfTestValue = data.cells[9].value;
                            specSheet.PassOrFail = data.cells[10].value;
                            specSheet.IsDeleted = false;
                            specSheet.POAnnexureId = annexure.POAnnexureId;
                            specSheetList.Add(specSheet);
                        }
                        cnt++;
                    }
                    if (specSheetList.Count > 0)
                    {
                        db.TEPOAnnexureSpecifications.AddRange(specSheetList);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
        }

        public void SaveServiceAnnexureSpecificationSheet(string serviceCode, int headStructId, int itemStructId)
        {
            try
            {
                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                HttpClient client = new HttpClient();
                List<object> list = new List<object>();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // var url = "https://clapi.total-environment.com/services/SBP000387";
                var url = ComponentLibraryHost + "/services/" + serviceCode;

                var tokenResponse = client.GetAsync(url).Result;
                POBAL.ParseServiceAnnexureJobject(tokenResponse, headStructId, itemStructId);
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetServiceClasificationDefinition(JObject json)
        {
            object DefinitionResult = new object();
            try
            {
                string serviceCode = json["ServiceCode"].ToObject<string>();

                Token tokenObj = generateToken_ComponentLibrary();
                string token = tokenObj.AccessToken;

                HttpClient client = new HttpClient();
                List<object> list = new List<object>();
                string ComponentLibraryHost = WebConfigurationManager.AppSettings["ComponentLibraryHost"];
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var url = ComponentLibraryHost + "/services/" + serviceCode;
                var tokenResponse = client.GetAsync(url).Result;
                var onjs = tokenResponse.Content.ReadAsStringAsync();
                JObject jo = JObject.Parse(onjs.Result);
                JToken type = jo.SelectToken("headers");

                foreach (JToken jad in type)
                {
                    if (jad["key"].ToString() == "classification_definition")
                    {
                        DefinitionResult = jad["columns"];
                    }
                }
                sinfo.errorcode = 1;
                sinfo.fromrecords = 0;
                sinfo.torecords = 0;
                sinfo.totalrecords = 1;
                sinfo.errormessage = "Success";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = DefinitionResult, info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.fromrecords = 0;
                sinfo.torecords = 0;
                sinfo.errormessage = "Failed To get Data";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = DefinitionResult, info = sinfo }) };
            }

        }

        [HttpPost]
        public HttpResponseMessage InitiatePOFromPR(PRDetails pr)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                var PRObj = db.TEPurchaseRequests.Where(a => a.PurchaseRequestId == pr.PurchaseRequestId && a.Active == true).FirstOrDefault();
                if (PRObj != null)
                {
                    if (PRObj.POStatus == "Rejected")
                    {
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "PO Cannot be Initiated For Rejected PR";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                int resultPOId = InitiatePOFromPRDetails(pr);

                PRObj.POStatus = "Pending Action";
                db.Entry(PRObj).CurrentValues.SetValues(PRObj);
                db.SaveChanges();

                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Initiated. PO FugueId: " + resultPOId;
                return new HttpResponseMessage { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Initiate";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        [HttpPost]
        public HttpResponseMessage POApprovalList(JObject json)
        {
            HttpResponseMessage hrm = new HttpResponseMessage();
            try
            {
                int UniqueID = json["UniqueID"].ToObject<int>();
                var PRObj = db.TEPOApprovers.Where(x => x.POStructureId == UniqueID && x.IsDeleted == false).ToList();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfull";
                return new HttpResponseMessage { Content = new JsonContent(new { result= PRObj, info = sinfo }) };
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Fail to Get List";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo, result = "" }) };
            }
        }

        public decimal GetOrderedQntyForPRIteminPO(int? prId, string materialNo)
        {
            decimal ordQnty = 0;
            var itmList = (from itm in db.TEPOItemStructures
                           join HeadItem in db.TEPOHeaderStructures on itm.POStructureId equals HeadItem.Uniqueid
                           where itm.IsDeleted == false && itm.PRRef == prId
                           && itm.Material_Number == materialNo && HeadItem.Status == "Active"
                           select new
                           {
                               itm.Order_Qty
                           }).ToList();
            if (itmList.Count > 0)
            {
                foreach (var itm in itmList)
                {
                    if (!string.IsNullOrEmpty(itm.Order_Qty))
                    {
                        decimal orderQnty = 0;
                        orderQnty = Convert.ToDecimal(itm.Order_Qty);
                        ordQnty += orderQnty;
                    }
                }
            }

            return ordQnty;
        }

        public decimal GetOrderedQntyForPRIteminPO(int? prId, string materialNo, int POStructureID)
        {
            decimal ordQnty = 0;
            var itmList = (from itm in db.TEPOItemStructures
                           join Headitem in db.TEPOHeaderStructures on itm.POStructureId equals Headitem.Uniqueid
                           where itm.IsDeleted == false && itm.PRRef == prId
                           && Headitem.Status == "Active"
                           && itm.Material_Number == materialNo && itm.Uniqueid != POStructureID
                           select new
                           {
                               itm.Order_Qty
                           }).ToList();
            if (itmList.Count > 0)
            {
                foreach (var itm in itmList)
                {
                    if (!string.IsNullOrEmpty(itm.Order_Qty))
                    {
                        decimal orderQnty = 0;
                        orderQnty = Convert.ToDecimal(itm.Order_Qty);
                        ordQnty += orderQnty;
                    }
                }
            }

            return ordQnty;
        }

        public void UpdatePRitemBalanceQnty(int? prId, string materialNo)
        {
            decimal orderQnty = 0;
            orderQnty = GetOrderedQntyForPRIteminPO(prId, materialNo);
            if (orderQnty > 0)
            {
                var prItem = db.TEPRItemStructures.Where(a => a.PurchaseRequestId == prId
                && a.Material_Number == materialNo && a.IsDeleted == false).FirstOrDefault();
                if (prItem != null)
                {
                    decimal prItmQnty = 0, poItmQnty = 0;
                    if (!string.IsNullOrEmpty(prItem.Order_Qty))
                        prItmQnty = Convert.ToDecimal(prItem.Order_Qty);
                    poItmQnty = orderQnty;
                    prItem.Balance_Qty = prItmQnty - poItmQnty;
                    if ((prItmQnty - poItmQnty) < 0)
                    {
                        prItem.Balance_Qty = 0;
                    }
                    db.Entry(prItem).CurrentValues.SetValues(prItem);
                    db.SaveChanges();
                }
            }
        }

        public bool IsPOItemQntyCrossedPRItemQnty(int? prId, string materialNo, int POStructureID, Decimal upateOrder_Qty)
        {
            bool IsCrossed = false;
            decimal orderQnty = 0;
            orderQnty = GetOrderedQntyForPRIteminPO(prId, materialNo, POStructureID) + upateOrder_Qty;
            if (orderQnty > 0)
            {
                //var prItem = db.TEPRItemStructures.Where(a => a.PurchaseRequestId == prId
                //&& a.Material_Number == materialNo && a.IsDeleted == false).FirstOrDefault();

                var prItem = db.TEPRItemStructures.Where(a => a.PurchaseRequestId == prId
                && a.Material_Number == materialNo && a.IsDeleted == false).ToList();

                if (prItem != null && prItem.Count > 0)
                {

                    decimal PRCnt = 0;
                    for (int i = 0; i < prItem.Count; i++)
                    {
                        PRCnt += Convert.ToDecimal(prItem[i].Order_Qty);
                    }
                    decimal prItmQnty = 0, poItmQnty = 0;
                    if (PRCnt > 0) prItmQnty = Convert.ToDecimal(PRCnt);
                    poItmQnty = orderQnty;
                    if (poItmQnty > prItmQnty)
                    {
                        IsCrossed = true;
                    }
                }
            }
            return IsCrossed;
        }

        public bool IsPOItemQntyCrossedPRItemQnty(int? prId, string materialNo, Decimal upateOrder_Qty)
        {
            bool IsCrossed = false;
            decimal orderQnty = 0;
            orderQnty = GetOrderedQntyForPRIteminPO(prId, materialNo) + upateOrder_Qty;
            if (orderQnty > 0)
            {
                var prItem = db.TEPRItemStructures.Where(a => a.PurchaseRequestId == prId
                && a.Material_Number == materialNo && a.IsDeleted == false).FirstOrDefault();
                if (prItem != null)
                {
                    decimal prItmQnty = 0, poItmQnty = 0;
                    if (!string.IsNullOrEmpty(prItem.Order_Qty))
                        prItmQnty = Convert.ToDecimal(prItem.Order_Qty);
                    poItmQnty = orderQnty;
                    //if (poItmQnty > prItmQnty)
                    //{
                    //    IsCrossed = true;
                    //}
                    if (poItmQnty > prItmQnty)
                    {
                        IsCrossed = true;
                    }
                }
            }
            return IsCrossed;
        }

        public int InitiatePOFromPRDetails_Old(PRDetails pr)
        {
            int resultphsUniqueid = 0, loginId = 0; string PlantStorageCode = string.Empty;
            if (pr != null)
            {
                loginId = GetLogInUserId();
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(loginId);
                var projectsList = (from proj in db.TEProjects
                                    join cmp in db.TECompanies on proj.CompanyID equals cmp.Uniqueid
                                    join fund in db.TEPOFundCenters on proj.ProjectCode equals fund.ProjectCode
                                    where fund.IsDeleted == false && cmp.IsDeleted == false && proj.IsDeleted == false && fund.Uniqueid == pr.FundCenterId
                                    select new { proj.ProjectCode, proj.ProjectName, proj.ProjectShortName, proj.CompanyID, proj.ProjectID, cmp.Name, cmp.CompanyCode, cmp.StateCode, cmp.StateName }).FirstOrDefault();
                var billingData = db.TEPOPlantStorageDetails.Where(d => d.ProjectCode == projectsList.ProjectCode && d.Type == "billing" && d.isdeleted == false).FirstOrDefault();
                TEPOHeaderStructure hdrStructure = new TEPOHeaderStructure();
                hdrStructure.FundCenterID = pr.FundCenterId;
                hdrStructure.PO_Title = pr.PurchaseRequestTitle;
                hdrStructure.PODescription = pr.PurchaseRequestDesc;
                hdrStructure.ProjectID = projectsList.ProjectID;
                hdrStructure.Company_Code = projectsList.CompanyCode;
                hdrStructure.BilledToID = billingData.PlantStorageDetailsID;
                hdrStructure.CreatedBy = loginId;
                hdrStructure.PurchaseRequestId = pr.PurchaseRequestId;
                hdrStructure.SubmitterName = loginuser;
                hdrStructure.IsPRPO = true;
                resultphsUniqueid = new PurchaseOrderBAL().SavePO(hdrStructure, loginId);
                if (resultphsUniqueid > 0)
                {
                    var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == hdrStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                    PlantStorageCode = plantStorage.PlantStorageCode;
                    if (pr.PurchaseItemList.Count > 0)
                    {
                        Dictionary<int, int> HeadVal = new Dictionary<int, int>();
                        foreach (var itemStructure in pr.PurchaseItemList)
                        {
                            TEPOItemStructure itms = new TEPOItemStructure();
                            itms.PRRef = pr.PurchaseRequestId;
                            itms.POStructureId = hdrStructure.Uniqueid;
                            itms.ServiceHeaderId = itemStructure.serviceHeaderid;
                            itms.Material_Number = itemStructure.Material_Number;
                            itms.Short_Text = itemStructure.Short_Text;
                            itms.Long_Text = itemStructure.Long_Text;
                            itms.ItemType = itemStructure.ItemType;
                            itms.HSNCode = itemStructure.HSN_Code;
                            itms.Brand = itemStructure.Brand;
                            itms.Unit_Measure = itemStructure.Unit_Measure;
                            itms.Order_Qty = itemStructure.Order_Qty;
                            itms.WBSElementCode = itemStructure.WBSElementCode;
                            itms.CreatedOn = System.DateTime.Now.ToString();
                            itms.CreatedBy = loginuser;
                            itms.LastModifiedBy = loginuser;
                            itms.LastModifiedOn = System.DateTime.Now.ToString();
                            itms.IsDeleted = false;

                            if (itms.ItemType == "MaterialOrder")
                            {
                                if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                {
                                    itms.GLAccountNo = "110600";
                                }
                                else
                                {
                                    itms.GLAccountNo = "110950";
                                }
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                itms.GLAccountNo = "515200";

                              
                            }
                            if (itms.ItemType == "ExpenseOrder")
                            {
                                itms.GLAccountNo = "529900";
                            }
                            db.TEPOItemStructures.Add(itms);
                            db.SaveChanges();

                            string checkListId = string.Empty;
                            if (itms.ItemType == "MaterialOrder")
                            {
                                checkListId = GetMaterialSpecificationChecklistId(itms.Material_Number);

                                if (!string.IsNullOrEmpty(checkListId))
                                {
                                    SaveAnnexureSpecificationSheet(checkListId, hdrStructure.Uniqueid, itms.Uniqueid, loginId);
                                }
                                TEPurchase_SaveMaterialSpecifications(itms.Material_Number, hdrStructure.Uniqueid, itms.Uniqueid, loginId);
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                if (!string.IsNullOrEmpty(checkListId))
                                {
                                    SaveServiceAnnexureSpecificationSheet(itms.Material_Number, hdrStructure.Uniqueid, itms.Uniqueid);
                                }
                               

                            }
                            if (itms.PRRef > 0)
                            {
                                UpdatePRitemBalanceQnty(itms.PRRef, itms.Material_Number);
                            }
                        }
                    }
                }
            }
            return resultphsUniqueid;
        }

        public int InitiatePOFromPRDetails(PRDetails pr)
        {
            int resultphsUniqueid = 0, loginId = 0; string PlantStorageCode = string.Empty;
            if (pr != null)
            {
                loginId = GetLogInUserId();
                string loginuser = new PurchaseOrderBAL().getloginUsernamebyId(loginId);
                var projectsList = (from proj in db.TEProjects
                                    join cmp in db.TECompanies on proj.CompanyID equals cmp.Uniqueid
                                    join fund in db.TEPOFundCenters on proj.ProjectCode equals fund.ProjectCode
                                    where fund.IsDeleted == false && cmp.IsDeleted == false && proj.IsDeleted == false && fund.Uniqueid == pr.FundCenterId
                                    select new { proj.ProjectCode, proj.ProjectName, proj.ProjectShortName, proj.CompanyID, proj.ProjectID, cmp.Name, cmp.CompanyCode, cmp.StateCode, cmp.StateName }).FirstOrDefault();
                var billingData = db.TEPOPlantStorageDetails.Where(d => d.ProjectCode == projectsList.ProjectCode && d.Type == "billing" && d.isdeleted == false).FirstOrDefault();
                TEPOHeaderStructure hdrStructure = new TEPOHeaderStructure();
                hdrStructure.FundCenterID = pr.FundCenterId;
                hdrStructure.PO_Title = pr.PurchaseRequestTitle;
                hdrStructure.PODescription = pr.PurchaseRequestDesc;
                hdrStructure.ProjectID = projectsList.ProjectID;
                hdrStructure.Company_Code = projectsList.CompanyCode;
                hdrStructure.BilledToID = billingData.PlantStorageDetailsID;
                hdrStructure.CreatedBy = loginId;
                hdrStructure.PurchaseRequestId = pr.PurchaseRequestId;
                hdrStructure.SubmitterName = loginuser;
                hdrStructure.IsPRPO = true;
                resultphsUniqueid = new PurchaseOrderBAL().SavePO(hdrStructure, loginId);
                if (resultphsUniqueid > 0)
                {
                    var plantStorage = db.TEPOPlantStorageDetails.Where(a => a.PlantStorageDetailsID == hdrStructure.BilledToID && a.isdeleted == false).FirstOrDefault();
                    PlantStorageCode = plantStorage.PlantStorageCode;
                    if (pr.PurchaseItemList.Count > 0)
                    {
                        Dictionary<int, int> HeadVal = new Dictionary<int, int>();
                        foreach (var itemStructure in pr.PurchaseItemList.OrderBy(x => x.ItemUniqueid))
                        {
                            TEPOItemStructure itms = new TEPOItemStructure();
                            itms.PRRef = pr.PurchaseRequestId;
                            itms.POStructureId = hdrStructure.Uniqueid;
                            itms.Material_Number = itemStructure.Material_Number;
                            //itms.ServiceHeaderId = itemStructure.serviceHeaderid;
                            itms.Short_Text = itemStructure.Short_Text;
                            itms.Long_Text = itemStructure.Long_Text;
                            //itms.Item_Number = itemStructure.Item_Number;
                            itms.ItemType = itemStructure.ItemType;
                            itms.HSNCode = itemStructure.HSN_Code;
                            itms.Brand = itemStructure.Brand;
                            itms.Unit_Measure = itemStructure.Unit_Measure;
                            itms.Order_Qty = itemStructure.Order_Qty;
                            itms.WBSElementCode = itemStructure.WBSElementCode;
                            itms.CreatedOn = System.DateTime.Now.ToString();
                            itms.CreatedBy = loginuser;
                            itms.LastModifiedBy = loginuser;
                            itms.LastModifiedOn = System.DateTime.Now.ToString();
                            itms.IsDeleted = false;

                            if (itms.ItemType == "MaterialOrder")
                            {
                                if (PlantStorageCode == "1050" || PlantStorageCode == "1051")
                                {
                                    itms.GLAccountNo = "110600";
                                }
                                else
                                {
                                    itms.GLAccountNo = "110950";
                                }
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                itms.GLAccountNo = "515200";

                                if (itemStructure.serviceHeaderid != 0)
                                {
                                    if (HeadVal.ContainsKey(itemStructure.serviceHeaderid))
                                        itms.ServiceHeaderId = HeadVal[itemStructure.serviceHeaderid];
                                    else
                                    {
                                        TEPRServiceHeader TEHead = db.TEPRServiceHeaders.Where(x => x.UniqueID == itemStructure.serviceHeaderid && x.IsDeleted == false).FirstOrDefault();
                                        if (TEHead != null)
                                        {
                                            TEPOServiceHeader TEPOHead = new TEPOServiceHeader();
                                            TEPOHead.POHeaderStructureid = hdrStructure.Uniqueid;
                                            TEPOHead.Title = TEHead.Title;
                                            TEPOHead.Description = TEHead.Description;
                                            TEPOHead.IsDeleted = false;
                                            TEPOHead.LastModifiedBy = loginId;
                                            TEPOHead.LastModifiedOn = DateTime.Now;
                                            TEPOHead.ItemNumber = TEHead.ItemNumber;
                                            db.TEPOServiceHeaders.Add(TEPOHead);
                                            db.SaveChanges();
                                            int HeadID = TEPOHead.UniqueID;
                                            HeadVal[itemStructure.serviceHeaderid] = HeadID;
                                            itms.ServiceHeaderId = HeadID;
                                        }
                                    }
                                }

                            }
                            if (itms.ItemType == "ExpenseOrder")
                            {
                                itms.GLAccountNo = "529900";
                            }
                            db.TEPOItemStructures.Add(itms);
                            db.SaveChanges();

                            string checkListId = string.Empty;
                            if (itms.ItemType == "MaterialOrder")
                            {
                                checkListId = GetMaterialSpecificationChecklistId(itms.Material_Number);

                                if (!string.IsNullOrEmpty(checkListId))
                                {
                                    SaveAnnexureSpecificationSheet(checkListId, hdrStructure.Uniqueid, itms.Uniqueid, loginId);
                                }
                                TEPurchase_SaveMaterialSpecifications(itms.Material_Number, hdrStructure.Uniqueid, itms.Uniqueid, loginId);
                            }
                            if (itms.ItemType == "ServiceOrder")
                            {
                                if (!string.IsNullOrEmpty(checkListId))
                                {
                                    SaveServiceAnnexureSpecificationSheet(itms.Material_Number, hdrStructure.Uniqueid, itms.Uniqueid);
                                }
                            }
                            if (itms.PRRef > 0)
                            {
                                UpdatePRitemBalanceQnty(itms.PRRef, itms.Material_Number);
                            }
                        }
                    }
                }
            }
            return resultphsUniqueid;
        }




        public static int CustomIndexOf(string source, char toFind, int position)
        {
            int index = -1;
            for (int i = 0; i < position; i++)
            {
                index = source.IndexOf(toFind, index + 1);

                if (index == -1)
                    break;
            }

            return index;
        }

        public class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
            [JsonProperty("error")]
            public string Error { get; set; }
        }

        public class SpecAnnexValue
        {
            public string value { get; set; }
            public string type { get; set; }
        }
        public class TEPOSpecificatonAnnexureDTO
        {
            public int POItemStructureId { get; set; }
            public int POHeaderStructureId { get; set; }
            public string MaterialName { get; set; }
            public string MaterialCode { get; set; }
            public string MaterialGroup { get; set; }
            public List<TEPOSpecificationAnnexure> SPecsData { get; set; }
        }
        public class PurchaseItemStructureList
        {
            public int PO_OrderID { get; set; }
            public string PO_Title { get; set; }
            public string PO_Description { get; set; }
            public List<int> ItemStructureID { get; set; }
            public int HeaderStructureID { get; set; }
            public string PurchasingOrderNumber { get; set; }
            public string Item_Number { get; set; }
            public string Assignment_Category { get; set; }
            public string Item_Category { get; set; }
            public List<string> Material_Number { get; set; }
            public List<string> Short_Text { get; set; }
            public List<string> Long_Text { get; set; }
            public string Line_item { get; set; }
            public List<string> Order_Qty { get; set; }
            public List<string> Objectid { get; set; }
            public List<string> Unit_Measure { get; set; }
            public string Delivery_Date { get; set; }
            public string Net_Price { get; set; }
            public string Material_Group { get; set; }
            public string Plant { get; set; }
            public string Storage_Location { get; set; }
            public string Requirement_Tracking_Number { get; set; }
            public string Requisition_Number { get; set; }
            public string Item_Purchase_Requisition { get; set; }
            public string Returns_Item { get; set; }
            public List<string> Tax_salespurchases_code { get; set; }
            public string Overall_limit { get; set; }
            public string Expected_Value { get; set; }
            public string Actual_Value { get; set; }
            public string No_Limit { get; set; }
            public string Overdelivery_Tolerance { get; set; }
            public string Underdelivery_Tolerance { get; set; }
            public List<string> HSN_Code { get; set; }
            public List<string> ItemType { get; set; }
            public string Status { get; set; }
            public List<string> WBSElementCode { get; set; }
            public List<string> WBSElementCode2 { get; set; }
            public List<string> InternalOrderNumber { get; set; }
            public List<string> GLAccountNo { get; set; }
            public List<string> Brand { get; set; }
            public List<decimal?> Rate { get; set; }
            public List<decimal?> TotalAmount { get; set; }
            public List<decimal?> IGSTRate { get; set; }
            public List<decimal?> IGSTAmount { get; set; }
            public List<decimal?> CGSTRate { get; set; }
            public List<decimal?> CGSTAmount { get; set; }
            public List<decimal?> SGSTRate { get; set; }
            public List<decimal?> SGSTAmount { get; set; }
            public List<decimal?> TotalTaxAmount { get; set; }
            public List<decimal?> GrossAmount { get; set; }
            public int CreatedByID { get; set; }
            public int LastModifiedByID { get; set; }
            public string CreatedBy { get; set; }
            public string LastModifiedBy { get; set; }
            public decimal PO_PurchaseItemsTotalAmount { get; set; }
            public List<string> AnnexureChecklistId { get; set; }
            public List<string> ManufactureCode { get; set; }
            public List<string> Level1 { get; set; }
            public List<string> Level2 { get; set; }
            public List<string> Level3 { get; set; }
            public List<string> Level4 { get; set; }
            public List<string> Level5 { get; set; }
            public List<string> Level6 { get; set; }
            public List<string> Level7 { get; set; }
            public List<string> gst_procurement { get; set; }
            public List<int?> ServiceHeaderId { get; set; }
        }

        public class MaterialSpecification
        {
            public List<string> MaterialCode { get; set; }
        }

        public class GeneralTandCList
        {
            public int POHeaderstructureID { get; set; }
            public List<int> MasterTandCId { get; set; }
            public int PickListItemId { get; set; }
            public int LastModifiedBy { get; set; }
        }

        public class ObjFromComponentLib
        {
            public List<columnsMain> headers { get; set; }
            public string group { get; set; }
            public string id { get; set; }

        }
        public class columnsMain
        {
            public List<columnsStruct> columns { get; set; }
            public string key { get; set; }
            public string name { get; set; }
        }


        public class columnsStruct
        {
            public dataTypeStruct dataType { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public object value { get; set; }
            public bool isRequired { get; set; }
            public bool isSearchable { get; set; }
        }

        public class dataTypeStruct
        {
            public string name { get; set; }
            public object subType { get; set; }
        }

        public partial class Purchase_LinkedPO_List
        {
            public int UniqueID { get; set; }
            public int MainPOID { get; set; }
            public List<int> LinkedPOID { get; set; }
            public Nullable<int> LastModifiedBy { get; set; }
        }
        public class UpdateMaterialRateRequest
        {
            public string MaterialCode { get; set; }
            public MaterialPurchaseDetail LastPurchaseRate { get; set; }
            public MaterialPurchaseDetail WeightedAveragePurchaseRate { get; set; }
        }
        public class UpdateServiceRateRequest
        {
            public ServicePurchaseDetail lastPurchaseRate { get; set; }
            public ServicePurchaseDetail weightedAveragePurchaseRate { get; set; }
        }
        public class MaterialPurchaseDetail
        {
            public string Amount { get; set; }
            public string Currency { get; set; }
        }
        public class ServicePurchaseDetail
        {
            public string currency { get; set; }
            public string amount { get; set; }
        }

        public class MtlSpecTandC
        {
            public MtlSpecCellsTandCEntries content;
            public string title { get; set; }
            public string checkListId { get; set; }
            public string id { get; set; }
            public string template { get; set; }
        }
        public class MtlSpecCellsTandCEntries
        {
            public List<MtlSpecCellsTandC> entries { get; set; }
        }
        public class MtlSpecCellsTandC
        {
            public List<MtlSpecCellsTandCValue> cells { get; set; }
        }
        public class MtlSpecCellsTandCValue
        {
            public string value { get; set; }
        }

        public class SPecialTC_ObjFromComponentLib
        {
            public List<SPecialTC_columnsMain> headers { get; set; }
            public string group { get; set; }
            public string id { get; set; }

        }
        public class SPecialTC_columnsMain
        {
            public List<SpecifiTCcolumnsStruct> columns { get; set; }
            public string key { get; set; }
            public string name { get; set; }
        }

        public class SpecifiTCcolumnsStruct
        {
            public dataTypeStruct dataType { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public SpecifiTCValueClass value { get; set; }
            public bool isRequired { get; set; }
            public bool isSearchable { get; set; }
        }
        public class SpecifiTCValueClass
        {
            public string ui { get; set; }
            public string url { get; set; }
            public string id { get; set; }
        }
        public class SPecificMainTermsandCoditions
        {
            public int? POHeaderStructureID;
            public int POItemStructureID;
            public string MaterialCode;
            public string MaterialName;
            public string MaterialDescription;
            public List<SPecificTermsandCoditionsDTO> SPecificTCMaster;
            public List<TEPOSpecificTCDetail> PO_SpecificTCDetails;
        }

        public class SPecificTermsandCoditionsDTO
        {
            public int? SpecificTCMasterId;
            public string SpecTCDescription;
            public string SpecTCCmpLibraryID;
            public string SpecTCTitle;
            public string CheckListId;
            public string Template;
            public List<TEPOSpecificTCDetail> PO_SpecificTCDetails;
        }

        public class CmpLibraryItemDTO
        {
            public object Mtl_Classific_Info { get; set; }
            public object Mtl_General_Info { get; set; }
            public object Mtl_Purchase_Info { get; set; }
            public object Mtl_Planning_Info { get; set; }
            public object Mtl_Quality_Info { get; set; }
            public object Mtl_Log_Info { get; set; }
            public object Mtl_Specs_Info { get; set; }
            public string annex_CheckListId { get; set; }
            public object Mtl_Definition_Info { get; set; }
        }
    }
}
