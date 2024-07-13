using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.Interface.Services.BlogPostServices
{
    public class BlogPostFavoriteService : GenericRepository<BlogPost_Favorites>, IBlogPostFavoriteService, IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<BlogPost_Favorites> table = null;

        public BlogPostFavoriteService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<BlogPost_Favorites>();
        }

        public BlogPostFavoriteService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<BlogPost_Favorites>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BlogPost_Favorites> GetAllBlogPostFavourites()
        {
            return _context.BlogPost_Favorites.ToList();
        }

        public BlogPost_Favorites GetFavoriteDataByUserandBlogId(string UserId = "", int BlogId = 0)
        {
            return _context.BlogPost_Favorites.Where(m => m.UserId == UserId && m.BlogId == BlogId).FirstOrDefault();
        }

        public int GetFavoriteCountByBlogId(int BlogId = 0)
        {
            return _context.BlogPost_Favorites.Where(m => m.BlogId == BlogId && m.IsFavorite).Count();
        }

        public int GetLikeCountByBlogId(int BlogId = 0)
        {
            return _context.BlogPost_Favorites.Where(m => m.BlogId == BlogId && m.IsLike).Count();
        }

    }
}