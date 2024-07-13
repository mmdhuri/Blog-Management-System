using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Management_System.Models.BlogPosts
{
    public class BlogPostsModel
    {
        public BlogPostsModel()
        {
            this.BlogPostsList = new List<BlogPost>();
            this.BlogCategorySelectList = new List<SelectListItem>();
        }
        public int BlogId { get; set; }
        public int BlogCategoryId { get; set; }
        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public bool BlogStatus { get; set; }
        public string BlogImage { get; set; }
        public string BlogCategory { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public int RemovedBlogImage { get; set; }
        public int BlogFavouriteCount { get; set; }
        public int BlogLikesCount { get; set; }
        public string HiddenBlogImage { get; set; }
        public bool IsBlogFavourite { get; set; }
        public bool IsBlogLike { get; set; }
        public int BlogFavouriteId { get; set; }
        public int FavouriteCount { get; set; }
        public int LikeCount { get; set; }
        public HttpPostedFileBase BlogFile { get; set; }
        public List<BlogPost> BlogPostsList { get; set; }

        public List<SelectListItem> BlogCategorySelectList { get; set; }
    }
}