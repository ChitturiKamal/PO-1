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
    
    public partial class TETaskFeedBack
    {
        public int ID { get; set; }
        public Nullable<int> TaskID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> QuestionID { get; set; }
        public string QuestionName { get; set; }
        public string Value { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string Remarks { get; set; }
        public string ResponsibleConsultant { get; set; }
    }
}
