using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Student_Management_System.Models.BlogCategorys
{
    public class BlogCategoryModel
    {
        public BlogCategoryModel()
        {
            this.BlogCategoryList = new List<BlogCategory>();
        }
        public int BlogCategoryId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string BlogCategoryName { get; set; }
        public string BlogCategoryDescription { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Deleted { get; set; }
        public string BlogCategoryImage { get; set; }
        public int RemovedBlogImage { get; set; }
        public string HiddenBlogImage { get; set; }
        public HttpPostedFileBase BlogCategoryFile { get; set; }
        public List<BlogCategory> BlogCategoryList { get; set; }
    }
}