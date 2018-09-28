using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{
    public class TEComplainceModel
    {
        //Customization
       
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Uniqueid { get; set; }
        public string OldUniqueid { get; set; }
        public string Objectid { get; set; }
        public Nullable<int> ISSUEID { get; set; }
        public string UNITONBOARDINGID { get; set; }
        public string Subject { get; set; }
        public string Descritpion { get; set; }
        public string Summary { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Categoryname { get; set; }
        public Nullable<int> ParentCatID { get; set; }
        public string ParentCategoryName { get; set; }
        public Nullable<int> QueueID { get; set; }
        public string Queuename { get; set; }
        public Nullable<System.DateTime> Closed_Date { get; set; }
        public string Priority { get; set; }
        public string PriorityName { get; set; }
        public string Status { get; set; }
        public string Stage { get; set; }
        public int AssignedTo { get; set; }
        public string ClosedBy { get; set; }
        public int RaisedBy { get; set; }
        public string RaisedByName { get; set; }
        public string Attachments { get; set; }
        public Nullable<System.DateTime> Estimated_Close_date { get; set; }
        public string Actual_close_date { get; set; }
        public string Assigned { get; set; }
        public string AssignToName { get; set; }
        public string Start { get; set; }
        public Nullable<System.DateTime> Pasue { get; set; }
        public string Resolved_Date { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string Projectcode { get; set; }
        public string ProjectName { get; set; }
        public string UnitID { get; set; }
        public string AREATYPE { get; set; }
        public Nullable<System.DateTime> Scheduled_Date { get; set; }
        public string RateUS { get; set; }
        public Nullable<int> COSTINVOLED { get; set; }
        public Nullable<int> EstimateCOST { get; set; }
        public Nullable<int> ActualCost { get; set; }
        public Nullable<int> LaborCost { get; set; }
        public Nullable<int> MaterialCost { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string AvailableEnd { get; set; }
        public string AvailableStart { get; set; }
        public string PrefferedTime { get; set; }
        public string Feedback { get; set; }
        public string FeedbackTechnician { get; set; }
        public string TechnicianRate { get; set; }
        public string Reopen { get; set; }
        public DateTime? ReopenDate { get; set; }
        public string Worklocation { get; set;}
        public string DismissedReason { get; set; }
        public string ReopenReason { get; set; }
        public string Disposition { get; set; }

        public int AssigneManagerId { get; set; }
        public string AssigneManagerName { get; set; }
        public int Age { get; set; }
        public int issuePriority { get; set; }
        public string UnitNumber { get; set; }

        public int primaryAssigne { get; set; }
    }
    public class TEUnitModel
    {
        //Customization
        //public string AssignToName { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public string CreatedBy { get; set; }
        //public Nullable<System.DateTime> LastModifiedOn { get; set; }
        //public string LastModifiedBy { get; set; }
        //public bool IsDeleted { get; set; }

        public Nullable<int> UNITONBOARDINGID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public string UnitID { get; set; }
        public Nullable<int> Userid { get; set; }
        public string CallName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string photo { get; set; }
        public string Status { get; set; }
    }

    public class TEContactModel
    {
        public Nullable<int> Uniqueid { get; set; }
        public Nullable<int> Projectid { get; set; }
        public string ProjectName { get; set; }
        public string TOWERNAME { get; set; }
        public string ColourCode { get; set; }
        public string Type { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CallName { get; set; }
        public string Unitid { get; set; }
        public string Mobile { get; set; }
        public string Emailid { get; set; }
        public string Photo { get; set; }
        public Nullable<int> UserId { get; set; }
    }
    public class TEqueueModel
    {
        public int Uniqueid { get; set; }
        public string QueueName { get; set; }
        public string CategoryName { get; set; }
        //public string Status { get; set; }
        public int QueueCount { get; set; }
        public int CategoryCount { get; set; }
        public int INQUE { get; set; }
        public int ASSIGNED { get; set; }
        public int ACCEPTED { get; set; }
        public int COMMENCED { get; set; }
        public int PAUSED { get; set; }
        public int COSTAPPROVAL { get; set; }
        public int RESUMED { get; set; }
        public int RESOLVED { get; set; }
        public int CLOSED { get; set; }
        public int REOPEN { get; set; }
    }

    public class TEUnit_Onboardingmodel
    {
        public string ProjectCity { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string Objectid { get; set; }
        public string OldUniqueid { get; set; }
        public string PMDAPPROVEDBY { get; set; }
        public DateTime? PMDAPPROVEDON { get; set; }
        public string PMDAPPROVEDREASON { get; set; }
        public int? Project { get; set; }
        public string Status { get; set; }
        public int Uniqueid { get; set; }
        public string Unit { get; set; }
        public int? UNITONBOARDINGID { get; set; }
        public int? UserID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CallName { get; set; }
        public string ProjectName { get; set; }
        public string COLOURCODE { get; set; }      
        
    }
      public class TEProjectUnitmodel
    {
        public int? projectuniqueid { get; set; }
        public string ProjectName { get; set; }
           public string ProjectCode { get; set; }
        public string COLOURCODE { get; set; }

          public string toweruniqueid { get; set; }
        public string towername { get; set; }
           public string towercode { get; set; }
          
           public int? Uniqueid { get; set; }
        public string Unit { get; set; }
      }
    public class TEQueueDepartmentModel
    {

        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string AUTOASSIGNMENT { get; set; }
        public string AUTOCOMMUNICATION { get; set; }
        public int? CATDEPID { get; set; }
        public string ParentCategory { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Default_assignee { get; set; }
        public string Default_assignee_Name { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public bool? IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string Objectid { get; set; }
        public string OldUniqueid { get; set; }
        public int? QueueID { get; set; }
        public string QueueName { get; set; }
        public string SLACritical { get; set; }
        public string SLAHigh { get; set; }
        public string SLALow { get; set; }
        public string SLAMedium { get; set; }
        public int? SubfunctionID { get; set; }
        public string SubfunctionName { get; set; }
        public int? SurveyID { get; set; }
        public int Uniqueid { get; set; }
        public int? TELineOfBussiness { get; set; }

        public TEEmpBasicInfo TEEmpDefaultAssignee { get; set; } 
    }

    public class TEEmployeeAssignee
    {
        public int? teempbasicinfo { get; set; }
        public string callname { get; set; }
        public int? Userid { get; set; }
        public string UserName { get; set; }
        public string EmpNameCode { get; set; }
    }
    public class TEEmployeeDetailsAssignee
    {
        public int? teempbasicinfo { get; set; }
        public string callname { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string WorkLocation { get; set; }
    }

    public class TEprojectModel
    {
        public string Address { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string City { get; set; }
        public string COLOURCODE { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EmailTemplateId { get; set; }
        public int? HeadUser { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsNewProject { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool MAINTENANCEFLAG { get; set; }
        public string Objectid { get; set; }
        public string OldUniqueid { get; set; }
        public int? PMDHEAD { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectShortName { get; set; }
        public string ProjectStatus { get; set; }
        public string Status { get; set; }
        public int? TECompany { get; set; }
        public int Uniqueid { get; set; }
    }

    public class TENotificationModel
    {
        public int? Project { get; set; }
        public string ProjectName { get; set; }
        public int? Tower { get; set; }
        public string unit { get; set; }

        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string description { get; set; }
        public bool IsDeleted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string Name { get; set; }
        public string Objectid { get; set; }
        public string OldUniqueid { get; set; }
        public bool? ReadStatus { get; set; }
        public int? ReceivedBy { get; set; }
        public int? SendBy { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Uniqueid { get; set; }
    }

    public class TERoleModel
    {
        public int userid { get; set; }
        public string Uniqueid { get; set; }
    }

    public class TEResidentprojectModel
    {
        public Nullable<int> CountTowers { get; set; }
        public Nullable<int> Countunit { get; set; }
        public Nullable<int> Countusers { get; set; }
        public string ProjectName { get; set; }
        public int Uniqueid { get; set; }
        public string ProjectCode { get; set; }
    }

    public class TEIssueCountModel
    {
        public int count { get; set; }
    }
}