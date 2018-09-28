using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa.MVC;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.IO;

namespace PO.Controllers
{
    public class POPdfController : Controller
    {
        // GET: POPdf
        public ActionResult Index()
        {
            return View();
        }

        public PDFResultDefaultOffer GetPOPdf(string poNumber)
        {
            infoDeatails infDet = new infoDeatails();

            string finalResult = string.Empty;

            PDFResultDefaultOffer pdfResultdefaultoffer = new PDFResultDefaultOffer();
            
            
            string base64String = "Failed to Create PDF, Try again launch";
            //string customSwitches = string.Format("--header-html  \"{0}\" " +
            //                 "--header-spacing \"0\" " + "--dpi \"180\"" + 
            //                 "--footer-html \"{1}\" " +
            //                 "--footer-spacing \"10\" " +
            //                 "--footer-font-size \"10\" " +
            //                 "--header-font-size \"10\" ",
            //                 Url.Action("SamplePDFFooter", "SampleOffer", new { id = mv.UnitID }, "http"),
            //                 Url.Action("FooterTS", "DummyPDF", new { area = "" }, "http"));

            var actionResult = new Rotativa.MVC.ActionAsPdf("GeneratePDF", new
            {
                poNumber= poNumber
            }) //some route values)
            {
                FileName = "Test.pdf", 
                 
              
            };

            string filePath = HostingEnvironment.MapPath("~/UploadDocs/");
            string filename = string.Empty;
            filename = DateTime.Now.ToString();
            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
            //byte[] bytes = System.IO.File.ReadAllBytes(new RazorPDF.PdfResult(0,"FinalPDFNEWTS") );
            var byteArray = actionResult.BuildPdf(ControllerContext);
            var fileStream = new FileStream(filePath + "\\pdf-" + filename + ".pdf", FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();

            base64String = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            pdfResultdefaultoffer.PathToFile = "pdf-" + filename;
            pdfResultdefaultoffer.Base64String = base64String;
            return pdfResultdefaultoffer;

             
        }

        public ActionResult GeneratePDF(string poNumber)
        {




            return View();
        }

    }

    public class infoDeatails
    {
        public Successinfo info { get; set; }
        public PDFResultDefaultOffer result { get; set; }
    }

    public class PDFResultDefaultOffer
    {
        public string PathToFile { get; set; }
        public string Base64String { get; set; }

    }
    public class Successinfo {
        public int errorcode { get; set; }
        public string errormessage { get; set; }
    }
}