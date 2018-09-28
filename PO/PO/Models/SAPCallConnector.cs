using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Xml.Serialization;
//using EbuildService=PO.POSAPService;
using POService = PO.POSAPServiceInterface;
using VendorService = PO.VendorSAPServiceInterface;
using log4net;

namespace PO.Models
{
    public class SAPCallConnector
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public IList<EbuildService.PurchaseOrderResItem> CreatePO(EbuildService.PurchaseOrderReq POReq)
        //{
        //    //if (log.IsDebugEnabled)
        //    //    log.Debug("Entered into CreateCustomer SAP Call");            

        //    string Username = WebConfigurationManager.AppSettings["SAPConnectorUserName"].ToString();
        //    string Pasword = WebConfigurationManager.AppSettings["SAPConnectorPassword"].ToString();

        //    EbuildService.PurchaseOrder_OutClient CustomerService = new EbuildService.PurchaseOrder_OutClient();
        //   // CustomerService.Credentials = new System.Net.NetworkCredential(Username, Pasword);
        //    CustomerService.ClientCredentials.UserName.UserName = Username;
        //    CustomerService.ClientCredentials.UserName.Password = Pasword;

        //    //IList<EbuildService.PurchaseOrderReqItem> CustomerReqList = new List<EbuildService.PurchaseOrderReqItem>();

        //    IList<EbuildService.PurchaseOrderResItem> CustomerRespList = new List<EbuildService.PurchaseOrderResItem>();                       

        //    try
        //    {
        //        CustomerRespList = CustomerService.PurchaseOrder_Out(POReq);
        //    }
        //    catch (Exception ex)
        //    {
        //        //if (log.IsDebugEnabled)
        //        //    log.Debug("Exception From SAP Call:" + ex.Message);
        //    }

        //    return CustomerRespList;
        //}
        public IList<POService.PurchaseOrderResItem> CreatePO(POService.PurchaseOrderReq POReq)
        {
            if (log.IsDebugEnabled)
                log.Debug("Entered into PO SAP Call");
            string inputValues = "";
            string Username = WebConfigurationManager.AppSettings["SAPConnectorUserName"].ToString();
            string Pasword = WebConfigurationManager.AppSettings["SAPConnectorPassword"].ToString();

            POService.PurchaseOrder_OutService CustomerService = new POService.PurchaseOrder_OutService();
            CustomerService.Credentials = new System.Net.NetworkCredential(Username, Pasword);            

            //IList<EbuildService.PurchaseOrderReqItem> CustomerReqList = new List<EbuildService.PurchaseOrderReqItem>();

            IList<POService.PurchaseOrderResItem> CustomerRespList = new List<POService.PurchaseOrderResItem>();

            try
            {
                inputValues = InPutValuesSendingToSAP(POReq, false);
                if (log.IsDebugEnabled)
                    log.Debug("Input Values For PO SAP Call: " + inputValues);
                CustomerRespList = CustomerService.PurchaseOrder_Out(POReq);
            }
            catch (Exception ex)
            {
                new RecordException().RecordUnHandledException(ex);
                if (log.IsDebugEnabled)
                    log.Debug("Exception From PO SAP Call:" + ex.Message);
            }

            return CustomerRespList;
        }

        public IList<VendorService.VendorMasterResItem> CreateVendor(List<VendorService.VendorMasterReqItem> VendorReq)
        {
            if (log.IsDebugEnabled)
                log.Debug("Entered into Vendor SAP Call");
            string inputValues = "";
            string Username = WebConfigurationManager.AppSettings["SAPConnectorUserName"].ToString();
            string Pasword = WebConfigurationManager.AppSettings["SAPConnectorPassword"].ToString();

            VendorService.VendorMaster_OutService vendrService = new VendorService.VendorMaster_OutService();
            vendrService.Credentials = new System.Net.NetworkCredential(Username, Pasword);

            //IList<EbuildService.PurchaseOrderReqItem> CustomerReqList = new List<EbuildService.PurchaseOrderReqItem>();

            IList<VendorService.VendorMasterResItem> CustomerRespList = new List<VendorService.VendorMasterResItem>();

            try
            {
                inputValues = InPutValuesSendingToSAP(VendorReq, false);
                if (log.IsDebugEnabled)
                    log.Debug("Input Values For Vendor SAP Call: " + inputValues);
                CustomerRespList = vendrService.VendorMaster_Out(VendorReq.ToArray());
            }
            catch (Exception ex)
            {
                new RecordException().RecordUnHandledException(ex);
                if (log.IsDebugEnabled)
                    log.Debug("Exception From Vendor SAP Call:" + ex.Message);
            }

            return CustomerRespList;
        }
        public static string InPutValuesSendingToSAP(Object objToXml,
                      bool includeNameSpace)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer =
                      new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                if (!includeNameSpace)
                {

                    System.Xml.Serialization.XmlSerializerNamespaces xs =
                                         new XmlSerializerNamespaces();

                    //To remove namespace and any other inline 
                    //information tag                      
                    xs.Add("", "");
                    xmlSerializer.Serialize(stWriter, objToXml, xs);
                }
                else
                {
                    xmlSerializer.Serialize(stWriter, objToXml);
                }
                buffer = Encoding.ASCII.GetString(memStream.GetBuffer());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return buffer;
        }
    }
}