using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management_System.Interface.Services.BlogPostServices
{
    public interface IBlogPostFavoriteService : IGenericRepository<BlogPost_Favorites>
    {
        List<BlogPost_Favorites> GetAllBlogPostFavourites();
        BlogPost_Favorites GetFavoriteDataByUserandBlogId(string UserId = "", int BlogId = 0);

        int GetFavoriteCountByBlogId(int BlogId = 0);
        int GetLikeCountByBlogId(int BlogId = 0);
    }
}
