using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using Rotativa.Options;
using System.Web.Hosting;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace PO.Controllers
{
    public class POGeneratePDFController : Controller
    {
        public TETechuvaDBContext db = new TETechuvaDBContext();
        SuccessInfo sinfo = new SuccessInfo();
        COOutputs result = new COOutputs();
        RecordException ExceptionObj = new RecordException();
        PurchaseOrderBAL POBAL = new PurchaseOrderBAL();
        public POGeneratePDFController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        public ActionResult PODefaultView(POInput POIn)
        {
            string downloadFilePath = string.Empty;
            string Page = "0"; var ToPage = "0";

            PurchaseHeaderStructureDetail HeaderStructureDetails = new PurchaseHeaderStructureDetail();
            HeaderStructureDetails = POBAL.GetPurchaseHeaderStructureDetails(POIn.POID);

            string customSwitches = string.Format("--footer-html  \"{0}\" " +
                                                "--header-html \"{1}\" ",
                                                Url.Action("AFSFooter", "POGeneratePDF", new { PageNumber = Page, TotalPage = ToPage, VendorName = HeaderStructureDetails.VendorName, POManager = HeaderStructureDetails.POManager, CompanyName = HeaderStructureDetails.CompanyName, POStatus = HeaderStructureDetails.POStatus }, "http"),
                                                Url.Action("AFSHeader", "POGeneratePDF", new { }, "http"));
            var actionResult = new Rotativa.ActionAsPdf("PurchaseOrderPDFView", new
            {
                POID = POIn.POID
            })
            {
                FileName = "PurchaseOrder.pdf",
                PageSize = Size.A4,
                PageMargins = { Left = 10, Right = 10, Top = 10, Bottom = 25 },
                CustomSwitches = customSwitches

            };
            // string filePath = WebConfigurationManager.AppSettings["DMSDownloadedFiles"];
            //filePath = filePath + "/";
            string filePath = HostingEnvironment.MapPath("~/UploadDocs/");
            string filename = string.Empty;
            filename = DateTime.Now.ToString();
            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");


            var byteArray = actionResult.BuildPdf(ControllerContext);
            var fileStream = new FileStream(filePath + "\\PurchaseOrder-" + filename + ".pdf", FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();
            string base64String = string.Empty;
            base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            string url = WebConfigurationManager.AppSettings["DMSDownloadedFiles"];
            downloadFilePath = url + "/" + "PurchaseOrder-" + filename + ".pdf";
            sinfo.errormessage = "Success";
            sinfo.errorcode = 0;
            result.info = sinfo;
            result.Result = downloadFilePath;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AFSFooter(string Page, string ToPage, string VendorName, string POManager, string CompanyName, string POStatus)
        {
            ViewBag.VendorName = VendorName;
            ViewBag.POManager = POManager;
            ViewBag.CompanyName = CompanyName;
            ViewBag.POStatus = POStatus;
            ViewBag.Page = Page;
            ViewBag.ToPage = ToPage;
            return View();
        }
        public ActionResult AFSHeader()
        {
            return View();
        }

        public string CurrencyFormat(string Number, string countryCode)
        {
            string Result = "0.00";
            if (Number != "")
            {
                decimal parsed = decimal.Parse(Number, CultureInfo.InvariantCulture);
                //CultureInfo CurrencyFormat = new CultureInfo("hi-IN");
                CultureInfo CurrencyFormat = new CultureInfo(countryCode);
                Result = string.Format(CurrencyFormat, "{0:C}", parsed);
                if (countryCode.Equals("hi-IN"))
                    Result = Result.Replace("₹", "");
                else
                    Result = Result.Replace("$", "");
                return Result;
            }
            return Result;
        }

        public ActionResult PurchaseOrderPDFView(int POID)
        {
            POPDFModel PODetails = new POPDFModel();
            string htmlContent = GetHtmlStringByProjectTemplateId("POTemplate.html");
            string CompanyLogoPath = string.Empty;
            string ServiceProviderPath = WebConfigurationManager.AppSettings["ServiceProviderUrl"];
            string staticcompanylogo = WebConfigurationManager.AppSettings["POCompanyLogo"];
            if (!string.IsNullOrEmpty(ServiceProviderPath))
                CompanyLogoPath = ServiceProviderPath;
            PODetails = POBAL.GetDataForPOPDF(POID);

            String OrdType = (from HeadStruct in db.TEPOHeaderStructures
                              join OrdTy in db.TEPurchase_OrderTypes on HeadStruct.PO_OrderTypeID equals OrdTy.UniqueId
                              where HeadStruct.Uniqueid == POID
                              select OrdTy.Code).FirstOrDefault();
            if (PODetails == null) {
                ViewBag.htmltext = "PO Details Not Found.";
            }
            else
            {
              
                var cultureInfo = new CultureInfo("hi-IN");
                var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                numberFormatInfo.CurrencySymbol = "";

                ViewBag.WaterMark = "";
                string CompanyLogo = string.Empty;
                string POSignatoryHtml = string.Empty;
                if (PODetails.PurchaseHeaderStructureDetails != null)
                {
                    string PODate = string.Empty;
                    if (PODetails.PurchaseHeaderStructureDetails.PODate != null) PODate = Convert.ToDateTime(PODetails.PurchaseHeaderStructureDetails.PODate).ToString("dd-MMM-yyyy");
                    CompanyLogo = CompanyLogoPath + PODetails.PurchaseHeaderStructureDetails.CompanyLogo;
                    htmlContent = htmlContent.Replace("##COMPANYNAME##", PODetails.PurchaseHeaderStructureDetails.CompanyName != null ? PODetails.PurchaseHeaderStructureDetails.CompanyName : "");
                    htmlContent = htmlContent.Replace("##COMPANYADDRESS##", PODetails.PurchaseHeaderStructureDetails.CompanyAddress);
                    htmlContent = htmlContent.Replace("##COMPANYCIN##", PODetails.PurchaseHeaderStructureDetails.CompanyCIN);
                    htmlContent = htmlContent.Replace("##COMPANYGSTIN##", PODetails.PurchaseHeaderStructureDetails.CompanyGSTIN);
                    htmlContent = htmlContent.Replace("##PROJECTORFUN##", PODetails.PurchaseHeaderStructureDetails.ProjectOrFnc);
                    // htmlContent = htmlContent.Replace("##COMPANYLOGO##", CompanyLogoPath + PODetails.PurchaseHeaderStructureDetails.CompanyLogo);
                    htmlContent = htmlContent.Replace("##COMPANYLOGO##", staticcompanylogo);

                    htmlContent = htmlContent.Replace("##WBSCODE##", PODetails.WBSCode);
                    htmlContent = htmlContent.Replace("##PONUMBER##", PODetails.PurchaseHeaderStructureDetails.PurchaseOrderNo);
                    htmlContent = htmlContent.Replace("##POMANAGER##", PODetails.PurchaseHeaderStructureDetails.POManager);
                    htmlContent = htmlContent.Replace("##PODATE##", PODate);
                    htmlContent = htmlContent.Replace("##REVISION##", PODetails.PurchaseHeaderStructureDetails.Revisioin);
                    htmlContent = htmlContent.Replace("##VENDORNAME##", PODetails.PurchaseHeaderStructureDetails.VendorName);
                    htmlContent = htmlContent.Replace("##VENDORADDRESS##", PODetails.PurchaseHeaderStructureDetails.VendorAddress);

                    string VendorBillingCity = string.Empty;
                    string VendorShippingFrmAdd = string.Empty;
                    VendorShippingFrmAdd += PODetails.PurchaseHeaderStructureDetails.ShipFrom;
                    if (PODetails.PurchaseHeaderStructureDetails.VendorBillingCity != null && PODetails.PurchaseHeaderStructureDetails.VendorBillingCity != "")
                    {
                        VendorBillingCity = "<br><span style='font-weight:normal;'>" + PODetails.PurchaseHeaderStructureDetails.VendorBillingCity + "</span>";
                        VendorShippingFrmAdd += ", " + PODetails.PurchaseHeaderStructureDetails.VendorBillingCity;
                    }
                    string VendorBillingState = string.Empty;
                    if (PODetails.PurchaseHeaderStructureDetails.VendorBillingState != null && PODetails.PurchaseHeaderStructureDetails.VendorBillingState != "")
                    {
                        VendorBillingState = "<br><span style='font-weight:normal;'>" + PODetails.PurchaseHeaderStructureDetails.VendorBillingState + "</span>";
                        VendorShippingFrmAdd += ", " + PODetails.PurchaseHeaderStructureDetails.VendorBillingState;
                    }
                    string VendorBillingCountry = string.Empty;
                    if (PODetails.PurchaseHeaderStructureDetails.VendorBillingCountry != null && PODetails.PurchaseHeaderStructureDetails.VendorBillingCountry != "")
                    {
                        VendorBillingCountry = "<br><span style='font-weight:normal;'>" + PODetails.PurchaseHeaderStructureDetails.VendorBillingCountry + "</span>";
                        VendorShippingFrmAdd += ", " + PODetails.PurchaseHeaderStructureDetails.VendorBillingCountry;
                    }
                    string VendorBillingPostalCode = string.Empty;
                    if (PODetails.PurchaseHeaderStructureDetails.VendorBillingPostalCode != null && PODetails.PurchaseHeaderStructureDetails.VendorBillingPostalCode != "")
                    {
                        VendorBillingPostalCode = "<br><span style='font-weight:normal;'>" + PODetails.PurchaseHeaderStructureDetails.VendorBillingPostalCode + "</span>";
                        VendorShippingFrmAdd += ", " + PODetails.PurchaseHeaderStructureDetails.VendorBillingPostalCode;
                    }
                    htmlContent = htmlContent.Replace("##VENDORBCITY##", VendorBillingCity);
                    htmlContent = htmlContent.Replace("##VENDORBPOSTAL##", VendorBillingPostalCode);
                    htmlContent = htmlContent.Replace("##VENDORBSTATE##", VendorBillingState);
                    htmlContent = htmlContent.Replace("##VENDORBCOUNTRY##", VendorBillingCountry);

                    htmlContent = htmlContent.Replace("##VENDORCODE##", PODetails.PurchaseHeaderStructureDetails.VendorCode);
                    htmlContent = htmlContent.Replace("##VENDORCIN##", PODetails.PurchaseHeaderStructureDetails.VendorCIN);
                    htmlContent = htmlContent.Replace("##VENDORGSTIN##", PODetails.PurchaseHeaderStructureDetails.VendorGSTIN);
                    htmlContent = htmlContent.Replace("##CURRENCY##", PODetails.PurchaseHeaderStructureDetails.VendorCurrency);
                    htmlContent = htmlContent.Replace("##POTITLE##", PODetails.PurchaseHeaderStructureDetails.POTitle);
                    htmlContent = htmlContent.Replace("##PODESC##", PODetails.PurchaseHeaderStructureDetails.PODescripton);
                    htmlContent = htmlContent.Replace("##SHIPTO##", PODetails.PurchaseHeaderStructureDetails.ShipTo);
                    htmlContent = htmlContent.Replace("##SHIPFROM##", VendorShippingFrmAdd);
                    htmlContent = htmlContent.Replace("##POSTATUS##", PODetails.PurchaseHeaderStructureDetails.POStatus);
                    if (PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                    {
                        htmlContent = htmlContent.Replace("##POTOTALVALUE##", CurrencyFormat(PODetails.POTotalAmount.ToString(), "hi-IN"));
                        htmlContent = htmlContent.Replace("##POTOTATAXLVALUE##", CurrencyFormat(PODetails.POTotalTaxes.ToString(), "hi-IN"));
                        htmlContent = htmlContent.Replace("##POTOTALGROSSVALUE##", CurrencyFormat(PODetails.POTotalGrossAmount.ToString(), "hi-IN"));
                        htmlContent = htmlContent.Replace("##POTOTALPAYMENTVALUE##", CurrencyFormat(PODetails.POTotalPaymentTerms.ToString(), "hi-IN"));
                    }
                    else
                    {
                        htmlContent = htmlContent.Replace("##POTOTALVALUE##", CurrencyFormat(PODetails.POTotalAmount.ToString(), "en-US"));
                        htmlContent = htmlContent.Replace("##POTOTATAXLVALUE##", CurrencyFormat(PODetails.POTotalTaxes.ToString(), "en-US"));
                        htmlContent = htmlContent.Replace("##POTOTALGROSSVALUE##", CurrencyFormat(PODetails.POTotalGrossAmount.ToString(), "en-US"));
                        htmlContent = htmlContent.Replace("##POTOTALPAYMENTVALUE##", CurrencyFormat(PODetails.POTotalPaymentTerms.ToString(), "en-US"));
                    }
                    //htmlContent = htmlContent.Replace("##POTOTALVALUE##", Convert.ToDecimal(PODetails.POTotalAmount).ToString("#,##,##,##0.00"));
                    //htmlContent = htmlContent.Replace("##POTOTATAXLVALUE##", Convert.ToDecimal(PODetails.POTotalTaxes).ToString("#,##,##,##0.00"));
                    //htmlContent = htmlContent.Replace("##POTOTALGROSSVALUE##", Convert.ToDecimal(PODetails.POTotalGrossAmount).ToString("#,##,##,##0.00"));
                    //htmlContent = htmlContent.Replace("##POTOTALPAYMENTVALUE##", Convert.ToDouble(PODetails.POTotalPaymentTerms).ToString("#,##,##,##0.00"));
                    if (PODetails.PurchaseHeaderStructureDetails.POStatus == "Draft" || PODetails.PurchaseHeaderStructureDetails.POStatus == "Pending For Approval")
                    {
                        ViewBag.WaterMark = "Draft";
                        htmlContent = htmlContent.Replace("##PODRAFTTEXT##", "");
                    }
                    else
                    {
                        htmlContent = htmlContent.Replace("##PODRAFTTEXT##", "<tr><th style='border:1px solid rgba(202, 202, 202,1);text-align:left;color:#4F94CD;' colspan = '3'>This is a LEGALLY BINDING AGREEMENT. Please READ IT CAREFULLY. If you do not understand the impact of this Agreement, please consult your attorney BEFORE signing.</th></tr>");
                        //POSignatoryHtml += "<hr style='margin: 2px 0;border:1px solid #262626;'>";
                        //POSignatoryHtml += "<table style='width: 100%;border-collapse:collapse;font-size:9.5pt;margin-top:100px;' cellpadding = '2'>";
                        //POSignatoryHtml += "<tr>";
                        //POSignatoryHtml += "<td style = 'text-align:left;width:40%;'>Authorised Signatory</td>";
                        //POSignatoryHtml += "<td style = 'text-align:left;width:30%;'>PO Manager</td>";
                        //POSignatoryHtml += "<td style = 'text-align:right;width:30%;'>Authorised Signatory</td>";
                        //POSignatoryHtml += "</tr>";
                        //POSignatoryHtml += "<tr>";
                        //POSignatoryHtml += "<td style = 'text-align:left;' ><b> " + PODetails.PurchaseHeaderStructureDetails.VendorName + " </b></td>";
                        //POSignatoryHtml += "<td style = 'text-align:left;' ><b> " + PODetails.PurchaseHeaderStructureDetails.POManager + " </b></td>";
                        //POSignatoryHtml += "<td style = 'text-align:right;' colspan = '2' ><b> " + PODetails.PurchaseHeaderStructureDetails.CompanyName + " </b></td>";
                        //POSignatoryHtml += "</tr>";
                        //POSignatoryHtml += "</table>";
                        //POSignatoryHtml += "<hr style='margin: 2px 0;border: 1px solid #262626;'>";
                    }
                }
                else
                {
                    htmlContent = htmlContent.Replace("##COMPANYNAME##", "");
                    htmlContent = htmlContent.Replace("##COMPANYADDRESS##", "");
                    htmlContent = htmlContent.Replace("##COMPANYCIN##", "");
                    htmlContent = htmlContent.Replace("##COMPANYGSTIN##", "");
                    htmlContent = htmlContent.Replace("##PROJECTORFUN##", "");
                    htmlContent = htmlContent.Replace("##COMPANYLOGO##", "");
                    htmlContent = htmlContent.Replace("##WBSCODE##", "");
                    htmlContent = htmlContent.Replace("##PONUMBER##", "");
                    htmlContent = htmlContent.Replace("##POMANAGER##", "");
                    htmlContent = htmlContent.Replace("##PODATE##", "");
                    htmlContent = htmlContent.Replace("##REVISION##", "");
                    htmlContent = htmlContent.Replace("##VENDORNAME##", "");
                    htmlContent = htmlContent.Replace("##VENDORADDRESS##", "");
                    htmlContent = htmlContent.Replace("##VENDORCODE##", "");
                    htmlContent = htmlContent.Replace("##VENDORCIN##", "");
                    htmlContent = htmlContent.Replace("##VENDORGSTIN##", "");
                    htmlContent = htmlContent.Replace("##CURRENCY##", "");
                    htmlContent = htmlContent.Replace("##POTITLE##", "");
                    htmlContent = htmlContent.Replace("##PODESC##", "");
                    htmlContent = htmlContent.Replace("##SHIPTO##", "");
                    htmlContent = htmlContent.Replace("##SHIPFROM##", "");
                    htmlContent = htmlContent.Replace("##POSTATUS##", "");
                    htmlContent = htmlContent.Replace("##POTOTALVALUE##", "");
                    htmlContent = htmlContent.Replace("##POTOTATAXLVALUE##", "");
                    htmlContent = htmlContent.Replace("##POTOTALGROSSVALUE##", "");
                    htmlContent = htmlContent.Replace("##POTOTALPAYMENTVALUE##", "");
                    htmlContent = htmlContent.Replace("##PODRAFTTEXT##", "");

                }

                //Getting PO Materials Starts
                string POMaterialsHtml = string.Empty;
                List<Mat_Serv_Seq> SeqMat_Serv_List = this.Seq_Mat_Service(PODetails, POID);

                #region
                if (SeqMat_Serv_List != null)
                {
                    int iss = 1;
                    int HeadCount = 0;
                    if (SeqMat_Serv_List.Count > 0)
                    {

                        foreach (Mat_Serv_Seq HeadItemData in SeqMat_Serv_List)
                        {
                            if (HeadItemData.ServHead != null)
                            {
                                HeadCount++;
                                var POData = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == HeadItemData.ServHead.UniqueID).ToList();

                                decimal? IGST;
                                decimal? SGST;
                                decimal? CGST;
                                decimal? HeadTotal;
                                decimal TaxTotal;
                                decimal GrossAmount;

                                // Start of Taxes
                                String Item = POData.Select(x => x.ItemNo).FirstOrDefault();
                                var TaxRates = PODetails.PurchaseItemStructureDetails.Where(x => x.Item_Category == "D" && x.Assignment_Category == "P" && x.ItemNo == Item).FirstOrDefault();
                                if (TaxRates != null)
                                {
                                    IGST = Convert.ToDecimal(TaxRates.IGSTRate);
                                    SGST = Convert.ToDecimal(TaxRates.SGSTRate);
                                    CGST = Convert.ToDecimal(TaxRates.CGSTRate);
                                    HeadTotal = POData.Where(x => x.Item_Category != "D" && x.Assignment_Category != "P").Sum(x => x.Amount);
                                    TaxTotal = Convert.ToDecimal(((HeadTotal * IGST) / 100) + ((HeadTotal * SGST) / 100) + ((HeadTotal * CGST) / 100));
                                    GrossAmount = Convert.ToDecimal(TaxTotal + HeadTotal);

                                }
                                else
                                {
                                    IGST = Convert.ToDecimal(POData.Select(x => x.IGSTRate).FirstOrDefault());
                                    SGST = Convert.ToDecimal(POData.Select(x => x.SGSTRate).FirstOrDefault());
                                    CGST = Convert.ToDecimal(POData.Select(x => x.CGSTRate).FirstOrDefault());
                                    HeadTotal = POData.Sum(x => x.Amount);
                                    TaxTotal = Convert.ToDecimal(((HeadTotal * IGST) / 100) + ((HeadTotal * SGST) / 100) + ((HeadTotal * CGST) / 100));
                                    GrossAmount = Convert.ToDecimal(TaxTotal + HeadTotal);
                                }
                                //End of the Taxes

                                String bgSstring = "ServiceOrder";
                                
                                POMaterialsHtml += "<tr>";
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgSstring + "' class='font4numbers'>" + HeadCount + "</td>";
                                String Description = String.Empty;

                                if (!String.IsNullOrEmpty(HeadItemData.ServHead.Description))
                                    Description = " - " + HeadItemData.ServHead.Description;

                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:left;background:rgba(202, 202, 202,1);'>" + HeadItemData.ServHead.Title + Description + "</td>";
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                                if(PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                                {
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CurrencyFormat(HeadTotal.ToString(), "hi-IN") + "</td>";
                                }
                                else
                                {
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" +
                                        CurrencyFormat(HeadTotal.ToString(), "en-US") + "</td>";
                                }
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + IGST + "%</td>";
                             
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + SGST + "%</td>";
                                
                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CGST + "%</td>";
                                if (PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                                {
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CurrencyFormat(TaxTotal.ToString(), "hi-IN") + "</td>";
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CurrencyFormat(GrossAmount.ToString(), "hi-IN") + "</td>";

                                }
                                else
                                {
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CurrencyFormat(TaxTotal.ToString(), "en-US") + "</td>";
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CurrencyFormat(GrossAmount.ToString(), "en-US") + "</td>";
                                }

                                POMaterialsHtml += "</tr>";
                            }
                            if (HeadItemData.ItemS.Count > 0)
                            {
                                int ItemCount = 0;
                                foreach (var data in HeadItemData.ItemS)
                                {

                                    var bgstring = "";
                                    //bgstring = data.ItemType == "ServiceOrder" ? "background:rgba(202, 202, 202,1);" : "";

                                    // if (data.BrandName == null) data.BrandName = "-";
                                    // if (data.ManufactureCode == null) data.ManufactureCode = "-";
                                    POMaterialsHtml += "<tr>";
                                    bool Show = true;
                                    if (HeadItemData.ServHead != null)
                                    {
                                        if (data.Assignment_Category == "P" && data.Item_Category == "D")
                                            Show = false;
                                        else
                                        {
                                            ItemCount++;
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgstring + "' class='font4numbers'>" + HeadCount + "." + ItemCount + "</td>";
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;'>" + data.POItemLongText;
                                            
                                            POMaterialsHtml += "</td>";
                                        }
                                    }
                                    else
                                    {
                                        HeadCount++;
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgstring + "' class='font4numbers'>" + HeadCount + "</td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;'>" + data.POItemLongText;
                                        if (!String.IsNullOrEmpty(data.BrandName))
                                            POMaterialsHtml += "<br/><b>Manufacturer/Brand : </b>" + data.BrandName;
                                        if (!String.IsNullOrEmpty(data.ManufactureCode))
                                            POMaterialsHtml += "<br/> <b>Manufacturer’s Code : </b>" + data.ManufactureCode;
                                        POMaterialsHtml += "</td>";
                                    }
                                    if (Show)
                                    { 
                                   

                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;" + bgstring + "'></td>";
                                        if (data.Quantity.Contains('.'))
                                        {
                                            String[] x = data.Quantity.ToString().Split('.');

                                            if(x[1].Count() > 1)
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + data.Quantity.Substring(0, (data.Quantity.IndexOf('.') + 3)) + "</td>";
                                            else
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" +  data.Quantity + "</td>";

                                        }
                                        else
                                        {
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + data.Quantity + "</td>";
                                        }
                                    
                                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;width: 45px;" + bgstring + "'>" + data.Unit + "</td>";
                                    if (PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                                        {
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + CurrencyFormat(data.Rate.ToString(), "hi-IN") + "</td>";
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + CurrencyFormat(data.Amount.ToString(), "hi-IN") + "</td>";
                                        }
                                        else
                                        {
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + CurrencyFormat(data.Rate.ToString(), "en-US") + "</td>";
                                            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + CurrencyFormat(data.Amount.ToString(), "en-US") + "</td>";
                                        }
                                    if (HeadItemData.ServHead != null)
                                    {
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                                    }
                                    else
                                    {
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>"+ data.IGSTRate+ "</td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>"+ data.SGSTRate + "</td>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>" + data.CGSTRate + "</td>";
                                            if (PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                                            {
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>" + CurrencyFormat(data.TaxAmount.ToString(), "hi-IN") + "</td>";
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>" +     CurrencyFormat(data.GrossAmount.ToString(), "hi-IN") + "</td>";
                                            }
                                            else
                                            {
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>" +    CurrencyFormat(data.TaxAmount.ToString(), "en-US") + "</td>";
                                                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'>" + CurrencyFormat(data.GrossAmount.ToString(), "en-US") + "</td>";
                                            }
                                    }

                                    POMaterialsHtml += "</tr>";
                                    if (data.ItemType == "MaterialOrder" && data.POTechnicalSpecifiactionList.Count > 0)
                                    {
                                        POMaterialsHtml += "<tr>";
                                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);'></td>";
                                        POMaterialsHtml += "<td colspan='11' style='border:1px solid rgba(202, 202, 202,1);text-align:left;'> <b> Technical Specifications </b>: <span>";
                                        string tempTechSpecs = string.Empty;
                                        int cnt = 0;
                                        foreach (var techspc in data.POTechnicalSpecifiactionList)
                                        {
                                            if (techspc.Value != null && techspc.Value != "" && techspc.Value != "-- NA --" && techspc.Name != "Specification Sheet")
                                            {
                                                cnt++;
                                                if (cnt < data.POTechnicalSpecifiactionList.Count)
                                                    tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType + "&nbsp; | &nbsp; ";
                                                else
                                                    tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType;
                                            }
                                        }
                                        //if (data.POTechnicalSpecifiactionList.Count == 0)
                                        //{
                                        //    tempTechSpecs += "No Technical Specifications Available For Material - " + data.POItemName;
                                        //}
                                        POMaterialsHtml += tempTechSpecs + "</span>";
                                        POMaterialsHtml += "</td>";
                                        POMaterialsHtml += "</tr>";
                                    }
                                    iss++;
                                }
                            }
                            }
                        }
                    }

                }
                #endregion

                #region Old Code of the Purchase Items
                //if (PODetails.PurchaseItemStructureDetails != null)
                //{
                //    int iss = 1;
                //    int HeadCount = 0;
                //    if (PODetails.POServiceHeader.Count > 0)
                //    {

                //        foreach (var HeadItemData in PODetails.POServiceHeader)
                //        {
                //            HeadCount++;
                //            var POData = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == HeadItemData.UniqueID).ToList();
                //            String bgSstring = "ServiceOrder";
                //            decimal? HeadTotal = POData.Sum(x => x.Amount);
                //            POMaterialsHtml += "<tr>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgSstring + "' class='font4numbers'>" + HeadCount + "</td>";
                //            String Description = String.Empty;

                //            if (!String.IsNullOrEmpty(HeadItemData.Description))
                //                Description = " - " + HeadItemData.Description;

                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:left;background:rgba(202, 202, 202,1);'>" + HeadItemData.Title + Description + "</td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'></td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + Convert.ToDecimal(HeadTotal).ToString("#,##,##,##0.00") + "</td>";
                //            decimal? IGST = Convert.ToDecimal(POData.Select(x => x.IGSTRate).FirstOrDefault());
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + IGST + "%</td>";
                //            decimal? SGST = Convert.ToDecimal(POData.Select(x => x.SGSTRate).FirstOrDefault());
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + SGST + "%</td>";
                //            decimal? CGST = Convert.ToDecimal(POData.Select(x => x.CGSTRate).FirstOrDefault());
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + CGST + "%</td>";

                //            decimal TaxTotal = Convert.ToDecimal(((HeadTotal * IGST) / 100) + ((HeadTotal * SGST) / 100) + ((HeadTotal * CGST) / 100));

                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + TaxTotal.ToString("#,##,##,##0.00") + "</td>";
                //            decimal GrossAmount = Convert.ToDecimal(TaxTotal + HeadTotal);
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:right;background:rgba(202, 202, 202,1);'>" + GrossAmount.ToString("#,##,##,##0.00") + "</td>";
                //            POMaterialsHtml += "</tr>";

                //            if (POData.Count > 0)
                //            {
                //                int ItemCount = 0;
                //                foreach (var data in POData)
                //                {
                //                    ItemCount++;
                //                    var bgstring = "";
                //                    //bgstring = data.ItemType == "ServiceOrder" ? "background:rgba(202, 202, 202,1);" : "";

                //                    // if (data.BrandName == null) data.BrandName = "-";
                //                    // if (data.ManufactureCode == null) data.ManufactureCode = "-";
                //                    POMaterialsHtml += "<tr>";
                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgstring + "' class='font4numbers'>" + HeadCount + "." + ItemCount + "</td>";

                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;'>" + data.POItemShortText + "</br>" + data.POItemLongText + "</td>";

                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;" + bgstring + "'></td>";
                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + data.Quantity + "</td>";
                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;width: 45px;" + bgstring + "'>" + data.Unit + "</td>";
                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.Rate).ToString("#,##,##,##0.00") + "</td>";
                //                    POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.Amount).ToString("#,##,##,##0.00") + "</td>";
                //                    if (data.ItemUniqueId > 0)
                //                    {
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'></td>";
                //                    }
                //                    else
                //                    {
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                    }

                //                    POMaterialsHtml += "</tr>";
                //                    if (data.ItemType == "MaterialOrder" && data.POTechnicalSpecifiactionList.Count > 0)
                //                    {
                //                        POMaterialsHtml += "<tr>";
                //                        POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);'></td>";
                //                        POMaterialsHtml += "<td colspan='11' style='border:1px solid rgba(202, 202, 202,1);text-align:left;'> <b> Technical Specifications </b>: <span>";
                //                        string tempTechSpecs = string.Empty;
                //                        int cnt = 0;
                //                        foreach (var techspc in data.POTechnicalSpecifiactionList)
                //                        {
                //                            if (techspc.Value != null && techspc.Value != "" && techspc.Value != "-- NA --" && techspc.Name != "Specification Sheet")
                //                            {
                //                                cnt++;
                //                                if (cnt < data.POTechnicalSpecifiactionList.Count)
                //                                    tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType + "&nbsp; | &nbsp; ";
                //                                else
                //                                    tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType;
                //                            }
                //                        }
                //                        //if (data.POTechnicalSpecifiactionList.Count == 0)
                //                        //{
                //                        //    tempTechSpecs += "No Technical Specifications Available For Material - " + data.POItemName;
                //                        //}
                //                        POMaterialsHtml += tempTechSpecs + "</span>";
                //                        POMaterialsHtml += "</td>";
                //                        POMaterialsHtml += "</tr>";
                //                    }
                //                    iss++;
                //                }
                //            }
                //        }
                //    }

                //    var POItems = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == 0 || x.POHeaderStructureid == null).ToList();
                //    if (POItems.Count > 0)
                //    {
                //        foreach (var data in POItems)
                //        {

                //            var bgstring = "";
                //            bgstring = data.ItemType == "ServiceOrder" ? "background:rgba(202, 202, 202,1);" : "";

                //            // if (data.BrandName == null) data.BrandName = "-";
                //            // if (data.ManufactureCode == null) data.ManufactureCode = "-";
                //            POMaterialsHtml += "<tr>";
                //            if (data.ItemNo.ToString().Contains('.'))
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgstring + "' class='font4numbers'>" + data.ItemNo + "</td>";
                //            else
                //            {
                //                HeadCount++;
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;" + bgstring + "' class='font4numbers'>" + HeadCount + "</td>";
                //            }
                //            if (data.ItemType == "MaterialOrder")
                //            {
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;'>" + (String.IsNullOrEmpty(data.POItemLongText) ? data.POItemShortText : data.POItemLongText);
                //                if (!string.IsNullOrEmpty(data.BrandName)) POMaterialsHtml += "<br>&nbsp;&nbsp;Brand : " + data.BrandName;
                //                if (!string.IsNullOrEmpty(data.ManufactureCode)) POMaterialsHtml += "<br>&nbsp;&nbsp;ManufactureCode : " + data.ManufactureCode;
                //                POMaterialsHtml += "</td>";
                //            }
                //            else if (data.ItemType == "ServiceOrder")
                //            {
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;background:rgba(202, 202, 202,1);'>" + (String.IsNullOrEmpty(data.POItemLongText) ? data.POItemShortText : data.POItemLongText) + "</td>";
                //            }
                //            else POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;'>" + (String.IsNullOrEmpty(data.POItemLongText) ? data.POItemShortText : data.POItemLongText) + "</td>";

                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;" + bgstring + "'></td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + data.Quantity + "</td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;width: 45px;" + bgstring + "'>" + data.Unit + "</td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.Rate).ToString("#,##,##,##0.00") + "</td>";
                //            POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.Amount).ToString("#,##,##,##0.00") + "</td>";
                //            if (data.ItemUniqueId > 0)
                //            {
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.IGSTRate).ToString() + " %</td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.SGSTRate).ToString() + " %</td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.CGSTRate).ToString() + " %</td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.TaxAmount).ToString("#,##,##,##0.00") + "</td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;" + bgstring + "' class='font4numbers'>" + Convert.ToDecimal(data.GrossAmount).ToString("#,##,##,##0.00") + "</td>";
                //            }
                //            else
                //            {
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;' class='font4numbers'></td>";
                //            }

                //            POMaterialsHtml += "</tr>";
                //            if (data.ItemType == "MaterialOrder" && data.POTechnicalSpecifiactionList.Count > 0)
                //            {
                //                POMaterialsHtml += "<tr  style = 'page-break-inside: avoid;'>";
                //                POMaterialsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);'></td>";
                //                POMaterialsHtml += "<td colspan='11' style='border:1px solid rgba(202, 202, 202,1);text-align:left;'> <b> Technical Specifications </b>: <span>";
                //                string tempTechSpecs = string.Empty;
                //                int cnt = 0;
                //                foreach (var techspc in data.POTechnicalSpecifiactionList)
                //                {
                //                    if (techspc.Value != null && techspc.Value != "" && techspc.Value != "-- NA --" && techspc.Name != "Specification Sheet")
                //                    {
                //                        cnt++;
                //                        if (cnt < data.POTechnicalSpecifiactionList.Count)
                //                            tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType + "&nbsp; | &nbsp; ";
                //                        else
                //                            tempTechSpecs += techspc.Name + ":" + techspc.Value + " &nbsp;" + techspc.ValueType;
                //                    }
                //                }
                //                //if (data.POTechnicalSpecifiactionList.Count == 0)
                //                //{
                //                //    tempTechSpecs += "No Technical Specifications Available For Material - " + data.POItemName;
                //                //}
                //                POMaterialsHtml += tempTechSpecs + "</span>";
                //                POMaterialsHtml += "<br/>&nbsp</td>";
                //                POMaterialsHtml += "</tr>";
                //            }
                //            iss++;
                //        }
                //    }
                //}
                #endregion


                htmlContent = htmlContent.Replace("##POMATERIALS##", POMaterialsHtml);
                //Getting PO Materials Ends

                //Getting Payment Terms Starts
                string POMilestonesHtml = string.Empty;
                if (PODetails.PurchaseVendorPaymentMileStoneDetails != null)
                {
                    int mss = 1;
                    foreach (var data in PODetails.PurchaseVendorPaymentMileStoneDetails)
                    {
                        //int? poHeaderStuId = 0;
                        double? totalRate = 0;
                        //poHeaderStuId = db.TEPOVendorPaymentMilestones.Where(x => x.UniqueId == data.MileStoneID && x.IsDeleted == false).Select(x => x.POHeaderStructureId).FirstOrDefault();
                        //if (poHeaderStuId != null)
                        //{
                            totalRate = Convert.ToDouble(db.TEPOItemStructures.Where(x => x.POStructureId == POID && x.IsDeleted == false).Sum(x => x.TotalAmount));
                        //}
                        if (totalRate != null && data.Amount != null)
                        {
                            data.Percentage = totalRate == data.Amount ? 100 : data.Percentage;
                        }

                        POMilestonesHtml += "<tr>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'>" + mss + "</td>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left;' colspan='5'>" + data.PaymentTerm + "</td>";
                        if (data.PaymentDate != null) POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width:100px;'>" + Convert.ToDateTime(data.PaymentDate).ToString("dd-MMM-yyyy") + "</td>";
                        else POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width:100px;'>-</td>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 50px;'>&nbsp;</td>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 50px;'> &nbsp;</td>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width: 50px;'> &nbsp;</td>";
                        POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width:100px;'> " + Convert.ToDouble(data.Percentage).ToString() + " %</td>";
                        if (PODetails.PurchaseHeaderStructureDetails.VendorCurrency.Equals("INR"))
                        {
                            POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width:100px;' class='font4numbers'>" + CurrencyFormat(data.Amount.ToString(), "hi-IN") + "</td>";
                        }
                        else
                        {
                            POMilestonesHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right;width:100px;' class='font4numbers'>" + CurrencyFormat(data.Amount.ToString(), "en-US") + "</td>";
                        }
                        
                        POMilestonesHtml += "</tr>";
                        mss++;
                    }
                }
                htmlContent = htmlContent.Replace("##POMILESTONES##", POMilestonesHtml);
                //Getting Payment Terms Ends

                //Getting Specific Terms & Conditions Starts
                string POSPLTermsHtml = string.Empty;
                if (PODetails.SpecialTermsAndConditions != null)
                {
                    var Spltanc = PODetails.SpecialTermsAndConditions.GroupBy(a => a.Title).Select(a => a.FirstOrDefault()).ToList();
                    int spss = 1;
                    foreach (var data in Spltanc)
                    {
                        POSPLTermsHtml += "<tbody>";
                        POSPLTermsHtml += "<tr>";
                        POSPLTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'>" + spss + "</td>";
                        POSPLTermsHtml += "<th style = 'border:1px solid rgba(202, 202, 202,1);text-align:left;' colspan='2'>" + data.Title + "</th>";
                        POSPLTermsHtml += "</tr>";
                        foreach (var data1 in PODetails.SpecialTermsAndConditions)
                        {
                            string temp = string.Empty;
                            if (data1.Condition != null && data1.Condition != "")
                            {
                                temp = data1.Condition;
                                temp = temp.Replace("<p><br/></p>", "").Replace("<p style=\"text - align: justify;\"></p>", "").Replace("<p></p>", "").Replace("<br/><br/>", "<br/>").Replace("<p>", "").Replace("<br/></p>", "</p>").Replace("</p>", "<br/>&nbsp;");
                            }
                            if (data1.Title.Equals(data.Title))
                            {
                                POSPLTermsHtml += "<tr >";
                                POSPLTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'></td>";
                                POSPLTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;'></td>";
                                POSPLTermsHtml += "<td style='border:1px solid rgba(202, 202, 202,1); text-align:left; text-align:justify;'>" + temp.Trim() + "</td>";
                                POSPLTermsHtml += "</tr>";
                            }
                        }
                        spss++;
                        POSPLTermsHtml += "</tbody>";
                    }
                }

                string POSPECIFICTermsHtml = string.Empty;
                if (PODetails.SpecificTermsAndConditions != null)
                {
                    var SpecificTandC = PODetails.SpecificTermsAndConditions;
                    int sess = 1;
                    foreach (var data in SpecificTandC)
                    {
                        POSPECIFICTermsHtml += "<tbody>";
                        POSPECIFICTermsHtml += "<tr>";
                        POSPECIFICTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'>" + sess + "</td>";
                        POSPECIFICTermsHtml += "<th style = 'border:1px solid rgba(202, 202, 202,1);text-align:left;' colspan='2'>" + data.Title + "</th>";
                        POSPECIFICTermsHtml += "</tr>";
                        foreach (var dataa in data.SpecSubTitlesList)
                        {
                            int se = 0;
                            foreach (var data1 in dataa.SpecificTCList)
                            {
                                string temp = string.Empty;
                                if (data1.Description != null && data1.Description != "")
                                {
                                    temp = data1.Description;
                                    temp = temp.Replace("<br/>&nbsp;<p", "<p").Replace("<p><br/></p>", "").Replace("<p style=\"text-align: justify;\"></p>", "").Replace("<p></p>", "").Replace("<br/><br/>", "<br/>").Replace("<p>", "").Replace("<br/></p>", "</p>").Replace("</p>", "<br/>&nbsp;").Replace("<br/>&nbsp;<p", "<p");
                                }
                                POSPECIFICTermsHtml += "<tr>";
                                POSPECIFICTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'></td>";
                                //if (se == 0)
                                //{
                                //    POSPECIFICTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;vertical-align:middle;width:15%' rowspan='" + dataa.SpecificTCList.Count + "'>" + dataa.SubTitleDesc + "</td>";
                                //}
                                POSPECIFICTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;vertical-align:middle;width:15%'>" + dataa.SubTitleDesc + "</td>";
                                POSPECIFICTermsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left; text-align:justify; '>" + temp.Trim() + "</td>";
                                POSPECIFICTermsHtml += "</tr>";
                                se++;
                            }
                        }
                        sess++;
                        POSPECIFICTermsHtml += "</tbody >";
                    }
                }
                // change Special to Specific Date 29_06
                htmlContent = htmlContent.Replace("##POSPECIFICTERMS##", POSPECIFICTermsHtml);
                //Getting Special Terms & Conditions Ends

                //Getting Specific Terms & Conditions Starts
                
                // change Specific to Special  Date 29_06
                htmlContent = htmlContent.Replace("##POSPLTERMS##", POSPLTermsHtml);
                //Getting Specific Terms & Conditions Ends

                //Getting General Terms & Conditions Starts
                string POGENTermsHtml = string.Empty;
                if (PODetails.GeneralTermsAndConditions != null)
                {
                    //var Gentanc = PODetails.GeneralTermsAndConditions.GroupBy(a => a.Title).Select(a => a.FirstOrDefault()).OrderBy(a=>a.SequenceId).ToList();
                    var Gentanc = PODetails.GeneralTermsAndConditions;
                    int genss = 1;
                    foreach (var data in Gentanc)
                    {
                        if (!string.IsNullOrEmpty(data.Title))
                        {
                            POGENTermsHtml += "<tbody>";
                            POGENTermsHtml += "<tr>";
                            POGENTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px; margin: 10px ;' class='font4numbers'>" + genss + "</td>";
                            POGENTermsHtml += "<th style = 'border:1px solid rgba(202, 202, 202,1);text-align:left;' colspan='2'>" + data.Title + "</th>";
                            POGENTermsHtml += "</tr>";
                        }
                        foreach (var data1 in PODetails.GeneralTermsAndConditions)
                        {
                            if (!string.IsNullOrEmpty(data1.Title) && !string.IsNullOrEmpty(data.Title))
                            {
                                string temp = string.Empty;
                                if (data1.Condition != null && data1.Condition != "")
                                {
                                    temp = data1.Condition;
                                    temp = temp.Replace("<br/>&nbsp;<p", "<p").Replace("<p><br/></p>", "").Replace("<p style=\"text-align: justify;\"></p>", "").Replace("<p></p>", "").Replace("<br/><br/>", "<br/>").Replace("<p>", "").Replace("<br/></p>", "</p>").Replace("</p>", "<br/>&nbsp;").Replace("<br/>&nbsp;<p", "<p");
                                }
                                if (data1.Title.Equals(data.Title))
                                {
                                    POGENTermsHtml += "<tr >";
                                    POGENTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px;' class='font4numbers'></td>";
                                    POGENTermsHtml += "<td style = 'border:1px solid rgba(202, 202, 202,1);text-align:right;width: 30px; '></td>";
                                    POGENTermsHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:left; text-align:justify; '>" + temp.Trim() + "<br/></td>";
                                    POGENTermsHtml += "</tr>";
                                }
                            }
                        }
                        POGENTermsHtml += "</tbody>";
                        genss++;
                    }
                }
                htmlContent = htmlContent.Replace("##POGENTERMS##", POGENTermsHtml);
                //Getting General Terms & Conditions Ends

                String POAnxHtml = String.Empty;

                #region Annexure
                //Getting Material Annexure Starts
                string POMaterialAnxHtml = string.Empty;
                //String POAnxHead = String.Empty;
                //if ((PODetails.AnnexureSpecifications.Count > 0 || PODetails.ServiceAnnexureSpecifications.Count > 0))
                //{
                //    POAnxHead += "<div class='breakPage'></div>";
                //    POAnxHead += "<table style='width:100%;border-collapse:collapse;font-size:9.5pt;margin-bottom:10px;' cellpadding='2'>";
                //    POAnxHead += "<tr><th style='background:#4F94CD; border:2px solid #4F94CD;color:#FFF;text-align:center;' colspan='3'><b>Annexure-1</b></th></tr>";
                //    POAnxHead += "</table>";
                //}

                //if (PODetails.AnnexureSpecifications != null && PODetails.AnnexureSpecifications.Count > 0)
                //{
                //    POMaterialAnxHtml += POAnxHead;
                //     var AnxMaterial = PODetails.AnnexureSpecifications;
                //    int kjbm = 1;
                //    foreach (var data in AnxMaterial)
                //    {
                //        POMaterialAnxHtml += "<table style = 'width: 100%;border-collapse: collapse;font-size:9.5pt;margin-bottom:10px;' cellpadding='2'>";
                //            POMaterialAnxHtml += "<tr>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);' width='150px'></td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center;vertical-align:middle;'>";
                //                    POMaterialAnxHtml += "<h3> Material Specification Sheet - "+ data.MaterialName + " </h3>";
                //                POMaterialAnxHtml += "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center;vertical-align:middle;' width='200px'>";
                //                    POMaterialAnxHtml += "<img src = '"+ CompanyLogo + "' width='200px' />";
                //                POMaterialAnxHtml += "</td>";
                //            POMaterialAnxHtml += "</tr>";
                //        POMaterialAnxHtml += "</table>";
                //        POMaterialAnxHtml += "<table style='width: 100%;border-collapse: collapse;font-size:9.5pt;' cellpadding = '2'>";
                //            POMaterialAnxHtml += "<tr>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='20px'>#</th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;'>Test Object </th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='100px'>Test Method </th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='100px'>Value 1 </th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='100px'>Value 2 </th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='100px'>Unit of Measurement</th>";
                //                POMaterialAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='100px'>Acceptance Criteria </th>";
                //            POMaterialAnxHtml += "</tr>";
                //            int kbjm = 1;
                //            foreach (var data1 in data.SpecsData)
                //            {

                //                POMaterialAnxHtml += "<tr>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);'>"+ kbjm + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center'>" + data1.TestObject + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center'>" + data1.TestMethod + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right'>" + data1.Value1 + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right'>" + data1.Value2 + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center'>" + data1.UnitOfMeasurement + "</td>";
                //                POMaterialAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center'>" + data1.AcceptanceCriteria + "</td>";
                //                POMaterialAnxHtml += "</tr>";
                //                kbjm++;
                //            }
                //        POMaterialAnxHtml += "</table>";
                //        if (kjbm < AnxMaterial.Count)POMaterialAnxHtml += "<div class='breakPage'></div>";
                //        if (kjbm==AnxMaterial.Count && PODetails.ServiceAnnexureSpecifications.Count>0) POMaterialAnxHtml += "<div class='breakPage'></div>";


                //        kjbm++;
                //    }
                //}
                htmlContent = htmlContent.Replace("##MATERIALANNEXURE##", POMaterialAnxHtml);

                //string POServiceAnxHtml = string.Empty;
                //if (PODetails.ServiceAnnexureSpecifications != null && PODetails.ServiceAnnexureSpecifications.Count > 0)
                //{
                //    var AnxService = PODetails.ServiceAnnexureSpecifications;
                //    int kjbma = 1;

                //    foreach (var data in AnxService)
                //    {
                //        bool ValPrest = false;
                //             POServiceAnxHtml += "<table style = 'width: 100%;border-collapse: collapse;font-size:9.5pt;margin-bottom:10px;' cellpadding='2'>";
                //            POServiceAnxHtml += "<tr>";
                //            POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);' width='150px'></td>";
                //            POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center;vertical-align:middle;'>";
                //            POServiceAnxHtml += "<h3> Service Specification Sheet - " + data.ServiceName + " </h3>";
                //            POServiceAnxHtml += "</td>";
                //            POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:center;vertical-align:middle;' width='200px'>";
                //            POServiceAnxHtml += "<img src = '" + CompanyLogo + "' width='200px' />";
                //            POServiceAnxHtml += "</td>";
                //            POServiceAnxHtml += "</tr>";
                //            POServiceAnxHtml += "</table>";
                //            POServiceAnxHtml += "<table style='width: 100%;border-collapse: collapse;font-size:9.5pt;' cellpadding = '2'>";
                //            POServiceAnxHtml += "<tr>";
                //            POServiceAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='20px'>#</th>";
                //            POServiceAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;' width='250px'>Name</th>";
                //            POServiceAnxHtml += "<th style='border:1px solid rgba(202, 202, 202,1);text-align:center;'>Condition</th>";
                //            POServiceAnxHtml += "</tr>";
                //            int kbjma = 1;
                //            foreach (var data1 in data.SpecsData)
                //            {
                //            ValPrest = true;
                //                if (!string.IsNullOrEmpty(data1.Value))
                //                {
                //                    POServiceAnxHtml += "<tr>";
                //                    POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);'>" + kbjma + "</td>";
                //                    POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right'>" + data1.Name + "</td>";
                //                    POServiceAnxHtml += "<td style='border:1px solid rgba(202, 202, 202,1);text-align:right'>" + data1.Value + "</td>";
                //                    POServiceAnxHtml += "</tr>";
                //                }
                //                kbjma++;
                //            }
                //            POServiceAnxHtml += "</table>";
                //            if (kjbma < AnxService.Count) POServiceAnxHtml += "<div class='breakPage'></div>";
                //        if (ValPrest)
                //        {
                //            if (kjbma == 1)
                //            {
                //                POAnxHtml += POAnxHead;
                //            }
                //            POAnxHtml += POServiceAnxHtml;
                //            POServiceAnxHtml = String.Empty;
                //        }
                //        kjbma++;
                //    }
                //}
                htmlContent = htmlContent.Replace("##SERVICEANNEXURE##", POAnxHtml);
                //Getting Material Annexure Ends
                #endregion

                htmlContent = htmlContent.Replace("##POSIGNATORY##", POSignatoryHtml);
            }
            htmlContent = htmlContent.Replace("\r", "");
            htmlContent = htmlContent.Replace("\n", "");
            htmlContent = htmlContent.Replace("  ", "");
            ViewBag.htmltext = htmlContent;
            return View();
        }

        public List<Mat_Serv_Seq> Seq_Mat_Service(POPDFModel PODetails, int POID)
        {
            List<Mat_Serv_Seq> SeqList = new List<Mat_Serv_Seq>();

            int Mat_Seq = 0; int Service_Seq = 0;

            if (PODetails.PurchaseItemStructureDetails.Where(x => x.ItemNo != null).Count() > 0)
             Mat_Seq = PODetails.PurchaseItemStructureDetails.Where(x =>x.ItemNo != null).Select(v => Convert.ToInt32(v.ItemNo)).Max();

            if (PODetails.POServiceHeader.Where(x => x.ItemNumber != null).Count() > 0)
            Service_Seq = PODetails.POServiceHeader.Where(x => x.ItemNumber != null).Max(x => Convert.ToInt32(x.ItemNumber));

            if (Mat_Seq != 0 || Service_Seq != 0)
            {
                int MaxSeq = 0;

                if (Convert.ToInt32(Mat_Seq) > Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Mat_Seq);
                else if (Convert.ToInt32(Mat_Seq) < Convert.ToInt32(Service_Seq))
                    MaxSeq = Convert.ToInt32(Service_Seq);
                else if(Convert.ToInt32(Mat_Seq) == Convert.ToInt32(Service_Seq))
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
                        ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ItemHead.UniqueID).ToList();
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
                        NullListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ServHead.UniqueID).ToList();
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
                foreach(TEPOServiceHeader ServHead in PODetails.POServiceHeader)
                {
                    Mat_Serv_Seq ListNonSeq = new Mat_Serv_Seq();
                    TEPOServiceHeader TempServ = new TEPOServiceHeader();
                    ListNonSeq.ServHead = ServHead;
                    ListNonSeq.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == ServHead.UniqueID).ToList();
                    SeqList.Add(ListNonSeq);
                }

                Mat_Serv_Seq ListNonSeqMat = new Mat_Serv_Seq();
                ListNonSeqMat.ItemS = PODetails.PurchaseItemStructureDetails.Where(x => x.POHeaderStructureid == null || x.POHeaderStructureid == 0).ToList();
                SeqList.Add(ListNonSeqMat);
            }
            return SeqList;
        }

        public string GetHtmlStringByProjectTemplateId(string POTemplate)
        {
            string htmlcontent = "";
            string fileName = "";
            string filePath = string.Empty;
            var headers = Request.Headers;
            fileName = POTemplate;
            string assemblyFile = string.Empty;
            assemblyFile = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            int index = assemblyFile.LastIndexOf("/bin/");
            string PortFolioDocTemplates = WebConfigurationManager.AppSettings["DocTemplatesPath"];
            string downloadFilePath = PortFolioDocTemplates + "/" + fileName;
            if (System.IO.File.Exists(downloadFilePath))
            {
                htmlcontent = System.IO.File.ReadAllText(downloadFilePath);
            }
            return htmlcontent;
        }
    }

    public class Mat_Serv_Seq
    {
        public TEPOServiceHeader ServHead { get; set; }

        public List<PurchaseItemStructureDetail> ItemS { get; set; }
    }

    public class COOutputs
    {
        public SuccessInfo info { get; set; }
        public string Result { get; set; }
    }

    public class POInput
    {
       public int POID { get; set; }
    }
}