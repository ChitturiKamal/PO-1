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
    
    public partial class TEPayRollInputFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEPayRollInputFile()
        {
            this.TEPayRollAbscondingReports = new HashSet<TEPayRollAbscondingReport>();
            this.TEPayRollBankAcReports = new HashSet<TEPayRollBankAcReport>();
            this.TEPayRollFFReports = new HashSet<TEPayRollFFReport>();
            this.TEPayRollTransferReports = new HashSet<TEPayRollTransferReport>();
            this.TEPayRollNewJoineeReports = new HashSet<TEPayRollNewJoineeReport>();
            this.TEPayRollResignationReports = new HashSet<TEPayRollResignationReport>();
            this.TEPayRollLOPReports = new HashSet<TEPayRollLOPReport>();
            this.TEPayRollLeaveReports = new HashSet<TEPayRollLeaveReport>();
            this.TEPayRollLeaveEncashmentReports = new HashSet<TEPayRollLeaveEncashmentReport>();
        }
    
        public string Createdby { get; set; }
        public Nullable<System.DateTime> Createdon { get; set; }
        public int PayrollInputFileID { get; set; }
        public Nullable<int> ReportingPeriodID { get; set; }
        public string FinancialYear { get; set; }
        public string PayrollMonth { get; set; }
        public bool IsDownloaded { get; set; }
        public Nullable<System.DateTime> DownloadDate { get; set; }
        public string DMSDocID { get; set; }
        public string DMSPath { get; set; }
        public string DBAdminComments { get; set; }
        public string Status { get; set; }
        public bool Isdeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedon { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollAbscondingReport> TEPayRollAbscondingReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollBankAcReport> TEPayRollBankAcReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollFFReport> TEPayRollFFReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollTransferReport> TEPayRollTransferReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollNewJoineeReport> TEPayRollNewJoineeReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollResignationReport> TEPayRollResignationReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollLOPReport> TEPayRollLOPReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollLeaveReport> TEPayRollLeaveReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEPayRollLeaveEncashmentReport> TEPayRollLeaveEncashmentReports { get; set; }
        public virtual TEPayRollReportingPeriod TEPayRollReportingPeriod { get; set; }
    }
}
