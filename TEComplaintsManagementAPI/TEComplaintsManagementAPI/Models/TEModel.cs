using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TECommonEntityLayer;

namespace TEComplaintsManagementAPI.Models
{    
        public class TELoginModel
    {
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Projectid { get; set; }
        public string ProjectName { get; set; }
        public string Type { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CallName { get; set; }
        public string Unitid { get; set; }
        public string Phone { get; set; }
        public string email { get; set; }
        public string Photo { get; set; }
        public string Password { get; set; }
    }

    public class TELoginuserModel
    {
        public string AndroidToken { get; set; }
        public string Area { get; set; }
        public string CallName { get; set; }
        public string city { get; set; }
        public string Country { get; set; }
        public string email { get; set; }
        public string facebookid { get; set; }
        public string googleid { get; set; }
        public string House_no { get; set; }
        public string IosToken { get; set; }
        public string path { get; set; }
        public string Phone { get; set; }
        public string photo { get; set; }
        public int? pincode { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }

    public class TECommentsModel
    {
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime CommentDate { get; set; }
        public string Commentedby { get; set; }
        public string Comments { get; set; }
        public int CommentsID { get; set; }
        public string CreatedBy { get; set; }       
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int IssueID { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string Status { get; set; }
        public int Uniqueid { get; set; }
        public bool ViewByCustomer { get; set; }
        public bool ViewByTechnician { get; set; }

        public string CreatedByName { get; set; }
    }
}