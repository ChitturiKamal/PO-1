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
    using System.Collections.Generic;
    
    public partial class TEDmsUploadBatchUtilityDetail
    {
        public int ID { get; set; }
        public string OldSaleOrderId { get; set; }
        public string DocumentName { get; set; }
        public int DocuemntCode { get; set; }
        public string FileName { get; set; }
        public string LocationOfTheDocument { get; set; }
        public string UploadStatus { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> UploadedOn { get; set; }
        public Nullable<System.DateTime> ProcessedOn { get; set; }
        public int DMSReferenceId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
