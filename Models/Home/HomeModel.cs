using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Management_System.Models.Home
{
    public class HomeModel
    {
        public HomeModel()
        {
            this.BlogPostsList = new List<BlogPost>();
            this.BlogCategoryList = new List<BlogCategory>();
            this.UsersList = new List<ApplicationUsersDetail>();
            this.BlogPostsModelList = new List<BlogPostsModel>();
        }

        public int BlogPostCount { get; set; }
        public int BlogCategoryCount { get; set; }
        public int UserCount { get; set; }
        public int BlogId { get; set; }
        public string BlogName { get; set; }
        public string BlogShortDescription { get; set; }
        public string BlogImage { get; set; }
        public string CreatedBy { get; set; }
        public string BlogCreatedDate { get; set; }
        public string BlogCreatedProfileImage { get; set; }
        public List<BlogPost> BlogPostsList { get; set; }
        public List<BlogCategory> BlogCategoryList { get; set; }
        public List<ApplicationUsersDetail> UsersList { get; set; }
        public List<BlogPostsModel> BlogPostsModelList { get; set; }
        //Get Login User Name 
        public static string GetLoginUserFullName()
        {
            StudentManagementSystemEntities db = new StudentManagementSystemEntities();

            var data = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId());
            string name = data != null && db.ApplicationUsersDetails.FirstOrDefault(m=>m.AspNetUserId == data.Id).AspNetUserId != null? db.ApplicationUsersDetails.FirstOrDefault(m => m.AspNetUserId == data.Id).FirstName + " " + db.ApplicationUsersDetails.FirstOrDefault(m => m.AspNetUserId == data.Id).LastName : "";
            return name;
        }
    }

    public class BlogPostsModel
    {
        public int BlogId { get; set; }
        public int BlogCategoryId { get; set; }
        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public bool BlogStatus { get; set; }
        public string BlogImage { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsBlogFavourite { get; set; }
        public bool IsBlogLike { get; set; }
        public int BlogFavouriteId { get; set; }
        public int FavouriteCount { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class ContactModel
    {
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}