//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PO.Models
{
    using System;
    
    public partial class uspTECollections_GetSAPDailyInvoice_Result
    {
        public int projectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string UnitNumber { get; set; }
        public string Customer { get; set; }
        public int InVoiceID { get; set; }
        public Nullable<System.DateTime> InVoiceDate { get; set; }
        public string InVoiceStatus { get; set; }
        public Nullable<System.DateTime> PreparedOn { get; set; }
        public Nullable<decimal> TotalInvoiceAmount { get; set; }
        public string SAPInVoiceID { get; set; }
        public Nullable<System.DateTime> InvoicePreparedOn { get; set; }
        public Nullable<System.DateTime> InvoiceDueDate { get; set; }
        public string CustomerEmailId { get; set; }
        public string SAPCustomerID { get; set; }
        public string DMSURL { get; set; }
    }
}