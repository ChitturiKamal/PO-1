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
    
    public partial class TEScheduleMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEScheduleMaster()
        {
            this.TEOrderPaymentSchedules = new HashSet<TEOrderPaymentSchedule>();
        }
    
        public int ScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public string ScheduleCode { get; set; }
        public string ScheduleType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LostModifiedBy { get; set; }
        public Nullable<System.DateTime> LostModifiedDate { get; set; }
        public string OldUniqueID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEOrderPaymentSchedule> TEOrderPaymentSchedules { get; set; }
    }
}
