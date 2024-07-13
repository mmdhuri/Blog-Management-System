using Microsoft.AspNet.Identity;
using Student_Management_System.Interface.Services.ApplicationUserDetails;
using Student_Management_System.Interface.Services.BlogPostCategoryServices;
using Student_Management_System.Interface.Services.BlogPostServices;
using Student_Management_System.Models;
using Student_Management_System.Models.BlogPosts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Management_System.Controllers
{
    public class BlogPostController : Controller
    {
        private IBlogPostService _blogPostService = null;
        private IBlogPostCategoryService _blogPostCategoryService = null;
        private IBlogPostFavoriteService _blogPostFavoriteService = null;
        private IApplicationUserDetailsService _applicationUserDetailsService = null;

        public BlogPostController()
        {
            this._blogPostService = new BlogPostService();
            this._blogPostCategoryService = new BlogPostCategoryService();
            this._blogPostFavoriteService = new BlogPostFavoriteService();
            this._applicationUserDetailsService = new ApplicationUserDetailsService();
        }
        public BlogPostController(IBlogPostService blogPostService,
            IBlogPostCategoryService blogPostCategoryService,
            IBlogPostFavoriteService blogPostFavoriteService,
            IApplicationUserDetailsService applicationUserDetailsService)
        {
            this._blogPostService = blogPostService;
            this._blogPostCategoryService = blogPostCategoryService;
            this._blogPostFavoriteService = blogPostFavoriteService;
            this._applicationUserDetailsService = applicationUserDetailsService;
        }
        // GET: BlogPost
        public ActionResult Index()
        {
            BlogPostsModel model = new BlogPostsModel();
            model.BlogPostsList = _blogPostService.GetAllBlogPosts();
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateEdit(int BlogId = 0)
        {
            var model = new BlogPostsModel();
            model.BlogCategorySelectList.Add(new SelectListItem { Text = "--Select Category--", Value = "" });
            var CategoryData = _blogPostCategoryService.GetAllBlogCategories();
            if(CategoryData.Count() > 0)
            {
                foreach(var items in CategoryData)
                    model.BlogCategorySelectList.Add(new SelectListItem { Text = items.BlogCategoryName, Value = items.BlogCategoryId.ToString() });
            }
            if (BlogId > 0)
            {
                var BlogData = _blogPostService.GetById(BlogId);
                model.BlogId = BlogData.BlogId;
                model.BlogName = BlogData.BlogName;
                model.BlogDescription = BlogData.BlogDescription;
                model.BlogCategoryId = (int)BlogData.BlogCategoryId;
                model.BlogStatus = BlogData.BlogStatus;
                model.BlogImage = BlogData.BlogImage;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateEdit(BlogPostsModel model)
        {
            var BlogData = model.BlogId > 0 ? _blogPostService.GetById(model.BlogId) : new Models.BlogPost();

            //BlogData.BlogId = model.BlogId;
            BlogData.BlogName = model.BlogName;
            BlogData.BlogDescription = model.BlogDescription;
            BlogData.BlogCategoryId = (int)model.BlogCategoryId;
            BlogData.BlogStatus = model.BlogStatus;

            var uploadsFolderPath = Server.MapPath("~/Uploads/BlogImage");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            string FullFileName = null;
            if (model.BlogFile != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(model.BlogFile.FileName);
                var fileExtension = Path.GetExtension(model.BlogFile.FileName);
                FullFileName = fileName + Guid.NewGuid() + fileExtension;
                var path = Path.Combine(uploadsFolderPath, FullFileName);
                model.BlogFile.SaveAs(path);
            }

            if (model.RemovedBlogImage == 1)
                BlogData.BlogImage = null;
            else if(model.HiddenBlogImage != null)
                BlogData.BlogImage = model.HiddenBlogImage;
            if(FullFileName !=null)
                BlogData.BlogImage = FullFileName;
            if (model.BlogId > 0)
            {
                BlogData.UpdatedBy = User.Identity.GetUserId();
                BlogData.UpdatedDate = DateTime.Now;
                _blogPostService.Update(BlogData);
            }
            else
            {
                BlogData.CreatedBy = User.Identity.GetUserId();
                BlogData.UpdatedBy = User.Identity.GetUserId();
                BlogData.CreatedDate = DateTime.Now;
                BlogData.UpdatedDate = DateTime.Now;
                _blogPostService.Insert(BlogData);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    var Data = _blogPostService.GetById(id);
                    Data.Deleted = true;
                    Data.UpdatedDate = DateTime.Now;
                    Data.UpdatedBy = User.Identity.GetUserId();
                    _blogPostService.Update(Data);
                    return Json(new { response = true });
                }
                return Json(new { response = false });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        #region UploadImage
        /// <summary>
        /// UploadImage
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UploadBlogEditorImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads/BlogContentImages"), filename);
                file.SaveAs(path);
                return Json(new { location = "/Uploads/BlogContentImages/" + filename });
            }
            return Json(false);
        }
        #endregion

        [HttpPost]
        public ActionResult AddBlogIntoFavourite(int BlogFavouriteId = 0,int BlogId = 0,bool IsFavourite = false,bool IsLike = false, string ButtonAction = "")
        {
            try
            {
                if (BlogId > 0)
                {
                    var LoggedUserId = User.Identity.GetUserId();
                    int BlogFavId = 0;
                    var Data = _blogPostFavoriteService.GetById(BlogFavouriteId);
                    if( Data != null)
                    {
                        if(ButtonAction == "Favourite")
                            Data.IsFavorite = IsFavourite;
                        if(ButtonAction == "Like")
                            Data.IsLike = IsLike;
                        Data.BlogId = BlogId;
                        Data.UserId = LoggedUserId;
                        _blogPostFavoriteService.Update(Data);
                        BlogFavId = Data.BlogFavouriteId;
                    }
                    else
                    {
                        var BlogFavourite = new BlogPost_Favorites();
                        if (ButtonAction == "Favourite")
                            BlogFavourite.IsFavorite = IsFavourite;
                        if (ButtonAction == "Like")
                            BlogFavourite.IsLike = IsLike;
                        BlogFavourite.BlogId = BlogId;
                        BlogFavourite.UserId = LoggedUserId;
                        _blogPostFavoriteService.Insert(BlogFavourite);
                        BlogFavId = BlogFavourite.BlogFavouriteId;
                    }

                    int favouriteCount = _blogPostFavoriteService.GetFavoriteCountByBlogId(BlogId);
                    int likeCount = _blogPostFavoriteService.GetLikeCountByBlogId(BlogId);
                    return Json(new { response = true , isfavourite = IsFavourite, islike= IsLike, favouriteCount = favouriteCount, likeCount = likeCount, blogfavouriteid = BlogFavId,blogid=BlogId });
                }
                return Json(new { response = false });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ViewBlogDetails(int BlogId = 0)
        {
            var Data = _blogPostService.GetById(BlogId);
            var model = new BlogPostsModel();
            var CreatedByUser = _applicationUserDetailsService.GetAppUserById(Data.CreatedBy);
            int favouriteCount = _blogPostFavoriteService.GetFavoriteCountByBlogId(BlogId);
            int likeCount = _blogPostFavoriteService.GetLikeCountByBlogId(BlogId);
            model.BlogId = BlogId;
            model.BlogName = Data.BlogName;
            model.BlogDescription = Data.BlogDescription;
            model.BlogImage = Data.BlogImage;
            model.BlogStatus = Data.BlogStatus;
            model.BlogCategory = Data.BlogCategory.BlogCategoryImage;
            model.CreatedBy = CreatedByUser.FirstName + " " + CreatedByUser.LastName;
            model.BlogFavouriteCount = favouriteCount;
            model.BlogLikesCount = likeCount;
            var favouriteData = _blogPostFavoriteService.GetFavoriteDataByUserandBlogId(User.Identity.GetUserId(), BlogId);
            model.FavouriteCount = _blogPostFavoriteService.GetFavoriteCountByBlogId(BlogId);
            model.LikeCount = _blogPostFavoriteService.GetLikeCountByBlogId(BlogId);
            model.IsBlogFavourite = favouriteData != null ? favouriteData.IsFavorite : false;
            model.IsBlogLike = favouriteData != null ? favouriteData.IsLike : false;
            model.BlogFavouriteId = favouriteData != null ? favouriteData.BlogFavouriteId : 0;
            return View(model);
        }
    }

}