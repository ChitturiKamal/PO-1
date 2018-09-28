using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PO.Models;
using PO.POSAPServiceInterface;
using PO.VendorSAPServiceInterface;
using AutoMapper;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Net.Http.Headers;
using log4net;
using System.Xml.Serialization;

namespace PO.BAL
{
    public class POHSNRate
    {

        TETechuvaDBContext context = new TETechuvaDBContext();
        RecordExceptions exception = new RecordExceptions();
        
        SuccessInfo sinfo = new SuccessInfo();

        public List<TEPOHSNTaxCodeMapping> GETPOHSNTaxRate()
        {
            // var HSNTaxCodeList = context.TEPOHSNTaxCodeMappings.Where(x => x.IsDeleted == false).ToList();
            var HSNTaxCodeList = (from HSNTx in context.TEPOHSNTaxCodeMappings
                                  where HSNTx.IsDeleted == false
                                  select new TEPOHSNTaxCodeMapping
                                  {
                                      ApplicableTo = HSNTx.ApplicableTo,
                                      DestinationCountry = HSNTx.DestinationCountry,
                                      VendorRegionDescription = HSNTx.VendorRegionDescription,
                                      DeliveryPlantRegionDescription = HSNTx.DeliveryPlantRegionDescription,
                                      GSTVendorClassification = HSNTx.GSTVendorClassification,
                                      HSNCode = HSNTx.HSNCode,
                                      MaterialGSTApplicability = HSNTx.MaterialGSTApplicability,
                                      VendorGSTApplicability = HSNTx.VendorGSTApplicability,
                                      ValidFrom = HSNTx.ValidFrom,
                                      ValidTo = HSNTx.ValidTo,
                                      TaxType = HSNTx.TaxType,
                                      TaxCode = HSNTx.TaxCode,
                                      TaxRate = HSNTx.TaxRate,
                                      UniqueID = HSNTx.UniqueID,
                                      IsDeleted = HSNTx.IsDeleted
                                  }).ToList();



            return HSNTaxCodeList;
        }

        public HttpResponseMessage SavePOHSNRate(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
          

            if (this.Check_Redund(HSNTaxCode))
            {
                int VendReg = Convert.ToInt32(HSNTaxCode.VendorRegionCode);
                int DeliReg = Convert.ToInt32(HSNTaxCode.DeliveryPlantRegionCode);

                HSNTaxCode.LastModifiedOn = DateTime.Now;
                HSNTaxCode.IsDeleted = false;

                TEGSTNStateMaster Venstate = context.TEGSTNStateMasters.Where(a => a.StateCode == HSNTaxCode.VendorRegionCode && a.IsDeleted == false).FirstOrDefault();
                if (Venstate != null)
                {
                    HSNTaxCode.VendorRegionCode = Venstate.StateCode;
                    HSNTaxCode.VendorRegionDescription = Venstate.StateName;
                }

                TEGSTNStateMaster Delstate = context.TEGSTNStateMasters.Where(a => a.StateCode == HSNTaxCode.DeliveryPlantRegionCode && a.IsDeleted == false).FirstOrDefault();
                if (Delstate != null)
                {
                    HSNTaxCode.DeliveryPlantRegionCode = Delstate.StateCode;
                    HSNTaxCode.DeliveryPlantRegionDescription = Delstate.StateName;
                }

                context.TEPOHSNTaxCodeMappings.Add(HSNTaxCode);
                context.SaveChanges();

                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Saved";
            }
            else
            {
                sinfo.errorcode = 1;
                sinfo.errormessage = "Combination already present in the Database";
            }
            return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };

        }


