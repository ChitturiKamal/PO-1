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
    
    public partial class TEAccountTransfer
    {
        public int UniqueID { get; set; }
        public Nullable<int> CONTEXTID { get; set; }
        public string FROMCUSTOMERID { get; set; }
        public string TOCUSTOMERID { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<System.DateTime> TRANSFERON { get; set; }
        public Nullable<int> TRANSFERBY { get; set; }
        public string SAPDOCUMENTID { get; set; }
        public string RELATIONSHIP { get; set; }
        public string STATUS { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<int> LASTMODIFIEDBY { get; set; }
        public Nullable<System.DateTime> LASTMODIFIEDON { get; set; }
        public Nullable<bool> ISDELETED { get; set; }
    }
}