using Microsoft.AspNet.Identity;
using RestSharp;
using Student_Management_System.Interface.Services.BlogPostCategoryServices;
using Student_Management_System.Interface.Services.BlogPostServices;
using Student_Management_System.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using TweetSharp;
using System.IO;
using System.Net;
using Student_Management_System.Models;
using Student_Management_System.Interface.Services.Contacts;
using Vonage;
using Vonage.Request;
using Vonage.Messaging;
using Student_Management_System.Interface.Services.EmailServices;
using Student_Management_System.Interface.Services.ApplicationUserDetails;

namespace Student_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private IBlogPostService _blogPostService = null;
        private IBlogPostCategoryService _blogPostCategoryService = null;
        private IBlogPostFavoriteService _blogPostFavoriteService = null;
        private IContactService _contactService = null;
        private IApplicationUserDetailsService _applicationUserDetailsService = null;
        private SEmail _sendEmails;
        public HomeController()
        {
            this._blogPostService = new BlogPostService();
            this._blogPostCategoryService = new BlogPostCategoryService();
            this._blogPostFavoriteService = new BlogPostFavoriteService();
            this._contactService = new ContactService();
            this._applicationUserDetailsService = new ApplicationUserDetailsService();
            this._sendEmails = new SEmail();
        }
        public HomeController(IBlogPostService blogPostService,
            IBlogPostCategoryService blogPostCategoryService,
            IBlogPostFavoriteService blogPostFavoriteService,
            IContactService contactService,
            IApplicationUserDetailsService applicationUserDetailsService)
        {
            this._blogPostService = blogPostService;
            this._blogPostCategoryService = blogPostCategoryService;
            this._blogPostFavoriteService = blogPostFavoriteService;
            this._contactService = contactService;
            this._applicationUserDetailsService = applicationUserDetailsService;
        }
        public ActionResult Index()
        {
            var model = new HomeModel();
            var BlogPostData = _blogPostService.GetAllBlogPosts().OrderByDescending(m=>m.UpdatedDate);

            model.BlogCategoryList = _blogPostCategoryService.GetAllBlogCategories().OrderByDescending(m => m.UpdatedDate).ToList();
            if(BlogPostData.Count() > 0)
            {
                foreach(var items in BlogPostData)
                {
                    var B_Obj = new BlogPostsModel();
                    B_Obj.BlogId = items.BlogId;
                    B_Obj.BlogName = items.BlogName;
                    B_Obj.BlogImage = items.BlogImage;
                    B_Obj.BlogDescription = items.BlogDescription;
                    var favouriteData = _blogPostFavoriteService.GetFavoriteDataByUserandBlogId(User.Identity.GetUserId(),items.BlogId);
                    B_Obj.FavouriteCount = _blogPostFavoriteService.GetFavoriteCountByBlogId(items.BlogId);
                    B_Obj.LikeCount = _blogPostFavoriteService.GetLikeCountByBlogId(items.BlogId);
                    B_Obj.IsBlogFavourite = favouriteData != null ? favouriteData.IsFavorite : false;
                    B_Obj.IsBlogLike = favouriteData != null ? favouriteData.IsLike : false;
                    B_Obj.BlogFavouriteId = favouriteData != null ? favouriteData.BlogFavouriteId : 0;
                    model.BlogPostsModelList.Add(B_Obj);
                }
            }

            var LatestUpdatedBlogData = BlogPostData.FirstOrDefault();
            var UserData = _applicationUserDetailsService.GetAppUserById(LatestUpdatedBlogData.CreatedBy);
            model.BlogId = LatestUpdatedBlogData.BlogId;
            model.BlogName = LatestUpdatedBlogData.BlogName;
            model.BlogImage = LatestUpdatedBlogData.BlogImage;
            model.BlogShortDescription = LatestUpdatedBlogData.BlogShortDescription;
            model.CreatedBy = UserData != null ? UserData.FirstName + " " + UserData.LastName : "";
            //model.BlogCreatedProfileImage = UserData != null ? UserData.pr + " " + UserData.LastName : "";
            model.BlogCreatedDate = LatestUpdatedBlogData.CreatedDate.Value.ToString("MMMM dd, yyyy");
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var model = new ContactModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactModel model)
        {
            ViewBag.Message = "Your contact page.";

            var CModel = new Contact
            {
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                PhoneNumber = model.PhoneNumber,
                ContactAddress = model.ContactAddress,
                Message = model.Message,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _contactService.Insert(CModel);
            string Userbody = $"Thank you so much for connect with us";
            await _sendEmails.SendEmailAsync(model.ContactEmail, "Contact Message", Userbody);

            //Send Email to Admin
            string Adminbody = $"Contact message from ${model.ContactName}" +
                $"<p>Email Address: ${model.ContactEmail}</p>" +
                $"<p>Phone Number: ${model.PhoneNumber}</p>" +
                $"<p>Contact Address: ${model.ContactAddress}</p>" +
                $"<p>Message: ${model.Message}</p>" +
                $"Thank You...";
            await _sendEmails.SendEmailAsync(model.ContactEmail, "Contact Message", Adminbody);
            /*
            var credentials = Credentials.FromApiKeyAndSecret(
                "402c3e0a", // replace with your actual API key
                "qluuYAxgUiVLrj4k" // replace with your actual API secret
            );

            var vonageClient = new VonageClient(credentials);
            var response = await vonageClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
            {
                To = "919975424059", // replace with the actual recipient number
                From = model.PhoneNumber, // replace with your desired sender name/number
                Text = "A text message"
            });

            if (response.Messages[0].Status == "0")
            {
                ViewBag.Message = "Message sent successfully!";
            }
            else
            {
                ViewBag.Message = $"Error: {response.Messages[0].ErrorText}";
            }
            */
            return RedirectToAction("Contact");
        }

        public ActionResult AdminDashboard()
        {
            var model = new HomeModel();
            model.BlogPostsList = _blogPostService.GetAllBlogPosts();
            model.BlogCategoryList = _blogPostCategoryService.GetAllBlogCategories();
            model.BlogPostCount = model.BlogPostsList.Count();
            model.BlogCategoryCount = model.BlogCategoryList.Count();
            return View(model);
        }
        
        public ActionResult UserDashboard()
        {
            var model = new HomeModel();
            return View(model);
        }
        
        public ActionResult AddNewUser()
        {
            return View();
        }

        public ActionResult EditUser(int id = 0)
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> TwitterPost(string tweetContent, HttpPostedFileBase tweetImage)
        {

            if (string.IsNullOrEmpty(tweetContent))
            {
                ModelState.AddModelError("tweetContent", "Tweet content cannot be empty.");
                return View();
            }

            // Twitter API credentials
            var consumerKey = "xIBVuCeaUKz25ojpqiPFPRQEs";
            var consumerSecret = "kl6CIUIDmy2dBZkqEBNnNmfn4ny1YB4aUzpluZgZs4twlv5OXt";
            var accessToken = "1803003735032872960-8OurDBIWYVQ4vV6QjFgicZQ9TUO1dW";
            var accessTokenSecret = "PibQt10VqVoONCw1nmjMm2CnNOtw65EaU9vueUjZx6kvm";

            var baseUrl = "https://api.twitter.com/2/tweets";

            //var client = new RestClient(baseUrl);
            //Request Object
            var authenticator = OAuth1Authenticator.ForAccessToken(
                consumerKey,
                consumerSecret,
                accessToken,
                accessTokenSecret,
                OAuthSignatureMethod.HmacSha1
            );

            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = authenticator
            };
            var client = new RestClient(options);
            var request = new RestRequest(baseUrl, Method.Post);

            var body = new
            {
                text = tweetContent
            };
            request.AddJsonBody(body);
            var response = await client.PostAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ModelState.AddModelError("", "Unable to post tweet.");
                return View();
            }
            else
            {
                ViewBag.SuccessMessage = "Tweet posted successfully!";

            }
            return View();
        }

        

    }
}