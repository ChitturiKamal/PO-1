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
    
    public partial class uspTEEvaluation_GetByEvaluationID_Result
    {
        public int EvaluationID { get; set; }
        public Nullable<int> LeadID { get; set; }
        public string EvaluationAnswer { get; set; }
        public string EvaluationNotes { get; set; }
        public string Weigtage { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> EQMID { get; set; }
        public Nullable<int> IsAnswered { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    }
}