        public bool Check_Redund(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            var HSNTaxCount = context.TEPOHSNTaxCodeMappings.Where(m => m.HSNCode == HSNTaxCode.HSNCode
          && m.ApplicableTo == HSNTaxCode.ApplicableTo && m.DestinationCountry == HSNTaxCode.DestinationCountry
          && m.VendorRegionCode == HSNTaxCode.VendorRegionCode && m.DeliveryPlantRegionCode == HSNTaxCode.DeliveryPlantRegionCode &&
          m.MaterialGSTApplicability == HSNTaxCode.MaterialGSTApplicability &&
          m.VendorGSTApplicability == HSNTaxCode.VendorGSTApplicability && 
          m.TaxType == HSNTaxCode.TaxType && m.TaxCode == HSNTaxCode.TaxCode).ToList();

            if (HSNTaxCount.Count > 0)
                return false;
            else
                return true;
        }

        public HttpResponseMessage UpdatePOHSNRate(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            TEPOHSNTaxCodeMapping HSNObj = context.TEPOHSNTaxCodeMappings.Where(a => a.UniqueID == HSNTaxCode.UniqueID && a.IsDeleted == false).FirstOrDefault();
            if (HSNObj != null)
            {
                int VendReg = Convert.ToInt32(HSNTaxCode.VendorRegionCode);
                int DeliReg = Convert.ToInt32(HSNTaxCode.DeliveryPlantRegionCode);

                HSNObj.LastModifiedOn = DateTime.Now;
                HSNObj.IsDeleted = false;

                TEGSTNStateMaster Venstate = context.TEGSTNStateMasters.Where(a => a.StateCode == HSNTaxCode.VendorRegionCode && a.IsDeleted == false).FirstOrDefault();
                if (Venstate != null)
                {
                    HSNObj.VendorRegionCode = Venstate.StateCode;
                    HSNObj.VendorRegionDescription = Venstate.StateName;
                }

                TEGSTNStateMaster Delstate = context.TEGSTNStateMasters.Where(a => a.StateCode == HSNTaxCode.DeliveryPlantRegionCode && a.IsDeleted == false).FirstOrDefault();
                if (Delstate != null)
                {
                    HSNObj.DeliveryPlantRegionCode = Delstate.StateCode;
                    HSNObj.DeliveryPlantRegionDescription = Delstate.StateName;
                }

                HSNObj.ApplicableTo = HSNTaxCode.ApplicableTo;
                HSNObj.DestinationCountry = HSNTaxCode.DestinationCountry;
                //HSNObj.GSTVendorClassification = HSNTaxCode.GSTVendorClassification;
               // HSNObj.HSNCode = HSNTaxCode.HSNCode;
                HSNObj.MaterialGSTApplicability = HSNTaxCode.MaterialGSTApplicability;
                HSNObj.VendorGSTApplicability = HSNTaxCode.VendorGSTApplicability;
                HSNObj.ValidFrom = HSNTaxCode.ValidFrom;
                HSNObj.ValidTo = HSNTaxCode.ValidTo;
                HSNObj.TaxType = HSNTaxCode.TaxType;
                HSNObj.TaxCode = HSNTaxCode.TaxCode;
                HSNObj.TaxRate = HSNTaxCode.TaxRate;
                HSNObj.LastModifiedBy = HSNTaxCode.LastModifiedBy;

                context.Entry(HSNObj).CurrentValues.SetValues(HSNObj);
                context.SaveChanges();
                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Updated";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            else
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Unable to Update";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }

        public HttpResponseMessage DeletePOHSNRate(TEPOHSNTaxCodeMapping HSNTaxCode)
        {
            TEPOHSNTaxCodeMapping HSNObj = context.TEPOHSNTaxCodeMappings.Where(a => a.UniqueID == HSNTaxCode.UniqueID && a.IsDeleted == false).FirstOrDefault();
            if (HSNObj != null)
            {
                HSNObj.LastModifiedOn = DateTime.Now;
                HSNObj.IsDeleted = true;
                context.Entry(HSNObj).CurrentValues.SetValues(HSNObj);
                context.SaveChanges();

                sinfo.errorcode = 0;
                sinfo.errormessage = "Successfully Deleted";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
            else
            {
                sinfo.errorcode = 0;
                sinfo.errormessage = "Unable to Delete";
                return new HttpResponseMessage() { Content = new JsonContent(new { info = sinfo }) };
            }
        }


       

    }
    
}