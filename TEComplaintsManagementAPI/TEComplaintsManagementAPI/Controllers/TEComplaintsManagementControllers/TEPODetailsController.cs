using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TECommonEntityLayer;
using TEComplaintsManagementAPI.Models;
using TEComplaintsManagementAPI.SAPResponse;
using System.IO;
using System.Configuration;
using System.Globalization;




namespace TEComplaintsManagementAPI.Controllers.TEComplaintsManagementControllers
{
    public class TEPODetailsController : ApiController
    {
        //api/TEPoDetails/
        // GET: /TEPODetails/

        TEHRIS_DevEntities db = new TEHRIS_DevEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<TEPurchase_header_structure> GetTEPurchaseHeaderStructure()
        {
            // List<TEPurchase_header_structure> listOfPOHeader = db.TEPurchase_header_structure
            //                    .OrderByDescending(x => x.Purchasing_Order_Number).Distinct().ToList();
            //foreach (var item in listOfPOHeader)
            //{
            //    TEPurchaseModel tempModel = CopyEntityToModel(item);

            //    try
            //    {
            //    }
            //    catch (Exception ex)
            //    {
            //        db.ApplicationErrorLogs.Add(
            //       new ApplicationErrorLog
            //       {
            //           Error = ex.Message,
            //           ExceptionDateTime = System.DateTime.Now,
            //           InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
            //           Source = "From TEIssue API | Entity to model Conversion | " + this.GetType().ToString(),
            //           Stacktrace = ex.StackTrace
            //       }
            //       );
            //        db.SaveChanges();
            //    }
            return db.TEPurchase_header_structure.OrderByDescending(x => x.Purchasing_Order_Number); ;
        }

      

        public TEPurchaseModel CopyEntityToModel(TEPurchase_header_structure a)
        {
            TEPurchaseModel b = new TEPurchaseModel();

            // copy fields
            var typeOfEntity = a.GetType();
            var typeOfModel = b.GetType();

            // copy properties
            foreach (var propertyOfA in typeOfEntity.GetProperties())
            {
                try
                {
                    var propertyOfB = typeOfModel.GetProperty(propertyOfA.Name);
                    propertyOfB.SetValue(b, propertyOfA.GetValue(a));
                }
                catch (Exception ex)
                {
                    db.ApplicationErrorLogs.Add(
                        new ApplicationErrorLog
                            {
                                Error = ex.Message,
                                ExceptionDateTime = System.DateTime.Now,
                                InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                                Source = "From TEPurchase_header_structure API | Entity to model Conversion | " + this.GetType().ToString(),
                                Stacktrace = ex.StackTrace
                            }
                        );
                }
            }

            return b;
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

  [HttpGet]
  public IEnumerable<TEPurchase_Assignment> GetTEPurchaseAssignment_(string PurchasingOrderNumber)
  {
      return db.TEPurchase_Assignment.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                              .OrderByDescending(x => x.Uniqueid);
  }

        [HttpGet]
        public IEnumerable<TEPurchase_header> GetTEPurchaseHeader(string PurchasingOrderNumber)
        {
            return db.TEPurchase_header.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                                    .OrderByDescending(x => x.Uniqueid);
        }


        [HttpGet]
        public IEnumerable<TEPurchase_header_condition> GetTEPurchaseHeaderCondition(string PurchasingOrderNumber)
        {
            return db.TEPurchase_header_condition.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                                    .OrderByDescending(x => x.Uniqueid);
        }

        [HttpGet]
        public IEnumerable<TEPurchase_Item_Structure> GetTEPurchaseItemStructure(string PurchasingOrderNumber)
        {
            return db.TEPurchase_Item_Structure.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                                    .OrderByDescending(x => x.Uniqueid);
        }


        [HttpGet]
        public IEnumerable<TEPurchase_Itemwise> GetTEPurchaseItemwise(string PurchasingOrderNumber)
        {
            return db.TEPurchase_Itemwise.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                                    .OrderByDescending(x => x.Uniqueid);
        }

        [HttpGet]
        public IEnumerable<TEPurchase_Service> GetTEPurchaseService(string PurchasingOrderNumber)
        {
            return db.TEPurchase_Service.Where(x => (x.Purchasing_Order_Number == PurchasingOrderNumber))
                                                    .OrderByDescending(x => x.Uniqueid);
        }


        [HttpGet]
        public IEnumerable<TEPurchaseOrderApprover> GetPOApprovers(string PurchasingOrderNumber,int POUniqueId)
        {
            return db.TEPurchaseOrderApprovers.Where(x => (x.POStructureId == POUniqueId) && x.IsDeleted == false && x.SequenceNumber != 0)
                .OrderBy(x => x.UniqueId);
                                                    

        }

        [HttpGet]
        public IEnumerable<TEPurchaseHomeModel> TEPurchaseHome( string ReleaseGroup, string ReleaseStrategy, string ReleaseCode)
        {
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            try
            {
                var dte = System.DateTime.Now;
                var TotalPrice = (from q in db.TEPurchase_header_condition
                                  where (q.Type != "NAVS")
                                     group new { q } by new { q.Purchasing_Order_Number } into g
                                     orderby g.Key.Purchasing_Order_Number descending
                                     select new
                                       {
                                           g.Key.Purchasing_Order_Number,
                                           amount = g.Sum(x => x.q.Rate)
                                           //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                       });

                var query =( from header in db.TEPurchase_header_structure
                            join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                            join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code                                                       
                            orderby header.Purchasing_Order_Number descending
                            select new
                           {
                               header.Purchasing_Order_Number,
                               header.Vendor_Account_Number,
                               header.Purchasing_Document_Date,
                               header.path,
                               fund.FundCenter_Description,
                           }).Distinct();

                foreach (var item in query)
                {

                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        FundCenter_Description = item.FundCenter_Description,
                        Path = item.path,
                        Amount
                        =
                        TotalPrice
                        .Where(x => x.Purchasing_Order_Number
                            == item.Purchasing_Order_Number)
                            .Select(x => x.amount).FirstOrDefault()
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

            db.SaveChanges();
            return result;
        }

        public void getWbsElement(string[] WbsList, string ele, int i)
        {
           // i = WbsList.Length-1;
            string Element = ele;
            char firstChar = Element[0];
            if (firstChar == '0')
            {
                WbsList[i] = Element.Substring(0, 4);
                //i++;
            }
            else if (firstChar == 'A')
            {
                WbsList[i] = Element.Substring(0, 5);
               // i++;
            }
            else if (firstChar == 'C')
            {
                WbsList[i] = Element.Substring(0, 5);
               // i++;
            }
            else if (firstChar == 'M')
            {
                string twoChar = Element.Substring(0, 2);
                if (twoChar == "MN")
                {
                    WbsList[i] = Element.Substring(0, 7);
                   // i++;
                }
                else if (twoChar == "MC")
                {
                    WbsList[i] = Element.Substring(0, 4);
                   // i++;
                }
            }
            else if (firstChar == 'Y')
            {
                string twoChar = Element.Substring(0, 2);
                if (twoChar == "YS")
                {
                    WbsList[i] = Element.Substring(0, 7);
                    //i++;
                }

            }
            else if (firstChar == 'O')
            {
                string threeChar = Element.Substring(0, 3);
                if (threeChar == "OB2")
                {
                    WbsList[i] = Element;
                   // i++;
                }

            }
        }
        [HttpGet]
        public IEnumerable<TEPurchaseHomeModel> TEPurchaseHomeByUser(int UserId, string Status, int PageCount,string SortBy)
        {
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            try
            {
                var dte = System.DateTime.Now;
                //var TotalPrice = (from q in db.TEPurchase_header_condition
                //                  where (q.Type != "NAVS") && (q.IsDeleted == false)
                //                  group new { q } by new { q.Purchasing_Order_Number } into g
                //                  orderby g.Key.Purchasing_Order_Number descending
                //                  select new
                //                  {
                //                      g.Key.Purchasing_Order_Number,
                //                      amount = g.Sum(x => x.q.Rate)
                //                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                //                  });
             
                //var TotalPrice = (from q in db.TEPurchase_Itemwise
                //                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                //                  group new { q } by new { q.Purchasing_Order_Number } into g
                //                  orderby g.Key.Purchasing_Order_Number descending
                //                  select new
                //                  {
                //                      g.Key.Purchasing_Order_Number,
                //                      amount = g.Sum(x => x.q.Condition_rate)
                //                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                //                  });

                var status = db.TEPurchase_header_structure;
                var vendor = db.TEPurchase_Vendor;
                int PoCount = 0;

                //var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)&&(x.webpages_Roles.r)).ToList();
               
                string user = "";
                 string username = "";
               
                db.Configuration.ProxyCreationEnabled = true;
               
                UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
                
                user = profile.CallName;
                username=profile.UserName;

                string user1 = "";
                    user1= "Approver";
                    foreach (var item in profile.webpages_Roles)
                    {
                        if (item.RoleName.Equals("PO  Approval Admin"))
                        {
                            user1 = "PO  Approval Admin";
                            break;
                        }
                    }

              
                if (user1 == "PO  Approval Admin")
                {



                    var query = (from header in db.TEPurchase_header_structure
                                 // join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code
                                 where (header.ReleaseCode2Status == Status && header.IsDeleted==false)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     //fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     // v.Vendor_Owner
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by
                                 }).Distinct().ToList();
                    PoCount = query.Count();
                    query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                    if (Status == "All")
                    {
                         query = (from header in db.TEPurchase_header_structure
                                     // join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code
                                  where (header.ReleaseCode2Status ==
                                  "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected" && header.IsDeleted == false)
                                     orderby header.Uniqueid descending
                                     select new
                                     {
                                         header.Purchasing_Order_Number,
                                         header.Vendor_Account_Number,
                                         header.Purchasing_Document_Date,
                                         header.path,
                                         header.ReleaseCode2,
                                         header.ReleaseCode2Status,
                                         header.ReleaseCode2Date,
                                         header.ReleaseCode3,
                                         header.ReleaseCode3Status,
                                         header.ReleaseCode3Date,
                                         header.ReleaseCode4,
                                         header.ReleaseCode4Status,
                                         header.ReleaseCode4Date,
                                         //fund.FundCenter_Description,
                                         header.Currency_Key,
                                         //appr.UniqueId,
                                         //appr.SequenceNumber,
                                         //appr.ReleaseCode,
                                         // v.Vendor_Owner
                                         header.Uniqueid,
                                         header.PO_Title,
                                         header.SubmitterName,
                                         header.Managed_by
                                     }).Distinct().ToList();
                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
                    }
                    if (Status == "Draft")
                    {
                        query = (from header in db.TEPurchase_header_structure
                                 // join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code
                                 where (header.ReleaseCode2Status == "Pending for Approval" && header.IsDeleted == false)
                               
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     //fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     // v.Vendor_Owner
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by
                                 }).Distinct().ToList();
                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
                    }
                    foreach (var item in query)
                    {
                        string releasestatus = "";
                        DateTime? releasedate=null;
                        releasestatus = Status;
                      //  string[] WbsList ={null};
                        double? TotalPrice = 0.00;
                        DateTime GSTDate = new DateTime(2017, 07, 03);
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                                                                                            && (i.Condition_Type != "NAVS"
                                                                                            && i.Condition_Type != "JICG"
                                                                                        && i.Condition_Type != "JISG"
                                                                                        && i.Condition_Type != "JICR"
                                                                                        && i.Condition_Type != "JISR"
                                                                                        && i.Condition_Type != "JIIR"
                                                                                        )
                                                                                        && (i.VendorCode == null
                                                                                            || i.VendorCode == ""
                                                                                            || i.VendorCode == item.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                            && i.IsDeleted == false
                                                                                            ).ToList();

                        if (wiseList.Count > 0)
                        {
                            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                        }


                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();
                        
                       
                        var ItemStructure= db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted==false)
                                                                ).ToList();

                       
                        foreach (var IS in ItemStructure)
                        {
                            var WBSElementsList = new List<string>(); 
                           
                            if (IS.Item_Category != "D" )
                            {
                                 WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid )
                                                            && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber==IS.Item_Number)
                                            .Select(x => x.WBS_Element).ToList();

                            }
                            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                            {
                                 WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                          && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                          .Select(x => x.WBS_Element).ToList();
                            }


                                foreach (var ele in WBSElementsList)
                                {
                                    //getWbsElement(WbsList, ele, i);



                                    int occ = ele.Count(x => x == '-');

                                    if (occ >= 3)
                                    {
                                        int index = CustomIndexOf(ele, '-', 3);
                                        // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                        string part = ele.Substring(0, index);
                                        WbsHeadsList.Add(part);
                                    }
                                    else
                                    {
                                        WbsHeadsList.Add(ele);
                                    }

                                    string Element = ele;
                                    char firstChar = Element[0];
                                    if (firstChar == '0')
                                    {
                                        
                                        WbsList.Add(Element.Substring(0, 4));
                                       
                                    }
                                    else if (firstChar == 'A')
                                    {
                                      
                                        WbsList.Add(Element.Substring(0, 5));
                                       
                                    }
                                    else if (firstChar == 'C')
                                    {
                                   
                                        WbsList.Add(Element.Substring(0, 5));
                                       
                                    }
                                    else if (firstChar == 'M')
                                    {
                                        string twoChar = Element.Substring(0, 2);
                                        if (twoChar == "MN")
                                        {
                                            
                                            WbsList.Add(Element.Substring(0, 7));
                                            
                                        }
                                        else if (twoChar == "MC")
                                        {
                                            
                                            WbsList.Add(Element.Substring(0, 4));
                                            
                                        }
                                    }
                                    else if (firstChar == 'Y')
                                    {
                                        string twoChar = Element.Substring(0, 2);
                                        if (twoChar == "YS")
                                        {
                                          
                                            WbsList.Add(Element.Substring(0, 7));
                                            
                                        }

                                    }
                                    else if (firstChar == 'O')
                                    {
                                        string threeChar = Element.Substring(0, 3);
                                        if (threeChar == "OB2")
                                        {
                                           
                                            WbsList.Add(Element);
                                            
                                        }

                                    }
                                }

                            }
                            

                            
                         


                        WbsList = WbsList.Distinct().ToList();
                        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                        string WbsHeads = "";
                        foreach (var w in WbsHeadsList)
                        {
                            if (WbsHeads == "")
                            {
                                WbsHeads = w;
                            }
                            else
                            {
                                WbsHeads = WbsHeads + "," + w;
                            }
                        }

                        string ProjectCodes = "";
                        foreach (var w in WbsList)
                        {
                            if (ProjectCodes == "")
                            {
                                ProjectCodes = w;
                            }
                            else 
                            {
                                ProjectCodes =ProjectCodes+","+ w;
                            }
                        }

                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            //Vendor_Account_Number = item.Vendor_Account_Number,
                           // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                            //FundCenter_Description = item.FundCenter_Description,
                            Path = item.path,
                            ReleaseCodeStatus = releasestatus,
                            Purchasing_Release_Date=releasedate,
                            Amount
                            =TotalPrice,                         
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid = item.Uniqueid,
                            PoCount = PoCount,
                            PoTitle=item.PO_Title,
                            VendorName = (vendor
                            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                            Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                            POStatus=item.ReleaseCode2Status,
                            ProjectCodes = ProjectCodes,
                            SubmitterName=item.SubmitterName,
                            WbsHeads = WbsHeads,
                            ManagerName=item.Managed_by


                        });
                    }
                }
                else
                {
                    var query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                                 //join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code
                                 //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                                 // join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code                             
                                 //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

                                 //join vend in db.TEPurchase_Vendor on header.Vendor_Account_Number equals vend.Vendor_Code
                                 where (appr.ApproverId == UserId && appr.Status == Status && appr.IsDeleted == false && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     // vend.Vendor_Owner,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     // fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by

                                 }).Distinct().ToList();

