//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PurchaseOrder.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TEEmailMessage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEEmailMessage()
        {
            this.R_TEEmailMessage_TEEdmTemplate = new HashSet<R_TEEmailMessage_TEEdmTemplate>();
            this.TEEmailLogs = new HashSet<TEEmailLog>();
        }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string ObjectId { get; set; }
        public int UniqueId { get; set; }
        public string Trasnport { get; set; }
        public string Type { get; set; }
        public string Sender { get; set; }
        public string SenderIP { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Recipients { get; set; }
        public string Attachments { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public string Size { get; set; }
        public Nullable<System.DateTime> ScheduleOn { get; set; }
        public Nullable<int> LastModifiedBy_Id { get; set; }
        public string OldUniqueID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_TEEmailMessage_TEEdmTemplate> R_TEEmailMessage_TEEdmTemplate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEEmailLog> TEEmailLogs { get; set; }
    }
}
