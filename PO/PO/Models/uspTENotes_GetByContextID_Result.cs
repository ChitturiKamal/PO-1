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
    
    public partial class uspTENotes_GetByContextID_Result
    {
        public int NotesID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> ContextID { get; set; }
        public string Notes { get; set; }
        public string NotesBy { get; set; }
        public Nullable<System.DateTime> NotesDateTime { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    }
}