                    PoCount = query.Count();
                    query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                 
                    if (Status == "All")
                    {


                        query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId

                                 where (appr.ApproverId == UserId && appr.IsDeleted == false && (appr.Status == "Pending for Approval" || appr.Status == "Draft" || appr.Status == "Approved" || appr.Status == "Rejected") && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && appr.SequenceNumber != 0)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     // vend.Vendor_Owner,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     // fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by

                                 }).Distinct().ToList();

                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

                    }
                  

                    foreach (var item in query)
                    {
                        string releasestatus = "";
                        DateTime? releasedate = null;
                        releasestatus = Status;
                       
                        double? TotalPrice = 0.00;
                        DateTime GSTDate = new DateTime(2017, 07, 03);
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);                                                                   
                        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid 
                                                                                            && (i.Condition_Type != "NAVS"
                                                                                            && i.Condition_Type != "JICG"
                                                                                        && i.Condition_Type != "JISG"
                                                                                        && i.Condition_Type != "JICR"
                                                                                        && i.Condition_Type != "JISR"
                                                                                        && i.Condition_Type != "JIIR"
                                                                                        )
                                                                                        && (i.VendorCode == null
                                                                                            || i.VendorCode == ""
                                                                                            || i.VendorCode == item.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                            && i.IsDeleted == false).ToList();

                        if (wiseList.Count > 0)
                        {
                            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                        }

                       
                      

                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();

                        var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                ).ToList();


                        foreach (var IS in ItemStructure)
                        {
                            var WBSElementsList = new List<string>();

                            if (IS.Item_Category != "D")
                            {
                                WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                           .Select(x => x.WBS_Element).ToList();

                            }
                            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                            {
                                WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                         .Select(x => x.WBS_Element).ToList();
                            }


                            foreach (var ele in WBSElementsList)
                            {
                                //getWbsElement(WbsList, ele, i);



                                int occ = ele.Count(x => x == '-');

                                if (occ >= 3)
                                {
                                    int index = CustomIndexOf(ele, '-', 3);
                                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                    string part = ele.Substring(0, index);
                                    WbsHeadsList.Add(part);
                                }
                                else
                                {
                                    WbsHeadsList.Add(ele);
                                }

                                string Element = ele;
                                char firstChar = Element[0];
                                if (firstChar == '0')
                                {

                                    WbsList.Add(Element.Substring(0, 4));

                                }
                                else if (firstChar == 'A')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'C')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'M')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "MN")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }
                                    else if (twoChar == "MC")
                                    {

                                        WbsList.Add(Element.Substring(0, 4));

                                    }
                                }
                                else if (firstChar == 'Y')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "YS")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }

                                }
                                else if (firstChar == 'O')
                                {
                                    string threeChar = Element.Substring(0, 3);
                                    if (threeChar == "OB2")
                                    {

                                        WbsList.Add(Element);

                                    }

                                }
                            }

                        }


                        WbsList = WbsList.Distinct().ToList();
                        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                        string WbsHeads = "";
                        foreach (var w in WbsHeadsList)
                        {
                            if (WbsHeads == "")
                            {
                                WbsHeads = w;
                            }
                            else
                            {
                                WbsHeads = WbsHeads + "," + w;
                            }
                        }
                        string ProjectCodes = "";
                        foreach (var w in WbsList)
                        {
                            if (ProjectCodes == "")
                            {
                                ProjectCodes = w;
                            }
                            else
                            {
                                ProjectCodes = ProjectCodes + "," + w;
                            }
                        }



                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                           // FundCenter_Description = item.FundCenter_Description,
                            Path = item.path,
                            ReleaseCodeStatus = releasestatus,
                            Purchasing_Release_Date = releasedate,
                            Amount
                            =TotalPrice,                         
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid=item.Uniqueid,
                            PoCount = PoCount,
                            PoTitle = item.PO_Title,
                            VendorName = (vendor
                            .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                                .Select(x => x.Vendor_Owner).FirstOrDefault()),
                            Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                            POStatus = item.ReleaseCode2Status,
                            ProjectCodes=ProjectCodes,
                            SubmitterName=item.SubmitterName,
                            WbsHeads = WbsHeads,
                            ManagerName=item.Managed_by

                        });
                    }

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

            db.SaveChanges();

            if (SortBy=="PoDate")
                return result.OrderBy(x => x.Purchasing_Document_Date).ToList();

            else if (SortBy=="VendorName")
            return result.OrderBy(x => x.VendorName).ToList();
            //else if (SortBy == "PoNumber")
            //    return result.OrderBy(x => x.Purchasing_Order_Number).ToList();
            else
                return result.OrderBy(x => x.HeaderUniqueid).ToList();
        }

        [HttpGet]
        public IEnumerable<TEPurchaseItemModel> TEPurchaseItemListheader(string Purchasing_Order_Number, int UserId,int POUniqueId)
        {
            List<TEPurchaseItemModel> result = new List<TEPurchaseItemModel>();
            try
            {
                var dte = System.DateTime.Now;
                //var TotalPrice = (from q in db.TEPurchase_header_condition
                //                  where (q.Type != "NAVS") && (q.IsDeleted == false)
                //                  group new { q } by new { q.Purchasing_Order_Number } into g
                //                  orderby g.Key.Purchasing_Order_Number descending
                //                  select new
                //                  {
                //                      g.Key.Purchasing_Order_Number,
                //                      amount = g.Sum(x => x.q.Rate)
                //                  });
                //var TotalPrice = (from q in db.TEPurchase_Itemwise
                //                  where (q.Purchasing_Order_Number == Purchasing_Order_Number) &&  (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                //                  group new { q } by new { q.Purchasing_Order_Number } into g
                //                  orderby g.Key.Purchasing_Order_Number descending
                //                  select new
                //                  {
                //                      g.Key.Purchasing_Order_Number,
                //                      amount = g.Sum(x => x.q.Condition_rate)
                                      
                //                  });


                double TotalPrice = 0.00;

                TEPurchase_header_structure POStructure = db.TEPurchase_header_structure.Where(x => x.Uniqueid == POUniqueId).FirstOrDefault();
                DateTime GSTDate = new DateTime(2017, 07, 03);
                DateTime postingDate = Convert.ToDateTime(POStructure.Purchasing_Document_Date);
                List<TEPurchase_Itemwise> wiselistTotal = db.TEPurchase_Itemwise.Where(q => 
                                                                                        q.IsDeleted == false &&
                                                                                        q.POStructureId == POUniqueId &&
                                                                                        (q.Condition_Type != "NAVS"
                                                                                        && q.Condition_Type != "JICG" 
                                                                                        && q.Condition_Type != "JISG"
                                                                                        && q.Condition_Type != "JICR"
                                                                                        && q.Condition_Type != "JISR"
                                                                                        && q.Condition_Type != "JIIR"
                                                                                        && q.Condition_Type != "JIIG"
                                                                                        && q.Condition_Type != "jimd"
                                                                                        ) 
                                                                                        && (q.VendorCode == null
                                                                                            || q.VendorCode == ""
                                                                                            || q.VendorCode == POStructure.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                        
                                                                                        ).ToList();

                if (wiselistTotal.Count > 0)
                {
                    TotalPrice = wiselistTotal.Sum(q => q.Condition_rate).Value;
                }


                double OtherCharges = 0.00;


                List<TEPurchase_Itemwise> wiselistOther = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == POUniqueId && ((q.Condition_Type == "ZPA1")
                                    || (q.Condition_Type == "ZPA2")
                                      || (q.Condition_Type == "ZPA3")
                                        || (q.Condition_Type == "ZHAN")
                                          || (q.Condition_Type == "ZHA1")
                                            || (q.Condition_Type == "ZOT1")
                                              || (q.Condition_Type == "ZENT")
                                                || (q.Condition_Type == "FRA1")
                                                  || (q.Condition_Type == "FRB1")
                                                  || (q.Condition_Type == "FRC1")
                                                  || (q.Condition_Type == "ZOT4")
                                                  || (q.Condition_Type == "ZOT2")
                                                  || (q.Condition_Type == "ZOT3"))
                                                  && (q.VendorCode == null || q.VendorCode == "" || q.VendorCode == POStructure.Vendor_Account_Number || postingDate <= GSTDate)
                                                  ).ToList();

                if (wiselistOther.Count > 0)
                {
                    OtherCharges = wiselistOther.Sum(q => q.Condition_rate).Value;
                }

                double ItemTax = 0.00;
                List<TEPurchase_Itemwise> wiselistTax = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == POUniqueId && (q.Condition_Type == "JEXS")
                               ).ToList();

                if (wiselistTax.Count > 0)
                {
                    ItemTax = wiselistTax.Sum(q => q.Condition_rate).Value;
                }



                double TPrice = TotalPrice;
                double OthrAmount = OtherCharges;
                double ITax = ItemTax;

                double AmtExclTax = TPrice - (OthrAmount + ITax);

                string AmtExclTaxFinal = Convert.ToString(AmtExclTax);

                if (AmtExclTaxFinal.Contains('.'))
                {
                    string[] convertedstring = AmtExclTaxFinal.Split('.');
                    string decimalvalue = convertedstring[1];
                    if (decimalvalue.Length >= 2)
                    {
                        decimalvalue = decimalvalue.Substring(0, 2);
                    }
                    AmtExclTaxFinal = convertedstring[0] + "." + decimalvalue;
                }
                //var callnme = db.TEEmpBasicInfoes.Where(x => (x.Uniqueid == UserId)).Distinct().First();
                var callnme = db.UserProfiles.Where(x => (x.UserId == UserId)).Distinct().First();
                List<UserProfile> usrinfo = db.UserProfiles.Distinct().ToList();

                string DocumentSubject = db.TEEmailTemplates.Where(x => x.ModuleName == "PODocumnet").Select(x => x.Subject).FirstOrDefault();
                string DocumentBody = db.TEEmailTemplates.Where(x => x.ModuleName == "PODocumnet").Select(x => x.EmailTemplate).FirstOrDefault();

                //var vendor = db.TEPurchase_Vendor;
                //var CompanyName = db.TECompanies.Where();

               


                var query = (from header in db.TEPurchase_header_structure
                           // join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                            //join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code
                            // join vend in db.TEPurchase_Vendor on header.Vendor_Account_Number equals vend.Vendor_Code
                            where
                            (header.Uniqueid == POUniqueId && header.IsDeleted==false)
                             //&& (assign.IsDeleted == false)
                            orderby header.Purchasing_Order_Number descending
                            select new
                            {
                                header.Purchasing_Order_Number,
                                header.Vendor_Account_Number,
                                //vend.Vendor_Owner,
                                header.Purchasing_Document_Date,
                                header.path,
                               // fund.FundCenter_Description,
                                header.ReleaseCode2,
                                header.ReleaseCode2By,
                                header.ReleaseCode2Status,
                                header.ReleaseCode2Date,
                                header.ReleaseCode3,
                                header.ReleaseCode3By,
                                header.ReleaseCode3Status,
                                header.ReleaseCode3Date,
                                header.ReleaseCode4,
                                header.ReleaseCode4By,
                                header.ReleaseCode4Status,
                                header.ReleaseCode4Date,
                                header.Currency_Key,
                                header.SubmitterName,
                                header.Company_Code,
                                header.SubmitterComments,
                                header.SubmittedBy,
                                header.PO_Title,
                                header.Managed_by,
                                header.PartnerFunction1,
                                header.PartnerFunction2,
                                header.PartnerFunctionVendorCode1,
                                header.PartnerFunctionVendorCode2,
                                header.ShipTpCode
                                
                            }).Distinct();

                foreach (var item in query)
                {
                    string releasestatus = "";
                    DateTime? releasedate = null;

                    TECompany Company = db.TECompanies.Where(x=>x.CompanyCode==item.Company_Code).FirstOrDefault();
                    
                    TEPurchase_Vendor vendor = db.TEPurchase_Vendor.Where(x => x.IsDeleted == false && x.Vendor_Code == item.Vendor_Account_Number).FirstOrDefault();
                    PlantStorageDetail shipfrom = new PlantStorageDetail();
                    if (item.PartnerFunction1 != null && item.PartnerFunction1 != "")
                    {
                        List<string> partnerfunction = item.PartnerFunction1.Split(',').ToList();
                        if (partnerfunction.Any(str => str.Contains("GS")))
                        {
                            var GSCode = (partnerfunction.Where(x => x.Contains("GS")).FirstOrDefault() == null ? null : partnerfunction.Where(x => x.Contains("GS")).FirstOrDefault().Split('-')[1]);
                            if (GSCode != null)
                                shipfrom = db.PlantStorageDetails.Where(x => x.isdeleted == false && x.PlantStorageCode == item.PartnerFunctionVendorCode1).FirstOrDefault();
                        }
                        else
                        {
                            shipfrom.ProjectName = vendor.Vendor_Owner;
                            shipfrom.GSTIN = vendor.GSTIN;
                            shipfrom.CountryCode = vendor.Country;
                            shipfrom.Address = vendor.Address;
                            shipfrom.StateCode = vendor.RegionCode;
                        }
                    }
                    else
                    {
                        shipfrom.ProjectName = vendor.Vendor_Owner;
                        shipfrom.GSTIN = vendor.GSTIN;
                        shipfrom.CountryCode = vendor.Country;
                        shipfrom.Address = vendor.Address;
                        shipfrom.StateCode = vendor.RegionCode;
                    }
                    
                    string FundCenter = "";
                    TEPurchase_Item_Structure itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && x.Item_Category == "D").FirstOrDefault();
                    var plantcode = "";
                    var storagecode = "";
                    if (itemStructure != null)
                    {
                        TEPurchase_Service service = db.TEPurchase_Service.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                                 && x.Item_Number == itemStructure.Item_Number).FirstOrDefault();
                        
                        if (service != null)
                        {
                            FundCenter = service.Fund_Center;
                            plantcode = itemStructure.Plant;
                            storagecode = itemStructure.Storage_Location;
                        }

                    }
                    else
                    {
                        itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).FirstOrDefault();
                        if (itemStructure != null)
                        {
                            TEPurchase_Assignment assign = db.TEPurchase_Assignment.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                                && x.ItemNumber == itemStructure.Item_Number).FirstOrDefault();
                            plantcode = itemStructure.Plant;
                            storagecode = itemStructure.Storage_Location;
                            if (assign != null)
                            {
                                FundCenter = assign.Fund_Center;
                            }
                        }
                    }
                    PlantStorageDetail plant = db.PlantStorageDetails.Where(x => x.isdeleted == false && x.PlantStorageCode == plantcode).FirstOrDefault();
                    PlantStorageDetail shipto = new PlantStorageDetail();
                    if (item.ShipTpCode != "" && item.ShipTpCode != null)
                    {
                        shipto = db.PlantStorageDetails.Where(x => x.isdeleted == false && x.StorageLocationCode == item.ShipTpCode).FirstOrDefault();
                    }
                    else
                    {
                        shipto = db.PlantStorageDetails.Where(x => x.isdeleted == false && x.StorageLocationCode == storagecode).FirstOrDefault();
                    }
                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();

                    var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == POUniqueId && x.IsDeleted == false)
                                                            ).ToList();


                    foreach (var IS in ItemStructure)
                    {

                        if (WbsList.Count == 0)
                        {
                            string ele = "";
                            if (IS.Item_Category != "D")
                            {
                                //WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                //                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                //           .Select(x => x.WBS_Element).ToList();

                                ele = db.TEPurchase_Assignment.Where(x => (x.POStructureId == POUniqueId)
                                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                           .Select(x => x.WBS_Element).FirstOrDefault();

                            }
                            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                            {
                                ele = db.TEPurchase_Service.Where(x => (x.POStructureId == POUniqueId)
                                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                         .Select(x => x.WBS_Element).FirstOrDefault();
                            }


                            //getWbsElement(WbsList, ele, i);




                            string Element = ele;
                            char firstChar = Element[0];
                            if (firstChar == '0')
                            {

                                WbsList.Add(Element.Substring(0, 4));

                            }
                            else if (firstChar == 'A')
                            {

                                WbsList.Add(Element.Substring(0, 5));

                            }
                            else if (firstChar == 'C')
                            {

                                WbsList.Add(Element.Substring(0, 5));

                            }
                            else if (firstChar == 'M')
                            {
                                string twoChar = Element.Substring(0, 2);
                                if (twoChar == "MN")
                                {

                                    WbsList.Add(Element.Substring(0, 7));

                                }
                                else if (twoChar == "MC")
                                {

                                    WbsList.Add(Element.Substring(0, 4));

                                }
                            }
                            else if (firstChar == 'Y')
                            {
                                string twoChar = Element.Substring(0, 2);
                                if (twoChar == "YS")
                                {

                                    WbsList.Add(Element.Substring(0, 7));

                                }

                            }
                            else if (firstChar == 'O')
                            {
                                string threeChar = Element.Substring(0, 3);
                                if (threeChar == "OB2")
                                {

                                    WbsList.Add(Element);

                                }

                            }

                        }
                        else
                        {
                            break;
                        }
                    }


                    WbsList = WbsList.Distinct().ToList();
                    WbsHeadsList = WbsHeadsList.Distinct().ToList();

                   
                    string ProjectCodes = "";
                    foreach (var w in WbsList)
                    {
                        if (ProjectCodes == "")
                        {
                            ProjectCodes = w;
                        }
                        else
                        {
                            ProjectCodes = ProjectCodes + "," + w;
                        }
                    }


                    TEPurchase_FundCenter Fund = db.TEPurchase_FundCenter.Where(x => x.IsDeleted == false && x.FundCenter_Code == ProjectCodes).FirstOrDefault();

                    string FundCode = "";
                    string FundDescription = "";

                    if (Fund != null)
                    {
                        FundCode = Fund.FundCenter_Code;
                        FundDescription = Fund.FundCenter_Description;
                    }
                    string OrderType = "Purchase Order";
                    //if (POStructure.Purchasing_Document_Type == "NB")
                    //{
                    //    OrderType = "Service Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "YNB")
                    //{
                    //    OrderType = "Manual Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "ZAO")
                    //{
                    //    OrderType = "Asset Order";
                    //}
                    //else  if (POStructure.Purchasing_Document_Type == "ZEX")
                    //{
                    //    OrderType = "Expense Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "ZI")
                    //{
                    //    OrderType = "Internal Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "ZW1")
                    //{
                    //    OrderType = "Material Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "ZWO")
                    //{
                    //    OrderType = "Material Order";
                    //}
                    //else if (POStructure.Purchasing_Document_Type == "ZWW")
                    //{
                    //    OrderType = "Work Order";
                    //}
                    

                    result.Add(new TEPurchaseItemModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                       // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number + ")",
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = releasestatus,
                        Purchasing_Release_Date = releasedate,
                        ReleaseCode2 = item.ReleaseCode2,
                        ReleaseCode2By = item.ReleaseCode2By,
                        ReleaseCode2ByUserid = (usrinfo.Where(x => x.CallName == item.ReleaseCode2By)
                                            .Select(x => x.UserId).FirstOrDefault()) ,
                        ReleaseCode2Status = item.ReleaseCode2Status,
                        //ReleaseCode2Date = item.ReleaseCode2Date,
                        ReleaseCode2Date = item.ReleaseCode2Date == Convert.ToDateTime("01-01-1900 00:00:00") ? null : item.ReleaseCode2Date,
                        ReleaseCode3 = item.ReleaseCode3,
                        ReleaseCode3By = item.ReleaseCode3By,
                        ReleaseCode3ByUserid = (usrinfo.Where(x => x.CallName == item.ReleaseCode3By)
                                           .Select(x => x.UserId).FirstOrDefault()),
                        //ReleaseCode3Status = ReleaseCode3Status,// commented by sai.k
                        //ReleaseCode3Date = item.ReleaseCode3Date,
                        ReleaseCode3Date = item.ReleaseCode3Date == Convert.ToDateTime("01-01-1900 00:00:00") ? null : item.ReleaseCode3Date,
                        ReleaseCode4 = item.ReleaseCode4,
                        ReleaseCode4By = item.ReleaseCode4By,
                        ReleaseCode4ByUserid = (usrinfo.Where(x => x.CallName == item.ReleaseCode4By)
                                          .Select(x => x.UserId).FirstOrDefault()),
                        //ReleaseCode4Status = ReleaseCode4Status,// commented by sai.k
                        //ReleaseCode4Date = item.ReleaseCode4Date,
                        ReleaseCode4Date = item.ReleaseCode4Date == Convert.ToDateTime("01-01-1900 00:00:00") ? null : item.ReleaseCode4Date,
                        //FundCenter_Description = item.FundCenter_Description,// commented by sai.k
                         Amount
                        =
                       Convert.ToString(TotalPrice),
                        //.Where(x => x.Purchasing_Order_Number
                        //    == item.Purchasing_Order_Number)
                        //    .Select(x => x.amount).FirstOrDefault(),
                        Currency_Key=item.Currency_Key,
                        OfficialEmail = callnme.email,
                        DocumentSubject = DocumentSubject,
                        DocumentBody = DocumentBody,
                        SubmitterName=item.SubmitterName,
                        OtherCharges = Convert.ToString(OthrAmount),
                        ItemTax = Convert.ToString(ITax),
                        AmountExclTax = AmtExclTaxFinal,
                        CompanyName=Company.Name,
                        CompanyAddress=Company.Address,
                        OrderType = OrderType,
                        RevisionNumber=POStructure.Version,
                        POManager=POStructure.Managed_by,
                        PODate=POStructure.Purchasing_Document_Date,
                        VendorName = vendor.Vendor_Owner,
                        VendorAddress=vendor.Address,
                        VendorPanNumber=vendor.PanNumber,
                        VendorServiceTax=vendor.ServiceTax,
                        VendorVat=vendor.Vat,
                        FundCenterCode = FundCode,
                        FundCenterDescription = FundDescription,
                        SubmitterComments=item.SubmitterComments,
                        POTitle = item.PO_Title,
                        ManagerName=item.Managed_by,

                        VenderGSTCode = (vendor==null?"":vendor.GSTIN),
                        VenderRegionCode = (vendor == null ? "" : vendor.RegionCode),
                        VenderRegionCodeDesc=(vendor==null?"":vendor.RegionCodeDesc),
                        VenderCountry=(vendor==null?"":vendor.Country),
                        //BillFromGSTCode = (billfrom==null?"":billfrom.GSTIN),
                        //BillFromAddress =(billfrom==null?"":billfrom.Address) ,
                        //BillFromName = (billfrom==null?"":billfrom.ProjectName),
                        //BillFromCountry =(billfrom==null?"":billfrom.CountryCode) ,
                        ShipFromGSTCode = (shipfrom==null?"":shipfrom.StateCode),
                        ShipFromAddress = (shipfrom==null?"":shipfrom.Address),
                        ShipFromName = (shipfrom==null?"":shipfrom.ProjectName),
                        ShipFromCountry = (shipfrom==null?"":shipfrom.CountryCode),
                        ShipToGSTCode = (shipto==null?"":shipto.StateCode),
                        ShipToAddress = (shipto==null?"":shipto.Address),
                        ShipToName = (shipto==null?"":shipto.ProjectName),
                        ShipToCountry = (shipto==null?"":shipto.CountryCode),
                        PlantCode = (plant == null ? "" : plant.PlantStorageCode),
                        PlantRegionCode = (plant == null ? "" : plant.StateCode),
                        PlantGSTIN = (plant == null ? "" : plant.GSTIN),
                        PlantAddress = (plant == null ? "" : plant.Address),
                        PlantCountry = (plant == null ? "" : plant.CountryCode),
                        PlantName = (plant == null ? "" : plant.ProjectName),
                        ShipToCode = (shipto == null ? "" : shipto.StorageLocationCode),
                        ShipFromCode = (shipfrom == null ? "" : shipfrom.StorageLocationCode)
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
                        Source = "From TEPurchaseItemListheader API | TEPurchaseItemListheader | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );

                db.SaveChanges();
            }

           
            return result;
        }

        [HttpGet]
        public IEnumerable<TEPurchaseItemlistModel> TEPurchaseItemList(string Purchasing_Order_Number,int POUniqueId)
        {
            List<TEPurchaseItemlistModel> result = new List<TEPurchaseItemlistModel>();
            try
            {
                TEPurchase_header_structure purhaseHeader = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                DateTime GSTDate = new DateTime(2017, 07, 03);
                DateTime postingDate = Convert.ToDateTime(purhaseHeader.Purchasing_Document_Date);
                var dte = System.DateTime.Now;
                var TotalPrice = (from q in db.TEPurchase_Itemwise
                                  where (q.Condition_Type != "NAVS" 
                                  && q.Condition_Type != "JICG" 
                                  && q.Condition_Type != "JISG"
                                  && q.Condition_Type != "JICR"
                                  && q.Condition_Type != "JISR"
                                  && q.Condition_Type != "JIIR"
                                  && q.Condition_Type != "JIIG"
                                  && q.Condition_Type != "jimd"
                                  ) && (q.IsDeleted == false)
                                  && (q.VendorCode == null || q.VendorCode == "" || q.VendorCode == purhaseHeader.Vendor_Account_Number || postingDate<=GSTDate)
                                  group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      g.Key.Item_Number_of_Purchasing_Document,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemAmount = (from q in db.TEPurchase_Itemwise
                                  where ((q.Condition_Type != "NAVS") 
                                  && (q.Condition_Type != "JEXS")
                                  && q.Condition_Type != "JICG"
                                  && q.Condition_Type != "JISG"
                                  && q.Condition_Type != "JICR"
                                  && q.Condition_Type != "JISR"
                                  && q.Condition_Type != "JIIR"
                                  && q.Condition_Type != "JIIG"
                                  && q.Condition_Type != "jimd"
                                  && (q.IsDeleted == false)
                                  && (q.VendorCode == null || q.VendorCode == "" || q.VendorCode == purhaseHeader.Vendor_Account_Number || postingDate <= GSTDate)
                                  )
                                  group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      g.Key.Item_Number_of_Purchasing_Document,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemTax = (from q in db.TEPurchase_Itemwise
                               where (q.Condition_Type == "JEXS") && (q.IsDeleted == false)
                               group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                               orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      g.Key.Item_Number_of_Purchasing_Document,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                //Deepak
                var GSTTax = (from q in db.TEPurchase_Itemwise
                               where ((q.Condition_Type == "JICG")
                                        ||(q.Condition_Type=="JISG")
                                        ||(q.Condition_Type=="JIIG")
                                        ) && (q.IsDeleted == false)
                               group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document,q.Condition_Type } into g
                               orderby g.Key.POStructureId descending
                               select new
                               {
                                   g.Key.POStructureId,
                                   g.Key.Item_Number_of_Purchasing_Document,
                                   amount = g.Sum(x => (x.q.Condition_rate.Value)),
                                   g.Key.Condition_Type,
                                   //TotalPrice = g.Sum(x => int.Parse(x.rate))
                               });

                var assign = (from  ass in db.TEPurchase_Assignment
                              where (ass.POStructureId == POUniqueId)
                               && (ass.IsDeleted == false)
                            select new
                             {
                                 ass.POStructureId,
                                 ass.WBS_Element,
                                 ass.Commitment_item,
                                 ass.ItemNumber,
                                 ass.Fund_Center
                             }).Distinct();
                //int purVendorNumber = 0;
                //if (purhaseHeader.Vendor_Account_Number != null && purhaseHeader.Vendor_Account_Number!="")
                //{
                // purVendorNumber=Convert.ToInt32(purhaseHeader.Vendor_Account_Number);
                //}
                var OtherCharges = (from q in db.TEPurchase_Itemwise
                                    where ((q.POStructureId == POUniqueId) &&
                                    
                                    ((q.Condition_Type == "ZPA1")
                                    || (q.Condition_Type == "ZPA2")
                                      || (q.Condition_Type == "ZPA3")
                                        || (q.Condition_Type == "ZHAN")
                                          || (q.Condition_Type == "ZHA1")
                                            || (q.Condition_Type == "ZOT1")
                                              || (q.Condition_Type == "ZENT")
                                                || (q.Condition_Type == "FRA1")
                                                  || (q.Condition_Type == "FRB1")
                                                  || (q.Condition_Type == "FRC1")
                                                  || (q.Condition_Type == "ZOT4")
                                                  || (q.Condition_Type == "ZOT2")
                                                  || (q.Condition_Type == "ZOT3"))
                                                  
                                                  //|| (q.Condition_Type == "ZOFP")
                                                  //|| (q.Condition_Type == "ZOFV")
                                                  //  || (q.Condition_Type == "JINS")
                                                  //    || (q.Condition_Type == "JLDC")
                                                  //      || (q.Condition_Type == "JCDB")
                                                  //        || (q.Condition_Type == "JCV1")
                                                  //          || (q.Condition_Type == "JECV")
                                                  //            || (q.Condition_Type == "JEDB")
                                                  //            || (q.Condition_Type == "JADC")
                                                  //            || (q.Condition_Type == "JCFA")
                                                  //            || (q.Condition_Type == "JFR1")
                                                  //            || (q.Condition_Type == "JFR2")

                                    && (q.IsDeleted == false)
                                    && (q.VendorCode == null || q.VendorCode == "" ||  q.VendorCode == purhaseHeader.Vendor_Account_Number || postingDate <= GSTDate)
                                    )
                                    group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                    orderby g.Key.POStructureId descending
                                    select new
                                    {
                                        g.Key.POStructureId,
                                        g.Key.Item_Number_of_Purchasing_Document,
                                        amount = g.Sum(x => (x.q.Condition_rate.Value))
                                        //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                    });
                
                var query = (from structure in db.TEPurchase_Item_Structure
                             //join ass in db.TEPurchase_Assignment on  structure.Purchasing_Order_Number equals ass.Purchasing_Order_Number into tempassign
                             ////join ass in db.TEPurchase_Assignment on "0000" + structure.Item_Number equals ass.ItemNumber into tempassign
                             //from tempassign1 in tempassign.DefaultIfEmpty()                          
                             where
                             // //(structure.Purchasing_Order_Number == ass.Purchasing_Order_Number)&&
                            // ("0000" + structure.Item_Number == tempassign1.ItemNumber) &&
                             // (structure.Purchasing_Order_Number == tempassign1.Purchasing_Order_Number) &&
                            (structure.POStructureId == POUniqueId)
                            //&& (structure.IsDeleted == false) && (ass.IsDeleted == false)
                             && (structure.IsDeleted == false)// && (tempassign1.IsDeleted == false)
                             orderby structure.Uniqueid descending 
                             select new
                             {
                                 structure.Uniqueid,
                                 structure.POStructureId,
                                 structure.Material_Number,
                                 structure.Item_Category,
                                 structure.Short_Text,
                                 structure.Long_Text,
                                 structure.Line_item,
                                 structure.Order_Qty,
                                 structure.Unit_Measure,
                                 structure.Net_Price,
                                 ItemNumber =structure.Item_Number,
                                 structure.Purchasing_Order_Number,
                                 structure.Tax_salespurchases_code,
                                 structure.HSNCode,
                                 structure.Plant
                                 //tempassign1.WBS_Element,
                                 //tempassign1.Commitment_item,
                                 //tempassign1.ItemNumber
                                 //ass.WBS_Element,
                                 //ass.Commitment_item,
                                 //ass.ItemNumber
                             }).Distinct();

                int gstcondition = 0;
                int isIgst = 0;
                foreach (var item in query)
                {

                   // int itemnumber = item.ItemNumber;
                    PlantStorageDetail plantdetails = db.PlantStorageDetails.Where(x => x.PlantStorageCode == item.Plant && x.isdeleted == false).FirstOrDefault();
                    TEPurchase_Vendor vendor = db.TEPurchase_Vendor.Where(x => x.Vendor_Code == purhaseHeader.Vendor_Account_Number && x.IsDeleted==false).FirstOrDefault();
                    if (plantdetails.StateCode == vendor.RegionCode)
                    {
                        isIgst = 0;
                        gstcondition = 1;
                    }
                    else
                    {
                        isIgst = 1;
                        gstcondition = 1;
                    }
                   
                    double amt =TotalPrice
                    .Where(x => (x.POStructureId == item.POStructureId) 
                        && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                        .Select(x => x.amount).FirstOrDefault();
                        //amt = Math.Round(amt);
                  
                    double? netPrice = 0.00;
                    if (item.Item_Category == "D")
                    {

                        List<TEPurchase_Service> serviceList = db.TEPurchase_Service.Where(x => x.POStructureId == item.POStructureId && x.IsDeleted == false && x.Item_Number == item.ItemNumber).ToList();
                        foreach (var ser in serviceList)
                        {
                            double tempPrice = Convert.ToDouble(ser.Net_Price);
                            netPrice = netPrice + tempPrice;
                        }
                        
                    }
                    else
                    {
                        netPrice = Convert.ToDouble(item.Net_Price);
                    }
                    double? tamount = Convert.ToDouble(item.Order_Qty) * netPrice;

                    double? Others = (OtherCharges.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                            .Select(x => x.amount).FirstOrDefault());
                    double? itemamt = (ItemAmount.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                            .Select(x => x.amount).FirstOrDefault());

                    double? iTax = (ItemTax.Where(x => (x.POStructureId == item.POStructureId)
                                                         && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                           .Select(x => x.amount).FirstOrDefault());
                    double IGST = 0;
                    double CGST = 0;
                    double SGST = 0;
                    if (isIgst == 0)
                    {
                        IGST = 0;
                        CGST = (GSTTax.Where(x => (x.POStructureId == item.POStructureId)
                                                    && (x.Condition_Type == "JICG")
                                                         && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                           .Select(x => x.amount).FirstOrDefault());
                        SGST = (GSTTax.Where(x => (x.POStructureId == item.POStructureId)
                                                    && (x.Condition_Type == "JISG")
                                                         && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                           .Select(x => x.amount).FirstOrDefault());
                        //CGST = (iTax > 0 ? (((iTax.Value / tamount.Value) * 100) / 2) : 0);
                        //SGST = (iTax > 0 ? (((iTax.Value / tamount.Value) * 100) / 2) : 0);
                    }
                    else
                    {
                        IGST = (GSTTax.Where(x => (x.POStructureId == item.POStructureId)
                                                    && (x.Condition_Type == "JIIG")
                                                         && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                           .Select(x => x.amount).FirstOrDefault());
                        //IGST = (iTax > 0 ? (((iTax.Value / tamount.Value) * 100)) : 0);
                        CGST = 0;
                        SGST = 0;
                    }
                    result.Add(new TEPurchaseItemlistModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Material_Number = item.Material_Number,
                        Item_Category = item.Item_Category,
                        Short_Text = item.Short_Text,
                        Long_Text = item.Long_Text +" "+ item.Line_item,
                        Order_Qty = item.Order_Qty,
                        Unit_Measure = item.Unit_Measure,
                        Net_Price = Convert.ToString(netPrice),
                        ItemAmount = itemamt.ToString(),
                        ItemTax = iTax.ToString(),
                        itemTotal = amt.ToString(),
                        //WBS_Element=item.WBS_Element,
                        //Commitment_item=item.Commitment_item,       "0000" +
                        WBS_Element = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber == item.ItemNumber))
                                            .Select(x => x.WBS_Element).FirstOrDefault()),
                        Commitment_item = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber ==  item.ItemNumber))
                                            .Select(x => x.Commitment_item).FirstOrDefault()),
                        ItemNumber = item.ItemNumber,
                        FundCenter = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber == item.ItemNumber))
                                            .Select(x => x.Fund_Center).FirstOrDefault()),
                        OtherCharges = Others.ToString(),
                        Amount = tamount.ToString(),
                        TaxCode=item.Tax_salespurchases_code,
                        HSNCode=item.HSNCode,
                        IGST = IGST.ToString(),
                        CGST = CGST.ToString(),
                        SGST = SGST.ToString(),
                        Plant=item.Plant
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

            db.SaveChanges();
            return result;
        }

        [HttpGet]
        public IEnumerable<TEPurchaseItemlistDetailsModel> TEPurchaseItemlistDetails(string Purchasing_Order_Number, string ItemNumber,int POUniqueId)
        {
            List<TEPurchaseItemlistDetailsModel> result = new List<TEPurchaseItemlistDetailsModel>();
            try
            {
                var dte = System.DateTime.Now;
                var TotalPrice = (from q in db.TEPurchase_Itemwise
                                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                                  group new { q } by new { q.POStructureId } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemTax = (from q in db.TEPurchase_Itemwise
                               where (q.Condition_Type == "JEXS") && (q.IsDeleted == false) && (q.Item_Number_of_Purchasing_Document == ItemNumber)
                               group new { q } by new { q.POStructureId } into g
                               orderby g.Key.POStructureId descending
                               select new
                               {
                                   g.Key.POStructureId,
                                   amount = g.Sum(x => (x.q.Condition_rate.Value))
                                   //TotalPrice = g.Sum(x => int.Parse(x.rate))
                               });
                var assign = //db.TEPurchase_Assignment.where (x =>x.IsDeleted == false);
                (from q in db.TEPurchase_Assignment
                 where (q.POStructureId == POUniqueId) && (q.IsDeleted == false) && (q.ItemNumber == ItemNumber)
                 orderby q.Uniqueid ascending
                 select new
                 {
                     q.Uniqueid,
                     q.IsDeleted,
                    q.Purchasing_Order_Number,
                    q.ItemNumber,
                    q.WBS_Element,
                    q.Commitment_item,
                    q.POStructureId
                  
                 });
                TEPurchase_header_structure purhaseHeader = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                TEPurchase_Item_Structure purhaseItem = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && (x.IsDeleted == false)).FirstOrDefault();
                int isIgst = 0;
                var IGST = "";
                var CGST = "";
                var SGST = "";
                //if (purhaseItem.Plant == purhaseHeader.Vendor_Account_Number)
                //{
                //    IGST = "";
                //    CGST = ItemTax.ToString();
                //    SGST = ItemTax.ToString();
                //}
                //else
                //{
                //    IGST = ItemTax.ToString();
                //    CGST = "";
                //    SGST = "";
                //}
                var query = (from Service in db.TEPurchase_Service
                             //join ass in db.TEPurchase_Assignment on  Service.Item_Number equals ass.ItemNumber
                             where
                             // (Service.Purchasing_Order_Number == ass.Purchasing_Order_Number) &&
                             //( Service.Line_item_number == "00000000"+ass.NetworkNumber_AccountAssignment+"0")&&
                             // (Service.Item_Number==ass.ItemNumber)&&
                             (Service.Item_Number==ItemNumber)&&
                            (Service.POStructureId == POUniqueId)
                            && (Service.IsDeleted == false) //&& (ass.IsDeleted == false)
                             orderby Service.Uniqueid descending 
                             select new
                             {
                                 Service.Uniqueid,
                                 Service.Purchasing_Order_Number,
                                 Service.Activity_Number,
                                 Service.Item_Number,
                                 Service.Short_Text,
                                 Service.LongText,
                                 Service.line_item,
                                 Service.Actual_Qty,
                                 Service.Unit_Measure,
                                 Service.Net_Price,
                                 Service.Gross_Price,
                                 Service.WBS_Element,
                                 Service.Commitment_item,
                                 Service.Fund_Center,
                                 Service.POStructureId,
                                 Service.SACCode,
                                 
                                 //ass.WBS_Element,
                                 //ass.Commitment_item,
                                 // ass.ItemNumber

                             }).Distinct();

                int web = 0;
                int count1 = 0;
                int uniqueid = (from x in assign
                                select x.Uniqueid).FirstOrDefault();
                var qry = (from x in assign
                           select x).Count();
                foreach (var item in query)
                {
                   
                    double amt = TotalPrice
                    .Where(x => x.POStructureId
                        == item.POStructureId)
                        .Select(x => x.amount).FirstOrDefault();
                    //string webselement="";
                    //string Commitment = "";   
                 
                    //if ((web==count1)  && (qry>=count1))
                    //    {
                    //        webselement = assign
                    //            .Where(x => (x.Uniqueid == uniqueid)).Select(x => x.WBS_Element).Take(1).FirstOrDefault();
                    //     Commitment = assign
                    //            .Where(x => (x.Uniqueid == uniqueid)).Select(x => x.Commitment_item).Take(1).FirstOrDefault();
                    //        web =web+ 1;
                    //    }
                   
                   
                   
                    result.Add(new TEPurchaseItemlistDetailsModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Activity_Number = item.Activity_Number,
                        Short_Text = item.Short_Text,
                        Long_Text = item.LongText + " " + item.line_item,
                        Order_Qty = item.Actual_Qty,
                        Unit_Measure = item.Unit_Measure,
                        Net_Price = item.Net_Price,
                        Gross_Price = item.Gross_Price,
                        itemTotal = amt,
                        WBS_Element = item.WBS_Element,
                        // WBS_Element= webselement,
                        ////WBS_Element =  assign
                        ////    .Where (x => (x.Purchasing_Order_Number  == item.Purchasing_Order_Number) &&
                        ////     ( x.ItemNumber == item.Item_Number))
                        ////        .Select(x => x.WBS_Element).FirstOrDefault(),
                       
                        Commitment_item = item.Commitment_item,
                        //Commitment_item =Commitment,
                        ////Commitment_item = assign
                        ////   .Where(x => (x.Purchasing_Order_Number == item.Purchasing_Order_Number) &&
                        ////    (x.ItemNumber == item.Item_Number))
                        ////       .Select(x => x.Commitment_item).FirstOrDefault(),
                        ////  ItemNumber = item.ItemNumber,
                        ItemNumber = assign
                           .Where(x => (x.POStructureId == item.POStructureId) &&
                            (x.ItemNumber == item.Item_Number) )
                               .Select(x => x.ItemNumber).FirstOrDefault(),
                        ItemTax = (ItemTax.Where(x => x.POStructureId == item.POStructureId)
                                         .Select(x => x.amount).FirstOrDefault()),
                                         FundCenter=item.Fund_Center,
                        SACCode=item.SACCode,
                        IGST = IGST,
                        CGST = CGST,
                        SGST = SGST
                    });

                    uniqueid = uniqueid + 1;
                    count1 = count1+1;
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
                        Source = "From TEPurchaseItemlistDetails API | TEPurchaseItemlistDetails | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }

        [HttpPost]
        public object PurchaseApprove(TEPurchaseUpdateModel value)
        {
            TEPurchaseUpdateModel result = value;

            //Approve
            //Edit
            db = new TEHRIS_DevEntities();
            var result1 = (from u in db.TEPurchase_header_structure where (u.Purchasing_Order_Number == value.Purchasing_Order_Number) select u);
            var uniqid = result1.FirstOrDefault().Uniqueid;
           
            //var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
            var usrval = (from usr in db.UserProfiles
                          join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode2By
                          where (terel.Release_Code2 == result.ReleaseCode)
                          select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();
            var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();
           
            if (result1.Count() != 0)
            {
                var dbuser = result1.First();
                db.TEPurchase_header_structure.Attach(dbuser);

                potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();

                if (value.ReleaseCodeStatus == "Rejected")
                {
                    TEPurchase_header_structure releasecode1 = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode2 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                    if (releasecode1 != null)
                    {
                        dbuser.ReleaseCode2Status = "Rejected";
                        db.Entry(dbuser).Property(x => x.ReleaseCode2Status).IsModified = true;

                        dbuser.ReleaseCode2Date = System.DateTime.Now;
                        db.Entry(dbuser).Property(x => x.ReleaseCode2Date).IsModified = true;

                        db.SaveChanges();

                        try{
                            SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                        }

                        return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));    
                    }
                    releasecode1 = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode3 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                    if (releasecode1 != null)
                    {
                        dbuser.ReleaseCode3Status = "Rejected";
                        db.Entry(dbuser).Property(x => x.ReleaseCode3Status).IsModified = true;

                        dbuser.ReleaseCode3Date = System.DateTime.Now;
                        db.Entry(dbuser).Property(x => x.ReleaseCode3Date).IsModified = true;

                        db.SaveChanges();

                        try{
                             usrval = (from usr in db.UserProfiles
                                          join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode3By
                                          where (terel.Release_Code3 == result.ReleaseCode)
                                          select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

                             SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp.NotificationsTemplate.ToString(), uniqid);
                        var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == result.Purchasing_Order_Number.ToString()).FirstOrDefault();

                        var usrval1 = (from usr1 in db.UserProfiles
                                       join terel in db.TEPurchase_ReleaseCode on usr1.CallName equals terel.ReleaseCode2By
                                       where (terel.Release_Code2 == purchaseorder.ReleaseCode2)
                                       select new
                                       {
                                           usr1.UserId,
                                           usr1.CallName
                                       }).Distinct().FirstOrDefault();
                        var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();
                        SendNotification(usrval1.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp1.NotificationsTemplate.ToString() + " " + usrval1.CallName.ToString(), uniqid);
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                        }

                        return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));
                    }
                    releasecode1 = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode4 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                    if (releasecode1 != null)
                    {
                        dbuser.ReleaseCode4Status = "Rejected";
                        db.Entry(dbuser).Property(x => x.ReleaseCode4Status).IsModified = true;

                        dbuser.ReleaseCode4Date = System.DateTime.Now;
                        db.Entry(dbuser).Property(x => x.ReleaseCode4Date).IsModified = true;

                        db.SaveChanges();

                        try{
                            usrval = (from usr in db.UserProfiles
                                      join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode4By
                                      where (terel.Release_Code4 == result.ReleaseCode)
                                      select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();

                            SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp.NotificationsTemplate.ToString(), uniqid);
                        var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == result.Purchasing_Order_Number.ToString()).FirstOrDefault();
                        
                            var usrval1 = (from usr1 in db.UserProfiles
                                           join terel in db.TEPurchase_ReleaseCode on usr1.CallName equals terel.ReleaseCode3By
                                           where (terel.Release_Code3 == purchaseorder.ReleaseCode3)
                                           select new
                                           {
                                               usr1.UserId,
                                               usr1.CallName
                                           }).Distinct().FirstOrDefault();
                            var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();
                            SendNotification(usrval1.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp1.NotificationsTemplate.ToString() + " " + usrval1.CallName.ToString(), uniqid);

                             usrval1 = (from usr1 in db.UserProfiles
                                           join terel in db.TEPurchase_ReleaseCode on usr1.CallName equals terel.ReleaseCode2By
                                           where (terel.Release_Code2 == purchaseorder.ReleaseCode2)
                                           select new
                                           {
                                               usr1.UserId,
                                               usr1.CallName
                                           }).Distinct().FirstOrDefault();
                             potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();
                             SendNotification(usrval1.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp1.NotificationsTemplate.ToString() + " " + usrval1.CallName.ToString(), uniqid);
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                        }
                        return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));
                    }
                }
                    else
                    {
                        PORelease1_OutClient poclient = new PORelease1_OutClient();

                        List<POReleaseReq1Item> porequest = new List<POReleaseReq1Item>();


                        POReleaseReq1Item list = new POReleaseReq1Item();
                        list.FUGUE_ID = value.Fugue_Purchasing_Order_Number;
                        list.PURCHASEORDER = value.Purchasing_Order_Number;
                        list.REL_CODE = value.ReleaseCode;
                        list.REMARKS = "";
                        porequest.Add(list);

                        List<POReleaseRes1Item> poresponse = new List<POReleaseRes1Item>();
                        poclient.ClientCredentials.UserName.UserName = "jktpi";
                        poclient.ClientCredentials.UserName.Password = "jkt1234";
                        poresponse = poclient.PORelease1_Out(porequest.ToArray()).ToList();


                        //dbuser.ReleaseCode1By = value.ReleaseCodeBy;
                        //db.Entry(dbuser).Property(x => x.ReleaseCode1By).IsModified = true;
                        if (poresponse != null)
                        {
                            potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();

                            TEPurchase_header_structure releasecode = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode2 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                            if (releasecode != null)
                            {
                                dbuser.ReleaseCode2Status = "Approved";
                                db.Entry(dbuser).Property(x => x.ReleaseCode2Status).IsModified = true;

                                dbuser.ReleaseCode2Date = System.DateTime.Now;
                                db.Entry(dbuser).Property(x => x.ReleaseCode2Date).IsModified = true;

                                db.SaveChanges();

                                try
                                {
                                    //notificatons
                                    //var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
                                    //var usrval = (from usr in db.UserProfiles
                                    //              join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode2By
                                    //              where (terel.Release_Code2 == result.ReleaseCode)
                                    //              select usr.UserId).Distinct().FirstOrDefault();
                                    //var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();
                                    string pushmessage = "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp.NotificationsTemplate.ToString() ;
                                    SendNotification(usrval.UserId, pushmessage.ToString(), uniqid);

                                    var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == result.Purchasing_Order_Number).FirstOrDefault();
                                    if ((purchaseorder.ReleaseCode3 != null)||(purchaseorder.ReleaseCode3 != ""))
                                    {
                                        var usrval1 = (from usr1 in db.UserProfiles
                                                       join terel in db.TEPurchase_ReleaseCode on usr1.CallName equals terel.ReleaseCode3By
                                                       where (terel.Release_Code3 == purchaseorder.ReleaseCode3)
                                                       select new
                                                       {
                                                           usr1.UserId,
                                                           usr1.CallName
                                                       }).Distinct().FirstOrDefault();
                                        var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                                        SendNotification(usrval1.UserId, "Purchase Order " + result.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(), uniqid);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ex.Message.ToString();
                                }
                                return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));
                            }
                            releasecode = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode3 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                            if (releasecode != null)
                            {
                                dbuser.ReleaseCode3Status = "Approved";
                                db.Entry(dbuser).Property(x => x.ReleaseCode3Status).IsModified = true;

                                dbuser.ReleaseCode3Date = System.DateTime.Now;
                                db.Entry(dbuser).Property(x => x.ReleaseCode3Date).IsModified = true;

                                db.SaveChanges();

                                try{
                                    usrval = (from usr in db.UserProfiles
                                              join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode3By
                                              where (terel.Release_Code3 == result.ReleaseCode)
                                              select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();
                                    SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);
                                var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == result.Purchasing_Order_Number).FirstOrDefault();
                               
                                    usrval = (from usr in db.UserProfiles
                                          join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode2By
                                          where (terel.Release_Code2 == purchaseorder.ReleaseCode2)
                                          select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();
                                    SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);
                               
                                    if ((purchaseorder.ReleaseCode4 != null)||(purchaseorder.ReleaseCode4 != ""))
                                {
                                   
                                    var usrval1 = (from usr1 in db.UserProfiles
                                                   join terel in db.TEPurchase_ReleaseCode on usr1.CallName equals terel.ReleaseCode4By
                                                   where (terel.Release_Code4 == purchaseorder.ReleaseCode4)
                                                   select new
                                                   {
                                                       usr1.UserId,
                                                       usr1.CallName
                                                   }).Distinct().FirstOrDefault();
                                    var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                                    SendNotification(usrval1.UserId, "Purchase Order " + result.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(), uniqid);
                                }
                                }
                                catch (Exception ex)
                                {
                                    ex.Message.ToString();
                                }
                                return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));
                            }
                            releasecode = db.TEPurchase_header_structure.Where(x => (x.ReleaseCode4 == value.ReleaseCode) && (x.Purchasing_Order_Number == value.Purchasing_Order_Number)).FirstOrDefault();
                            if (releasecode != null)
                            {
                                dbuser.ReleaseCode4Status = "Approved";
                                db.Entry(dbuser).Property(x => x.ReleaseCode4Status).IsModified = true;

                                dbuser.ReleaseCode4Date = System.DateTime.Now;
                                db.Entry(dbuser).Property(x => x.ReleaseCode4Date).IsModified = true;

                                db.SaveChanges();
                                try{
                                    SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);
                               
                                var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == result.Purchasing_Order_Number).FirstOrDefault();
                               
                                    usrval = (from usr in db.UserProfiles
                                              join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode2By
                                              where (terel.Release_Code2 == purchaseorder.ReleaseCode2)
                                              select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();
                                    SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);

                                    usrval = (from usr in db.UserProfiles
                                              join terel in db.TEPurchase_ReleaseCode on usr.CallName equals terel.ReleaseCode3By
                                              where (terel.Release_Code3 == purchaseorder.ReleaseCode3)
                                              select new { usr.UserId, usr.CallName }).Distinct().FirstOrDefault();
                                    SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + " " + potemp.NotificationsTemplate.ToString(), uniqid);
                                } 
                                catch (Exception ex)
                                {
                                    ex.Message.ToString();
                                }
                                return db.TEPurchase_header_structure.Where(x => (x.Purchasing_Order_Number == result.Purchasing_Order_Number));
                            }

                        }                   
                }
                db.SaveChanges();
            }


          
            //SendNotification(usrval.UserId, "Purchase Order " + result.Purchasing_Order_Number.ToString() + potemp.NotificationsTemplate.ToString());
                          

                return db.TEPurchase_header_structure.Find(result.Purchasing_Order_Number);            
        }

        [HttpPost]
        public IEnumerable<TEPurchaseAproveModel> PurchaseApproveReject(TEPurchaseAproveModel value)
        {
             List<TEPurchaseAproveModel> result = new List<TEPurchaseAproveModel>();
        
            db = new TEHRIS_DevEntities();

            int AprUniqueid = Convert.ToInt32(value.AproverUniqueid);
            var result1 = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == AprUniqueid && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);
            var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();

            var result2 = (from p in db.TEPurchase_header_structure where (p.Uniqueid == value.POUniqueId) select p);
            var PoUniqeid = result2.FirstOrDefault().Uniqueid;
            var POHeader = db.TEPurchase_header_structure.Where(u => u.Uniqueid == value.POUniqueId).FirstOrDefault();
            UserProfile Submitter = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == POHeader.SubmittedBy).FirstOrDefault();
            string FirstApproverName = POHeader.SubmitterName;
            string FirstApproverEmailId = POHeader.SubmitterEmailID;
            //var TotalPrice = (from q in db.TEPurchase_Itemwise
            //                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
            //                  group new { q } by new { q.Purchasing_Order_Number } into g
            //                  orderby g.Key.Purchasing_Order_Number descending
            //                  select new
            //                  {
            //                      g.Key.Purchasing_Order_Number,
            //                      amount = g.Sum(x => x.q.Condition_rate)
            //                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
            //                  });
            double? doublePrice = 0.00;
            string totalvalue = "";
            doublePrice = db.TEPurchase_Itemwise.Where(i => i.POStructureId == value.POUniqueId && i.Condition_Type != "NAVS" && i.IsDeleted == false).Sum(x => x.Condition_rate.Value);
            //totalvalue = String.Format("{0:0.00}", doublePrice); 

            string VendorName = (db.TEPurchase_Vendor
                                      .Where(x => x.Vendor_Code == POHeader.Vendor_Account_Number)
                                      .Select(x => x.Vendor_Owner).FirstOrDefault());

            //double? POValue = TotalPrice
            //         .Where(x => x.Purchasing_Order_Number
            //         == value.Purchasing_Order_Number)
            //         .Select(x => x.amount).FirstOrDefault();


            var dbuser = result1.First();
            db.TEPurchaseOrderApprovers.Attach(dbuser);
            try
            {

                if (result1.Count() != 0)
                {


                    if (value.ReleaseCodeStatus == "Approved")
                    {

                        //PORelease1_OutClient poclient = new PORelease1_OutClient();

                        //List<POReleaseReq1Item> porequest = new List<POReleaseReq1Item>();


                        //POReleaseReq1Item list = new POReleaseReq1Item();
                        //list.FUGUE_ID = PoUniqeid.ToString();
                        //list.PURCHASEORDER = value.Purchasing_Order_Number;
                        //list.REL_CODE = value.ReleaseCode;
                        //list.REMARKS = "";
                        //porequest.Add(list);

                        //List<POReleaseRes1Item> poresponse = new List<POReleaseRes1Item>();
                        //poclient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SapUserId"].ToString();
                        //poclient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SapPassword"].ToString();
                        //poresponse = poclient.PORelease1_Out(porequest.ToArray()).ToList();
                       
                        

                       
                        //if (poresponse != null && poresponse[0].MESSAGE == "PO Released")
                        //{
                            potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();

                            int ApproverSquence = Convert.ToInt32(value.SequenceId);
                            int NextSequenceId = ApproverSquence + 1;
                            

                            var NextApprovers = (from u in db.TEPurchaseOrderApprovers
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
                            int NxtAprover = Convert.ToInt32(value.UserId);
                            if (NextApprovers.Count() >0 )
                            {

                                dbuser.Status = "Approved";
                                db.Entry(dbuser).Property(x => x.Status).IsModified = true;

                                dbuser.ApprovedOn = System.DateTime.Now;
                                db.Entry(dbuser).Property(x => x.ApprovedOn).IsModified = true;

                                dbuser.Comments = value.Comments;
                                db.Entry(dbuser).Property(x => x.Comments).IsModified = true;
                                
                                db.SaveChanges();

                              




                                int sequenceCheck = 0;
                                foreach (var item in NextApprovers)
                                {
                                    sequenceCheck = sequenceCheck + 1;
                                    if (item.ApproverId == NxtAprover)
                                    {

                                        //PORelease1_OutClient poclient1 = new PORelease1_OutClient();

                                        //List<POReleaseReq1Item> porequest1 = new List<POReleaseReq1Item>();


                                        //POReleaseReq1Item list1 = new POReleaseReq1Item();
                                        //list1.FUGUE_ID = PoUniqeid.ToString();
                                        //list1.PURCHASEORDER = value.Purchasing_Order_Number;
                                        //list1.REL_CODE = item.ReleaseCode;
                                        //list1.REMARKS = "";
                                        //porequest1.Add(list1);

                                        //List<POReleaseRes1Item> poresponse1 = new List<POReleaseRes1Item>();
                                        //poclient1.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SapUserId"].ToString();
                                        //poclient1.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SapPassword"].ToString();
                                        //poresponse1 = poclient1.PORelease1_Out(porequest1.ToArray()).ToList();
                                      
                                       
                                        //if (poresponse1 != null && poresponse1[0].MESSAGE == "PO Released")
                                        //{

                                           

                                       // }
                                            if (sequenceCheck == NextApprovers.Count)
                                            {
                                                PORelease1_OutClient poclient1 = new PORelease1_OutClient();

                                                List<POReleaseReq1Item> porequest1 = new List<POReleaseReq1Item>();


                                                POReleaseReq1Item list1 = new POReleaseReq1Item();
                                                list1.FUGUE_ID = PoUniqeid.ToString();
                                                list1.PURCHASEORDER = value.Purchasing_Order_Number;
                                                list1.REL_CODE = item.ReleaseCode;
                                                list1.REMARKS = "";
                                                porequest1.Add(list1);

                                                List<POReleaseRes1Item> poresponse1 = new List<POReleaseRes1Item>();
                                                poclient1.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SapUserId"].ToString();
                                                poclient1.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SapPassword"].ToString();
                                                poresponse1 = poclient1.PORelease1_Out(porequest1.ToArray()).ToList();


                                                if (poresponse1 != null && poresponse1[0].MESSAGE == "PO Released")
                                                {
                                                    var NextApprover = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == item.UniqueId && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);

                                                    var dbuser1 = NextApprover.First();
                                                    db.TEPurchaseOrderApprovers.Attach(dbuser1);

                                                    dbuser1.Status = "Approved";
                                                    db.Entry(dbuser1).Property(x => x.Status).IsModified = true;

                                                    dbuser1.ApprovedOn = System.DateTime.Now;
                                                    db.Entry(dbuser1).Property(x => x.ApprovedOn).IsModified = true;

                                                    dbuser1.Comments = value.Comments;
                                                    db.Entry(dbuser1).Property(x => x.Comments).IsModified = true;
                                                    db.SaveChanges();


                                                    var PO = (from u in db.TEPurchase_header_structure where (u.Uniqueid == value.POUniqueId) select u);

                                                    var dbuser2 = PO.First();
                                                    db.TEPurchase_header_structure.Attach(dbuser2);

                                                    dbuser2.ReleaseCode2Status = "Approved";
                                                    db.Entry(dbuser2).Property(x => x.ReleaseCode2Status).IsModified = true;


                                                    db.SaveChanges();
                                                }
                                                else
                                                {
                                                    db = new TEHRIS_DevEntities();
                                                    db.ApplicationErrorLogs.Add(
                                                   new ApplicationErrorLog
                                                  {
                                                      Error = poresponse1[0].MESSAGE,
                                                      ExceptionDateTime = System.DateTime.Now,
                                                      InnerException = poresponse1[0].MESSAGE,
                                                      Source = "From TEPODetailsController | SAP PO Approval | " + this.GetType().ToString(),
                                                      Stacktrace = poresponse1[0].MESSAGE


                                                  }
                                                 );
                                                    db.SaveChanges();

                                                    result.Add(new TEPurchaseAproveModel()
                                                   {
                                                       Purchasing_Order_Number = value.Purchasing_Order_Number,
                                                       ReleaseCodeStatus = "SAP Error",

                                                   });
                                                }
                                            }
                                            else
                                            {
                                                var NextApprover = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == item.UniqueId && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);

                                                var dbuser1 = NextApprover.First();
                                                db.TEPurchaseOrderApprovers.Attach(dbuser1);

                                                dbuser1.Status = "Approved";
                                                db.Entry(dbuser1).Property(x => x.Status).IsModified = true;

                                                dbuser1.ApprovedOn = System.DateTime.Now;
                                                db.Entry(dbuser1).Property(x => x.ApprovedOn).IsModified = true;

                                                dbuser1.Comments = value.Comments;
                                                db.Entry(dbuser1).Property(x => x.Comments).IsModified = true;


                                                db.SaveChanges();

                                            }
                                    }
                                    else
                                    {
                                        var NextApprover = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == item.UniqueId && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);

                                        var dbuser1 = NextApprover.First();
                                        db.TEPurchaseOrderApprovers.Attach(dbuser1);

                                        dbuser1.Status = "Pending for Approval";
                                        db.Entry(dbuser1).Property(x => x.Status).IsModified = true;


                                        db.SaveChanges();


                                        break;
                                    }

                                }
                            }
                            else
                            {
                                PORelease1_OutClient poclient = new PORelease1_OutClient();

                                List<POReleaseReq1Item> porequest = new List<POReleaseReq1Item>();


                                POReleaseReq1Item list = new POReleaseReq1Item();
                                list.FUGUE_ID = PoUniqeid.ToString();
                                list.PURCHASEORDER = value.Purchasing_Order_Number;
                                list.REL_CODE = value.ReleaseCode;
                                list.REMARKS = "";
                                porequest.Add(list);

                                List<POReleaseRes1Item> poresponse = new List<POReleaseRes1Item>();
                                poclient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SapUserId"].ToString();
                                poclient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SapPassword"].ToString();
                                poresponse = poclient.PORelease1_Out(porequest.ToArray()).ToList();




                                if (poresponse != null && poresponse[0].MESSAGE == "PO Released")
                                {

                                    dbuser.Status = "Approved";
                                    db.Entry(dbuser).Property(x => x.Status).IsModified = true;

                                    dbuser.ApprovedOn = System.DateTime.Now;
                                    db.Entry(dbuser).Property(x => x.ApprovedOn).IsModified = true;

                                    dbuser.Comments = value.Comments;
                                    db.Entry(dbuser).Property(x => x.Comments).IsModified = true;

                                    db.SaveChanges();


                                    var PO = (from u in db.TEPurchase_header_structure where (u.Uniqueid == value.POUniqueId) select u);

                                    var dbuser1 = PO.First();
                                    db.TEPurchase_header_structure.Attach(dbuser1);

                                    dbuser1.ReleaseCode2Status = "Approved";
                                    db.Entry(dbuser1).Property(x => x.ReleaseCode2Status).IsModified = true;


                                    db.SaveChanges();
                                }
                                else
                                {
                                    db = new TEHRIS_DevEntities();
                                    db.ApplicationErrorLogs.Add(
                                   new ApplicationErrorLog
                                  {
                                      Error = poresponse[0].MESSAGE,
                                      ExceptionDateTime = System.DateTime.Now,
                                      InnerException = poresponse[0].MESSAGE,
                                      Source = "From TEPODetailsController | SAP PO Approval | " + this.GetType().ToString(),
                                      Stacktrace = poresponse[0].MESSAGE


                                  }
                                 );
                                    db.SaveChanges();

                                    result.Add(new TEPurchaseAproveModel()
                                   {
                                       Purchasing_Order_Number = value.Purchasing_Order_Number,
                                       ReleaseCodeStatus = "SAP Error",

                                   });
                                }
                            }

                            var NextApproversList = (from u in db.TEPurchaseOrderApprovers
                                                     join usr in db.UserProfiles on u.ApproverId equals usr.UserId
                                                     where (u.POStructureId == value.POUniqueId && (u.Status == "Approved" || u.Status == "Pending for Approval") && u.IsDeleted == false && u.SequenceNumber != 0)
                                                     orderby u.UniqueId

                                                     select new
                                                     {
                                                         u.UniqueId,
                                                         u.SequenceNumber,
                                                         u.ApproverName,
                                                         usr.UserId,
                                                         u.Status

                                                     }).ToList();

                            string NotifApprover = "";
                            if (NextApproversList.Count>0)
                            {
                                //TEPurchase_header_structure POHeader = db.TEPurchase_header_structure.Where(u => u.Purchasing_Order_Number == value.Purchasing_Order_Number).FirstOrDefault();
                               // TEPurchaseOrderApprover app = new TEPurchaseOrderApprover();
                                 //var   FirstApprover = NextApproversList.First();
                                UserProfile User = db.UserProfiles.Where(x => x.email == FirstApproverEmailId).FirstOrDefault();
                                if (User != null)
                                {
                                    string pushmessage = "Purchase Order " + value.Purchasing_Order_Number + " " + potemp.NotificationsTemplate.ToString();
                                    SendNotification(User.UserId, pushmessage.ToString(), PoUniqeid);
                                }
                                 //Email Code

                                if (FirstApproverEmailId != null || FirstApproverEmailId != "")
                                {
                                    var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POApprove").ToList();
                                    if (Emails.Count > 0)
                                    {
                                        var Email = Emails.First();
                                        Email.Subject = Email.Subject.Replace("$ApproverName", value.AproverName);
                                        Email.Subject = Email.Subject.Replace("$PO", value.Purchasing_Order_Number);

                                        Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", Submitter.CallName);
                                        Email.EmailTemplate = Email.EmailTemplate.Replace("$ApproverName", value.AproverName);
                                        Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                        Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                        SendEmail(Email.Subject, Email.EmailTemplate, Submitter.email);
                                    }
                                }

                                        
                                       
                                        
                                 


                                foreach (var item in NextApproversList)
                                {
                                    UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();

                                

                                    if (item.Status == "Approved" && item.ApproverName != NotifApprover)
                                    {
                                        //string pushmessage = "Purchase Order " + value.Purchasing_Order_Number +" "+ potemp.NotificationsTemplate.ToString();
                                        //SendNotification(item.UserId, pushmessage.ToString(), PoUniqeid);

                                        ////Email Code


                                        //if (item.ApproverName != value.AproverName)
                                        //{
                                        //    var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POApprove").ToList();
                                        //    if (Emails.Count > 0)
                                        //    {
                                        //        var Email = Emails.First();
                                        //        Email.Subject = Email.Subject.Replace("$ApproverName", value.AproverName);
                                        //        Email.Subject = Email.Subject.Replace("$PO", value.Purchasing_Order_Number);

                                        //        Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", ToUser.CallName);
                                        //        Email.EmailTemplate = Email.EmailTemplate.Replace("$ApproverName", value.AproverName);
                                        //        Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                        //        Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                        //        SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                                        //    }
                                        //}


                                        

                                        NotifApprover = item.ApproverName;
                                    }
                                    else if (item.Status == "Pending for Approval")
                                    {
                                       

                                        var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                                        SendNotification(item.UserId, "Purchase Order " + value.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(), PoUniqeid);

                                        var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmit").ToList();
                                        if (Emails.Count > 0)
                                        {
                                            var Email = Emails.First();
                                            Email.Subject = Email.Subject.Replace("$VendorName", VendorName);
                                          

                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", ToUser.CallName);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$POValue", doublePrice.ToString());
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$SubmitterName", Submitter.CallName);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$POTitle", POHeader.PO_Title);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$POVersion", "R" + POHeader.Version);
                                            
                                            SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                                        }

                                    }
                                }
                            }

                            result.Add(new TEPurchaseAproveModel()
                            {
                                Purchasing_Order_Number = value.Purchasing_Order_Number,
                                ReleaseCodeStatus = "Success",

                            });

                        //}
                        //else
                        //{
                        //    db = new TEHRIS_DevEntities();
                        //    db.ApplicationErrorLogs.Add(
                        //   new ApplicationErrorLog
                        //  {
                        //      Error = poresponse[0].MESSAGE,
                        //     ExceptionDateTime = System.DateTime.Now,
                        //      InnerException = poresponse[0].MESSAGE,
                        //     Source = "From TEPODetailsController | SAP PO Approval | " + this.GetType().ToString(),
                        //      Stacktrace = poresponse[0].MESSAGE


                        //     }
                        // );
                        //    db.SaveChanges();

                        // result.Add(new TEPurchaseAproveModel()
                        //{
                        //    Purchasing_Order_Number = value.Purchasing_Order_Number, 
                        //    ReleaseCodeStatus = "SAP Error",
                          
                        //});

                        //}
                    }
                    if (value.ReleaseCodeStatus == "Rejected")
                    {

                        potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();


                        dbuser.Status = "Rejected";
                        db.Entry(dbuser).Property(x => x.Status).IsModified = true;

                        dbuser.ApprovedOn = System.DateTime.Now;
                        db.Entry(dbuser).Property(x => x.ApprovedOn).IsModified = true;

                        dbuser.Comments = value.Comments;
                        db.Entry(dbuser).Property(x => x.Comments).IsModified = true;

                        db.SaveChanges();

                        int ApproverSquence = Convert.ToInt32(value.SequenceId);
                        int NextSequenceId = ApproverSquence + 1;


                        var PO = (from u in db.TEPurchase_header_structure where (u.Uniqueid == value.POUniqueId && u.IsDeleted == false) select u);

                        var dbuser1 = PO.First();
                        db.TEPurchase_header_structure.Attach(dbuser1);

                        dbuser1.ReleaseCode2Status = "Rejected";
                        db.Entry(dbuser1).Property(x => x.ReleaseCode2Status).IsModified = true;


                        db.SaveChanges();


                        var NextApprovers = (from u in db.TEPurchaseOrderApprovers
                                             where (u.SequenceNumber > ApproverSquence && u.POStructureId == value.POUniqueId && u.IsDeleted == false)
                                             orderby u.UniqueId

                                             select new
                                             {
                                                 u.UniqueId,
                                                 u.SequenceNumber,
                                                 u.ApproverName,
                                                 u.ApproverId
                                             }).ToList();
                        string NxtAprover = value.AproverName;
                        if (NextApprovers.Count > 0)
                        {
                            foreach (var item in NextApprovers)
                            {
                                if (item.ApproverName != NxtAprover)
                                {
                                    var NextApprover = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == item.UniqueId && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);

                                    var dbuser2 = NextApprover.First();
                                    db.TEPurchaseOrderApprovers.Attach(dbuser2);

                                    dbuser2.Status = "NULL";
                                    db.Entry(dbuser2).Property(x => x.Status).IsModified = true;

                                    dbuser2.ApprovedOn = System.DateTime.Now;
                                    db.Entry(dbuser2).Property(x => x.ApprovedOn).IsModified = true;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    var NextApprover = (from u in db.TEPurchaseOrderApprovers where (u.UniqueId == item.UniqueId && u.POStructureId == value.POUniqueId && u.IsDeleted == false && u.SequenceNumber != 0) select u);

                                    var dbuser2 = NextApprover.First();
                                    db.TEPurchaseOrderApprovers.Attach(dbuser2);

                                    dbuser2.Status = "Rejected";
                                    db.Entry(dbuser2).Property(x => x.Status).IsModified = true;

                                    dbuser2.ApprovedOn = System.DateTime.Now;
                                    db.Entry(dbuser2).Property(x => x.ApprovedOn).IsModified = true;
                                    db.SaveChanges();
                                }
                                

                            }
                        }
                        //else
                        //{
                        //    var PO = (from u in db.TEPurchase_header_structure where (u.Purchasing_Order_Number == value.Purchasing_Order_Number && u.IsDeleted == false) select u);

                        //    var dbuser1 = PO.First();
                        //    db.TEPurchase_header_structure.Attach(dbuser1);

                        //    dbuser1.ReleaseCode2Status = "Rejected";
                        //    db.Entry(dbuser1).Property(x => x.ReleaseCode2Status).IsModified = true;


                        //    db.SaveChanges();

                        //}

                        var NextApproversList = (from u in db.TEPurchaseOrderApprovers
                                                 join usr in db.UserProfiles on u.ApproverId equals usr.UserId
                                                 where (u.POStructureId == value.POUniqueId && (u.Status == "Approved" || u.Status == "Rejected") && u.IsDeleted == false && u.SequenceNumber != 0)
                                                 orderby u.UniqueId

                                                 select new
                                                 {
                                                     u.UniqueId,
                                                     u.SequenceNumber,
                                                     u.ApproverName,
                                                     usr.UserId
                                                 }).ToList();

                        string NotifApprover = "";
                        if (NextApproversList.Count>0)
                        {
                            UserProfile User = db.UserProfiles.Where(x => x.email == FirstApproverEmailId).FirstOrDefault();
                            if (User != null)
                            {
                                string pushmessage = "Purchase Order " + value.Purchasing_Order_Number + " " + potemp.NotificationsTemplate.ToString();
                                SendNotification(User.UserId, pushmessage.ToString(), PoUniqeid);
                            }
                            //Email Code

                            if (FirstApproverEmailId != null || FirstApproverEmailId != "")
                            {
                                var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POReject").ToList();
                                if (Emails.Count > 0)
                                {
                                    var Email = Emails.First();
                                    Email.Subject = Email.Subject.Replace("$ApproverName", value.AproverName);
                                    Email.Subject = Email.Subject.Replace("$PO", value.Purchasing_Order_Number);

                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", Submitter.CallName);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$ApproverName", value.AproverName);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                    SendEmail(Email.Subject, Email.EmailTemplate, Submitter.email);
                                }
                            }


                            foreach (var item in NextApproversList)
                            {
                                UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();
                               
                                if (item.ApproverName != value.AproverName && item.ApproverName != NotifApprover)
                                {

                                    string pushmessage = "Purchase Order " + value.Purchasing_Order_Number +" "+ potemp.NotificationsTemplate.ToString();
                                    SendNotification(item.UserId, pushmessage.ToString(), PoUniqeid);
                                    NotifApprover = item.ApproverName;

                                    if (item.ApproverName != value.AproverName)
                                    {

                                        var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POReject").ToList();
                                        if (Emails.Count > 0)
                                        {
                                            var Email = Emails.First();
                                            Email.Subject = Email.Subject.Replace("$ApproverName", value.AproverName);
                                            Email.Subject = Email.Subject.Replace("$PO", value.Purchasing_Order_Number);

                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", ToUser.CallName);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$ApproverName", value.AproverName);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                            Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                            //SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                                        }
                                    }
                                }
                            }
                        }

                        result.Add(new TEPurchaseAproveModel()
                        {
                            Purchasing_Order_Number = value.Purchasing_Order_Number,
                            ReleaseCodeStatus = "Success",

                        });
                    }
                }
            }
            catch (Exception ex)
            
            
            {
                db = new TEHRIS_DevEntities();
                db.ApplicationErrorLogs.Add(
                 new ApplicationErrorLog
                 {
                     Error = ex.Message,
                     ExceptionDateTime = System.DateTime.Now,
                     InnerException = ex.InnerException != null ? ex.InnerException.Message : "",
                     Source = "From TEPODetailsController | TEPurchaseApproveReject | " + this.GetType().ToString(),
                     Stacktrace = ex.StackTrace



                 }
                 );
                db.SaveChanges();
                result.Add(new TEPurchaseAproveModel()
                {
                    Purchasing_Order_Number = value.Purchasing_Order_Number,
                    ReleaseCodeStatus = ex.Message,

                });

            }
           // db.SaveChanges();
            return result;
        }

        [HttpPost]
        public TENotification SendNotification(int UserId, string description,int purchaseordernumber)
        {
          
                string description1;
                //string[] msg = description.Split('$');
                //var purchaseorder = db.TEPurchase_header_structure.Where(x => x.Purchasing_Order_Number == msg[0]).FirstOrDefault();
                //if (msg[2] == "Approved")
                //{
                //    var usrval = (from usrid in db.UserProfiles
                //                  join terel in db.TEPurchase_ReleaseCode on usrid.CallName equals terel.ReleaseCode2By
                //                  where (terel.Release_Code2 == msg[1])
                //                  select new { usrid.UserId,
                //                               usrid.CallName,
                //                               usrid.AndroidToken,usrid.IosToken
                //                  }).Distinct().FirstOrDefault();
                //    var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POAPPROVED").FirstOrDefault();

                //    UserId = usrval.UserId;
                //}
                //if (msg[2] == "Rejected")
                //{
                //    var usrval = (from usrid in db.UserProfiles
                //                  join terel in db.TEPurchase_ReleaseCode on usrid.CallName equals terel.ReleaseCode2By
                //                  where (terel.Release_Code2 == msg[1])
                //                  select new
                //                  {
                //                      usrid.UserId,
                //                      usrid.CallName
                //                  }).Distinct().FirstOrDefault();
                //    var potemp = db.TENotificationsTemplates.Where(x => x.ModuleName == "POReject").FirstOrDefault();
                //    description1 = "Issue: " + description.ToString() + " " + usrval.CallName.ToString();
                //    UserId = usrval.UserId;
                //}

                //var usr = db.UserProfiles.Where(x => x.UserId == value.RaisedBy).FirstOrDefault();
                //
                //
                var usr = db.UserProfiles.Where(x => x.UserId == UserId).FirstOrDefault();
                //description1 = description.ToString() + " " + usr.CallName.ToString();
                if (usr.CallName != null)
                    description1 = description.ToString() + " " + usr.CallName.ToString();
                else
                {
                    description1 = description.ToString();
                }
                //description1 = description.ToString();

                TENotification notify = new TENotification
                {
                    ApprovedBy = "PO.Admin",
                    ApprovedOn = System.DateTime.UtcNow,
                    CreatedBy = "PO.Admin",
                    CreatedOn = System.DateTime.UtcNow,
                    //Have to be dynamic...

                    IsDeleted = false,
                    LastModifiedBy = "PO.Admin",
                    LastModifiedOn = System.DateTime.UtcNow,

                    Name = "PO Details",
                    description = description1,
                    ReceivedBy = UserId, 
                    SendBy = UserId,                                      
                    ReadStatus = false,
                    Status = "Active",
                    Module = "PO",
                    ModuleUniqueid = purchaseordernumber
                    //Type

                };

                var usrtype = (from usrs in db.UserProfiles
                               join tebas in db.TEEmpBasicInfoes on usrs.UserName equals tebas.UserId
                               where (usrs.UserId == UserId)
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
                        // tepush.SendNotification_IOS(usr.IosToken.ToString(), description1, apptype);

                        //TEPushNotificationAPI.Controllers.TEPushNotification tepush = new TEPushNotificationAPI.Controllers.TEPushNotification();
                        tepush.SendNotification_IOS(usr.IosToken, description1, apptype);



                         
                     
                     
                     }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
            return new TENotificationsController().AddNotifications(notify);
        }


        [HttpPost]
        public string SendEmail(string Subject, string Body, string ToEmail)
        {
            try
            {
                TEEmail.TEEmailSending.ITEEmailSendingManager emailMgr = TEEmail.TEEmailSending.TEEmailSendingManagerFactory.GetInstance().Create();
                TEEmailModel.TEEmailSendingModels.TEEmailSendingRequestModel emailReq = new TEEmailModel.TEEmailSendingModels.TEEmailSendingRequestModel();
                emailReq.Subject = Subject;
                emailReq.From = ConfigurationManager.AppSettings["EmailUserId"].ToString();
                emailReq.To = ToEmail;
                //emailReq.Bcc = ConfigurationManager.AppSettings["EmailBccId"].ToString();
                emailReq.Html = Body;
                emailReq.SenderType = TEEmailModel.Enums.SenderType.Individual;
                //emailReq.AttachmentPath = new List<string>();
                //emailReq.AttachmentPath.Add("E:\\Sai Kumar\\Response.pdf");
                emailMgr.SendComplexMessage(emailReq);
            }
            catch(Exception Ex)
            {

            }
            return "Success";
        }


        //public  string SendNotification_IOS(string IosToken, string Message, string AppType)
        //{
        //    string returnvalue = "";
        //    var appleCert = File.ReadAllBytes("C:\\Certificates.onlykeyproduction.p12");
        //    string CertificatePassword = "";
        //    try
        //    {
        //        //string DeviceID = "0c11cdc92d14b96d6f309cc661c4cd36abe9cb1f38bccf4019de31b6cf97992b";
        //        var push = new PushBroker();
        //        if (AppType == "Fugue")
        //        {
        //            appleCert = File.ReadAllBytes("C:\\Certificates.onlykeyproduction.p12");
        //            CertificatePassword = "sairam";
        //        }
        //        else if (AppType == "Yellow")
        //        {
        //            appleCert = File.ReadAllBytes("C:\\CertificatesYellowPushwithonlykey.p12");
        //            CertificatePassword = "sairam";
        //        }

        //        push.OnChannelCreated += pushr_OnChannelCreated;
        //        push.OnChannelDestroyed += push_OnChannelDestroyed;
        //        push.OnChannelException += push_OnChannelException;
        //        push.OnNotificationRequeue += push_OnNotificationRequeue;
        //        push.OnServiceException += pushr_OnServiceException;
        //        push.OnNotificationSent += push_OnNotificationSent;
        //        push.OnNotificationFailed += push_OnNotificationFailed;
        //        push.RegisterAppleService(new ApplePushChannelSettings(true, appleCert, CertificatePassword));
        //        push.QueueNotification(new AppleNotification()
        //                      .ForDeviceToken(IosToken)
        //                      .WithAlert(Message)

        //          );
        //        returnvalue = "IOS Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        returnvalue = Convert.ToString(ex.Message);
        //    }
        //    return returnvalue;
        //}

      
         [HttpGet]
        public IEnumerable<TEPurchasetermsModel> TEPurchaseTerms(string Purchasing_Order_Number)
        {
            List<TEPurchasetermsModel> result = new List<TEPurchasetermsModel>();
            try
            {
                 //var query = (from header in db.TEPurchase_header
                 //             join terms in db.TEPurchase_Terms on header.LineNo equals terms.LineNo                          
                 //           where
                 //           (header.Purchasing_Order_Number == Purchasing_Order_Number)
                 //           orderby header.Purchasing_Order_Number descending
                 //           select new
                 //           {
                 //               header.Purchasing_Order_Number,
                 //                  header.LineNo,
                 //               terms.Header,
                 //              header.Header_text
                 //               //descrption = (from descr in db.TEPurchase_header
                 //               //         where (descr.LineNo == header.LineNo)
                 //               //       select new { descr.Header_text }).Distinct()
                 //           }).Distinct();
                //foreach (var item in query)
                //{
                //    result.Add(new TEPurchasetermsModel()
                //    {
                //        Purchasing_Order_Number = item.Purchasing_Order_Number,
                //        LineNo = item.LineNo,
                //        Header = item.Header,
                //        //Description = item.descrption
                //        Description = item.Header_text
                //    });
                //}

                var query = (from header in db.TEPurchase_header
                              where
                            (header.Purchasing_Order_Number == Purchasing_Order_Number)
                            && (header.IsDeleted == false)
                            && (header.LineNo != "55") && (header.LineNo != "56") && (header.LineNo != "57")                           
                             orderby header.Uniqueid descending 
                              select new
                              {
                                  header.Purchasing_Order_Number,
                                header.LineNo,
                            }).Distinct();
                 foreach (var item in query)
                {
                    var hea = (from header1 in db.TEPurchase_header
                                 where
                               (header1.Purchasing_Order_Number == Purchasing_Order_Number)
                               && (header1.LineNo == item.LineNo)
                                && (header1.IsDeleted == false)
                                 && (header1.Header_text != "REMARKS") && (header1.Header_text != "APPROVER") && (header1.Header_text.Substring(0, 5) != "Fugue")
                           
                                 orderby header1.Uniqueid
                                 select new
                                 {
                                     header1.Uniqueid,
                                     header1.LineNo,
                                     header1.Header_text
                                 }).Distinct();
                    string description = "";
                    foreach (var item1 in hea)
                    {
                       
                        description = description +" "+ item1.Header_text;
                    }
                    var head = (from header2 in db.TEPurchase_Terms
                                where (header2.LineNo == item.LineNo)
                               select new
                               {
                                   header2.Header,
                               }).Distinct();
                    string header = "";
                    foreach (var item2 in head)
                    {
                        header = item2.Header;
                    }
                    result.Add(new TEPurchasetermsModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        LineNo = item.LineNo,
                        Header = header,
                        //Description = item.descrption
                        Description = description
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
                        Source = "From TEPurchaseTerms API | TEPurchaseTerms | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }

         [HttpGet]
         public void PoDailyReport()
         {
             var ApproverList = db.TEPurchaseOrderApprovers.Where(x => x.IsDeleted == false).Select(e=>new {e.ApproverName,e.ApproverId}).Distinct().ToList();

            
             foreach (var item in ApproverList)
             {
                 string Body = "";
                 int SnNo = 1;
                 var PoList=(from a in db.TEPurchaseOrderApprovers  join p in db.TEPurchase_header_structure 
                             on a.PurchaseOrderNumber equals p.Purchasing_Order_Number
                             where (a.ApproverId == item.ApproverId && a.IsDeleted == false && a.Status == "Pending for Approval")
                                  select new{
                                     a.PurchaseOrderNumber,
                                     p.Vendor_Account_Number,
                                     p.PO_Title,
                                     p.CreatedOn
                                  }).Distinct().ToList();
              
                 foreach (var Po in PoList)
                 {
                     double doublePrice = 0.00;
                     List<TEPurchase_Itemwise> temp = db.TEPurchase_Itemwise.Where(i => i.Purchasing_Order_Number == Po.PurchaseOrderNumber && i.Condition_Type != "NAVS" && i.IsDeleted == false).ToList();//.Sum(x => x.Condition_rate.HasValue?x.Condition_rate.Value:0.00);
                     if (temp.Count > 0)
                     {
                         doublePrice = temp.Sum(x => x.Condition_rate.HasValue ? x.Condition_rate.Value : 0.00);
                     }

                     doublePrice = Math.Round(doublePrice, 1, MidpointRounding.AwayFromZero);
                     // string value=string.Format("{0:n0}", doublePrice).ToString(); // no digits after the decimal point.

                     //string value = string.Format("{0:#,###0}", doublePrice).ToString();

                     CultureInfo cultureInfo = new CultureInfo("hi-IN");
                    //var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                    //numberFormatInfo.CurrencySymbol = ",";
                     string value = doublePrice.ToString("C", cultureInfo);
                     //string value = doublePrice.ToString();
                     //String indianFormatNumber = value.Substring(3);
                     string VendorName = (db.TEPurchase_Vendor
                                               .Where(x => x.Vendor_Code == Po.Vendor_Account_Number)
                                               .Select(x => x.Vendor_Owner).FirstOrDefault());
                     DateTime CreatedOn=Convert.ToDateTime( Po.CreatedOn);
                     string PoDate = CreatedOn.ToString("dd-MMM-yyyy");
                     if (Body == "")
                     {

                         Body = "<tr><td>" + SnNo + "</td><td>" + Po.PurchaseOrderNumber + "</td><td>" + Po.PO_Title + "</td><td>" + VendorName + "</td><td></td><td style=\"text-align:right;\" class='currency'>" + value + "</td></tr>";
                     }
                     else
                     {
                         Body = Body + "<tr><td>" + SnNo + "</td><td>" + Po.PurchaseOrderNumber + "</td><td>" + Po.PO_Title + "</td><td>" + VendorName + "</td><td></td><td style=\"text-align:right;\" class='currency'>" + value + "</td></tr>";
                     }
                      //counter=counter+1;
                      //if (PoList.Count == counter)
                      //{
                      //    counter = 0;
                      //}
                      //else
                      //{
                      //    Body = Body + "$Body";
                      //}
                     SnNo = SnNo + 1;
                 }

                 UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.ApproverId).FirstOrDefault();
                 var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmitDailyReport").ToList();
                 if (Emails.Count > 0)
                 {
                     TEEmailTemplate Email = new TEEmailTemplate() ;

                     Email.EmailTemplate = Emails[0].EmailTemplate.Replace("$Employee", ToUser.CallName);
                     Email.EmailTemplate = Email.EmailTemplate.Replace("$Body", Body);
                     Email.Subject = Emails[0].Subject;
                     SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                 }

                 
             }
         }


        [HttpGet]
        public IEnumerable<TEPurchaseHomeModel> TEPurchaseHomeByUserFilter(int UserId, string Status, int PageCount,string FilterBy)
        {
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();
            try
            {
                var dte = System.DateTime.Now;
                
                //var TotalPrice = (from q in db.TEPurchase_Itemwise
                //                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                //                  group new { q } by new { q.Purchasing_Order_Number } into g
                //                  orderby g.Key.Purchasing_Order_Number descending
                //                  select new
                //                  {
                //                      g.Key.Purchasing_Order_Number,
                //                      amount = g.Sum(x => x.q.Condition_rate)
                //                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                //                  });

                var status = db.TEPurchase_header_structure;
                var vendor = db.TEPurchase_Vendor;
                int PoCount = 0;

              

                string user = "";
                string username = "";
               
                db.Configuration.ProxyCreationEnabled = true;
                
                UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
               
                user = profile.CallName;
                username = profile.UserName;

                string user1 = "";
                user1 = "Approver";
                foreach (var item in profile.webpages_Roles)
                {
                    if (item.RoleName.Equals("PO  Approval Admin"))
                    {
                        user1 = "PO Approval Admin";
                        break;
                    }
                }
                if (user1 == "PO Approval Admin")
                {



                    var query = (from header in db.TEPurchase_header_structure
                                join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                                 from v in ps.DefaultIfEmpty()
                                 where (header.ReleaseCode2Status == Status && header.IsDeleted==false  && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     //fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     // v.Vendor_Owner
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by
                                 }).Distinct().ToList();
                    PoCount = query.Count();
                    query = query.Skip((PageCount - 1) * 20).Take(20).ToList();


                    if (Status == "All")
                    {


                        query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                                 join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                                 from v in ps.DefaultIfEmpty()
                                 //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                                 // join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code                             
                                 //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

                                 //join vend in db.TEPurchase_Vendor on header.Vendor_Account_Number equals vend.Vendor_Code
                                 where (appr.IsDeleted == false && header.IsDeleted == false && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)) && appr.SequenceNumber != 0)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     // vend.Vendor_Owner,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     // fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by

                                 }).Distinct().ToList();

                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 20).Take(20).ToList();

                    }
                    if (Status == "Draft")
                    {
                        query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                                 join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                                 from v in ps.DefaultIfEmpty()
                                 // join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code
                                 where (header.ReleaseCode2Status == "Pending for Approval" && header.IsDeleted == false && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)) && appr.SequenceNumber != 0)

                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     //fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     // v.Vendor_Owner
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by
                                 }).Distinct().ToList();
                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 20).Take(20).ToList();
                    }
                    foreach (var item in query)
                    {
                        string releasestatus = "";
                        DateTime? releasedate = null;
                        releasestatus = Status;

                        double? TotalPrice = 0.00;
                        DateTime GSTDate = new DateTime(2017, 07, 03);
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                                                                                            && (i.Condition_Type != "NAVS"
                                                                                            && i.Condition_Type != "JICG"
                                                                                        && i.Condition_Type != "JISG"
                                                                                        && i.Condition_Type != "JICR"
                                                                                        && i.Condition_Type != "JISR"
                                                                                        && i.Condition_Type != "JIIR"
                                                                                        )
                                                                                        && (i.VendorCode == null
                                                                                            || i.VendorCode == ""
                                                                                            || i.VendorCode == item.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                            && i.IsDeleted == false).ToList();
                        //List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid && i.Condition_Type != "NAVS" && i.IsDeleted == false).ToList();

                        if (wiseList.Count > 0)
                        {
                            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                        }

                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();

                        var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                ).ToList();


                        foreach (var IS in ItemStructure)
                        {
                            var WBSElementsList = new List<string>();

                            if (IS.Item_Category != "D")
                            {
                                WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                           .Select(x => x.WBS_Element).ToList();

                            }
                            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                            {
                                WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                         .Select(x => x.WBS_Element).ToList();
                            }


                            foreach (var ele in WBSElementsList)
                            {
                                //getWbsElement(WbsList, ele, i);



                                int occ = ele.Count(x => x == '-');

                                if (occ >= 3)
                                {
                                    int index = CustomIndexOf(ele, '-', 3);
                                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                    string part = ele.Substring(0, index);
                                    WbsHeadsList.Add(part);
                                }
                                else
                                {
                                    WbsHeadsList.Add(ele);
                                }

                                string Element = ele;
                                char firstChar = Element[0];
                                if (firstChar == '0')
                                {

                                    WbsList.Add(Element.Substring(0, 4));

                                }
                                else if (firstChar == 'A')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'C')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'M')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "MN")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }
                                    else if (twoChar == "MC")
                                    {

                                        WbsList.Add(Element.Substring(0, 4));

                                    }
                                }
                                else if (firstChar == 'Y')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "YS")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }

                                }
                                else if (firstChar == 'O')
                                {
                                    string threeChar = Element.Substring(0, 3);
                                    if (threeChar == "OB2")
                                    {

                                        WbsList.Add(Element);

                                    }

                                }
                            }

                        }


                        WbsList = WbsList.Distinct().ToList();
                        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                        string WbsHeads = "";
                        foreach (var w in WbsHeadsList)
                        {
                            if (WbsHeads == "")
                            {
                                WbsHeads = w;
                            }
                            else
                            {
                                WbsHeads = WbsHeads + "," + w;
                            }
                        }
                        string ProjectCodes = "";
                        foreach (var w in WbsList)
                        {
                            if (ProjectCodes == "")
                            {
                                ProjectCodes = w;
                            }
                            else
                            {
                                ProjectCodes = ProjectCodes + "," + w;
                            }
                        }


                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            //Vendor_Account_Number = item.Vendor_Account_Number,
                            // Vendor_Account_Number = item.Vendor_Owner + " (" + item.Vendor_Account_Number+")",
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                            //FundCenter_Description = item.FundCenter_Description,
                            Path = item.path,
                            ReleaseCodeStatus = releasestatus,
                            Purchasing_Release_Date = releasedate,
                            Amount
                            =TotalPrice,                           
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid = item.Uniqueid,
                            PoCount = PoCount,
                            PoTitle = item.PO_Title,
                            VendorName = (vendor
                           .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                               .Select(x => x.Vendor_Owner).FirstOrDefault()),
                            Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                            POStatus = item.ReleaseCode2Status,
                            ProjectCodes=ProjectCodes,
                            SubmitterName=item.SubmitterName,
                            WbsHeads = WbsHeads,
                            ManagerName=item.Managed_by
                        });
                    }
                }
                else
                {
                    var query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                                 join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                                 from v in ps.DefaultIfEmpty() 
                                 //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                                 // join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code                             
                                 //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

                                 //join vend in db.TEPurchase_Vendor on header.Vendor_Account_Number equals vend.Vendor_Code
                                 where (appr.ApproverId == UserId && appr.Status == Status && appr.IsDeleted == false && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)) && appr.SequenceNumber != 0)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     // vend.Vendor_Owner,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     // fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by

                                 }).Distinct().ToList();

                    PoCount = query.Count();
                    query = query.Skip((PageCount - 1) * 20).Take(20).ToList();

                  

                    if (Status == "All")
                    {


                        query = (from header in db.TEPurchase_header_structure
                                 join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                                 join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                                 from v in ps.DefaultIfEmpty()
                                 //join assign in db.TEPurchase_Assignment on header.Purchasing_Order_Number equals assign.Purchasing_Order_Number
                                 // join fund in db.TEPurchase_FundCenter on assign.Fund_Center equals fund.FundCenter_Code                             
                                 //join usr in db.UserProfiles on appr.ApproverName equals usr.CallName

                                 //join vend in db.TEPurchase_Vendor on header.Vendor_Account_Number equals vend.Vendor_Code
                                 where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && header.ReleaseCode2Status != "Superseded" && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)) && appr.SequenceNumber != 0)
                                 orderby header.Uniqueid descending
                                 select new
                                 {
                                     header.Purchasing_Order_Number,
                                     header.Vendor_Account_Number,
                                     // vend.Vendor_Owner,
                                     header.Purchasing_Document_Date,
                                     header.path,
                                     header.ReleaseCode2,
                                     header.ReleaseCode2Status,
                                     header.ReleaseCode2Date,
                                     header.ReleaseCode3,
                                     header.ReleaseCode3Status,
                                     header.ReleaseCode3Date,
                                     header.ReleaseCode4,
                                     header.ReleaseCode4Status,
                                     header.ReleaseCode4Date,
                                     // fund.FundCenter_Description,
                                     header.Currency_Key,
                                     //appr.UniqueId,
                                     //appr.SequenceNumber,
                                     //appr.ReleaseCode,
                                     header.Uniqueid,
                                     header.PO_Title,
                                     header.SubmitterName,
                                     header.Managed_by

                                 }).Distinct().ToList();

                        PoCount = query.Count();
                        query = query.Skip((PageCount - 1) * 20).Take(20).ToList();

                    }
                    foreach (var item in query)
                    {
                        string releasestatus = "";
                        DateTime? releasedate = null;
                        releasestatus = Status;



                        double? TotalPrice = 0.00;
                        DateTime GSTDate = new DateTime(2017, 07, 03);
                        DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);
                        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid
                                                                                            && (i.Condition_Type != "NAVS"
                                                                                            && i.Condition_Type != "JICG"
                                                                                        && i.Condition_Type != "JISG"
                                                                                        && i.Condition_Type != "JICR"
                                                                                        && i.Condition_Type != "JISR"
                                                                                        && i.Condition_Type != "JIIR"
                                                                                        )
                                                                                        && (i.VendorCode == null
                                                                                            || i.VendorCode == ""
                                                                                            || i.VendorCode == item.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                            && i.IsDeleted == false).ToList();
                        //List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid && i.Condition_Type != "NAVS" && i.IsDeleted == false).ToList();

                        if (wiseList.Count > 0)
                        {
                            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                        }

                        List<string> WbsList = new List<string>();
                        List<string> WbsHeadsList = new List<string>();

                        var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                  ).ToList();


                        foreach (var IS in ItemStructure)
                        {
                            var WBSElementsList = new List<string>();

                            if (IS.Item_Category != "D")
                            {
                                WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                           && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                           .Select(x => x.WBS_Element).ToList();

                            }
                            else if (IS.Item_Category == "D" && IS.Material_Number == "")
                            {
                                WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                         .Select(x => x.WBS_Element).ToList();
                            }


                            foreach (var ele in WBSElementsList)
                            {
                                //getWbsElement(WbsList, ele, i);



                                int occ = ele.Count(x => x == '-');

                                if (occ >= 3)
                                {
                                    int index = CustomIndexOf(ele, '-', 3);
                                    // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                    string part = ele.Substring(0, index);
                                    WbsHeadsList.Add(part);
                                }
                                else
                                {
                                    WbsHeadsList.Add(ele);
                                }

                                string Element = ele;
                                char firstChar = Element[0];
                                if (firstChar == '0')
                                {

                                    WbsList.Add(Element.Substring(0, 4));

                                }
                                else if (firstChar == 'A')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'C')
                                {

                                    WbsList.Add(Element.Substring(0, 5));

                                }
                                else if (firstChar == 'M')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "MN")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }
                                    else if (twoChar == "MC")
                                    {

                                        WbsList.Add(Element.Substring(0, 4));

                                    }
                                }
                                else if (firstChar == 'Y')
                                {
                                    string twoChar = Element.Substring(0, 2);
                                    if (twoChar == "YS")
                                    {

                                        WbsList.Add(Element.Substring(0, 7));

                                    }

                                }
                                else if (firstChar == 'O')
                                {
                                    string threeChar = Element.Substring(0, 3);
                                    if (threeChar == "OB2")
                                    {

                                        WbsList.Add(Element);

                                    }

                                }
                            }

                        }


                        WbsList = WbsList.Distinct().ToList();
                        WbsHeadsList = WbsHeadsList.Distinct().ToList();

                        string WbsHeads = "";
                        foreach (var w in WbsHeadsList)
                        {
                            if (WbsHeads == "")
                            {
                                WbsHeads = w;
                            }
                            else
                            {
                                WbsHeads = WbsHeads + "," + w;
                            }
                        }
                        string ProjectCodes = "";
                        foreach (var w in WbsList)
                        {
                            if (ProjectCodes == "")
                            {
                                ProjectCodes = w;
                            }
                            else
                            {
                                ProjectCodes = ProjectCodes + "," + w;
                            }
                        }



                        result.Add(new TEPurchaseHomeModel()
                        {
                            Purchasing_Order_Number = item.Purchasing_Order_Number,
                            Vendor_Account_Number = item.Vendor_Account_Number,
                            Purchasing_Document_Date = item.Purchasing_Document_Date,
                            // FundCenter_Description = item.FundCenter_Description,
                            Path = item.path,
                            ReleaseCodeStatus = releasestatus,
                            Purchasing_Release_Date = releasedate,
                            Amount
                            =TotalPrice,                            
                            Currency_Key = item.Currency_Key,
                            HeaderUniqueid = item.Uniqueid,
                            PoCount = PoCount,
                            PoTitle = item.PO_Title,
                            VendorName = (vendor
                           .Where(x => x.Vendor_Code == item.Vendor_Account_Number)
                               .Select(x => x.Vendor_Owner).FirstOrDefault()),
                            Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber != 0).ToList(),
                            POStatus = item.ReleaseCode2Status,
                            ProjectCodes=ProjectCodes,
                            SubmitterName=item.SubmitterName,
                            WbsHeads = WbsHeads,
                            ManagerName=item.Managed_by
                        });
                    }

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

            //db.SaveChanges();
            return result.OrderBy(x => x.HeaderUniqueid).ToList();
        }

        [HttpPost]
        public void EmailTest(TEPurchaseAproveModel value)
        {
            var NextApproversList = (from u in db.TEPurchaseOrderApprovers
                                     join usr in db.UserProfiles on u.ApproverName equals usr.CallName
                                     where (u.PurchaseOrderNumber == value.Purchasing_Order_Number && (u.Status == "Approved" || u.Status == "Pending for Approval") && u.IsDeleted == false)
                                     orderby u.UniqueId

                                     select new
                                     {
                                         u.UniqueId,
                                         u.SequenceNumber,
                                         u.ApproverName,
                                         usr.UserId,
                                         u.Status
                                     }).ToList();
            string NotifApprover = "";
            if (NextApproversList.Count > 0)
            {
                foreach (var item in NextApproversList)
                {
                    UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();



                    if (item.Status == "Approved" && item.ApproverName != NotifApprover)
                    {
                        

                        //Email Code


                        if (item.ApproverName != value.AproverName)
                        {
                            var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POApprove").ToList();
                            if (Emails.Count > 0)
                            {
                                var Email = Emails.First();
                                Email.Subject = Email.Subject.Replace("$ApproverName", value.AproverName);
                                Email.Subject = Email.Subject.Replace("$PO", value.Purchasing_Order_Number);

                                Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", ToUser.CallName);
                                Email.EmailTemplate = Email.EmailTemplate.Replace("$ApproverName", value.AproverName);
                                Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                                Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", "Test");
                                SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                            }
                        }




                        NotifApprover = item.ApproverName;
                    }
                    else if (item.Status == "Pending for Approval")
                    {
 

                        var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmit").ToList();
                        if (Emails.Count > 0)
                        {
                            var Email = Emails.First();
                            Email.Subject = Email.Subject.Replace("$VendorName", "Test1");


                            Email.EmailTemplate = Email.EmailTemplate.Replace("$Employee", ToUser.CallName);
                            Email.EmailTemplate = Email.EmailTemplate.Replace("$POValue", "345");
                            Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber", value.Purchasing_Order_Number);
                            Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", "Test1");
                            SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);
                        }

                    }
                }
            }
        }

        [HttpGet]
        public IEnumerable<TEPurchase_Vendor> GetVendors(int PageCount)
        {
            return db.TEPurchase_Vendor.Where(x => x.IsDeleted==false)
                .OrderBy(x => x.Vendor_Code).Skip((PageCount - 1) * 20).Take(20).ToList();
             
        }

      

        [HttpGet]
        public TEPurchaseCountModel TEPurchaseOrdersCount(int UserId)
        {
            TEPurchaseCountModel result = new TEPurchaseCountModel();
            result.PendingCount = db.TEPurchaseOrderApprovers.Where(x => x.ApproverId == UserId && x.Status == "Pending for Approval" && x.IsDeleted==false).Count();
            result.ApprovedCount = db.TEPurchaseOrderApprovers.Where(x => x.ApproverId == UserId && x.Status == "Approved" && x.IsDeleted == false).Count();
            result.UpcomingCount = db.TEPurchaseOrderApprovers.Where(x => x.ApproverId == UserId && x.Status == "Draft" && x.IsDeleted == false).Count();
            result.RejectedCount = db.TEPurchaseOrderApprovers.Where(x => x.ApproverId == UserId && x.Status == "Rejected" && x.IsDeleted == false).Count();
            result.TotalCount = db.TEPurchaseOrderApprovers.Where(x => x.ApproverId == UserId && x.Status != "NULL" && x.IsDeleted == false).Count();
           
            return result;
        }


        #region Terms and Conditions

        // get master terms and conditions 
        [HttpGet]
        public IEnumerable<TEMasterTerm> GetMasterTermsAndConditions()
        {
            List<TEMasterTerm> result = new List<TEMasterTerm>();

            List<TEMasterTermsCondition> MasterList = db.TEMasterTermsConditions.Where(x => x.IsDeleted == false).ToList();

            foreach (var item in MasterList)
            {
                string TypeDescription="";
                if (item.Type != null)
                {
                    int iType = Convert.ToInt32(item.Type);
                    TypeDescription=db.TEPickListItems.Where(x => x.Uniqueid == iType && x.IsDeleted == false).Select(x => x.Description).FirstOrDefault();
                }
                
                result.Add(new TEMasterTerm()
                {
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    Condition = item.Condition,
                    // FundCenter_Description = item.FundCenter_Description,
                    IsActive = item.IsActive,
                    IsDeleted = item.IsDeleted,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedOn
                    = item.LastModifiedOn,
                    SequenceId = item.SequenceId,
                    Title = item.Title,
                    Type = item.Type,
                    UniqueId = item.UniqueId,
                    TypeDesc = TypeDescription
                   
                });
            }
            return result.OrderBy(x => x.SequenceId).ToList();

        }
        [HttpGet]
        public IEnumerable<TEMasterTerm> GetMasterTermsAndConditionsByType(string Type)
        {
            List<TEMasterTerm> result = new List<TEMasterTerm>();

            var MasterList = db.TEMasterTermsConditions.Where(x => x.IsDeleted == false).ToList();
            
           

            foreach (var item in MasterList)
            {
               
                string TypeDescription = "";
                if (item.Type != null)
                {

                    int iType = Convert.ToInt32(item.Type);
                    TypeDescription = db.TEPickListItems.Where(x => x.Uniqueid == iType && x.IsDeleted == false).Select(x => x.Description).FirstOrDefault();
                    
                }
                if (TypeDescription == Type)
                {
                    result.Add(new TEMasterTerm()
                    {
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        Condition = item.Condition,
                        // FundCenter_Description = item.FundCenter_Description,
                        IsActive = item.IsActive,
                        IsDeleted = item.IsDeleted,
                        LastModifiedBy = item.LastModifiedBy,
                        LastModifiedOn
                        = item.LastModifiedOn,
                        SequenceId = item.SequenceId,
                        Title = item.Title,
                        Type = item.Type,
                        UniqueId = item.UniqueId,
                        TypeDesc = TypeDescription

                    });
                }
            }
            return result.OrderBy(x => x.SequenceId).ToList();

        }
        // get master terms and condition by id
        [HttpGet]
        public TEMasterTermsCondition GetMasterTermsAndConditionById(int Id)
        {
            
            return db.TEMasterTermsConditions.Where(x => x.UniqueId == Id && x.IsDeleted == false).FirstOrDefault();
               
        }

        //create or update master terms and conditions
        [HttpPost]
        public TEMasterTermsCondition PostMasterTerms(TEMasterTermsCondition value)
        {
            TEMasterTermsCondition result = value;

            List<TEMasterTermsCondition> oldMaster = db.TEMasterTermsConditions.Where(x => x.IsDeleted == false && x.Type == result.Type).ToList();


            int maxsequence = 0;
            if (oldMaster.Count > 0)
            {
                maxsequence = oldMaster.Select(x => x.SequenceId).Max();
            }


            if (!(value.UniqueId + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result.ModuleName = "PO";
                result.SequenceId = maxsequence + 1;
                result = db.TEMasterTermsConditions.Add(value);
            }
            else
            {
                db = new TEHRIS_DevEntities();
                db.TEMasterTermsConditions.Attach(value);


                foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                {
                    string propname = item.Name;
                    if (propname.ToLower() == "createdon")
                        continue;
                    object propValue = item.GetValue(value);
                    if (propValue != null || Convert.ToString(propValue).Length != 0)
                        db.Entry(value).Property(propname).IsModified = true;
                }

                value.LastModifiedOn = System.DateTime.Now;
                db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
            }

            db.SaveChanges();

            return db.TEMasterTermsConditions.Find(value.UniqueId);
        }

        #endregion

        #region po terms and conditions
        // get  terms and conditions 
        [HttpGet]
        public IEnumerable<TEPOTerm> GetTermsAndConditions(string PONumber)
        {

            List<TEPOTerm> result = new List<TEPOTerm>();

            List<TETermsAndCondition> MasterList = db.TETermsAndConditions.Where(x => x.IsDeleted == false && x.ContextIdentifier == PONumber).ToList();

            foreach (var item in MasterList)
            {
                string TypeDescription = "";
                if (item.Type != null)
                {
                    int iType = Convert.ToInt32(item.Type);
                    TypeDescription = db.TEPickListItems.Where(x => x.Uniqueid == iType && x.IsDeleted == false).Select(x => x.Description).FirstOrDefault();
                }

                result.Add(new TEPOTerm()
                {
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    Condition = item.Condition,
                    // FundCenter_Description = item.FundCenter_Description,
                    IsActive = item.IsActive,
                    IsDeleted = item.IsDeleted,
                    LastModifiedBy = item.LastModifiedBy,
                    LastModifiedOn
                    = item.LastModifiedOn,
                    SequenceId = item.SequenceId,
                    Title = item.Title,
                    Type = item.Type,
                    UniqueId = item.UniqueId,
                    TypeDesc = TypeDescription,
                    MasterId=item.MasterId,
                    ContextIdentifier=item.ContextIdentifier

                });
            }
            return result.OrderBy(x=>x.SequenceId).ToList();
           


        }
        [HttpGet]
        public IEnumerable<TEPOTerm> GetTermsAndConditionsByType(string PONumber,string Type)
        {

            List<TEPOTerm> result = new List<TEPOTerm>();

            List<TETermsAndCondition> MasterList = db.TETermsAndConditions.Where(x => x.IsDeleted == false && x.ContextIdentifier == PONumber).ToList();

            foreach (var item in MasterList)
            {
                string TypeDescription = "";
                if (item.Type != null)
                {
                    int iType = Convert.ToInt32(item.Type);
                    TypeDescription = db.TEPickListItems.Where(x => x.Uniqueid == iType && x.IsDeleted == false).Select(x => x.Description).FirstOrDefault();
                }
                if (TypeDescription == Type)
                {
                    result.Add(new TEPOTerm()
                    {
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        Condition = item.Condition,
                        // FundCenter_Description = item.FundCenter_Description,
                        IsActive = item.IsActive,
                        IsDeleted = item.IsDeleted,
                        LastModifiedBy = item.LastModifiedBy,
                        LastModifiedOn
                        = item.LastModifiedOn,
                        SequenceId = item.SequenceId,
                        Title = item.Title,
                        Type = item.Type,
                        UniqueId = item.UniqueId,
                        TypeDesc = TypeDescription,
                        MasterId = item.MasterId,
                        ContextIdentifier = item.ContextIdentifier

                    });
                }
            }
            return result.OrderBy(x => x.SequenceId).ToList();



        }
        // get  terms and condition by id
        [HttpGet]
        public TETermsAndCondition GetTermsAndConditionById(int Id)
        {

            return db.TETermsAndConditions.Where(x => x.UniqueId == Id && x.IsDeleted == false).FirstOrDefault();

        }

        // create or update   terms and condition
        [HttpPost]
        public TETermsAndCondition PostTermsAndConditions(TETermsAndCondition value)
        {
            TETermsAndCondition result = value;
            List<TETermsAndCondition> oldTerms = db.TETermsAndConditions.Where(x => x.IsDeleted == false && x.Type == result.Type && x.ContextIdentifier==value.ContextIdentifier).ToList();


            //int maxsequence = 0;
            //if (oldTerms.Count > 0)
            //{
            //    maxsequence = oldTerms.Select(x => x.SequenceId).Max();
            //    maxsequence = maxsequence + 1;
            //}

           
            
            
            if (!(value.UniqueId + "".Length > 0))
            {
                int maxsequence = 0;
                if (oldTerms.Count > 0)
                {
                    maxsequence = oldTerms.Select(x => x.SequenceId).Max();
                    maxsequence = maxsequence + 1;
                }


                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
               

                result.SequenceId = maxsequence + 1;
                result.ModuleName = "PO";
                result = db.TETermsAndConditions.Add(value);
            }
            else
            {
                db = new TEHRIS_DevEntities();
                db.TETermsAndConditions.Attach(value);

               
                foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                {
                    string propname = item.Name;
                    if (propname.ToLower() == "createdon")
                        continue;
                    object propValue = item.GetValue(value);
                    if (propValue != null || Convert.ToString(propValue).Length != 0)
                        db.Entry(value).Property(propname).IsModified = true;
                }

                value.LastModifiedOn = System.DateTime.Now;
                db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
            }

            db.SaveChanges();

            return db.TETermsAndConditions.Find(value.UniqueId);
        }
        #endregion

        #region po terms and coditions
        // get  payment milestones 
        [HttpGet]
        public IEnumerable<TEVendorPaymentMilestone> GetPOMilestones(string PONumber)
        {
            return db.TEVendorPaymentMilestones.Where(x => x.IsDeleted == false && x.ContextIdentifier == PONumber)
                .OrderBy(x => x.CreatedOn).ToList();


        }

        // get payment milestones  by id
        [HttpGet]
        public TEVendorPaymentMilestone GetPOMilestonesById(int Id)
        {

            return db.TEVendorPaymentMilestones.Where(x => x.UniqueId == Id && x.IsDeleted == false).FirstOrDefault();

        }

        // create or update   payment milestones 
        [HttpPost]
        public TEVendorPaymentMilestone PostPOMilestones(TEVendorPaymentMilestone value)
        {
            TEVendorPaymentMilestone result = value;

           

            if (!(value.UniqueId + "".Length > 0))
            {
                result.CreatedOn = System.DateTime.Now;
                result.LastModifiedOn = System.DateTime.Now;
                result.ModuleName = "PO";
                result = db.TEVendorPaymentMilestones.Add(value);
            }
            else
            {
                db = new TEHRIS_DevEntities();
                db.TEVendorPaymentMilestones.Attach(value);


                foreach (System.Reflection.PropertyInfo item in result.GetType().GetProperties())
                {
                    string propname = item.Name;
                    if (propname.ToLower() == "createdon")
                        continue;
                    object propValue = item.GetValue(value);
                    if (propValue != null || Convert.ToString(propValue).Length != 0)
                        db.Entry(value).Property(propname).IsModified = true;
                }

                value.LastModifiedOn = System.DateTime.Now;
                db.Entry(value).Property(x => x.LastModifiedOn).IsModified = true;
            }

            db.SaveChanges();

            //int Poid = Convert.ToInt32(result.ContextIdentifier);
            //List<TEPurchaseOrderApprover> AprList = db.TEPurchaseOrderApprovers.Where(x => x.IsDeleted == false && x.POStructureId == Poid).ToList();
            //if (AprList != null)
            //{
            //    foreach (TEPurchaseOrderApprover item in AprList)
            //    {
            //        db = new TEHRIS_DevEntities();
            //        db.TEPurchaseOrderApprovers.Attach(item);

            //        item.IsDeleted = true;
            //        db.Entry(item).Property(x => x.IsDeleted).IsModified = true;

            //        item.LastModifiedOn = System.DateTime.Now;
            //        db.Entry(item).Property(x => x.LastModifiedOn).IsModified = true;

            //        item.LastModifiedBy = result.LastModifiedBy;
            //        db.Entry(item).Property(x => x.LastModifiedBy).IsModified = true;
                   
            //        db.SaveChanges();



                    
            //    }

              


            //    db = new TEHRIS_DevEntities();

            //    TEPurchase_header_structure POStructure = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == Poid).FirstOrDefault();
            //    db.TEPurchase_header_structure.Attach(POStructure);

            //    POStructure.ReleaseCode2Status = "Draft";
            //    db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

            //    POStructure.LastModifiedOn = Convert.ToString(System.DateTime.Now);
            //    db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;

            //    POStructure.LastModifiedBy = result.LastModifiedBy;
            //    db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

            //    db.SaveChanges();
            //}

            return db.TEVendorPaymentMilestones.Find(value.UniqueId);
        }
        #endregion

       
        #region Finalize PO list 
        [HttpGet]
        public IEnumerable<TEPurchaseHomeModel> TEPurchaseDraftList_old(int UserId, int PageCount, string FilterBy)
        {
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();



            List<TEPurchase_header_structure> DraftPoList = new List<TEPurchase_header_structure>();



            DraftPoList = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && (x.ReleaseCode2Status == "Draft" || x.ReleaseCode2Status == "Pending for Approval" || x.ReleaseCode2Status == "Approved")).ToList();


            if (FilterBy != null)
            {

                DraftPoList = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && (x.ReleaseCode2Status == "Draft" || x.ReleaseCode2Status == "Pending for Approval" || x.ReleaseCode2Status == "Approved") && (x.Purchasing_Order_Number.Contains(FilterBy) || db.TEPurchase_Vendor.Where(v => v.IsDeleted == false && v.Vendor_Code == x.Vendor_Account_Number).Select(v => v.Vendor_Owner).Contains(FilterBy) || x.PO_Title.Contains(FilterBy) || x.Managed_by.Contains(FilterBy))).ToList();
            }
            
            //var DraftPoList=(from h in db.TEPurchase_header_structure 
            //                     join i in db.TEPurchase_OrderTypes on h.Purchasing_Document_Type equals i.Code
            //                     join w in db.TEPurchase_Itemwise )

            int PoCount = 0;
            UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
                string user1 = "";
                    user1= "Approver";
                    foreach (var item in profile.webpages_Roles)
                    {
                        if (item.RoleName.Equals("PO  Approval Admin"))
                        {
                            user1 = "PO  Approval Admin";
                            break;
                        }
                    }
                if (user1 == "PO  Approval Admin")
                {
                    DraftPoList = DraftPoList.Skip((PageCount - 1) * 10).Take(10).ToList();
                }
                
                    foreach (var item in DraftPoList)
                    {
                        double TotalPrice = 0.00;

                      
                        List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid && i.Condition_Type != "NAVS" && i.IsDeleted == false).ToList();

                        if (wiseList.Count > 0)
                        {
                            TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                            TotalPrice = Math.Round(TotalPrice);
                            TotalPrice = Math.Truncate(TotalPrice);
                        }

                        TEPurchase_Vendor Vendor = db.TEPurchase_Vendor.Where(x => x.IsDeleted == false && x.Vendor_Code == item.Vendor_Account_Number).FirstOrDefault();
                        string vendor_owner = "";
                        if (Vendor != null)
                        {
                            vendor_owner = Vendor.Vendor_Owner;
                        }

                        
                          
                  if (user1 == "PO  Approval Admin")
                  {
                      

                      List<string> WbsList = new List<string>();
                      List<string> WbsHeadsList = new List<string>();

                      var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                              ).ToList();


                      foreach (var IS in ItemStructure)
                      {
                          var WBSElementsList = new List<string>();

                          if (IS.Item_Category != "D")
                          {
                              WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                         && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                         .Select(x => x.WBS_Element).ToList();


                          }
                          else if (IS.Item_Category == "D" && IS.Material_Number == "")
                          {
                              WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                       && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                       .Select(x => x.WBS_Element).ToList();

                          }
                          foreach (var ele in WBSElementsList)
                          {
                              //getWbsElement(WbsList, ele, i);



                              int occ = ele.Count(x => x == '-');

                              if (occ >= 3)
                              {
                                  int index = CustomIndexOf(ele, '-', 3);
                                  // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                  string part = ele.Substring(0, index);
                                  WbsHeadsList.Add(part);
                              }
                              else
                              {
                                  WbsHeadsList.Add(ele);
                              }

                              string Element = ele;
                              char firstChar = Element[0];
                              if (firstChar == '0')
                              {

                                  WbsList.Add(Element.Substring(0, 4));

                              }
                              else if (firstChar == 'A')
                              {

                                  WbsList.Add(Element.Substring(0, 5));

                              }
                              else if (firstChar == 'C')
                              {

                                  WbsList.Add(Element.Substring(0, 5));

                              }
                              else if (firstChar == 'M')
                              {
                                  string twoChar = Element.Substring(0, 2);
                                  if (twoChar == "MN")
                                  {

                                      WbsList.Add(Element.Substring(0, 7));

                                  }
                                  else if (twoChar == "MC")
                                  {

                                      WbsList.Add(Element.Substring(0, 4));

                                  }
                              }
                              else if (firstChar == 'Y')
                              {
                                  string twoChar = Element.Substring(0, 2);
                                  if (twoChar == "YS")
                                  {

                                      WbsList.Add(Element.Substring(0, 7));

                                  }

                              }
                              else if (firstChar == 'O')
                              {
                                  string threeChar = Element.Substring(0, 3);
                                  if (threeChar == "OB2")
                                  {

                                      WbsList.Add(Element);

                                  }

                              }
                          }
                      }

                      WbsList = WbsList.Distinct().ToList();
                      WbsHeadsList = WbsHeadsList.Distinct().ToList();

                      string WbsHeads = "";
                      foreach (var w in WbsHeadsList)
                      {
                          if (WbsHeads == "")
                          {
                              WbsHeads = w;
                          }
                          else
                          {
                              WbsHeads = WbsHeads + "," + w;
                          }
                      }

                      string ProjectCodes = "";
                      foreach (var w in WbsList)
                      {
                          if (ProjectCodes == "")
                          {
                              ProjectCodes = w;
                          }
                          else
                          {
                              ProjectCodes = ProjectCodes + "," + w;
                          }
                      }



                      PoCount = PoCount + 1;
                      result.Add(new TEPurchaseHomeModel()
                      {
                          Purchasing_Order_Number = item.Purchasing_Order_Number,
                          Vendor_Account_Number = item.Vendor_Account_Number,
                          Purchasing_Document_Date = item.Purchasing_Document_Date,
                          Path = item.path,
                          ReleaseCodeStatus = item.ReleaseCode2Status,
                          Amount
                          = TotalPrice,
                          Currency_Key = item.Currency_Key,
                          HeaderUniqueid = item.Uniqueid,
                          PoCount = PoCount,
                          PoTitle = item.PO_Title,
                          VendorName = vendor_owner,
                          Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                          POStatus = item.ReleaseCode2Status,
                          ProjectCodes = ProjectCodes,
                          SubmitterName = item.SubmitterName,
                          WbsHeads = WbsHeads

                      });
                  }
                  else
                  {
                      string FundCenter = "";
                        TEPurchase_Item_Structure itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == item.Uniqueid && x.Item_Category=="D").FirstOrDefault();

                        if (itemStructure != null)
                        {
                            TEPurchase_Service service= db.TEPurchase_Service.Where(x => x.IsDeleted == false && x.POStructureId == item.Uniqueid
                                 && x.Item_Number == itemStructure.Item_Number).FirstOrDefault();

                            if (service!=null)
                            {
                                FundCenter = service.Fund_Center;
                            }

                        }
                        else
                        {
                            itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == item.Uniqueid).FirstOrDefault();
                            if (itemStructure != null)
                            {
                                TEPurchase_Assignment assign = db.TEPurchase_Assignment.Where(x => x.IsDeleted == false && x.POStructureId == item.Uniqueid
                                    && x.ItemNumber == itemStructure.Item_Number).FirstOrDefault();

                                if (assign != null)
                                {
                                    FundCenter = assign.Fund_Center;
                                }
                            }
                        }
                       

                        TEPurchase_FundCenter Fund = db.TEPurchase_FundCenter.Where(x => x.IsDeleted == false && x.FundCenter_Code == FundCenter).FirstOrDefault();
                        //TEPurchasingGroup PGroup = db.TEPurchasingGroups.Where(x => x.IsDeleted == false && x.Code == item.Purchasing_Group).FirstOrDefault();
                        TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.Code == item.Purchasing_Document_Type).FirstOrDefault();

                        if (Fund != null && POrderType != null)
                        {
                            TEPOApprovalCondition AppCon = db.TEPOApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == Fund.Uniqueid
                                 && x.OrderType == POrderType.UniqueId && TotalPrice >= x.MinAmount && TotalPrice <= x.MaxAmount).FirstOrDefault();


                            if (AppCon != null)
                            {
                                List<TEPOMasterApprover> MasterApprovers = db.TEPOMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == AppCon.UniqueId && x.Type == "Submitter").ToList();

                                bool has = MasterApprovers.Any(cus => cus.ApproverId == UserId);

                                if (has)
                                {
                                    List<string> WbsList = new List<string>();
                                    List<string> WbsHeadsList = new List<string>();

                                    var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                                            ).ToList();


                                    foreach (var IS in ItemStructure)
                                    {
                                        var WBSElementsList = new List<string>();

                                        if (IS.Item_Category != "D")
                                        {
                                            WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                                       && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                                       .Select(x => x.WBS_Element).ToList();


                                        }
                                        else if (IS.Item_Category == "D" && IS.Material_Number == "")
                                        {
                                            WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                                     && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                                     .Select(x => x.WBS_Element).ToList();

                                        }
                                        foreach (var ele in WBSElementsList)
                                        {
                                            //getWbsElement(WbsList, ele, i);



                                            int occ = ele.Count(x => x == '-');

                                            if (occ >= 3)
                                            {
                                                int index = CustomIndexOf(ele, '-', 3);
                                                // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                                string part = ele.Substring(0, index);
                                                WbsHeadsList.Add(part);
                                            }
                                            else
                                            {
                                                WbsHeadsList.Add(ele);
                                            }

                                            string Element = ele;
                                            char firstChar = Element[0];
                                            if (firstChar == '0')
                                            {

                                                WbsList.Add(Element.Substring(0, 4));

                                            }
                                            else if (firstChar == 'A')
                                            {

                                                WbsList.Add(Element.Substring(0, 5));

                                            }
                                            else if (firstChar == 'C')
                                            {

                                                WbsList.Add(Element.Substring(0, 5));

                                            }
                                            else if (firstChar == 'M')
                                            {
                                                string twoChar = Element.Substring(0, 2);
                                                if (twoChar == "MN")
                                                {

                                                    WbsList.Add(Element.Substring(0, 7));

                                                }
                                                else if (twoChar == "MC")
                                                {

                                                    WbsList.Add(Element.Substring(0, 4));

                                                }
                                            }
                                            else if (firstChar == 'Y')
                                            {
                                                string twoChar = Element.Substring(0, 2);
                                                if (twoChar == "YS")
                                                {

                                                    WbsList.Add(Element.Substring(0, 7));

                                                }

                                            }
                                            else if (firstChar == 'O')
                                            {
                                                string threeChar = Element.Substring(0, 3);
                                                if (threeChar == "OB2")
                                                {

                                                    WbsList.Add(Element);

                                                }

                                            }
                                        }
                                    }

                                    WbsList = WbsList.Distinct().ToList();
                                    WbsHeadsList = WbsHeadsList.Distinct().ToList();

                                    string WbsHeads = "";
                                    foreach (var w in WbsHeadsList)
                                    {
                                        if (WbsHeads == "")
                                        {
                                            WbsHeads = w;
                                        }
                                        else
                                        {
                                            WbsHeads = WbsHeads + "," + w;
                                        }
                                    }

                                    string ProjectCodes = "";
                                    foreach (var w in WbsList)
                                    {
                                        if (ProjectCodes == "")
                                        {
                                            ProjectCodes = w;
                                        }
                                        else
                                        {
                                            ProjectCodes = ProjectCodes + "," + w;
                                        }
                                    }


                                    PoCount = PoCount + 1;
                                    result.Add(new TEPurchaseHomeModel()
                                    {
                                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                                        Vendor_Account_Number = item.Vendor_Account_Number,
                                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                                        Path = item.path,
                                        ReleaseCodeStatus = item.ReleaseCode2Status,
                                        Amount
                                        = TotalPrice,
                                        Currency_Key = item.Currency_Key,
                                        HeaderUniqueid = item.Uniqueid,
                                        PoCount = PoCount,
                                        PoTitle = item.PO_Title,
                                        VendorName = vendor_owner,
                                        Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false).ToList(),
                                        POStatus = item.ReleaseCode2Status,
                                        ProjectCodes = ProjectCodes,
                                        SubmitterName = item.SubmitterName,
                                        WbsHeads = WbsHeads

                                    });
                                }
                            }
                        }
                       }
                            
                    
                }
            result = result.Skip((PageCount - 1) * 10).Take(10).ToList();
            return result;
        }
        #endregion

        [HttpGet]
        public IEnumerable<TEPurchaseHomeModel> TEPurchaseDraftList(int UserId, int PageCount, string FilterBy)
        {
            List<TEPurchaseHomeModel> result = new List<TEPurchaseHomeModel>();


            int PoCount = 0;
            UserProfile profile = db.UserProfiles.Where(x => x.UserId == UserId).First();
            string user1 = "";
            user1 = "Approver";
            foreach (var item in profile.webpages_Roles)
            {
                if (item.RoleName.Equals("PO  Approval Admin"))
                {
                    user1 = "PO  Approval Admin";
                    break;
                }
            }


            var query = (from header in db.TEPurchase_header_structure
                         join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId

                         where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))
                        
                         select new
                         {
                             header.Purchasing_Order_Number,
                             header.Vendor_Account_Number,
                             // vend.Vendor_Owner,
                             header.Purchasing_Document_Date,
                             header.path,
                             header.ReleaseCode2Status,
                             header.Currency_Key,
                             header.Uniqueid,
                             header.PO_Title,
                             header.SubmitterName

                         }).Distinct().OrderByDescending(x=>x.Uniqueid).ToList();

            PoCount = query.Count();
            query = query.Skip((PageCount - 1) * 10).Take(10).ToList();

            if (user1 == "PO  Approval Admin")
            {
                 query = (from header in db.TEPurchase_header_structure
                          where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))
                           
                             select new
                             {
                                 header.Purchasing_Order_Number,
                                 header.Vendor_Account_Number,
                                 // vend.Vendor_Owner,
                                 header.Purchasing_Document_Date,
                                 header.path,
                                 header.ReleaseCode2Status,                             
                                 header.Currency_Key,                                
                                 header.Uniqueid,
                                 header.PO_Title,
                                 header.SubmitterName

                             }).Distinct().OrderByDescending(x => x.Uniqueid).ToList();


                 if (FilterBy != null)
                 {
                     query = (from header in db.TEPurchase_header_structure
                              join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                              from v in ps.DefaultIfEmpty()
                              where (header.IsDeleted == false && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))
                             
                              select new
                              {
                                  header.Purchasing_Order_Number,
                                  header.Vendor_Account_Number,
                                  // vend.Vendor_Owner,
                                  header.Purchasing_Document_Date,
                                  header.path,                                
                                  header.ReleaseCode2Status,                               
                                  header.Currency_Key,                                
                                  header.Uniqueid,
                                  header.PO_Title,
                                  header.SubmitterName

                              }).Distinct().OrderByDescending(x => x.Uniqueid).ToList();
                 }


                PoCount = query.Count();
                query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
            }
            else
            {



                 query = (from header in db.TEPurchase_header_structure
                             join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId

                          where (appr.ApproverId == UserId && appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected"))
                           
                             select new
                             {
                                 header.Purchasing_Order_Number,
                                 header.Vendor_Account_Number,
                                 // vend.Vendor_Owner,
                                 header.Purchasing_Document_Date,
                                 header.path,                                
                                 header.ReleaseCode2Status,                              
                                 header.Currency_Key,                              
                                 header.Uniqueid,
                                 header.PO_Title,
                                 header.SubmitterName

                             }).Distinct().OrderByDescending(x => x.Uniqueid).ToList();


                 if (FilterBy != null)
                 {
                     query = (from header in db.TEPurchase_header_structure
                              join appr in db.TEPurchaseOrderApprovers on header.Uniqueid equals appr.POStructureId
                              join v in db.TEPurchase_Vendor on header.Vendor_Account_Number equals v.Vendor_Code into ps
                              from v in ps.DefaultIfEmpty()
                              where (appr.IsDeleted == false && header.IsDeleted == false && appr.SequenceNumber == 0 && (header.ReleaseCode2Status == "Draft" || header.ReleaseCode2Status == "Pending for Approval" || header.ReleaseCode2Status == "Approved" || header.ReleaseCode2Status == "Rejected") && (header.Purchasing_Order_Number.Contains(FilterBy) || v.Vendor_Owner.Contains(FilterBy) || header.PO_Title.Contains(FilterBy) || header.Managed_by.Contains(FilterBy)))
                            
                              select new
                              {
                                  header.Purchasing_Order_Number,
                                  header.Vendor_Account_Number,
                                  // vend.Vendor_Owner,
                                  header.Purchasing_Document_Date,
                                  header.path,
                                  header.ReleaseCode2Status,
                                  header.Currency_Key,
                                  header.Uniqueid,
                                  header.PO_Title,
                                  header.SubmitterName

                              }).Distinct().OrderByDescending(x => x.Uniqueid).ToList();
                 }


                PoCount = query.Count();
                query = query.Skip((PageCount - 1) * 10).Take(10).ToList();
            }

                foreach (var item in query)
                {
                    double TotalPrice = 0.00;
                    DateTime GSTDate = new DateTime(2017, 07, 03);
                    DateTime postingDate = Convert.ToDateTime(item.Purchasing_Document_Date);

                    List<TEPurchase_Itemwise> wiseList = db.TEPurchase_Itemwise.Where(i => i.POStructureId == item.Uniqueid 
                                                                                        && (i.Condition_Type != "NAVS"
                                                                                        && i.Condition_Type != "JICG"
                                                                                        && i.Condition_Type != "JISG"
                                                                                        && i.Condition_Type != "JICR"
                                                                                        && i.Condition_Type != "JISR"
                                                                                        && i.Condition_Type != "JIIR"
                                                                                        )
                                                                                        && (i.VendorCode == null
                                                                                            || i.VendorCode == ""
                                                                                            || i.VendorCode == item.Vendor_Account_Number
                                                                                            || postingDate <= GSTDate
                                                                                            )
                                                                                        && i.IsDeleted == false).ToList();

                    if (wiseList.Count > 0)
                    {
                        TotalPrice = wiseList.Sum(x => x.Condition_rate.Value);
                        TotalPrice = Math.Round(TotalPrice);
                        TotalPrice = Math.Truncate(TotalPrice);
                    }

                    TEPurchase_Vendor Vendor = db.TEPurchase_Vendor.Where(x => x.IsDeleted == false && x.Vendor_Code == item.Vendor_Account_Number).FirstOrDefault();
                    string vendor_owner = "";
                    if (Vendor != null)
                    {
                        vendor_owner = Vendor.Vendor_Owner;
                    }

                    List<string> WbsList = new List<string>();
                    List<string> WbsHeadsList = new List<string>();

                    var ItemStructure = db.TEPurchase_Item_Structure.Where(x => (x.POStructureId == item.Uniqueid && x.IsDeleted == false)
                                                            ).ToList();


                    foreach (var IS in ItemStructure)
                    {
                        var WBSElementsList = new List<string>();

                        if (IS.Item_Category != "D")
                        {
                            WBSElementsList = db.TEPurchase_Assignment.Where(x => (x.POStructureId == item.Uniqueid)
                                                       && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.ItemNumber == IS.Item_Number)
                                       .Select(x => x.WBS_Element).ToList();


                        }
                        else if (IS.Item_Category == "D" && IS.Material_Number == "")
                        {
                            WBSElementsList = db.TEPurchase_Service.Where(x => (x.POStructureId == item.Uniqueid)
                                                     && x.IsDeleted == false && (x.WBS_Element != null && x.WBS_Element != "") && x.Item_Number == IS.Item_Number)
                                     .Select(x => x.WBS_Element).ToList();

                        }
                        foreach (var ele in WBSElementsList)
                        {
                            //getWbsElement(WbsList, ele, i);



                            int occ = ele.Count(x => x == '-');

                            if (occ >= 3)
                            {
                                int index = CustomIndexOf(ele, '-', 3);
                                // int index = ele.IndexOf('-', ele.IndexOf('-') + 2);
                                string part = ele.Substring(0, index);
                                WbsHeadsList.Add(part);
                            }
                            else
                            {
                                WbsHeadsList.Add(ele);
                            }

                            string Element = ele;
                            char firstChar = Element[0];
                            if (firstChar == '0')
                            {

                                WbsList.Add(Element.Substring(0, 4));

                            }
                            else if (firstChar == 'A')
                            {

                                WbsList.Add(Element.Substring(0, 5));

                            }
                            else if (firstChar == 'C')
                            {

                                WbsList.Add(Element.Substring(0, 5));

                            }
                            else if (firstChar == 'M')
                            {
                                string twoChar = Element.Substring(0, 2);
                                if (twoChar == "MN")
                                {

                                    WbsList.Add(Element.Substring(0, 7));

                                }
                                else if (twoChar == "MC")
                                {

                                    WbsList.Add(Element.Substring(0, 4));

                                }
                            }
                            else if (firstChar == 'Y')
                            {
                                string twoChar = Element.Substring(0, 2);
                                if (twoChar == "YS")
                                {

                                    WbsList.Add(Element.Substring(0, 7));

                                }

                            }
                            else if (firstChar == 'O')
                            {
                                string threeChar = Element.Substring(0, 3);
                                if (threeChar == "OB2")
                                {

                                    WbsList.Add(Element);

                                }

                            }
                        }
                    }

                    WbsList = WbsList.Distinct().ToList();
                    WbsHeadsList = WbsHeadsList.Distinct().ToList();

                    string WbsHeads = "";
                    foreach (var w in WbsHeadsList)
                    {
                        if (WbsHeads == "")
                        {
                            WbsHeads = w;
                        }
                        else
                        {
                            WbsHeads = WbsHeads + "," + w;
                        }
                    }

                    string ProjectCodes = "";
                    foreach (var w in WbsList)
                    {
                        if (ProjectCodes == "")
                        {
                            ProjectCodes = w;
                        }
                        else
                        {
                            ProjectCodes = ProjectCodes + "," + w;
                        }
                    }



                    PoCount = PoCount + 1;
                    result.Add(new TEPurchaseHomeModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Vendor_Account_Number = item.Vendor_Account_Number,
                        Purchasing_Document_Date = item.Purchasing_Document_Date,
                        Path = item.path,
                        ReleaseCodeStatus = item.ReleaseCode2Status,
                        Amount
                        = TotalPrice,
                        Currency_Key = item.Currency_Key,
                        HeaderUniqueid = item.Uniqueid,
                        PoCount = PoCount,
                        PoTitle = item.PO_Title,
                        VendorName = vendor_owner,
                        Approvers = db.TEPurchaseOrderApprovers.Where(x => x.POStructureId == item.Uniqueid && x.IsDeleted == false && x.SequenceNumber!=0).ToList(),
                        POStatus = item.ReleaseCode2Status,
                        ProjectCodes = ProjectCodes,
                        SubmitterName = item.SubmitterName,
                        WbsHeads = WbsHeads

                    });           
            
            }


            return result;

        }



        #region po submit for approval and creating approvers



        [HttpPost]
        public string POSubmitforApproval(Submitforapprovalreq requestObj)
        {
             int POUniqueId =requestObj.POUniqueId;
            string PurchaseOrderNumber =requestObj.PurchaseOrderNumber;
            string SubmitterComments =requestObj.SubmitterComments;
            int UserId = requestObj.UserId;
            string shipTo =requestObj.shipTo;
            string response = "";
            try
            {
                db = new TEHRIS_DevEntities();

                TEPurchase_header_structure POStructure = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();

                UserProfile user = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == UserId).FirstOrDefault();

                db.TEPurchase_header_structure.Attach(POStructure);

                POStructure.ReleaseCode2Status = "Pending for Approval";
                db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                POStructure.LastModifiedOn = Convert.ToString(System.DateTime.Now);
                db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;

                POStructure.LastModifiedBy = user.UserName;
                db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                POStructure.SubmittedBy = UserId;
                db.Entry(POStructure).Property(x => x.SubmittedBy).IsModified = true;

                POStructure.SubmitterComments = SubmitterComments;
                db.Entry(POStructure).Property(x => x.SubmitterComments).IsModified = true;

                POStructure.ShipTpCode = shipTo;
                db.Entry(POStructure).Property(x => x.ShipTpCode).IsModified = true;
                

                // approvers code

                
                string FundCenter = "";
                TEPurchase_Item_Structure itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && x.Item_Category == "D").FirstOrDefault();

                if (itemStructure != null)
                {
                    FundCenter = db.TEPurchase_Service.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                         && x.Item_Number == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();

                }
                else
                {
                    itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).FirstOrDefault();

                    FundCenter = db.TEPurchase_Assignment.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId
                        && x.ItemNumber == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();
                }
                
                double TotalPrice = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == POUniqueId && (q.Condition_Type != "NAVS")
                              ).Sum(q => q.Condition_rate).Value;

                TotalPrice = TotalPrice * Convert.ToDouble(POStructure.Exchange_Rate);
                TotalPrice = Math.Round(TotalPrice);
                TotalPrice = Math.Truncate(TotalPrice);
                if (requestObj.annexureModel != null)
                {
                    foreach (TEPurchaseItemListAnnexureModel item in requestObj.annexureModel)
                    {
                        //Annexure other charges
                        TEPurchase_Annexure_OtherCharges Annexure = new TEPurchase_Annexure_OtherCharges();
                        Annexure.CreatedBy = user.UserName;
                        Annexure.CreatedOn = System.DateTime.Today.ToString();
                        Annexure.LastModifiedBy = user.UserName;
                        Annexure.LastModifiedOn = System.DateTime.Today.ToString();
                        Annexure.IsDeleted = false;
                        Annexure.POStructureId = requestObj.POUniqueId;
                        Annexure.PurchasingOrderNumber = itemStructure.Purchasing_Order_Number;
                        Annexure.PackingForwardingCondition = item.PackingForwardingCondition;
                        Annexure.PackingForwardingValue = item.PackingForwardingValue;
                        Annexure.FreightCondition = item.FreightCondition;
                        Annexure.FreightValue = item.FreightValue;
                        Annexure.OtherServicesCondition = item.OtherServicesCondition;
                        Annexure.OtherServicesType = item.OtherServicesType;
                        Annexure.OtherServicesValue = item.OtherServicesValue;
                        db.TEPurchase_Annexure_OtherCharges.Add(Annexure);
                    }
                }
                TEPurchase_FundCenter Fund = db.TEPurchase_FundCenter.Where(x => x.IsDeleted == false && x.FundCenter_Code == FundCenter).FirstOrDefault();
                //TEPurchasingGroup PGroup = db.TEPurchasingGroups.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Group).FirstOrDefault();
                TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Document_Type).FirstOrDefault();

                TEPOApprovalCondition AppCon = db.TEPOApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == Fund.Uniqueid
                     && x.OrderType == POrderType.UniqueId && TotalPrice >= x.MinAmount && TotalPrice<=x.MaxAmount).FirstOrDefault();

                if (AppCon != null)
                {
                    List<TEPOMasterApprover> MasterApprovers = db.TEPOMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == AppCon.UniqueId && x.Type == "Approver").OrderBy(x => x.SequenceId).ToList();

                    int count = 0;

                    foreach (var Appr in MasterApprovers)
                    {
                        TEPurchaseOrderApprover result = new TEPurchaseOrderApprover();

                        count = count + 1;

                        string AprName = "Not Available";

                        result.CreatedOn = System.DateTime.Now;
                        result.LastModifiedOn = System.DateTime.Now;
                        result.CreatedBy = user.UserName;
                        result.LastModifiedBy = user.UserName;
                        result.SequenceNumber = Convert.ToInt32(Appr.SequenceId);
                        result.PurchaseOrderNumber = POStructure.Purchasing_Order_Number;

                        AprName = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == Appr.ApproverId).Select(x => x.CallName).FirstOrDefault();

                        if (AprName == null)
                        {
                            result.ApproverName = "Not Available";
                        }
                        else
                        {
                            result.ApproverName = AprName;
                        }
                        if (Appr.SequenceId == 1)
                        {
                            result.Status = "Pending for Approval";
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
                        db.TEPurchaseOrderApprovers.Add(result);
                        
                       
                        
                        db.SaveChanges();
                    }


                    try
                    {




                        

                        string VendorName = (db.TEPurchase_Vendor
                                                  .Where(x => x.Vendor_Code == POStructure.Vendor_Account_Number)
                                                  .Select(x => x.Vendor_Owner).FirstOrDefault());

                        var NextApproversList = (from u in db.TEPurchaseOrderApprovers
                                                 join usr in db.UserProfiles on u.ApproverId equals usr.UserId
                                                 where (u.POStructureId == POStructure.Uniqueid && u.IsDeleted == false && u.Status == "Pending for Approval" && u.SequenceNumber != 0)
                                                 orderby u.UniqueId

                                                 select new
                                                 {
                                                     u.UniqueId,
                                                     u.SequenceNumber,
                                                     u.ApproverName,
                                                     usr.UserId,
                                                     u.Status
                                                 }).ToList();

                        if (NextApproversList.Count > 0)
                        {
                            foreach (var item in NextApproversList)
                            {
                                UserProfile ToUser = db.UserProfiles.Where(x => x.UserId == item.UserId).FirstOrDefault();
                                UserProfile Submitter = db.UserProfiles.Where(x => x.UserId == POStructure.SubmittedBy).FirstOrDefault();
                                TEEmpBasicInfo emp = db.TEEmpBasicInfoes.Where(x => x.UserId == Submitter.UserName && x.IsDeleted == false).FirstOrDefault();
                                var Emails = db.TEEmailTemplates.Where(x => x.ModuleName == "POSubmit").ToList();
                                if (Emails.Count > 0)
                                {


                                    TEEmailTemplate Email = new TEEmailTemplate();

                                    Email.Subject = Emails[0].Subject.Replace("$VendorName", VendorName);
                                    Email.EmailTemplate = Emails[0].EmailTemplate.Replace("$Employee", ToUser.CallName);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$POValue", TotalPrice.ToString());
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$PONumber ", POStructure.Purchasing_Order_Number);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$VendorName", VendorName);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$SubmitterName", Submitter.CallName);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$EmpCode", emp.EmployeeId);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$POTitle", POStructure.PO_Title);
                                    Email.EmailTemplate = Email.EmailTemplate.Replace("$POVersion", "R"+POStructure.Version);
                                    SendEmail(Email.Subject, Email.EmailTemplate, ToUser.email);


                                }


                                var potemp1 = db.TENotificationsTemplates.Where(x => x.ModuleName == "POApproval").FirstOrDefault();
                                SendNotification(item.UserId, "Purchase Order " + POStructure.Purchasing_Order_Number + " " + potemp1.NotificationsTemplate.ToString(),POStructure.Uniqueid);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }


                   
                    response = "Success";
                }
                else
                {
                    db = new TEHRIS_DevEntities();
                  
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
                }
            }
            catch (Exception ex)
            {
                db = new TEHRIS_DevEntities();
                db.ApplicationErrorLogs.Add(
               
               new ApplicationErrorLog
               {
                   Error = "Exception "+ex.Message,
                   ExceptionDateTime = System.DateTime.Now,
                   InnerException = "PO UniqueId: " + POUniqueId + " , PO Number: " + PurchaseOrderNumber,
                   Source = "From TEPODetailsController | POSubmitforApproval Mehod | " + this.GetType().ToString(),
                   Stacktrace = ex.StackTrace

               }
               );
                
                db.SaveChanges();
              
                response = ex.Message;
            }
            return response;
        }
        
        #endregion

        #region withdraw
        [HttpGet]
        public string POwithdraw(int POUniqueId,int UserId)
        {
            string response = "";
            try
            {
                UserProfile User = db.UserProfiles.Where(x => x.UserId == UserId && x.IsDeleted == false).FirstOrDefault();
                List<TEPurchaseOrderApprover> AprList = db.TEPurchaseOrderApprovers.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && x.SequenceNumber != 0).ToList();
                if (AprList != null)
                {
                    foreach (TEPurchaseOrderApprover item in AprList)
                    {
                        db = new TEHRIS_DevEntities();
                        db.TEPurchaseOrderApprovers.Attach(item);

                        item.IsDeleted = true;
                        db.Entry(item).Property(x => x.IsDeleted).IsModified = true;

                        item.LastModifiedOn = System.DateTime.Now;
                        db.Entry(item).Property(x => x.LastModifiedOn).IsModified = true;

                        item.LastModifiedBy = User.UserName;
                        db.Entry(item).Property(x => x.LastModifiedBy).IsModified = true;

                        db.SaveChanges();




                    }
                    List<TEPurchase_Annexure_OtherCharges> annexure = db.TEPurchase_Annexure_OtherCharges.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).ToList();

                    foreach (TEPurchase_Annexure_OtherCharges item in annexure)
                    {
                        db = new TEHRIS_DevEntities();
                        db.TEPurchase_Annexure_OtherCharges.Attach(item);

                        item.IsDeleted = true;
                        db.Entry(item).Property(x => x.IsDeleted).IsModified = true;

                        item.LastModifiedOn = System.DateTime.Today.ToString();
                        db.Entry(item).Property(x => x.LastModifiedOn).IsModified = true;

                        item.LastModifiedBy = User.UserName;
                        db.Entry(item).Property(x => x.LastModifiedBy).IsModified = true;

                        db.SaveChanges();
                    }

                    db = new TEHRIS_DevEntities();

                    TEPurchase_header_structure POStructure = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                    db.TEPurchase_header_structure.Attach(POStructure);

                    POStructure.ReleaseCode2Status = "Draft";
                    db.Entry(POStructure).Property(x => x.ReleaseCode2Status).IsModified = true;

                    POStructure.LastModifiedOn = Convert.ToString(System.DateTime.Now);
                    db.Entry(POStructure).Property(x => x.LastModifiedOn).IsModified = true;

                    POStructure.LastModifiedBy = User.UserName;
                    db.Entry(POStructure).Property(x => x.LastModifiedBy).IsModified = true;

                    db.SaveChanges();
                }

                response="Success";
            }
            catch (Exception ex)
            {
                db = new TEHRIS_DevEntities();
                db.ApplicationErrorLogs.Add(

               new ApplicationErrorLog
               {
                   Error = "Exception " + ex.Message,
                   ExceptionDateTime = System.DateTime.Now,
                   InnerException = "PO UniqueId: " + POUniqueId ,
                   Source = "From TEPODetailsController | POWithdraw Mehod | " + this.GetType().ToString(),
                   Stacktrace = ex.StackTrace

               }
               );

                db.SaveChanges();
                response = ex.Message;
            }
            return response;
        }
        #endregion

        [HttpGet]
        public List<PlantStorageDetail> GETAllStorageDetails()
        {
            return db.PlantStorageDetails.Where(x => x.isdeleted == false && x.StorageLocationCode != null).ToList();
        }
        [HttpGet]
        public List<PlantStorageDetail> GETAllStorageDetailsByStorageCode(string Code)
        {
            return db.PlantStorageDetails.Where(x => x.isdeleted == false && x.StorageLocationCode != null && x.StateCode==Code).ToList();
        }

        [HttpGet]
        public IEnumerable<TEPurchaseItemListAnnexureModel> TEPurchaseItemListForAnnexure(string Purchasing_Order_Number, int POUniqueId)
        {
            List<TEPurchaseItemListAnnexureModel> result = new List<TEPurchaseItemListAnnexureModel>();
            try
            {
                var dte = System.DateTime.Now;
                var TotalPrice = (from q in db.TEPurchase_Itemwise
                                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                                  group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      g.Key.Item_Number_of_Purchasing_Document,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemAmount = (from q in db.TEPurchase_Itemwise
                                  where ((q.Condition_Type != "NAVS")
                                  && (q.Condition_Type != "JEXS")
                                  && (q.IsDeleted == false))
                                  group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      g.Key.Item_Number_of_Purchasing_Document,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemTax = (from q in db.TEPurchase_Itemwise
                               where (q.Condition_Type == "JEXS") && (q.IsDeleted == false)
                               group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                               orderby g.Key.POStructureId descending
                               select new
                               {
                                   g.Key.POStructureId,
                                   g.Key.Item_Number_of_Purchasing_Document,
                                   amount = g.Sum(x => (x.q.Condition_rate.Value))
                                   //TotalPrice = g.Sum(x => int.Parse(x.rate))
                               });

                var assign = (from ass in db.TEPurchase_Assignment
                              where (ass.POStructureId == POUniqueId)
                               && (ass.IsDeleted == false)
                              select new
                              {
                                  ass.POStructureId,
                                  ass.WBS_Element,
                                  ass.Commitment_item,
                                  ass.ItemNumber,
                                  ass.Fund_Center
                              }).Distinct();
                var PackingForwardingValue = (from q in db.TEPurchase_Itemwise
                                    where ((q.POStructureId == POUniqueId) &&

                                    ((q.Condition_Type == "ZPA1")
                                    || (q.Condition_Type == "ZPA2")
                                      || (q.Condition_Type == "ZPA3"))

                                    && (q.IsDeleted == false))
                                    group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                    orderby g.Key.POStructureId descending
                                    select new
                                    {
                                        g.Key.POStructureId,
                                        g.Key.Item_Number_of_Purchasing_Document,
                                        amount = g.Sum(x => (x.q.Condition_rate.Value))
                                        //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                    });
                var FreightValue = (from q in db.TEPurchase_Itemwise
                                              where ((q.POStructureId == POUniqueId) &&

                                              ((q.Condition_Type == "FRA1")
                                              || (q.Condition_Type == "FRB1")
                                                || (q.Condition_Type == "FRC1"))

                                              && (q.IsDeleted == false))
                                              group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                              orderby g.Key.POStructureId descending
                                              select new
                                              {
                                                  g.Key.POStructureId,
                                                  g.Key.Item_Number_of_Purchasing_Document,
                                                  amount = g.Sum(x => (x.q.Condition_rate.Value))
                                                  //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                              });
                var OtherServicesValue = (from q in db.TEPurchase_Itemwise
                                    where ((q.POStructureId == POUniqueId) &&

                                    ((q.Condition_Type == "ZHAN")
                                          || (q.Condition_Type == "ZHA1")
                                            || (q.Condition_Type == "ZOT1")
                                              || (q.Condition_Type == "ZENT")
                                                  || (q.Condition_Type == "ZOT4")
                                                  || (q.Condition_Type == "ZOT2")
                                                  || (q.Condition_Type == "ZOT3"))

                                    && (q.IsDeleted == false))
                                    group new { q } by new { q.POStructureId, q.Item_Number_of_Purchasing_Document } into g
                                    orderby g.Key.POStructureId descending
                                    select new
                                    {
                                        g.Key.POStructureId,
                                        g.Key.Item_Number_of_Purchasing_Document,
                                        amount = g.Sum(x => (x.q.Condition_rate.Value))
                                        //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                    });

                TEPurchase_header_structure purhaseHeader = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                List<TEPurchase_Annexure_OtherCharges> Annexure = db.TEPurchase_Annexure_OtherCharges.Where(x => x.IsDeleted == false && x.POStructureId==POUniqueId).ToList();
                var query = (from structure in db.TEPurchase_Item_Structure
                             //join ass in db.TEPurchase_Assignment on  structure.Purchasing_Order_Number equals ass.Purchasing_Order_Number into tempassign
                             ////join ass in db.TEPurchase_Assignment on "0000" + structure.Item_Number equals ass.ItemNumber into tempassign
                             //from tempassign1 in tempassign.DefaultIfEmpty()                          
                             where
                                 // //(structure.Purchasing_Order_Number == ass.Purchasing_Order_Number)&&
                                 // ("0000" + structure.Item_Number == tempassign1.ItemNumber) &&
                                 // (structure.Purchasing_Order_Number == tempassign1.Purchasing_Order_Number) &&
                            (structure.POStructureId == POUniqueId)
                                 //&& (structure.IsDeleted == false) && (ass.IsDeleted == false)
                             && (structure.IsDeleted == false)// && (tempassign1.IsDeleted == false)
                             orderby structure.Uniqueid descending
                             select new
                             {
                                 structure.Uniqueid,
                                 structure.POStructureId,
                                 structure.Material_Number,
                                 structure.Item_Category,
                                 structure.Short_Text,
                                 structure.Long_Text,
                                 structure.Line_item,
                                 structure.Order_Qty,
                                 structure.Unit_Measure,
                                 structure.Net_Price,
                                 ItemNumber = structure.Item_Number,
                                 structure.Purchasing_Order_Number,
                                 structure.Tax_salespurchases_code,
                                 structure.HSNCode,
                                 structure.Plant
                                 //tempassign1.WBS_Element,
                                 //tempassign1.Commitment_item,
                                 //tempassign1.ItemNumber
                                 //ass.WBS_Element,
                                 //ass.Commitment_item,
                                 //ass.ItemNumber
                             }).Distinct();
                foreach (var item in query)
                {

                    // int itemnumber = item.ItemNumber;
                    double amt = TotalPrice
                    .Where(x => (x.POStructureId == item.POStructureId)
                        && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                        .Select(x => x.amount).FirstOrDefault();
                    //amt = Math.Round(amt);

                    double? netPrice = 0.00;
                    if (item.Item_Category == "D")
                    {

                        List<TEPurchase_Service> serviceList = db.TEPurchase_Service.Where(x => x.POStructureId == item.POStructureId && x.IsDeleted == false && x.Item_Number == item.ItemNumber).ToList();
                        foreach (var ser in serviceList)
                        {
                            double tempPrice = Convert.ToDouble(ser.Net_Price);
                            netPrice = netPrice + tempPrice;
                        }

                    }
                    else
                    {
                        netPrice = Convert.ToDouble(item.Net_Price);
                    }
                    double? tamount = Convert.ToDouble(item.Order_Qty) * netPrice;
                    double? PackingForwarding = 0;
                    double? Freight = 0;
                    double? OtherServices = 0;
                    string PackingForwardingCondition = null;
                    string FreightCondition = null;
                    string OtherServicesCondition = null;
                    string OtherServicesType = null;
                    if (Annexure != null)
                    {
                        if (Annexure.Any(x => x.ItemNumber == item.ItemNumber))
                        {
                            TEPurchase_Annexure_OtherCharges tempannexure = Annexure.Where(x => x.ItemNumber == item.ItemNumber).FirstOrDefault();
                            PackingForwarding = Convert.ToDouble(tempannexure.PackingForwardingValue);
                            Freight = Convert.ToDouble(tempannexure.FreightValue);
                            OtherServices = Convert.ToDouble(tempannexure.OtherServicesValue);
                            PackingForwardingCondition = tempannexure.PackingForwardingCondition;
                            FreightCondition = tempannexure.FreightCondition;
                            OtherServicesCondition = tempannexure.OtherServicesCondition;
                            OtherServicesType = tempannexure.OtherServicesType;
                        }
                        else
                        {
                            PackingForwarding = (PackingForwardingValue.Where(x => (x.POStructureId == item.POStructureId)
                                                                   && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                                   .Select(x => x.amount).FirstOrDefault());
                            Freight = (FreightValue.Where(x => (x.POStructureId == item.POStructureId)
                                                                   && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                                   .Select(x => x.amount).FirstOrDefault());
                            OtherServices = (OtherServicesValue.Where(x => (x.POStructureId == item.POStructureId)
                                                                   && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                                   .Select(x => x.amount).FirstOrDefault());
                        }
                    }
                    else
                    {
                        PackingForwarding = (PackingForwardingValue.Where(x => (x.POStructureId == item.POStructureId)
                                                                   && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                                   .Select(x => x.amount).FirstOrDefault());
                        Freight = (FreightValue.Where(x => (x.POStructureId == item.POStructureId)
                                                               && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                               .Select(x => x.amount).FirstOrDefault());
                        OtherServices = (OtherServicesValue.Where(x => (x.POStructureId == item.POStructureId)
                                                               && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                               .Select(x => x.amount).FirstOrDefault());
                    }
                     double? itemamt = (ItemAmount.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                            .Select(x => x.amount).FirstOrDefault());

                    double? iTax = (ItemTax.Where(x => (x.POStructureId == item.POStructureId)
                                                         && (x.Item_Number_of_Purchasing_Document == item.ItemNumber))
                                           .Select(x => x.amount).FirstOrDefault());
                    result.Add(new TEPurchaseItemListAnnexureModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Material_Number = item.Material_Number,
                        Item_Category = item.Item_Category,
                        Short_Text = item.Short_Text,
                        Long_Text = item.Long_Text + " " + item.Line_item,
                        Order_Qty = item.Order_Qty,
                        Unit_Measure = item.Unit_Measure,
                        Net_Price = Convert.ToString(netPrice),
                        ItemAmount = itemamt.ToString(),
                        ItemTax = iTax.ToString(),
                        itemTotal = amt.ToString(),
                        //WBS_Element=item.WBS_Element,
                        //Commitment_item=item.Commitment_item,       "0000" +
                        WBS_Element = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber == item.ItemNumber))
                                            .Select(x => x.WBS_Element).FirstOrDefault()),
                        Commitment_item = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber == item.ItemNumber))
                                            .Select(x => x.Commitment_item).FirstOrDefault()),
                        ItemNumber = item.ItemNumber,
                        FundCenter = (assign.Where(x => (x.POStructureId == item.POStructureId)
                                                            && (x.ItemNumber == item.ItemNumber))
                                            .Select(x => x.Fund_Center).FirstOrDefault()),
                        PackingForwardingValue = PackingForwarding.ToString(),
                        FreightValue = Freight.ToString(),
                        OtherServicesValue = OtherServices.ToString(),
                        Amount = tamount.ToString(),
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

            db.SaveChanges();
            return result;
        }

        [HttpGet]
        public IEnumerable<TEPurchaseItemlistDetailsAnnexureModel> TEPurchaseItemlistDetailsForAnnexure(string Purchasing_Order_Number, string ItemNumber, int POUniqueId)
        {
            List<TEPurchaseItemlistDetailsAnnexureModel> result = new List<TEPurchaseItemlistDetailsAnnexureModel>();
            try
            {
                var dte = System.DateTime.Now;
                var TotalPrice = (from q in db.TEPurchase_Itemwise
                                  where (q.Condition_Type != "NAVS") && (q.IsDeleted == false)
                                  group new { q } by new { q.POStructureId } into g
                                  orderby g.Key.POStructureId descending
                                  select new
                                  {
                                      g.Key.POStructureId,
                                      amount = g.Sum(x => (x.q.Condition_rate.Value))
                                      //TotalPrice = g.Sum(x => int.Parse(x.rate))
                                  });
                var ItemTax = (from q in db.TEPurchase_Itemwise
                               where (q.Condition_Type == "JEXS") && (q.IsDeleted == false) && (q.Item_Number_of_Purchasing_Document == ItemNumber)
                               group new { q } by new { q.POStructureId } into g
                               orderby g.Key.POStructureId descending
                               select new
                               {
                                   g.Key.POStructureId,
                                   amount = g.Sum(x => (x.q.Condition_rate.Value))
                                   //TotalPrice = g.Sum(x => int.Parse(x.rate))
                               });
                var assign = //db.TEPurchase_Assignment.where (x =>x.IsDeleted == false);
                (from q in db.TEPurchase_Assignment
                 where (q.POStructureId == POUniqueId) && (q.IsDeleted == false) && (q.ItemNumber == ItemNumber)
                 orderby q.Uniqueid ascending
                 select new
                 {
                     q.Uniqueid,
                     q.IsDeleted,
                     q.Purchasing_Order_Number,
                     q.ItemNumber,
                     q.WBS_Element,
                     q.Commitment_item,
                     q.POStructureId

                 });
                TEPurchase_header_structure purhaseHeader = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == POUniqueId).FirstOrDefault();
                TEPurchase_Item_Structure purhaseItem = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId && (x.IsDeleted == false)).FirstOrDefault();
                var query = (from Service in db.TEPurchase_Service
                             //join ass in db.TEPurchase_Assignment on  Service.Item_Number equals ass.ItemNumber
                             where
                                 // (Service.Purchasing_Order_Number == ass.Purchasing_Order_Number) &&
                                 //( Service.Line_item_number == "00000000"+ass.NetworkNumber_AccountAssignment+"0")&&
                                 // (Service.Item_Number==ass.ItemNumber)&&
                             (Service.Item_Number == ItemNumber) &&
                            (Service.POStructureId == POUniqueId)
                            && (Service.IsDeleted == false) //&& (ass.IsDeleted == false)
                             orderby Service.Uniqueid descending
                             select new
                             {
                                 Service.Uniqueid,
                                 Service.Purchasing_Order_Number,
                                 Service.Activity_Number,
                                 Service.Item_Number,
                                 Service.Short_Text,
                                 Service.LongText,
                                 Service.line_item,
                                 Service.Actual_Qty,
                                 Service.Unit_Measure,
                                 Service.Net_Price,
                                 Service.Gross_Price,
                                 Service.WBS_Element,
                                 Service.Commitment_item,
                                 Service.Fund_Center,
                                 Service.POStructureId,
                                 Service.SACCode,

                                 //ass.WBS_Element,
                                 //ass.Commitment_item,
                                 // ass.ItemNumber

                             }).Distinct();

                int web = 0;
                int count1 = 0;
                int uniqueid = (from x in assign
                                select x.Uniqueid).FirstOrDefault();
                var qry = (from x in assign
                           select x).Count();
                foreach (var item in query)
                {

                    double amt = TotalPrice
                    .Where(x => x.POStructureId
                        == item.POStructureId)
                        .Select(x => x.amount).FirstOrDefault();
                    //string webselement="";
                    //string Commitment = "";   

                    //if ((web==count1)  && (qry>=count1))
                    //    {
                    //        webselement = assign
                    //            .Where(x => (x.Uniqueid == uniqueid)).Select(x => x.WBS_Element).Take(1).FirstOrDefault();
                    //     Commitment = assign
                    //            .Where(x => (x.Uniqueid == uniqueid)).Select(x => x.Commitment_item).Take(1).FirstOrDefault();
                    //        web =web+ 1;
                    //    }



                    result.Add(new TEPurchaseItemlistDetailsAnnexureModel()
                    {
                        Purchasing_Order_Number = item.Purchasing_Order_Number,
                        Activity_Number = item.Activity_Number,
                        Short_Text = item.Short_Text,
                        Long_Text = item.LongText + " " + item.line_item,
                        Order_Qty = item.Actual_Qty,
                        Unit_Measure = item.Unit_Measure,
                        Net_Price = item.Net_Price,
                        Gross_Price = item.Gross_Price,
                        itemTotal = amt,
                        WBS_Element = item.WBS_Element,
                        // WBS_Element= webselement,
                        ////WBS_Element =  assign
                        ////    .Where (x => (x.Purchasing_Order_Number  == item.Purchasing_Order_Number) &&
                        ////     ( x.ItemNumber == item.Item_Number))
                        ////        .Select(x => x.WBS_Element).FirstOrDefault(),

                        Commitment_item = item.Commitment_item,
                        //Commitment_item =Commitment,
                        ////Commitment_item = assign
                        ////   .Where(x => (x.Purchasing_Order_Number == item.Purchasing_Order_Number) &&
                        ////    (x.ItemNumber == item.Item_Number))
                        ////       .Select(x => x.Commitment_item).FirstOrDefault(),
                        ////  ItemNumber = item.ItemNumber,
                        ItemNumber = assign
                           .Where(x => (x.POStructureId == item.POStructureId) &&
                            (x.ItemNumber == item.Item_Number))
                               .Select(x => x.ItemNumber).FirstOrDefault(),
                        ItemTax = (ItemTax.Where(x => x.POStructureId == item.POStructureId)
                                         .Select(x => x.amount).FirstOrDefault()),
                        FundCenter = item.Fund_Center,
                        SACCode = item.SACCode,
                    });

                    uniqueid = uniqueid + 1;
                    count1 = count1 + 1;
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
                        Source = "From TEPurchaseItemlistDetails API | TEPurchaseItemlistDetails | " + this.GetType().ToString(),
                        Stacktrace = ex.StackTrace
                    }
                    );
            }

            db.SaveChanges();
            return result;
        }

        [HttpGet]
        public string testing(int PoId)
        {
            string lsResponse = "";
            try
            {

                 string FundCenter = "";
                 TEPurchase_header_structure POStructure = db.TEPurchase_header_structure.Where(x => x.IsDeleted == false && x.Uniqueid == PoId).FirstOrDefault();

                 TEPurchase_Item_Structure itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == PoId && x.Item_Category == "D").FirstOrDefault();

                if (itemStructure != null)
                {
                    FundCenter = db.TEPurchase_Service.Where(x => x.IsDeleted == false && x.POStructureId == PoId
                         && x.Item_Number == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();

                }
                else
                {
                    itemStructure = db.TEPurchase_Item_Structure.Where(x => x.IsDeleted == false && x.POStructureId == PoId).FirstOrDefault();

                    FundCenter = db.TEPurchase_Assignment.Where(x => x.IsDeleted == false && x.POStructureId == PoId
                        && x.ItemNumber == itemStructure.Item_Number).Select(s => s.Fund_Center).FirstOrDefault();
                }

                double TotalPrice = db.TEPurchase_Itemwise.Where(q => q.IsDeleted == false && q.POStructureId == PoId && (q.Condition_Type != "NAVS")
                              ).Sum(q => q.Condition_rate).Value;
                TotalPrice = Math.Round(TotalPrice);
                TotalPrice = Math.Truncate(TotalPrice);


                TEPurchase_FundCenter Fund = db.TEPurchase_FundCenter.Where(x => x.IsDeleted == false && x.FundCenter_Code == FundCenter).FirstOrDefault();
                //TEPurchasingGroup PGroup = db.TEPurchasingGroups.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Group).FirstOrDefault();
                TEPurchase_OrderTypes POrderType = db.TEPurchase_OrderTypes.Where(x => x.IsDeleted == false && x.Code == POStructure.Purchasing_Document_Type).FirstOrDefault();

                TEPOApprovalCondition AppCon = db.TEPOApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == Fund.Uniqueid
                     && x.OrderType == POrderType.UniqueId && TotalPrice >= x.MinAmount && TotalPrice<=x.MaxAmount).FirstOrDefault();

                if (AppCon != null)
                {
                    List<TEPOMasterApprover> MasterApprovers = db.TEPOMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == AppCon.UniqueId && x.Type == "Submitter").ToList();

                    int count = 0;

                    foreach (var Appr in MasterApprovers)
                    {
                        TEPurchaseOrderApprover result = new TEPurchaseOrderApprover();

                        count = count + 1;

                        string AprName = "Not Available";

                        result.CreatedOn = System.DateTime.Now;
                        result.LastModifiedOn = System.DateTime.Now;
                        result.CreatedBy = "SapAdmin";
                        result.LastModifiedBy = "SapAdmin";
                        result.SequenceNumber = 0;
                        result.PurchaseOrderNumber = POStructure.Purchasing_Order_Number;

                        AprName = db.UserProfiles.Where(x => x.IsDeleted == false && x.UserId == Appr.ApproverId).Select(x => x.CallName).FirstOrDefault();

                        if (AprName == null)
                        {
                            result.ApproverName = "Not Available";
                        }
                        else
                        {
                            result.ApproverName = AprName;
                        }
                       
                            result.Status = "NULL";
                        
                       
                        result.ApproverId = Appr.ApproverId;
                        result.POStructureId = POStructure.Uniqueid;
                        db.TEPurchaseOrderApprovers.Add(result);



                        db.SaveChanges();
                        lsResponse = "Submitters created successfully";
                    }

                }
            }
            catch (Exception Exc)
            {
                return "Error: " + Exc.Message;
            }
            if (String.IsNullOrEmpty(lsResponse))
            {
                return "Error: Unspecified problem while adding task.";
            }
            return lsResponse;
        }

    }

}
