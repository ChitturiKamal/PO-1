using Newtonsoft.Json.Linq;
using PO.BAL;
using PO.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PO.Controllers
{
    public class TEPOSearchAPIController : ApiController
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        RecordException ExceptionObj = new RecordException();

        public TEPOSearchAPIController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        #region My PO Search Methods
        public int GetCount()
        {
            var VendList = db.TEPOHeaderStructures.Where(x => x.IsDeleted == false).ToList();
            return VendList.Count;
        }
        public int GetSearchedMyPOFullCount(string FilterBy, int UserId)
        {
            int res = 0;
            string cnString = "";
            string cnStringBegin = "SELECT COUNT(t1.Uniqueid) ";
            cnStringBegin += " FROM dbo.TEPOHeaderStructure AS t1 ";
            if (FilterBy == "" || FilterBy == null)
            {
                cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOItemStructure AS t2t WHERE t2t.POStructureId = t1.Uniqueid ) t2 ";
            }
            else
            {
                cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOItemStructure AS t2t WHERE t2t.POStructureId = t1.Uniqueid AND ( t2t.Short_Text like '%" + FilterBy + "%' OR t2t.Long_Text like '%" + FilterBy + "%' )) t2 ";
            }
            cnStringBegin += " JOIN dbo.TEPOFundCenter AS t3 ON t1.FundCenterID = t3.Uniqueid ";
            cnStringBegin += " JOIN dbo.TEPOVendorMasterDetail AS t4 ON t1.VendorID = t4.POVendorDetailId ";
            cnStringBegin += " LEFT OUTER JOIN dbo.TEPOFundCenterUserMapping AS t5 ON t1.FundCenterID = t5.FundCenterId ";
            cnStringBegin += " LEFT OUTER JOIN dbo.TEPurchase_OrderTypes AS t6 ON t1.PO_OrderTypeID = t6.UniqueId ";
            cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOApprovers AS t7t WHERE t7t.POStructureId = t1.Uniqueid AND t7t.ApproverId = " + UserId + " and t7t.isdeleted=0) t7 ";
            cnStringBegin += " LEFT OUTER JOIN dbo.UserProfile AS t8 ON t1.POManagerID = t8.UserId ";
            cnStringBegin += " JOIN dbo.TEPOVendorMaster t9 ON t4.POVendorMasterId = t9.POVendorMasterId ";
            cnStringBegin += " WHERE t1.IsDeleted=0 AND ";
            cnStringBegin += SetWhereClause(FilterBy, UserId);
            cnString = cnStringBegin;
            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                Int32 ContactList1 = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                res = ContactList1;
            }
            return res;
        }
        [HttpPost]
        public HttpResponseMessage GetSearchedMyPOs(JObject TEData)
        {
            try
            {
                List<SearchPODet> GetSearchedPOList = new List<SearchPODet>();
                string FilterBy = TEData["FilterBy"].ToObject<string>();
                int UserId = TEData["UserId"].ToObject<int>();
                var isPoAccess = (from userrole in db.webpages_UsersInRoles
                                  join webrole in db.webpages_Roles on userrole.RoleId equals webrole.RoleId
                                  where userrole.UserId == UserId && webrole.RoleName.ToLower() == "po access"
                                  select new { webrole.RoleName }).FirstOrDefault();

                if (isPoAccess == null)
                {
                    sinfo.errorcode = 0;
                    sinfo.errormessage = "Don't have PO Access";
                    sinfo.listcount = 0;
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = string.Empty, info = sinfo }) };
                }

                int? PageNumber = TEData["pageNumber"].ToObject<int>();
                int? PageperCount = TEData["pagePerCount"].ToObject<int>();
                if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
                int? FromRecords = ((PageNumber - 1) * PageperCount) + 1;
                if (PageperCount == 0)
                {
                    PageperCount = GetCount();
                }
                int? ToRecords = (PageNumber * PageperCount);
                sinfo.fromrecords = Convert.ToInt32(FromRecords);
                sinfo.torecords = Convert.ToInt32(ToRecords);
                sinfo.totalrecords = GetSearchedMyPOFullCount(FilterBy, UserId);
                sinfo.listcount = Convert.ToInt32(PageperCount);
                if (sinfo.totalrecords <= 0)
                {
                    sinfo.errorcode = 1; sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedPOList, info = sinfo }) };
                }
                string cnString = "";
                string cnStringBegin = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t1.Uniqueid DESC) as RowNum, ";
                cnStringBegin += " t1.Uniqueid as HeaderUniqueid, ";
                cnStringBegin += " t3.ProjectCode as ProjectCodes, ";
                cnStringBegin += " t3.FundCenter_Description as WbsHeads, ";
                cnStringBegin += " t1.Purchasing_Order_Number as Purchasing_Order_Number, ";
                cnStringBegin += " t1.PO_Title as PoTitle, ";
                cnStringBegin += " t1.Purchasing_Document_Date as Purchasing_Document_Date, ";
                cnStringBegin += " t1.ReleaseCode2Status as POStatus, ";
                cnStringBegin += " t9.VendorName as VendorName, ";
                cnStringBegin += " t8.CallName as ManagerName, ";
                cnStringBegin += " t1.Version as Version, ";
                cnStringBegin += " t1.PurchaseRequestId as PurchaseRequestId, ";
                cnStringBegin += " t1.CreatedBy as CreatedBy, ";
                cnStringBegin += " t1.IsNewPO as IsNewPO, ";
                cnStringBegin += " (SELECT SUM(TotalAmount) FROM TEPOItemStructure where POStructureId=t1.Uniqueid and isdeleted=0) as Amount ";

                cnStringBegin += " FROM dbo.TEPOHeaderStructure AS t1 ";
                if (FilterBy == "" || FilterBy == null)
                {
                    cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOItemStructure AS t2t WHERE t2t.POStructureId = t1.Uniqueid ) t2 ";
                }
                else
                {
                    cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOItemStructure AS t2t WHERE t2t.POStructureId = t1.Uniqueid AND ( t2t.Short_Text like '%" + FilterBy + "%' OR t2t.Long_Text like '%" + FilterBy + "%' )) t2 ";
                }
                cnStringBegin += " JOIN dbo.TEPOFundCenter AS t3 ON t1.FundCenterID = t3.Uniqueid ";
                cnStringBegin += " JOIN dbo.TEPOVendorMasterDetail AS t4 ON t1.VendorID = t4.POVendorDetailId ";
                cnStringBegin += " LEFT OUTER JOIN dbo.TEPOFundCenterUserMapping AS t5 ON t1.FundCenterID = t5.FundCenterId ";
                cnStringBegin += " LEFT OUTER JOIN dbo.TEPurchase_OrderTypes AS t6 ON t1.PO_OrderTypeID = t6.UniqueId ";
                cnStringBegin += " OUTER APPLY(SELECT TOP 1 * FROM dbo.TEPOApprovers AS t7t WHERE t7t.POStructureId = t1.Uniqueid AND t7t.ApproverId = " + UserId + " and t7t.isdeleted=0) t7 ";
                cnStringBegin += " LEFT OUTER JOIN dbo.UserProfile AS t8 ON t1.POManagerID = t8.UserId ";
                cnStringBegin += " JOIN dbo.TEPOVendorMaster t9 ON t4.POVendorMasterId = t9.POVendorMasterId ";
                cnStringBegin += " WHERE t1.IsDeleted=0 AND ";
                cnStringBegin += SetWhereClause(FilterBy, UserId);
                //cnStringBegin += " t1.IsDeleted=0";
                cnStringBegin += ")";
                cnString = cnStringBegin + "AS T WHERE RowNum >= " + FromRecords + " and RowNum <= " + ToRecords + "";
                if (cnString != "")
                {
                    SqlConnection cn = new SqlConnection();
                    SqlCommand cmd = new SqlCommand();
                    cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = cnString;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GetSearchedPOList.Add(new SearchPODet
                            {
                                Purchasing_Order_Number = (dr["Purchasing_Order_Number"] != DBNull.Value) ? dr["Purchasing_Order_Number"].ToString() : "",
                                Purchasing_Document_Date = (dr["Purchasing_Document_Date"] != DBNull.Value) ? Convert.ToDateTime(dr["Purchasing_Document_Date"].ToString()) : (DateTime?)null,
                                CreatedBy = (dr["CreatedBy"] != DBNull.Value) ? Convert.ToInt32(dr["CreatedBy"].ToString()) : 0,
                                POStatus = (dr["POStatus"] != DBNull.Value) ? dr["POStatus"].ToString() : "",
                                Version = (dr["Version"] != DBNull.Value) ? ("R" + dr["Version"].ToString()) : "",
                                VendorName = (dr["VendorName"] != DBNull.Value) ? dr["VendorName"].ToString() : "",
                                HeaderUniqueid = (dr["HeaderUniqueid"] != DBNull.Value) ? Convert.ToInt32(dr["HeaderUniqueid"].ToString()) : 0,
                                PoTitle = (dr["PoTitle"] != DBNull.Value) ? dr["PoTitle"].ToString() : "",
                                ManagerName = (dr["ManagerName"] != DBNull.Value) ? dr["ManagerName"].ToString() : "",
                                PurchaseRequestId = (dr["PurchaseRequestId"] != DBNull.Value) ? Convert.ToInt32(dr["PurchaseRequestId"].ToString()) : 0,
                                ProjectCodes = (dr["ProjectCodes"] != DBNull.Value) ? dr["ProjectCodes"].ToString() : "",
                                IsNewPO = (dr["IsNewPO"] != DBNull.Value) ? Convert.ToBoolean(dr["IsNewPO"]) : false,
                                WbsHeads = (dr["WbsHeads"] != DBNull.Value) ? dr["WbsHeads"].ToString() : "",
                                isRevisionAllowed=true,
                                //Amount = db.TEPOItemStructures.Where(a => a.POStructureId == Convert.ToInt32(dr["HeaderUniqueid"])).Sum(a => a.TotalAmount)
                                Amount = (dr["Amount"] != DBNull.Value) ? Convert.ToDecimal(dr["Amount"].ToString()) : 0,
                            });
                        }
                    }
                    
                    dr.Close();
                    cn.Close();
                }
                if (GetSearchedPOList.Count > 0) { sinfo.errorcode = 0; sinfo.errormessage = "Success";}
                else { sinfo.errorcode = 1; sinfo.errormessage = "Fail"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedPOList, info = sinfo }) };
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
        public string SetWhereClause(string a, int b)
        {
            string FinalQuery = string.Empty;
            string Mandate = " t1.Status = 'Active' AND t1.ReleaseCode2Status = 'Approved' ";
            string AQuery = " AND ((t7.ApproverId = " + b + " AND t7.IsDeleted = 0) OR (t5.UserId = " + b + " AND t5.IsDeleted = 0) OR (t1.createdby = " + b + " AND t1.IsDeleted = 0)) ";
            if (a == "" || a == null)
            {
                FinalQuery = Mandate + AQuery;
                return FinalQuery;
            }
            else
            {

                string q1 = " t2.Material_Number like '%" + a + "%' ";
                string q2 = " OR t2.Short_Text like '%" + a + "%' ";
                string q3 = " OR t2.Long_Text like '%" + a + "%' ";
                string q4 = " OR t1.Purchasing_Order_Number like '%" + a + "%' ";
                string q5 = " OR t1.PO_Title like '%" + a + "%' ";
                string q6 = " OR t1.Status like '%" + a + "%' ";
                string q7 = " OR t1.Uniqueid like '%" + a + "%' ";
                string q8 = " OR t6.Description like '%" + a + "%' ";
                string q9 = " OR t1.SubmitterName like '%" + a + "%' ";
                string q10 = " OR t8.CallName like '%" + a + "%' ";
                string q11 = " OR t3.ProjectCode like '%" + a + "%' ";
                string q12 = " OR t3.FundCenter_Description like '%" + a + "%' ";
                string q13 = " OR t9.VendorName like '%" + a + "%' ";
                string orQuery = " AND (" + q1 + q2 + q3 + q4 + q5 + q6 + q7 + q8 + q9 + q10 + q11 + q12 + q13 + ") ";
                FinalQuery = Mandate + AQuery + orQuery;
                return FinalQuery;
            }

        }
        #endregion
        #region HSN TAX Master Search
        [HttpPost]
        public HttpResponseMessage GetHSNSearched(HSNObject TEData)
        {
            try
            {
                List<HSNObjClass> GetSearchedHSNList = new List<HSNObjClass>();
                int? PageNumber = TEData.pageNumber;
                int? PageperCount = TEData.pagePerCount;
                if (PageNumber == 0 || PageNumber == null) PageNumber = 1;
                int? FromRecords = ((PageNumber - 1) * PageperCount) + 1;
                if (PageperCount == 0)
                {
                    PageperCount = GetHSNCount();
                }
                int? ToRecords = (PageNumber * PageperCount);
                sinfo.fromrecords = Convert.ToInt32(FromRecords);
                sinfo.torecords = Convert.ToInt32(ToRecords);
                sinfo.totalrecords = GetSearchedHSNFullCount(TEData.HSNFilter);
                sinfo.listcount = Convert.ToInt32(PageperCount);
                if (sinfo.totalrecords <= 0)
                {
                    sinfo.errorcode = 1; sinfo.errormessage = "No Records Found";
                    return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedHSNList, info = sinfo }) };
                }
                string cnString = "";
                string cnStringBegin = "SELECT * FROM (select ROW_NUMBER() OVER(ORDER BY HSNTx.Uniqueid DESC) as RowNum, ";
                cnStringBegin += " HSNTx.ApplicableTo, HSNTx.DestinationCountry, HSNTx.VendorRegionDescription, HSNTx.DeliveryPlantRegionDescription, HSNTx.HSNCode," +
                    "MaterialGSTApplicability = tempMatApp.Description, VendorGSTApplicability = tempVenApp.Description, HSNTx.ValidFrom, HSNTx.ValidTo, HSNTx.TaxType, HSNTx.TaxCode," +
                    "HSNTx.TaxRate,HSNTx.UniqueID, HSNTx.LastModifiedOn, prof.UserName, HSNTx.IsDeleted ";
               

                cnStringBegin += " from TEPOHSNTaxCodeMapping HSNTx";
                
                cnStringBegin += "  left outer join TEPOGSTApplicabilityMaster tempMatApp  on HSNTx.MaterialGSTApplicability = tempMatApp.UniqueID ";
                cnStringBegin += " left outer join TEPOGSTApplicabilityMaster tempVenApp on HSNTx.VendorGSTApplicability = tempVenApp.UniqueID  ";
                cnStringBegin += " left outer join UserProfile prof on HSNTx.LastModifiedBy = prof.UserId ";
                cnStringBegin += " WHERE HSNTx.IsDeleted=0 ";
                cnStringBegin += SetHSNWhereClause(TEData.HSNFilter);
                //cnStringBegin += " t1.IsDeleted=0";
                cnStringBegin += ")";
                cnString = cnStringBegin + "AS T WHERE RowNum >= " + FromRecords + " and RowNum <= " + ToRecords + "";
                if (cnString != "")
                {
                    SqlConnection cn = new SqlConnection();
                    SqlCommand cmd = new SqlCommand();
                    cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                    cn.Open();
                    cmd.Connection = cn;
                    cmd.CommandText = cnString;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GetSearchedHSNList.Add(new HSNObjClass
                            {
                                ApplicableTo = (dr["ApplicableTo"] != DBNull.Value) ? dr["ApplicableTo"].ToString() : "",
                                DestinationCountry = (dr["DestinationCountry"] != DBNull.Value) ? dr["DestinationCountry"].ToString() : "",
                                VendorRegionDescription = (dr["VendorRegionDescription"] != DBNull.Value) ? dr["VendorRegionDescription"].ToString() : "",
                                DeliveryPlantRegionDescription = (dr["DeliveryPlantRegionDescription"] != DBNull.Value) ? dr["DeliveryPlantRegionDescription"].ToString() : "",
                                HSNCode = (dr["HSNCode"] != DBNull.Value) ? (dr["HSNCode"].ToString()) : "",
                                MaterialGSTApplicability = (dr["MaterialGSTApplicability"] != DBNull.Value) ? dr["MaterialGSTApplicability"].ToString() : "",
                                VendorGSTApplicability = (dr["VendorGSTApplicability"] != DBNull.Value) ? dr["VendorGSTApplicability"].ToString() : "",
                                TaxType = (dr["TaxType"] != DBNull.Value) ? dr["TaxType"].ToString() : "",
                                TaxCode = (dr["TaxCode"] != DBNull.Value) ? dr["TaxCode"].ToString() : "",
                                TaxRate = (dr["TaxRate"] != DBNull.Value) ? Convert.ToDecimal(dr["TaxRate"].ToString()) : 0,
                                UniqueID = (dr["UniqueID"] != DBNull.Value) ? Convert.ToInt32(dr["UniqueID"].ToString()) : 0,
                                LastModifiedOn = (dr["LastModifiedOn"] != DBNull.Value) ? Convert.ToDateTime(dr["LastModifiedOn"].ToString()) : (DateTime?)null,
                                UserName = (dr["UserName"] != DBNull.Value) ? dr["UserName"].ToString() : "",
                                IsDeleted = (dr["IsDeleted"] != DBNull.Value) ? Convert.ToBoolean(dr["IsDeleted"]) : false
                                
                            });
                        }
                    }

                    dr.Close();
                    cn.Close();
                }
                if (GetSearchedHSNList.Count > 0) { sinfo.errorcode = 0; sinfo.errormessage = "Success"; }
                else { sinfo.errorcode = 1; sinfo.errormessage = "Fail"; }
                return new HttpResponseMessage() { Content = new JsonContent(new { result = GetSearchedHSNList, info = sinfo }) };
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
        public int GetHSNCount()
        {
            var HSNList = db.TEPOHSNTaxCodeMappings.Where(x => x.IsDeleted == false).ToList();
            return HSNList.Count;
        }
        public string SetHSNWhereClause(HSN filterData)
        {
            string orQuery = String.Empty;
            if (!String.IsNullOrEmpty(filterData.Applicability) || !String.IsNullOrEmpty(filterData.Destination) || !String.IsNullOrEmpty(filterData.Vend_Region) ||
                !String.IsNullOrEmpty(filterData.Delivery_Plant) || !String.IsNullOrEmpty(filterData.HSN_Code) || !String.IsNullOrEmpty(filterData.Material_GST_Appl) || 
                !String.IsNullOrEmpty(filterData.Vend_GST_Appl) || !String.IsNullOrEmpty(filterData.TAX_Type) || !String.IsNullOrEmpty(filterData.TAX_Code) || !String.IsNullOrEmpty(filterData.TAX_Rate)
                    )
            {
                String q1 = String.Empty;
                if (!String.IsNullOrEmpty(filterData.Applicability))
                    q1 = " HSNTx.ApplicableTo like '%" + filterData.Applicability + "%' ";
                if (!String.IsNullOrEmpty(filterData.Destination))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.DestinationCountry like '%" + filterData.Destination + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.Vend_Region))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.VendorRegionDescription like '%" + filterData.Vend_Region + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.Delivery_Plant))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.DeliveryPlantRegionDescription Like '%" + filterData.Delivery_Plant + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.HSN_Code))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.HSNCode like '%" + filterData.HSN_Code + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.Material_GST_Appl))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " MaterialGSTApplicability like '%" + filterData.Material_GST_Appl + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.Vend_GST_Appl))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " VendorGSTApplicability like '%" + filterData.Vend_GST_Appl + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.TAX_Type))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.TaxType like '%" + filterData.TAX_Type + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.TAX_Code))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.TaxCode like '%" + filterData.TAX_Code + "%' ";
                }
                if (!String.IsNullOrEmpty(filterData.TAX_Rate))
                {
                    if (!String.IsNullOrEmpty(q1))
                        q1 += " OR ";
                    q1 += " HSNTx.TaxRate like '%" + filterData.TAX_Rate + "%' ";
                }

                 orQuery = " AND (" + q1 + ") ";
            }
            return orQuery;
        }
        public int GetSearchedHSNFullCount(HSN FilterBy)
        {
            int res = 0;
            string cnString = "";
            string cnStringBegin = "select count(*) ";
            

            cnStringBegin += " from TEPOHSNTaxCodeMapping HSNTx";

            cnStringBegin += "  left outer join TEPOGSTApplicabilityMaster tempMatApp  on HSNTx.MaterialGSTApplicability = tempMatApp.UniqueID ";
            cnStringBegin += " left outer join TEPOGSTApplicabilityMaster tempVenApp on HSNTx.VendorGSTApplicability = tempVenApp.UniqueID  ";
            cnStringBegin += " left outer join UserProfile prof on HSNTx.LastModifiedBy = prof.UserId ";
            cnStringBegin += " WHERE HSNTx.IsDeleted=0 ";
            cnStringBegin += SetHSNWhereClause(FilterBy);
            cnString = cnStringBegin;
            if (cnString != "")
            {
                SqlConnection cn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["TEConnection"].ToString();
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = cnString;
                Int32 ContactList1 = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
                res = ContactList1;
            }
            return res;
        }
        #endregion
        [HttpPost]
        public HttpResponseMessage ApprovePO(JObject TEData)
        {
            TEPOApprover LastApprovedItem = new TEPOApprover();
            try
            {
                int ActualSeq = -1;int CurrentSeq = -1;
                var re = Request;
                var header = re.Headers;
                int authuser = 0;
                if (header.Contains("authUser")) authuser = Convert.ToInt32(header.GetValues("authUser").First());
                int Approver = authuser;
                int POUniqueId = TEData["POUniqueId"].ToObject<int>();
                string ApproverComments = TEData["SubmitterComments"].ToObject<string>();
                TEPOHeaderStructure PODet = new TEPOHeaderStructure();
                PODet = db.TEPOHeaderStructures.Where(a => a.Uniqueid == POUniqueId).FirstOrDefault();
                if(!PODet.ReleaseCode2Status.Equals("Pending For Approval"))
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Cannot Approve as PO is not in Approval Pending Stage";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                List<TEPOApprover> ApproverList = new List<TEPOApprover>();
                ApproverList = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a=>a.SequenceNumber).ToList();
                ActualSeq = ApproverList.Select(a => a.SequenceNumber).OrderBy(a => a).FirstOrDefault();

                List<TEPOApprover> CurrentApproverList = new List<TEPOApprover>();
                CurrentApproverList = ApproverList.Where(a => a.ApproverId == Approver).OrderBy(a => a.SequenceNumber).ToList();
                CurrentSeq = CurrentApproverList.Select(a => a.SequenceNumber).OrderBy(a => a).FirstOrDefault();
                if (CurrentApproverList.Count <= 0 || CurrentSeq != ActualSeq)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "You are not Authorized to Approved";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                if (ActualSeq < 0 || CurrentSeq < 0)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Cannot Approve as PO is not having proper Approving Data";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                TEPOApprover CurrentApproverItem = new TEPOApprover();
               
                for (int i=0;i< CurrentApproverList.Count; i++)
                {
                    CurrentApproverItem = CurrentApproverList[i];
                    if (i == 0) {
                        CurrentApproverItem.Status = "Approved";
                        CurrentApproverItem.LastModifiedOn = DateTime.Now;
                        CurrentApproverItem.ApprovedOn = DateTime.Now;
                        CurrentApproverItem.Comments = ApproverComments;
                        db.Entry(CurrentApproverItem).CurrentValues.SetValues(CurrentApproverItem);
                        db.SaveChanges();
                        LastApprovedItem = CurrentApproverItem;
                    }
                    else
                    {
                        if((CurrentSeq+1)== CurrentApproverItem.SequenceNumber)
                        {
                            CurrentApproverItem.Status = "Approved";
                            CurrentApproverItem.LastModifiedOn = DateTime.Now;
                            CurrentApproverItem.ApprovedOn = DateTime.Now;
                            CurrentApproverItem.Comments = ApproverComments;
                            db.Entry(CurrentApproverItem).CurrentValues.SetValues(CurrentApproverItem);
                            db.SaveChanges();
                            LastApprovedItem = CurrentApproverItem;
                        }
                        else
                        {
                            break;
                        }
                    }
                    CurrentSeq = CurrentApproverItem.SequenceNumber;
                }
                List<TEPOApprover> RestApproverList = new List<TEPOApprover>();
                RestApproverList = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a => a.SequenceNumber).ToList();
                if (RestApproverList.Count <= 0)
                {
                    SAPResponse SResponse = new SAPResponse();
                    PurchaseOrderBAL POBal = new PurchaseOrderBAL();
                    PurchaseOrderController POCntrl = new PurchaseOrderController();
                    //Sending to SAP
                    if (String.IsNullOrEmpty(PODet.Purchasing_Order_Number))
                    {
                        POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, true);
                        SResponse = POBal.SendPODetailsToSAP(POUniqueId);
                        PODet.Purchasing_Document_Date = DateTime.Now;
                    }
                    else
                    {
                        POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, false);
                        SResponse = POBal.UpdatePODetailsToSAP(POUniqueId);
                        
                    }
                    if(SResponse == null)
                    {
                        LastApprovedItem.Status = "Pending For Approval";
                        db.Entry(LastApprovedItem).CurrentValues.SetValues(LastApprovedItem);
                        db.SaveChanges();
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Approve as sending to SAP had some issues";
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                    if (SResponse.ReturnCode.Equals("0"))
                    {
                        PODet.Purchasing_Order_Number = SResponse.PONumber;
                        PODet.ReleaseCode2Status = "Approved";
                        db.Entry(PODet).CurrentValues.SetValues(PODet);
                        db.SaveChanges();
                    }
                    else
                    {
                        LastApprovedItem.Status = "Pending For Approval";
                        db.Entry(LastApprovedItem).CurrentValues.SetValues(LastApprovedItem);
                        db.SaveChanges();
                        sinfo.errorcode = 1;
                        sinfo.errormessage = "Failed to Update PO Number in SAP.Exception:" + SResponse.Message;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    TEPOApprover NextApprover = new  TEPOApprover();
                    NextApprover = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a => a.SequenceNumber).FirstOrDefault();
                    NextApprover.Status = "Pending For Approval";
                    db.Entry(NextApprover).CurrentValues.SetValues(NextApprover);
                    db.SaveChanges();
                    //Update the Next Approver to Pending for Approver
                }
                new EmailSendingBL().POEmail_Approved(POUniqueId, PODet.CreatedBy, Approver);
                sinfo.errorcode = 0;
                sinfo.errormessage = "PO Successfully Approved";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                if (LastApprovedItem != null)
                {
                    LastApprovedItem.Status = "Pending For Approval";
                    db.Entry(LastApprovedItem).CurrentValues.SetValues(LastApprovedItem);
                    db.SaveChanges();

                }
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Approve";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }
        [HttpPost]
        public HttpResponseMessage SubmitForApprove(JObject TEData)
        {
            var re = Request;
            var header = re.Headers;
            int authuser = 0;
            if (header.Contains("authUser")) authuser = Convert.ToInt32(header.GetValues("authUser").First());
            int Approver = authuser;
            TEPOHeaderStructure PODet = new TEPOHeaderStructure();
            try
            {
                int POUniqueId = TEData["POUniqueId"].ToObject<int>();
                string ApproverComments = TEData["SubmitterComments"].ToObject<string>();
                
                PODet = db.TEPOHeaderStructures.Where(a => a.Uniqueid == POUniqueId).FirstOrDefault();
                UserProfile UserDet = new UserProfile();
                UserDet = db.UserProfiles.Where(a => a.UserId == Approver).FirstOrDefault();

                SAPResponse SResponse = new SAPResponse();
                PurchaseOrderBAL POBal = new PurchaseOrderBAL();
                PurchaseOrderController POCntrl = new PurchaseOrderController();
                decimal? ItemsTotal = 0;
                ItemsTotal = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == POUniqueId).Sum(a => a.TotalAmount);
                ItemsTotal = ItemsTotal != null ? Convert.ToDecimal(ItemsTotal) : 0;

                double? PaymentTotal = 0;
                PaymentTotal = db.TEPOVendorPaymentMilestones.Where(x => x.IsDeleted == false && x.POHeaderStructureId == POUniqueId).Sum(a => a.Amount);
                PaymentTotal = PaymentTotal != null ? Convert.ToDouble(PaymentTotal) : 0;
                if (Convert.ToDecimal(ItemsTotal) != Convert.ToDecimal(PaymentTotal))
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "InComplete PO. Fill PaymentTerms of PO before Submit for approval";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                #region Validating with SAP

                if (String.IsNullOrEmpty(PODet.Purchasing_Order_Number))
                {
                    POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, true);
                    SResponse = POBal.SendPODetailsToSAP(POUniqueId, true);
                }
                else
                {
                    POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, false);
                    SResponse = POBal.UpdatePODetailsToSAP(POUniqueId, true);
                }
                //if (String.IsNullOrEmpty(SubmitValidation(POUniqueId)))
                //    SResponse.ReturnCode = "0";
                #endregion

                if (SResponse == null)
                {
                    PODet.ReleaseCode2Status = "Draft";
                    PODet.LastModifiedOn = DateTime.Now;
                    db.Entry(PODet).CurrentValues.SetValues(PODet);
                    db.SaveChanges();
                    DeleteApprovers(PODet.Uniqueid, Approver);
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Submit for Approval as sending to SAP had some issues";
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                if (SResponse.ReturnCode.Equals("0"))
                {
                    PODet.ReleaseCode2Status = "Pending For Approval";
                    PODet.LastModifiedBy = Approver;
                    PODet.LastModifiedOn = DateTime.Now;
                    db.Entry(PODet).CurrentValues.SetValues(PODet);
                    db.SaveChanges();
                    DeleteApprovers(PODet.Uniqueid, Approver);

                    SuccessInfo InsResponse = new SuccessInfo();
                    InsResponse=InsertPOApprovers(POUniqueId, UserDet.UserId);
                    if (InsResponse.errorcode != 0)
                    {
                        PODet.ReleaseCode2Status = "Draft";
                        PODet.LastModifiedOn = DateTime.Now;
                        db.Entry(PODet).CurrentValues.SetValues(PODet);
                        db.SaveChanges();
                        DeleteApprovers(PODet.Uniqueid, Approver);
                        sinfo.errorcode = 1;
                        sinfo.errormessage = InsResponse.errormessage;
                        return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                    }
                }
                else
                {
                    PODet.ReleaseCode2Status = "Draft";
                    PODet.LastModifiedOn = DateTime.Now;
                    db.Entry(PODet).CurrentValues.SetValues(PODet);
                    db.SaveChanges();
                    DeleteApprovers(PODet.Uniqueid, Approver);
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Submit for Approval in SAP.Exception:" + SResponse.Message;
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                SuccessInfo AppResponse = new SuccessInfo();
                AppResponse=POGetApprovedHere(POUniqueId, UserDet.UserId, ApproverComments);
                if (AppResponse.errorcode != 0)
                {
                    PODet.ReleaseCode2Status = "Draft";
                    PODet.LastModifiedOn = DateTime.Now;
                    db.Entry(PODet).CurrentValues.SetValues(PODet);
                    db.SaveChanges();
                    DeleteApprovers(PODet.Uniqueid, Approver);
                    sinfo.errorcode = 1;
                    sinfo.errormessage = AppResponse.errormessage;
                    return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "PO Successfully Submitted for Approval";
                new EmailSendingBL().POEmail_SubmitForApproval(PODet.Uniqueid, PODet.CreatedBy, Approver);
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            catch (Exception ex)
            {
                if (PODet != null)
                {
                    PODet.ReleaseCode2Status = "Draft";
                    PODet.LastModifiedOn = DateTime.Now;
                    db.Entry(PODet).CurrentValues.SetValues(PODet);
                    db.SaveChanges();
                    DeleteApprovers(PODet.Uniqueid, Approver);
                }
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Approve";
                return new HttpResponseMessage() { Content = new JsonContent(new { result = ex.Message, info = sinfo }) };
            }
        }
        public void DeleteApprovers(int POID,int UserID)
        {
            List<TEPOApprover> DeleteList = new List<TEPOApprover>();
            var UserDet = db.UserProfiles.Where(a => a.UserId == UserID).FirstOrDefault();
            DeleteList = db.TEPOApprovers.Where(a => a.POStructureId == POID && a.IsDeleted == false).ToList();
            if(DeleteList.Count > 0)
            DeleteList.ForEach(a => { a.IsDeleted = true;a.LastModifiedBy = UserDet.UserName; a.LastModifiedOn = DateTime.Now; });
            db.SaveChanges();
            TEPOHeaderStructure PODet = new TEPOHeaderStructure();
            PODet = db.TEPOHeaderStructures.Where(a => a.Uniqueid == POID).FirstOrDefault();
            if (PODet.CreatedBy != null && PODet.CreatedBy != 0)
            {
                UserProfile SubmitterDet = new UserProfile();
                SubmitterDet = db.UserProfiles.Where(a => a.UserId == PODet.CreatedBy).FirstOrDefault();
                TEPOApprover Submitter = new TEPOApprover();
                Submitter.CreatedOn = System.DateTime.Now;
                Submitter.LastModifiedOn = System.DateTime.Now;
                Submitter.CreatedBy = UserDet.UserName;
                Submitter.LastModifiedBy = UserDet.UserName;
                Submitter.SequenceNumber = 0;
                Submitter.POStructureId = POID;
                Submitter.PurchaseOrderNumber = POID.ToString();
                Submitter.ApproverName = SubmitterDet.UserName;
                Submitter.Status = "Pending For Approval";
                Submitter.ApproverId = SubmitterDet.UserId;
                Submitter.IsDeleted = false;
                db.TEPOApprovers.Add(Submitter);
                db.SaveChanges();
            }
            if (PODet.POManagerID != null && PODet.POManagerID != 0)
            {
                UserProfile POManagerDet = new UserProfile();
                TEPOApprover POManagerApproval = new TEPOApprover();
                POManagerDet = db.UserProfiles.Where(a => a.UserId == PODet.POManagerID).FirstOrDefault();
                POManagerApproval.CreatedOn = System.DateTime.Now;
                POManagerApproval.LastModifiedOn = System.DateTime.Now;
                POManagerApproval.CreatedBy = UserDet.UserName;
                POManagerApproval.LastModifiedBy = UserDet.UserName;
                POManagerApproval.SequenceNumber = 1;
                POManagerApproval.POStructureId = POID;
                POManagerApproval.PurchaseOrderNumber = POID.ToString();
                POManagerApproval.ApproverName = POManagerDet.UserName;
                POManagerApproval.Status = "Draft";
                POManagerApproval.ApproverId = POManagerDet.UserId;
                POManagerApproval.IsDeleted = false;
                db.TEPOApprovers.Add(POManagerApproval);
                db.SaveChanges();
            }
        }

        public SuccessInfo InsertPOApprovers(int POID, int UserID)
        {
            SuccessInfo res = new SuccessInfo();
            try
            {
                UserProfile UserDet = new UserProfile();
                UserDet = db.UserProfiles.Where(a => a.UserId == UserID).FirstOrDefault();
                TEPOHeaderStructure PODet = new TEPOHeaderStructure();
                PODet = db.TEPOHeaderStructures.Where(a => a.Uniqueid == POID).FirstOrDefault();
                TEPOFundCenter FundCenterDetails = new TEPOFundCenter();
                decimal? POValue = 0;
                FundCenterDetails = db.TEPOFundCenters.Where(x => x.IsDeleted == false && x.Uniqueid == PODet.FundCenterID).FirstOrDefault();
                POValue = db.TEPOItemStructures.Where(x => x.IsDeleted == false && x.POStructureId == PODet.Uniqueid).Sum(a => a.TotalAmount);
                if (POValue == null) POValue = 0;
                if (FundCenterDetails == null)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Submit for Approval as Funcenter Details are not Available";
                    return sinfo;
                }
                double poTotal = Convert.ToDouble(POValue);
                POApprovalCondition ApprovalCondition = new POApprovalCondition();
                ApprovalCondition = db.POApprovalConditions.Where(x => x.IsDeleted == false && x.FundCenter == FundCenterDetails.Uniqueid && poTotal >= x.MinAmount && poTotal <= x.MaxAmount).FirstOrDefault();
                if (ApprovalCondition == null)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Submit for Approval as Approval Conditions are not Available";
                    return sinfo;
                }
                List<POMasterApprover> POApproversList = new List<POMasterApprover>();
                POApproversList = db.POMasterApprovers.Where(x => x.IsDeleted == false && x.ApprovalConditionId == ApprovalCondition.UniqueId && x.Type == "Approver").OrderBy(x => x.SequenceId).ToList();
                if (POApproversList.Count <= 0)
                {
                    sinfo.errorcode = 1;
                    sinfo.errormessage = "Failed to Submit for Approval as Approvers are not Available";
                    return sinfo;
                }
                for (int i = 0; i < POApproversList.Count; i++)
                {
                    POMasterApprover POApproversNow = new POMasterApprover();
                    POApproversNow = POApproversList[i];
                    int CSeq = POApproversNow.SequenceId != null ? Convert.ToInt32(POApproversNow.SequenceId) : 0;
                    UserProfile ApproverDet = new UserProfile();
                    TEPOApprover ApprovalSubmit = new TEPOApprover();
                    ApproverDet = db.UserProfiles.Where(a => a.UserId == POApproversNow.ApproverId).FirstOrDefault();
                    ApprovalSubmit.CreatedOn = System.DateTime.Now;
                    ApprovalSubmit.LastModifiedOn = System.DateTime.Now;
                    ApprovalSubmit.CreatedBy = UserDet.UserName;
                    ApprovalSubmit.LastModifiedBy = UserDet.UserName;
                    ApprovalSubmit.SequenceNumber = i + 2;
                    ApprovalSubmit.POStructureId = PODet.Uniqueid;
                    ApprovalSubmit.PurchaseOrderNumber = PODet.Uniqueid.ToString();
                    ApprovalSubmit.ApproverName = ApproverDet.UserName;
                    ApprovalSubmit.Status = "Draft";
                    ApprovalSubmit.ApproverId = ApproverDet.UserId;
                    db.TEPOApprovers.Add(ApprovalSubmit);
                    db.SaveChanges();
                }
                sinfo.errorcode = 0;
                sinfo.errormessage = "Success";
                return res;
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Submit For Approve";
                return res;
            }
        }

        public SuccessInfo POGetApprovedHere(int POUniqueId, int Approver,string ApproverComments)
        {
            SuccessInfo res = new SuccessInfo();
            try
            {
                SAPResponse SResponse = new SAPResponse();
                PurchaseOrderBAL POBal = new PurchaseOrderBAL();
                PurchaseOrderController POCntrl = new PurchaseOrderController();
                TEPOHeaderStructure PODet = new TEPOHeaderStructure();
                PODet = db.TEPOHeaderStructures.Where(a => a.Uniqueid == POUniqueId).FirstOrDefault();
                List<TEPOApprover> ApproverList = new List<TEPOApprover>();
                ApproverList = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a => a.SequenceNumber).ToList();
                int ActualSeq = 0;
                List<TEPOApprover> CurrentApproverList = new List<TEPOApprover>();
                CurrentApproverList = ApproverList.Where(a => a.ApproverId == Approver).OrderBy(a => a.SequenceNumber).ToList();
                int CurrentSeq = CurrentApproverList.Select(a => a.SequenceNumber).OrderBy(a => a).FirstOrDefault();
                if (ActualSeq == CurrentSeq)
                {
                    TEPOApprover CurrentApproverItem = new TEPOApprover();

                    for (int i = 0; i < CurrentApproverList.Count; i++)
                    {
                        CurrentApproverItem = CurrentApproverList[i];
                        if (i == 0)
                        {
                            CurrentApproverItem.Status = "Approved";
                            CurrentApproverItem.LastModifiedOn = DateTime.Now;
                            CurrentApproverItem.ApprovedOn = DateTime.Now;
                            CurrentApproverItem.Comments = ApproverComments;
                            db.Entry(CurrentApproverItem).CurrentValues.SetValues(CurrentApproverItem);
                            db.SaveChanges();
                        }
                        else
                        {
                            if ((CurrentSeq + 1) == CurrentApproverItem.SequenceNumber)
                            {
                                CurrentApproverItem.Status = "Approved";
                                CurrentApproverItem.LastModifiedOn = DateTime.Now;
                                CurrentApproverItem.ApprovedOn = DateTime.Now;
                                CurrentApproverItem.Comments = ApproverComments;
                                db.Entry(CurrentApproverItem).CurrentValues.SetValues(CurrentApproverItem);
                                db.SaveChanges();
                            }
                            else
                            {
                                break;
                            }
                        }
                        CurrentSeq = CurrentApproverItem.SequenceNumber;
                    }
                    List<TEPOApprover> RestApproverList = new List<TEPOApprover>();
                    RestApproverList = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a => a.SequenceNumber).ToList();
                    if (RestApproverList.Count <= 0)
                    {
                        //Sending to SAP
                        if (String.IsNullOrEmpty(PODet.Purchasing_Order_Number))
                        {
                            POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, true);
                            SResponse = POBal.SendPODetailsToSAP(POUniqueId);
                        }
                        else
                        {
                            POCntrl.SetItemDataReadyForSAPPosting(POUniqueId, false);
                            SResponse = POBal.UpdatePODetailsToSAP(POUniqueId);
                        }
                        if (SResponse == null)
                        {
                            res.errorcode = 1;
                            res.errormessage = "Failed to Approve as sending to SAP had some issues";
                            return res;
                        }
                        if (SResponse.ReturnCode.Equals("0"))
                        {
                            PODet.Purchasing_Order_Number = SResponse.PONumber;
                            PODet.ReleaseCode2Status = "Approved";
                            db.Entry(PODet).CurrentValues.SetValues(PODet);
                            db.SaveChanges();
                        }
                        else
                        {
                            res.errorcode = 1;
                            res.errormessage = "Failed to Update PO Number in SAP.Exception:" + SResponse.Message;
                            return res;
                        }
                    }
                    else
                    {
                        TEPOApprover NextApprover = new TEPOApprover();
                        NextApprover = db.TEPOApprovers.Where(a => a.POStructureId == POUniqueId && a.IsDeleted == false && !a.Status.Equals("Approved")).OrderBy(a => a.SequenceNumber).FirstOrDefault();
                        NextApprover.Status = "Pending For Approval";
                        db.Entry(NextApprover).CurrentValues.SetValues(NextApprover);
                        db.SaveChanges();
                        //Update the Next Approver to Pending for Approver
                    }
                }
                res.errorcode = 0;
                res.errormessage = "Success";
                return res;
            }
            catch (Exception ex)
            {
                ExceptionObj.RecordUnHandledException(ex);
                sinfo.errorcode = 1;
                sinfo.errormessage = "Failed To Approve";
                return res;
            }
        }

        public String SubmitValidation(int headID)
        {
            TEPOHeaderStructure POStructure = db.TEPOHeaderStructures.Where(x => x.IsDeleted == false && x.Uniqueid == headID).FirstOrDefault();

            if (POStructure != null)
            {
                PurchaseOrderController POCntrl = new PurchaseOrderController();
                string Purchasing_Order_Number = POStructure.Purchasing_Order_Number;

                SAPResponse sapRespnse = new SAPResponse();

                if (String.IsNullOrEmpty(Purchasing_Order_Number))
                {
                    POCntrl.SetItemDataReadyForSAPPosting(headID, true);
                    sapRespnse = new PurchaseOrderBAL().SendPODetailsToSAP(headID, true);
                    if (sapRespnse.ReturnCode == "1")
                        return sapRespnse.Message;
                }
                else
                {
                    POCntrl.SetItemDataReadyForSAPPosting(headID, false);
                    sapRespnse = new PurchaseOrderBAL().UpdatePODetailsToSAP(headID, true);
                    if (sapRespnse.ReturnCode == "1")
                        return sapRespnse.Message;
                }

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

    }
    public class SearchPODet
    {
        public int HeaderUniqueid { get; set; }
        public string ProjectCodes { get; set; }
        public string WbsHeads { get; set; }
        public string Purchasing_Order_Number { get; set; }
        public string PoTitle { get; set; }
        public DateTime? Purchasing_Document_Date { get; set; }
        public string POStatus { get; set; }
        public string VendorName { get; set; }
        public decimal? Amount { get; set; }
        public string ManagerName { get; set; }
        public string Version { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? CreatedBy { get; set; }
        public bool isRevisionAllowed { get; set; }
        public bool isCurrentApprover { get; set; }
        public bool? IsNewPO { get; set; }
    }

    public class HSNObject
    {
        public HSN HSNFilter { get; set; }
        public int pageNumber { get; set; }
        public int pagePerCount { get; set; }

    }

    public class HSN
    {
        public String Applicability { get; set; }
        public String Destination { get; set; }
        public String Vend_Region { get; set; }
        public String Delivery_Plant { get; set; }
        public String HSN_Code { get; set; }
        public String Material_GST_Appl { get; set; }
        public String Vend_GST_Appl { get; set; }
        public String TAX_Type { get; set; }
        public String TAX_Code { get; set; }
        public String TAX_Rate { get; set; }
    }

    public class HSNObjClass
    {
       public string ApplicableTo {get; set;}
       public string DestinationCountry {get; set;}
       public string VendorRegionDescription {get; set;}
       public string DeliveryPlantRegionDescription {get; set;}
       public string HSNCode {get; set;}
       public string MaterialGSTApplicability { get; set;}
       public string VendorGSTApplicability { get; set;}
       public DateTime? ValidFrom {get; set;}
       public DateTime? ValidTo {get; set;}
       public string TaxType {get; set;}
       public string TaxCode {get; set;}
       public decimal? TaxRate {get; set;}
       public int UniqueID {get; set;}
       public DateTime? LastModifiedOn {get; set;}
       public string UserName {get; set;}
       public bool IsDeleted { get; set; }
    }

}
