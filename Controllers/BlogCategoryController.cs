using Microsoft.AspNet.Identity;
using Student_Management_System.Interface.Services.BlogPostCategoryServices;
using Student_Management_System.Models;
using Student_Management_System.Models.BlogCategorys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Management_System.Controllers
{
    public class BlogCategoryController : Controller
    {
        private IBlogPostCategoryService _blogPostCategoryService = null;

        public BlogCategoryController()
        {
            this._blogPostCategoryService = new BlogPostCategoryService();
        }
        public BlogCategoryController(IBlogPostCategoryService blogPostCategoryService)
        {
            this._blogPostCategoryService = blogPostCategoryService;
        }
        // GET: BlogCategory
        public ActionResult Index()
        {
            var model = new BlogCategoryModel();
            model.BlogCategoryList = _blogPostCategoryService.GetAllBlogCategories().Where(m => !m.Deleted).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateEdit(int BlogCategoryId = 0)
        {
            var model = new BlogCategoryModel();
            if(BlogCategoryId != 0)
            {
                var BlogData = _blogPostCategoryService.GetById(BlogCategoryId);
                model.BlogCategoryId = BlogCategoryId;
                model.BlogCategoryName = BlogData.BlogCategoryName;
                model.BlogCategoryDescription = BlogData.BlogCategoryDescription;
                model.BlogCategoryImage = BlogData.BlogCategoryImage;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateEdit(BlogCategoryModel model)
        {
            try
            {
                var LoggedUserId = User.Identity.GetUserId();
                var CModel = model.BlogCategoryId > 0 ? _blogPostCategoryService.GetById(model.BlogCategoryId) : new BlogCategory();
                CModel.BlogCategoryName = model.BlogCategoryName;
                CModel.BlogCategoryDescription = model.BlogCategoryDescription;

                var uploadsFolderPath = Server.MapPath("~/Uploads/BlogCategoryImage");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                string FullFileName = null;
                if (model.BlogCategoryFile != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.BlogCategoryFile.FileName);
                    var fileExtension = Path.GetExtension(model.BlogCategoryFile.FileName);
                    FullFileName = fileName + Guid.NewGuid() + fileExtension;
                    var path = Path.Combine(uploadsFolderPath, FullFileName);
                    model.BlogCategoryFile.SaveAs(path);
                }

                if (model.RemovedBlogImage == 1)
                    CModel.BlogCategoryImage = null;
                else if (model.HiddenBlogImage != null)
                    CModel.BlogCategoryImage = model.HiddenBlogImage;
                if (FullFileName != null)
                    CModel.BlogCategoryImage = FullFileName;

                if (model.BlogCategoryId > 0)
                {
                    CModel.UpdatedDate = DateTime.Now;
                    CModel.UpdatedBy = LoggedUserId;
                    _blogPostCategoryService.Update(CModel);
                }
                else
                {
                    CModel.CreatedBy = LoggedUserId;
                    CModel.UpdatedBy = LoggedUserId;
                    CModel.CreatedDate = DateTime.Now;
                    CModel.UpdatedDate = DateTime.Now;
                    _blogPostCategoryService.Insert(CModel);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if(id > 0)
                {
                    var Data = _blogPostCategoryService.GetById(id);
                    Data.Deleted = true;
                    Data.UpdatedDate = DateTime.Now;
                    Data.UpdatedBy = User.Identity.GetUserId();
                    _blogPostCategoryService.Update(Data);
                    return Json(new { response = true});
                }
                return Json(new { response = false });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
    }
}